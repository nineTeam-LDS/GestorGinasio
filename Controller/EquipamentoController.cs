// File: Controller/EquipamentoController.cs
using System;
using System.Linq;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;
using GestorGinasio.Model.Exceptions;

namespace GestorGinasio.Controller
{
    public class EquipamentoController : IEquipamentoController
    {
        private readonly IEquipamentoService _service;
        private readonly IEquipamentoView _view;
        private readonly IErrorHandler _errorHandler;

        public EquipamentoController(IEquipamentoService service, IEquipamentoView view, IErrorHandler errorHandler)
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
                    var key = _view.MostrarMenu();
                    switch (key)
                    {
                        case ConsoleKey.D1:
                            ExecutarComTratamento(() =>
                            {
                                var todos = _service.GetAll();
                                _view.MostrarLista(todos);
                            });
                            break;

                        case ConsoleKey.D2:
                            ExecutarComTratamento(() =>
                            {
                                var novo = _view.PedirNovoEquipamento();

                                // calcular próximo ID
                                var maxId = _service.GetAll()
                                                   .DefaultIfEmpty()
                                                   .Max(x => x?.Id ?? 0);
                                novo.Id = maxId + 1;

                                _service.Add(novo);
                                _view.Sucesso("Equipamento adicionado com sucesso!");
                            });
                            break;

                        case ConsoleKey.D3:
                            ExecutarComTratamento(() =>
                            {
                                var idE = _view.PedirIdParaEditar();
                                var orig = _service.GetAll().FirstOrDefault(x => x.Id == idE);

                                if (orig == null)
                                {
                                    _view.Avaliar("ID inexistente");
                                    return;
                                }

                                var edit = _view.PedirDadosEditados(orig);
                                _service.Update(edit);
                                _view.Sucesso("Equipamento atualizado com sucesso!");
                            });
                            break;

                        case ConsoleKey.D4:
                            ExecutarComTratamento(() =>
                            {
                                var idR = _view.PedirIdParaRemover();
                                if (!_service.GetAll().Any(x => x.Id == idR))
                                {
                                    _view.Avaliar("ID inexistente");
                                    return;
                                }

                                if (_view.Confirmar($"Confirma remover equipamento {idR}?"))
                                {
                                    _service.Delete(idR);
                                    _view.Sucesso("Equipamento removido com sucesso!");
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
                _errorHandler.ShowError("ERRO AO CARREGAR EQUIPAMENTOS: JSON inválido. Corrija o ficheiro e reinicie.");
                _errorHandler.LogError(ex, "EquipamentoController.Gerir");
                Environment.Exit(1);
            }
            catch (IOException ex)
            {
                _errorHandler.ShowError("ERRO DE I/O: não foi possível aceder aos dados de equipamentos.");
                _errorHandler.LogError(ex, "EquipamentoController.Gerir");
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError("ERRO INESPERADO: " + ex.Message);
                _errorHandler.LogError(ex, "EquipamentoController.Gerir");
                Environment.Exit(1);
            }
        }

        // Helper para executar ações com catch de exceções granulares.
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
                _errorHandler.ShowError("JSON de equipamentos corrompido.");
                _errorHandler.LogError(jex, nameof(ExecutarComTratamento));
            }
            catch (RepositoryException rex)
            {
                _errorHandler.ShowError("Falha ao aceder ao armazenamento de equipamentos.");
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

