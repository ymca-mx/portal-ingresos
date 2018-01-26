using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAlumnos.Controllers
{
    public class LoginController : ApiController
    {
        [Route("Api/Login/Valida")]
        [HttpPost]
        public IHttpActionResult Valida([FromBody]  DTO.DTOAlumno alumno)
        {
            return Ok(Universidad.BLL.BLLAlumno.LoginAcademico(alumno.AlumnoId, alumno.Password));
        }

        [Route("Api/Login/RecuperaPassword")]
        [HttpPost]
        public IHttpActionResult RecuperaPassword([FromBody] string Email)
        {
            //string email = "j053_pepe@outlook.com";
            try
            {
                Universidad.BLL.BLLAlumno.RecuperaPassword(Email);
                return Ok("Todo Correcto");
            }
            catch { return BadRequest("Fallo"); }
        }
    }
}
