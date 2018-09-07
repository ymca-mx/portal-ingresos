using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAlumnos.Controllers
{
    [RoutePrefix("Api/General")]
    public class GeneralController : ApiController
    {
        [Route("ConsultarEntidadFederativa")]
        [HttpGet]
        public IHttpActionResult ConsultarEntidadFederativa()
        {
            return Ok(BLLEntidadFederativa.ConsultarEntidadFederativa());
        }

        [Route("NombreCalendario/{Alumno}")]
        [HttpGet]
        public IHttpActionResult NombreCalendario(string Alumno)
        {
            return Ok(BLLAlumnoInscrito.NombreCalendario(int.Parse(Alumno)));

        }

        [Route("ConsultarMunicipios/{EntidadFederativaId:int}")]
        [HttpGet]
        public IHttpActionResult ConsultarMunicipios(int EntidadFederativaId)
        {
            return Ok(BLLMunicipio.ConsultarMunicipios(EntidadFederativaId));
        }


        [Route("ConsultarPaises")]
        [HttpGet]
        public IHttpActionResult ConsultarPaises()
        {
            return Ok(BLLPais.TraerPaises());
        }

        [Route("Ofertas_costos_Alumno/{AlumnoId}/{Anio}/{PeriodoId}")]
        [HttpGet]
        public IHttpActionResult Ofertas_costos_Alumno(string AlumnoId, string Anio, string PeriodoId)
        {
            return Ok(BLLCuota.TraerOfertasCuotasAlumno(int.Parse(AlumnoId), int.Parse(Anio), int.Parse(PeriodoId)));
        }

        [Route("Conceptos/{OfertaEducativaId:int}")]
        [HttpGet]
        public IHttpActionResult Conceptos(int OfertaEducativaId)
        {
            var Result = BLLPagoConcepto.ListaPagoConceptos(OfertaEducativaId);

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de consultar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("Conceptos/{OfertaEducativaId:int}/Alumno")]
        [HttpGet]
        public IHttpActionResult ConceptosAlumno(int OfertaEducativaId)
        {
            var Result = BLLPagoConcepto.ListaPagoConceptosAlumno(OfertaEducativaId);

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de consultar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("ConsultarPagoConcepto/{OfertaEducativaId:int}/{PagoConceptoId:int}")]
        public IHttpActionResult ConsultarPagoConcepto(int OfertaEducativaId, int PagoConceptoId)
        {
            return Ok(BLLPagoConcepto.TraerPagoConcepto(OfertaEducativaId, PagoConceptoId));
        }


        [Route("ConsultarGenero")]
        [HttpGet]
        public IHttpActionResult ConsultarGenero()
        {
            return Ok(BLLGenero.ConsultaTodosGenero());
        }

        [Route("ConsultarEstadoCivil")]
        [HttpGet]
        public IHttpActionResult ConsultarEstadoCivil()
        {
            return Ok(BLLEstadoCivil.ConsultarEstadosCiviles());
        }
    }
}
