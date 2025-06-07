using NUnit.Framework;
using GestorGinasio.Tests;

[TestFixture]
public class TestSocio
{
    [Test]
    public void AtualizarDados_DeveLancarEvento()
    {
        var socio = new Socio();
        bool eventoDisparado = false;

        socio.DadosAtualizados += (sender, e) => eventoDisparado = true;

        socio.AtualizarDados("Pedro", "pedro@exemplo.com");

        Assert.IsTrue(eventoDisparado);
    }
}
