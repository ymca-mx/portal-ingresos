using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/OfertaEducativa")]
    public class OfertaEducativaController : ApiController
    {
        [Route("Costos/{Anio:int}/{PeriodoId:int}/{OfertaEducativaId:int}")]
        [HttpGet]
        public IHttpActionResult GetCosto(int Anio, int PeriodoId, int OfertaEducativaId)
        {
            var Result = BLL.BLLOfertaEducativaTipo.GetCostos(Anio, PeriodoId, OfertaEducativaId);

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }

        }
        
    }
}
