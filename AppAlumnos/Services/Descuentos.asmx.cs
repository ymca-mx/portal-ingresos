using BLL;
using DTO;
//using Herramientas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
//using Utilities;

namespace AppAlumnos.Services
{
    /// <summary>
    /// Descripción breve de Descuentos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Descuentos : System.Web.Services.WebService
    {
        [WebMethod]
        public string ConsultarAdeudo(string AlumnoId)
        {
            return BLLPagoPortal.ConsultarAdeudo(int.Parse(AlumnoId));
        }

        [WebMethod]
        public DTOPagos GenerarPago(string AlumnoId, string OfertaEducativaId, string PagoConceptoId, string CuotaId)
        {
            return BLLPagoPortal.GenerarPago(int.Parse(AlumnoId), int.Parse(OfertaEducativaId), int.Parse(PagoConceptoId), int.Parse(CuotaId));            
        }

    }
}
