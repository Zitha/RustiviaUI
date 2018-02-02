// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfViewController.cs" company="SemanticArchitecture">
//   http://www.SemanticArchitecture.net pkalkie@gmail.com
// </copyright>
// <summary>
//   Extends the controller with functionality for rendering PDF views
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

namespace RustiviaSolutions.PDFGenerator
{
    /// <summary>
    ///     Extends the controller with functionality for rendering PDF views
    /// </summary>
    public class PdfViewController : Controller
    {
        private readonly HtmlViewRenderer _htmlViewRenderer;
        private readonly StandardPdfRenderer _standardPdfRenderer;

        public PdfViewController()
        {
            _htmlViewRenderer = new HtmlViewRenderer();
            _standardPdfRenderer = new StandardPdfRenderer();
        }

        protected ActionResult ViewPdf(string pageTitle, string viewName, object model)
        {
            // Render the view html to a string.
            string htmlText = _htmlViewRenderer.RenderViewToString(this, viewName, model);

            // Let the html be rendered into a PDF document through iTextSharp.
            byte[] buffer = _standardPdfRenderer.Render(htmlText, pageTitle);

            // Return the PDF as a binary stream to the client.
            return new BinaryContentResult(buffer, "application/pdf");
        }
    }
}