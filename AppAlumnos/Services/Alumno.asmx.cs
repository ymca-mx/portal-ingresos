using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Universidad.BLL;
using Universidad.DTO;
using Universidad.DTO.Alumno;

namespace AppAlumnos.Services
{
    /// <summary>
    /// Descripción breve de Alumno
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Alumno : System.Web.Services.WebService
    {

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public DTOAlumnoDatosGenerales Valida(int alumnoId, string password)
        {
            return Universidad.BLL.BLLAlumno.LoginAcademico(alumnoId, password);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public DTOAlumnoImagen Datos(int alumnoId)
        {
            return Universidad.BLL.BLLAlumno.ImagenIndex(alumnoId);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void RecuperaPassword(string email)
        {
            Universidad.BLL.BLLAlumno.RecuperaPassword(email);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void ActualizaPassword(string password, string token)
        {
            Universidad.BLL.BLLAlumno.ActualizaPassword(password, token);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertaBitacora(int alumnoId)
        {
            Universidad.BLL.BLLAlumno.InsertaBitacora(alumnoId);
        }
        [WebMethod]
        public string[] CalcularAnticipado(string alumnoId)
        {
            return BLLPeriodo.SaberAnticipado(int.Parse(alumnoId));
        }

        [WebMethod]
        public DTO.DTOAlumno ConsultarAlumnoReinscripcion(string AlumnoId)
        {
            return BLL.BLLAlumnoPortal.ObtenerAlumnoR(int.Parse(AlumnoId));
        }


        [WebMethod]
        public string TraerSede(string AlumnoId)
        {
            return BLL.BLLSede.SedeAlumno(int.Parse(AlumnoId));
        }

        [WebMethod]
        public bool VerificaAlumnoDatos(string AlumnoId)
        {
            return BLL.BLLAlumnoPortal.VerificaAlumnoDatos(int.Parse(AlumnoId));
        }

        [WebMethod]
        public bool VerificaAlumnoEncuesta(string AlumnoId)
        {
            return BLL.BLLAlumnoPortal.VerificaAlumnoEncuesta(int.Parse(AlumnoId));
        }


        //datos personales  generados por el alumnos
        [WebMethod]
        public DTO.DTOAlumno ObenerDatosAlumnoActualiza(string AlumnoId)
        {
            return BLL.BLLAlumnoPortal.ObenerDatosAlumnoActualiza(int.Parse(AlumnoId));
        }

        [WebMethod]
        public bool UpdateAlumnoDatos(DTO.DTOAlumnoDetalle AlumnoDatos)
        {

            return BLL.BLLAlumnoPortal.UpdateAlumnoDatos(AlumnoDatos);
        }



        [WebMethod]
        public List<DTO.DTOPreguntas> PreguntasPortal()
        {

            return BLL.BLLAlumnoPortal.PreguntasPortal();
        }

        [WebMethod]
        public bool GuardarRespuestas(DTO.DTORespuestas RespuestasEncuesta)
        {

            return BLL.BLLAlumnoPortal.GuardarRespuestas(RespuestasEncuesta);
        }

        public class doblesobj
        {
            public List<DTO.DTOPagoDetallado> item1 { get; set; }
            public bool item2 { get; set; }
        }

        [WebMethod]
        public doblesobj ConsultaPagosDetalle(string AlumnoId, string Anio, string PeriodoId)
        {
            doblesobj objr = new doblesobj();

            try
            {
                List<DTO.DTOPagoDetallado> lstRes = BLL.BLLPagoPortal.ReferenciasPago(int.Parse(AlumnoId), int.Parse(Anio), int.Parse(PeriodoId));
                bool rest;
                int cout = lstRes.Where(l => l.OtroDescuento.Length > 0).ToList().Count;
                rest = cout > 0 ? true : false;
                objr.item1 = lstRes;
                objr.item2 = rest;

                return objr;
            }
            catch
            {
                return objr;
            }
        }

        [WebMethod]
        public List<DTO.DTOPeriodo> ConsultarPeriodosAlumno(string AlumnoId)
        {
            return BLL.BLLPeriodoPortal.ConsultarPeriodos(int.Parse(AlumnoId));
        }

        [WebMethod]
        public DTO.DTOAlumno ConsultarAlumno(string AlumnoId)
        {
            return BLL.BLLAlumnoPortal.ObtenerAlumno(int.Parse(AlumnoId));
        }

        [WebMethod]
        public string ConsultarAdeudo(string AlumnoId, string OfertaEducativaId)
        {
            return BLL.BLLPagoPortal.TraerAdeudos(int.Parse(AlumnoId), int.Parse(OfertaEducativaId));
        }

        [WebMethod]
        public List<DTO.DTOPagos> ConsultarReferenciasCP(string AlumnoId)
        {
            return BLL.BLLPagoPortal.ConsultarReferenciasConceptos(int.Parse(AlumnoId));
        }
    }
}
