using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Pago")]
    public class PagoController : ApiController
    {

        [Route("ReferenciasSemestrales/{AlumnoId:int}/{OfertaEducativaId:int}")]
        [HttpGet]
        public IHttpActionResult ReferenciasSemestrales(int AlumnoId, int OfertaEducativaId)
        {
            return
                Ok(BLL.BLLPagoPortal.BuscarPagosActuales(AlumnoId, OfertaEducativaId));
        }

        [Route("ConsultaPagoDetalle/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetPagosAlumno(int AlumnoId)
        {
            try
            {
               return Ok(BLL.BLLPagoPortal.ReferenciasPago(AlumnoId));
            }
            catch(Exception Error)
            {
                return BadRequest(Error.Message);
            }
        }
    }
}
