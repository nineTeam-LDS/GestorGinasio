using System;
using System.Threading;
using GestorGinasio.Controller;
using GestorGinasio.Model.Entities;
using GestorGinasio.Model.Repositories;
using GestorGinasio.Model.Services;
using Microsoft.Extensions.DependencyInjection; // Requer NuGet: Microsoft.Extensions.DependencyInjection
using GestorGinasio.Model.Exceptions;
using GestorGinasio.View.Terminal;

namespace GestorGinasio
{
    class Program
    {
        static void Main()
        {
            // Configuração de DI
            var services = new ServiceCollection();

            // JsonService para leitura/gravação genérica
            services.AddSingleton<IJsonService, JsonService>();

            // repositório genérico para *todas* as entidades
            services.AddScoped(typeof(IRepository<>), typeof(JsonRepository<>));

            // Serviços de domínio
            services.AddTransient<ISocioService, SocioService>();
            services.AddTransient<IAulaService, AulaService>();
            services.AddTransient<IEquipamentoService, EquipamentoService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IInscricaoService, InscricaoService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddSingleton<IReportService, ReportService>();

            // ReportService
            services.AddSingleton<IReportService, ReportService>();
            //services.AddSingleton<IPdfRepository, PdfRepository>();
            //services.AddTransient<IPdfService, PdfService>();

            // Controllers
            services.AddTransient<ILoginController, LoginController>();
            services.AddTransient<IMenuPrincipalController, MenuPrincipalController>();
            services.AddTransient<IUserController, UserController>();
            services.AddTransient<ISocioController, SocioController>();
            services.AddTransient<IAulaController, AulaController>();
            services.AddTransient<IEquipamentoController, EquipamentoController>();
            services.AddTransient<IReportController, ReportController>();

            // Views
            services.AddTransient<ILoginView, LoginView>();
            services.AddTransient<IMenuPrincipalView, MenuPrincipalView>();
            services.AddTransient<IUserView, GerirUtilizadoresView>();
            services.AddTransient<ISociosView, GerirSociosView>();
            services.AddTransient<IAulaView, GerirAulasView>();
            services.AddTransient<IEquipamentoView, GerirEquipamentosView>();
            services.AddTransient<IReportView, GerarRelatoriosView>();

            var provider = services.BuildServiceProvider();
            var loginCtrl = provider.GetRequiredService<ILoginController>();
            var menuCtrl = provider.GetRequiredService<IMenuPrincipalController>();

            while (true)
            {
                loginCtrl.RealizarLogin(); // internamente faz login e chama o menu
                Console.Write("Trocar de utilizador? (S/N) ");
                if (!Console.ReadLine()!.Trim().Equals("S", StringComparison.OrdinalIgnoreCase))
                    break;
            }

            Console.WriteLine("Aplicação encerrada. Enter para sair.");
            Console.ReadKey();
        }
    }
}
