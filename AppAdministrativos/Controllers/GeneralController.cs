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

        [Route("ConsultarPais")]
        [HttpGet]
        public IHttpActionResult GetPais()
        {
            return Ok(BLLPeriodoPortal.ConsultarPeriodos());
        }

        [Route("ConsultarEntidadFederativa")]
        [HttpGet]
        public IHttpActionResult GetEntidadFederativa()
        {
            return Ok(BLLEntidadFederativa.ConsultarEntidadFederativa());
        }

        [Route("ConsultarMunicipios/{EntidadFederativaId:int}")]
        [HttpGet]
        public IHttpActionResult GetMunicipios(int EntidadFederativaId)
        {
            return Ok(BLLMunicipio.ConsultarMunicipios(EntidadFederativaId));
        }

        [Route("ConsultarEstadoCivil")]
        [HttpGet]
        public IHttpActionResult GetEstadoCivil()
        {
            return Ok(BLLEstadoCivil.ConsultarEstadosCiviles());
        }
    }
}
