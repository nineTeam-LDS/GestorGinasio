using System;
using System.Collections.Generic;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Implementação de IEquipamentoView usando console.
    public class GerirEquipamentosView : IEquipamentoView
    {
         public ConsoleKey MostrarMenu()
         {
             Console.Clear();
             Console.WriteLine("\n===== EQUIPAMENTOS =====\n");
             Console.WriteLine("1. Listar");
             Console.WriteLine("2. Adicionar");
             Console.WriteLine("3. Editar");
             Console.WriteLine("4. Remover");
             Console.WriteLine("0. Voltar");
             Console.Write("Opção: ");
             return Console.ReadKey(true).Key;
         }

        /*--------------------------------------------------------------*
        * LISTAGEM                                                     *
        *--------------------------------------------------------------*/
        public void MostrarLista(IEnumerable<Equipamento> lista)
        {
            Console.Clear();
            Console.WriteLine("\n===== EQUIPAMENTOS =====\n");
            Console.WriteLine("{0,-4} {1,-20} {2,-12} {3,-12} {4}", "Id", "Equipamento", "Quantidade", "Instrutor", "Horario");
            foreach (var e in lista)
                Console.WriteLine($"{e.Id,-4} {e.Nome,-20} {e.Quantidade,-12} {e.Instrutor,-12} {e.Horario}");
            Console.WriteLine("\n<Enter> para continuar…");
            Console.ReadLine();
        }

        /*--------------------------------------------------------------*
         * NOVO EQUIPAMENTO                                             *
         *--------------------------------------------------------------*/
        public Equipamento PedirNovoEquipamento()
        {
            Console.Clear();
            Console.WriteLine("\n===== NOVO EQUIPAMENTO =====\n");

            Console.Write("Equipamento: "); var nome = Console.ReadLine()!;
            Console.Write("Quantidade: "); var qOk = int.TryParse(Console.ReadLine(), out var qtd);
            Console.Write("Instrutor:  "); var instr = Console.ReadLine()!;
            Console.Write("Horário:    "); var hora = Console.ReadLine()!;
            return new Equipamento
            {
                Nome = nome,
                Quantidade = qOk && qtd > 0 ? qtd : 1,
                Instrutor = instr,
                Horario = hora
            };
        }

        /*--------------------------------------------------------------*
         * EDITAR / REMOVER                                             *
         *--------------------------------------------------------------*/
        public int PedirIdParaEditar()
        {
            Console.Write("\nId para editar: ");
            return int.TryParse(Console.ReadLine(), out var id) ? id : -1;
        }

        public int PedirIdParaRemover()
        {
            Console.Write("\nId para remover: ");
            return int.TryParse(Console.ReadLine(), out var id) ? id : -1;
        }

        public Equipamento PedirDadosEditados(Equipamento existente)
        {
            Console.Clear();
            Console.WriteLine("\\===== EDITAR EQUIPAMENTO =====");
            Console.WriteLine("(Enter = manter o valor actual)\n");
            Console.Write($"Equipamento [{existente.Nome}]: ");   var n = Console.ReadLine();
            Console.Write($"Quantidade [{existente.Quantidade}]: "); var qStr = Console.ReadLine();
            Console.Write($"Instrutor [{existente.Instrutor}]: "); var i = Console.ReadLine();
            Console.Write($"Horário [{existente.Horario}]: "); var h = Console.ReadLine();

            int qVal;
            bool okQ = int.TryParse(qStr, out qVal);
            return new Equipamento
            {
                Id = existente.Id,
                Nome = string.IsNullOrWhiteSpace(n) ? existente.Nome : n,
                Quantidade = okQ && qVal > 0 ? qVal : existente.Quantidade,
                Instrutor = string.IsNullOrWhiteSpace(i) ? existente.Instrutor : i,
                Horario = string.IsNullOrWhiteSpace(h) ? existente.Horario : h
            };
        }

        /*--------------------------------------------------------------*
         * UTILITÁRIOS                                                  *
         *--------------------------------------------------------------*/
        public bool Confirmar(string mensagem)
        {
            Console.Write($"{mensagem} (S/N) ");
            return Console.ReadKey(true).Key == ConsoleKey.S;
        }

        public void IdInexistente()
        {
            Console.WriteLine("\nId inexistente.");
            Console.WriteLine("<Enter> para continuar…");
            Console.ReadLine();
        }

        public void Sucesso(string mensagem)
        {
            Console.WriteLine($"\n{mensagem}");
            Console.WriteLine("<Enter> para continuar…");
            Console.ReadLine();
        }
    }
}
