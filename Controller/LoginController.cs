// File: Controller/LoginController.cs
using System;
using System.Threading;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;
using GestorGinasio.Model.Exceptions;

namespace GestorGinasio.Controller
{
    public class LoginController : ILoginController
    {
        private readonly ILoginView _loginView;
        private readonly IAuthService _authService;
        private readonly IMenuPrincipalController _menuCtrl;
        private readonly IErrorHandler _errorHandler;

        public LoginController(
            ILoginView loginView,
            IAuthService authService,
            IMenuPrincipalController menuCtrl,
            IErrorHandler errorHandler)
        {
            _loginView = loginView ?? throw new ArgumentNullException(nameof(loginView));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _menuCtrl = menuCtrl ?? throw new ArgumentNullException(nameof(menuCtrl));
            _errorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        public void RealizarLogin()
        {
            try
            {
                while (true)
                {
                    // 1) View já só devolve User com username+password não vazios
                    User utilizador = _loginView.SolicitarCredenciais();

                    // 2) Valida credenciais: pode lançar JsonFileFormatException ou RepositoryException
                    bool valido;
                    try
                    {
                        valido = _authService.ValidarCredenciais(utilizador);
                    }
                    catch (JsonFileFormatException jex)
                    {
                        // JSON de users corrompido → erro fatal
                        _errorHandler.ShowError("ERRO: o ficheiro de utilizadores está corrompido. Corrija e reinicie.");
                        _errorHandler.LogError(jex, nameof(RealizarLogin));
                        Environment.Exit(1);
                        return;
                    }
                    catch (RepositoryException rex)
                    {
                        // Problema de I/O ao ler users.json → erro fatal
                        _errorHandler.ShowError("ERRO DE I/O ao aceder aos utilizadores. Verifique permissões.");
                        _errorHandler.LogError(rex, nameof(RealizarLogin));
                        Environment.Exit(1);
                        return;
                    }

                    // 3) Mostrar resultado
                    _loginView.MostrarResultadoLogin(valido);

                    if (!valido)
                    {
                        // credenciais inválidas, volta a pedir
                        Console.WriteLine("Prima Enter para tentar novamente…");
                        Console.ReadLine();
                        continue;
                    }

                    // 4) Login OK: avança para o menu principal
                    Console.WriteLine("A iniciar aplicação...");
                    Thread.Sleep(1000);
                    Console.Clear();
                    _menuCtrl.MostrarMenu(utilizador.Username, utilizador.Role);
                    break;
                }
            }
            catch (BusinessException bex)
            {
                // Este catch é improvável, pois ValidarCredenciais devolve false em vez de lançar BusinessException,
                // mas se algum método vier a lançar BusinessException, capturamo-lo aqui.
                _errorHandler.ShowError("Falha de negócio: " + bex.Message);
                _errorHandler.LogError(bex, nameof(RealizarLogin));
                // Vamos voltar ao início
                Console.WriteLine("Prima Enter para recomeçar login…");
                Console.ReadLine();
                RealizarLogin();
            }
            catch (Exception ex)
            {
                // Qualquer outro erro imprevisto
                _errorHandler.ShowError("ERRO INESPERADO: " + ex.Message);
                _errorHandler.LogError(ex, nameof(RealizarLogin));
                Environment.Exit(1);
            }
        }
    }
}

/*
1. Injetamos IErrorHandler no construtor.

2. Envolvemos toda a lógica de login num try/catch externo (para apanhar BusinessException ou qualquer outro Exception).

3. Dentro do loop, chamamos _authService.ValidarCredenciais num bloco try/catch específico para distinguir:

   * JsonFileFormatException → “JSON de utilizadores corrompido” → erro fatal (Exit).

   * RepositoryException → “I/O ao aceder users.json” → erro fatal (Exit).

4. Se valido == false, continuo o loop (pede credenciais de novo).

5. Se aparecer um BusinessException em qualquer ponto (e.g. no futuro, se decidirmos lançar BusinessException em campos de login), 
capturamos no catch (BusinessException) e mostramos “Falha de negócio: …” e chamamos de novo RealizarLogin().

6. Qualquer outro Exception cai no catch (Exception) mais genérico, mostramos “Erro inesperado” e encerramos a aplicação.
 */ 