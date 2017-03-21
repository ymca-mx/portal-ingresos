using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DTO;
using BLL;
using System.Globalization;

namespace AppAdministrativos.WS
{
    /// <summary>
    /// Descripción breve de GrupoIdioma
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
     [System.Web.Script.Services.ScriptService]
    public class GrupoIdioma : System.Web.Services.WebService
    {
        [WebMethod]
        public List<DTOGrupoIdiomas> ConsultarGruposIdiomas()
        {
            return BLLGrupoIdioma.ConsultarGruposIdiomas();
        }
         [WebMethod]
        public string GuardarGrupoIdioma(string nombre,  string anio,  string periodoid,string usuarioid,string grupoId)
        {
            return BLLGrupoIdioma.GuardarGrupoIdioma(nombre, int.Parse(anio), int.Parse(periodoid), int.Parse(usuarioid), int.Parse(grupoId));
        }
         [WebMethod]
         public string EliminarGrupoIdioma(string grupoId)
         {
             return BLLGrupoIdioma.EliminarGrupoIdioma(int.Parse(grupoId));
         }

         [WebMethod]
         public List<DTOAlumnoIdiomas> ConsultarAlumnosIdiomas()
        {
            return BLLGrupoIdioma.ConsultarAlumnosIdiomas();
        }

         [WebMethod]
         public List<DTOAlumnoIdiomas> ConsultarAlumnosIdiomasGrupo(string GrupoId )
         {
             return BLLGrupoIdioma.ConsultarAlumnosIdiomasGrupo(int.Parse(GrupoId));
         }

         [WebMethod]
         public string AsignarAlumnosIdiomas(string AlumnoId, string GrupoId, string TipoCurso, string usuarioid, string OfertaId, string TM)
         {
             return BLLGrupoIdioma.AsignarAlumnosIdiomas(int.Parse(AlumnoId), int.Parse(GrupoId), TipoCurso, int.Parse(usuarioid), int.Parse(OfertaId), int.Parse(TM));


         }
    }
}
