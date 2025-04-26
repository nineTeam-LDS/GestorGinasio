using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GestorGinasio.Model.Repositories
{
    public class JsonRepository<T>
    {
        private readonly string _path;      // caminho do ficheiro
        private readonly List<T> _items;    // cache em memória

        // ------------- Construtor ---------------------------------------
        public JsonRepository(string fileName)
        {
            var projectRoot = Path.GetFullPath(Path.Combine(
                                 AppContext.BaseDirectory, "..", "..", "..")); // sobe 3 níveis
            _path = Path.Combine(projectRoot, "Data", fileName);
            _items = LoadAll();
        }

        // ------------- API pública --------------------------------------
        public List<T> GetAll() => _items;

        public void Add(T item)
        {
            _items.Add(item);
            SaveAll();
        }

        public void Update(T updated, Func<T, int> keySelector)
        {
            var idx = _items.FindIndex(x => keySelector(x) == keySelector(updated));
            if (idx >= 0) _items[idx] = updated;
            SaveAll();
        }

        public void Delete(int id, Func<T, int> keySelector)
        {
            _items.RemoveAll(x => keySelector(x) == id);
            SaveAll();
        }

        // ------------- Internos -----------------------------------------
        private List<T> LoadAll()
        {
            if (!File.Exists(_path)) return new();

            var txt = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<T>>(txt) ?? new();
        }

        private void SaveAll()
        {
            var json = JsonConvert.SerializeObject(_items, Formatting.Indented);
            File.WriteAllText(_path, json);
        }
    }
}

