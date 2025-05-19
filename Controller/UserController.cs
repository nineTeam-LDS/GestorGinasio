using System;
using System.Linq;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    // Controller para gerir utilizadores, via DI de serviços e view.
    public class UserController : IUserController
    {
        private readonly IUserService _service;
        private readonly IUserView _view;

        public UserController(IUserService service, IUserView view)
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
                        var novo = _view.PedirNovoUtilizador();
                        novo.Id = _service.GetAll().DefaultIfEmpty().Max(u => u?.Id ?? 0) + 1;
                        _service.Add(novo);
                        _view.Sucesso("Utilizador criado!");
                        break;

                    case ConsoleKey.D3:
                        var idE = _view.PedirIdParaEditar();
                        var orig = _service.GetAll().FirstOrDefault(u => u.Id == idE);
                        if (orig == null)
                        {
                            _view.IdInexistente();
                            break;
                        }
                        var edit = _view.PedirDadosEditados(orig);
                        _service.Update(edit);
                        _view.Sucesso("Utilizador atualizado!");
                        break;

                    case ConsoleKey.D4:
                        var idR = _view.PedirIdParaRemover();
                        if (idR < 0) break;
                        if (_view.Confirmar($"Confirma remover Id {idR}??"))
                        {
                            _service.Delete(idR);
                            _view.Sucesso("Utilizador removido!");
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

