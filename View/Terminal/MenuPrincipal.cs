using System;
using System.Threading;
using GestorGinasio.Model.Services;

namespace GestorGinasio.View.Terminal
{
    public class MenuPrincipal
    {
        public string ActiveUsername { get; set; }
        public string ActiveUserRole { get; set; }
        public AuthService AuthService { get; set; }

        public void ExibirMenu()
        {
            while (true)
            {
                Console.Clear();
                DrawHeader();

                // Itens do menu
                Console.WriteLine();
                Console.WriteLine("=== MENU PRINCIPAL ===");
                Console.WriteLine("1. Sócios");
                Console.WriteLine("2. Aulas");
                Console.WriteLine("3. Equipamentos");
                Console.WriteLine("4. Utilizadores");
                Console.WriteLine("5. Relatórios");
                Console.WriteLine("6. Trocar de Utilizador");
                Console.WriteLine("7. Sair da aplicação");
                Console.Write("Selecione uma opção (1-7): ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        new SociosView(ActiveUsername, ActiveUserRole, new SocioService()).Exibir();
                        break;
                    case "2":
                        new AulasView(ActiveUsername, ActiveUserRole, new AulaService()).Exibir();
                        break;
                    case "3":
                        new EquipamentosView(ActiveUsername, ActiveUserRole, new EquipamentoService()).Exibir();
                        break;
                    case "4":
                        new UtilizadoresView(ActiveUsername, ActiveUserRole, AuthService).Exibir();
                        break;
                    case "5":
                        new RelatoriosView(ActiveUsername, ActiveUserRole).Exibir();
                        break;
                    case "6":
                        return; // logout
                    case "7":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opção inválida! Tente novamente...");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }

        private void DrawHeader()
        {
            int w = Console.WindowWidth;
            Console.WriteLine(new string('=', w));
            // data/hora à esquerda
            string dt = DateTime.Now.ToString("dddd HH:mm:ss");
            Console.Write(dt);
            // utilizador à direita
            string lbl = ActiveUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase)
                ? "Administrador: " : "Utilizador: ";
            string usr = lbl + ActiveUsername;
            Console.SetCursorPosition(w - usr.Length, Console.CursorTop);
            Console.WriteLine(usr);
            Console.WriteLine(new string('=', w));
        }
    }
}
