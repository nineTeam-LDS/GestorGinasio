using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Exceptions;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    public class AulaController : IAulaController
    {
        private readonly IAulaService _service;
        private readonly IAulaView _view;

        // DI-ready: recebe apenas a interface
        public AulaController(IAulaService service, IAulaView view)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Gerir()
        {
            while (true)
            {
                var op = _view.MostrarMenu();
                switch (op)
                {
                    case ConsoleKey.D1: _view.MostrarLista(_service.GetAll()); break;
                    case ConsoleKey.D2:
                        var nova = _view.PedirNovaAula();
                        nova.Id = _service.GetAll().DefaultIfEmpty().Max(a => a?.Id ?? 0) + 1;
                        _service.Add(nova);
                        _view.Sucesso("Aula criada!");
                        break;
                    case ConsoleKey.D3:
                        var idE = _view.PedirIdParaEditar();
                        var ex = _service.GetAll().FirstOrDefault(a => a.Id == idE);
                        if (ex == null) { _view.Aviso("Id não existe."); break; }
                        var edit = _view.PedirDadosEditados(ex);
                        _service.Update(edit);
                        _view.Sucesso("Aula atualizada!");
                        break;
                    case ConsoleKey.D4:
                        var idR = _view.PedirIdParaRemover();
                        if (!_service.GetAll().Any(a => a.Id == idR))
                        { _view.Aviso("Id não existe."); break; }
                        if (_view.Confirmar("Remover definitivamente?"))
                        {
                            _service.Delete(idR);
                            _view.Sucesso("Aula removida.");
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
