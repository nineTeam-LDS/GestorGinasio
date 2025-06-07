// File: View/Terminal/ErrorHandler.cs
using System;
using System.IO;

namespace GestorGinasio.View.Terminal
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly string _logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nERRO: " + message);
            Console.ResetColor();
            Console.WriteLine("<Enter> para continuar…");
            Console.ReadLine();
        }

        public void LogError(Exception ex, string context)
        {
            try
            {
                Directory.CreateDirectory(_logDirectory);
                var logFile = Path.Combine(_logDirectory,
                    $"erros_{DateTime.Now:yyyyMMdd}.txt");

                File.AppendAllText(
                    logFile,
                    $"[{DateTime.Now:HH:mm:ss}] Contexto: {context}\n" +
                    ex.ToString() + "\n\n"
                );
            }
            catch
            {
                // Se falhar a gravar log, não impede o resto do programa.
            }
        }
    }
}

