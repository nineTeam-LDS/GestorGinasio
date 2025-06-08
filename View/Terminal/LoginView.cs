// File: View/Terminal/LoginView.cs
using System;
using System.Text;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Implementação concreta da interface de login no terminal.
    // Valida no mínimo que o username e password não fiquem em branco.
    public class LoginView : ILoginView
    {
        public User SolicitarCredenciais()
        {
            Console.Clear();
            Console.WriteLine("---- nineTeam : IT solutions ----");
            Console.WriteLine("\n===== LOGIN =====\n");

            string username;
            do
            {
                Console.Write("Utilizador: ");
                username = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("O nome de utilizador não pode ficar em branco.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(username));

            string password;
            do
            {
                Console.Write("Senha: ");
                password = ReadPassword().Trim();
                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("A password não pode ficar em branco.");
                    Console.ResetColor();
                }
            } while (string.IsNullOrWhiteSpace(password));

            return new User { Username = username, Password = password };
        }

        private string ReadPassword()
        {
            var sb = new StringBuilder();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Enter)
                {
                    sb.Append(key.KeyChar);
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return sb.ToString();
        }

        public void MostrarResultadoLogin(bool sucesso)
        {
            Console.WriteLine(sucesso
                ? "Login efetuado com sucesso!"
                : "Credenciais inválidas. Tente novamente.");
        }
    }
}
