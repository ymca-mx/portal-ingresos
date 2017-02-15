using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Universidad.DTO.Usuario;
using Universidad.DTO;
using Universidad.BLL;

namespace UI.Services
{
    /// <summary>
    /// Descripción breve de Login
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Login : System.Web.Services.WebService
    {
        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public DTOLogin Valida(int username, string password)
        {
            return Universidad.BLL.BLLUsuario.LoginAdministrativo(username, password);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public DTOUsuarioImagen Datos(int username)
        {
            return Universidad.BLL.BLLUsuario.ImagenIndex(username);
        }
    }
}
