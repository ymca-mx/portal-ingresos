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
            return Ok(BLLEntidadFederativa.ConsultarEstadosCiviles());
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

        [Route("Api/General/Conceptos/{AlumnoId}/{OfertaEducativa}")]
        [HttpGet]
        public IHttpActionResult Conceptos(string AlumnoId, string OfertaEducativa)
        {
            return Ok(BLLPagoConcepto.ListaPagoConceptos(int.Parse(AlumnoId), int.Parse(OfertaEducativa)));
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
