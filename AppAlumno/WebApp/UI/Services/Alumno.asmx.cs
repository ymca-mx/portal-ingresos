using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Universidad.BLL;
using Universidad.DTO;
using Universidad.DTO.Alumno;

namespace UI.Services
{
    /// <summary>
    /// Descripción breve de Alumno
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Alumno : System.Web.Services.WebService
    {

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public DTOAlumnoDatosGenerales Valida(int alumnoId, string password)
        {
            return Universidad.BLL.BLLAlumno.LoginAcademico(alumnoId, password);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public DTOAlumnoImagen Datos(int alumnoId)
        {
            return Universidad.BLL.BLLAlumno.ImagenIndex(alumnoId);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void RecuperaPassword(string email)
        {
            Universidad.BLL.BLLAlumno.RecuperaPassword(email);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void ActualizaPassword(string password, string token)
        {
            Universidad.BLL.BLLAlumno.ActualizaPassword(password, token);
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertaBitacora(int alumnoId)
        {
            Universidad.BLL.BLLAlumno.InsertaBitacora(alumnoId);
        }
        [WebMethod]
        public string[] CalcularAnticipado(string alumnoId)
        {
            return BLLPeriodo.SaberAnticipado(int.Parse(alumnoId));
        }
    }
}
