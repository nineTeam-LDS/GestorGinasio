// File: View/Terminal/IEquipamentosView.cs
using System;
using System.Collections.Generic;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Contrato para a interface de I/O do ecrã de gestão de equipamentos.
    public interface IEquipamentoView
    {
        // Mostra o menu de opções e devolve a tecla selecionada.
        ConsoleKey MostrarMenu();
        void MostrarLista(IEnumerable<Equipamento> lista);
        Equipamento PedirNovoEquipamento();
        int PedirIdParaEditar();
        Equipamento PedirDadosEditados(Equipamento existente);
        int PedirIdParaRemover();
        bool Confirmar(string mensagem);
        void Sucesso(string mensagem);
        void Avaliar(string mensagem);
    }
}
