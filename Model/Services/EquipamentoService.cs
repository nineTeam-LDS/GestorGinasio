// File: Model/Services/EquipamentoService.cs
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;
using GestorGinasio.Model.Exceptions;

namespace GestorGinasio.Model.Services
{
    public class EquipamentoService : IEquipamentoService
    {
        private readonly IRepository<Equipamento> _repo;

        public EquipamentoService(IRepository<Equipamento> repo)
          => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        public IEnumerable<Equipamento> GetAll() => _repo.GetAll();         // ---- LISTAR ----

        public void Add(Equipamento e)                                      // ---- CRIAR ----
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            // Regras de negócio:
            if (string.IsNullOrWhiteSpace(e.Nome))
                throw new BusinessException("O nome do equipamento não pode ficar em branco.");

            if (e.Quantidade <= 0)
                throw new BusinessException("A quantidade deve ser um número maior que zero.");

            if (string.IsNullOrWhiteSpace(e.Instrutor))
                throw new BusinessException("O nome do instrutor não pode ficar em branco.");

            if (string.IsNullOrWhiteSpace(e.Horario))
                throw new BusinessException("O horário não pode ficar em branco.");

            // Exemplo: equipamento com mesmo nome → não permitir duplicado
            if (_repo.GetAll()
                     .Any(x => x.Nome.Equals(e.Nome, StringComparison.OrdinalIgnoreCase)))
            {
                throw new BusinessException("Já existe um equipamento com esse nome.");
            }

            _repo.Add(e);
        }

        public void Update(Equipamento e)                                   // ---- EDITAR ----
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            if (e.Id <= 0)
                throw new BusinessException("Id de equipamento inválido.");

            if (string.IsNullOrWhiteSpace(e.Nome))
                throw new BusinessException("O nome do equipamento não pode ficar em branco.");

            if (e.Quantidade <= 0)
                throw new BusinessException("A quantidade deve ser um número maior que zero.");

            if (string.IsNullOrWhiteSpace(e.Instrutor))
                throw new BusinessException("O nome do instrutor não pode ficar em branco.");

            if (string.IsNullOrWhiteSpace(e.Horario))
                throw new BusinessException("O horário não pode ficar em branco.");

            // Se alterou Nome, garantir que não existe duplicado (excluindo ele mesmo)
            var todos = _repo.GetAll().Where(x => x.Id != e.Id);
            if (todos.Any(x => x.Nome.Equals(e.Nome, StringComparison.OrdinalIgnoreCase)))
                throw new BusinessException("Outro equipamento já está a usar esse nome.");

            _repo.Update(e, x => x.Id);
        }

        public void Delete(int id)                                        // ---- REMOVER ----
        {
            if (id <= 0)
                throw new BusinessException("Id de equipamento inválido.");

            _repo.Delete(id, x => x.Id);
        }
    }
}