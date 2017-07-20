using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DTO;
using BLL;
using System.Web.Script.Services;
using System.IO;

namespace AppAdministrativos.WS
{
    /// <summary>
    /// Descripción breve de Calendario
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Calendario : System.Web.Services.WebService
    {

        [WebMethod]
        public List<DTOCalendarioEscolar>Listar()
        {
            return BLLCalendarioEscolar.TraerCalendarios();
        }

        [WebMethod]
        public int Insert(DTOCalendarioEscolar Calendario)
        {
            return
            BLLCalendarioEscolar.NuevoCalendario(Calendario);
        }

        [WebMethod]
        public bool Update(DTOCalendarioEscolar Calendario)
        {
            return BLLCalendarioEscolar.ModificarCalendario(Calendario);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool GuardarCalendario()
        {
            HttpContext Contex = HttpContext.Current;
            HttpFileCollection httpFileCollection = Context.Request.Files;
            System.Collections.Specialized.NameValueCollection Calendario = Context.Request.Form;
            try
            {
                int CalendarioId = int.Parse(Calendario["CalendarioEscolarId"]);
                HttpPostedFile httpCalendario = httpFileCollection["DocumentoCalendario"];
                Stream strCalendario = httpCalendario.InputStream;
                byte[] CalendarioFil = Herramientas.ConvertidorT.ConvertirStream(strCalendario, httpCalendario.ContentLength);

                string RutaServe =
                            Server.MapPath("../Documentos/Calendarios/");
                if (File.Exists(RutaServe + CalendarioId + ".pdf"))
                {
                    File.Delete(RutaServe + CalendarioId + ".pdf");
                    File.WriteAllBytes(RutaServe + CalendarioId + ".pdf", CalendarioFil);
                }
                else { File.WriteAllBytes(RutaServe + CalendarioId + ".pdf", CalendarioFil); }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
