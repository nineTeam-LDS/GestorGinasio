// File: View/RelatoriosView.cs
using System;
using System.Threading;
using GestorGinasio.Model.Services;

namespace GestorGinasio.View.Terminal
{
    public static class GerarRelatoriosView
    {
        public static ConsoleKey MenuRelatorios()
        {
            Console.Clear();
            Console.WriteLine("=== RELATÓRIOS ===");
            Console.WriteLine("1. Sócios");
            Console.WriteLine("2. Aulas");
            Console.WriteLine("3. Equipamentos");
            Console.WriteLine("0. Voltar");
            Console.Write("Opção: ");
            return Console.ReadKey(true).Key;
        }
    }
}
