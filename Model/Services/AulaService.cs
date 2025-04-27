using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public class AulaService
    {
        private readonly JsonRepository<Aula> _repo;

        public AulaService(string filePath = "aulas.json")
        {
            _repo = new JsonRepository<Aula>(filePath);
        }

        public List<Aula> GetAll() => _repo.GetAll();               // ---- LISTAR ----
        public void Add(Aula a) => _repo.Add(a);                    // ---- CRIAR ----
        public void Update(Aula a) => _repo.Update(a, x => x.Id);   // ---- EDITAR ----
        public void Delete(int id) => _repo.Delete(id, x => x.Id);  // ---- REMOVER ----
    }
}

