using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.Model.Services
{
    public class EquipamentoService
    {
        private readonly string _path =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "equipamentos.json");

        public List<Equipamento> GetAll()
        {
            if (!File.Exists(_path))
                return new List<Equipamento>();
            var json = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<Equipamento>>(json)
                   ?? new List<Equipamento>();
        }
    }
}

