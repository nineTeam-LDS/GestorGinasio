using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public interface ISocioService
    {
        IEnumerable<Socio> GetAll();    // ---- LISTAR ----
        void Add(Socio s);              // ---- CRIAR ----
        void Update(Socio s);           // ---- EDITAR ----
        void Delete(int id);            // ---- REMOVER ----
    }
}
