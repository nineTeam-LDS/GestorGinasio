// File: Model/Services/JsonService.cs
using System;
using System.IO;
using Newtonsoft.Json;
using GestorGinasio.Model.Exceptions;

namespace GestorGinasio.Model.Services
{
    // Lê e grava objetos em ficheiros JSON, 
    // encapsulando JsonException e IOException em exceções de domínio.
    public class JsonService : IJsonService
    {
        public T Load<T>(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json)!;
            }
            catch (JsonException ex)
            {
                // JSON inválido ou não corresponde ao T
                throw new JsonFileFormatException($"Ficheiro JSON '{path}' mal formatado.", ex);
            }
            catch (IOException ex)
            {
                // Problemas de I/O ao ler
                throw new RepositoryException($"Falha de I/O ao ler o ficheiro '{path}'.", ex);
            }
        }

        public void Save<T>(string path, T data)
        {
            try
            {
                // Garante que a pasta existe
                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrWhiteSpace(directory))
                    Directory.CreateDirectory(directory);

                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (JsonException ex)
            {
                // Falha ao serializar o T para JSON
                throw new JsonFileFormatException($"Falha ao serializar dados para o ficheiro '{path}'.", ex);
            }
            catch (IOException ex)
            {
                // Problemas de I/O ao gravar
                throw new RepositoryException($"Falha de I/O ao gravar o ficheiro '{path}'.", ex);
            }
        }
    }
}


/*
 * Principais pontos desta versão (06/06/2025):
 * 

1. Construtor genérico:

    Só recebe o IJsonService jsonService.

    Deduz projectRoot/Data/{tipo.ToLower()}s.json (por exemplo, Data/equipamentos.json se T == Equipamento).

    Tenta carregar o JSON; se o ficheiro existir mas estiver corrompido, inicializa _items = new List<T>() e imediatamente chama SaveAll() 
    para sobrescrever com lista vazia e recuperar um ficheiro válido.

2. GetAll():

    Lê sempre o disco (via _jsonService.Load<List<T>>), garantindo que se alterou algo “fora” ou noutra instância, a lista reflete o estado real.

    Envolve JsonException em JsonFileFormatException e IOException em RepositoryException.

3. Add/Update/Delete:

    Cada operação volta a chamar GetAll().ToList() para trabalhar sobre uma cópia do estado atual no disco, adiciona/atualiza/remove 
    e finalmente chama _jsonService.Save(_path, …).

    Se a gravação falhar, o método repassa a exceção adequada (JsonFileFormatException ou RepositoryException).

    Sincroniza a lista em memória _items logo que a operação no disco for bem‐sucedida.

4. SaveAll():

    Útil no construtor para “limpar” ficheiros corrompidos. Se falhar, repassa como RepositoryException 
    (porém não impede o programa de continuar no construtor).

-----------------------------------------------------------------------------------------------

Não é necessário passar mais nenhuma string de caminho a partir do Program.cs.
Sempre que se injetar, por exemplo, IRepository<Equipamento> ou IRepository<Socio>, o container chamará o construtor JsonRepository<T>(IJsonService) 
e esse construtor irá deduzir automaticamente:

    Path.Combine(projectRoot, "Data", $"{typeof(T).Name.ToLowerInvariant()}s.json")

(O projectRoot é calculado subindo três pastas a partir de AppContext.BaseDirectory, supondo que existe a pasta Data no nível da solução.)

-----------------------------------------------------------------------------------------------

Com as atuais (06/06/2025) versões de JsonService e JsonRepository<T>, o Program.cs mantém-se exatamente como está, sem precisar de registos de caminho para cada entidade. 
Basta garantir que existe a pasta Data e que existem os ficheiros JSON corretos (ou, na primeira execução, o sistema criará um [] vazio). 
Isso deixa todo o esquema de persistência “por convenção” e preparada para crescer sem precisar de tocar no Program.cs cada vez que se adiciona uma nova entidade.

*/