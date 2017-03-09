using System;
using Reportes.Reportes;
using System.IO;
namespace SistemaUni
{
    public partial class Referencias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //int AlumnoId = int.Parse(Request.QueryString["AlumnoId"]);
            //int OfertaEducativaid = int.Parse(Request.QueryString["OfertaEducativaId"]);
            //List<object> lstobj = BLLCuota.CuotaCredencial(AlumnoId, OfertaEducativaid);
            RptReferencias rptCredenciales = new RptReferencias();
            //rptCredenciales.Database.Tables["Alumno"].SetDataSource(lstobj[0]);
            //rptCredenciales.Database.Tables["OfertaEducativa"].SetDataSource(lstobj[1]);
            //rptCredenciales.Database.Tables["Cuota"].SetDataSource(lstobj[2]);
            //rptCredenciales.SetParameterValue("CredencialN","100");
            //rptCredenciales.SetParameterValue("CredencialR", "200");
            string ruta = Path.GetTempPath();
            ruta += "Referencias.pdf";

            //CrystalReportViewer1.ReportSource = rptCredenciales;
            Stream pdf;
            rptCredenciales.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, ruta);

            pdf = rptCredenciales.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var MemoryStream = new MemoryStream();
            pdf.CopyTo(MemoryStream);

            Response.Expires = 0;
            Response.Buffer = true;
            Response.ClearContent();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", MemoryStream.Length.ToString());
            Response.BinaryWrite(MemoryStream.ToArray());
            MemoryStream.Close();
        }
    }
}