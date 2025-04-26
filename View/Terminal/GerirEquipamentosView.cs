using System;
using GestorGinasio.Model.Services;

namespace GestorGinasio.View.Terminal
{
    public class EquipamentosView
    {
        private readonly string _user, _role;
        private readonly EquipamentoService _service;

        public EquipamentosView(string username, string role, EquipamentoService service)
        {
            _user = username;
            _role = role;
            _service = service;
        }

        public void Exibir()
        {
            Console.Clear();
            DrawHeader();
            Console.WriteLine();
            Console.WriteLine("=== EQUIPAMENTOS ===");
            foreach (var e in _service.GetAll())
                Console.WriteLine($"{e.Id}: {e.Nome} (Qtd: {e.Quantidade})");
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
        }

        private void DrawHeader()
        {
            int w = Console.WindowWidth;
            Console.WriteLine(new string('=', w));
            Console.Write(DateTime.Now.ToString("dddd HH:mm:ss"));
            string lbl = _role.Equals("Admin", StringComparison.OrdinalIgnoreCase)
                ? "Administrador: " : "Utilizador: ";
            string usr = lbl + _user;
            Console.SetCursorPosition(w - usr.Length, Console.CursorTop);
            Console.WriteLine(usr);
            Console.WriteLine(new string('=', w));
        }
    }
}
