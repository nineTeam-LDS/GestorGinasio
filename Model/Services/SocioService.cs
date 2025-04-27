using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public class SocioService
    {
        private readonly JsonRepository<Socio> _repo;

        public SocioService(string filePath = "socios.json")
        {
            _repo = new JsonRepository<Socio>(filePath);
        }

        public List<Socio> GetAll() => _repo.GetAll();              // ---- LISTAR ----
        public void Add(Socio s) => _repo.Add(s);                   // ---- CRIAR ----
        public void Update(Socio s) => _repo.Update(s, x => x.Id);  // ---- EDITAR ----
        public void Delete(int id) => _repo.Delete(id, x => x.Id);  // ---- REMOVER ----
    }
}
