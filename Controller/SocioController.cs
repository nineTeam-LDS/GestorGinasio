// File: Controller/SocioController.cs
using System;
using System.Linq;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Exceptions;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    public class SocioController : ISocioController
    {
        private readonly ISocioService _service;
        private readonly ISociosView _view;
        private readonly IErrorHandler _errorHandler;

        public SocioController(
            ISocioService service,
            ISociosView view,
            IErrorHandler errorHandler)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _errorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        public void Gerir()
        {
            // Se houver falha no JSON (formato, encoding, I/O), sa�mos j� da aplica��o.
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
                                var todos = _service.GetAll();
                                _view.MostrarLista(todos);
                            });
                            break;

                        case ConsoleKey.D2:
                            ExecutarComTratamento(() =>
                            {
                                var novo = _view.PedirNovoSocio();

                                // VALIDAR DUPLICADO DE EMAIL NO SERVI�O (OPCIONAL):
                                if (_service.GetAll().Any(s => s.Email.Equals(novo.Email, StringComparison.OrdinalIgnoreCase)))
                                {
                                    throw new BusinessException("J� existe um s�cio com esse email.");
                                }

                                // CALCULAR NEXT ID:
                                var maxId = _service.GetAll()
                                                  .DefaultIfEmpty()
                                                  .Max(s => s?.Id ?? 0);
                                novo.Id = maxId + 1;

                                _service.Add(novo);
                                _view.Sucesso("S�cio criado com sucesso!");
                            });
                            break;

                        case ConsoleKey.D3:
                            ExecutarComTratamento(() =>
                            {
                                var idE = _view.PedirIdParaEditar();
                                var orig = _service.GetAll()
                                                   .FirstOrDefault(s => s.Id == idE);

                                if (orig == null)
                                {
                                    _view.Avaliar("ID inexistente");
                                    return;
                                }

                                var edit = _view.PedirDadosEditados(orig);

                                // validar aqui, por ex: email duplicado ou nome vazio ------------------------------- TO DO: validar email duplicado
                                if (string.IsNullOrWhiteSpace(edit.Nome))
                                    throw new BusinessException("Nome do s�cio n�o pode ficar em branco.");

                                _service.Update(edit);
                                _view.Sucesso("S�cio atualizado com sucesso!");
                            });
                            break;

                        case ConsoleKey.D4:
                            ExecutarComTratamento(() =>
                            {
                                var idR = _view.PedirIdParaRemover();
                                var existe = _service.GetAll().Any(s => s.Id == idR);
                                if (!existe)
                                {
                                    _view.Avaliar("ID inexistente");
                                    return;
                                }

                                if (_view.Confirmar("Remover definitivamente?"))
                                {
                                    _service.Delete(idR);
                                    _view.Sucesso("S�cio removido com sucesso!");
                                }
                            });
                            break;

                        case ConsoleKey.D0:
                        case ConsoleKey.Escape:
                            return;   // sair do menu S�cios

                        default:
                            // tecla n�o reconhecida, simplesmente repete o menu
                            break;
                    }
                }
            }
            catch (JsonFileFormatException ex)
            {
                // Erro fatal: dados de s�cios corrompidos / formato JSON inv�lido
                _errorHandler.ShowError("ERRO AO CARREGAR S�CIOS: JSON inv�lido. Corrija o ficheiro e reinicie.");
                _errorHandler.LogError(ex, "SocioController.Gerir");
                Environment.Exit(1);
            }
            catch (IOException ex)
            {
                // Falha de I/O geral (por ex. ficheiro bloqueado, sem permiss�o)
                _errorHandler.ShowError("ERRO DE I/O: n�o foi poss�vel aceder aos dados de s�cios.");
                _errorHandler.LogError(ex, "SocioController.Gerir");
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                // Catch �� espera� para qualquer outra falha imprevista
                _errorHandler.ShowError("ERRO INESPERADO: " + ex.Message);
                _errorHandler.LogError(ex, "SocioController.Gerir");
                Environment.Exit(1);
            }
        }

        // Executa uma a��o (normalmente que invoque o Service) com
        // tratamento local de exce��es granulares: Business, Repository, etc.
        private void ExecutarComTratamento(Action acao)
        {
            try
            {
                acao();
            }
            catch (BusinessException bex)
            {
                // ex.: �email duplicado� ou regra de neg�cio violada
                _errorHandler.ShowError("Falha de neg�cio: " + bex.Message);
                _errorHandler.LogError(bex, nameof(ExecutarComTratamento));
            }
            catch (JsonFileFormatException jex)
            {
                // Podia "rethrow" e ser apanhado no m�todo Gerir()
                _errorHandler.ShowError("JSON dos s�cios est� corrompido.");
                _errorHandler.LogError(jex, nameof(ExecutarComTratamento));
            }
            catch (RepositoryException rex)
            {
                _errorHandler.ShowError("N�o foi poss�vel aceder ao armazenamento de s�cios.");
                _errorHandler.LogError(rex, nameof(ExecutarComTratamento));
            }
            catch (Exception ex)
            {
                // Qualquer outro erro inesperado (n�o de neg�cio, nem JSON, nem I/O)
                _errorHandler.ShowError("Erro interno: " + ex.Message);
                _errorHandler.LogError(ex, nameof(ExecutarComTratamento));
            }
        }
    }
}
