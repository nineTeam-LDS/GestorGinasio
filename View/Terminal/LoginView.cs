using System;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    public class LoginView
    {
        public User SolicitarCredenciais()
        {
            Console.WriteLine("=== LOGIN ===");
            Console.Write("Usuário: ");
            var username = Console.ReadLine();
            Console.Write("Senha: ");
            var password = ReadPassword();
            return new User { Username = username, Password = password };
        }

        private string ReadPassword()
        {
            var sb = new System.Text.StringBuilder();
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
