using System;
using System.Threading;
using GestorGinasio.Controller;
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
                var loginView = new LoginView();
                var authService = new AuthService();

                var user = loginView.SolicitarCredenciais();
                bool ok = authService.ValidarCredenciais(user);
                loginView.MostrarResultadoLogin(ok);

                if (!ok)
                {
                    Console.WriteLine("Prima Enter para tentar novamente…");
                    Console.ReadLine();
                    continue;           // em vez de break
                }

                // --- só entra aqui se o login for bem-sucedido ---
                Console.WriteLine("A iniciar aplicação...");
                Thread.Sleep(1000);
                Console.Clear();

                var menuCtrl = new MenuPrincipalController();
                menuCtrl.MostrarMenu(user.Username, user.Role, authService);

                Console.Write("Trocar de utilizador? (S/N) ");
                if (!Console.ReadLine()!.Trim()
                        .Equals("S", StringComparison.OrdinalIgnoreCase))
                    break;
            }

            Console.WriteLine("Aplicação encerrada. Enter para sair.");
            Console.ReadKey();
        }
    }
}
