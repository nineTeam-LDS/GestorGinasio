using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using GestorGinasio.Model.Entities;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace GestorGinasio.Model.Services
{
    public static class ReportService
    {
        // -----------  Preparação global  -----------
        static ReportService() =>
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        private static readonly string _baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _logoPath = Path.Combine(_baseDir, "View", "Assets", "logo.jpg");


        // -----------  Facades públicos  -----------
        public static void GerarSocios(IEnumerable<Socio> lista, string user) =>
            GerarPdfInterno(
                "Relatório de Sócios",
                lista.Select(s => $"{s.Id,3}  {s.Nome,-20}  {s.Email,-25} {s.DataInscricao:yyyy-MM-dd}"),
                Path.Combine(_baseDir, "Relatorios", $"Socios_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"),
                user);

        public static void GerarAulas(IEnumerable<Aula> lista, string user) =>
            GerarPdfInterno(
                "Relatório de Aulas",
                lista.Select(a => $"{a.Id,3}  {a.Nome,-15}  {a.Instrutor,-15}  {a.Sala,3}  {a.Horario}"),
                Path.Combine(_baseDir, "Relatorios", $"Aulas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"),
                user);

        public static void GerarEquip(IEnumerable<Equipamento> lista, string user) =>
            GerarPdfInterno(
                "Relatório de Equipamentos",
                lista.Select(e => $"{e.Id,3}  {e.Nome,-20}  Qtd: {e.Quantidade,3}"),
                Path.Combine(_baseDir, "Relatorios", $"Equip_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"),
                user);


        // -----------  Motor interno  -----------
        private static void GerarPdfInterno(string titulo,
                                            IEnumerable<string> linhas,
                                            string ficheiro,
                                            string utilizador)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ficheiro)!);

            using var doc = new PdfDocument { Info = { Title = titulo } };
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            double y = 20;

            //-- Logo
            if (File.Exists(_logoPath))
            {
                using var img = XImage.FromFile(_logoPath);
                const double largura = 100;
                double altura = img.PixelHeight * largura / img.PixelWidth;
                gfx.DrawImage(img, 40, y, largura, altura);
                y += altura + 10;
            }

            //-- Título
            var fTitulo = new XFont("Verdana", 16, XFontStyle.Bold);
            gfx.DrawString(titulo, fTitulo, XBrushes.Black,
                           new XRect(40, y, page.Width - 80, 30),
                           XStringFormats.TopCenter);
            y += 40;

            //-- Meta-dados (data e utilizador)
            var fMeta = new XFont("Verdana", 10);
            string dataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            gfx.DrawString(dataHora, fMeta, XBrushes.Black, new XPoint(40, y));

            string userText = $"Utilizador: {utilizador}";
            double wUser = gfx.MeasureString(userText, fMeta).Width;
            gfx.DrawString(userText, fMeta, XBrushes.Black,
                           new XPoint(page.Width - 40 - wUser, y));
            y += 20;

            gfx.DrawLine(XPens.Black, 40, y, page.Width - 40, y);
            y += 20;

            //-- Corpo
            var fTexto = new XFont("Verdana", 12);
            foreach (string linha in linhas)
            {
                gfx.DrawString(linha, fTexto, XBrushes.Black, new XPoint(40, y));
                y += 20;

                if (y > page.Height - 40)
                {
                    page = doc.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 40;
                }
            }

            doc.Save(ficheiro);
            Process.Start(new ProcessStartInfo(ficheiro) { UseShellExecute = true });
            Console.WriteLine($"\nRelatório gerado em: {ficheiro}");
        }
    }
}

