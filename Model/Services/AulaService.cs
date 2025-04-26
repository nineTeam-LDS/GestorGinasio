using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.Model.Services
{
    public class AulaService
    {
        private readonly string _path =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "aulas.json");

        public List<Aula> GetAll()
        {
            if (!File.Exists(_path))
                return new List<Aula>();
            var json = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<Aula>>(json)
                   ?? new List<Aula>();
        }
    }
}
