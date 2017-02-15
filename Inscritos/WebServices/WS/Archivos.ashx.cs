using BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebServices.WS
{
    /// <summary>
    /// Descripción breve de Archivos
    /// </summary>
    public class Archivos : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            GenerarPDF2(context);
        }
        public void GenerarPDF2(HttpContext obj)
        {

            string DocumentoId = "";
            DocumentoId = obj.Request.Params["DocumentoId"];
            DocumentoId = DocumentoId.Replace(".pdf", "");
            if (DocumentoId != null)
            {
                //return Convert.ToBase64String(BLLAlumnoInscrito.TraerDocumento(int.Parse(DocumentoId)));
                Stream stream = new MemoryStream(BLLAlumnoInscrito.TraerDocumento(int.Parse(DocumentoId)));
                var MemoryStream = new MemoryStream();
                stream.CopyTo(MemoryStream);

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("Content-Type", "application/pdf");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "inline; filename=" + "Beca" + ".pdf");

                HttpContext.Current.Response.BinaryWrite(MemoryStream.ToArray());
                //HttpContext.Current.Response.ContentType = "application/pdf";
                //HttpContext.Current.Response.AddHeader("content-disposition", MemoryStream.Length.ToString());
                MemoryStream.Close();
                HttpContext.Current.Response.Flush();

                HttpContext.Current.Response.End();

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}