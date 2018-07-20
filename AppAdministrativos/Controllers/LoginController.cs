using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Login")]
    public class LoginController : ApiController
    {
        [Route("Valida")]
        [HttpPost]
        public IHttpActionResult Login(DTOUsuario Usuario)
        {
            var Result = Universidad.BLL.BLLUsuario.LoginAdministrativo(Usuario.UsuarioId, Usuario.Password);

            if (Result == null)
                return NotFound();

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
    }
}
