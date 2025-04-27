using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    internal class MenuPrincipalController
    {
        private readonly SocioController _socios = new();
        private readonly UserController _users = new();

        public void MostrarMenu(string activeUser, string activeRole, AuthService authService)
        {
            while (true)
            {
                MenuPrincipalView.MostrarCabecalho(activeUser, activeRole);
                var key = MenuPrincipalView.MostrarOpcoes();

                switch (key)
                {
                    case ConsoleKey.D1:                         // 1. Sócios
                        new SocioController().Gerir();
                        break;
                    case ConsoleKey.D2:
                        new AulaController().Gerir();           // 2. Aulas 
                        break;
                    case ConsoleKey.D3:                         // 3. Equipamentos
                        new EquipamentoController().Gerir();
                        break;
                    case ConsoleKey.D4:                         // 4. Utilizadores
                        _users.Gerir();
                        break;
                    case ConsoleKey.D5:                         // 5. Relatórios
                        new ReportController(activeUser).Gerir();
                        break;
                    case ConsoleKey.D0:
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}

