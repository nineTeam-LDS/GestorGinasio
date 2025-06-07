// File: Model/Repositories/JsonRepository.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;                          // Para FirstOrDefault
using Newtonsoft.Json;
using GestorGinasio.Model.Exceptions;
using GestorGinasio.Model.Services;

namespace GestorGinasio.Model.Repositories
{
    // Repositório genérico que grava/ler coleções de T em "Data/{tipo}s.json".
    // Caso o ficheiro não exista, é criado um "[]". 
    // A leitura (e consequente validação do JSON) só acontece em GetAll(), Update(), Add(), Delete(), e não no construtor.
    public class JsonRepository<T> : IRepository<T>
    {
        private readonly IJsonService _jsonService;
        private readonly string _path;
        private List<T>? _itemsCache; // Mantém em memória após a 1ª carga bem sucedida

        // Construtor que deduz automaticamente o caminho do ficheiro:
        // Data/{typeof(T).Name.ToLowerInvariant()}s.json

        public JsonRepository(IJsonService jsonService)
        {
            _jsonService = jsonService ?? throw new ArgumentNullException(nameof(jsonService));

            // Determinar projectRoot/Data/{tipo}s.json
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var dataFolder = Path.Combine(projectRoot, "Data");
            Directory.CreateDirectory(dataFolder);

            var fileName = $"{typeof(T).Name.ToLowerInvariant()}s.json";
            _path = Path.Combine(dataFolder, fileName);

            // Se o ficheiro não existir, cria um [] vazio e grava
            if (!File.Exists(_path))
            {
                try
                {
                    _jsonService.Save(_path, new List<T>());
                }
                catch (Exception ex)
                {
                    // Se não conseguir criar a pasta/ficheiro, empacotamos em RepositoryException
                    throw new RepositoryException($"Falha de I/O ao criar ou inicializar '{_path}'.", ex);
                }
            }

            // Atenção: NÃO carregamos o JSON nem atribuímos _itemsCache aqui.
            // O Load só acontecerá quando se chamar GetAll(), etc.
        }

        // ------------- API pública --------------------------------------

        // ---- LISTAR ----------------------------------------------------------------
        public IEnumerable<T> GetAll()
        {
            try
            {
                // A cada chamada, tentamos ler do disco. Se for a primeira vez, não temos cache.
                var lista = _jsonService.Load<List<T>>(_path);
                _itemsCache = lista; // opcional: guardamos em cache para repetidas leituras
                return lista ?? Enumerable.Empty<T>();
            }
            catch (JsonException ex)
            {
                // Qualquer erro de desserialização JSON
                throw new JsonFileFormatException($"Ficheiro JSON de {typeof(T).Name} mal formatado.", ex);
            }
            catch (IOException ex)
            {
                // Qualquer falha de I/O ao ler o ficheiro
                throw new RepositoryException($"Falha de I/O ao ler o ficheiro '{_path}'.", ex);
            }
        }

        // Usa LINQ em vez de List.Find para aceitar Func<T,bool>
        public T? GetById(Func<T, bool> predicate)
        {
            // Se já tiver cache carregada, usamos; senão, invocamos GetAll()
            var lista = _itemsCache ?? GetAll().ToList();
            return lista.FirstOrDefault(predicate);
        }

        // ---- ADICIONAR ----------------------------------------------------------------
        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            // Vamos buscar todos (e validamos JSON aqui)
            var atual = GetAll().ToList();
            atual.Add(item);

            try
            {
                _jsonService.Save(_path, atual);
                _itemsCache = atual; // atualizamos cache
            }
            catch (JsonException ex)
            {
                throw new JsonFileFormatException($"Falha ao serializar lista de {typeof(T).Name}.", ex);
            }
            catch (IOException ex)
            {
                throw new RepositoryException($"Não foi possível gravar no ficheiro '{_path}'.", ex);
            }
        }

        // ---- ATUALIZAR ----------------------------------------------------------------
        public void Update(T updated, Func<T, int> keySelector)
        {
            if (updated == null)
                throw new ArgumentNullException(nameof(updated));

            var atual = GetAll().ToList();  // Lê e valida JSON
            var id = keySelector(updated);
            var idx = atual.FindIndex(x => keySelector(x) == id);

            if (idx < 0)
                throw new BusinessException($"{typeof(T).Name} com Id={id} não encontrado.");

            atual[idx] = updated;

            try
            {
                _jsonService.Save(_path, atual);
                _itemsCache = atual; // atualizamos cache
            }
            catch (JsonException ex)
            {
                throw new JsonFileFormatException($"Falha ao serializar lista de {typeof(T).Name}.", ex);
            }
            catch (IOException ex)
            {
                throw new RepositoryException($"Não foi possível gravar no ficheiro '{_path}'.", ex);
            }
        }

        // ---- REMOVER ----------------------------------------------------------------
        public void Delete(int id, Func<T, int> keySelector)
        {
            var atual = GetAll().ToList();  // Lê e valida JSON
            var idx = atual.FindIndex(x => keySelector(x) == id);

            if (idx < 0)
                throw new BusinessException($"{typeof(T).Name} com Id={id} não encontrado.");

            atual.RemoveAt(idx);

            try
            {
                _jsonService.Save(_path, atual);
                _itemsCache = atual; // atualizamos cache
            }
            catch (JsonException ex)
            {
                throw new JsonFileFormatException($"Falha ao serializar lista de {typeof(T).Name}.", ex);
            }
            catch (IOException ex)
            {
                throw new RepositoryException($"Não foi possível gravar no ficheiro '{_path}'.", ex);
            }
        }

        /*
        // Grava todos os itens atualmente em memória em JSON.
        // Usado internamente no construtor ao detectar JSON corrompido.
        private void SaveAll()                                                          // ---- GRAVAR ----
        {
            try
            {
                _jsonService.Save(_path, _items);
            }
            catch (Exception ex)
            {
                // Se falhar aqui, não queremos quebrar o construtor; apenas logar ou ignorar
                throw new RepositoryException($"Não foi possível repor o ficheiro '{_path}'.", ex);
            }
        }
        */
    }
}

