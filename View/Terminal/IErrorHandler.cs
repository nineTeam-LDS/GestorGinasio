// File: View/Terminal/IErrorHandler.cs
namespace GestorGinasio.View.Terminal
{
    // Responsável por “formatar” e exibir erros de todo o sistema.
    public interface IErrorHandler
    {
        // Mostra uma mensagem de erro no ecrã e aguarda ENTER (estilo consola).
        void ShowError(string message);

        // Grava o stack‐trace ou detalhes no ficheiro de log (opcional).
        void LogError(Exception ex, string context);
    }
}

