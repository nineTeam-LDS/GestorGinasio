// File: View/Terminal/GerirEquipamentosView.cs
using System;
using System.Collections.Generic;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Implementação de IEquipamentoView usando a consola.
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

        // ---- LISTAR ----------------------------------------------------
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

        // ---- ADICIONAR -------------------------------------------------
        public Equipamento PedirNovoEquipamento()
        {
            Console.Clear();
            Console.WriteLine("\n===== NOVO EQUIPAMENTO =====\n");

            // Nome (não pode ficar vazio)
            string nome;
            do
            {
                Console.Write("Equipamento: ");
                nome = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(nome))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Nome do equipamento não pode ficar em branco.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(nome));

            // Quantidade (inteiro > 0)
            int quantidade;
            while (true)
            {
                Console.Write("Quantidade: ");
                var txtQ = Console.ReadLine();
                if (int.TryParse(txtQ, out quantidade) && quantidade > 0)
                    break;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Quantidade inválida. Introduza um número inteiro maior que zero.");
                Console.ResetColor();
            }

            // Instrutor (não pode ficar vazio)
            string instrutor;
            do
            {
                Console.Write("Instrutor: ");
                instrutor = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(instrutor))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Nome do instrutor não pode ficar em branco.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(instrutor));

            // Horário (não pode ficar vazio; validação mínima)
            string horario;
            do
            {
                Console.Write("Horário: ");
                horario = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(horario))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Horário não pode ficar em branco.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(horario));

            return new Equipamento
            {
                Nome = nome.Trim(),
                Quantidade = quantidade,
                Instrutor = instrutor.Trim(),
                Horario = horario.Trim()
            };
        }

        // ---- EDITAR --------------------------------------------------------
        public int PedirIdParaEditar()
        {
            while (true)
            {
                Console.Write("\nId para editar: ");
                var texto = Console.ReadLine();
                if (int.TryParse(texto, out var id) && id > 0)
                    return id;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Id inválido. Introduza um número inteiro maior que zero.");
                Console.ResetColor();
            }
        }

        public Equipamento PedirDadosEditados(Equipamento existente)
        {
            Console.Clear();
            Console.WriteLine("\n===== EDITAR EQUIPAMENTO =====");
            Console.WriteLine("(Enter = manter o valor atual)\n");

            Console.Write($"Equipamento [{existente.Nome}]: ");
            var nome = Console.ReadLine();

            Console.Write($"Quantidade [{existente.Quantidade}]: ");
            var txtQ = Console.ReadLine();

            Console.Write($"Instrutor [{existente.Instrutor}]: ");
            var instr = Console.ReadLine();

            Console.Write($"Horário [{existente.Horario}]: ");
            var hora = Console.ReadLine();

            // Se o texto for vazio, mantém o valor atual
            var novoNome = string.IsNullOrWhiteSpace(nome) ? existente.Nome : nome.Trim();

            int qtdAtual = existente.Quantidade;
            int quantidade;
            if (string.IsNullOrWhiteSpace(txtQ) || !int.TryParse(txtQ, out quantidade) || quantidade <= 0)
                quantidade = qtdAtual;

            var novoInstr = string.IsNullOrWhiteSpace(instr) ? existente.Instrutor : instr.Trim();
            var novoHora = string.IsNullOrWhiteSpace(hora) ? existente.Horario : hora.Trim();

            return new Equipamento
            {
                Id = existente.Id,
                Nome = novoNome,
                Quantidade = quantidade,
                Instrutor = novoInstr,
                Horario = novoHora
            };
        }

        // ---- REMOVER ---------------------------------------------------
        public int PedirIdParaRemover()
        {
            while (true)
            {
                Console.Write("\nId para remover: ");
                var texto = Console.ReadLine();
                if (int.TryParse(texto, out var id) && id > 0)
                    return id;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Id inválido. Introduza um número inteiro maior que zero.");
                Console.ResetColor();
            }
        }

        // ---- UTILITÁRIOS ---------------------------------------------------
        public bool Confirmar(string mensagem)
        {
            Console.Write($"{mensagem} (S/N) ");
            return Console.ReadKey(true).Key == ConsoleKey.S;
        }

        public void Sucesso(string mensagem)
        {
            Console.WriteLine($"\n{mensagem}");
            Console.WriteLine("<Enter> para continuar…");
            Console.ReadLine();
        }
        public void Avaliar(string mensagem)
        {
            // Caso queira exibir mensagens de aviso leve (ex.: “Id não existe”)
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n{mensagem}");
            Console.ResetColor();
            Console.WriteLine("<Enter> para continuar…");
            Console.ReadLine();
        }
    }
}
