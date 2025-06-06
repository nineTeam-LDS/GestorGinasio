// File: Model/Services/SocioService.cs
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;
using GestorGinasio.Model.Exceptions;

namespace GestorGinasio.Model.Services
{
    public class SocioService : ISocioService
    {
        private readonly IRepository<Socio> _repo;

        public SocioService(IRepository<Socio> repo)
          => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public IEnumerable<Socio> GetAll() => _repo.GetAll();       // ---- LISTAR ----
        public void Add(Socio s)                                    // ---- CRIAR ----
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            if (string.IsNullOrWhiteSpace(s.Nome))
                throw new BusinessException("Nome do sócio não pode ficar vazio.");

            if (!s.Email.Contains("@"))
                throw new BusinessException("Email inválido.");

            // Eventual regra de negócio: email único → 
            var todos = _repo.GetAll();
            if (todos.Any(x => x.Email.Equals(s.Email, StringComparison.OrdinalIgnoreCase)))
                throw new BusinessException("Já existe um sócio com esse email.");

            _repo.Add(s);
        }
        public void Update(Socio s)                                 // ---- EDITAR ----
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            if (string.IsNullOrWhiteSpace(s.Nome))
                throw new BusinessException("Nome do sócio não pode ficar vazio.");

            // Regra de negócio: se alterar email, verificar duplicados
            var todos = _repo.GetAll().Where(x => x.Id != s.Id);
            if (todos.Any(x => x.Email.Equals(s.Email, StringComparison.OrdinalIgnoreCase)))
                throw new BusinessException("Outro sócio já usa esse email.");

            _repo.Update(s, x => x.Id);
        }
        public void Delete(int id)                                  // ---- REMOVER ----
        {
            if (id <= 0)
                throw new BusinessException("Id inválido.");

            // Regra de negócio: não deixar eliminar se tiver histórico (ex: aulas marcadas)
            // (caso não exista histórico, basta chamar Delete)
            _repo.Delete(id, x => x.Id);
        }
}
}
