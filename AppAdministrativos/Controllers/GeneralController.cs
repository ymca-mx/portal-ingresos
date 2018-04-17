using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/General")]
    public class GeneralController : ApiController
    {
        [Route("ConsultarGenero")]
        [HttpGet]
        public IHttpActionResult GetGenero()
        {
            return Ok(BLLGenero.ConsultaTodosGenero());
        }

        [Route("ConsultarTurnos")]
        [HttpGet]
        public IHttpActionResult GetTurno()
        {
            return Ok(BLLTurno.ConsultarTurno());
        }

        [Route("Plantel")]
        [HttpGet]
        public IHttpActionResult ConsultarPlantel()
        {
            return Ok(BLLSucursal.ConsultarSucursales());
        }

        [Route("OFertaEducativaTipo/{PlantelId:int}")]
        [HttpGet]
        public IHttpActionResult GetOfertaEducativaTipo(int PlantelId)
        {
            return Ok(BLLOfertaEducativaTipo.ConsultaOfertaTipo(PlantelId));
        }

        [Route("OFertaEducativa/{PlantelId:int}/{TipoOfertaId:int}")]
        [HttpGet]
        public IHttpActionResult GetOfertasEducativas(int PlantelId, int TipoOfertaId)
        {
            return Ok(BLLOfertaEducativaPortal.ConsultarOfertasEducativas(TipoOfertaId, PlantelId));
        }

        [Route("ConsultarPeriodos")]
        [HttpGet]
        public IHttpActionResult GetPeriodosSiguientes()
        {
            return Ok(BLLPeriodoPortal.ConsultarPeriodos());
        }
    }
}
