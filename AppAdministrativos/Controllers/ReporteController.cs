using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("api/Reporte")]
    public class ReporteController : ApiController
    {
        [Route("CargarReporteVoBo/{anio:int}/{periodoid:int}/{usuarioid:int}")]
        [HttpGet]
        public IHttpActionResult CargarReporteVoBo(int anio, int periodoid, int usuarioid) => Ok(BLL.BLLReportePortal.CargarReporteVoBo(anio, periodoid, usuarioid));

        [Route("CargarCuatrimestre")]
        [HttpGet]
        public IHttpActionResult CargarCuatrimestre() => Ok(BLL.BLLReportePortal.CargarCuatrimestre());

        [Route("CargarReporteSaldos")]
        [HttpGet]
        public IHttpActionResult GetRptSaldos()
        {
            var Result=BLLReportePortal.CargarReporteSaldos();

            if (Result != null)
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo");
            }
        }

        [Route("ReporteSaldosAlumnos/{OfertaEducativaId:int}")]
        [HttpGet]
        public IHttpActionResult GetSaldoDetalleAlumnos(int OfertaEducativaId)
        {
            var Result = BLLReportePortal.CargarReporteSaldosOfertaEducativa(OfertaEducativaId);

            if (Result != null)
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al traer a los Alumnos.");
            }
        }
    }
}
