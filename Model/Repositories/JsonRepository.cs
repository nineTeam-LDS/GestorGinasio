using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;                          // Para FirstOrDefault
using Newtonsoft.Json;
using GestorGinasio.Model.Exceptions;
using GestorGinasio.Model.Services;

namespace GestorGinasio.Model.Repositories
{
    public class JsonRepository<T> : IRepository<T>
    {
        private readonly IJsonService _jsonService;
        private readonly string _path;      // caminho do ficheiro
        private readonly List<T> _items;    // cache em memória

        // ------------- Construtor ---------------------------------------
        public JsonRepository(IJsonService jsonService, string fileName)
        {
            if (jsonService == null)
                throw new ArgumentNullException(nameof(jsonService));
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName is required", nameof(fileName));

            _jsonService = jsonService;

            // Preserva a lógica de resolver o diretório Data
            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            _path = Path.Combine(projectRoot, "Data", fileName);

            // Carrega em memória
            _items = _jsonService.Load<List<T>>(_path) ?? new List<T>();
        }

        // Overload para DI genérico: deduz "<tipo>s.json"
        public JsonRepository(IJsonService jsonService)
            : this(
                jsonService,
                $"{typeof(T).Name.ToLowerInvariant()}s.json"
              )
        {
        }

        // ------------- API pública --------------------------------------
        public IEnumerable<T> GetAll() => _items;                                       // ---- LISTAR ----

        // Usa LINQ em vez de List.Find para aceitar Func<T,bool>
        public T? GetById(Func<T, bool> predicate) => _items.FirstOrDefault(predicate);

        public void Add(T item)                                                         // ---- ADICIONAR ----
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
            SaveAll();
        }

        public void Update(T updated, Func<T, int> keySelector)                         // ---- ATUALIZAR ----
        {
            if (updated == null) throw new ArgumentNullException(nameof(updated));
            int id = keySelector(updated);
            var index = _items.FindIndex(x => keySelector(x) == id);
            if (index >= 0)
            {
                _items[index] = updated;
                SaveAll();
            }
        }

        public void Delete(int id, Func<T, int> keySelector)                            // ---- REMOVER ----
        {
            _items.RemoveAll(x => keySelector(x) == id);
            SaveAll();
        }

        private void SaveAll()                                                          // ---- GRAVAR ----
        {
            _jsonService.Save(_path, _items);
        }
    }
}

