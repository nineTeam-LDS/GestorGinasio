﻿// File: View/Terminal/IAulaView.cs
using System.Collections.Generic;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Operações de I/O para gerir Aulas no terminal.
    public interface IAulaView
    {
        ConsoleKey MostrarMenu();
        void MostrarLista(IEnumerable<Aula> lista);
        Aula PedirNovaAula();
        int PedirIdParaEditar();
        Aula PedirDadosEditados(Aula existente);
        int PedirIdParaRemover();
        bool Confirmar(string mensagem);
        void Sucesso(string mensagem);
        void Avaliar(string mensagem);
    }
}
