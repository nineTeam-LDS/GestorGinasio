using System;
using System.Threading;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.Title = "nineTeam - Gestão de Ginásio";
                Console.WriteLine("Bem‑vindo à Gestão de Ginásio da nineTeam");
                Console.WriteLine();

                var loginView = new LoginView();
                var authService = new AuthService();
                var user = loginView.SolicitarCredenciais();
                var ok = authService.ValidarCredenciais(user);
                loginView.MostrarResultadoLogin(ok);

                if (!ok) break;

                Console.WriteLine("A iniciar aplicação...");
                Thread.Sleep(2000);
                Console.Clear();

                var menu = new MenuPrincipal
                {
                    ActiveUsername = user.Username,
                    ActiveUserRole = user.Role,
                    AuthService = authService
                };
                menu.ExibirMenu();

                Console.Write("Trocar de utilizador? (S/N) ");
                if (!Console.ReadLine().Trim().Equals("S", StringComparison.OrdinalIgnoreCase))
                    break;
            }

            Console.WriteLine("Aplicação encerrada. Pressione Enter para sair.");
            Console.ReadKey();
        }
    }
}
