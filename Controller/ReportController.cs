using System;
using GestorGinasio.Model.Exceptions;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    public class ReportController : IReportController
    {
        private readonly ISocioService _socioSvc;
        private readonly IAulaService _aulaSvc;
        private readonly IEquipamentoService _equipSvc;
        private readonly IReportService _reportSvc;
        private readonly IReportView _view;

        public ReportController(
            ISocioService socioSvc,
            IAulaService aulaSvc,
            IEquipamentoService equipSvc,
            IReportService reportSvc,
            IReportView view)
        {
            _socioSvc = socioSvc ?? throw new ArgumentNullException(nameof(socioSvc));
            _aulaSvc = aulaSvc ?? throw new ArgumentNullException(nameof(aulaSvc));
            _equipSvc = equipSvc ?? throw new ArgumentNullException(nameof(equipSvc));
            _reportSvc = reportSvc ?? throw new ArgumentNullException(nameof(reportSvc));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Gerir(string currentUser)
        {
            try
            {
                while (true)
                {
                    var key = _view.MostrarMenu();
                    switch (key)
                    {
                        case ConsoleKey.D1:
                            _reportSvc.GenerateSociosReport(_socioSvc.GetAll(), currentUser);
                            break;
                        case ConsoleKey.D2:
                            _reportSvc.GenerateAulasReport(_aulaSvc.GetAll(), currentUser);
                            break;
                        case ConsoleKey.D3:
                            _reportSvc.GenerateEquipamentosReport(_equipSvc.GetAll(), currentUser);
                            break;
                        case ConsoleKey.D0:
                        case ConsoleKey.Escape:
                            return;
                    }
                    Console.Write("\n<Enter> para voltar…");
                    Console.ReadLine();
                }
            }
            catch (JsonFileFormatException ex)
            {
                Console.Clear();
                Console.WriteLine($"Erro de Formato JSON:\n{ex.Message}");
                Console.WriteLine("O programa será encerrado...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
    }
}
