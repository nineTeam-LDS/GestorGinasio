using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorGinasio.Model.Entities
{
    public class Inscricao
    {
        public int Id { get; set; }
        public int SocioId { get; set; }
        public int AulaId { get; set; }
        public DateTime Data { get; set; }
    }
}
