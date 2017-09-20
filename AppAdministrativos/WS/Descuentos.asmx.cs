using BLL;
using DTO;
using Herramientas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Utilities;

namespace AppAdministrativos.WS
{
    /// <summary>
    /// Descripción breve de Descuentos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Descuentos : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] GuardarDescuentos(int AlumnoId, string DescuentoIns, string JustificacionIns, string DescuentoBec, string JustificacionBec,
            string Observacion, string SistemaPago, string DescuentoExamen, string JustificacionExam, string Credencial, string JustificacionCred, int Usuario)
        {
            
            try
            {
                DTOAlumnoInscrito objAl = BLLAlumnoInscrito.ConsultarAlumnoInscrito(AlumnoId);

                BLLAlumnoInscrito.ActializarAlumnoInscrito(AlumnoId, int.Parse(SistemaPago));

                DTOAlumnoPassword objPas = BLLAlumnoPassword.GuardarPassword(AlumnoId, Utilities.Seguridad.Encripta(27, ConvertidorT.CrearPass()));


                string[] Resultados;
                Resultados = BLLAlumnoDescuento.InsertarDescuentoNormal(AlumnoId,
                     decimal.Parse(DescuentoBec), decimal.Parse(DescuentoIns),
                     JustificacionIns, JustificacionBec, decimal.Parse(DescuentoExamen),
                     JustificacionExam, decimal.Parse(Credencial), JustificacionCred,
                     int.Parse(SistemaPago), Usuario, 0,false);

                decimal BecaAca = decimal.Parse(DescuentoBec);

                return Resultados;
                //return BLLAlumnoDescuento.InsertarDescuento(AlumnoId,
                //    decimal.Parse(DescuentoBec), decimal.Parse(DescuentoIns),
                //    JustificacionIns, JustificacionBec, 2016, 1);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GuardarDocumentos()
        {
            try
            {
                HttpContext Contex = HttpContext.Current;
                HttpFileCollection httpFileCollection = Context.Request.Files;
                System.Collections.Specialized.NameValueCollection DescuentosID = Context.Request.Form;

                List<DTOAlumnoDescuentoDocumento> lstDoc = new List<DTOAlumnoDescuentoDocumento>();
                try
                {
                    int descuentoBecaId = int.Parse(DescuentosID["DescuentoIdB"]);
                    HttpPostedFile httpColegiatura = httpFileCollection["DocBeca"];
                    Stream strCole = httpColegiatura.InputStream;
                    lstDoc.Add(new DTOAlumnoDescuentoDocumento
                    {
                        AlumnoDescuentoDocumento1 = Herramientas.ConvertidorT.ConvertirStream(strCole, httpColegiatura.ContentLength),
                        AlumnoDescuentoId = descuentoBecaId
                    });
                }
                catch { }
                try
                {
                    int descuentoInscId = int.Parse(DescuentosID["DescuentoIdI"]);
                    HttpPostedFile httpInscripcion = httpFileCollection["DocInscipcion"];
                    Stream strInsc = httpInscripcion.InputStream;
                    lstDoc.Add(new DTOAlumnoDescuentoDocumento
                    {
                        AlumnoDescuentoDocumento1 = Herramientas.ConvertidorT.ConvertirStream(strInsc, httpInscripcion.ContentLength),
                        AlumnoDescuentoId = descuentoInscId
                    });
                }
                catch { }
                try
                {
                    int descuentoExam = int.Parse(DescuentosID["DescuentoExam"]);
                    HttpPostedFile httpExamen = httpFileCollection["DocExamen"];
                    Stream strExam = httpExamen.InputStream;
                    lstDoc.Add(new DTOAlumnoDescuentoDocumento
                    {
                        AlumnoDescuentoDocumento1 = Herramientas.ConvertidorT.ConvertirStream(strExam, httpExamen.ContentLength),
                        AlumnoDescuentoId = descuentoExam
                    });
                }
                catch { }

                if (lstDoc.Count > 0)
                {
                    BLLAlumnoDescuentoDocumento.AlumnoDocumentoGuardar(lstDoc);
                    return "1";
                }
                else { return "0"; }

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        [WebMethod]
        public void GuardarDocumentosIngles()
        {
            HttpFileCollection httpFileCollection = Context.Request.Files;
            System.Collections.Specialized.NameValueCollection DescuentosID = Context.Request.Form;

            DTOAlumnoDescuentoDocumento objDoc;
            try
            {
                int AlumnoId = int.Parse(DescuentosID["AlumnoId"]);
                int OfertaEducativaid = int.Parse(DescuentosID["OfertaEducativaId"]);
                DTOAlumnoDescuento objAlDesc = BLLAlumnoDescuento.TraerDescuentos(AlumnoId, OfertaEducativaid, 807);

                HttpPostedFile httpColegiatura = httpFileCollection["DocBeca"];
                Stream strCole = httpColegiatura.InputStream;
                objDoc = new DTOAlumnoDescuentoDocumento
                {
                    AlumnoDescuentoDocumento1 = Herramientas.ConvertidorT.ConvertirStream(strCole, httpColegiatura.ContentLength),
                    AlumnoDescuentoId = objAlDesc.AlumnoDescuentoId
                };
            }
            catch { objDoc = null; }
            if (objDoc != null)
            {
                BLLAlumnoDescuentoDocumento.AlumnoDocumentoGuardar(new List<DTOAlumnoDescuentoDocumento> { objDoc });
            }
        }
        [WebMethod]
        public byte[] TraerDocumento()
        {
            byte[] file = BLLAlumnoDescuentoDocumento.AlumnoDocumentoConsultar().AlumnoDescuentoDocumento1;
            return file;
        }
        [WebMethod]
        public List<DTOCuota> TraerDescuentos(int AlumnoId)
        {
            return BLLAlumnoDescuento.TraerDescuentos(AlumnoId);
        }
        [WebMethod]
        public List<DTOCuota> TraerDescuentosPeriodo(int OfertaEducativaId, string Periodo)
        {
            return BLLAlumnoDescuento.TraerDescuentos(OfertaEducativaId, Periodo);
        }
        [WebMethod]
        public List<DTOCuota> ConsultarCuotaOfertaEducativaPeriodo(string OfertaEducativaId, string Periodo)
        {
            return BLLAlumnoDescuento.TraerCuotasOfertaEducativaPeriodo(int.Parse(OfertaEducativaId), Periodo);
        }
        [WebMethod]
        public string GuardarIngles(int AlumnoId, int UsuarioId)
        {
            BLLAlumnoInscrito.ActializarAlumnoInscrito(AlumnoId, 5);
            if (BLLAlumnoDescuento.InsertarDescuentosIngles(AlumnoId) != null)
            { return "guardado"; }
            else
            { return null; }
        }
        [WebMethod]
        public string[] GuardarIdioma(int AlumnoId, int OfertaEducativa, string Turno, string Periodo, string SistemaPago, string DescuentoBec,
            string JustificacionBec, string Credencial, string JustificacionCred, string Material, string EsEmpresa, string DescuentoExamen,
            string JustificacionExam, string DescuentoIns, string JustificacionIns, int Usuario)
        {
            try
            {
                Nullable<int> defaul = null;

                DTOOfertaEducativaTipo objOfti = BLLOfertaEducativaTipo.ConsultarOferta(OfertaEducativa);
                int Anio = BLLPeriodoPortal.ConsultarPeriodo(Periodo).Anio, Periodoid = int.Parse(Periodo.Substring(0, 1));
                int usu = Usuario;
                DTOAlumnoInscrito objInscribir = new DTOAlumnoInscrito
                {
                    AlumnoId = AlumnoId,
                    PeriodoId = Periodoid,
                    Anio = Anio,
                    OfertaEducativaId = OfertaEducativa,
                    PagoPlanId = bool.Parse(EsEmpresa) == true ? defaul : int.Parse(SistemaPago),
                    EsEmpresa = bool.Parse(EsEmpresa),
                    TurnoId = int.Parse(Turno),
                    UsuarioId = usu
                };
                DTOAlumnoInscrito objinsc = BLLAlumnoInscrito.ConsultarAlumnoInscrito(objInscribir.AlumnoId, objInscribir.OfertaEducativaId);

                if ((objinsc?.Anio ?? Anio) == Anio || (objinsc?.PeriodoId ?? Periodoid) == Periodoid)
                {
                    if (bool.Parse(EsEmpresa) == false)
                    {
                        if (objinsc == null)
                        {

                            BLLAlumnoInscrito.InsertarAlumnoInscrito2(objInscribir);
                        }
                        else { BLLAlumnoInscrito.ActializarAlumnoInscrito(AlumnoId, int.Parse(SistemaPago)); }
                    }
                    else
                    {
                        #region Empresa
                        objInscribir.PagoPlanId = null;
                        objInscribir.EsEmpresa = true;
                        BLLAlumnoInscrito.InsertarAlumnoInscrito2(objInscribir);

                        #endregion
                    }
                }
                else
                {
                    objinsc.TurnoId = objInscribir.TurnoId;
                    objinsc.UsuarioId = objInscribir.UsuarioId;
                    objinsc.PagoPlanId = objInscribir.PagoPlanId;

                    if (!BLLAlumnoInscrito.ActializarAlumnoInscrito(objinsc, Anio, Periodoid)) { objOfti = null; }
                }
                if (objOfti.OfertaEducativaTipoId != 4)
                {
                    if (bool.Parse(EsEmpresa) == true) { return new string[] { "Guardado" }; } //Codicion si EsEmpresa
                    else
                    {
                        return BLLAlumnoDescuento.InsertarDescuentoNormal(AlumnoId, decimal.Parse(DescuentoBec), decimal.Parse(DescuentoIns),
                            (JustificacionIns == "null" ? " " : JustificacionIns), (JustificacionBec == "null" ? " " : JustificacionBec),
                            decimal.Parse(DescuentoExamen), JustificacionExam == "null" ? " " : JustificacionExam,
                            decimal.Parse(Credencial), JustificacionCred == "null" ? " " : JustificacionCred,
                            int.Parse(SistemaPago), usu, OfertaEducativa,true);
                    }
                }
                else
                {
                    if (bool.Parse(EsEmpresa) == true) { return new string[] { "Guardado" }; }
                    else
                    {
                        DTOAlumnoPassword objAlumPas = BLLAlumnoPassword.ConsultarAlumnoPassword(AlumnoId);
                        if (objAlumPas != null)
                        {
                            if (objAlumPas.AlumnoId == 0)
                            {
                                DTOAlumnoPassword objp = BLLAlumnoPassword.GuardarPassword(AlumnoId, Utilities.Seguridad.Encripta(27, ConvertidorT.CrearPass()));
                            }
                        }
                        return new string[]{ BLLAlumnoDescuento.InsertarDescuentosIdiomas(AlumnoId, OfertaEducativa,
                        decimal.Parse(DescuentoBec), JustificacionBec == "null" ? " " : JustificacionBec,
                        decimal.Parse(Credencial), (JustificacionCred).ToString() == "null" ? " " : (JustificacionCred).ToString(),
                        bool.Parse(Material),Usuario ,Anio,Periodoid)};
                    }
                }
            }
            catch (Exception e)
            {
                return new string[] { e.Message };
            }
        }

        [WebMethod]
        public DTOPagos GenerarPagoB(string AlumnoId, string OfertaEducativaId, string PagoConceptoId, string CuotaId, string UsuarioId)
        {
            return BLLPagoPortal.GenerarPago(int.Parse(AlumnoId), int.Parse(OfertaEducativaId), int.Parse(PagoConceptoId), int.Parse(CuotaId), int.Parse(UsuarioId));            
        }

        [WebMethod]
        public DTOPagos GenerarPagoC(string AlumnoId, string OfertaEducativaId, string PagoConceptoId, string CuotaId, string UsuarioId, string Anio, string PeriodoId)
        {
            DTOPagos objPago = BLLPagoPortal.GenerarPagoC(int.Parse(AlumnoId), int.Parse(OfertaEducativaId), int.Parse(PagoConceptoId), int.Parse(CuotaId), int.Parse(UsuarioId), int.Parse(Anio), int.Parse(PeriodoId));

            return objPago;
        }

        [WebMethod]
        public DTOPagos GenerarPago(string AlumnoId, string OfertaEducativaId, string PagoConceptoId, string CuotaId)
        {
            DTOPagos objPago = BLLPagoPortal.GenerarPago(int.Parse(AlumnoId), int.Parse(OfertaEducativaId), int.Parse(PagoConceptoId), int.Parse(CuotaId));

            return objPago;
        }
        [WebMethod]
        public DTOPagos GenerarPago2(string AlumnoId, string OfertaEducativaId, string PagoConceptoId, string CuotaId, string Monto)
        {
            DTOPagos objPago = BLLPagoPortal.GenerarPago(int.Parse(AlumnoId), int.Parse(OfertaEducativaId), int.Parse(PagoConceptoId), int.Parse(CuotaId), decimal.Parse(Monto));

            return objPago;
        }
        [WebMethod]
        public DTOPagos GenerarPago2B(string AlumnoId, string OfertaEducativaId, string PagoConceptoId, string CuotaId, string Monto, string UsuarioId)
        {
            DTOPagos objPago = BLLPagoPortal.GenerarPago(int.Parse(AlumnoId), int.Parse(OfertaEducativaId), int.Parse(PagoConceptoId), int.Parse(CuotaId), decimal.Parse(Monto), int.Parse(UsuarioId));

            return objPago;
        }
        [WebMethod]
        public List<DTOCuota> TraerDescuentosIdiomas(string Idioma, string Periodo)
        {
            return BLLAlumnoDescuento.TraerCuotaIdiomas(int.Parse(Idioma), Periodo);
        }
        public Boolean EnviarMail(DTOPagos obPago)
        {
            var refere = new Utilities.ProcessResult();
            DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();
            string body = "";
            DTOAlumno opjAl = BLLAlumnoPortal.ObtenerAlumno(obPago.AlumnoId);
            #region "HTML"
            body = "<!DOCTYPE html>" +
            "<html xmlns='http://www.w3.org/1999/xhtml'>" +
            "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "    <title></title>" +
            "</head>" +
            "<body>" +
            "<div style='height:200px;height:300px; background-color:#eff3f8'>" +
             "<h2>Estimado Alumno " + opjAl.Nombre + " " + opjAl.Paterno + " " + opjAl.Materno + "</h2>" +
              "<h3>Te informamos que tienes un cargo por concepto de </h3>" +
               "<table border='1px'>" +
                "<thead>" +
                 "<tr>" +
                  "<th>Concepto&nbsp</th>" +
                   "<th>Referencia Bancaria&nbsp</th>" +
                    "<th>Monto &nbsp</th>" +
                     "</tr>" +
                      "</thead>" +
                       "<tbody>" +
                        "<tr>" +
                         "<td>" + obPago.DTOCuota.DTOPagoConcepto.Descripcion + "</td>" +
                          "<td style='text-align:center'>" + obPago.Referencia + "</td>" +
                           "<td>" + obPago.objNormal.Monto + "</td>" +
                            "</tr>" +
                        "</tbody>" +
                    "</table>" +
                    "<p>" +
                        "Banco Scotiabank <br />" +
                        "No. de Cuenta: 00105771894<br />" +
                        "Nombre del Cliente: Universidad YMCA A.C.<br />" +
                        "Moneda: MXN<br />" +
                    "</p>            " +
                "</div>" +
            "</body>" +
            "</html>";
            #endregion
            return Email.Enviar(objCuenta.Email, objCuenta.Password, objCuenta.DisplayName, /*opjAl.DTOAlumnoDetalle.Email*/"j053_pepe@hotmail.com", ',', "Cargo por " + obPago.DTOCuota.DTOPagoConcepto.Descripcion, body, "", ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true, ref refere);
            //return Email.Enviar("sistemastest@ymcacdmex.org.mx", "Sis99TesX1", "Bienvenido", opjAl.DTOAlumnoDetalle.Email, ',', "Universidad YMCA", body, "", ',', "mail.ymcacdmex.org.mx", 587, false, true, ref refere);
        }
        [WebMethod]
        public Boolean EnviarMailId(int PagoId)
        {
            DTOPagos obPago = BLLPagoPortal.ObtenerPago(PagoId);
            var refere = new Utilities.ProcessResult();
            DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();
            string body = "";
            DTOAlumno opjAl = BLLAlumnoPortal.ObtenerAlumno1(obPago.AlumnoId);
            #region "HTML"
            body = "<!DOCTYPE html>" +
            "<html xmlns='http://www.w3.org/1999/xhtml'>" +
            "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "    <title></title>" +
            "</head>" +
            "<body>" +
            "<div style='height:200px;height:300px; background-color:#eff3f8'>" +
             "<h2>Estimado Alumno " + opjAl.Nombre + " " + opjAl.Paterno + " " + opjAl.Materno + "</h2>" +
              "<h3>Te informamos que tienes un cargo por concepto de </h3>" +
               "<table border='1px'>" +
                "<thead>" +
                 "<tr>" +
                  "<th>Concepto&nbsp</th>" +
                   "<th>Referencia Bancaria&nbsp</th>" +
                    "<th>Monto &nbsp</th>" +
                     "</tr>" +
                      "</thead>" +
                       "<tbody>" +
                        "<tr>" +
                         "<td>" + obPago.DTOCuota.DTOPagoConcepto.Descripcion + "</td>" +
                          "<td style='text-align:center'>" + obPago.Referencia + "</td>" +
                           "<td>" + obPago.objNormal.Monto + "</td>" +
                            "</tr>" +
                        "</tbody>" +
                    "</table>" +
                    "<p>" +
                        "Banco Scotiabank <br />" +
                        "No. de Cuenta: 00105771894<br />" +
                        "Nombre del Cliente: Universidad YMCA A.C.<br />" +
                        "Moneda: MXN<br />" +
                    "</p>            " +
                "</div>" +
            "</body>" +
            "</html>";
            #endregion
            try
            {
                return Email.Enviar(objCuenta.Email, objCuenta.Password, objCuenta.DisplayName, opjAl.DTOAlumnoDetalle.Email, ',', "Cargo por " + obPago.DTOCuota.DTOPagoConcepto.Descripcion, body, "", ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true, ref refere);

            }
            catch
            {
                return false;
            }
            //return Email.Enviar("sistemastest@ymcacdmex.org.mx", "Sis99TesX1", "Bienvenido", opjAl.DTOAlumnoDetalle.Email, ',', "Universidad YMCA", body, "", ',', "mail.ymcacdmex.org.mx", 587, false, true, ref refere);
        }
        [WebMethod]
        public string MAndarMailContraseña(int AlumnoId)
        {
            bool enviado = false;
            DTOAlumnoPassword objAlumPas = BLLAlumnoPassword.ConsultarAlumnoPassword(AlumnoId);
            if (objAlumPas != null)
            {
                if (objAlumPas.AlumnoId == 0)
                {
                    DTOAlumnoPassword objp = BLLAlumnoPassword.GuardarPassword(AlumnoId, Utilities.Seguridad.Encripta(27, ConvertidorT.CrearPass()));
                }
            }
            while (enviado == false)
            {
                enviado = EnviarMail11(AlumnoId);
            }
            return "Enviado";
        }
        public Boolean EnviarMail(int AlumnoId, string Pass)
        {
            var refere = new Utilities.ProcessResult();
            DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();
            DTOAlumnoPassword objPas;
            objPas = BLLAlumnoPassword.ConsultarAlumnoPassword(AlumnoId);
            if (objPas == null)
            { objPas = BLLAlumnoPassword.GuardarPassword(AlumnoId, Utilities.Seguridad.Encripta(27, Pass)); }
            string body = "";
            DTOAlumno opjAl = BLLAlumnoPortal.ObtenerAlumno(objPas.AlumnoId);
            #region "HTML"
            body = "<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>" +
                        "<head>" +
                        "<meta charset='utf-8' />" +
                        "<title>Bienvenida Alumnos</title>" +
                        "<meta http-equiv='X-UA-Compatible' content='IE=edge' />" +
                        "<meta content='width=device-width, initial-scale=1.0' name='viewport' />" +
                        "<meta http-equiv='Content-type' content='text/html; charset=utf-8' />" +
                        "<meta content='' name='description' />" +
                        "<meta content='' name='author' />" +
                       "<style>" +
                            "body {" +
                                "color: #333333;" +
                                "font-family: 'Open Sans', sans-serif;" +
                                "padding: 0px !important;" +
                                "margin: 0px !important;" +
                                "font-size: 13px;" +
                                "direction: ltr;" +
                            "}" +
                            "body {" +
                                "font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif;" +
                                "font-size: 14px;" +
                                "line-height: 1.42857143;" +
                                "color: #333;" +
                                "background-color: #fff;" +
                            "}" +
                            "Inherited from html html {" +
                                "font-size: 10px;" +
                                "-webkit-tap-highlight-color: rgba(0,0,0,0);" +
                            "}" +
                            "html {" +
                                "font-family: sans-serif;" +
                                "-webkit-text-size-adjust: 100%;" +
                                "-ms-text-size-adjust: 100%;" +
                            "}" +
                            "div, input, select, textarea, span, img, table, label, td, th, p, a, button, ul, code, pre, li {" +
                                "-webkit-border-radius: 0 !important;" +
                                "-moz-border-radius: 0 !important;" +
                                "border-radius: 0 !important;" +
                            "}" +
                            "* {" +
                                "-webkit-box-sizing: border-box;" +
                                "-moz-box-sizing: border-box;" +
                                "box-sizing: border-box;" +
                            "}" +
                            "@media (min-width: 1200px) {" +
                                ".container {" +
                                    "width: 1170px;" +
                                "}" +
                            "}" +
                            "@media (min-width: 992px) {" +
                                ".container {" +
                                    "width: 970px;" +
                                "}" +
                            "}" +
                            "@media (min-width: 768px) {" +
                                ".container {" +
                                    "width: 750px;" +
                                "}" +
                            "}" +
                            ".container {" +
                                "padding-right: 15px;" +
                                "padding-left: 15px;" +
                                "margin-right: auto;" +
                                "margin-left: auto;" +
                            "}" +
                                    ".row {" +
                                        "margin-right: -15px;" +
                                        "margin-left: -15px;" +
                                    "}" +
                                    ".col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9 {" +
                                "float: left;" +
                            "}" +
                            ".col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {" +
                                "position: relative;" +
                                "min-height: 1px;" +
                                "padding-right: 15px;" +
                                "padding-left: 15px;" +
                            "}" +
                            ".portlet.light {" +
                                "padding: 12px 20px 15px 20px;" +
                                "background-color: #fff;" +
                            "}" +
                            ".portlet {" +
                                "margin-top: 0px;" +
                                "margin-bottom: 25px;" +
                                "padding: 0px;" +
                                "-webkit-border-radius: 4px;" +
                                "-moz-border-radius: 4px;" +
                                "-ms-border-radius: 4px;" +
                                "-o-border-radius: 4px;" +
                                "border-radius: 4px;" +
                            "}" +
                            ".portlet.light > .portlet-title {" +
                                "padding: 0;" +
                                "min-height: 48px;" +
                            "}" +
                            ".portlet > .portlet-title {" +
                                "border-bottom: 1px solid #eee;" +
                                "padding: 0;" +
                                "margin-bottom: 10px;" +
                                "min-height: 41px;" +
                                "-webkit-border-radius: 4px 4px 0 0;" +
                                "-moz-border-radius: 4px 4px 0 0;" +
                                "-ms-border-radius: 4px 4px 0 0;" +
                                "-o-border-radius: 4px 4px 0 0;" +
                                "border-radius: 4px 4px 0 0;" +
                            "}" +
                            ".portlet.light > .portlet-title > .caption {" +
                                "color: #666;" +
                                "padding: 10px 0;" +
                            "}" +
                            ".portlet > .portlet-title > .caption {" +
                                "float: left;" +
                                "display: inline-block;" +
                                "font-size: 18px;" +
                                "line-height: 18px;" +
                                "padding: 10px 0;" +
                            "}" +
                            ".uppercase {" +
                                "text-transform: uppercase !important;" +
                            "}" +
                            ".bold {" +
                                "font-weight: 700 !important;" +
                            "}" +
                            "h2 {" +
                                "font-size: 27px;" +
                            "}" +
                            "h3 {" +
                                "font-size: 23px;" +
                            "}" +
                            "h4 {" +
                                "font-size: 17px;" +
                            "}" +
                            ".h4, h4 {" +
                                "font-size: 18px;" +
                            "}" +
                            "h1, h2, h3, h4, h5, h6 {" +
                                "font-family: 'Open Sans', sans-serif;" +
                                "font-weight: 300;" +
                            "}" +
                            ".h2, h2 {" +
                                "font-size: 30px;" +
                            "}" +
                            ".h1, .h2, .h3, h1, h2, h3 {" +
                                "margin-top: 20px;" +
                                "margin-bottom: 10px;" +
                            "}" +
                            ".h1, .h2, .h3, .h4, .h5, .h6, h1, h2, h3, h4, h5, h6 {" +
                                "font-family: inherit;" +
                                "font-weight: 500;" +
                                "line-height: 1.1;" +
                                "color: inherit;" +
                            "}" +
                            "* {" +
                                "-webkit-box-sizing: border-box;" +
                                "-moz-box-sizing: border-box;" +
                                "box-sizing: border-box;" +
                            "}" +
                            "user agent stylesheeth2 {" +
                                "display: block;" +
                                "font-size: 1.5em;" +
                                "-webkit-margin-before: 0.83em;" +
                                "-webkit-margin-after: 0.83em;" +
                                "-webkit-margin-start: 0px;" +
                                "-webkit-margin-end: 0px;" +
                                "font-weight: bold;" +
                            "}" +
                            ".table {" +
                                "width: 100%;" +
                                "max-width: 100%;" +
                                "margin-bottom: 20px;" +
                            "}" +
                            ".font-green-sharp {" +
                                "color: #4DB3A2 !important;" +
                            "}" +
                            ".uppercase {" +
                                "text-transform: uppercase !important;" +
                            "}" +
                            ".bold {" +
                                "font-weight: 700 !important;" +
                            "}" +
                            ".font-blue {" +
                                "color: #3598dc !important;" +
                            "}" +
                            "hr {" +
                                "margin: 20px 0;" +
                                "border: 0;" +
                                "border-top: 1px solid #eee;" +
                                "border-bottom: 0;" +
                            "}" +
                            "hr {" +
                                "margin-top: 20px;" +
                                "margin-bottom: 20px;" +
                                "border: 0;" +
                                "border-top: 1px solid #eee;" +
                            "}" +
                            "hr {" +
                                "height: 0;" +
                                "-webkit-box-sizing: content-box;" +
                                "-moz-box-sizing: content-box;" +
                                "box-sizing: content-box;" +
                            "}" +
                            "* {" +
                                "-webkit-box-sizing: border-box;" +
                                "-moz-box-sizing: border-box;" +
                                "box-sizing: border-box;" +
                            "}" +
                            "user agent stylesheethr {" +
                                "display: block;" +
                                "-webkit-margin-before: 0.5em;" +
                                "-webkit-margin-after: 0.5em;" +
                                "-webkit-margin-start: auto;" +
                                "-webkit-margin-end: auto;" +
                                "border-style: inset;" +
                                "border-width: 1px;" +
                            "}" +
                        "</style>" +
                        "</head>" +
                        "<body>" +
                            "<div class='page-head'>" +
                                "<div class='container'>" +
                                    "<div class='table'>" +
                                        "<div class='row'>" +
                                            "<div class='col-md-12'>" +
                                                "<div class='col-md-3'>" +
                                                "</div>" +
                                                "<div class='col-md-7 footer-gray portlet light '>" +
                                                    "<div class='portlet-title '>" +
                                                        "<div class='caption'>" +
                                                            "<h2 class='caption font-green-sharp bold uppercase'>Bienvenido a la Universidad YMCA</h2>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                            "<div class='col-md-12'>" +
                                                "<div class='col-md-3'>" +
                                                "</div>" +
                                                "<div class='col-md-7 footer-gray portlet light portlet-title'>" +
                                                    "<h3 class='caption font-blue'>" + opjAl.Nombre + " " + opjAl.Paterno + " " + opjAl.Materno + "</h3>" +
                                                    "<hr />" +
                                                    "<h3 class='caption font-blue'>Los siguientes datos son tus credenciales para poder acceder al portal de la universidad</h3>" +
                                                    "<hr />" +
                                                    "<h3 class='caption font-blue'>Usuario</h3>" +
                                                    "<h4 class='caption font-blue-dark'>" + opjAl.AlumnoId.ToString() + "</h4>" +
                                                        "<h3 class='caption font-blue'>Contraseña</h3>" +
                                                    "<h4 class='caption font-blue-dark'>" + Pass + "</h4>" +
                                                    "<hr />" +
                                                    "<h3 class='caption font-blue'>Puedes acceder a el desde el siguiente enlace.</h3>" +
                                                    "<a class='caption font-blue-dark' href='http://108.163.172.122/portalalumno/login.html'>YMCA</a>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</body>" +
                        "</html>";
            #endregion
            return Email.Enviar(objCuenta.Email, objCuenta.Password, objCuenta.DisplayName, opjAl.DTOAlumnoDetalle.Email, ',', "programador1@ymcacdmex.org.mx", ';', "Portal YMCA Universidad", body, "", ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true, ref refere);

            //return Email.Enviar("sistemastest@ymcacdmex.org.mx", "Sis99TesX1", "Bienvenido", opjAl.DTOAlumnoDetalle.Email, ',', "Universidad YMCA", body, "", ',', "mail.ymcacdmex.org.mx", 587, false, true, ref refere);
        }
        public bool EnviarMail11(int Alumnoid)
        {
            try
            {
                var refere = new Utilities.ProcessResult();
                DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();
                DTOAlumnoPassword objPas = BLLAlumnoPassword.ConsultarAlumnoPassword(Alumnoid);
                objPas.Password = Utilities.Seguridad.Desencripta(27, objPas.Password);
                string body = "";
                DTOAlumno opjAl = BLLAlumnoPortal.ObtenerAlumno(objPas.AlumnoId);
                #region "HTML"
                body = "<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>" +
                            "<head>" +
                            "<meta charset='utf-8' />" +
                            "<title>Bienvenida Alumnos</title>" +
                            "<meta http-equiv='X-UA-Compatible' content='IE=edge' />" +
                            "<meta content='width=device-width, initial-scale=1.0' name='viewport' />" +
                            "<meta http-equiv='Content-type' content='text/html; charset=utf-8' />" +
                            "<meta content='' name='description' />" +
                            "<meta content='' name='author' />" +
                           "<style>" +
                                "body {" +
                                    "color: #333333;" +
                                    "font-family: 'Open Sans', sans-serif;" +
                                    "padding: 0px !important;" +
                                    "margin: 0px !important;" +
                                    "font-size: 13px;" +
                                    "direction: ltr;" +
                                "}" +
                                "body {" +
                                    "font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif;" +
                                    "font-size: 14px;" +
                                    "line-height: 1.42857143;" +
                                    "color: #333;" +
                                    "background-color: #fff;" +
                                "}" +
                                "Inherited from html html {" +
                                    "font-size: 10px;" +
                                    "-webkit-tap-highlight-color: rgba(0,0,0,0);" +
                                "}" +
                                "html {" +
                                    "font-family: sans-serif;" +
                                    "-webkit-text-size-adjust: 100%;" +
                                    "-ms-text-size-adjust: 100%;" +
                                "}" +
                                "div, input, select, textarea, span, img, table, label, td, th, p, a, button, ul, code, pre, li {" +
                                    "-webkit-border-radius: 0 !important;" +
                                    "-moz-border-radius: 0 !important;" +
                                    "border-radius: 0 !important;" +
                                "}" +
                                "* {" +
                                    "-webkit-box-sizing: border-box;" +
                                    "-moz-box-sizing: border-box;" +
                                    "box-sizing: border-box;" +
                                "}" +
                                "@media (min-width: 1200px) {" +
                                    ".container {" +
                                        "width: 1170px;" +
                                    "}" +
                                "}" +
                                "@media (min-width: 992px) {" +
                                    ".container {" +
                                        "width: 970px;" +
                                    "}" +
                                "}" +
                                "@media (min-width: 768px) {" +
                                    ".container {" +
                                        "width: 750px;" +
                                    "}" +
                                "}" +
                                ".container {" +
                                    "padding-right: 15px;" +
                                    "padding-left: 15px;" +
                                    "margin-right: auto;" +
                                    "margin-left: auto;" +
                                "}" +
                                        ".row {" +
                                            "margin-right: -15px;" +
                                            "margin-left: -15px;" +
                                        "}" +
                                        ".col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9 {" +
                                    "float: left;" +
                                "}" +
                                ".col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {" +
                                    "position: relative;" +
                                    "min-height: 1px;" +
                                    "padding-right: 15px;" +
                                    "padding-left: 15px;" +
                                "}" +
                                ".portlet.light {" +
                                    "padding: 12px 20px 15px 20px;" +
                                    "background-color: #fff;" +
                                "}" +
                                ".portlet {" +
                                    "margin-top: 0px;" +
                                    "margin-bottom: 25px;" +
                                    "padding: 0px;" +
                                    "-webkit-border-radius: 4px;" +
                                    "-moz-border-radius: 4px;" +
                                    "-ms-border-radius: 4px;" +
                                    "-o-border-radius: 4px;" +
                                    "border-radius: 4px;" +
                                "}" +
                                ".portlet.light > .portlet-title {" +
                                    "padding: 0;" +
                                    "min-height: 48px;" +
                                "}" +
                                ".portlet > .portlet-title {" +
                                    "border-bottom: 1px solid #eee;" +
                                    "padding: 0;" +
                                    "margin-bottom: 10px;" +
                                    "min-height: 41px;" +
                                    "-webkit-border-radius: 4px 4px 0 0;" +
                                    "-moz-border-radius: 4px 4px 0 0;" +
                                    "-ms-border-radius: 4px 4px 0 0;" +
                                    "-o-border-radius: 4px 4px 0 0;" +
                                    "border-radius: 4px 4px 0 0;" +
                                "}" +
                                ".portlet.light > .portlet-title > .caption {" +
                                    "color: #666;" +
                                    "padding: 10px 0;" +
                                "}" +
                                ".portlet > .portlet-title > .caption {" +
                                    "float: left;" +
                                    "display: inline-block;" +
                                    "font-size: 18px;" +
                                    "line-height: 18px;" +
                                    "padding: 10px 0;" +
                                "}" +
                                ".uppercase {" +
                                    "text-transform: uppercase !important;" +
                                "}" +
                                ".bold {" +
                                    "font-weight: 700 !important;" +
                                "}" +
                                "h2 {" +
                                    "font-size: 27px;" +
                                "}" +
                                "h3 {" +
                                    "font-size: 23px;" +
                                "}" +
                                "h4 {" +
                                    "font-size: 17px;" +
                                "}" +
                                ".h4, h4 {" +
                                    "font-size: 18px;" +
                                "}" +
                                "h1, h2, h3, h4, h5, h6 {" +
                                    "font-family: 'Open Sans', sans-serif;" +
                                    "font-weight: 300;" +
                                "}" +
                                ".h2, h2 {" +
                                    "font-size: 30px;" +
                                "}" +
                                ".h1, .h2, .h3, h1, h2, h3 {" +
                                    "margin-top: 20px;" +
                                    "margin-bottom: 10px;" +
                                "}" +
                                ".h1, .h2, .h3, .h4, .h5, .h6, h1, h2, h3, h4, h5, h6 {" +
                                    "font-family: inherit;" +
                                    "font-weight: 500;" +
                                    "line-height: 1.1;" +
                                    "color: inherit;" +
                                "}" +
                                "* {" +
                                    "-webkit-box-sizing: border-box;" +
                                    "-moz-box-sizing: border-box;" +
                                    "box-sizing: border-box;" +
                                "}" +
                                "user agent stylesheeth2 {" +
                                    "display: block;" +
                                    "font-size: 1.5em;" +
                                    "-webkit-margin-before: 0.83em;" +
                                    "-webkit-margin-after: 0.83em;" +
                                    "-webkit-margin-start: 0px;" +
                                    "-webkit-margin-end: 0px;" +
                                    "font-weight: bold;" +
                                "}" +
                                ".table {" +
                                    "width: 100%;" +
                                    "max-width: 100%;" +
                                    "margin-bottom: 20px;" +
                                "}" +
                                ".font-green-sharp {" +
                                    "color: #4DB3A2 !important;" +
                                "}" +
                                ".uppercase {" +
                                    "text-transform: uppercase !important;" +
                                "}" +
                                ".bold {" +
                                    "font-weight: 700 !important;" +
                                "}" +
                                ".font-blue {" +
                                    "color: #3598dc !important;" +
                                "}" +
                                "hr {" +
                                    "margin: 20px 0;" +
                                    "border: 0;" +
                                    "border-top: 1px solid #eee;" +
                                    "border-bottom: 0;" +
                                "}" +
                                "hr {" +
                                    "margin-top: 20px;" +
                                    "margin-bottom: 20px;" +
                                    "border: 0;" +
                                    "border-top: 1px solid #eee;" +
                                "}" +
                                "hr {" +
                                    "height: 0;" +
                                    "-webkit-box-sizing: content-box;" +
                                    "-moz-box-sizing: content-box;" +
                                    "box-sizing: content-box;" +
                                "}" +
                                "* {" +
                                    "-webkit-box-sizing: border-box;" +
                                    "-moz-box-sizing: border-box;" +
                                    "box-sizing: border-box;" +
                                "}" +
                                "user agent stylesheethr {" +
                                    "display: block;" +
                                    "-webkit-margin-before: 0.5em;" +
                                    "-webkit-margin-after: 0.5em;" +
                                    "-webkit-margin-start: auto;" +
                                    "-webkit-margin-end: auto;" +
                                    "border-style: inset;" +
                                    "border-width: 1px;" +
                                "}" +
                            "</style>" +
                            "</head>" +
                            "<body>" +
                                "<div class='page-head'>" +
                                    "<div class='container'>" +
                                        "<div class='table'>" +
                                            "<div class='row'>" +
                                                "<div class='col-md-12'>" +
                                                    "<div class='col-md-3'>" +
                                                    "</div>" +
                                                    "<div class='col-md-7 footer-gray portlet light '>" +
                                                        "<div class='portlet-title '>" +
                                                            "<div class='caption'>" +
                                                                "<h2 class='caption font-green-sharp bold uppercase'>Bienvenido a la Universidad YMCA</h2>" +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class='col-md-12'>" +
                                                    "<div class='col-md-3'>" +
                                                    "</div>" +
                                                    "<div class='col-md-7 footer-gray portlet light portlet-title'>" +
                                                        "<h3 class='caption font-blue'>" + opjAl.Nombre + " " + opjAl.Paterno + " " + opjAl.Materno + "</h3>" +
                                                        "<hr />" +
                                                        "<h3 class='caption font-blue'>Los siguientes datos son tus credenciales para poder acceder al portal de la universidad</h3>" +
                                                        "<hr />" +
                                                        "<h3 class='caption font-blue'>Usuario</h3>" +
                                                        "<h4 class='caption font-blue-dark'>" + opjAl.AlumnoId.ToString() + "</h4>" +
                                                            "<h3 class='caption font-blue'>Contraseña</h3>" +
                                                        "<h4 class='caption font-blue-dark'>" + objPas.Password + "</h4>" +
                                                        "<hr />" +
                                                        "<h3 class='caption font-blue'>Puedes acceder a el desde el siguiente enlace.</h3>" +
                                                        "<a class='caption font-blue-dark' href='http://108.163.172.122/portalalumno/login.html'>YMCA</a>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</body>" +
                            "</html>";
                #endregion
                return Email.Enviar(objCuenta.Email, objCuenta.Password, objCuenta.DisplayName, opjAl.DTOAlumnoDetalle.Email, ',', "Recordatario - Portal YMCA Universidad", body, "", ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true, ref refere);
            }
            catch
            {
                return false;
            }
        }
        [WebMethod]
        public string EnviarMailUpdate(string AlumnoId, string UsuarioId)
        {
            string Fallidos = "";
            DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();
            try
            {
                var refere = new Utilities.ProcessResult();
                DTOAlumnoPassword objPas = BLLAlumnoPassword.ConsultarAlumnoPassword(int.Parse(AlumnoId));
                string body = "";
                if (objPas == null)
                {
                    BLLAlumnoPassword.GuardarPassword(int.Parse(AlumnoId), Utilities.Seguridad.Encripta(27, Herramientas.ConvertidorT.CrearPass()));
                    objPas = BLLAlumnoPassword.ConsultarAlumnoPassword(int.Parse(AlumnoId));
                }
                DTOUsuarioDetalle objUsDetalle = BLLUsuarioPortal.ObtenerDetalle(int.Parse(UsuarioId));
                objPas.Password = Utilities.Seguridad.Desencripta(27, objPas.Password);
                DTOAlumno opjAl = BLLAlumnoPortal.ObtenerAlumno(objPas.AlumnoId);
                #region "HTML"
                body = "<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>" +
                            "<head>" +
                            "<meta charset='utf-8' />" +
                            "<title>Bienvenida Alumnos</title>" +
                            "<meta http-equiv='X-UA-Compatible' content='IE=edge' />" +
                            "<meta content='width=device-width, initial-scale=1.0' name='viewport' />" +
                            "<meta http-equiv='Content-type' content='text/html; charset=utf-8' />" +
                            "<meta content='' name='description' />" +
                            "<meta content='' name='author' />" +
                           "<style>" +
                                "body {" +
                                    "color: #333333;" +
                                    "font-family: 'Open Sans', sans-serif;" +
                                    "padding: 0px !important;" +
                                    "margin: 0px !important;" +
                                    "font-size: 13px;" +
                                    "direction: ltr;" +
                                "}" +
                                "body {" +
                                    "font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif;" +
                                    "font-size: 14px;" +
                                    "line-height: 1.42857143;" +
                                    "color: #333;" +
                                    "background-color: #fff;" +
                                "}" +
                                "Inherited from html html {" +
                                    "font-size: 10px;" +
                                    "-webkit-tap-highlight-color: rgba(0,0,0,0);" +
                                "}" +
                                "html {" +
                                    "font-family: sans-serif;" +
                                    "-webkit-text-size-adjust: 100%;" +
                                    "-ms-text-size-adjust: 100%;" +
                                "}" +
                                "div, input, select, textarea, span, img, table, label, td, th, p, a, button, ul, code, pre, li {" +
                                    "-webkit-border-radius: 0 !important;" +
                                    "-moz-border-radius: 0 !important;" +
                                    "border-radius: 0 !important;" +
                                "}" +
                                "* {" +
                                    "-webkit-box-sizing: border-box;" +
                                    "-moz-box-sizing: border-box;" +
                                    "box-sizing: border-box;" +
                                "}" +
                                "@media (min-width: 1200px) {" +
                                    ".container {" +
                                        "width: 1170px;" +
                                    "}" +
                                "}" +
                                "@media (min-width: 992px) {" +
                                    ".container {" +
                                        "width: 970px;" +
                                    "}" +
                                "}" +
                                "@media (min-width: 768px) {" +
                                    ".container {" +
                                        "width: 750px;" +
                                    "}" +
                                "}" +
                                ".container {" +
                                    "padding-right: 15px;" +
                                    "padding-left: 15px;" +
                                    "margin-right: auto;" +
                                    "margin-left: auto;" +
                                "}" +
                                        ".row {" +
                                            "margin-right: -15px;" +
                                            "margin-left: -15px;" +
                                        "}" +
                                        ".col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9 {" +
                                    "float: left;" +
                                "}" +
                                ".col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {" +
                                    "position: relative;" +
                                    "min-height: 1px;" +
                                    "padding-right: 15px;" +
                                    "padding-left: 15px;" +
                                "}" +
                                ".portlet.light {" +
                                    "padding: 12px 20px 15px 20px;" +
                                    "background-color: #fff;" +
                                "}" +
                                ".portlet {" +
                                    "margin-top: 0px;" +
                                    "margin-bottom: 25px;" +
                                    "padding: 0px;" +
                                    "-webkit-border-radius: 4px;" +
                                    "-moz-border-radius: 4px;" +
                                    "-ms-border-radius: 4px;" +
                                    "-o-border-radius: 4px;" +
                                    "border-radius: 4px;" +
                                "}" +
                                ".portlet.light > .portlet-title {" +
                                    "padding: 0;" +
                                    "min-height: 48px;" +
                                "}" +
                                ".portlet > .portlet-title {" +
                                    "border-bottom: 1px solid #eee;" +
                                    "padding: 0;" +
                                    "margin-bottom: 10px;" +
                                    "min-height: 41px;" +
                                    "-webkit-border-radius: 4px 4px 0 0;" +
                                    "-moz-border-radius: 4px 4px 0 0;" +
                                    "-ms-border-radius: 4px 4px 0 0;" +
                                    "-o-border-radius: 4px 4px 0 0;" +
                                    "border-radius: 4px 4px 0 0;" +
                                "}" +
                                ".portlet.light > .portlet-title > .caption {" +
                                    "color: #666;" +
                                    "padding: 10px 0;" +
                                "}" +
                                ".portlet > .portlet-title > .caption {" +
                                    "float: left;" +
                                    "display: inline-block;" +
                                    "font-size: 18px;" +
                                    "line-height: 18px;" +
                                    "padding: 10px 0;" +
                                "}" +
                                ".uppercase {" +
                                    "text-transform: uppercase !important;" +
                                "}" +
                                ".bold {" +
                                    "font-weight: 700 !important;" +
                                "}" +
                                "h2 {" +
                                    "font-size: 27px;" +
                                "}" +
                                "h3 {" +
                                    "font-size: 23px;" +
                                "}" +
                                "h4 {" +
                                    "font-size: 17px;" +
                                "}" +
                                ".h4, h4 {" +
                                    "font-size: 18px;" +
                                "}" +
                                "h1, h2, h3, h4, h5, h6 {" +
                                    "font-family: 'Open Sans', sans-serif;" +
                                    "font-weight: 300;" +
                                "}" +
                                ".h2, h2 {" +
                                    "font-size: 30px;" +
                                "}" +
                                ".h1, .h2, .h3, h1, h2, h3 {" +
                                    "margin-top: 20px;" +
                                    "margin-bottom: 10px;" +
                                "}" +
                                ".h1, .h2, .h3, .h4, .h5, .h6, h1, h2, h3, h4, h5, h6 {" +
                                    "font-family: inherit;" +
                                    "font-weight: 500;" +
                                    "line-height: 1.1;" +
                                    "color: inherit;" +
                                "}" +
                                "* {" +
                                    "-webkit-box-sizing: border-box;" +
                                    "-moz-box-sizing: border-box;" +
                                    "box-sizing: border-box;" +
                                "}" +
                                "user agent stylesheeth2 {" +
                                    "display: block;" +
                                    "font-size: 1.5em;" +
                                    "-webkit-margin-before: 0.83em;" +
                                    "-webkit-margin-after: 0.83em;" +
                                    "-webkit-margin-start: 0px;" +
                                    "-webkit-margin-end: 0px;" +
                                    "font-weight: bold;" +
                                "}" +
                                ".table {" +
                                    "width: 100%;" +
                                    "max-width: 100%;" +
                                    "margin-bottom: 20px;" +
                                "}" +
                                ".font-green-sharp {" +
                                    "color: #4DB3A2 !important;" +
                                "}" +
                                ".uppercase {" +
                                    "text-transform: uppercase !important;" +
                                "}" +
                                ".bold {" +
                                    "font-weight: 700 !important;" +
                                "}" +
                                ".font-blue {" +
                                    "color: #3598dc !important;" +
                                "}" +
                                "hr {" +
                                    "margin: 20px 0;" +
                                    "border: 0;" +
                                    "border-top: 1px solid #eee;" +
                                    "border-bottom: 0;" +
                                "}" +
                                "hr {" +
                                    "margin-top: 20px;" +
                                    "margin-bottom: 20px;" +
                                    "border: 0;" +
                                    "border-top: 1px solid #eee;" +
                                "}" +
                                "hr {" +
                                    "height: 0;" +
                                    "-webkit-box-sizing: content-box;" +
                                    "-moz-box-sizing: content-box;" +
                                    "box-sizing: content-box;" +
                                "}" +
                                "* {" +
                                    "-webkit-box-sizing: border-box;" +
                                    "-moz-box-sizing: border-box;" +
                                    "box-sizing: border-box;" +
                                "}" +
                                "user agent stylesheethr {" +
                                    "display: block;" +
                                    "-webkit-margin-before: 0.5em;" +
                                    "-webkit-margin-after: 0.5em;" +
                                    "-webkit-margin-start: auto;" +
                                    "-webkit-margin-end: auto;" +
                                    "border-style: inset;" +
                                    "border-width: 1px;" +
                                "}" +
                            "</style>" +
                            "</head>" +
                            "<body>" +
                                "<div class='page-head'>" +
                                    "<div class='container'>" +
                                        "<div class='table'>" +
                                            "<div class='row'>" +
                                                "<div class='col-md-12'>" +
                                                    "<div class='col-md-3'>" +
                                                    "</div>" +
                                                    "<div class='col-md-7 footer-gray portlet light '>" +
                                                        "<div class='portlet-title '>" +
                                                            "<div class='caption'>" +
                                                                "<h2 class='caption font-green-sharp bold uppercase'>Bienvenido a la Universidad YMCA</h2>" +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class='col-md-12'>" +
                                                    "<div class='col-md-3'>" +
                                                    "</div>" +
                                                    "<div class='col-md-7 footer-gray portlet light portlet-title'>" +
                                                        "<h3 class='caption font-blue'>" + opjAl.Nombre + " " + opjAl.Paterno + " " + opjAl.Materno + "</h3>" +
                                                        "<hr />" +
                                                        "<h3 class='caption font-blue'>Los siguientes datos son tus credenciales para poder acceder al portal de la universidad</h3>" +
                                                        "<hr />" +
                                                        "<h3 class='caption font-blue'>Usuario</h3>" +
                                                        "<h4 class='caption font-blue-dark'>" + opjAl.AlumnoId.ToString() + "</h4>" +
                                                            "<h3 class='caption font-blue'>Contraseña</h3>" +
                                                        "<h4 class='caption font-blue-dark'>" + objPas.Password + "</h4>" +
                                                        "<hr />" +
                                                        "<h3 class='caption font-blue'>Puedes acceder a el desde el siguiente enlace.</h3>" +
                                                        "<a class='caption font-blue-dark' href='http://108.163.172.122/portalalumno/login.html'>YMCA</a>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</body>" +
                            "</html>";
                #endregion
                Email.Enviar(objCuenta.Email, objCuenta.Password, objCuenta.DisplayName, opjAl.DTOAlumnoDetalle.Email, ',', "programador1@ymcacdmex.org.mx;" + objUsDetalle.Email, ';', "Claves de acceso Portal Universidad YMCA", body, "", ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true, ref refere);
            }
            catch (Exception e)
            {
                Fallidos = AlumnoId + " " + e.Message;
            }

            return Fallidos;
            //return Email.Enviar("sistemastest@ymcacdmex.org.mx", "Sis99TesX1", "Bienvenido", opjAl.DTOAlumnoDetalle.Email, ',', "Universidad YMCA", body, "", ',', "mail.ymcacdmex.org.mx", 587, false, true, ref refere);
        }

        [WebMethod]
        public string EnviarMail2(string listaID)
        {
            string Fallidos = "";
            DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();
            string[] ListaAlumnos = listaID.Split(',');
            foreach (string AlumnoId in ListaAlumnos)
            {
                try
                {
                    var refere = new Utilities.ProcessResult();
                    DTOAlumnoPassword objPas = null;
                    objPas = BLLAlumnoPassword.ConsultarAlumnoPassword(int.Parse(AlumnoId));
                    if (objPas == null)
                    {
                        BLLAlumnoPassword.GuardarPassword(int.Parse(AlumnoId), Utilities.Seguridad.Encripta(27, Herramientas.ConvertidorT.CrearPass()));
                        objPas = BLLAlumnoPassword.ConsultarAlumnoPassword(int.Parse(AlumnoId));
                    }
                    objPas.Password = Utilities.Seguridad.Desencripta(27, objPas.Password);
                    string body = "";
                    DTOAlumno opjAl = BLLAlumnoPortal.ObtenerAlumno(objPas.AlumnoId);
                    string ruta = opjAl.Plantel == 1
                                ? @"C:\inetpub\wwwroot\portaladministrativo\Documentos\Reglamento.pdf"
                                : opjAl.Plantel == 2
                                ? @"C:\inetpub\wwwroot\portaladministrativo\Documentos\ReglamentoCamohmila.pdf"
                                : "";
                    #region "HTML"
                    body = "<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>" +
                                "<head>" +
                                "<meta charset='utf-8' />" +
                                "<title>Bienvenida Alumnos</title>" +
                                "<meta http-equiv='X-UA-Compatible' content='IE=edge' />" +
                                "<meta content='width=device-width, initial-scale=1.0' name='viewport' />" +
                                "<meta http-equiv='Content-type' content='text/html; charset=utf-8' />" +
                                "<meta content='' name='description' />" +
                                "<meta content='' name='author' />" +
                               "<style>" +
                                    "body {" +
                                        "color: #333333;" +
                                        "font-family: 'Open Sans', sans-serif;" +
                                        "padding: 0px !important;" +
                                        "margin: 0px !important;" +
                                        "font-size: 13px;" +
                                        "direction: ltr;" +
                                    "}" +
                                    "body {" +
                                        "font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif;" +
                                        "font-size: 14px;" +
                                        "line-height: 1.42857143;" +
                                        "color: #333;" +
                                        "background-color: #fff;" +
                                    "}" +
                                    "Inherited from html html {" +
                                        "font-size: 10px;" +
                                        "-webkit-tap-highlight-color: rgba(0,0,0,0);" +
                                    "}" +
                                    "html {" +
                                        "font-family: sans-serif;" +
                                        "-webkit-text-size-adjust: 100%;" +
                                        "-ms-text-size-adjust: 100%;" +
                                    "}" +
                                    "div, input, select, textarea, span, img, table, label, td, th, p, a, button, ul, code, pre, li {" +
                                        "-webkit-border-radius: 0 !important;" +
                                        "-moz-border-radius: 0 !important;" +
                                        "border-radius: 0 !important;" +
                                    "}" +
                                    "* {" +
                                        "-webkit-box-sizing: border-box;" +
                                        "-moz-box-sizing: border-box;" +
                                        "box-sizing: border-box;" +
                                    "}" +
                                    "@media (min-width: 1200px) {" +
                                        ".container {" +
                                            "width: 1170px;" +
                                        "}" +
                                    "}" +
                                    "@media (min-width: 992px) {" +
                                        ".container {" +
                                            "width: 970px;" +
                                        "}" +
                                    "}" +
                                    "@media (min-width: 768px) {" +
                                        ".container {" +
                                            "width: 750px;" +
                                        "}" +
                                    "}" +
                                    ".container {" +
                                        "padding-right: 15px;" +
                                        "padding-left: 15px;" +
                                        "margin-right: auto;" +
                                        "margin-left: auto;" +
                                    "}" +
                                            ".row {" +
                                                "margin-right: -15px;" +
                                                "margin-left: -15px;" +
                                            "}" +
                                            ".col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9 {" +
                                        "float: left;" +
                                    "}" +
                                    ".col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {" +
                                        "position: relative;" +
                                        "min-height: 1px;" +
                                        "padding-right: 15px;" +
                                        "padding-left: 15px;" +
                                    "}" +
                                    ".portlet.light {" +
                                        "padding: 12px 20px 15px 20px;" +
                                        "background-color: #fff;" +
                                    "}" +
                                    ".portlet {" +
                                        "margin-top: 0px;" +
                                        "margin-bottom: 25px;" +
                                        "padding: 0px;" +
                                        "-webkit-border-radius: 4px;" +
                                        "-moz-border-radius: 4px;" +
                                        "-ms-border-radius: 4px;" +
                                        "-o-border-radius: 4px;" +
                                        "border-radius: 4px;" +
                                    "}" +
                                    ".portlet.light > .portlet-title {" +
                                        "padding: 0;" +
                                        "min-height: 48px;" +
                                    "}" +
                                    ".portlet > .portlet-title {" +
                                        "border-bottom: 1px solid #eee;" +
                                        "padding: 0;" +
                                        "margin-bottom: 10px;" +
                                        "min-height: 41px;" +
                                        "-webkit-border-radius: 4px 4px 0 0;" +
                                        "-moz-border-radius: 4px 4px 0 0;" +
                                        "-ms-border-radius: 4px 4px 0 0;" +
                                        "-o-border-radius: 4px 4px 0 0;" +
                                        "border-radius: 4px 4px 0 0;" +
                                    "}" +
                                    ".portlet.light > .portlet-title > .caption {" +
                                        "color: #666;" +
                                        "padding: 10px 0;" +
                                    "}" +
                                    ".portlet > .portlet-title > .caption {" +
                                        "float: left;" +
                                        "display: inline-block;" +
                                        "font-size: 18px;" +
                                        "line-height: 18px;" +
                                        "padding: 10px 0;" +
                                    "}" +
                                    ".uppercase {" +
                                        "text-transform: uppercase !important;" +
                                    "}" +
                                    ".bold {" +
                                        "font-weight: 700 !important;" +
                                    "}" +
                                    "h2 {" +
                                        "font-size: 27px;" +
                                    "}" +
                                    "h3 {" +
                                        "font-size: 23px;" +
                                    "}" +
                                    "h4 {" +
                                        "font-size: 17px;" +
                                    "}" +
                                    ".h4, h4 {" +
                                        "font-size: 18px;" +
                                    "}" +
                                    "h1, h2, h3, h4, h5, h6 {" +
                                        "font-family: 'Open Sans', sans-serif;" +
                                        "font-weight: 300;" +
                                    "}" +
                                    ".h2, h2 {" +
                                        "font-size: 30px;" +
                                    "}" +
                                    ".h1, .h2, .h3, h1, h2, h3 {" +
                                        "margin-top: 20px;" +
                                        "margin-bottom: 10px;" +
                                    "}" +
                                    ".h1, .h2, .h3, .h4, .h5, .h6, h1, h2, h3, h4, h5, h6 {" +
                                        "font-family: inherit;" +
                                        "font-weight: 500;" +
                                        "line-height: 1.1;" +
                                        "color: inherit;" +
                                    "}" +
                                    "* {" +
                                        "-webkit-box-sizing: border-box;" +
                                        "-moz-box-sizing: border-box;" +
                                        "box-sizing: border-box;" +
                                    "}" +
                                    "user agent stylesheeth2 {" +
                                        "display: block;" +
                                        "font-size: 1.5em;" +
                                        "-webkit-margin-before: 0.83em;" +
                                        "-webkit-margin-after: 0.83em;" +
                                        "-webkit-margin-start: 0px;" +
                                        "-webkit-margin-end: 0px;" +
                                        "font-weight: bold;" +
                                    "}" +
                                    ".table {" +
                                        "width: 100%;" +
                                        "max-width: 100%;" +
                                        "margin-bottom: 20px;" +
                                    "}" +
                                    ".font-green-sharp {" +
                                        "color: #4DB3A2 !important;" +
                                    "}" +
                                    ".uppercase {" +
                                        "text-transform: uppercase !important;" +
                                    "}" +
                                    ".bold {" +
                                        "font-weight: 700 !important;" +
                                    "}" +
                                    ".font-blue {" +
                                        "color: #3598dc !important;" +
                                    "}" +
                                    "hr {" +
                                        "margin: 20px 0;" +
                                        "border: 0;" +
                                        "border-top: 1px solid #eee;" +
                                        "border-bottom: 0;" +
                                    "}" +
                                    "hr {" +
                                        "margin-top: 20px;" +
                                        "margin-bottom: 20px;" +
                                        "border: 0;" +
                                        "border-top: 1px solid #eee;" +
                                    "}" +
                                    "hr {" +
                                        "height: 0;" +
                                        "-webkit-box-sizing: content-box;" +
                                        "-moz-box-sizing: content-box;" +
                                        "box-sizing: content-box;" +
                                    "}" +
                                    "* {" +
                                        "-webkit-box-sizing: border-box;" +
                                        "-moz-box-sizing: border-box;" +
                                        "box-sizing: border-box;" +
                                    "}" +
                                    "user agent stylesheethr {" +
                                        "display: block;" +
                                        "-webkit-margin-before: 0.5em;" +
                                        "-webkit-margin-after: 0.5em;" +
                                        "-webkit-margin-start: auto;" +
                                        "-webkit-margin-end: auto;" +
                                        "border-style: inset;" +
                                        "border-width: 1px;" +
                                    "}" +
                                "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<div class='page-head'>" +
                                        "<div class='container'>" +
                                            "<div class='table'>" +
                                                "<div class='row'>" +
                                                    "<div class='col-md-12'>" +
                                                        "<div class='col-md-3'>" +
                                                        "</div>" +
                                                        "<div class='col-md-7 footer-gray portlet light '>" +
                                                            "<div class='portlet-title '>" +
                                                                "<div class='caption'>" +
                                                                    "<h2 class='caption font-green-sharp bold uppercase'>Bienvenido a la Universidad YMCA</h2>" +
                                                                "</div>" +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<div class='col-md-12'>" +
                                                        "<div class='col-md-3'>" +
                                                        "</div>" +
                                                        "<div class='col-md-7 footer-gray portlet light portlet-title'>" +
                                                            "<h3 class='caption font-blue'>" + opjAl.Nombre + " " + opjAl.Paterno + " " + opjAl.Materno + "</h3>" +
                                                            "<hr />" +
                                                            "<h3 class='caption font-blue'>Los siguientes datos son tus credenciales para poder acceder al portal de la universidad</h3>" +
                                                            "<hr />" +
                                                            "<h3 class='caption font-blue'>Usuario</h3>" +
                                                            "<h4 class='caption font-blue-dark'>" + opjAl.AlumnoId.ToString() + "</h4>" +
                                                                "<h3 class='caption font-blue'>Contraseña</h3>" +
                                                            "<h4 class='caption font-blue-dark'>" + objPas.Password + "</h4>" +
                                                            "<hr />" +
                                                            "<h3 class='caption font-blue'>Puedes acceder a el desde el siguiente enlace.</h3>" +
                                                            "<a class='caption font-blue-dark' href='http://108.163.172.122/portalalumno/login.html'>YMCA</a>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</body>" +
                                "</html>";
                    #endregion
                    
                    Email.Enviar(objCuenta.Email, objCuenta.Password, objCuenta.DisplayName, opjAl.DTOAlumnoDetalle.Email, ',', "programador1@ymcacdmex.org.mx;obeddelcastillo@uniymca.edu.mx", ';', "Claves de acceso Portal Universidad YMCA", body,ruta, ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true, ref refere);
                }
                catch (Exception e)
                {
                    Fallidos = AlumnoId + " " + e.Message;
                }
            }
            return Fallidos;
            //return Email.Enviar("sistemastest@ymcacdmex.org.mx", "Sis99TesX1", "Bienvenido", opjAl.DTOAlumnoDetalle.Email, ',', "Universidad YMCA", body, "", ',', "mail.ymcacdmex.org.mx", 587, false, true, ref refere);
        }

        [WebMethod]
        public string EnviarMail2OtroCorreo(string listaID, string email)
        {
            string Fallidos = "";
            DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();
            string[] ListaAlumnos = listaID.Split(',');
            foreach (string AlumnoId in ListaAlumnos)
            {
                try
                {
                    var refere = new Utilities.ProcessResult();
                    DTOAlumnoPassword objPas = BLLAlumnoPassword.ConsultarAlumnoPassword(int.Parse(AlumnoId));
                    objPas.Password = Utilities.Seguridad.Desencripta(27, objPas.Password);
                    string body = "";
                    DTOAlumno opjAl = BLLAlumnoPortal.ObtenerAlumno(objPas.AlumnoId);
                    #region "HTML"
                    body = "<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>" +
                                "<head>" +
                                "<meta charset='utf-8' />" +
                                "<title>Bienvenida Alumnos</title>" +
                                "<meta http-equiv='X-UA-Compatible' content='IE=edge' />" +
                                "<meta content='width=device-width, initial-scale=1.0' name='viewport' />" +
                                "<meta http-equiv='Content-type' content='text/html; charset=utf-8' />" +
                                "<meta content='' name='description' />" +
                                "<meta content='' name='author' />" +
                               "<style>" +
                                    "body {" +
                                        "color: #333333;" +
                                        "font-family: 'Open Sans', sans-serif;" +
                                        "padding: 0px !important;" +
                                        "margin: 0px !important;" +
                                        "font-size: 13px;" +
                                        "direction: ltr;" +
                                    "}" +
                                    "body {" +
                                        "font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif;" +
                                        "font-size: 14px;" +
                                        "line-height: 1.42857143;" +
                                        "color: #333;" +
                                        "background-color: #fff;" +
                                    "}" +
                                    "Inherited from html html {" +
                                        "font-size: 10px;" +
                                        "-webkit-tap-highlight-color: rgba(0,0,0,0);" +
                                    "}" +
                                    "html {" +
                                        "font-family: sans-serif;" +
                                        "-webkit-text-size-adjust: 100%;" +
                                        "-ms-text-size-adjust: 100%;" +
                                    "}" +
                                    "div, input, select, textarea, span, img, table, label, td, th, p, a, button, ul, code, pre, li {" +
                                        "-webkit-border-radius: 0 !important;" +
                                        "-moz-border-radius: 0 !important;" +
                                        "border-radius: 0 !important;" +
                                    "}" +
                                    "* {" +
                                        "-webkit-box-sizing: border-box;" +
                                        "-moz-box-sizing: border-box;" +
                                        "box-sizing: border-box;" +
                                    "}" +
                                    "@media (min-width: 1200px) {" +
                                        ".container {" +
                                            "width: 1170px;" +
                                        "}" +
                                    "}" +
                                    "@media (min-width: 992px) {" +
                                        ".container {" +
                                            "width: 970px;" +
                                        "}" +
                                    "}" +
                                    "@media (min-width: 768px) {" +
                                        ".container {" +
                                            "width: 750px;" +
                                        "}" +
                                    "}" +
                                    ".container {" +
                                        "padding-right: 15px;" +
                                        "padding-left: 15px;" +
                                        "margin-right: auto;" +
                                        "margin-left: auto;" +
                                    "}" +
                                            ".row {" +
                                                "margin-right: -15px;" +
                                                "margin-left: -15px;" +
                                            "}" +
                                            ".col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9 {" +
                                        "float: left;" +
                                    "}" +
                                    ".col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {" +
                                        "position: relative;" +
                                        "min-height: 1px;" +
                                        "padding-right: 15px;" +
                                        "padding-left: 15px;" +
                                    "}" +
                                    ".portlet.light {" +
                                        "padding: 12px 20px 15px 20px;" +
                                        "background-color: #fff;" +
                                    "}" +
                                    ".portlet {" +
                                        "margin-top: 0px;" +
                                        "margin-bottom: 25px;" +
                                        "padding: 0px;" +
                                        "-webkit-border-radius: 4px;" +
                                        "-moz-border-radius: 4px;" +
                                        "-ms-border-radius: 4px;" +
                                        "-o-border-radius: 4px;" +
                                        "border-radius: 4px;" +
                                    "}" +
                                    ".portlet.light > .portlet-title {" +
                                        "padding: 0;" +
                                        "min-height: 48px;" +
                                    "}" +
                                    ".portlet > .portlet-title {" +
                                        "border-bottom: 1px solid #eee;" +
                                        "padding: 0;" +
                                        "margin-bottom: 10px;" +
                                        "min-height: 41px;" +
                                        "-webkit-border-radius: 4px 4px 0 0;" +
                                        "-moz-border-radius: 4px 4px 0 0;" +
                                        "-ms-border-radius: 4px 4px 0 0;" +
                                        "-o-border-radius: 4px 4px 0 0;" +
                                        "border-radius: 4px 4px 0 0;" +
                                    "}" +
                                    ".portlet.light > .portlet-title > .caption {" +
                                        "color: #666;" +
                                        "padding: 10px 0;" +
                                    "}" +
                                    ".portlet > .portlet-title > .caption {" +
                                        "float: left;" +
                                        "display: inline-block;" +
                                        "font-size: 18px;" +
                                        "line-height: 18px;" +
                                        "padding: 10px 0;" +
                                    "}" +
                                    ".uppercase {" +
                                        "text-transform: uppercase !important;" +
                                    "}" +
                                    ".bold {" +
                                        "font-weight: 700 !important;" +
                                    "}" +
                                    "h2 {" +
                                        "font-size: 27px;" +
                                    "}" +
                                    "h3 {" +
                                        "font-size: 23px;" +
                                    "}" +
                                    "h4 {" +
                                        "font-size: 17px;" +
                                    "}" +
                                    ".h4, h4 {" +
                                        "font-size: 18px;" +
                                    "}" +
                                    "h1, h2, h3, h4, h5, h6 {" +
                                        "font-family: 'Open Sans', sans-serif;" +
                                        "font-weight: 300;" +
                                    "}" +
                                    ".h2, h2 {" +
                                        "font-size: 30px;" +
                                    "}" +
                                    ".h1, .h2, .h3, h1, h2, h3 {" +
                                        "margin-top: 20px;" +
                                        "margin-bottom: 10px;" +
                                    "}" +
                                    ".h1, .h2, .h3, .h4, .h5, .h6, h1, h2, h3, h4, h5, h6 {" +
                                        "font-family: inherit;" +
                                        "font-weight: 500;" +
                                        "line-height: 1.1;" +
                                        "color: inherit;" +
                                    "}" +
                                    "* {" +
                                        "-webkit-box-sizing: border-box;" +
                                        "-moz-box-sizing: border-box;" +
                                        "box-sizing: border-box;" +
                                    "}" +
                                    "user agent stylesheeth2 {" +
                                        "display: block;" +
                                        "font-size: 1.5em;" +
                                        "-webkit-margin-before: 0.83em;" +
                                        "-webkit-margin-after: 0.83em;" +
                                        "-webkit-margin-start: 0px;" +
                                        "-webkit-margin-end: 0px;" +
                                        "font-weight: bold;" +
                                    "}" +
                                    ".table {" +
                                        "width: 100%;" +
                                        "max-width: 100%;" +
                                        "margin-bottom: 20px;" +
                                    "}" +
                                    ".font-green-sharp {" +
                                        "color: #4DB3A2 !important;" +
                                    "}" +
                                    ".uppercase {" +
                                        "text-transform: uppercase !important;" +
                                    "}" +
                                    ".bold {" +
                                        "font-weight: 700 !important;" +
                                    "}" +
                                    ".font-blue {" +
                                        "color: #3598dc !important;" +
                                    "}" +
                                    "hr {" +
                                        "margin: 20px 0;" +
                                        "border: 0;" +
                                        "border-top: 1px solid #eee;" +
                                        "border-bottom: 0;" +
                                    "}" +
                                    "hr {" +
                                        "margin-top: 20px;" +
                                        "margin-bottom: 20px;" +
                                        "border: 0;" +
                                        "border-top: 1px solid #eee;" +
                                    "}" +
                                    "hr {" +
                                        "height: 0;" +
                                        "-webkit-box-sizing: content-box;" +
                                        "-moz-box-sizing: content-box;" +
                                        "box-sizing: content-box;" +
                                    "}" +
                                    "* {" +
                                        "-webkit-box-sizing: border-box;" +
                                        "-moz-box-sizing: border-box;" +
                                        "box-sizing: border-box;" +
                                    "}" +
                                    "user agent stylesheethr {" +
                                        "display: block;" +
                                        "-webkit-margin-before: 0.5em;" +
                                        "-webkit-margin-after: 0.5em;" +
                                        "-webkit-margin-start: auto;" +
                                        "-webkit-margin-end: auto;" +
                                        "border-style: inset;" +
                                        "border-width: 1px;" +
                                    "}" +
                                "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<div class='page-head'>" +
                                        "<div class='container'>" +
                                            "<div class='table'>" +
                                                "<div class='row'>" +
                                                    "<div class='col-md-12'>" +
                                                        "<div class='col-md-3'>" +
                                                        "</div>" +
                                                        "<div class='col-md-7 footer-gray portlet light '>" +
                                                            "<div class='portlet-title '>" +
                                                                "<div class='caption'>" +
                                                                    "<h2 class='caption font-green-sharp bold uppercase'>Bienvenido a la Universidad YMCA</h2>" +
                                                                "</div>" +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<div class='col-md-12'>" +
                                                        "<div class='col-md-3'>" +
                                                        "</div>" +
                                                        "<div class='col-md-7 footer-gray portlet light portlet-title'>" +
                                                            "<h3 class='caption font-blue'>" + opjAl.Nombre + " " + opjAl.Paterno + " " + opjAl.Materno + "</h3>" +
                                                            "<hr />" +
                                                            "<h3 class='caption font-blue'>Los siguientes datos son tus credenciales para poder acceder al portal de la universidad</h3>" +
                                                            "<hr />" +
                                                            "<h3 class='caption font-blue'>Usuario</h3>" +
                                                            "<h4 class='caption font-blue-dark'>" + opjAl.AlumnoId.ToString() + "</h4>" +
                                                                "<h3 class='caption font-blue'>Contraseña</h3>" +
                                                            "<h4 class='caption font-blue-dark'>" + objPas.Password + "</h4>" +
                                                            "<hr />" +
                                                            "<h3 class='caption font-blue'>Puedes acceder a el desde el siguiente enlace.</h3>" +
                                                            "<a class='caption font-blue-dark' href='http://108.163.172.122/portalalumno/login.html'>YMCA</a>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</body>" +
                                "</html>";
                    #endregion
                    Email.Enviar(objCuenta.Email, objCuenta.Password, objCuenta.DisplayName, email, ',', "Recordatario - Portal YMCA Universidad", body, "", ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true, ref refere);
                }
                catch (Exception e)
                {
                    Fallidos = AlumnoId + " " + e.Message;
                }
            }
            return Fallidos;
            //return Email.Enviar("sistemastest@ymcacdmex.org.mx", "Sis99TesX1", "Bienvenido", opjAl.DTOAlumnoDetalle.Email, ',', "Universidad YMCA", body, "", ',', "mail.ymcacdmex.org.mx", 587, false, true, ref refere);
        }
        [WebMethod]
        public string EnviarMail3(string listaID)
        {
            string Fallidos = "";
            DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();
            string[] ListaAlumnos = listaID.Split(',');
            foreach (string AlumnoId in ListaAlumnos)
            {
                try
                {
                    var refere = new Utilities.ProcessResult();
                    DTOAlumnoPassword objPas = BLLAlumnoPassword.ConsultarAlumnoPassword(int.Parse(AlumnoId));
                    objPas.Password = Utilities.Seguridad.Desencripta(27, objPas.Password);
                    string body = "";
                    DTOAlumno opjAl = BLLAlumnoPortal.ObtenerAlumno(objPas.AlumnoId);
                    #region "HTML"
                    body = "<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>" +
                                "<head>" +
                                "<meta charset='utf-8' />" +
                                "<title>Bienvenida Alumnos</title>" +
                                "<meta http-equiv='X-UA-Compatible' content='IE=edge' />" +
                                "<meta content='width=device-width, initial-scale=1.0' name='viewport' />" +
                                "<meta http-equiv='Content-type' content='text/html; charset=utf-8' />" +
                                "<meta content='' name='description' />" +
                                "<meta content='' name='author' />" +
                               "<style>" +
                                    "body {" +
                                        "color: #333333;" +
                                        "font-family: 'Open Sans', sans-serif;" +
                                        "padding: 0px !important;" +
                                        "margin: 0px !important;" +
                                        "font-size: 13px;" +
                                        "direction: ltr;" +
                                    "}" +
                                    "body {" +
                                        "font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif;" +
                                        "font-size: 14px;" +
                                        "line-height: 1.42857143;" +
                                        "color: #333;" +
                                        "background-color: #fff;" +
                                    "}" +
                                    "Inherited from html html {" +
                                        "font-size: 10px;" +
                                        "-webkit-tap-highlight-color: rgba(0,0,0,0);" +
                                    "}" +
                                    "html {" +
                                        "font-family: sans-serif;" +
                                        "-webkit-text-size-adjust: 100%;" +
                                        "-ms-text-size-adjust: 100%;" +
                                    "}" +
                                    "div, input, select, textarea, span, img, table, label, td, th, p, a, button, ul, code, pre, li {" +
                                        "-webkit-border-radius: 0 !important;" +
                                        "-moz-border-radius: 0 !important;" +
                                        "border-radius: 0 !important;" +
                                    "}" +
                                    "* {" +
                                        "-webkit-box-sizing: border-box;" +
                                        "-moz-box-sizing: border-box;" +
                                        "box-sizing: border-box;" +
                                    "}" +
                                    "@media (min-width: 1200px) {" +
                                        ".container {" +
                                            "width: 1170px;" +
                                        "}" +
                                    "}" +
                                    "@media (min-width: 992px) {" +
                                        ".container {" +
                                            "width: 970px;" +
                                        "}" +
                                    "}" +
                                    "@media (min-width: 768px) {" +
                                        ".container {" +
                                            "width: 750px;" +
                                        "}" +
                                    "}" +
                                    ".container {" +
                                        "padding-right: 15px;" +
                                        "padding-left: 15px;" +
                                        "margin-right: auto;" +
                                        "margin-left: auto;" +
                                    "}" +
                                            ".row {" +
                                                "margin-right: -15px;" +
                                                "margin-left: -15px;" +
                                            "}" +
                                            ".col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9 {" +
                                        "float: left;" +
                                    "}" +
                                    ".col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {" +
                                        "position: relative;" +
                                        "min-height: 1px;" +
                                        "padding-right: 15px;" +
                                        "padding-left: 15px;" +
                                    "}" +
                                    ".portlet.light {" +
                                        "padding: 12px 20px 15px 20px;" +
                                        "background-color: #fff;" +
                                    "}" +
                                    ".portlet {" +
                                        "margin-top: 0px;" +
                                        "margin-bottom: 25px;" +
                                        "padding: 0px;" +
                                        "-webkit-border-radius: 4px;" +
                                        "-moz-border-radius: 4px;" +
                                        "-ms-border-radius: 4px;" +
                                        "-o-border-radius: 4px;" +
                                        "border-radius: 4px;" +
                                    "}" +
                                    ".portlet.light > .portlet-title {" +
                                        "padding: 0;" +
                                        "min-height: 48px;" +
                                    "}" +
                                    ".portlet > .portlet-title {" +
                                        "border-bottom: 1px solid #eee;" +
                                        "padding: 0;" +
                                        "margin-bottom: 10px;" +
                                        "min-height: 41px;" +
                                        "-webkit-border-radius: 4px 4px 0 0;" +
                                        "-moz-border-radius: 4px 4px 0 0;" +
                                        "-ms-border-radius: 4px 4px 0 0;" +
                                        "-o-border-radius: 4px 4px 0 0;" +
                                        "border-radius: 4px 4px 0 0;" +
                                    "}" +
                                    ".portlet.light > .portlet-title > .caption {" +
                                        "color: #666;" +
                                        "padding: 10px 0;" +
                                    "}" +
                                    ".portlet > .portlet-title > .caption {" +
                                        "float: left;" +
                                        "display: inline-block;" +
                                        "font-size: 18px;" +
                                        "line-height: 18px;" +
                                        "padding: 10px 0;" +
                                    "}" +
                                    ".uppercase {" +
                                        "text-transform: uppercase !important;" +
                                    "}" +
                                    ".bold {" +
                                        "font-weight: 700 !important;" +
                                    "}" +
                                    "h2 {" +
                                        "font-size: 27px;" +
                                    "}" +
                                    "h3 {" +
                                        "font-size: 23px;" +
                                    "}" +
                                    "h4 {" +
                                        "font-size: 17px;" +
                                    "}" +
                                    ".h4, h4 {" +
                                        "font-size: 18px;" +
                                    "}" +
                                    "h1, h2, h3, h4, h5, h6 {" +
                                        "font-family: 'Open Sans', sans-serif;" +
                                        "font-weight: 300;" +
                                    "}" +
                                    ".h2, h2 {" +
                                        "font-size: 30px;" +
                                    "}" +
                                    ".h1, .h2, .h3, h1, h2, h3 {" +
                                        "margin-top: 20px;" +
                                        "margin-bottom: 10px;" +
                                    "}" +
                                    ".h1, .h2, .h3, .h4, .h5, .h6, h1, h2, h3, h4, h5, h6 {" +
                                        "font-family: inherit;" +
                                        "font-weight: 500;" +
                                        "line-height: 1.1;" +
                                        "color: inherit;" +
                                    "}" +
                                    "* {" +
                                        "-webkit-box-sizing: border-box;" +
                                        "-moz-box-sizing: border-box;" +
                                        "box-sizing: border-box;" +
                                    "}" +
                                    "user agent stylesheeth2 {" +
                                        "display: block;" +
                                        "font-size: 1.5em;" +
                                        "-webkit-margin-before: 0.83em;" +
                                        "-webkit-margin-after: 0.83em;" +
                                        "-webkit-margin-start: 0px;" +
                                        "-webkit-margin-end: 0px;" +
                                        "font-weight: bold;" +
                                    "}" +
                                    ".table {" +
                                        "width: 100%;" +
                                        "max-width: 100%;" +
                                        "margin-bottom: 20px;" +
                                    "}" +
                                    ".font-green-sharp {" +
                                        "color: #4DB3A2 !important;" +
                                    "}" +
                                    ".uppercase {" +
                                        "text-transform: uppercase !important;" +
                                    "}" +
                                    ".bold {" +
                                        "font-weight: 700 !important;" +
                                    "}" +
                                    ".font-blue {" +
                                        "color: #3598dc !important;" +
                                    "}" +
                                    "hr {" +
                                        "margin: 20px 0;" +
                                        "border: 0;" +
                                        "border-top: 1px solid #eee;" +
                                        "border-bottom: 0;" +
                                    "}" +
                                    "hr {" +
                                        "margin-top: 20px;" +
                                        "margin-bottom: 20px;" +
                                        "border: 0;" +
                                        "border-top: 1px solid #eee;" +
                                    "}" +
                                    "hr {" +
                                        "height: 0;" +
                                        "-webkit-box-sizing: content-box;" +
                                        "-moz-box-sizing: content-box;" +
                                        "box-sizing: content-box;" +
                                    "}" +
                                    "* {" +
                                        "-webkit-box-sizing: border-box;" +
                                        "-moz-box-sizing: border-box;" +
                                        "box-sizing: border-box;" +
                                    "}" +
                                    "user agent stylesheethr {" +
                                        "display: block;" +
                                        "-webkit-margin-before: 0.5em;" +
                                        "-webkit-margin-after: 0.5em;" +
                                        "-webkit-margin-start: auto;" +
                                        "-webkit-margin-end: auto;" +
                                        "border-style: inset;" +
                                        "border-width: 1px;" +
                                    "}" +
                                "</style>" +
                                "</head>" +
                                "<body>" +
                                    "<div class='page-head'>" +
                                        "<div class='container'>" +
                                            "<div class='table'>" +
                                                "<div class='row'>" +
                                                    "<div class='col-md-12'>" +
                                                        "<div class='col-md-3'>" +
                                                        "</div>" +
                                                        "<div class='col-md-7 footer-gray portlet light '>" +
                                                            "<div class='portlet-title '>" +
                                                                "<div class='caption'>" +
                                                                    "<h2 class='caption font-green-sharp bold uppercase'>Bienvenido a la Universidad YMCA</h2>" +
                                                                "</div>" +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<div class='col-md-12'>" +
                                                        "<div class='col-md-3'>" +
                                                        "</div>" +
                                                        "<div class='col-md-7 footer-gray portlet light portlet-title'>" +
                                                            "<h3 class='caption font-blue'>" + opjAl.Nombre + " " + opjAl.Paterno + " " + opjAl.Materno + "</h3>" +
                                                            "<hr />" +
                                                            "<h3 class='caption font-blue'>Los siguientes datos son tus credenciales para poder acceder al portal de la universidad</h3>" +
                                                            "<hr />" +
                                                            "<h3 class='caption font-blue'>Usuario</h3>" +
                                                            "<h4 class='caption font-blue-dark'>" + opjAl.AlumnoId.ToString() + "</h4>" +
                                                                "<h3 class='caption font-blue'>Contraseña</h3>" +
                                                            "<h4 class='caption font-blue-dark'>" + objPas.Password + "</h4>" +
                                                            "<hr />" +
                                                            "<h3 class='caption font-blue'>Puedes acceder a el desde el siguiente enlace.</h3>" +
                                                            "<a class='caption font-blue-dark' href='http://108.163.172.122/portalalumno/login.html'>YMCA</a>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</body>" +
                                "</html>";
                    #endregion
                    Email.Enviar(objCuenta.Email, objCuenta.Password, objCuenta.DisplayName, opjAl.DTOAlumnoDetalle.Email, ',', "programador1@ymcacdmex.org.mx;obeddelcastillo@uniymca.edu.mx", ';', "Recordatario - Portal YMCA Universidad", body, "", ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true, ref refere);
                }
                catch (Exception e)
                {
                    Fallidos = AlumnoId + " " + e.Message;
                }
            }
            return Fallidos;
            //return Email.Enviar("sistemastest@ymcacdmex.org.mx", "Sis99TesX1", "Bienvenido", opjAl.DTOAlumnoDetalle.Email, ',', "Universidad YMCA", body, "", ',', "mail.ymcacdmex.org.mx", 587, false, true, ref refere);
        }
        [WebMethod]
        public string Contraseña(string AlumnoId)
        {
            DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();
            DTOAlumnoPassword objPas = BLLAlumnoPassword.ConsultarAlumnoPassword(int.Parse(AlumnoId));
            objPas.Password = Utilities.Seguridad.Desencripta(27, objPas.Password);
            return objPas.Password;
        }
        [WebMethod]
        public string CrearEncrip(string Nombre)
        {
            return Utilities.Seguridad.Encripta(27, Nombre);
        }
        [WebMethod]
        public List<DTOAlumnoDescuento> ConsultarDescuentos(string AlumnoId, string OfertaEducativaId)
        {
            return BLLDescuentos.ObtenerDescuentosAlumno(int.Parse(AlumnoId), int.Parse(OfertaEducativaId));
        }
        [WebMethod]
        public List<DTOAlumnoDescuento> ConsultarDescuentosO(string AlumnoId)
        {
            return BLLDescuentos.ObtenerDescuentosAlumno(int.Parse(AlumnoId));
        }
        [WebMethod]
        public string GuardarDescuento(string AlumnoId, string UsuarioId, string Beca, string SEP, string DescuentoBeca, string DescuentoSEP, string OfertaEducativaId)
        {
            return BLLDescuentos.GenerarDescuento(int.Parse(AlumnoId), int.Parse(UsuarioId), decimal.Parse(Beca), Boolean.Parse(SEP), DescuentoBeca != "null" ? int.Parse(DescuentoBeca) : 0, DescuentoSEP != "null" ? int.Parse(DescuentoSEP) : 0, int.Parse(OfertaEducativaId));
        }
        [WebMethod]
        public string GuardarDescuento2(string AlumnoId, string UsuarioId, string Inscripcion, string Financiamiento, string DescuentoInsc, string DescuentoFinan)
        {
            return BLLDescuentos.GenerarDescuento(int.Parse(AlumnoId), int.Parse(UsuarioId), decimal.Parse(Inscripcion), decimal.Parse(Financiamiento), DescuentoInsc != "null" ? int.Parse(DescuentoInsc) : 0, DescuentoFinan != "null" ? int.Parse(DescuentoFinan) : 0);
            //return " ";
        }
        [WebMethod]
        public string ConsultarAdeudo(string AlumnoId)
        {
            return BLLPagoPortal.ConsultarAdeudo(int.Parse(AlumnoId));
        }
        [WebMethod]
        public void GenerarReferenciaPagoId(string AlumnoId)
        {
            BLLPagoPortal.ActualizarReferencia(int.Parse(AlumnoId));
        }
        [WebMethod]
        public string ActualizarPromesa(string PagoId)
        {
            return BLLPagoPortal.AplicarDescuento(int.Parse(PagoId));
        }
        [WebMethod]
        public int GenerarRecargo(string PagoId, string Fecha)
        {
            DateTime FechaD = DateTime.ParseExact(Fecha.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return BLLPagoPortal.GenerarRecargo(int.Parse(PagoId), FechaD, 100000);
        }
        [WebMethod]
        public List<DTOPagos> ReferenciasSemestrales(int AlumnoId, int OfertaEducativaId)
        {
           return BLL.BLLPagoPortal.BuscarPagosActuales(AlumnoId, OfertaEducativaId);
        }
        [WebMethod]
        public bool GenerarSemestre(int AlumnoId,int OfertaEducativaId,int MesFinal,int MesInicial,int UsuarioId,string Inscripcion,string Colegiatura)
        {
            return
            BLL.BLLPagoPortal.GenerarSemestre(AlumnoId, OfertaEducativaId, MesFinal, MesInicial, UsuarioId, decimal.Parse(Inscripcion), decimal.Parse(Colegiatura));
        }
    }
}
