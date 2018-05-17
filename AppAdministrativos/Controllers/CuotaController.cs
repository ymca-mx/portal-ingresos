using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Cuota")]
    public class CuotaController : ApiController
    {
        [Route("TraerCuotaPeriodo/{OFertaEducativaId:int}/{Anio:int}/{PeriodoId:int}")]
        [HttpGet]
        public IHttpActionResult GetCuotaByPeriodo(int OFertaEducativaId, int Anio, int PeriodoId)
        {
            var Result = BLLAlumnoDescuento.TraerCuotasOfertaEducativaPeriodo(OFertaEducativaId, Anio, PeriodoId);

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
