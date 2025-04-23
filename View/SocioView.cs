using GestorGinasio.Model;
using System;

namespace GestorGinasio.View
{
    public class SocioView
    {
        public static void MostrarDadosSocio(Socio socio)
        {
            Console.WriteLine($"ID: {socio.Id}, Nome: {socio.Nome}, Email: {socio.Email}");
        }
    }
}
