// Error Handling MVC: Model/Exceptions/JsonFileFormatException.cs

namespace GestorGinasio.Model.Exceptions
{
    public class JsonFileFormatException : Exception
    {
        public JsonFileFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
