using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AppAdministrativos.WS
{
    /// <summary>
    /// Descripción breve de Beca
    /// </summary> 
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Beca : System.Web.Services.WebService
    {
        [WebMethod]
        public DTOAlumnoBecaComite ObtenerAlumno(string AlumnoId)
        {
            return BLLBeca.ObtenerAlumno(int.Parse(AlumnoId));
        }
        [WebMethod]
        public DTOAlumnoBecaDeportiva ObtenerAlumnoDeportiva(string AlumnoId)
        { 

            return BLLBeca.ObtenerAlumnoDeportiva(int.Parse(AlumnoId));
        }

        [WebMethod]
        public List<DTO.DTOAlumnoBecas> ListaAlumnosActuales()
        {
            //return BLL.BLLAlumno.ListaAlumnosActuales();
            return null;
        }
        [WebMethod]
        public DTO.AlumnoPagos BuscarAlumno(string AlumnoId, string OfertaEducativaId)
        {
            return BLL.BLLAlumnoPortal.BuscarAlumno(int.Parse(AlumnoId), int.Parse(OfertaEducativaId));
        }
        [WebMethod]
        public List<DTO.DTOAlumnoDescuento> DescuentosAnteriores(string AlumnoId, string OfertaEducativaId)
        {
            return BLL.BLLDescuentos.TraerDescuentos(int.Parse(AlumnoId), int.Parse(OfertaEducativaId));
        }
        [WebMethod]
        public string InsertarVarios()
        {
            Stopwatch objClock = new Stopwatch();
            objClock.Start();

            string Errores = "Alumnos  \n";
            List<DTO.Alumno.Beca.DTOAlumnoBeca> lstAlumnos = new BLL.ListaAlumnnosInscritos().lista;
            lstAlumnos.ForEach(a =>
            {
                AlumnoPagos objAl = BLL.BLLAlumnoPortal.BuscarAlumno(a.alumnoId);
                Errores += "Alumnoid:    " + objAl.AlumnoId;
                if (int.Parse(objAl.AlumnoId) != a.alumnoId)
                {
                    lstAlumnos.RemoveAt(lstAlumnos.FindIndex(P => P.alumnoId == a.alumnoId));
                }
            });


            //List<int> lstAlumnos=BLL.BLLAlumnoBeca.listarAlumnos()
            lstAlumnos.ForEach(a =>
            {
                try
                {
                    bool resp = true;// BLL.BLLAlumno.AplicaBecaAlumno(a);
                    if (resp)
                    {
                        BLL.BLLSaldoAFavor.AplicacionSaldoAlumno(a.alumnoId, true, false);
                    }
                }
                catch (Exception err)
                {
                    Errores += "\n  " + a.alumnoId + "      " + err.Message;
                }
            });
            TimeSpan ts = objClock.Elapsed;

            Errores += String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

            return Errores;
        }

        [WebMethod]
        public string BecaComite(string AlumnoId, string OfertaEducativaId, string Anio, string PeriodoId,
                string Porcentaje, string UsuarioId, string EsComite, string EsSEP, string EsEmpresa)
        {
            string Respuesta =
            BLL.BLLBeca.VerificarInscripcionActual(int.Parse(AlumnoId), int.Parse(OfertaEducativaId), int.Parse(Anio), int.Parse(PeriodoId));

            if (Respuesta == "Procede")
            {
                DTO.Alumno.Beca.DTOAlumnoBeca objBeca = new DTO.Alumno.Beca.DTOAlumnoBeca
                {
                    alumnoId = int.Parse(AlumnoId),
                    anio = int.Parse(Anio),
                    esSEP = bool.Parse(EsSEP),
                    ofertaEducativaId = int.Parse(OfertaEducativaId),
                    periodoId = int.Parse(PeriodoId),
                    porcentajeBeca = decimal.Parse(Porcentaje),
                    usuarioId = int.Parse(UsuarioId),
                    esComite = bool.Parse(EsComite),
                    esEmpresa = bool.Parse(EsEmpresa),
                    fecha=""
                };

                try
                {
                    BLL.BLLAlumnoPortal.AplicaBeca(objBeca, false);
                    return "Guardado";
                }
                catch(Exception k) { return "Fallo"; /* return k.InnerException.InnerException.Message;*/ }
            }
            else
            {
                return Respuesta;
            }
        }

        [WebMethod]
        public string BecaDeportiva(string AlumnoId, string OfertaEducativaId, string Anio, string PeriodoId,
                string Porcentaje, string UsuarioId)
        {
            DTO.Alumno.Beca.DTOAlumnoBecaDeportiva objBeca = new DTO.Alumno.Beca.DTOAlumnoBecaDeportiva
            {
                alumnoId = int.Parse(AlumnoId),
                anio = int.Parse(Anio),
                ofertaEducativaId = int.Parse(OfertaEducativaId),
                periodoId = int.Parse(PeriodoId),
                porcentajeBeca = decimal.Parse(Porcentaje),
                usuarioId = int.Parse(UsuarioId)               
            };
            try
            {
                BLL.BLLAlumnoPortal.AplicaBecaDeportiva(objBeca, false);
                return "Guardado";
            }
            catch { return "Fallo"; }
        }



        [WebMethod]
        public string InsertarBeca(string AlumnoId, string OfertaEducativaId, string Monto, string SEP,
            string Anio, string PeriodoId, string Usuario, string EsComite, string EsEmpresa, string Materias,
            string Asesorias)
        {
            DTO.Alumno.Beca.DTOAlumnoBeca objBeca;
            try
            {
                var obbjetos = bool.Parse(EsEmpresa) ? BLLGrupo.TraerInscripcion(int.Parse(AlumnoId), int.Parse(OfertaEducativaId), int.Parse(Anio), int.Parse(PeriodoId), int.Parse(Usuario), decimal.Parse(Monto)) : null;

                objBeca = new DTO.Alumno.Beca.DTOAlumnoBeca
                {
                    alumnoId = int.Parse(AlumnoId),
                    anio = int.Parse(Anio),
                    esSEP = bool.Parse(SEP),
                    ofertaEducativaId = int.Parse(OfertaEducativaId),
                    periodoId = int.Parse(PeriodoId),
                    porcentajeBeca = bool.Parse(EsEmpresa) ? obbjetos?.Where(l => l.DTOPagoConcepto.PagoConceptoId == 800)?.FirstOrDefault()?.Monto ?? 0 : decimal.Parse(Monto),
                    porcentajeInscripcion = obbjetos?.Where(l => l.DTOPagoConcepto.PagoConceptoId == 802)?.FirstOrDefault()?.Monto ?? 0,
                    usuarioId = int.Parse(Usuario),
                    esComite = bool.Parse(EsComite),
                    esEmpresa = bool.Parse(EsEmpresa),
                    fecha = ""
                };
            }
            catch { return "fallo"; }
            try
            {
                BLL.BLLAlumnoPortal.AplicaBeca(objBeca, false);
                return "Guardado";
            }
            catch { return "Fallo"; }
        }
        [WebMethod]
        public string AplicarBeca(string AlumnoId)
        {
            try
            {
                BLL.BLLSaldoAFavor.AplicacionSaldoAlumno(int.Parse(AlumnoId), true, false);
                return "Aplicado";
            }
            catch (Exception a) { return a.Message; }
        }
        [WebMethod]
        public string GuardarDocumentos()
        {
            HttpFileCollection httpFileCollection = Context.Request.Files;
            System.Collections.Specialized.NameValueCollection Datos = Context.Request.Form;

            List<DTOAlumnInscritoDocumento> lstDocuemtos = new List<DTOAlumnInscritoDocumento>();
            try
            {
                int AlumnoId = int.Parse(Datos["AlumnoId"]);
                int OfertaEducativaid = int.Parse(Datos["OfertaEducativaId"]);
                int Anio = int.Parse(Datos["Anio"]);
                int PeriodoId = int.Parse(Datos["Periodo"]);
                bool EsComite = bool.Parse(Datos["EsComite"]);
                int UsuarioId = int.Parse(Datos["UsuarioId"]);

                HttpPostedFile httpBeca = httpFileCollection["DocumentoBeca"];
                Stream strBeca = httpBeca == null ? null : httpBeca.InputStream;
                HttpPostedFile httpComite = httpFileCollection["DocumentoComite"];
                Stream strComite = httpComite == null ? null : httpComite.InputStream;
                HttpPostedFile httpDeportiva = httpFileCollection["DocumentoDeportiva"];
                Stream strDeportiva = httpDeportiva == null ? null : httpDeportiva.InputStream;
                if (httpBeca != null)
                {
                    lstDocuemtos.Add(new DTOAlumnInscritoDocumento
                    {
                        AlumnoId = AlumnoId,
                        OfertaEducativaId = OfertaEducativaid,
                        Anio = Anio,
                        PeriodoId = PeriodoId,
                        TipoDocumento = 1,
                        Archivo = httpBeca == null ? null : Herramientas.ConvertidorT.ConvertirStream(strBeca, httpBeca.ContentLength),
                        UsuarioDocumento = UsuarioId
                    });
                }
                if (httpComite != null)
                {
                    lstDocuemtos.Add(new DTOAlumnInscritoDocumento
                    {
                        AlumnoId = AlumnoId,
                        OfertaEducativaId = OfertaEducativaid,
                        Anio = Anio,
                        PeriodoId = PeriodoId,
                        TipoDocumento = 2,
                        Archivo = httpComite == null ? null : Herramientas.ConvertidorT.ConvertirStream(strComite, httpComite.ContentLength),
                        UsuarioDocumento = UsuarioId
                    });
                }
                if (httpDeportiva != null)
                {
                    lstDocuemtos.Add(new DTOAlumnInscritoDocumento
                    {
                        AlumnoId = AlumnoId,
                        OfertaEducativaId = OfertaEducativaid,
                        Anio = Anio,
                        PeriodoId = PeriodoId,
                        TipoDocumento = 3,
                        Archivo = httpDeportiva == null ? null : Herramientas.ConvertidorT.ConvertirStream(strDeportiva, httpDeportiva.ContentLength),
                        UsuarioDocumento = UsuarioId
                    });
                }
                //objDoc = new DTOAlumnInscritoDocumento
                //{
                //    ArchivoBeca = httpBeca == null ? null : Herramientas.ConvertidorT.ConvertirStream(strBeca, httpBeca.ContentLength),
                //    ArchivoComite = httpComite == null ? null : Herramientas.ConvertidorT.ConvertirStream(strComite, httpComite.ContentLength),
                //    AlumnoId = AlumnoId,
                //    Anio = Anio,
                //    EsComite = EsComite,
                //    OfertaEducativaId = OfertaEducativaid,
                //    PeriodoId = PeriodoId
                //};

                //return BLL.BLLAlumnoDescuentoDocumento.GuardarAlumnoInscritoBecaDocumentos(objDoc);
                if (lstDocuemtos.Count > 0)
                {
                    return BLLAlumnoInscritoDocumento.GuardarDocumentos(lstDocuemtos);
                }
                else { return "false"; }
            }
            catch (Exception a) { return a.Message; }

        }
        [WebMethod]
        public List<DTOOfertaEducativa> OfertasAlumno(string AlumnoId)
        {
            return BLL.BLLOfertaEducativaPortal.OfertaAlumno(int.Parse(AlumnoId));
        }
        [WebMethod]
        public string GenerarPDF(string DocumentoId)
        {
            byte[] Archivo = BLLAlumnoInscrito.TraerDocumento(int.Parse(DocumentoId));
            string nombre = Herramientas.ConvertidorT.CrearPass() + ".pdf";
            System.IO.File.WriteAllBytes("C:\\inetpub\\wwwroot\\Archivos\\" + nombre, Archivo);
            return "\\Archivos\\" + nombre;
        }

        [WebMethod]
        public void GenerarPDF2(string DocumentoId)
        {
            Stream stream = new MemoryStream(BLLAlumnoInscrito.TraerDocumento(int.Parse(DocumentoId)));
            var MemoryStream = new MemoryStream();
            stream.CopyTo(MemoryStream);

            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("Content-Type", "application/pdf");
            HttpContext.Current.Response.AddHeader("Content-Disposition", "inline; filename=" + "Beca" + ".pdf");
            HttpContext.Current.Response.BinaryWrite(MemoryStream.ToArray());
            HttpContext.Current.Response.Flush();
            MemoryStream.Close();
            HttpContext.Current.Response.End();
        }
    }
}
