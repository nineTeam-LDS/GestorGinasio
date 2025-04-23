// File: Services/ReportService.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using GestorGinasio.Model;

namespace GestorGinasio.Services
{
    public static class ReportService
    {
        static ReportService()
        {
            // Habilita code page 1252 para PdfSharp
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private static readonly string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string logoPath = Path.Combine(baseDir, "logo.png");

        public static void GenerateSociosReport(List<Socio> list, string username)
        {
            string dateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string userText = $"Utilizador: {username}";
            var lines = list.ConvertAll(s => $"{s.Id}: {s.Nome} - {s.Email}");
            var file = Path.Combine(baseDir, "Relatorios", $"Socios_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            GeneratePdf("Relatório de Sócios", dateTime, userText, lines, file);
        }

        public static void GenerateAulasReport(List<Aula> list, string username)
        {
            string dateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string userText = $"Utilizador: {username}";
            var lines = list.ConvertAll(a => $"{a.Id}: {a.Nome} - {a.Instrutor} às {a.Horario}");
            var file = Path.Combine(baseDir, "Relatorios", $"Aulas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            GeneratePdf("Relatório de Aulas", dateTime, userText, lines, file);
        }

        public static void GenerateEquipamentosReport(List<Equipamento> list, string username)
        {
            string dateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string userText = $"Utilizador: {username}";
            var lines = list.ConvertAll(e => $"{e.Id}: {e.Nome} (Qtd: {e.Quantidade})");
            var file = Path.Combine(baseDir, "Relatorios", $"Equipamentos_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            GeneratePdf("Relatório de Equipamentos", dateTime, userText, lines, file);
        }

        private static void GeneratePdf(string title, string dateTime, string userText, List<string> lines, string file)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(file)!);

            using var doc = new PdfDocument();
            doc.Info.Title = title;
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            double y = 20;
            // Desenha logo se existir
            if (File.Exists(logoPath))
            {
                var img = XImage.FromFile(logoPath);
                double imgWidth = 100;
                double imgHeight = img.PixelHeight * imgWidth / img.PixelWidth;
                gfx.DrawImage(img, 40, y, imgWidth, imgHeight);
                y += imgHeight + 10;
            }

            // Título
            var fontTitle = new XFont("Verdana", 16, XFontStyle.Bold);
            gfx.DrawString(title, fontTitle, XBrushes.Black,
                new XRect(40, y, page.Width - 80, 30), XStringFormats.TopCenter);
            y += 40;

            // Data/Hora e Utilizador
            var fontMeta = new XFont("Verdana", 10, XFontStyle.Regular);
            gfx.DrawString(dateTime, fontMeta, XBrushes.Black, new XPoint(40, y));
            double userWidth = gfx.MeasureString(userText, fontMeta).Width;
            gfx.DrawString(userText, fontMeta, XBrushes.Black,
                new XPoint(page.Width - 40 - userWidth, y));
            y += 20;

            // Linha separadora
            gfx.DrawLine(XPens.Black, 40, y, page.Width - 40, y);
            y += 20;

            // Conteúdo
            var fontText = new XFont("Verdana", 12, XFontStyle.Regular);
            foreach (var line in lines)
            {
                gfx.DrawString(line, fontText, XBrushes.Black, new XPoint(40, y));
                y += 20;
                if (y > page.Height - 40)
                {
                    page = doc.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 40;
                }
            }

            doc.Save(file);
            Process.Start(new ProcessStartInfo(file) { UseShellExecute = true });
            Console.WriteLine($"Relatório gerado: {file}");
        }
    }
}
