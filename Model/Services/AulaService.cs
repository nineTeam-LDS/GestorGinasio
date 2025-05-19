using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public class AulaService : IAulaService
    {
        private readonly IRepository<Aula> _repo;

        public AulaService(IRepository<Aula> repo)
          => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public IEnumerable<Aula> GetAll() => _repo.GetAll();        // ---- LISTAR ----
        public void Add(Aula a) => _repo.Add(a);                    // ---- CRIAR ----
        public void Update(Aula a) => _repo.Update(a, x => x.Id);   // ---- EDITAR ----
        public void Delete(int id) => _repo.Delete(id, x => x.Id);  // ---- REMOVER ----
    }
}

