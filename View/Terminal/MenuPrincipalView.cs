namespace GestorGinasio.View.Terminal
{
    public static class MenuPrincipalView
    {
        public static void MostrarCabecalho(string user, string role)
        {
            Console.Clear();
            Console.WriteLine(new string('=', 70));
            Console.WriteLine($"{DateTime.Now:dddd HH:mm:ss}".PadRight(40) +
                              $"Utilizador: {user} ({role})");
            Console.WriteLine(new string('=', 70));
        }

        public static ConsoleKey MostrarOpcoes()
        {
            Console.WriteLine("\n===== MENU PRINCIPAL =====\n");
            Console.WriteLine("1. Sócios");
            Console.WriteLine("2. Aulas");
            Console.WriteLine("3. Equipamentos");
            Console.WriteLine("4. Utilizadores");
            Console.WriteLine("5. Relatórios");
            Console.WriteLine("0. Sair");
            Console.Write("Opção: ");
            return Console.ReadKey(true).Key;
        }
    }
}

