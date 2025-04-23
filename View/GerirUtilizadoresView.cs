using System;
using System.Threading;
using GestorGinasio.Services;

namespace GestorGinasio.View
{
    public class UtilizadoresView
    {
        private readonly string _user, _role;
        private readonly AuthService _auth;

        public UtilizadoresView(string username, string role, AuthService authService)
        {
            _user = username;
            _role = role;
            _auth = authService;
        }

        public void Exibir()
        {
            if (_role != "Admin")
            {
                Console.WriteLine("Acesso negado.");
                Thread.Sleep(1500);
                return;
            }

            while (true)
            {
                Console.Clear();
                DrawHeader();
                Console.WriteLine();
                Console.WriteLine("=== UTILIZADORES ===");
                Console.WriteLine("1. Criar Novo Utilizador");
                Console.WriteLine("2. Listar Utilizadores");
                Console.WriteLine("3. Remover Utilizador");
                Console.WriteLine("4. Voltar");
                Console.Write("Opção (1-4): ");

                var op = Console.ReadLine();
                if (op == "4") break;
                if (op == "2")
                {
                    foreach (var u in _auth.ListarUsuarios())
                        Console.WriteLine($"{u.Username} - {u.Role}");
                    Console.ReadKey();
                }
                else
                {
                    // implementar criacão / remoção
                }
            }
        }

        private void DrawHeader()
        {
            int w = Console.WindowWidth;
            Console.WriteLine(new string('=', w));
            Console.Write(DateTime.Now.ToString("dddd HH:mm:ss"));
            string usr = "Administrador: " + _user;
            Console.SetCursorPosition(w - usr.Length, Console.CursorTop);
            Console.WriteLine(usr);
            Console.WriteLine(new string('=', w));
        }
    }
}
