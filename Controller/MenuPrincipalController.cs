using System;
using GestorGinasio.Controller;
using GestorGinasio.Model.Exceptions;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    public class MenuPrincipalController : IMenuPrincipalController
    {
        private readonly IMenuPrincipalView _view;
        private readonly ISocioController _socioCtrl;
        private readonly IAulaController _aulaCtrl;
        private readonly IEquipamentoController _equipCtrl;
        private readonly IUserController _userCtrl;
        private readonly IReportController _reportCtrl;

        public MenuPrincipalController(
            IMenuPrincipalView view,
            ISocioController socioCtrl,
            IAulaController aulaCtrl,
            IEquipamentoController equipCtrl,
            IUserController userCtrl,
            IReportController reportCtrl)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _socioCtrl = socioCtrl ?? throw new ArgumentNullException(nameof(socioCtrl));
            _aulaCtrl = aulaCtrl ?? throw new ArgumentNullException(nameof(aulaCtrl));
            _equipCtrl = equipCtrl ?? throw new ArgumentNullException(nameof(equipCtrl));
            _userCtrl = userCtrl ?? throw new ArgumentNullException(nameof(userCtrl));
            _reportCtrl = reportCtrl ?? throw new ArgumentNullException(nameof(reportCtrl));
        }

        public void MostrarMenu(string activeUser, string activeRole)
        {
            try
            {
                while (true)
                {
                    _view.MostrarCabecalho(activeUser, activeRole);
                    var key = _view.MostrarOpcoes();

                    switch (key)
                    {
                        case ConsoleKey.D1: _socioCtrl.Gerir(); break;
                        case ConsoleKey.D2: _aulaCtrl.Gerir(); break;
                        case ConsoleKey.D3: _equipCtrl.Gerir(); break;
                        case ConsoleKey.D4: _userCtrl.Gerir(); break;
                        case ConsoleKey.D5: _reportCtrl.Gerir(activeUser); break;
                        case ConsoleKey.D0:
                        case ConsoleKey.Escape:
                            return;
                    }
                }
            }
            catch (JsonFileFormatException ex)
            {
                Console.Clear();
                Console.WriteLine($"Erro de Formato JSON Detetado:\n{ex.Message}");
                Console.WriteLine("\nO programa será encerrado...");
                Console.ReadKey();
                Environment.Exit(1); // Ou voltar para MenuPrincipal
            }
        }
    }
}

