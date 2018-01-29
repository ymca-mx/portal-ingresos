using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Universidad.BLL;

namespace AppAlumnos.Controllers
{
    public class AlumnoController : ApiController
    {

        [Route("Api/Alumno/Datos/{alumnoId:int}")]
        [HttpGet]
        public IHttpActionResult Datos(int alumnoId)
        {
            return Ok(Universidad.BLL.BLLAlumno.ImagenIndex(alumnoId));
        }

        [Route("Api/Alumno/EstadoDeCuenta/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult EstadoDeCuenta(string AlumnoId,  [FromUri] string FechaI, [FromUri] string FechaF)
        {
            return Ok(
            BLL.BLLEstadoCuenta.ObtenerCargos(new DTO.DTOAlumno { AlumnoId = int.Parse(AlumnoId) },
                            DateTime.ParseExact(FechaI, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            DateTime.ParseExact(FechaF, "dd/MM/yyyy", CultureInfo.InvariantCulture),
            BLL.BLLEstadoCuenta.ObtenerAbonos(new DTO.DTOAlumno { AlumnoId = int.Parse(AlumnoId) },
                             DateTime.ParseExact(FechaI, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            DateTime.ParseExact(FechaF, "dd/MM/yyyy", CultureInfo.InvariantCulture))));
        }

        [Route("Api/Alumno/")]
        public void ActualizaPassword(string password, string token)
        {
            Universidad.BLL.BLLAlumno.ActualizaPassword(password, token);
        }

        [Route("Api/Alumno/InsertaBitacora/{alumnoId:int}")]
        [HttpGet]
        public void InsertaBitacora(int alumnoId)
        {
            Universidad.BLL.BLLAlumno.InsertaBitacora(alumnoId);
        }

        [Route("Api/Alumno/CalcularAnticipado/{alumnoId}")]
        [HttpGet]
        public IHttpActionResult CalcularAnticipado(string alumnoId)
        {
            return Ok(BLLPeriodo.SaberAnticipado(int.Parse(alumnoId)));
        }

        [Route("Api/Alumno/ConsultarAlumnoReinscripcion/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult ConsultarAlumnoReinscripcion(int AlumnoId)
        {
            return Ok(BLL.BLLAlumnoPortal.ObtenerAlumnoR(AlumnoId));
        }


        [Route("Api/Alumno/TraerSede/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult TraerSede(string AlumnoId)
        {
            return Ok(BLL.BLLSede.SedeAlumno(int.Parse(AlumnoId)));
        }

        [Route("Api/Alumno/VerificaAlumnoDatos/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult VerificaAlumnoDatos(string AlumnoId)
        {
            return Ok(BLL.BLLAlumnoPortal.VerificaAlumnoDatos(int.Parse(AlumnoId)));
        }

        [Route("Api/Alumno/VerificaAlumnoEncuesta/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult VerificaAlumnoEncuesta(string AlumnoId)
        {
            return Ok(BLL.BLLAlumnoPortal.VerificaAlumnoEncuesta(int.Parse(AlumnoId)));
        }

        [Route("Api/Alumno/ObenerDatosAlumnoActualiza/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult ObenerDatosAlumnoActualiza(string AlumnoId)
        {
            return Ok(BLL.BLLAlumnoPortal.ObenerDatosAlumnoActualiza(int.Parse(AlumnoId)));
        }

        [Route("Api/Alumno/UpdateAlumnoDatos")]
        [HttpPost]
        public IHttpActionResult UpdateAlumnoDatos([FromBody] DTO.DTOAlumnoDetalle AlumnoDatos)
        {
            return Ok(BLL.BLLAlumnoPortal.UpdateAlumnoDatos(AlumnoDatos));
        }

        [Route("Api/Alumno/PreguntasPortal")]
        [HttpGet]
        public IHttpActionResult PreguntasPortal()
        {
            return Ok(BLL.BLLAlumnoPortal.PreguntasPortal());
        }

        [Route("Api/Alumno/GuardarRespuestas")]
        [HttpPost]
        public IHttpActionResult GuardarRespuestas([FromBody] DTO.DTORespuestas RespuestasEncuesta)
        {
            return Ok(BLL.BLLAlumnoPortal.GuardarRespuestas(RespuestasEncuesta));
        }

        [Route("Api/Alumno/ConsultaPagosDetalle/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult ConsultaPagosDetalle(string AlumnoId)
        {
            try
            {
                return Ok(BLL.BLLPagoPortal.ReferenciasPago(int.Parse(AlumnoId)));
            }
            catch
            {
                return BadRequest("fallo");
            }
        }

        [Route("Api/Alumno/")]
        public IHttpActionResult ConsultarPeriodosAlumno(string AlumnoId)
        {
            return Ok(BLL.BLLPeriodoPortal.ConsultarPeriodos(int.Parse(AlumnoId)));
        }

        [Route("Api/Alumno/ConsultarAlumno/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult ConsultarAlumno(string AlumnoId)
        {
            return Ok(BLL.BLLAlumnoPortal.ObtenerAlumno(int.Parse(AlumnoId)));
        }

        [Route("Api/Alumno/ConsultarAdeudo/{AlumnoId:int}/{OfertaEducativaId}")]
        [HttpGet]
        public IHttpActionResult ConsultarAdeudo(int AlumnoId, int OfertaEducativaId)
        {
            return Ok(BLL.BLLPagoPortal.TraerAdeudos(AlumnoId, OfertaEducativaId));
        }

        [Route("Api/Alumno/ConsultarReferenciasCP/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult ConsultarReferenciasCP(int AlumnoId)
        {
            return Ok(BLL.BLLPagoPortal.ConsultarReferenciasConceptos(AlumnoId));
        }
    }
}
