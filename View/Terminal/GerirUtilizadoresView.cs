// File: View/Terminal/GerirUtilizadoresView.cs
using System;
using System.Collections.Generic;
using System.Linq;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Implementação concreta da interface de Gestão de Utilizadores no terminal.
    public class GerirUtilizadoresView : IUserView
    {
        public ConsoleKey MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("\n===== UTILIZADORES =====\n");
            Console.WriteLine("1. Listar");
            Console.WriteLine("2. Adicionar");
            Console.WriteLine("3. Editar");
            Console.WriteLine("4. Remover");
            Console.WriteLine("0. Voltar");
            Console.Write("Opção: ");
            return Console.ReadKey(true).Key;
        }

        // ---- LISTAR ----------------------------------------------------
        public void MostrarLista(IEnumerable<User> utilizadores)
        {
            Console.Clear();
            Console.WriteLine("\n===== UTILIZADORES =====\n");
            Console.WriteLine("Id   Username        Role");
            foreach (var u in utilizadores)
                Console.WriteLine($"{u.Id,-3} {u.Username,-15} {u.Role}");
            Console.WriteLine("\n<Enter> para voltar …");
            Console.ReadLine();
        }

        // ---- ADICIONAR -------------------------------------------------
        public User PedirNovoUtilizador()
        {
            Console.Clear();
            Console.WriteLine("\n===== NOVO UTILIZADOR =====\n");

            string username;
            do
            {
                Console.Write("Username : ");
                username = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Username não pode ficar em branco.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(username));

            string password;
            do
            {
                Console.Write("Password : ");
                password = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Password inválida (mínimo 4 caracteres).");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(password) || password.Length < 4);

            string role;
            do
            {
                Console.Write("Role     : ");
                role = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(role))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Role não pode ficar em branco.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(role));

            return new User
            {
                Username = username.Trim(),
                Password = password.Trim(),
                Role = role.Trim()
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

        public User PedirDadosEditados(User existente)
        {
            Console.Clear();
            Console.WriteLine("\n===== EDITAR UTILIZADOR =====");
            Console.WriteLine("(Enter = manter o valor actual)\n");

            Console.Write($"Username [{existente.Username}]: ");
            var username = Console.ReadLine();
            Console.Write($"Password [{new string('*', existente.Password.Length)}]: ");
            var password = Console.ReadLine();
            Console.Write($"Role [{existente.Role}]: ");
            var role = Console.ReadLine();

            return new User
            {
                Id = existente.Id,
                Username = string.IsNullOrWhiteSpace(username)
                              ? existente.Username
                              : username.Trim(),
                Password = string.IsNullOrWhiteSpace(password) || password.Length < 4
                              ? existente.Password
                              : password.Trim(),
                Role = string.IsNullOrWhiteSpace(role)
                              ? existente.Role
                              : role.Trim()
            };
        }

        // ---- REMOVER ---------------------------------------------------
        public int PedirIdParaRemover()
        {
            while (true)
            {
                Console.Write("\nId a remover: ");
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
