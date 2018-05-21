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

        [Route("ConsultarPeriodosPCF")]
        [HttpGet]
        public IHttpActionResult GetPeriodosAn_Ac_Sg()
        {
            object Result = BLLPeriodoPortal.GetPeriodo_An_Ac_Sg();


            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("ConsultarPais")]
        [HttpGet]
        public IHttpActionResult GetPais()
        {
            return Ok(BLLPais.TraerPaises());
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

        [Route("GetDirectory")]
        [HttpGet]
        public IHttpActionResult GetDiractorios()
        {
            return Ok(BLLAlumnoCambio.GetRoot());
        }

        [Route("TraerListaMedios")]
        [HttpGet]
        public IHttpActionResult MediosDifucion()
        {
            return Ok(BLLMedioDifusion.ConsultarListadeMedios());
        }

        [Route("ConsultarParentesco")]
        [HttpGet]
        public IHttpActionResult GetParentesco()
        {
            return Ok(BLLParentesco.ConsultarTodosParentesco());
        }

        [Route("ObtenerAreas")]
        [HttpGet]
        public IHttpActionResult GetAreas()
        {
            return Ok(BLLAreaAcademica.AreasAcademicas());
        }

        [Route("SistemaPagoAlumno/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult SistemaPagoAlumno(int AlumnoId)
        {
            var Result = BLLPagoPlan.ConsultarPagos(AlumnoId);

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de trer datos, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("EnviarMail2")]
        [HttpPost]
        public IHttpActionResult SendMail(List<int> lstAlumnos)
        {
            var Result = BLLEmail.SendMultipleEmail(lstAlumnos);

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                if (((List<string>)Result).Count > 0)
                {
                    return BadRequest(String.Join("/ ", ((List<string>)Result).ToArray()));
                }
                else
                {
                    return Ok(new
                    {
                        Message = "Se envio el email correctamente"
                    });
                }
            }
            else
            {

                return BadRequest(String.Join("/ ", ((List<string>)Result).ToArray()));
            }
        }
        [Route("ConsultarPagosPlanLenguas/{TipoOfertaId:int}")]
        [HttpGet]
        public IHttpActionResult PlanPagoLenguas(int TipoOfertaId)
        {
            var Result = BLLPagoPlan.ConsultarPagosPlanLenguas(TipoOfertaId);

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de trer datos, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }

        }
    }
}
