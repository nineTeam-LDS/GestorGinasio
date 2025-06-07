// File: Controller/AulaController.cs
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
        private readonly IErrorHandler _errorHandler;

        // DI-ready: recebe apenas a interface
        public AulaController(IAulaService service, IAulaView view, IErrorHandler errorHandler)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _errorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
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
                            ExecutarComTratamento(() =>
                            {
                                var todas = _service.GetAll();
                                _view.MostrarLista(todas);
                            });
                            break;

                        case ConsoleKey.D2:
                            ExecutarComTratamento(() =>
                            {
                                var nova = _view.PedirNovaAula();

                                // Calcula próximo ID
                                var maxId = _service.GetAll()
                                                   .DefaultIfEmpty()
                                                   .Max(a => a?.Id ?? 0);
                                nova.Id = maxId + 1;

                                _service.Add(nova);
                                _view.Sucesso("Aula criada!");
                            });
                            break;

                        case ConsoleKey.D3:
                            ExecutarComTratamento(() =>
                            {
                                var idE = _view.PedirIdParaEditar();
                                var existente = _service.GetAll().FirstOrDefault(a => a.Id == idE);
                                if (existente == null)
                                {
                                    _view.Avaliar("Id não existe.");
                                    return;
                                }

                                var edit = _view.PedirDadosEditados(existente);
                                _service.Update(edit);
                                _view.Sucesso("Aula atualizada!");
                            });
                            break;

                        case ConsoleKey.D4:
                            ExecutarComTratamento(() =>
                            {
                                var idR = _view.PedirIdParaRemover();
                                if (!_service.GetAll().Any(a => a.Id == idR))
                                {
                                    _view.Avaliar("Id não existe.");
                                    return;
                                }

                                if (_view.Confirmar("Remover definitivamente?"))
                                {
                                    _service.Delete(idR);
                                    _view.Sucesso("Aula removida.");
                                }
                            });
                            break;

                        case ConsoleKey.D0:
                        case ConsoleKey.Escape:
                            return;
                    }
                }
            }
            catch (JsonFileFormatException ex)
            {
                _errorHandler.ShowError("ERRO AO CARREGAR AULAS: JSON inválido. Corrija o ficheiro e reinicie.");
                _errorHandler.LogError(ex, "AulaController.Gerir");
                Environment.Exit(1);
            }
            catch (RepositoryException ex)
            {
                _errorHandler.ShowError("ERRO DE I/O: não foi possível aceder aos dados de aulas.");
                _errorHandler.LogError(ex, "AulaController.Gerir");
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError("ERRO INESPERADO: " + ex.Message);
                _errorHandler.LogError(ex, "AulaController.Gerir");
                Environment.Exit(1);
            }
        }

        private void ExecutarComTratamento(Action acao)
        {
            try
            {
                acao();
            }
            catch (BusinessException bex)
            {
                _errorHandler.ShowError("Falha de negócio: " + bex.Message);
                _errorHandler.LogError(bex, nameof(ExecutarComTratamento));
            }
            catch (JsonFileFormatException jex)
            {
                _errorHandler.ShowError("JSON de aulas corrompido.");
                _errorHandler.LogError(jex, nameof(ExecutarComTratamento));
            }
            catch (RepositoryException rex)
            {
                _errorHandler.ShowError("Falha ao aceder ao armazenamento de aulas.");
                _errorHandler.LogError(rex, nameof(ExecutarComTratamento));
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError("Erro interno: " + ex.Message);
                _errorHandler.LogError(ex, nameof(ExecutarComTratamento));
            }
        }
    }
}
