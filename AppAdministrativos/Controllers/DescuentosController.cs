using BLL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Descuentos")]
    public class DescuentosController : ApiController
    {
        [Route("TraerDescuentos/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult GetDescuentos(int AlumnoId)
        {
            var Result = BLLAlumnoDescuento.TraerDescuentos(AlumnoId);
            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("GuardarDescuentos")]
        [HttpPost]
        public IHttpActionResult GuardarDescuentos()
        {

            var objAlumno = JObject.Parse(HttpContext.Current.Request.Form["ObjAlumno"]);
            var alumno2 = objAlumno.ToObject<DTO.DTODescuentoAlumno>();

            Stream DocBeca = HttpContext.Current.Request.Files["DocBeca"].InputStream;
            Stream DocInscipcion = HttpContext.Current.Request.Files["DocInscipcion"].InputStream;
            Stream DocExamen = HttpContext.Current.Request.Files["DocExamen"].InputStream;

            alumno2.Descuentos.Find(a => a.PagoConceptoId == 800).Comprobante = Herramientas.ConvertidorT.ConvertirStream(DocBeca,
                HttpContext.Current.Request.Files["DocBeca"].ContentLength);

            alumno2.Descuentos.Find(a => a.PagoConceptoId == 802).Comprobante = Herramientas.ConvertidorT.ConvertirStream(DocInscipcion,
                HttpContext.Current.Request.Files["DocInscipcion"].ContentLength);

            alumno2.Descuentos.Find(a => a.PagoConceptoId == 1000).Comprobante = Herramientas.ConvertidorT.ConvertirStream(DocExamen,
                HttpContext.Current.Request.Files["DocExamen"].ContentLength);

            var Result = BLLAlumnoInscrito.GuardarDescuentosNuevoIngreso(alumno2);

            if ((int)Result.GetType().GetProperty("AlumnoId").GetValue(Result, null) > 0)
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }
    }
}
