using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/SEP")]
    public class SEPController : ApiController
    {
        [Route("Alumno/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetAlumno(int AlumnoId)
        {
            var Result = BLLSEP.GetAlumno(AlumnoId);

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

        [Route("TipoEstudio")]
        [HttpGet]
        public IHttpActionResult GetTipoEstudio()
        {
            object Result = BLLSEP.GetTipoEstudioAntecedente();

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("AutRec")]
        [HttpGet]
        public IHttpActionResult GetAutReco()
        {
            object Result = BLLSEP.GetAutorizacionRec();

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("MedioTitulacion")]
        [HttpGet]
        public IHttpActionResult GetMedioTitulacion()
        {
            object Result = BLLSEP.GetMedioTitulacion();

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("Servicio")]
        [HttpGet]
        public IHttpActionResult GetServicioSocial()
        {
            object Result = BLLSEP.GetServicioSocial();

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("Responsable")]
        [HttpGet]
        public IHttpActionResult GetResponsables()
        {
            object Result = BLLSEP.GetResponsable();

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("Nuevo")]
        [HttpPut]
        public IHttpActionResult AddAlumnos(List<DTO.SEP.TituloGeneral> Alumnos)
        {
            object Result = BLLSEP.NewSolicitud(Alumnos);

            if ((bool)Result.GetType().GetProperty("Status").GetValue(Result, null))
            {
                return Ok(Result);
            }
            else
            {
                string message = "'Gral':'Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null)
                        + "', 'Detalle': '" + Result.GetType().GetProperty("Inner").GetValue(Result, null)
                        + "', 'Detalle Inner': '" + Result.GetType().GetProperty("Inner2").GetValue(Result, null).ToString().Replace("'","\"") + "'";

                return BadRequest(message);
            }
        }
    }
}
