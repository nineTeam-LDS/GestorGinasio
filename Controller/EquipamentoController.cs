using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    internal class EquipamentoController
    {
        private readonly EquipamentoService _svc = new();

        public void Gerir()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n===== EQUIPAMENTOS =====\n");
                Console.WriteLine("1. Listar");
                Console.WriteLine("2. Adicionar");
                Console.WriteLine("3. Editar");
                Console.WriteLine("4. Remover");
                Console.WriteLine("0. Voltar");
                Console.Write("Opção: ");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        GerirEquipamentosView.MostrarLista(_svc.GetAll());
                        break;

                    case ConsoleKey.D2:
                        var novo = GerirEquipamentosView.PedirNovoEquipamento();
                        novo.Id = _svc.GetAll().DefaultIfEmpty()
                                      .Max(e => e?.Id ?? 0) + 1;
                        _svc.Add(novo);
                        break;

                    case ConsoleKey.D3:
                        Editar();
                        break;

                    case ConsoleKey.D4:
                        Remover();
                        break;

                    case ConsoleKey.D0:
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        // ---------- Auxiliares ----------
        private void Editar()
        {
            var id = GerirEquipamentosView.PedirIdParaEditar();
            var orig = _svc.GetAll().FirstOrDefault(e => e.Id == id);
            if (orig == null) { GerirEquipamentosView.IdInexistente(); return; }

            var edit = GerirEquipamentosView.PedirDadosEditados(orig);
            _svc.Update(edit);
        }

        private void Remover()
        {
            var id = GerirEquipamentosView.PedirIdParaRemover();
            if (id < 0) return;

            if (GerirEquipamentosView.Confirmar($"Remover equipamento {id}?"))
                _svc.Delete(id);
        }
    }
}

