using System.Collections.Generic;
using System.Linq;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public class AuthService
    {
        private readonly JsonRepository<User> _repo;

        public AuthService(string filePath = "users.json")
        {
            _repo = new JsonRepository<User>(filePath);

            // Se não houver utilizadores, cria o admin
            if (!_repo.GetAll().Any())
            {
                _repo.Add(new User { Username = "admin", Password = "admin", Role = "Admin" });
            }
        }

        public bool ValidarCredenciais(User user)
        {
            var u = _repo.GetAll().FirstOrDefault(x =>
                x.Username.Equals(user.Username, System.StringComparison.OrdinalIgnoreCase) &&
                x.Password == user.Password);

            if (u != null)
            {
                user.Role = string.IsNullOrWhiteSpace(u.Role) ? "Socio" : u.Role;
                return true;
            }
            return false;
        }

        public bool CriarNovoUsuario(User novo)
        {
            if (_repo.GetAll().Any(u => u.Username.Equals(novo.Username, System.StringComparison.OrdinalIgnoreCase)))
                return false;

            novo.Role = novo.Role.Trim().Equals("admin", System.StringComparison.OrdinalIgnoreCase) ? "Admin" : "Socio";
            _repo.Add(novo);
            return true;
        }

        public List<User> ListarUtilizadores() => _repo.GetAll();

        public bool RemoverUsuario(string username)
        {
            var user = _repo.GetAll().FirstOrDefault(u => u.Username.Equals(username, System.StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return false;

            _repo.Delete(user.Id, x => x.Id);
            return true;
        }
    }
}
