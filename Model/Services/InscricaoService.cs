using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public class InscricaoService
    {
        private readonly JsonRepository<Inscricao> _repo;
        public InscricaoService(string filePath = "inscricoes.json")
        {
            _repo = new JsonRepository<Inscricao>(filePath);
        }

        public List<Inscricao> GetAll() => _repo.GetAll();
        public void Add(Inscricao i) => _repo.Add(i);
        public void Delete(int id) => _repo.Delete(id, x => x.Id);
        public void Update(Inscricao updated) => _repo.Update(updated, x => x.Id);
    }
}
