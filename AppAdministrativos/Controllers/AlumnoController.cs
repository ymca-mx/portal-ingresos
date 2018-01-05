using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    public class AlumnoController : ApiController
    {

        [Route("api/alumno/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetAlumno(int AlumnoId) => Ok(BLL.BLLAlumnoPortal.GetAlumno(AlumnoId));

        [Route("api/alumno/{Alumno}")]
        [HttpGet]
        public IHttpActionResult GetAlumnoName(string Alumno) => Ok(BLL.BLLAlumnoPortal.GetAlumnos(Alumno));
    }
}
