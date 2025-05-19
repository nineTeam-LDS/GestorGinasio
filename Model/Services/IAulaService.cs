using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public interface IAulaService
    {
        IEnumerable<Aula> GetAll(); // ---- LISTAR ----
        void Add(Aula a);           // ---- CRIAR ----
        void Update(Aula a);        // ---- EDITAR ----
        void Delete(int id);        // ---- REMOVER ----
    }
}
