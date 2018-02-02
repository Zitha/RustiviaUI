﻿using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace IntroductionMVC5.Web.Utils.Printing
{
    /// <summary>
    ///     This class is responsible for rendering a html text string to a PDF document using the html renderer of iTextSharp.
    /// </summary>
    public class StandardPdfRenderer
    {
        private const int HorizontalMargin = 40;
        private const int VerticalMargin = 40;

        public byte[] Render(string htmlText, string pageTitle)
        {
            byte[] renderedBuffer;

            using (var outputMemoryStream = new MemoryStream())
            {
                using (
                    var pdfDocument = new Document(PageSize.A4, HorizontalMargin, HorizontalMargin, VerticalMargin,
                        VerticalMargin))
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, outputMemoryStream);
                    pdfWriter.CloseStream = false;
                    pdfWriter.PageEvent = new PrintHeaderFooter {Title = pageTitle};
                    pdfDocument.Open();
                    using (var htmlViewReader = new StringReader(htmlText))
                    {
                        using (var htmlWorker = new HTMLWorker(pdfDocument))
                        {
                            //                            htmlWorker.Open();
                            htmlWorker.Parse(htmlViewReader);
                            //                            htmlWorker.Close();
                        }
                    }
                }

                renderedBuffer = new byte[outputMemoryStream.Position];
                outputMemoryStream.Position = 0;
                outputMemoryStream.Read(renderedBuffer, 0, renderedBuffer.Length);
            }

            return renderedBuffer;
        }
    }
}