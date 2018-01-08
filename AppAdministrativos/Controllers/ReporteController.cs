using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    public class ReporteController : ApiController
    {
        [Route("api/Reporte/CargarReporteVoBo/{anio:int}/{periodoid:int}/{usuarioid:int}")]
        [HttpGet]
        public IHttpActionResult CargarReporteVoBo(int anio, int periodoid, int usuarioid) => Ok(BLL.BLLReportePortal.CargarReporteVoBo(anio, periodoid, usuarioid));

        [Route("api/Reporte/CargarCuatrimestre")]
        [HttpGet]
        public IHttpActionResult CargarCuatrimestre() => Ok(BLL.BLLReportePortal.CargarCuatrimestre());
    }
}
