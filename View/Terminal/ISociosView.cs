// File: View/Terminal/ISociosView.cs
using System;
using System.Collections.Generic;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Contrato para a interface de I/O do ecrã de gestão de sócios.
    public interface ISociosView
    {
        ConsoleKey MostrarMenu();
        void MostrarLista(IEnumerable<Socio> lista);
        void MostrarDetalhes(Socio socio);
        Socio PedirNovoSocio();
        int PedirIdParaEditar();
        Socio PedirDadosEditados(Socio existente);
        int PedirIdParaRemover();
        bool Confirmar(string mensagem);
        void Sucesso(string mensagem);
        void Avaliar(string mensagem);
    }
}
