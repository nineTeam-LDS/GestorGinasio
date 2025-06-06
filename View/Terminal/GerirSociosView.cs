// File: View/Terminal/GerirSociosView.cs
using System;
using System.Collections.Generic;
using System.Linq;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Implementação concreta de ISociosView para terminal.
    public class GerirSociosView : ISociosView
    {
        public ConsoleKey MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("\n===== SÓCIOS =====\n");
            Console.WriteLine("1. Listar");
            Console.WriteLine("2. Criar");
            Console.WriteLine("3. Editar");
            Console.WriteLine("4. Remover");
            Console.WriteLine("0. Voltar");
            Console.Write("Opção: ");
            return Console.ReadKey(true).Key;
        }

        // ---- LISTAR ----------------------------------------------------
        public void MostrarLista(IEnumerable<Socio> lista)
        {
            Console.Clear();
            Console.WriteLine("\n===== SÓCIOS =====\n");
            Console.WriteLine("{0,-4} {1,-20} {2,-25} {3}", "Id", "Nome", "Email", "Data Inscrição");
            foreach (var s in lista)
                Console.WriteLine($"{s.Id,-4} {s.Nome,-20} {s.Email,-25} {s.DataInscricao:yyyy-MM-dd}");
            Console.WriteLine("\n<Enter> para voltar…");
            Console.ReadLine();
        }
        public void MostrarDetalhes(Socio socio)
        {
            Console.Clear();
            Console.WriteLine($"ID: {socio.Id}\nNome: {socio.Nome}\nEmail: {socio.Email}\nData: {socio.DataInscricao:yyyy-MM-dd}");
            Console.WriteLine("\n<Enter> para continuar…");
            Console.ReadLine();
        }

        // ---- ADICIONAR -------------------------------------------------
        public Socio PedirNovoSocio()
        {
            Console.Clear();
            Console.WriteLine("\n===== NOVO SÓCIO =====\n");

            string nome;
            do
            {
                Console.Write("Nome:  ");
                nome = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(nome))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("O nome não pode ficar em branco.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(nome));

            string email;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Email inválido. Deve conter um '@'.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(email) || !email.Contains("@"));

            return new Socio
            {
                Nome = nome.Trim(),
                Email = email.Trim(),
                DataInscricao = DateTime.Today
            };
        }

        // ---- EDITAR --------------------------------------------------------
        public int PedirIdParaEditar()
        {
            while (true)
            {
                Console.Write("\nId a editar: ");
                var texto = Console.ReadLine();
                if (int.TryParse(texto, out var id) && id > 0)
                    return id;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Id inválido. Introduza um número inteiro maior que zero.");
                Console.ResetColor();
            }
        }

        public Socio PedirDadosEditados(Socio existente)
        {
            Console.Clear();
            Console.WriteLine("\n===== EDITAR SÓCIO ===== (Enter = manter)\n");
            Console.Write($"Nome  [{existente.Nome}]: "); var n = Console.ReadLine();
            Console.Write($"Email [{existente.Email}]: "); var e = Console.ReadLine();
            existente.Nome = string.IsNullOrWhiteSpace(n) ? existente.Nome : n;
            existente.Email = string.IsNullOrWhiteSpace(e) ? existente.Email : e;
            return existente;
        }

        // ---- REMOVER ---------------------------------------------------
        public int PedirIdParaRemover() => PedirIdParaEditar();

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
