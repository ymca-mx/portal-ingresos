using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    public class PagoController : ApiController
    {

        [Route("api/Pago/ReferenciasSemestrales/{AlumnoId:int}/{OfertaEducativaId:int}")]
        [HttpGet]
        public IHttpActionResult ReferenciasSemestrales(int AlumnoId, int OfertaEducativaId)
        {
            return
                Ok(BLL.BLLPagoPortal.BuscarPagosActuales(AlumnoId, OfertaEducativaId));
        }
    }
}
