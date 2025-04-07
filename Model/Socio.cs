using System;

public class Socio
{
    public event EventHandler DadosAtualizados;
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }

    public void AtualizarDados(string nome, string email)
    {
        Nome = nome;
        Email = email;
        DadosAtualizados?.Invoke(this, EventArgs.Empty);
    }
}
