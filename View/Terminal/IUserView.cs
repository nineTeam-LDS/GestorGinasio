// File: View/Terminal/IUserView.cs
using System;
using System.Collections.Generic;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Contrato para a interface de I/O do ecrã de gestão de utilizadores.
    public interface IUserView
    {
        ConsoleKey MostrarMenu();
        void MostrarLista(IEnumerable<User> utilizadores);
        User PedirNovoUtilizador();
        int PedirIdParaEditar();
        User PedirDadosEditados(User existente);
        int PedirIdParaRemover();
        bool Confirmar(string mensagem);
        void Sucesso(string mensagem);
        void Avaliar(string mensagem);
    }
}
