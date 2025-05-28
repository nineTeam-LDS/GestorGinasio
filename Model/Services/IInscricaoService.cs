using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public interface IInscricaoService
    {
        IEnumerable<Inscricao> GetAll();    // ---- LISTAR ----
        void Add(Inscricao i);              // ---- CRIAR ----
        void Delete(int id);                // ---- REMOVER ----
        void Update(Inscricao updated);     // ---- EDITAR ----
    }
}
