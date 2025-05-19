using System;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Services;

namespace GestorGinasio.View.Terminal
{
    public class GerirAulasView : IAulaView
    {
        public ConsoleKey MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("\n===== AULAS =====\n");
            Console.WriteLine("1. Listar");
            Console.WriteLine("2. Criar");
            Console.WriteLine("3. Editar");
            Console.WriteLine("4. Remover");
            Console.WriteLine("0. Voltar");
            Console.Write("Opção: ");
            return Console.ReadKey(true).Key;
        }
        
        /* LISTAR */
        public void MostrarLista(IEnumerable<Aula> lista)
        {
            Console.Clear();
            Console.WriteLine("\n===== AULAS =====\n");
            Console.WriteLine($"{"Id",-3} {"Nome",-15} {"Instrutor",-12} {"Sala",-6} Horário");
            foreach (var a in lista)
                Console.WriteLine($"{a.Id,-3} {a.Nome,-15} {a.Instrutor,-12} {a.Sala,-6} {a.Horario}");
            Console.WriteLine("\n<Enter> para voltar ."); 
            Console.ReadLine();
        }

        /* NOVO */
        public Aula PedirNovaAula()
        {
            Console.Clear();
            Console.WriteLine("\n===== NOVA AULA =====\n");
            Console.Write("Nome: "); var n = Console.ReadLine()!;
            Console.Write("Instrutor: "); var i = Console.ReadLine()!;
            Console.Write("Sala: "); var s = Console.ReadLine()!;
            Console.Write("Horário (HH:mm-HH:mm): "); var h = Console.ReadLine()!;
            return new Aula { Nome = n, Instrutor = i, Sala = s, Horario = h };
        }

        /* EDITAR / REMOVER */
        public int PedirIdParaEditar() => int.TryParse(Console.ReadLine(), out var id) ? id : -1;
        public int PedirIdParaRemover() => int.TryParse(Console.ReadLine(), out var id) ? id : -1;
        public Aula PedirDadosEditados(Aula o)
        {
            Console.Clear();
            Console.WriteLine("\n===== EDITAR AULA === (Enter = manter) ===\n");
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

        public bool Confirmar(string msg)
        {
            Console.Write($"{msg} (S/N) "); return Console.ReadKey().Key == ConsoleKey.S;
        }
        public void Sucesso(string msg) { Console.WriteLine($"\n{msg}\n<Enter>"); Console.ReadLine(); }
        public void Aviso(string mensagem)
        {
            Console.WriteLine($"*** {mensagem} ***");
            Console.WriteLine("<Enter> para continuar…");
            Console.ReadLine();
        }
    }
}

