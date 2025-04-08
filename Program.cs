//Ficheiro de Entrada
using System;

class Program
{
    static void Main()
    {
        var socio = new Socio { Id = 1, Nome = "Inicial", Email = "inicial@exemplo.com" };
        var view = new SocioView();
        var controller = new SocioController(socio, view);

        var menu = new MenuPrincipal();
        menu.ExibirMenu();

        controller.AtualizarSocio("Pedro", "pedro@exemplo.com");
    }
}
