using BLL;
using DTO.Alumno.Beca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Beca")]
    public class BecaController : ApiController
    {
        [Route("ObtenerAlumno/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetAlumno(int AlumnoId)
        {
            var Result= BLLBeca.ObtenerAlumno(AlumnoId);


            if (((bool)Result.GetType().GetProperty("Status").GetValue(Result, null)))
            {
                return Ok(Result.GetType().GetProperty("Alumno").GetValue(Result, null));
            }
            else
            {
                return BadRequest("Fallo: " + Result.GetType().GetProperty("Message").GetValue(Result, null) + " \n"
                    + "Detalle: " + Result.GetType().GetProperty("Inner").GetValue(Result, null));
            }
        }

        [Route("BecaComite")]
        [HttpPost]
        public IHttpActionResult AddBecaComite(DTOAlumnoBeca AlumnoBeca)
        {
            string Respuesta =
            BLL.BLLBeca.VerificarInscripcionActual(AlumnoBeca.alumnoId, AlumnoBeca.ofertaEducativaId, AlumnoBeca.anio, AlumnoBeca.periodoId);

            if (Respuesta == "Procede")
            {
                try
                {
                    BLL.BLLAlumnoPortal.AplicaBeca(AlumnoBeca, false);
                    return Ok("Guardado");
                }
                catch { return BadRequest("Fallo"); }
            }
            else
            {
                return Ok(Respuesta);
            }
        }
    }
}
