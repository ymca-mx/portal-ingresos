using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Reportes;
using DTO;
using BLL;
using Utilities;
using Reportes.Reportes;
using System.IO;
using System.Threading.Tasks;

namespace AppAdministrativos.Views.Alumno
{
    public partial class Credenciales : System.Web.UI.Page
    {
        int AlumnoId;
        int OfertaEducativaid;
        public async void Page_Load(object sender, EventArgs e)
        {
             AlumnoId= int.Parse(Request.QueryString["AlumnoId"]);
            OfertaEducativaid = int.Parse(Request.QueryString["OfertaEducativaId"]);

            await CrearReporteAsync();
            
        }
        public async Task CrearReporteAsync()
        {
            try
            {
                Tuple<List<CString_Alumno>, List<CString_OfertaEducativa>, List<CString_Cuota>> ObjetoCompuesto =
                    await BLLCuota.CuotaCredencial(AlumnoId, OfertaEducativaid);

                string rutarpt = "/Reportes/Reportes/EntregaCredenciales.rpt";
                EntregaCredenciales ReporteCredenciales = new EntregaCredenciales();

                ReporteCredenciales.Load(rutarpt);

                ReporteCredenciales.Database.Tables["Alumno"].SetDataSource(ObjetoCompuesto.Item1);
                ReporteCredenciales.Database.Tables["OfertaEducativa"].SetDataSource(ObjetoCompuesto.Item2);
                ReporteCredenciales.Database.Tables["Cuota"].SetDataSource(ObjetoCompuesto.Item3);
                string ruta = Path.GetTempPath();
                ruta += "Credencial.pdf";

                //CrystalReportViewer1.ReportSource = rptCredenciales;
                Stream pdf;
                ReporteCredenciales.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, ruta);

                pdf = ReporteCredenciales.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
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
            catch(Exception error)
            {
                //Label lblerror = new Label
                //{
                //    Text = "Error: " + error.InnerException.Message
                //};
                
                //Header.Controls.Add(lblerror);
                Response.Write(error);
            }
        }
    }
}