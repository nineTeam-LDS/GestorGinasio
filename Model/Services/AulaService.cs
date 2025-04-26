using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;

namespace GestorGinasio.Model.Services
{
    public class AulaService
    {
        private readonly JsonRepository<Aula> _rep = new("aulas.json");

        public List<Aula> GetAll() => _rep.GetAll();
        public void Add(Aula a) => _rep.Add(a);        // gera Id no repositório
        public void Update(Aula a) => _rep.Update(a, x => x.Id);
        public void Delete(int id) => _rep.Delete(id, x => x.Id);
    }
}

