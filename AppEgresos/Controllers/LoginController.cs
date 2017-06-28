using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilities;
using DAL;
using System.Web.Http.Description;

namespace AppEgresos.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        [ActionName("Valida")]
        public IHttpActionResult Valida([FromBody] DTOCredenciales credenciales)
        {

            credenciales.password = Utilities.Seguridad.Encripta(27, credenciales.password);
            using (UniversidadEntities db = new UniversidadEntities())
            {

                return Ok((from a in db.Usuario
                        where a.UsuarioId == credenciales.username
                        && a.Password == credenciales.password
                        && a.EstatusId == 1
                        select new DTOLogin
                        {
                            usuarioId = a.UsuarioId
                        }).FirstOrDefault());

            }
            
        }

    }
}
