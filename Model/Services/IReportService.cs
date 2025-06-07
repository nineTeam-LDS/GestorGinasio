// File: Model/Services/IReportService.cs
using System.Collections.Generic;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.Model.Services
{
    // Gera relatórios em PDF a partir dos dados das entidades.
    public interface IReportService
    {
        // Gera um PDF de sócios e devolve o caminho do ficheiro.
        string GenerateSociosReport(IEnumerable<Socio> socios, string currentUser);

        // Gera um PDF de aulas e devolve o caminho do ficheiro.
        string GenerateAulasReport(IEnumerable<Aula> aulas, string currentUser);

        // Gera um PDF de equipamentos e devolve o caminho do ficheiro.
        string GenerateEquipamentosReport(IEnumerable<Equipamento> equipamentos, string currentUser);
    }
}

