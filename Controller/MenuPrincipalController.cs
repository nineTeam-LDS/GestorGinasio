// File: Controller/MenuPrincipalController.cs
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
        private readonly IErrorHandler _errorHandler;

        public MenuPrincipalController(
            IMenuPrincipalView view,
            ISocioController socioCtrl,
            IAulaController aulaCtrl,
            IEquipamentoController equipCtrl,
            IUserController userCtrl,
            IReportController reportCtrl,
            IErrorHandler errorHandler)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _socioCtrl = socioCtrl ?? throw new ArgumentNullException(nameof(socioCtrl));
            _aulaCtrl = aulaCtrl ?? throw new ArgumentNullException(nameof(aulaCtrl));
            _equipCtrl = equipCtrl ?? throw new ArgumentNullException(nameof(equipCtrl));
            _userCtrl = userCtrl ?? throw new ArgumentNullException(nameof(userCtrl));
            _reportCtrl = reportCtrl ?? throw new ArgumentNullException(nameof(reportCtrl));
            _errorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
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
                        case ConsoleKey.D1:
                            ExecutarComTratamento(() => _socioCtrl.Gerir());
                            break;
                        case ConsoleKey.D2:
                            ExecutarComTratamento(() => _aulaCtrl.Gerir());
                            break;
                        case ConsoleKey.D3:
                            ExecutarComTratamento(() => _equipCtrl.Gerir());
                            break;
                        case ConsoleKey.D4:
                            ExecutarComTratamento(() => _userCtrl.Gerir());
                            break;
                        case ConsoleKey.D5:
                            ExecutarComTratamento(() => _reportCtrl.Gerir(activeUser));
                            break;
                        case ConsoleKey.D0:
                        case ConsoleKey.Escape:
                            return;
                        default:
                            // tecla inválida volta ao menu
                            break;
                    }
                }
            }
            catch (JsonFileFormatException ex)
            {
                Console.Clear();
                Console.WriteLine($"Erro de Formato JSON: {ex.Message}");
                _errorHandler.LogError(ex, nameof(MostrarMenu));
                Console.WriteLine("\nO programa será encerrado.");
                Console.ReadKey();
                Environment.Exit(1);
            }
            catch (RepositoryException ex)
            {
                Console.Clear();
                Console.WriteLine($"Erro de I/O: {ex.Message}");
                _errorHandler.LogError(ex, nameof(MostrarMenu));
                Console.WriteLine("\nO programa será encerrado.");
                Console.ReadKey();
                Environment.Exit(1);
            }
            catch (PdfGenerationException ex)
            {
                // Se vier deste menu (por ex. tentando gerar relatório), mostramos e voltamos ao menu
                Console.Clear();
                _errorHandler.ShowError($"Falha ao gerar PDF: {ex.Message}");
                _errorHandler.LogError(ex, nameof(MostrarMenu));
                // Não sai do programa; recomeça o loop
                MostrarMenu(activeUser, activeRole);
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                _errorHandler.LogError(ex, nameof(MostrarMenu));
                Console.WriteLine("\nO programa será encerrado.");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        private void ExecutarComTratamento(Action acao)
        {
            try
            {
                acao();
            }
            catch (BusinessException bex)
            {
                _errorHandler.ShowError($"Falha de negócio: {bex.Message}");
                _errorHandler.LogError(bex, nameof(ExecutarComTratamento));
            }
            catch (JsonFileFormatException jex)
            {
                _errorHandler.ShowError("JSON corrompido em dados de domínio.");
                _errorHandler.LogError(jex, nameof(ExecutarComTratamento));
                // se desejar abortar após falha grave, poderia chamar Environment.Exit(1)
            }
            catch (RepositoryException rex)
            {
                _errorHandler.ShowError("Falha de I/O ao aceder aos dados de domínio.");
                _errorHandler.LogError(rex, nameof(ExecutarComTratamento));
            }
            catch (PdfGenerationException pex)
            {
                _errorHandler.ShowError($"Falha ao gerar PDF: {pex.Message}");
                _errorHandler.LogError(pex, nameof(ExecutarComTratamento));
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError($"Erro interno: {ex.Message}");
                _errorHandler.LogError(ex, nameof(ExecutarComTratamento));
            }
        }
    }
}

