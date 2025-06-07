// File: Model/Services/AulaService.cs
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;
using GestorGinasio.Model.Exceptions;

namespace GestorGinasio.Model.Services
{
    public class AulaService : IAulaService
    {
        private readonly IRepository<Aula> _repo;

        public AulaService(IRepository<Aula> repo)
          => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public IEnumerable<Aula> GetAll() => _repo.GetAll();        // ---- LISTAR ----

        public void Add(Aula a)                                     // ---- CRIAR ----
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));

            if (string.IsNullOrWhiteSpace(a.Nome))
                throw new BusinessException("O nome da aula não pode ficar em branco.");

            if (string.IsNullOrWhiteSpace(a.Instrutor))
                throw new BusinessException("O nome do instrutor não pode ficar em branco.");

            if (string.IsNullOrWhiteSpace(a.Sala))
                throw new BusinessException("A sala não pode ficar em branco.");

            if (string.IsNullOrWhiteSpace(a.Horario) || !a.Horario.Contains("-"))
                throw new BusinessException("Horário inválido. Use formato HH:mm-HH:mm.");

            // Exemplo: não permitir duas aulas com mesmo nome e horário
            if (_repo.GetAll().Any(x =>
                x.Nome.Equals(a.Nome, StringComparison.OrdinalIgnoreCase) &&
                x.Horario.Equals(a.Horario, StringComparison.OrdinalIgnoreCase)))
            {
                throw new BusinessException("Já existe uma aula com esse nome e horário.");
            }

            _repo.Add(a);
        }

        public void Update(Aula a)                                // ---- EDITAR ----
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));

            if (a.Id <= 0)
                throw new BusinessException("Id de aula inválido.");

            if (string.IsNullOrWhiteSpace(a.Nome))
                throw new BusinessException("O nome da aula não pode ficar em branco.");

            if (string.IsNullOrWhiteSpace(a.Instrutor))
                throw new BusinessException("O nome do instrutor não pode ficar em branco.");

            if (string.IsNullOrWhiteSpace(a.Sala))
                throw new BusinessException("A sala não pode ficar em branco.");

            if (string.IsNullOrWhiteSpace(a.Horario) || !a.Horario.Contains("-"))
                throw new BusinessException("Horário inválido. Use formato HH:mm-HH:mm.");

            // Se alterou nome/horário, verificar duplicados (excluindo a própria)
            var outros = _repo.GetAll().Where(x => x.Id != a.Id);
            if (outros.Any(x =>
                x.Nome.Equals(a.Nome, StringComparison.OrdinalIgnoreCase) &&
                x.Horario.Equals(a.Horario, StringComparison.OrdinalIgnoreCase)))
            {
                throw new BusinessException("Outra aula já existe com esse nome e horário.");
            }

            _repo.Update(a, x => x.Id);
        }

        public void Delete(int id)                                    // ---- REMOVER ----
        {
            if (id <= 0)
                throw new BusinessException("Id de aula inválido.");

            _repo.Delete(id, x => x.Id);
        }
    }
}

