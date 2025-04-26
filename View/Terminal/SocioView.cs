using System;
using GestorGinasio.Model.Services;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    public class SocioView
    {
        public static void MostrarDadosSocio(Socio socio)
        {
            Console.WriteLine($"ID: {socio.Id}, Nome: {socio.Nome}, Email: {socio.Email}");
        }
    }
}
