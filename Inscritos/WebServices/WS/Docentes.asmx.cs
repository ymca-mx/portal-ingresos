using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebServices.WS
{
    /// <summary>
    /// Descripción breve de Docentes
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Docentes : System.Web.Services.WebService
    {

        [WebMethod]
        public string GuardarDocente(string Nombre, string Paterno, string Materno, string EstadoCivil,
            string FechaNacimiento, string Genero, string RFC, string Email, string TelCelular, string TelCasa, string UsuarioId)
        {
            return BLL.BLLDocente.NuevoDocente(new DTO.DTODocente
            {
                Nombre = Nombre,
                Paterno = Paterno,
                Materno = Materno,
                UsuarioId = int.Parse(UsuarioId),
                DocenteDetalle = new DTO.DTODocenteDetalle
                {
                    Email = Email,
                    EstadoCivilId = int.Parse(EstadoCivil),
                    FechaNacimiento = DateTime.ParseExact((FechaNacimiento.Replace('-', '/')), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    GeneroId = int.Parse(Genero),
                    RFC = RFC,
                    TelefonoCasa = TelCasa,
                    TelefonoCelular = TelCelular
                }
            });
        }
        [WebMethod]
        public List<DTO.DTODocente> ListaDocentes()
        {
            return BLL.BLLDocente.ListarDocentesNormal();
        }
    }
}
