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
        int PedirIdParaRemover();
        Equipamento PedirDadosEditados(Equipamento existente);
        bool Confirmar(string mensagem);
        void IdInexistente();
        void Sucesso(string mensagem);
    }
}
