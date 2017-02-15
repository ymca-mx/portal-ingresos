using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DTO;

namespace WebServices.WS
{
    /// <summary>
    /// Descripción breve de ExamenMedico
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class ExamenMedico : System.Web.Services.WebService
    {

        [WebMethod]
        public DTO.DTOExamenMedico ObtenerAlumno(string AlumnoId)
        {
            return BLL.BLLExamenMedico.TraerAlumno(int.Parse(AlumnoId));
        }

        [WebMethod]
        public bool GuardarExamen(DTOExamenMedico Alumno)
        {
            return BLL.BLLExamenMedico.GuardarExamenMedico(Alumno.AlumnoId, Alumno.ExamenMedico, Alumno.UsuarioId, Alumno.Comentario);
        }
    }
}
