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
            // Se houver falha no JSON (formato, encoding, I/O), saímos já da aplicação.
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

                                // VALIDAR DUPLICADO DE EMAIL NO SERVIÇO (OPCIONAL):
                                if (_service.GetAll().Any(s => s.Email.Equals(novo.Email, StringComparison.OrdinalIgnoreCase)))
                                {
                                    throw new BusinessException("Já existe um sócio com esse email.");
                                }

                                // CALCULAR NEXT ID:
                                var maxId = _service.GetAll()
                                                  .DefaultIfEmpty()
                                                  .Max(s => s?.Id ?? 0);
                                novo.Id = maxId + 1;

                                _service.Add(novo);
                                _view.Sucesso("Sócio criado com sucesso!");
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
                                    throw new BusinessException("Nome do sócio não pode ficar em branco.");

                                _service.Update(edit);
                                _view.Sucesso("Sócio atualizado com sucesso!");
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
                                    _view.Sucesso("Sócio removido com sucesso!");
                                }
                            });
                            break;

                        case ConsoleKey.D0:
                        case ConsoleKey.Escape:
                            return;   // sair do menu Sócios

                        default:
                            // tecla não reconhecida, simplesmente repete o menu
                            break;
                    }
                }
            }
            catch (JsonFileFormatException ex)
            {
                // Erro fatal: dados de sócios corrompidos / formato JSON inválido
                _errorHandler.ShowError("ERRO AO CARREGAR SÓCIOS: JSON inválido. Corrija o ficheiro e reinicie.");
                _errorHandler.LogError(ex, "SocioController.Gerir");
                Environment.Exit(1);
            }
            catch (IOException ex)
            {
                // Falha de I/O geral (por ex. ficheiro bloqueado, sem permissão)
                _errorHandler.ShowError("ERRO DE I/O: não foi possível aceder aos dados de sócios.");
                _errorHandler.LogError(ex, "SocioController.Gerir");
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                // Catch “à espera” para qualquer outra falha imprevista
                _errorHandler.ShowError("ERRO INESPERADO: " + ex.Message);
                _errorHandler.LogError(ex, "SocioController.Gerir");
                Environment.Exit(1);
            }
        }

        // Executa uma ação (normalmente que invoque o Service) com
        // tratamento local de exceções granulares: Business, Repository, etc.
        private void ExecutarComTratamento(Action acao)
        {
            try
            {
                acao();
            }
            catch (BusinessException bex)
            {
                // ex.: “email duplicado” ou regra de negócio violada
                _errorHandler.ShowError("Falha de negócio: " + bex.Message);
                _errorHandler.LogError(bex, nameof(ExecutarComTratamento));
            }
            catch (JsonFileFormatException jex)
            {
                // Podia "rethrow" e ser apanhado no método Gerir()
                _errorHandler.ShowError("JSON dos sócios está corrompido.");
                _errorHandler.LogError(jex, nameof(ExecutarComTratamento));
            }
            catch (RepositoryException rex)
            {
                _errorHandler.ShowError("Não foi possível aceder ao armazenamento de sócios.");
                _errorHandler.LogError(rex, nameof(ExecutarComTratamento));
            }
            catch (Exception ex)
            {
                // Qualquer outro erro inesperado (não de negócio, nem JSON, nem I/O)
                _errorHandler.ShowError("Erro interno: " + ex.Message);
                _errorHandler.LogError(ex, nameof(ExecutarComTratamento));
            }
        }
    }
}
