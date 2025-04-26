using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Services;

namespace GestorGinasio.View.Terminal
{
    public static class GerirSociosView
    {
        /* LISTA -------------------------------------------------------*/
        public static void MostrarLista(IEnumerable<Socio> lista)
        {
            Console.Clear();
            Console.WriteLine("=== SÓCIOS ===\n");
            Console.WriteLine("{0,-4} {1,-20} {2,-25} {3}",
                              "Id", "Nome", "Email", "Data Inscrição");

            foreach (var s in lista)
                Console.WriteLine($"{s.Id,-4} {s.Nome,-20} {s.Email,-25} {s.DataInscricao:yyyy-MM-dd}");

            Console.WriteLine("\n<Enter> para voltar .");
            Console.ReadLine();
        }

        /* NOVO --------------------------------------------------------*/
        public static Socio PedirNovoSocio()
        {
            Console.Clear();
            Console.WriteLine("=== NOVO SÓCIO ===");
            Console.Write("Nome:  "); var n = Console.ReadLine()!;
            Console.Write("Email: "); var e = Console.ReadLine()!;

            return new Socio { Nome = n, Email = e, DataInscricao = DateTime.Today };
        }

        /* EDITAR / REMOVER -------------------------------------------*/
        public static int PedirIdParaEditar()
        {
            Console.Write("\nId a editar: "); return int.TryParse(Console.ReadLine(), out var id) ? id : -1;
        }
        public static int PedirIdParaRemover() => PedirIdParaEditar();

        public static Socio PedirDadosEditados(Socio o)
        {
            Console.Clear();
            Console.WriteLine("=== EDITAR SÓCIO === (Enter = manter)");
            Console.Write($"Nome  [{o.Nome}]: "); var n = Console.ReadLine();
            Console.Write($"Email [{o.Email}]: "); var e = Console.ReadLine();

            return new Socio
            {
                Id = o.Id,
                Nome = string.IsNullOrWhiteSpace(n) ? o.Nome : n,
                Email = string.IsNullOrWhiteSpace(e) ? o.Email : e,
                DataInscricao = o.DataInscricao
            };
        }

        /* UTILITÁRIOS -------------------------------------------------*/
        public static void Sucesso(string msg)
        {
            Console.WriteLine($"\n{msg}\n<Enter>");
            Console.ReadLine();
        }
        public static bool Confirmar(string m)
        {
            Console.Write($"{m} (S/N) "); return Console.ReadKey(true).Key == ConsoleKey.S;
        }
        public static void IdInexistente()
        {
            Console.WriteLine("\nId inexistente. <Enter>");
            Console.ReadLine();
        }
    }
}
