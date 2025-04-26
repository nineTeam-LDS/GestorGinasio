using System;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Services;

namespace GestorGinasio.View.Terminal
{
    public static class GerirEquipamentosView
    {
        /*--------------------------------------------------------------*
         * LISTAGEM                                                     *
         *--------------------------------------------------------------*/
        public static void MostrarLista(IEnumerable<Equipamento> lista)
        {
            Console.Clear();
            Console.WriteLine("=== EQUIPAMENTOS ===\n");
            Console.WriteLine("{0,-4} {1,-20} {2,-12} {3,-12} {4}",
                              "Id", "Equipamento", "Quantidade", "Instrutor", "Horario");

            foreach (var e in lista)
                Console.WriteLine($"{e.Id,-4} {e.Nome,-20} {e.Quantidade,-12} {e.Instrutor,-12} {e.Horario}");

            Console.WriteLine("\n<Enter> para voltar .");
            Console.ReadLine();
        }

        /*--------------------------------------------------------------*
         * NOVO EQUIPAMENTO                                             *
         *--------------------------------------------------------------*/
        public static Equipamento PedirNovoEquipamento()
        {
            Console.Clear();
            Console.WriteLine("=== NOVO EQUIPAMENTO ===");

            Console.Write("Equipamento: "); var nome = Console.ReadLine()!; 
            Console.Write("Quantidade: "); int qtd = int.TryParse(Console.ReadLine(), out var q) ? q : 1;
            Console.Write("Instrutor: "); var inst = Console.ReadLine()!;
            Console.Write("Horario: "); var hora = Console.ReadLine()!;

            return new Equipamento
            {
                Nome = nome,
                Quantidade = qtd,
                Instrutor = inst,
                Horario = hora
            };
        }

        /*--------------------------------------------------------------*
         * EDITAR / REMOVER                                             *
         *--------------------------------------------------------------*/
        public static int PedirIdParaEditar()
        {
            Console.Write("\nId a editar: ");
            return int.TryParse(Console.ReadLine(), out var id) ? id : -1;
        }

        public static int PedirIdParaRemover()
        {
            Console.Write("\nId a remover: ");
            return int.TryParse(Console.ReadLine(), out var id) ? id : -1;
        }

        public static Equipamento PedirDadosEditados(Equipamento orig)
        {
            Console.Clear();
            Console.WriteLine("=== EDITAR EQUIPAMENTO ===");
            Console.WriteLine("(Enter = manter o valor actual)\n");

            Console.Write($"Equipamento [{orig.Nome}]: "); var n = Console.ReadLine();
            Console.Write($"Quantidade [{orig.Quantidade}]: "); var qStr = Console.ReadLine();
            Console.Write($"Instrutor [{orig.Instrutor}]: "); var i = Console.ReadLine();
            Console.Write($"Horario [{orig.Horario}]: "); var h = Console.ReadLine();


            int quantidade;
            bool quantidadeValida = int.TryParse(qStr, out quantidade);

            return new Equipamento
            {
                Id = orig.Id,
                Nome = string.IsNullOrWhiteSpace(n) ? orig.Nome : n,
                Quantidade = (quantidadeValida && quantidade > 0) ? quantidade : orig.Quantidade,
                Instrutor = string.IsNullOrWhiteSpace(i) ? orig.Instrutor : i,
                Horario = string.IsNullOrWhiteSpace(h) ? orig.Horario : h
            };
        }

        /*--------------------------------------------------------------*
         * UTILITÁRIOS                                                  *
         *--------------------------------------------------------------*/
        public static bool Confirmar(string msg)
        {
            Console.Write($"{msg} (S/N) ");
            return Console.ReadKey(true).Key == ConsoleKey.S;
        }

        public static void IdInexistente()
        {
            Console.WriteLine("Id inexistente. <Enter>");
            Console.ReadLine();
        }
    }
}
