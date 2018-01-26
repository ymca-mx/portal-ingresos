using BLL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAlumnos.Controllers
{
    public class DescuentosController : ApiController
    {
        [Route("Api/Descuentos/ConsultarAdeudo/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult ConsultarAdeudo(int AlumnoId)
        {
            return Ok(BLLPagoPortal.ConsultarAdeudo(AlumnoId));
        }

        [Route("Api/Descuentos/GenerarPago")]
        [HttpPost]
        public IHttpActionResult GenerarPago([FromBody] JObject objCuota)
        {
            return Ok(BLLPagoPortal.GenerarPago((int)objCuota["AlumnoId"],
                (int)objCuota["OfertaEducativaId"],
                (int)objCuota["PagoConceptoId"],
                (int)objCuota["CuotaId"]));
        }
    }
}
