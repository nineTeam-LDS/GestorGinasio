using System;
using System.Threading;
using GestorGinasio.Controller;
using GestorGinasio.View.Terminal;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Services;

namespace GestorGinasio.Controller
{
    public class LoginController
    {
        private LoginView _loginView;
        private AuthService _authService;

        public LoginController(LoginView loginView, AuthService authService)
        {
            _loginView = loginView;
            _authService = authService;
        }

        public void RealizarLogin()
        {
            // Obtém o utilizador a partir do fluxo de login.
            User utilizador = _loginView.SolicitarCredenciais();

            // Valida as credenciais utilizando o objeto 'usuario'
            bool valido = _authService.ValidarCredenciais(utilizador);

            // Mostra o resultado do login
            _loginView.MostrarResultadoLogin(valido);

            if (valido)
            {
                Console.WriteLine("A iniciar aplicação");
                Thread.Sleep(3000); // espera 3 segundos
                Console.Clear();    // limpa o console

                // Instancia e exibe o menu principal
                var menuCtrl = new MenuPrincipalController();
                menuCtrl.MostrarMenu(utilizador.Username, utilizador.Role, _authService);
            }
        }
    }
}
