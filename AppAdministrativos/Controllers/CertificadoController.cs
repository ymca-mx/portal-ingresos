using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Certificado")]
    public class CertificadoController : ApiController
    {
        [Route("Calificaciones/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetCalificaciones(int AlumnoId)
        {
            object Result = BLLCertificado.GetMateriaAlumno(AlumnoId);

            if ((bool)Result.GetType().GetProperty("Status").GetValue(Result, null))
            {
                return Ok(Result);
            }
            else
            {
                string message = "'Gral':'Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null)
                        + "', 'Detalle': '" + Result.GetType().GetProperty("Inner").GetValue(Result, null)
                        + "', 'Detalle Inner': '" + Result.GetType().GetProperty("Inner2").GetValue(Result, null).ToString().Replace("'", "\"") + "'";

                return BadRequest(message);
            }
        }

        [Route("Alumno/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetAlumno(int AlumnoId)
        {
            var Result = BLLCertificado.GetAlumno(AlumnoId);

            if (Result != null)
            {
                if ((bool)Result.GetType().GetProperty("Status").GetValue(Result, null))
                {
                    return Ok(Result);
                }
                else
                {
                    return BadRequest("Fallo: " + Result.GetType().GetProperty("Message").GetValue(Result, null) + " \n"
                        + "Detalle: " + Result.GetType().GetProperty("Inner").GetValue(Result, null));
                }
            }
            else { return NotFound(); }
        }
    }
}
