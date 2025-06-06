// File: Model/Exceptions/RepositoryException.cs
namespace GestorGinasio.Model.Exceptions
{
    // Exceção geral para falhas ao aceder/gravar no repositório de dados.
    public class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message) { }
        public RepositoryException(string message, Exception inner) : base(message, inner) { }
    }
}
