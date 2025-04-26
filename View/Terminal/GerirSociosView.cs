using System;
using GestorGinasio.Model.Services;

namespace GestorGinasio.View.Terminal
{
    public class SociosView
    {
        private readonly string _user, _role;
        private readonly SocioService _service;

        public SociosView(string username, string role, SocioService service)
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
            Console.WriteLine("=== SÓCIOS ===");
            foreach (var s in _service.GetAll())
                Console.WriteLine($"{s.Id}: {s.Nome} ({s.Email})");
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
