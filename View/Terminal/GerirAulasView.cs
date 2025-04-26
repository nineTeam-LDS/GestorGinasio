using System;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Services;

namespace GestorGinasio.View.Terminal
{
    public static class GerirAulasView
    {
        /* LISTAR */
        public static void MostrarLista(IEnumerable<Aula> lista)
        {
            Console.Clear();
            Console.WriteLine("=== AULAS ===");
            Console.WriteLine($"{"Id",-3} {"Nome",-15} {"Instrutor",-12} {"Sala",-6} Horário");
            foreach (var a in lista)
                Console.WriteLine($"{a.Id,-3} {a.Nome,-15} {a.Instrutor,-12} {a.Sala,-6} {a.Horario}");
            Console.WriteLine("\n<Enter> para voltar ."); Console.ReadLine();
        }

        /* NOVO */
        public static Aula PedirNovaAula()
        {
            Console.Clear();
            Console.WriteLine("=== NOVA AULA ===");
            Console.Write("Nome: "); var n = Console.ReadLine()!;
            Console.Write("Instrutor: "); var i = Console.ReadLine()!;
            Console.Write("Sala: "); var s = Console.ReadLine()!;
            Console.Write("Horário (HH:mm-HH:mm): "); var h = Console.ReadLine()!;
            return new Aula { Nome = n, Instrutor = i, Sala = s, Horario = h };
        }

        /* EDITAR / REMOVER */
        public static int PedirIdParaEditar() { Console.Write("\nId a editar: "); return int.TryParse(Console.ReadLine(), out var id) ? id : -1; }
        public static int PedirIdParaRemover() { Console.Write("\nId a remover: "); return int.TryParse(Console.ReadLine(), out var id) ? id : -1; }
        public static Aula PedirDadosEditados(Aula o)
        {
            Console.Clear();
            Console.WriteLine("=== EDITAR AULA === (Enter = manter)");
            Console.Write($"Nome [{o.Nome}]: "); var n = Console.ReadLine()!;
            Console.Write($"Instrutor [{o.Instrutor}]: "); var i = Console.ReadLine()!;
            Console.Write($"Sala [{o.Sala}]: "); var s = Console.ReadLine()!;
            Console.Write($"Horário [{o.Horario}]: "); var h = Console.ReadLine()!;

            o.Nome = string.IsNullOrWhiteSpace(n) ? o.Nome : n;
            o.Instrutor = string.IsNullOrWhiteSpace(i) ? o.Instrutor : i;
            o.Sala = string.IsNullOrWhiteSpace(s) ? o.Sala : s;
            o.Horario = string.IsNullOrWhiteSpace(h) ? o.Horario : h;
            return o;
        }

        public static bool Confirmar(string msg)
        {
            Console.Write($"{msg} (S/N) "); return Console.ReadKey().Key == ConsoleKey.S;
        }
        public static void Sucesso(string msg) { Console.WriteLine($"\n{msg}\n<Enter>"); Console.ReadLine(); }
        public static void IdInexistente() { Console.WriteLine("\nId inexistente!\n<Enter>"); Console.ReadLine(); }
    }
}

