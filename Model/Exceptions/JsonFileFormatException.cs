// File: Model/Exceptions/JsonFileFormatException.cs
namespace GestorGinasio.Model.Exceptions
{
    // Exceção para indicar que um JSON não está bem‐formado ou não pôde ser desserializado.
    public class JsonFileFormatException : Exception
    {
        public JsonFileFormatException(string message) : base(message) { }
        public JsonFileFormatException(string message, Exception inner) : base(message, inner) { }
    }
}