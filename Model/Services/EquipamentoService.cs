using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public class EquipamentoService
    {
        private readonly JsonRepository<Equipamento> _repo =
            new JsonRepository<Equipamento>("equipamentos.json");

        public List<Equipamento> GetAll() => _repo.GetAll();
        public void Add(Equipamento e) => _repo.Add(e);
        public void Update(Equipamento e) => _repo.Update(e, x => x.Id);
        public void Delete(int id) => _repo.Delete(id, x => x.Id);
    }
}