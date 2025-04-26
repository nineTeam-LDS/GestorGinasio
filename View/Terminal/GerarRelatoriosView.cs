// File: View/RelatoriosView.cs
using System;
using System.Threading;
using GestorGinasio.Model.Services;

namespace GestorGinasio.View.Terminal
{
    public class RelatoriosView
    {
        private readonly string _user, _role;

        public RelatoriosView(string username, string role)
        {
            _user = username;
            _role = role;
        }

        public void Exibir()
        {
            while (true)
            {
                Console.Clear();
                DrawHeader();
                Console.WriteLine();
                Console.WriteLine("=== RELATÓRIOS ===");
                Console.WriteLine("1. Sócios");
                Console.WriteLine("2. Aulas");
                Console.WriteLine("3. Equipamentos");
                Console.WriteLine("4. Voltar");
                Console.Write("Opção (1-4): ");

                var op = Console.ReadLine();
                if (op == "4") break;

                switch (op)
                {
                    case "1":
                        ReportService.GenerateSociosReport(
                            new SocioService().GetAll(),
                            _user
                        );
                        break;
                    case "2":
                        ReportService.GenerateAulasReport(
                            new AulaService().GetAll(),
                            _user
                        );
                        break;
                    case "3":
                        ReportService.GenerateEquipamentosReport(
                            new EquipamentoService().GetAll(),
                            _user
                        );
                        break;
                    default:
                        Console.WriteLine("Inválido!");
                        Thread.Sleep(1000);
                        continue;
                }

                Console.WriteLine();
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }

        private void DrawHeader()
        {
            int w = Console.WindowWidth;
            Console.WriteLine(new string('=', w));

            // Data/Hora à esquerda
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(DateTime.Now.ToString("dddd HH:mm:ss"));

            // Utilizador à direita
            string lbl = _role.Equals("Admin", StringComparison.OrdinalIgnoreCase)
                ? "Administrador: " : "Utilizador: ";
            string usr = lbl + _user;
            Console.SetCursorPosition(w - usr.Length, Console.CursorTop);
            Console.WriteLine(usr);

            Console.WriteLine(new string('=', w));
        }
    }
}
