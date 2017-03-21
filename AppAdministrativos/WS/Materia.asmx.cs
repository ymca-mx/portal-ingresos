using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AppAdministrativos.WS
{
    /// <summary>
    /// Descripción breve de Materia
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Materia : System.Web.Services.WebService
    {

        [WebMethod]
        public List<DTOAsignatura> ListarAsignatura(string OfertaEducativaId)
        {
            return BLLAsignatura.ListarAsignatura(int.Parse(OfertaEducativaId));
        }
    }
}
