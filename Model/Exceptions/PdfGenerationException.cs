// File: Model/Exceptions/PdfGenerationException.cs
namespace GestorGinasio.Model.Exceptions
{
    // Exceção lançada quando ocorre um erro ao gerar ou gravar um PDF.
    public class PdfGenerationException : Exception
    {
        public PdfGenerationException(string message) : base(message) { }
        public PdfGenerationException(string message, Exception inner) : base(message, inner) { }
    }
}
