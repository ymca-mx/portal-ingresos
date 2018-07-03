using Newtonsoft.Json.Linq;
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
    [RoutePrefix("Api/Alumno")]
    public class AlumnoController : ApiController
    {

        [Route("Datos/{alumnoId:int}")]
        [HttpGet]
        public IHttpActionResult Datos(int alumnoId)
        {
            return Ok(Universidad.BLL.BLLAlumno.ImagenIndex(alumnoId));
        }

        [Route("EstadoDeCuenta/{AlumnoId}")]
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

        [Route("ActualizaPassword")]
        [HttpPost]
        public IHttpActionResult ActualizaPassword([FromBody]JObject jObject)
        {
            try
            {
                Universidad.BLL.BLLAlumno.ActualizaPassword(
                   (string)jObject["password"],
                   (string)jObject["token"]);

                return Ok("Acutalizado Correctamente");
            }catch(Exception err)
            {
                return BadRequest("Fallo " + err.Message);
            }
        }

        [Route("InsertaBitacora/{alumnoId:int}")]
        [HttpGet]
        public void InsertaBitacora(int alumnoId)
        {
            Universidad.BLL.BLLAlumno.InsertaBitacora(alumnoId);
        }

        [Route("CalcularAnticipado/{alumnoId}")]
        [HttpGet]
        public IHttpActionResult CalcularAnticipado(string alumnoId)
        {
            return Ok(BLLPeriodo.SaberAnticipado(int.Parse(alumnoId)));
        }

        [Route("ConsultarAlumnoReinscripcion/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetReinscripcion(int AlumnoId)
        {
            var Result = BLL.BLLAlumnoPortal.ObtenerAlumnoR(AlumnoId);

            if (Result == null)
            {
                return NotFound();
            }
            else if ((bool)Result.GetType().GetProperty("Status").GetValue(Result, null))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo: " + Result.GetType().GetProperty("Message").GetValue(Result, null) + " \n"
                    + "Detalle: " + Result.GetType().GetProperty("Inner").GetValue(Result, null));
            }
        }


        [Route("TraerSede/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult TraerSede(string AlumnoId)
        {
            return Ok(BLL.BLLSede.SedeAlumno(int.Parse(AlumnoId)));
        }

        [Route("VerificaAlumnoDatos/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult VerificaAlumnoDatos(string AlumnoId)
        {
            return Ok(BLL.BLLAlumnoPortal.VerificaAlumnoDatos(int.Parse(AlumnoId)));
        }

        [Route("VerificaAlumnoEncuesta/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult VerificaAlumnoEncuesta(string AlumnoId)
        {
            return Ok(BLL.BLLAlumnoPortal.VerificaAlumnoEncuesta(int.Parse(AlumnoId)));
        }

        [Route("ObenerDatosAlumnoActualiza/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult ObenerDatosAlumnoActualiza(string AlumnoId)
        {
            return Ok(BLL.BLLAlumnoPortal.ObenerDatosAlumnoActualiza(int.Parse(AlumnoId)));
        }

        [Route("UpdateAlumnoDatos")]
        [HttpPost]
        public IHttpActionResult UpdateAlumnoDatos([FromBody] DTO.DTOAlumnoDetalle AlumnoDatos)
        {
            return Ok(BLL.BLLAlumnoPortal.UpdateAlumnoDatos(AlumnoDatos));
        }

        [Route("PreguntasPortal")]
        [HttpGet]
        public IHttpActionResult PreguntasPortal()
        {
            return Ok(BLL.BLLAlumnoPortal.PreguntasPortal());
        }

        [Route("GuardarRespuestas")]
        [HttpPost]
        public IHttpActionResult GuardarRespuestas([FromBody] DTO.DTORespuestas RespuestasEncuesta)
        {
            return Ok(BLL.BLLAlumnoPortal.GuardarRespuestas(RespuestasEncuesta));
        }

        [Route("ConsultaPagosDetalle/{AlumnoId}")]
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

        //[Route("")]
        //[HttpGet]
        //public IHttpActionResult ConsultarPeriodosAlumno(string AlumnoId)
        //{
        //    return Ok(BLL.BLLPeriodoPortal.ConsultarPeriodos(int.Parse(AlumnoId)));
        //}

        [Route("ConsultarAlumno/{AlumnoId}")]
        [HttpGet]
        public IHttpActionResult ConsultarAlumno(string AlumnoId)
        {
            return Ok(BLL.BLLAlumnoPortal.ObtenerAlumno(int.Parse(AlumnoId)));
        }

        [Route("ConsultarAdeudo/{AlumnoId:int}/{OfertaEducativaId}")]
        [HttpGet]
        public IHttpActionResult ConsultarAdeudo(int AlumnoId, int OfertaEducativaId)
        {
            return Ok(BLL.BLLPagoPortal.TraerAdeudos(AlumnoId, OfertaEducativaId));
        }

        [Route("ConsultarReferenciasCP/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult ConsultarReferenciasCP(int AlumnoId)
        {
            return Ok(BLL.BLLPagoPortal.ConsultarReferenciasConceptos(AlumnoId));
        }
    }
}
