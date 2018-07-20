using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Administracion")]
    public class AdministracionController : ApiController
    {
        [Route("Usuarios/{UsuarioId:int}")]
        [HttpGet]
        public IHttpActionResult GetUSuarios(int UsuarioId)
        {
            var Result = BLL.BLLUsuarioPortal.TraerTodos(UsuarioId);

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

        [Route("Usuario")]
        [HttpPost]
        public IHttpActionResult UpdateUsuario(DTO.DTOUsuario objUsuario)
        {
            var Result = BLL.BLLUsuarioPortal.UpdateUsuario(objUsuario);

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

        [Route("Usuario/{UsuarioId:int}")]
        [HttpGet]
        public IHttpActionResult GetUsuario(int UsuarioId)
        {
            var Result = BLL.BLLUsuarioPortal.GetUsuario(UsuarioId);

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
