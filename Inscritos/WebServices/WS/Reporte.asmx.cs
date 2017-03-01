using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebServices.WS
{
    /// <summary>
    /// Descripción breve de Reporte
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Reporte : System.Web.Services.WebService
    {

        [WebMethod]
        public List<DTOReporteAlumnoOferta> MostraReporteAlumnoOferta()
        {
            return BLLReporte.ObtenerReporteAlumnoOferta();
        }

        [WebMethod]
        public DTOFIltros MostrarCuatrimestre()
        {
            return BLLReporte.CargarCuatrimestre();
        }

        [WebMethod]
        public List<DTOReporteBecasCuatrimestre> MostrarReporteBecaCuatrimestre(int anio, int periodo)
        {
            return BLLReporte.CargaReporteBecaCuatrimestre(anio, periodo);
        }

        [WebMethod]
        public List<DTOReporteInscrito> MostrarReporteInscrito(int anio, int periodo)
        {
            return BLLReporte.CargaReporteInscrito(anio, periodo);
        }

        [WebMethod]
        public List<DTOReporteBecaSep> MostrarReporteBecaSep(int anio, int periodo)
        {
            return BLLReporte.CargaReporteBecaSep(anio, periodo);
        }


        [WebMethod]
        public List<DTOReporteInegi> MostrarReporteIneg(int anio, int periodo)
        {
            return BLLReporte.CargaReporteIneg(anio, periodo);
        }

        [WebMethod]
        public List<DTOReporteAlumnoReferencia> MostrarReporteAlumnoReferencia(int anio, int periodo)
        {
            return BLLReporte.CargaReporteAlumnoReferencia(anio, periodo);
        }
        [WebMethod]
        public DTOVoBo ReporteVoBo(int anio, int periodoid, int usuarioid)
        {
            return BLLReporte.ReporteVoBo(anio, periodoid,usuarioid);
        }

    }
}
