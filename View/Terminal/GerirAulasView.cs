// File: View/Terminal/GerirAulasView.cs
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

        // ---- LISTAR ----------------------------------------------------
        public void MostrarLista(IEnumerable<Aula> lista)
        {
            Console.Clear();
            Console.WriteLine("\n===== AULAS =====\n");
            Console.WriteLine($"{"Id",-3} {"Nome",-15} {"Instrutor",-12} {"Sala",-6} Horário");
            foreach (var a in lista)
                Console.WriteLine($"{a.Id,-3} {a.Nome,-15} {a.Instrutor,-12} {a.Sala,-6} {a.Horario}");
            Console.WriteLine("\n<Enter> para voltar…");
            Console.ReadLine();
        }

        // ---- ADICIONAR -------------------------------------------------
        public Aula PedirNovaAula()
        {
            Console.Clear();
            Console.WriteLine("\n===== NOVA AULA =====\n");

            // Nome (não pode ficar vazio)
            string nome;
            do
            {
                Console.Write("Nome: ");
                nome = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(nome))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("O nome da aula não pode ficar em branco.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(nome));

            // Instrutor (não pode ficar vazio)
            string instrutor;
            do
            {
                Console.Write("Instrutor: ");
                instrutor = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(instrutor))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("O nome do instrutor não pode ficar em branco.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(instrutor));

            // Sala (não pode ficar vazia)
            string sala;
            do
            {
                Console.Write("Sala: ");
                sala = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(sala))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("O campo Sala não pode ficar em branco.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(sala));

            // Horário (formato mínimo “HH:mm-HH:mm”)
            string horario;
            do
            {
                Console.Write("Horário (HH:mm-HH:mm): ");
                horario = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(horario) || !horario.Contains("-"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Horário inválido. Use formato exemplo: 09:00-10:00.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(horario) || !horario.Contains("-"));

            return new Aula
            {
                Nome = nome.Trim(),
                Instrutor = instrutor.Trim(),
                Sala = sala.Trim(),
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

        public Aula PedirDadosEditados(Aula existente)
        {
            Console.Clear();
            Console.WriteLine("\n===== EDITAR AULA === (Enter = manter valor atual) ===\n");

            Console.Write($"Nome [{existente.Nome}]: ");
            var n = Console.ReadLine();
            Console.Write($"Instrutor [{existente.Instrutor}]: ");
            var i = Console.ReadLine();
            Console.Write($"Sala [{existente.Sala}]: ");
            var s = Console.ReadLine();
            Console.Write($"Horário [{existente.Horario}]: ");
            var h = Console.ReadLine();

            existente.Nome = string.IsNullOrWhiteSpace(n) ? existente.Nome : n.Trim();
            existente.Instrutor = string.IsNullOrWhiteSpace(i) ? existente.Instrutor : i.Trim();
            existente.Sala = string.IsNullOrWhiteSpace(s) ? existente.Sala : s.Trim();
            existente.Horario = string.IsNullOrWhiteSpace(h) ? existente.Horario : h.Trim();

            return existente;
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

