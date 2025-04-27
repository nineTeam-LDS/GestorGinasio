using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    public static class GerirUtilizadoresView
    {
        // ---- LISTAR ----------------------------------------------------
        public static void MostrarLista(IEnumerable<User> lista)
        {
            Console.Clear();
            Console.WriteLine("\n===== UTILIZADORES =====\n");

            foreach (var u in lista)
                Console.WriteLine($"{u.Id,3}  {u.Username,-15}  ({u.Role})");

            Console.WriteLine("\n<Enter> para voltar …");
            Console.ReadLine();
        }

        // ---- ADICIONAR -------------------------------------------------
        public static User PedirNovoUtilizador()
        {
            Console.Clear();
            Console.WriteLine("\n===== NOVO UTILIZADOR =====\n");
            Console.Write("Username : "); var user = Console.ReadLine()!;
            Console.Write("Password : "); var pass = Console.ReadLine()!;
            Console.Write("Role     : "); var role = Console.ReadLine()!;

            return new User { Username = user, Password = pass, Role = role };
        }

        // ---- REMOVER ---------------------------------------------------
        public static int PedirIdParaRemover()
        {
            Console.Write("\nId a remover: ");
            return int.TryParse(Console.ReadLine(), out var id) ? id : -1;
        }

        // ---- EDITAR --------------------------------------------------------
        public static int PedirIdParaEditar()
        {
            Console.Write("\nId a editar: ");
            return int.TryParse(Console.ReadLine(), out var id) ? id : -1;
        }

        public static User PedirDadosEditados(User original)
        {
            Console.Clear();
            Console.WriteLine("\n===== EDITAR UTILIZADOR =====");
            Console.WriteLine($"(Enter = manter o valor actual)\n");

            Console.Write($"Username [{original.Username}]: ");
            var user = Console.ReadLine();
            Console.Write($"Password [{new string('*', original.Password.Length)}]: ");
            var pass = Console.ReadLine();
            Console.Write($"Role [{original.Role}]: ");
            var role = Console.ReadLine();

            return new User
            {
                Id = original.Id,
                Username = string.IsNullOrWhiteSpace(user) ? original.Username : user,
                Password = string.IsNullOrWhiteSpace(pass) ? original.Password : pass,
                Role = string.IsNullOrWhiteSpace(role) ? original.Role : role
            };
        }

        // Confirmação genérica ----------------------------------------------
        public static bool Confirmar(string mensagem)
        {
            Console.Write($"{mensagem} (S/N) ");
            return Console.ReadKey(true).Key == ConsoleKey.S;
        }
    }
}
