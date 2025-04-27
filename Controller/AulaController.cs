using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    internal class AulaController
    {
        private readonly AulaService _srv = new();
        public void Gerir()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n===== AULAS =====\n");
                Console.WriteLine("1. Listar");
                Console.WriteLine("2. Criar");
                Console.WriteLine("3. Editar");
                Console.WriteLine("4. Remover");
                Console.WriteLine("0. Voltar");
                Console.Write("Opção: ");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1: Listar(); break;
                    case ConsoleKey.D2: Criar(); break;
                    case ConsoleKey.D3: Editar(); break;
                    case ConsoleKey.D4: Remover(); break;
                    case ConsoleKey.D0: return;
                }
            }
        }

        /* CRUD */
        void Listar() => GerirAulasView.MostrarLista(_srv.GetAll());

        void Criar()
        {
            var novo = GerirAulasView.PedirNovaAula();
            novo.Id = _srv.GetAll().DefaultIfEmpty()
                           .Max(u => u?.Id ?? 0) + 1;
            _srv.Add(novo);
            GerirAulasView.Sucesso("Aula criada!");
        }

        void Editar()
        {
            var id = GerirAulasView.PedirIdParaEditar();
            var src = _srv.GetAll().FirstOrDefault(a => a.Id == id);
            if (src == null) { GerirAulasView.IdInexistente(); return; }

            var dst = GerirAulasView.PedirDadosEditados(src);
            _srv.Update(dst);
            GerirAulasView.Sucesso("Aula actualizada!");
        }

        void Remover()
        {
            var id = GerirAulasView.PedirIdParaRemover();
            if (!_srv.GetAll().Any(a => a.Id == id)) { GerirAulasView.IdInexistente(); return; }

            if (GerirAulasView.Confirmar("Remover definitivamente?"))
            { _srv.Delete(id); GerirAulasView.Sucesso("Aula removida."); }
        }
    }
}
