// File: Model/Exceptions/BusinessException.cs
namespace GestorGinasio.Model.Exceptions
{
    // Exceção para violações de regra de negócio (ex.: email duplicado, nome inválido…).
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, Exception inner) : base(message, inner) { }
    }
}
