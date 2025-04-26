using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.Model.Services
{
    public class SocioService
    {
        private readonly string _path =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "socios.json");

        public List<Socio> GetAll()
        {
            if (!File.Exists(_path))
                return new List<Socio>();
            var json = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<Socio>>(json)
                   ?? new List<Socio>();
        }
    }
}
