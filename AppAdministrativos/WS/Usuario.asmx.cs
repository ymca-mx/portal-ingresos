using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Universidad.DTO;
using Universidad.DTO.Usuario;

namespace AppAdministrativos.WS
{
    /// <summary>
    /// Descripción breve de Usuario
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
     [System.Web.Script.Services.ScriptService]
    public class Usuario : System.Web.Services.WebService
    {

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void RecuperaPassword(string email)
        {
            Universidad.BLL.BLLUsuario.RecuperaPassword(email);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertaBitacora(int usuarioId)
        {
            Universidad.BLL.BLLUsuario.InsertaBitacora(usuarioId);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object Datos(int usuarioId)
        {
            return Universidad.BLL.BLLUsuario.ImagenIndex(usuarioId);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void ActualizaPassword(string password, string token)
        {
            Universidad.BLL.BLLUsuario.ActualizaPassword(password, token);
        }
        // Consulta de Pestañas para usuarios
        [WebMethod]
        public object ConsultarMenu(string usuarioId)
        {
            return Universidad.BLL.BLLUsuario.TraerMenu(int.Parse(usuarioId));
        }
    }
}
