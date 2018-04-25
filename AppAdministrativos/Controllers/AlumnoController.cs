using BLL;
using System;
using System.Collections.Generic;
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

        [Route("ConsultarAlumnosNuevos")]
        [HttpGet]
        public IHttpActionResult GetAlumnosNuevoIngreso()
        {
            return Ok(BLLAlumnoPortal.ConsultarAlumnosNuevos());
        }

        [Route("Academicos/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetDatosAcademicos(int AlumnoId)
        {
            return Ok(BLLAlumnoPortal.AlumnoDatosAcademicos(AlumnoId));
        }

        [Route("ChageOffer")]
        [HttpPost]
        public IHttpActionResult CambiarOfertaEducativa(DTO.DTOAlumnoOfertaCuotas alumno)
        {
            return Ok(BLL.BLLAlumnoCambio.CambioGnral(alumno));
        }
    }
}
