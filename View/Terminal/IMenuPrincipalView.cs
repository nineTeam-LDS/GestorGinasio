// File: View/Terminal/IMenuPrincipalView.cs
using System;

namespace GestorGinasio.View.Terminal
{
    // Contrato para a interface da View do Menu Principal.
    public interface IMenuPrincipalView
    {
        // Exibe o cabeçalho com data, utilizador e role.
        void MostrarCabecalho(string user, string role);

        // Exibe as opções do menu e devolve a tecla escolhida.
        ConsoleKey MostrarOpcoes();
    }
}
