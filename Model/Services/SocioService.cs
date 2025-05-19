using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public class SocioService : ISocioService
    {
        private readonly IRepository<Socio> _repo;

        public SocioService(IRepository<Socio> repo)
          => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public IEnumerable<Socio> GetAll() => _repo.GetAll();              // ---- LISTAR ----
        public void Add(Socio s) => _repo.Add(s);                   // ---- CRIAR ----
        public void Update(Socio s) => _repo.Update(s, x => x.Id);  // ---- EDITAR ----
        public void Delete(int id) => _repo.Delete(id, x => x.Id);  // ---- REMOVER ----
    }
}
