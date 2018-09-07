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

        [Route("InsertaBitacora")]
        [HttpPost] 
        public IHttpActionResult AddBitacora(DTO.DTOUsuario objUser)
        {
            try
            {
                Universidad.BLL.BLLUsuario.InsertaBitacora(objUser.UsuarioId);
                return Ok(new
                {
                    Status = true,
                    message = "Guardado correctamente"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error.Message + " \n " + (error?.InnerException?.Message ?? ""));
            }
        }

        [Route("ConsultarMenu/{UsuarioId:int}")]
        [HttpGet]
        public IHttpActionResult GetMenu(int UsuarioId)
        {            
            var Result = Universidad.BLL.BLLUsuario.TraerMenu(UsuarioId);

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

        [Route("Datos/{UsuarioId:int}")]
        [HttpGet]
        public IHttpActionResult GetUser(int UsuarioId)
        {            
            var Result = Universidad.BLL.BLLUsuario.ImagenIndex(UsuarioId);

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
