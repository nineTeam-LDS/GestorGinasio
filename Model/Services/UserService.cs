using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public class UserService
    {
        private readonly JsonRepository<User> _repo = new("users.json");

        public List<User> GetAll() => _repo.GetAll();               // ---- LISTAR ----
        public void Add(User u) => _repo.Add(u);                    // ---- CRIAR ----
        public void Delete(int id) => _repo.Delete(id, x => x.Id);  // ---- REMOVER ----
        public void Update(User u) => _repo.Update(u, x => x.Id);   // ---- EDITAR ----
    }
}

