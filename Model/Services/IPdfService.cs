using System.Collections.Generic;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.Model.Services
{
    public interface IPdfService
    {
        // Gera um relatório de sócios em PDF, retorna o caminho do ficheiro gerado.
        string GenerateSociosReport(IEnumerable<Socio> socios, string user, string outputDirectory);

        // Gera um relatório de aulas em PDF, retorna o caminho do ficheiro gerado.
        string GenerateAulasReport(IEnumerable<Aula> aulas, string user, string outputDirectory);
    }
}
