using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public interface IEquipamentoService
    {
        IEnumerable<Equipamento> GetAll();  // ---- LISTAR ----
        void Add(Equipamento e);            // ---- CRIAR ----
        void Update(Equipamento e);         // ---- EDITAR ----
        void Delete(int id);                // ---- REMOVER ----
    }
}
