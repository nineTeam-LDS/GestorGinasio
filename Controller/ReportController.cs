// File: Controller/ReportController.cs
using System;
using GestorGinasio.Model.Exceptions;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.Controller
{
    public class ReportController : IReportController
    {
        private readonly ISocioService _socioSvc;
        private readonly IAulaService _aulaSvc;
        private readonly IEquipamentoService _equipSvc;
        private readonly IReportService _reportSvc;
        private readonly IReportView _view;
        private readonly IErrorHandler _errorHandler;

        public ReportController(
            ISocioService socioSvc,
            IAulaService aulaSvc,
            IEquipamentoService equipSvc,
            IReportService reportSvc,
            IReportView view,
            IErrorHandler errorHandler)
        {
            _socioSvc = socioSvc ?? throw new ArgumentNullException(nameof(socioSvc));
            _aulaSvc = aulaSvc ?? throw new ArgumentNullException(nameof(aulaSvc));
            _equipSvc = equipSvc ?? throw new ArgumentNullException(nameof(equipSvc));
            _reportSvc = reportSvc ?? throw new ArgumentNullException(nameof(reportSvc));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _errorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }


        public void Gerir(string currentUser)
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
                                var lista = _socioSvc.GetAll();
                                var path = _reportSvc.GenerateSociosReport(lista, currentUser);
                                Console.WriteLine($"\nPDF gerado em:\n{path}");
                            });
                            break;

                        case ConsoleKey.D2:
                            ExecutarComTratamento(() =>
                            {
                                var lista = _aulaSvc.GetAll();
                                var path = _reportSvc.GenerateAulasReport(lista, currentUser);
                                Console.WriteLine($"\nPDF gerado em:\n{path}");
                            });
                            break;

                        case ConsoleKey.D3:
                            ExecutarComTratamento(() =>
                            {
                                var lista = _equipSvc.GetAll();
                                var path = _reportSvc.GenerateEquipamentosReport(lista, currentUser);
                                Console.WriteLine($"\nPDF gerado em:\n{path}");
                            });
                            break;

                        case ConsoleKey.D0:
                        case ConsoleKey.Escape:
                            return;
                    }

                    Console.WriteLine("\n<Enter> para voltar…");
                    Console.ReadLine();
                }
            }
            catch (JsonFileFormatException ex)
            {
                // erro no JSON dos dados (sócios/aulas/equipamentos)
                _errorHandler.ShowError($"Erro de formato JSON: {ex.Message}\nO programa será encerrado.");
                _errorHandler.LogError(ex, nameof(Gerir));
                Environment.Exit(1);
            }
            catch (RepositoryException ex)
            {
                // erro I/O ao ler os dados de domínio
                _errorHandler.ShowError($"Erro de I/O: {ex.Message}\nO programa será encerrado.");
                _errorHandler.LogError(ex, nameof(Gerir));
                Environment.Exit(1);
            }
            catch (PdfGenerationException ex)
            {
                // algo falhou em PDFsharp ou I/O ao gravar o PDF
                _errorHandler.ShowError($"Erro ao gerar PDF: {ex.Message}");
                _errorHandler.LogError(ex, nameof(Gerir));
                // aqui não Salvamos Environment.Exit, pois o usuário pode tentar outro relatório
            }
            catch (Exception ex)
            {
                // qualquer outro erro inesperado
                _errorHandler.ShowError($"Erro inesperado: {ex.Message}\nO programa será encerrado.");
                _errorHandler.LogError(ex, nameof(Gerir));
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
                // Erros de regra de negócio (por ex., lista vazia? mas neste caso raramente se aplica)
                _errorHandler.ShowError("Falha de negócio: " + bex.Message);
                _errorHandler.LogError(bex, nameof(ExecutarComTratamento));
            }
            catch (JsonFileFormatException jex)
            {
                // JSON corrompido no domínio específico
                _errorHandler.ShowError("Erro de formato JSON ao ler dados de domínio.");
                _errorHandler.LogError(jex, nameof(ExecutarComTratamento));
            }
            catch (RepositoryException rex)
            {
                // I/O ao ler dados de domínio
                _errorHandler.ShowError("Erro de I/O ao aceder aos dados de domínio.");
                _errorHandler.LogError(rex, nameof(ExecutarComTratamento));
            }
            catch (PdfGenerationException pex)
            {
                // Erro ao gerar o PDF
                _errorHandler.ShowError("Falha ao gerar PDF: " + pex.Message);
                _errorHandler.LogError(pex, nameof(ExecutarComTratamento));
            }
            catch (Exception ex)
            {
                // Qualquer outro erro
                _errorHandler.ShowError("Erro interno: " + ex.Message);
                _errorHandler.LogError(ex, nameof(ExecutarComTratamento));
            }
        }
    }
}

/*
1. O try/catch exterior em Gerir captura:

    a. JsonFileFormatException – JSON corrompido nos ficheiros de “Sócios/Aulas/Equipamentos”: fatal, o programa sai.

    b. RepositoryException – falha de I/O ao ler os ficheiros (ex.: sem permissões, disco lotado): fatal, sai.

    c. PdfGenerationException – falha na própria geração ou gravação do PDF (ex.: PDFSharp lança PdfSharpException, disco cheio, etc.). 
       Neste caso, não saímos, mas mostramos mensagem vermelha e deixamos voltar ao menu.

    d. Exception genérico – erro inesperado, sai do programa.

2. Cada opção (D1, D2, D3) vai chamar ExecutarComTratamento(() => { … }). Dentro desse método:

    a. BusinessException (alguma regra de negócio no ReportService ou nos serviços de domínio) → mostramos falha de negócio.

    b. JsonFileFormatException ou RepositoryException → mensagem apropriada, mas sem mandar sair (já que pode ser um problema temporário de I/O).

    c. PdfGenerationException → mensagem vermelha de “Falha ao gerar PDF”.

    d. Qualquer outro “Exception” → mensagem interna.

3. Quando tudo corre bem, mostramos ao utilizador a mensagem com o caminho do PDF (ex.: Console.WriteLine($"\nPDF gerado em:\n{path}")) 
   e depois aguardamos <Enter> para voltar….
 */
