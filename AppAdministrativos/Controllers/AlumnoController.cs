using BLL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Alumno")]
    public class AlumnoController : ApiController
    {

        [Route("{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetAlumno(int AlumnoId) => Ok(BLL.BLLAlumnoPortal.GetAlumno(AlumnoId));

        [Route("{Alumno}")]
        [HttpGet]
        public IHttpActionResult GetAlumnoName(string Alumno) => Ok(BLL.BLLAlumnoPortal.GetAlumnos(Alumno));

        [Route("ConsultarAlumno/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetDatosAlumno(int AlumnoId) => Ok(BLLAlumnoPortal.ObtenerAlumno(AlumnoId));

        [Route("ConsultarAlumno/{AlumnoId:int}/basic")]
        [HttpGet]
        public IHttpActionResult GetDatosAlumnoAnonym(int AlumnoId)
        {
            var Result= BLLAlumnoPortal.ObtenerAlumnoAnon(AlumnoId);
            if (Result != null) {
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

        [Route("ConsultarAlumnosNuevos")]
        [HttpGet]
        public IHttpActionResult GetAlumnosNuevoIngreso()
        {
            return Ok(BLLAlumnoPortal.ConsultarAlumnosNuevos());
        }

        [Route("ConsultarAlumnosNuevosRP")]
        [HttpGet]
        public IHttpActionResult GetAlumnosNuevoIngresoRP()
        {
            object Result = BLLAlumnoPortal.NuevoIngreso();


            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("Academicos/{AlumnoId:int}/{OfertaEducativaId:int}")]
        [HttpGet]
        public IHttpActionResult GetDatosAcademicos(int AlumnoId, int OfertaEducativaId)
        {
            return Ok(BLLAlumnoPortal.AlumnoDatosAcademicos(AlumnoId, OfertaEducativaId));
        }

        [Route("ChageOffer")]
        [HttpPost]
        public IHttpActionResult CambiarOfertaEducativa(DTO.DTOAlumnoOfertaCuotas alumno)
        {
            var Result = BLL.BLLAlumnoCambio.CambioGnral(alumno);

            if (((bool)Result.GetType().GetProperty("Status").GetValue(Result, null)))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo: " + Result.GetType().GetProperty("Message").GetValue(Result, null) + " \n"
                    + "Detalle: " + Result.GetType().GetProperty("Inner").GetValue(Result, null));
            }
        }

        [Route("BuscarAlumnoString/{Filtro}")]
        [HttpGet]
        public IHttpActionResult BuscarAlumnoString(string Filtro)
        {
            return Ok(BLLAlumnoPortal.BuscarAlumnoTexto(Filtro));
        }

        [Route("BuscarAlumno")]
        [HttpPost]
        public IHttpActionResult GetHomonimos(DTO.DTOAlumno alumno)
        {
            var Result = BLLAlumnoPortal.ListarAlumnos(alumno.Nombre, alumno.Paterno, alumno.Materno);
            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        //Cambio turno
        [Route("ConsultaCambioTurno/{AlumnoId:int}/{UsuarioId:int}")]
        [HttpGet]
        public IHttpActionResult ConsultaCambioTurno(int AlumnoId, int UsuarioId)
        {
            return Ok(BLLAlumnoPortal.ConsultaCambioTurno(AlumnoId, UsuarioId));
        }

        [Route("AplicarCambioTurno")]
        [HttpPost]
        public IHttpActionResult AplicarCambioTurno(DTO.DTOAlumnoCambioTurno Cambio)
        {
            return Ok(BLLAlumnoPortal.AplicarCambioTurno(Cambio));
        }

        //  actualizar datos personales por el coordinador

        [Route("ObenerDatosAlumnoCordinador/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult ObenerDatosAlumnoCordinador(int AlumnoId)
        {
            return Ok(BLLAlumnoPortal.ObenerDatosAlumnoCordinador(AlumnoId));
        }

        [Route("UpdateAlumnoDatosCoordinador")]
        [HttpPost]
        public IHttpActionResult UpdateAlumnoDatosCoordinador(DTO.DTOAlumnoDetalle AlumnoDatos)
        {
            return Ok(BLLAlumnoPortal.UpdateAlumnoDatosCoordinador(AlumnoDatos));
        }

        [Route("UpdateAlumno")]
        [HttpPost]
        public IHttpActionResult UpdateAlumno(DTO.DTOAlumno Alumno)
        {
            Alumno.DTOAlumnoDetalle.FechaNacimiento = DateTime.ParseExact((Alumno.DTOAlumnoDetalle.FechaNacimientoC.Replace('-', '/')), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return Ok(BLLAlumnoPortal.UpdateAlumno(Alumno, Alumno.UsuarioId));
        }

        [Route("ObtenerDatosAlumno/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult ObtenerDatosAlumno(int AlumnoId)
        {
            return Ok(BLLAlumnoPortal.ObtenerAlumnoCompleto(AlumnoId));
        }
        [Route("ObenerDatosAlumnoTodos/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetDatosAlumnoTodos(int AlumnoId)
        {
            return Ok(BLLAlumnoPortal.ObenerDatosAlumnoTodos(AlumnoId));
        }

        [Route("ConsultarAlumnos")]
        [HttpGet]
        public IHttpActionResult ConsultarAlumnos()
        {
            var Result = BLLAlumnoPortal.ListarAlumnos();
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
        public IHttpActionResult AddAlumno(DTO.DTOAlumno Alumno)
        {
            var Result = BLLAlumnoPortal.InsertarAlumno(Alumno);

            if (Result is string)
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("ConsultarAlumnoPromocionCasa2/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetPromocionCasa(int AlumnoId)
        {
            var Result = BLLAlumnoPortal.ConsultarAlumnoPromocionCasa2(AlumnoId);

            if ((Result is DTO.DTOAlumnoPromocionCasa) || (Result == null))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("GuardarPromocionCasa")]
        [HttpPost]
        public IHttpActionResult GuardarPromocionCasa(DTO.DTOAlumnoPromocionCasa Promocion)
        {
            var Result= BLLAlumnoPortal.GuardarPromocionCasa(Promocion);
            if ((bool)Result)
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("NuevaOferta")]
        [HttpPut]
        public IHttpActionResult AddNuevaOferta(DTO.DTOAlumno objAlumno)
        {
            var Result = BLLAlumnoInscrito.InscribirNuevaOferta(objAlumno);

            if ((bool)Result.GetType().GetProperty("Estatus").GetValue(Result, null))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("GuardarAntecedentes")]
        [HttpPut]
        public IHttpActionResult AddAntecedente(DTO.DTOAlumnoAntecendente AlumnoAntecedente)
        {
            var Result= BLLAntecedente.GuardarAntecendente(AlumnoAntecedente);
            if ((bool)Result.GetType().GetProperty("Estatus").GetValue(Result, null))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("TraerAlumnoDetalle/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult TraerAlumnoDetalle(int AlumnoId)
        {
            var Result = BllAlumnoDetalle.GetAlumnoDetalle(AlumnoId);
            if ((bool)Result.GetType().GetProperty("Estatus").GetValue(Result, null))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("UpdateMail")]
        [HttpPost]
        public IHttpActionResult UpdateMail(JObject objAlumno)
        {

            var Result = BllAlumnoDetalle.UpdateEmail(int.Parse(objAlumno["AlumnoId"].ToString()), int.Parse(objAlumno["UsuarioId"].ToString()), objAlumno["Email"].ToString());

            if ((bool)Result.GetType().GetProperty("Estatus").GetValue(Result, null))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }
    }
}
