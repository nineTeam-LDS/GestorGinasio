using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Contrato para a interface de I/O do ecrã de login.
    public interface ILoginView
    {
        // Solicita as credenciais de utilizador (username e password).
        User SolicitarCredenciais();

        // Mostra o resultado do processo de login.
        void MostrarResultadoLogin(bool sucesso);
    }
}
