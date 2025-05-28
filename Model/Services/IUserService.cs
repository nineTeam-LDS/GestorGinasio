using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();    // ---- LISTAR ----
        void Add(User u);       // ---- CRIAR ----
        void Delete(int id);    // ---- REMOVER ----
        void Update(User u);    // ---- EDITAR ----
    }
}
