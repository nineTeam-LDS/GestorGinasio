using System.Text;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using GestorGinasio.Model.Entities;

namespace GestorGinasio.Model.Services
{
    public static class ReportService
    {
        static ReportService() => Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        private static readonly string _baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _logoPath = Path.Combine(_baseDir, "View", "Assets", "logo.jpg");

        private const int LinhasPorPagina = 40;

        private const string FormatoSocio = "{0,5} {1,-20} {2,-30} {3}";
        private const string FormatoAula = "{0,5} {1,-20} {2,-20} {3,5} {4}";
        private const string FormatoEquip = "{0,5} {1,-25} {2,5} {3,-20} {4}";

        public static void GerarSocios(IEnumerable<Socio> lista, string user)
        {
            var linhas = new List<string>
            {
                string.Format(FormatoSocio, "Id", "Nome", "Email", "Data Inscrição")
            };
            linhas.AddRange(lista.Select(s => string.Format(FormatoSocio, s.Id, s.Nome, s.Email, s.DataInscricao.ToString("yyyy-MM-dd"))));

            GerarPdfInterno("Relatório de Sócios", linhas, Path.Combine(_baseDir, "Relatorios", $"Socios_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"), user);
        }

        public static void GerarAulas(IEnumerable<Aula> lista, string user)
        {
            var linhas = new List<string>
            {
                string.Format(FormatoAula, "Id", "Nome", "Instrutor", "Sala", "Horário")
            };
            linhas.AddRange(lista.Select(a => string.Format(FormatoAula, a.Id, a.Nome, a.Instrutor, a.Sala, a.Horario)));

            GerarPdfInterno("Relatório de Aulas", linhas, Path.Combine(_baseDir, "Relatorios", $"Aulas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"), user);
        }

        public static void GerarEquip(IEnumerable<Equipamento> lista, string user)
        {
            var linhas = new List<string>
            {
                string.Format(FormatoEquip, "Id", "Nome", "Qtd", "Instrutor", "Horário")
            };
            linhas.AddRange(lista.Select(e => string.Format(FormatoEquip, e.Id, e.Nome, e.Quantidade, e.Instrutor, e.Horario)));

            GerarPdfInterno("Relatório de Equipamentos", linhas, Path.Combine(_baseDir, "Relatorios", $"Equip_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"), user);
        }

        private static void GerarPdfInterno(string titulo, IEnumerable<string> linhas, string ficheiro, string utilizador)
        {
            var doc = new PdfDocument();
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
            var fontHeader = new XFont("Arial", 12, XFontStyle.Bold);
            var fontLine = new XFont("Courier New", 11, XFontStyle.Regular);
            var fontRodape = new XFont("Arial", 9, XFontStyle.Italic);

            double y = 50;

            if (File.Exists(_logoPath))
            {
                using var img = XImage.FromFile(_logoPath);
                gfx.DrawImage(img, 50, y, 100, 50);
                y += 60;
            }

            gfx.DrawString(titulo, fontTitle, XBrushes.Black, new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);
            y += 40;

            gfx.DrawString(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), fontHeader, XBrushes.Black, 50, y);
            gfx.DrawString($"Utilizador: {utilizador}", fontHeader, XBrushes.Black, page.Width - 200, y);
            y += 30;

            gfx.DrawLine(XPens.Black, 50, y, page.Width - 50, y);
            y += 20;

            int contadorLinhas = 0;

            foreach (var linha in linhas)
            {
                if (contadorLinhas == LinhasPorPagina)
                {
                    InserirRodape(gfx, doc.PageCount);
                    page = doc.AddPage();
                    gfx.Dispose();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 50;
                    contadorLinhas = 0;
                }

                gfx.DrawString(linha, fontLine, XBrushes.Black, 50, y);
                y += 15;
                contadorLinhas++;
            }

            InserirRodape(gfx, doc.PageCount);

            Directory.CreateDirectory(Path.GetDirectoryName(ficheiro)!);
            doc.Save(ficheiro);

            // Abrir automaticamente o PDF
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = ficheiro,
                UseShellExecute = true
            });
        }

        private static void InserirRodape(XGraphics gfx, int pagina)
        {
            var fontRodape = new XFont("Arial", 9, XFontStyle.Italic);
            gfx.DrawString($"Página {pagina}", fontRodape, XBrushes.Gray, new XRect(0, gfx.PageSize.Height - 30, gfx.PageSize.Width, 20), XStringFormats.Center);
        }
    }
}


