using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebServices.WS
{
    /// <summary>
    /// Descripción breve de OperacionesPerfiles
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class OperacionesPerfiles : System.Web.Services.WebService
    {

        //[WebMethod]
        //public List<DTOPerfiles> ConsultarTodos()
        //{
        //    return BLLPerfil.ConsultarTodos();
        //}
        //[WebMethod]
        //public string GuardarPerfil(string Nombre, string Fecha)
        //{
        //    return BLLPerfil.GuardarPerfil(new DTOPerfiles
        //    {
        //        Descripcion = Nombre,
        //        FechaAlta = DateTime.ParseExact((Fecha.Replace('-', '/')), "dd/MM/yyyy", CultureInfo.InvariantCulture)
        //    }).ToString();
        //}
        //[WebMethod]
        //public string ModificarPerfil(string PerfilId, string Nombre, string Fecha)
        //{
        //    return BLLPerfil.ModificarPerfil(new DTOPerfiles
        //    {
        //        Descripcion = Nombre,
        //        FechaAlta = DateTime.ParseExact((Fecha.Replace('-', '/')), "dd/MM/yyyy", CultureInfo.InvariantCulture),
        //        PerfilId = int.Parse(PerfilId)
        //    });
        //}
        //[WebMethod]
        //public string EliminarPerfil(string PerfilId)
        //{
        //    return BLLPerfil.EliminarPerfil(int.Parse(PerfilId));
        //}
    }
}
