using System;
using System.Collections.Generic;
using System.Linq;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.View.Terminal
{
    // Implementação concreta de ISociosView para terminal.
    public class GerirSociosView : ISociosView
    {
        public ConsoleKey MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("\n===== SÓCIOS =====\n");
            Console.WriteLine("1. Listar");
            Console.WriteLine("2. Criar");
            Console.WriteLine("3. Editar");
            Console.WriteLine("4. Remover");
            Console.WriteLine("0. Voltar");
            Console.Write("Opção: ");
            return Console.ReadKey(true).Key;
        }

        /* LISTA -------------------------------------------------------*/
        public void MostrarLista(IEnumerable<Socio> lista)
        {
            Console.Clear();
            Console.WriteLine("\n===== SÓCIOS =====\n");
            Console.WriteLine("{0,-4} {1,-20} {2,-25} {3}", "Id", "Nome", "Email", "Data Inscrição");
            foreach (var s in lista)
                Console.WriteLine($"{s.Id,-4} {s.Nome,-20} {s.Email,-25} {s.DataInscricao:yyyy-MM-dd}");
            Console.WriteLine("\n<Enter> para voltar…");
            Console.ReadLine();
        }
        public void MostrarDetalhes(Socio socio)
        {
            Console.Clear();
            Console.WriteLine($"ID: {socio.Id}\nNome: {socio.Nome}\nEmail: {socio.Email}\nData: {socio.DataInscricao:yyyy-MM-dd}");
            Console.WriteLine("\n<Enter> para continuar…");
            Console.ReadLine();
        }

        /* NOVO --------------------------------------------------------*/
        public Socio PedirNovoSocio()
        {
            Console.Clear();
            Console.WriteLine("\n===== NOVO SÓCIO =====\n");
            Console.Write("Nome:  "); var n = Console.ReadLine()!;
            Console.Write("Email: "); var e = Console.ReadLine()!;
            return new Socio { Nome = n, Email = e, DataInscricao = DateTime.Today };
        }

        /* EDITAR / REMOVER -------------------------------------------*/
        public int PedirIdParaEditar()
        {
            Console.Write("\nId a editar: ");
            return int.TryParse(Console.ReadLine(), out var id) ? id : -1;
        }
        public int PedirIdParaRemover() => PedirIdParaEditar();

        public Socio PedirDadosEditados(Socio existente)
        {
            Console.Clear();
            Console.WriteLine("\n===== EDITAR SÓCIO ===== (Enter = manter)\n");
            Console.Write($"Nome  [{existente.Nome}]: "); var n = Console.ReadLine();
            Console.Write($"Email [{existente.Email}]: "); var e = Console.ReadLine();
            existente.Nome = string.IsNullOrWhiteSpace(n) ? existente.Nome : n;
            existente.Email = string.IsNullOrWhiteSpace(e) ? existente.Email : e;
            return existente;
        }

        /* UTILITÁRIOS -------------------------------------------------*/
        public bool Confirmar(string mensagem)
        {
            Console.Write($"{mensagem} (S/N) ");
            return Console.ReadKey(true).Key == ConsoleKey.S;
        }

        public void Sucesso(string mensagem)
        {
            Console.WriteLine($"\n{mensagem}");
            Console.WriteLine("<Enter> para continuar…");
            Console.ReadLine();
        }

        public void IdInexistente()
        {
            Console.WriteLine("\nId inexistente.");
            Console.WriteLine("<Enter> para continuar…");
            Console.ReadLine();
        }
    }
}
