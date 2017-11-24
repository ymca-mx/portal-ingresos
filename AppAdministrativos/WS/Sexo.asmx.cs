using BLL;
using DTO;
using Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AppAdministrativos.WS
{
    /// <summary>
    /// Descripción breve de Sexo
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Sexo : System.Web.Services.WebService
    {
        [WebMethod]
        public List<DTOEstadoCivil> ConsultarEstadoCivil()
        {
            //BLL.BLLAlumnoDescuento.InsertarDescuentoNormal(7712, decimal.Parse("62.88"), 50, "", "", 50, "", 0, "", 4, 6157, 3);
            //BLLAlumnoDescuento.InsertarDescuento(6699, 21, 0, 750);
            //    Descuentos objDescuento = new Descuentos();

            //    objDescuento.EnviarMail(7712, ConvertidorT.CrearPass());
            return BLLEstadoCivil.ConsultarEstadosCiviles();

        }
        [WebMethod]
        public List<string> EnviarMail(string Alumnos)
        {
            List<string> mensajes = new List<string>();
            string[] ListaAlumnos = Alumnos.Split(',');
            foreach (string AlumnoId in ListaAlumnos)
            {
                try
                {
                    Descuentos objDescuento = new Descuentos();
                    objDescuento.EnviarMail(int.Parse(AlumnoId), ConvertidorT.CrearPass());
                    mensajes.Add(AlumnoId + " Correcto");
                }
                catch (Exception e)
                {
                    mensajes.Add(AlumnoId + " " + e.Message);
                }
            }
            return mensajes;
        }
        [WebMethod]
        public List<string> InsertarOFerta(string AlumnoId, string OfertaEducativaId, string MontoInscripcion, string MontoColegiatura, string Anio, string PeriodoId, string Inscripcion, string GenerarDescuento)
        {
            string[] lstResultados =
           BLLAlumnoDescuento.InsertarDescuento(int.Parse(AlumnoId), int.Parse(OfertaEducativaId), decimal.Parse(MontoInscripcion), decimal.Parse(MontoColegiatura),
           int.Parse(Anio), int.Parse(PeriodoId), Inscripcion == "Si" ? true : false, GenerarDescuento == "Si" ? true : false, 0);

            List<string> lstR = lstResultados.ToList();
            return lstR;
        }
        [WebMethod]
        public void GenerarReferencias(string AlumnoId)
        {
            BLLAlumnoDescuento.GenerarReferencia(int.Parse(AlumnoId));
        }

        [WebMethod]
        public string FechaCompletaServer()
        {
            return DateTime.Now.ToString();
        }

        [WebMethod]
        public ObejtoC ExampleObject()
        {
            ObejtoC listaAlumnos = new ObejtoC { lstAlumnos = BLLAlumnoPortal.ListarAlumnos() };

            return listaAlumnos;
        }         
    }
    public class ObejtoC
    {
        public List<DTOAlumnoLigero> lstAlumnos { get; set; }
    }
}
