using GestorGinasio.Model;
using GestorGinasio.View;

namespace GestorGinasio.Controller
{
    public class SocioController
    {
        private readonly Socio socio;
        private readonly SocioView view;

        public SocioController(Socio socio, SocioView view)
        {
            this.socio = socio;
            this.view = view;
            //socio.DadosAtualizados += OnDadosAtualizados;
        }

        //public void AtualizarSocio(string nome, string email)
        //{
        //    socio.AtualizarDados(nome, email);
        //}

        private void OnDadosAtualizados(object sender, EventArgs e)
        {
            SocioView.MostrarDadosSocio(socio);
        }
    }
}
