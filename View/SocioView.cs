using System;

public class SocioView
{
    public void MostrarDadosSocio(Socio socio)
    {
        Console.WriteLine($"ID: {socio.Id}, Nome: {socio.Nome}, Email: {socio.Email}");
    }
}
