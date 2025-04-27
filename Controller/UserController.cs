using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    internal class UserController
    {
        private readonly UserService _svc = new();

        /* MENU PRINCIPAL DOS UTILIZADORES */
        public void Gerir()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n===== UTILIZADORES =====\n");
                Console.WriteLine("1. Listar");
                Console.WriteLine("2. Adicionar");
                Console.WriteLine("3. Editar");
                Console.WriteLine("4. Remover");
                Console.WriteLine("0. Voltar");
                Console.Write("Opção: ");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:         // ---- LISTAR ----
                        GerirUtilizadoresView.MostrarLista(_svc.GetAll());
                        break;

                    case ConsoleKey.D2:         // ---- CRIAR ----
                        var novo = GerirUtilizadoresView.PedirNovoUtilizador();
                        novo.Id = _svc.GetAll().DefaultIfEmpty()
                                       .Max(u => u?.Id ?? 0) + 1;
                        _svc.Add(novo);
                        break;

                    case ConsoleKey.D3:         // ---- EDITAR ----
                        EditarUtilizador();
                        break;

                    case ConsoleKey.D4:         // ---- REMOVER ----
                        RemoverUtilizador();
                        break;

                    case ConsoleKey.D0:
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        // ---------- Métodos auxiliares -------------------------------
        private void EditarUtilizador()
        {
            var id = GerirUtilizadoresView.PedirIdParaEditar();
            var original = _svc.GetAll().FirstOrDefault(u => u.Id == id);

            if (original is null)
            {
                Console.WriteLine("Id inexistente. <Enter>");
                Console.ReadLine();
                return;
            }

            var editado = GerirUtilizadoresView.PedirDadosEditados(original);
            _svc.Update(editado);
        }

        private void RemoverUtilizador()
        {
            var id = GerirUtilizadoresView.PedirIdParaRemover();
            if (id < 0) return;

            if (GerirUtilizadoresView.Confirmar($"Confirma remover Id {id}?"))
                _svc.Delete(id);
        }
    }
}

