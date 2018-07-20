using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Usuario")]
    public class UsuarioController : ApiController
    {
        [Route("RecuperaPassword")]
        [HttpPost]
        public IHttpActionResult RecoveryPass(JObject objEmail)
        {
            var Result = Universidad.BLL.BLLUsuario.RecuperaPassword(objEmail["Email"].ToString());

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

        [Route("ActualizaPassword")]
        [HttpPost]
        public IHttpActionResult UpdatePass(JObject objPass)
        {
            try
            {
                Universidad.BLL.BLLUsuario.ActualizaPassword(objPass["password"].ToString(), objPass["token"].ToString());
                return Ok("Se actualizo correctamente.");
            }
            catch
            {
                return BadRequest("Fallo la actualización.");
            }
        }
    }
}
