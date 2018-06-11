using BLL;
using DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAlumnos.Controllers
{
    public class ReinscripcionController : ApiController
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");

        [Route("Api/Reinscripcion/GenerarInscrCole")]
        [HttpPost]
        public IHttpActionResult GenerarInscrCole([FromBody] JObject jObjectAlumno)
        {
            var Result = BLLPagoPortal.GenerarInscripcionColegiatura((int)jObjectAlumno["AlumnoId"],
                    (int)jObjectAlumno["OfertaEducativaId"]);

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(((List<string>)Result)[0]);
            }
            else
            {
                return BadRequest("Fallo: " + Result.GetType().GetProperty("Message").GetValue(Result, null) + " \n"
                     + "Detalle: " + Result.GetType().GetProperty("Inner").GetValue(Result, null));
            }
        }

        [Route("Api/Reinscripcion/ConsultarPagodeMes/{AlumnoId:int}/{OfertaEducativaId:int}")]
        [HttpGet]
        public IHttpActionResult ConsultarPagodeMes(int AlumnoId, int OfertaEducativaId)
        {
            try { return Ok(BLLPagoPortal.EsteMesvsSiguiente(AlumnoId, OfertaEducativaId, DateTime.Now)); }
            catch {return BadRequest("Fallo"); }
        }

        [Route("Api/Reinscripcion/ConsultarPagosPeriodo/{AlumnoId:int}/{OfertaEducativaId:int}")]
        [HttpGet]
        public IHttpActionResult ConsultarPagosPeriodo(int AlumnoId, int OfertaEducativaId)
        {
            try
            { return Ok(BLLPagoPortal.BuscarPagoIngles(AlumnoId, OfertaEducativaId)); }
            catch (Exception err) { return BadRequest(err.Message); }
        }

        [Route("Api/Reinscripcion/GenerarColegiaturaIngles")]
        [HttpPost]
        public IHttpActionResult GenerarColegiaturaIngles([FromBody] JObject jObjectAlumno)
        {
            try
            {
                DateTime fhoy = DateTime.Now;

                fhoy = fhoy.Month == (int)jObjectAlumno["MesId"] ? DateTime.Now :
                    DateTime.ParseExact("01/" +
                    ((int)jObjectAlumno["MesId"] < 10 ? ("0" + (string)jObjectAlumno["MesId"])
                    : (string)jObjectAlumno["MesId"]) + " / " + fhoy.Year.ToString(), "dd/MM/yyyy", Cultura);

                return Ok(BLLPagoPortal.GenerarInscripcionColegiatura(
                    (int)jObjectAlumno["AlumnoId"],
                    (int)jObjectAlumno["OfertaEducativaId"], fhoy));
            }
            catch(Exception err)
            {
                return BadRequest("Fallo " + err.Message);
            }
        }

        [Route("Api/Reinscripcion/Pendiente/{AlumnoId:int}/{OfertaEducativaId:int}")]
        [HttpGet]
        public IHttpActionResult Pendiente(int AlumnoId, int OfertaEducativaId)
        {
            try
            { return Ok(BLLPagoPortal.MesPendiente(AlumnoId, OfertaEducativaId)); }
            catch (Exception err) { return BadRequest("Fallo " + err.Message); }
        }

        [Route("Api/Reinscripcion/InscribirGenerar")]
        [HttpPost]
        public string InscribirGenerar([FromBody] JObject jObjectAlumno)
        {
            DTOPeriodo PeriodoActual = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);
            string Menms = BLLPagoPortal.GenerarInscripcionColegiatura(
                (int)jObjectAlumno["AlumnoId"],
                (int)jObjectAlumno["OfertaEducativaId"],
                PeriodoActual.Anio,
                PeriodoActual.PeriodoId); 

            if (Menms == "Guardado")
            {
                bool resp = false;// BLL.BLLAlumno.AplicaBecaAlumno(objBeca);
                if (resp)
                {
                    BLL.BLLSaldoAFavor.AplicacionSaldoAlumno(
                        (int)jObjectAlumno["AlumnoId"], 
                        true, 
                        false);

                    return "Guardado";
                }
                else { return "Fallo"; }
            }
            else
            {
                return Menms;
            }
        }

        [Route("Api/Reinscripcion/")]
        public List<Flujo> ConsultarTabla(string AlumnoId, string OfertaEducativaId)
        {
            return BLLPagoPortal.FlujoReinscripcion(int.Parse(AlumnoId), int.Parse(OfertaEducativaId));
        }
    }
}
