// File: Model/Services/ReportService.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using GestorGinasio.Model.Entities;
using PdfSharp; // para PdfSharpException
using GestorGinasio.Model.Exceptions;

namespace GestorGinasio.Model.Services
{
    public class ReportService : IReportService
    {
        private readonly string _logoPath;
        private readonly string _reportsDir;
        private const int LinhasPorPagina = 40;
        private const string FormatoSocio = "{0,5} {1,-20} {2,-30} {3}";
        private const string FormatoAula = "{0,5} {1,-20} {2,-20} {3,5} {4}";
        private const string FormatoEquip = "{0,5} {1,-25} {2,5} {3,-20} {4}";

        public ReportService()
        {
            // necessário para PdfSharp no .NET Core
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // baseDir → e.g. bin\Debug\net6.0
            var baseDir = AppContext.BaseDirectory;

            // pasta Relatorios existe
            _reportsDir = Path.Combine(baseDir, "Relatorios");
            Directory.CreateDirectory(_reportsDir);

            // caminho para imagem logo
            _logoPath = Path.Combine(baseDir, "View", "Assets", "logo.jpg");
        }

        public string GenerateSociosReport(IEnumerable<Socio> socios, string currentUser)
        {
            // Prepara linhas
            var linhas = new List<string>
            {
                string.Format(FormatoSocio, "Id","Nome","Email","Data Inscrição")
            };
            linhas.AddRange(socios.Select(s =>
                string.Format(FormatoSocio, s.Id, s.Nome, s.Email, s.DataInscricao.ToString("yyyy-MM-dd"))
            ));

            var outputFile = Path.Combine(_reportsDir, $"Socios_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            try
            {
                GerarPdf("Relatório de Sócios", linhas, outputFile, currentUser);
                return outputFile;
            }
            catch (IOException ex)
            {
                throw new PdfGenerationException($"Falha de I/O ao gravar o PDF em '{outputFile}'.", ex);
            }
            catch (PdfSharpException ex)
            {
                throw new PdfGenerationException($"Erro interno do PDFsharp ao gerar relatório de sócios.", ex);
            }
            catch (Exception ex)
            {
                // Qualquer outro erro imprevisto
                throw new PdfGenerationException($"Erro inesperado ao gerar relatório de sócios.", ex);
            }
        }

        public string GenerateAulasReport(IEnumerable<Aula> aulas, string currentUser)
        {
            var linhas = new List<string>
            {
                string.Format(FormatoAula, "Id","Nome","Instrutor","Sala","Horário")
            };
            linhas.AddRange(aulas.Select(a =>
                string.Format(FormatoAula, a.Id, a.Nome, a.Instrutor, a.Sala, a.Horario)
            ));

            var outputFile = Path.Combine(_reportsDir, $"Aulas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            try
            {
                GerarPdf("Relatório de Aulas", linhas, outputFile, currentUser);
                return outputFile;
            }
            catch (IOException ex)
            {
                throw new PdfGenerationException($"Falha de I/O ao gravar o PDF em '{outputFile}'.", ex);
            }
            catch (PdfSharpException ex)
            {
                throw new PdfGenerationException($"Erro interno do PDFsharp ao gerar relatório de aulas.", ex);
            }
            catch (Exception ex)
            {
                throw new PdfGenerationException($"Erro inesperado ao gerar relatório de aulas.", ex);
            }
        }

        public string GenerateEquipamentosReport(IEnumerable<Equipamento> equipamentos, string currentUser)
        {
            var linhas = new List<string>
            {
                string.Format(FormatoEquip, "Id","Nome","Qtd","Instrutor","Horário")
            };
            linhas.AddRange(equipamentos.Select(e =>
                string.Format(FormatoEquip, e.Id, e.Nome, e.Quantidade, e.Instrutor, e.Horario)
            ));

            var outputFile = Path.Combine(_reportsDir, $"Equipamentos_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            try
            {
                GerarPdf("Relatório de Equipamentos", linhas, outputFile, currentUser);
                return outputFile;
            }
            catch (IOException ex)
            {
                throw new PdfGenerationException($"Falha de I/O ao gravar o PDF em '{outputFile}'.", ex);
            }
            catch (PdfSharpException ex)
            {
                throw new PdfGenerationException($"Erro interno do PDFsharp ao gerar relatório de equipamentos.", ex);
            }
            catch (Exception ex)
            {
                throw new PdfGenerationException($"Erro inesperado ao gerar relatório de equipamentos.", ex);
            }
        }

        private void GerarPdf(string titulo, IEnumerable<string> linhas, string ficheiro, string utilizador)
        {
            var doc = new PdfDocument();
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
            var fontHeader = new XFont("Arial", 12, XFontStyle.Bold);
            var fontLine = new XFont("Courier New", 11, XFontStyle.Regular);
            var fontRodape = new XFont("Arial", 9, XFontStyle.Italic);
            double y = 50;

            // Inserir logo, se existir
            if (File.Exists(_logoPath))
            {
                using var img = XImage.FromFile(_logoPath);
                gfx.DrawImage(img, 50, y, 100, 50);
                y += 60;
            }

            // Título
            gfx.DrawString(titulo, fontTitle, XBrushes.Black,
                new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);
            y += 40;

            // Cabeçalho com data e utilizador
            gfx.DrawString(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), fontHeader, XBrushes.Black, 50, y);
            gfx.DrawString($"Utilizador: {utilizador}", fontHeader, XBrushes.Black, page.Width - 200, y);
            y += 30;

            gfx.DrawLine(XPens.Black, 50, y, page.Width - 50, y);
            y += 20;

            int contador = 0;
            foreach (var linha in linhas)
            {
                if (contador == LinhasPorPagina)
                {
                    InserirRodape(gfx, doc.PageCount);
                    page = doc.AddPage();
                    gfx.Dispose();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 50;
                    contador = 0;
                }

                gfx.DrawString(linha, fontLine, XBrushes.Black, 50, y);
                y += 15;
                contador++;
            }

            InserirRodape(gfx, doc.PageCount);

            // Garante que a pasta existe (já foi criada no construtor)
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
            gfx.DrawString($"Página {pagina}", fontRodape, XBrushes.Gray,
                new XRect(0, gfx.PageSize.Height - 30, gfx.PageSize.Width, 20), XStringFormats.Center);
        }
    }
}


