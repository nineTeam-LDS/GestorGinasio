using System;
using GestorGinasio.Model.Services;

namespace GestorGinasio.View.Terminal
{
    public class AulasView
    {
        private readonly string _user, _role;
        private readonly AulaService _service;

        public AulasView(string username, string role, AulaService service)
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
            Console.WriteLine("=== AULAS ===");
            foreach (var a in _service.GetAll())
                Console.WriteLine($"{a.Id}: {a.Nome} - {a.Instrutor} às {a.Horario}");
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
