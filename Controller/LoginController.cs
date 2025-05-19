using System;
using System.Threading;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Services;
using GestorGinasio.View.Terminal;

namespace GestorGinasio.Controller
{
    public class LoginController : ILoginController
    {
        private readonly ILoginView _loginView;
        private readonly IAuthService _authService;
        private readonly IMenuPrincipalController _menuCtrl;

        public LoginController(
            ILoginView loginView,
            IAuthService authService,
            IMenuPrincipalController menuCtrl)
        {
            _loginView = loginView ?? throw new ArgumentNullException(nameof(loginView));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _menuCtrl = menuCtrl ?? throw new ArgumentNullException(nameof(menuCtrl));
        }

        public void RealizarLogin()
        {
            while (true)
            {
                User utilizador = _loginView.SolicitarCredenciais();
                bool valido = _authService.ValidarCredenciais(utilizador);
                _loginView.MostrarResultadoLogin(valido);

                if (!valido)
                {
                    Console.WriteLine("Prima Enter para tentar novamente…");
                    Console.ReadLine();
                    continue;
                }

                Console.WriteLine("A iniciar aplicação...");
                Thread.Sleep(1000);
                Console.Clear();

                _menuCtrl.MostrarMenu(utilizador.Username, utilizador.Role);
                break;
            }
        }
    }
}
