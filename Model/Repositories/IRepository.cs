using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorGinasio.Model.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();                            // ---- LISTAR ----
        T? GetById(Func<T, bool> predicate);
        void Add(T item);                                   // ---- CRIAR ----
        void Update(T updated, Func<T, int> keySelector);   // ---- EDITAR ----
        void Delete(int id, Func<T, int> keySelector);      // ---- REMOVER ----
    }
}
