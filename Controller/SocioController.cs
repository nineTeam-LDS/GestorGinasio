using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    internal class SocioController
    {
        private readonly SocioService _srv = new();

        /* MENU PRINCIPAL DOS SÓCIOS */
        public void Gerir()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SÓCIOS ===");
                Console.WriteLine("1. Listar");
                Console.WriteLine("2. Criar");
                Console.WriteLine("3. Editar");
                Console.WriteLine("4. Remover");
                Console.WriteLine("0. Voltar");
                Console.Write("Opção: ");
                var op = Console.ReadKey(true).Key;

                switch (op)
                {
                    case ConsoleKey.D1: Listar(); break;
                    case ConsoleKey.D2: Criar(); break;
                    case ConsoleKey.D3: Editar(); break;
                    case ConsoleKey.D4: Remover(); break;
                    case ConsoleKey.D0: return;
                }
            }
        }

        /* ---------- CRUD ----------- */

        private void Listar()
            => GerirSociosView.MostrarLista(_srv.GetAll());

        private void Criar()
        {
            var novo = GerirSociosView.PedirNovoSocio();
            novo.Id = _srv.GetAll().DefaultIfEmpty()
                          .Max(u => u?.Id ?? 0) + 1;       // gera Id
            _srv.Add(novo);

            GerirSociosView.Sucesso("Sócio criado!");
        }

        private void Editar()
        {
            var id = GerirSociosView.PedirIdParaEditar();
            var src = _srv.GetAll().FirstOrDefault(s => s.Id == id);

            if (src == null) { GerirSociosView.IdInexistente(); return; }

            var dst = GerirSociosView.PedirDadosEditados(src);
            _srv.Update(dst);
            GerirSociosView.Sucesso("Sócio actualizado!");
        }

        private void Remover()
        {
            var id = GerirSociosView.PedirIdParaRemover();
            if (!_srv.GetAll().Any(s => s.Id == id)) { GerirSociosView.IdInexistente(); return; }

            if (GerirSociosView.Confirmar("Remover definitivamente?"))
            {
                _srv.Delete(id);
                GerirSociosView.Sucesso("Sócio removido.");
            }
        }
    }

}