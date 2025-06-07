// File: Model/Services/AuthService.cs
using System.Collections.Generic;
using System.Linq;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;
using GestorGinasio.Model.Exceptions;

namespace GestorGinasio.Model.Services
{
    // Valida credenciais e faz operações CRUD básicas em “User”.
    // Se houver qualquer problema ao aceder a ficheiro JSON (I/O ou JSON corrompido),
    // essas exceções serão capturadas no JsonRepository e repassadas aqui como JsonFileFormatException
    // ou RepositoryException, que o Controller tratará como erro fatal.
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _repo;

        public AuthService(IRepository<User> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            // Se não houver utilizadores, cria o admin
            try
            {
                if (!_repo.GetAll().Any())
                {
                    _repo.Add(new User { Username = "admin", Password = "admin", Role = "Admin" });
                }
            }
            catch (Exception ex)
            {
                // Propaga JsonFileFormatException ou RepositoryException
                throw;
            }
        }

        public bool ValidarCredenciais(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Faz a consulta no repositório. Pode lançar JsonFileFormatException ou RepositoryException.
            var u = _repo.GetAll().FirstOrDefault(x =>
                x.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase) &&
                x.Password == user.Password);

            if (u != null)
            {
                user.Role = string.IsNullOrWhiteSpace(u.Role) ? "Socio" : u.Role;
                return true;
            }
            return false;
        }

        public bool CriarNovoUser(User novo)                     // ---- CRIAR ----
        {
            if (novo == null)
                throw new ArgumentNullException(nameof(novo));

            // Regra de negócio: username único
            if (_repo.GetAll()
                     .Any(u => u.Username.Equals(novo.Username, StringComparison.OrdinalIgnoreCase)))
                return false;

            novo.Role = novo.Role.Trim().Equals("admin", StringComparison.OrdinalIgnoreCase)
                ? "Admin" : "Socio";

            _repo.Add(novo); // Pode lançar JSON/I/O exceptions
            return true;
        }

        public IEnumerable<User> ListarUser()                    // ---- LISTAR ----
        {
            // Pode lançar JsonFileFormatException ou RepositoryException
            return _repo.GetAll();
        }

        public bool RemoverUser(string username)                // ---- REMOVER ----
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new BusinessException("Username não pode ficar vazio.");

            var user = _repo.GetAll()
                            .FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return false;

            _repo.Delete(user.Id, x => x.Id); // Pode lançar BusinessException (Id não existe) ou JSON/I/O
            return true;
        }
    }
}
