using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public class InscricaoService : IInscricaoService
    {
        private readonly IRepository<Inscricao> _repo;
        public InscricaoService(IRepository<Inscricao> repo)
          => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public IEnumerable<Inscricao> GetAll() => _repo.GetAll();                   // ---- LISTAR ----
        public void Add(Inscricao i) => _repo.Add(i);                               // ---- CRIAR ----
        public void Delete(int id) => _repo.Delete(id, x => x.Id);                  // ---- REMOVER ----
        public void Update(Inscricao updated) => _repo.Update(updated, x => x.Id);  // ---- EDITAR ----
    }
}
