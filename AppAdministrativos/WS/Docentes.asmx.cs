using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace AppAdministrativos.WS
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

        //[WebMethod]
        //public string GuardarDocente(string Nombre, string Paterno, string Materno, string EstadoCivil,
        //    string FechaNacimiento, string Genero, string RFC, string Email, string TelCelular, string TelCasa, string UsuarioId)
        //{
        //    return BLLDocente.NuevoDocente(new DTO.DTODocente
        //    {
        //        Nombre = Nombre,
        //        Paterno = Paterno,
        //        Materno = Materno,
        //        UsuarioId = int.Parse(UsuarioId),
        //        DocenteDetalle = new DTO.DTODocenteDetalle
        //        {
        //            Email = Email,
        //            EstadoCivilId = int.Parse(EstadoCivil),
        //            FechaNacimiento = DateTime.ParseExact((FechaNacimiento.Replace('-', '/')), "dd/MM/yyyy", CultureInfo.InvariantCulture),
        //            GeneroId = int.Parse(Genero),
        //            RFC = RFC,
        //            TelefonoCasa = TelCasa,
        //            TelefonoCelular = TelCelular
        //        }
        //    });
        //}
        //[WebMethod]
        //public List<DTO.DTODocente> ListaDocentes()
        //{
        //    return BLL.BLLDocente.ListarDocentesNormal();
        //}
        [WebMethod]
        public List<DTO.DTOPeriodo> TraerPeriodos()
        {
            return
            BLL.BLLDocente.TraerPeriodoActSig();
        }

        [WebMethod]
        public List<DTO.DTODocenteActualizar> TraerDocentes()
        {
            return
            BLL.BLLDocente.ListaDocentesActualizar();
        }

        [WebMethod]
        public List<DTO.DTODocenteActualizar> TraerDocentesConDatos()
        {
            return
            BLL.BLLDocente.ListaDocentesActualizarVbo();
        }

        [WebMethod]
        public int GuardarFormacion(int DocenteId, string Institucion, int OFertaTipo, string Carrera, bool Cedula, bool Titulo, int UsuarioId, int Anio, int PeriodoId)
        {
            return BLL.BLLDocente.GuardarFormacionAcademica(DocenteId, Institucion, OFertaTipo, Carrera, Cedula, Titulo, UsuarioId, Anio, PeriodoId);
        }

        [WebMethod]
        public int GuardarCurso(string NombreInstitucion, string TituloCurso,int Anio, int PeriodoId, int Duracion, string FechaInicial, string FechaFinal,bool EsCursoYmca,int DocenteId, int UsuarioId)
        {
            return BLL.BLLDocente.GuardarCurso(NombreInstitucion, TituloCurso, Anio, PeriodoId, Duracion, FechaFinal, FechaInicial, EsCursoYmca, DocenteId, UsuarioId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool GuardarFormacionDocumento()
        {
            HttpContext Contex = HttpContext.Current;
            HttpFileCollection httpFileCollection = Context.Request.Files;
            System.Collections.Specialized.NameValueCollection Documento = Context.Request.Form;
            try
            {
                int EstudioId = int.Parse(Documento["EstudioId"]);
                int TipoDocumentoId = int.Parse(Documento["TipoDocumento"]);
                HttpPostedFile httpDocumento = httpFileCollection["DocumentoComprobante"];
                Stream strDocumento = httpDocumento.InputStream;
                byte[] DocumentoByte = Herramientas.ConvertidorT.ConvertirStream(strDocumento, httpDocumento.ContentLength);
                 
                
                string RutaServe =
                            Server.MapPath("/EgresosUniYMCA/Documentos/");
                if (BLL.BLLDocente.GuardarRelacionDocumento(EstudioId, TipoDocumentoId, RutaServe + EstudioId + ".pdf"))
                {
                    if (File.Exists(RutaServe + EstudioId + ".pdf"))
                    {
                        File.Delete(RutaServe + EstudioId + ".pdf");
                        File.WriteAllBytes(RutaServe + EstudioId + ".pdf", DocumentoByte);
                    }
                    else { File.WriteAllBytes(RutaServe + EstudioId + ".pdf", DocumentoByte); }
                    return true;
                }
                else { return false; }
            }
            catch
            {
                return false;
            }
        }

        [WebMethod]
        public bool CancelarEstudio(int EstudioPeriodoId, string Comentario, int UsuarioId)
        {
            return BLL.BLLDocente.CancelarEstudio(EstudioPeriodoId, Comentario, UsuarioId);
        }

        [WebMethod]
        public bool CancelarCurso(int CursoId, string Comentario, int UsuarioId)
        {
            return BLL.BLLDocente.CancelarCurso(CursoId, Comentario, UsuarioId);
        }

        [WebMethod]
        public bool VboEstudio(int EstudioId, int UsuarioId)
        {
            return
            BLL.BLLDocente.VboEstudio(EstudioId, UsuarioId);
        }

        [WebMethod]
        public bool VboCurso(int CursoId, int UsuarioId)
        {
            return
            BLL.BLLDocente.VboCurso(CursoId, UsuarioId);
        }
    }
}
