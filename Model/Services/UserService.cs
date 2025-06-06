// File: Model/Services/UserService.cs
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;
using GestorGinasio.Model.Exceptions;

namespace GestorGinasio.Model.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repo;

        public UserService(IRepository<User> repo)
          => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public IEnumerable<User> GetAll() => _repo.GetAll();        // ---- LISTAR ----

        public void Add(User u)                                     // ---- CRIAR ----
        {
            if (u == null)
                throw new ArgumentNullException(nameof(u));

            // Regras de negócio:
            if (string.IsNullOrWhiteSpace(u.Username))
                throw new BusinessException("Username não pode ficar em branco.");

            if (u.Password == null || u.Password.Length< 4)
                throw new BusinessException("Password inválida (mínimo 4 caracteres).");

            if (string.IsNullOrWhiteSpace(u.Role))
                throw new BusinessException("Role não pode ficar em branco.");

            // Username deve ser único:
            if (_repo.GetAll().Any(x => x.Username.Equals(u.Username, StringComparison.OrdinalIgnoreCase)))
                throw new BusinessException("Já existe um utilizador com esse username.");

        // Se tudo OK, persiste:
        _repo.Add(u);
        }

        public void Delete(int id)                                  // ---- REMOVER ----
        {
            if (id <= 0)
                throw new BusinessException("Id de utilizador inválido.");

            _repo.Delete(id, x => x.Id);
        }

        public void Update(User u)                                  // ---- EDITAR ----
        {
            if (u == null)
                throw new ArgumentNullException(nameof(u));

            if (u.Id <= 0)
                throw new BusinessException("Id de utilizador inválido.");

            if (string.IsNullOrWhiteSpace(u.Username))
                throw new BusinessException("Username não pode ficar em branco.");

            if (u.Password == null || u.Password.Length < 4)
                throw new BusinessException("Password inválida (mínimo 4 caracteres).");

            if (string.IsNullOrWhiteSpace(u.Role))
                throw new BusinessException("Role não pode ficar em branco.");

            // Se alterou o username, verificar duplicado:
            var todos = _repo.GetAll().Where(x => x.Id != u.Id);
            if (todos.Any(x => x.Username.Equals(u.Username, StringComparison.OrdinalIgnoreCase)))
                throw new BusinessException("Outro utilizador já está a usar esse username.");

            _repo.Update(u, x => x.Id);
        }
    }
}

