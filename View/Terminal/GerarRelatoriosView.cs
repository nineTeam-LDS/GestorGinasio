// File: View/RelatoriosView.cs
using System;

namespace GestorGinasio.View.Terminal
{
    // Implementação de IReportView para o terminal.
    public class GerarRelatoriosView : IReportView
    {
        public ConsoleKey MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("\n===== RELATÓRIOS =====\n");
            Console.WriteLine("1. Sócios");
            Console.WriteLine("2. Aulas");
            Console.WriteLine("3. Equipamentos");
            Console.WriteLine("0. Voltar");
            Console.Write("Opção: ");
            return Console.ReadKey(true).Key;
        }
    }
}
