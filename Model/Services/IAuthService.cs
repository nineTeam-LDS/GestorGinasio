using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public interface IAuthService
    {
        bool ValidarCredenciais(User user);     // ---- VALIDAR ----
        bool CriarNovoUser(User novo);          // ---- CRIAR ----
        IEnumerable<User> ListarUser();         // ---- LISTAR ----
        bool RemoverUser(string username);      // ---- REMOVER ----
    }
}
