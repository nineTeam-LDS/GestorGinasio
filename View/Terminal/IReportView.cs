using System;

namespace GestorGinasio.View.Terminal
{
    // Contrato para a interface da View de geração de relatórios.
    public interface IReportView
    {
        // Exibe o menu de relatórios e devolve a tecla selecionada.
        ConsoleKey MostrarMenu();
    }
}
