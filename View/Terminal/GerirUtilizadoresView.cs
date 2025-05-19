using System;
using System.Collections.Generic;
using System.Linq;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Implementação concreta da interface de Gestão de Utilizadores no terminal.
    public class GerirUtilizadoresView : IUserView
    {
        public ConsoleKey MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("\n===== UTILIZADORES =====\n");
            Console.WriteLine("1. Listar");
            Console.WriteLine("2. Adicionar");
            Console.WriteLine("3. Editar");
            Console.WriteLine("4. Remover");
            Console.WriteLine("0. Voltar");
            Console.Write("Opção: ");
            return Console.ReadKey(true).Key;
        }

        // ---- LISTAR ----------------------------------------------------
        public void MostrarLista(IEnumerable<User> usuarios)
        {
            Console.Clear();
            Console.WriteLine("\n===== UTILIZADORES =====\n");
            Console.WriteLine("Id   Username        Role");
            foreach (var u in usuarios)
                Console.WriteLine($"{u.Id,-3} {u.Username,-15} {u.Role}");
            Console.WriteLine("\n<Enter> para voltar …");
            Console.ReadLine();
        }

        // ---- ADICIONAR -------------------------------------------------
        public User PedirNovoUtilizador()
        {
            Console.Clear();
            Console.WriteLine("\n===== NOVO UTILIZADOR =====\n");
            Console.Write("Username : "); var username = Console.ReadLine()!;
            Console.Write("Password : "); var password = Console.ReadLine()!;
            Console.Write("Role     : "); var role = Console.ReadLine()!;

            return new User
            {
                Username = username,
                Password = password,
                Role = role
            };
        }

        // ---- REMOVER ---------------------------------------------------
        public int PedirIdParaRemover()
        {
            Console.Write("\nId a remover: ");
            return int.TryParse(Console.ReadLine(), out var id) ? id : -1;
        }

        // ---- EDITAR --------------------------------------------------------
        public int PedirIdParaEditar()
        {
            Console.Write("\nId a editar: ");
            return int.TryParse(Console.ReadLine(), out var id) ? id : -1;
        }

        public User PedirDadosEditados(User existente)
        {
            Console.Clear();
            Console.WriteLine("\n===== EDITAR UTILIZADOR =====");
            Console.WriteLine("(Enter = manter o valor actual)\n");

            Console.Write($"Username [{existente.Username}]: ");
            var username = Console.ReadLine();
            Console.Write($"Password [{new string('*', existente.Password.Length)}]: ");
            var password = Console.ReadLine();
            Console.Write($"Role [{existente.Role}]: ");
            var role = Console.ReadLine();

            return new User
            {
                Id = existente.Id,
                Username = string.IsNullOrWhiteSpace(username) ? existente.Username : username,
                Password = string.IsNullOrWhiteSpace(password) ? existente.Password : password,
                Role = string.IsNullOrWhiteSpace(role) ? existente.Role : role
            };
        }

        // Confirmação genérica ----------------------------------------------
        public bool Confirmar(string mensagem)
        {
            Console.Write($"{mensagem} (S/N) ");
            return Console.ReadKey(true).Key == ConsoleKey.S;
        }

        public void Sucesso(string mensagem)
        {
            Console.WriteLine(mensagem);
            Console.WriteLine("<Enter> para continuar…");
            Console.ReadLine();
        }

        public void IdInexistente()
        {
            Console.WriteLine("Id inexistente.");
            Console.WriteLine("<Enter> para continuar…");
            Console.ReadLine();
        }
    }
}
