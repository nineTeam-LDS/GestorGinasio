using System;
using System.Collections.Generic;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Contrato para a interface de I/O do ecrã de gestão de utilizadores.
    public interface IUserView
    {
        ConsoleKey MostrarMenu();
        void MostrarLista(IEnumerable<User> usuarios);
        User PedirNovoUtilizador();
        int PedirIdParaEditar();
        int PedirIdParaRemover();
        User PedirDadosEditados(User existente);
        bool Confirmar(string mensagem);
        void Sucesso(string mensagem);
        void IdInexistente();
    }
}
