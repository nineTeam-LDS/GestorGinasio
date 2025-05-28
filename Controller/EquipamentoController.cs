using System;
using System.Linq;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    public class EquipamentoController : IEquipamentoController
    {
        private readonly IEquipamentoService _service;
        private readonly IEquipamentoView _view;

        public EquipamentoController(IEquipamentoService service, IEquipamentoView view)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Gerir()
        {
            while (true)
            {
                var key = _view.MostrarMenu();
                switch (key)
                {
                    case ConsoleKey.D1:
                        _view.MostrarLista(_service.GetAll());
                        break;
                    case ConsoleKey.D2:
                        var novo = _view.PedirNovoEquipamento();
                        novo.Id = _service.GetAll().DefaultIfEmpty().Max(e => e?.Id ?? 0) + 1;
                        _service.Add(novo);
                        _view.Sucesso("Equipamento adicionado!");
                        break;
                    case ConsoleKey.D3:
                        var idE = _view.PedirIdParaEditar();
                        var orig = _service.GetAll().FirstOrDefault(e => e.Id == idE);
                        if (orig == null) { _view.IdInexistente(); break; }
                        var edit = _view.PedirDadosEditados(orig);
                        _service.Update(edit);
                        _view.Sucesso("Equipamento atualizado!");
                        break;
                    case ConsoleKey.D4:
                        var idR = _view.PedirIdParaRemover();
                        if (idR < 0) break;
                        if (_view.Confirmar($"Remover equipamento {idR}?"))
                        {
                            _service.Delete(idR);
                            _view.Sucesso("Equipamento removido!");
                        }
                        break;
                    case ConsoleKey.D0:
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}

