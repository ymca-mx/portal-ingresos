using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AppAdministrativos.WS
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
        public List<DTOReporteAlumnoOferta> ObtenerReporteAlumnoOferta()
        {
            return BLLReportePortal.ObtenerReporteAlumnoOferta();
        }

        [WebMethod]
        public DTOCuatrimestre CargarCuatrimestre()
        {
            return BLLReportePortal.CargarCuatrimestre();
        }

        [WebMethod]
        public DTOCuatrimestre CargarCuatrimestreHistorico()
        {
            return BLLReportePortal.CargarCuatrimestreHistorico();
        }


        [WebMethod]
        public DTOReporteSaldos CargarReporteSaldos()
        {
            return BLLReportePortal.CargarReporteSaldos();
        }

        [WebMethod]
        public List<DTOSaldosDetalle> CargarReporteSaldosDetalle(int alumnoId)
        {
            return BLLReportePortal.CargarReporteSaldoDetalle(alumnoId);
        }

        [WebMethod]
        public DTOReporteBecas CargarReporteBecas(int anio, int periodo)
        {
            return BLLReportePortal.CargarReporteBecas(anio, periodo);
        }

        [WebMethod]
        public List<DTOReporteBecasCuatrimestre> CargaReporteBecaCuatrimestre(int anio, int periodo)
        {
            return BLLReportePortal.CargaReporteBecaCuatrimestre(anio, periodo);
        }

        [WebMethod]
        public List<DTOReporteInscrito> CargaReporteInscrito(int anio, int periodo)
        {
            return BLLReportePortal.CargaReporteInscrito(anio, periodo);
        }

        [WebMethod]
        public List<DTOReporteBecaSep> CargaReporteBecaSep(int anio, int periodo)
        {
            return BLLReportePortal.CargaReporteBecaSep(anio, periodo);
        }
        
        [WebMethod]
        public List<DTOReporteInegi> CargaReporteIneg(int anio, int periodo)
        {
            return BLLReportePortal.CargaReporteIneg(anio, periodo);
        }

        [WebMethod]
        public List<DTOReporteAlumnoReferencia> CargaReporteAlumnoReferencia(int anio, int periodo)
        {
            return BLLReportePortal.CargaReporteAlumnoReferencia(anio, periodo);
        }

        [WebMethod]
        public DTOVoBo CargarReporteVoBo(int anio, int periodoid, int usuarioid)
        {
            return BLLReportePortal.CargarReporteVoBo(anio, periodoid,usuarioid);
        }

        [WebMethod]
        public bool ReporteVoBoEnviarEmail(int AlumnoId, string EmailAlumno)
        {
            return BLLReportePortal.ReporteVoBoEnviarEmail(AlumnoId, EmailAlumno);
        }

        [WebMethod]
        public List<DTOReporteCarteraVencida> CarteraVencida(int Anio, int PeriodoId, string FechaInicial, string FechaFinal)
        {
            return
            BLLReportePortal.CarteraVencida(Anio, PeriodoId, FechaInicial, FechaFinal);
        }

    }
}
