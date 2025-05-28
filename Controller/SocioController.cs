using System;
using System.Linq;
using GestorGinasio.Model.Exceptions;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    public class SocioController : ISocioController
    {
        private readonly ISocioService _service;
        private readonly ISociosView _view;

        public SocioController(ISocioService service, ISociosView view)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Gerir()
        {
            try
            {
                while (true)
                {
                    var op = _view.MostrarMenu();
                    switch (op)
                    {
                        case ConsoleKey.D1:
                            _view.MostrarLista(_service.GetAll());
                            break;
                        case ConsoleKey.D2:
                            var novo = _view.PedirNovoSocio();
                            novo.Id = _service.GetAll().DefaultIfEmpty().Max(s => s?.Id ?? 0) + 1;
                            _service.Add(novo);
                            _view.Sucesso("Sócio criado!");
                            break;
                        case ConsoleKey.D3:
                            var idE = _view.PedirIdParaEditar();
                            var orig = _service.GetAll().FirstOrDefault(s => s.Id == idE);
                            if (orig == null) { _view.IdInexistente(); break; }
                            var edit = _view.PedirDadosEditados(orig);
                            _service.Update(edit);
                            _view.Sucesso("Sócio atualizado!");
                            break;
                        case ConsoleKey.D4:
                            var idR = _view.PedirIdParaRemover();
                            if (!_service.GetAll().Any(s => s.Id == idR))
                            { _view.IdInexistente(); break; }
                            if (_view.Confirmar("Remover definitivamente?"))
                            {
                                _service.Delete(idR);
                                _view.Sucesso("Sócio removido.");
                            }
                            break;
                        case ConsoleKey.D0:
                        case ConsoleKey.Escape:
                            return;
                    }
                }
            }
            catch (JsonFileFormatException ex)
            {
                Console.WriteLine("Erro ao carregar dados.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Corrija o JSON e reinicie.");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
    }
}