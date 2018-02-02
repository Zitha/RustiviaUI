﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryContentResult.cs" company="SemanticArchitecture">
//   http://www.SemanticArchitecture.net pkalkie@gmail.com
// </copyright>
// <summary>
//   An ActionResult used to send binary data to the browser.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Web;
using System.Web.Mvc;

namespace RustiviaSolutions.PDFGenerator
{
    /// <summary>
    ///     An ActionResult used to send binary data to the browser.
    /// </summary>
    public class BinaryContentResult : ActionResult
    {
        private readonly byte[] _contentBytes;
        private readonly string _contentType;

        public BinaryContentResult(byte[] contentBytes, string contentType)
        {
            this._contentBytes = contentBytes;
            this._contentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.ContentType = _contentType;

            using (var stream = new MemoryStream(_contentBytes))
            {
                stream.WriteTo(response.OutputStream);
                stream.Flush();
            }
        }
    }
}