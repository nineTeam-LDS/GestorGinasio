using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller;

internal class ReportController
{
    private readonly string _activeUser;
    public ReportController(string activeUser) => _activeUser = activeUser;

    public void Gerir()
    {
        while (true)
        {
            var k = GerarRelatoriosView.MenuRelatorios();   // devolve ConsoleKey
            switch (k)
            {
                case ConsoleKey.D1:
                    ReportService.GerarSocios(new SocioService().GetAll(), _activeUser);
                    break;
                case ConsoleKey.D2:
                    ReportService.GerarAulas(new AulaService().GetAll(), _activeUser);
                    break;
                case ConsoleKey.D3:
                    ReportService.GerarEquip(new EquipamentoService().GetAll(), _activeUser);
                    break;
                case ConsoleKey.D0:
                case ConsoleKey.Escape:
                    return;
            }
            Console.Write("\n<Enter> para voltar…"); Console.ReadLine();
        }
    }
}

