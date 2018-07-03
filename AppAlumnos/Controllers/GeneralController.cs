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
    public class GeneralController : ApiController
    {
        [Route("Api/General/ConsultarEntidadFederativa")]
        [HttpGet]
        public IHttpActionResult ConsultarEntidadFederativa()
        {
            return Ok(BLLEntidadFederativa.ConsultarEntidadFederativa());
        }

        [Route("Api/General/NombreCalendario/{Alumno}")]
        [HttpGet]
        public IHttpActionResult NombreCalendario(string Alumno)
        {
            return Ok(BLLAlumnoInscrito.NombreCalendario(int.Parse(Alumno)));

        }

        [Route("Api/General/ConsultarMunicipios/{EntidadFederativaId:int}")]
        [HttpGet]
        public IHttpActionResult ConsultarMunicipios(int EntidadFederativaId)
        {
            return Ok(BLLMunicipio.ConsultarMunicipios(EntidadFederativaId));
        }


        [Route("Api/General/ConsultarPaises")]
        [HttpGet]
        public IHttpActionResult ConsultarPaises()
        {
            return Ok(BLLPais.TraerPaises());
        }

        [Route("Api/General/Ofertas_costos_Alumno/{AlumnoId}/{Anio}/{PeriodoId}")]
        [HttpGet]
        public IHttpActionResult Ofertas_costos_Alumno(string AlumnoId, string Anio, string PeriodoId)
        {
            return Ok(BLLCuota.TraerOfertasCuotasAlumno(int.Parse(AlumnoId), int.Parse(Anio), int.Parse(PeriodoId)));
        }

        [Route("Api/General/Conceptos/{OfertaEducativaId:int}")]
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

        [Route("Api/General/ConsultarPagoConcepto/{OfertaEducativaId:int}/{PagoConceptoId:int}")]
        public IHttpActionResult ConsultarPagoConcepto(int OfertaEducativaId, int PagoConceptoId)
        {
            return Ok(BLLPagoConcepto.TraerPagoConcepto(OfertaEducativaId, PagoConceptoId));
        }


        [Route("Api/General/ConsultarGenero")]
        [HttpGet]
        public IHttpActionResult ConsultarGenero()
        {
            return Ok(BLLGenero.ConsultaTodosGenero());
        }

        [Route("Api/General/ConsultarEstadoCivil")]
        [HttpGet]
        public IHttpActionResult ConsultarEstadoCivil()
        {
            return Ok(BLLEstadoCivil.ConsultarEstadosCiviles());
        }
    }
}
