using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public class EquipamentoService : IEquipamentoService
    {
        private readonly IRepository<Equipamento> _repo;

        public EquipamentoService(IRepository<Equipamento> repo)
          => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public IEnumerable<Equipamento> GetAll() => _repo.GetAll();         // ---- LISTAR ----
        public void Add(Equipamento e) => _repo.Add(e);                     // ---- CRIAR ----
        public void Update(Equipamento e) => _repo.Update(e, x => x.Id);    // ---- EDITAR ----
        public void Delete(int id) => _repo.Delete(id, x => x.Id);          // ---- REMOVER ----
    }
}