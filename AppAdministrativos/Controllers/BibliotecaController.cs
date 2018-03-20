using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Biblioteca")]
    public class BibliotecaController : ApiController
    {
        [Route("SendComunicado")]
        [HttpPost]
        public IHttpActionResult EnviarComunicado()
        {
         //   BLL.BLLBiblioteca.SendComunicado()
            return Ok("Procesando");
        }
    }
}
