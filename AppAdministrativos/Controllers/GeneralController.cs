﻿using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilities;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/General")]
    public class GeneralController : ApiController
    {
        [Route("ConsultarGenero")]
        [HttpGet]
        public IHttpActionResult GetGenero()
        {
            return Ok(BLLGenero.ConsultaTodosGenero());
        }

        [Route("ConsultarTurnos")]
        [HttpGet]
        public IHttpActionResult GetTurno()
        {
            return Ok(BLLTurno.ConsultarTurno());
        }

        [Route("Plantel")]
        [HttpGet]
        public IHttpActionResult ConsultarPlantel()
        {
            return Ok(BLLSucursal.ConsultarSucursales());
        }

        [Route("OFertaEducativaTipo/{PlantelId:int}")]
        [HttpGet]
        public IHttpActionResult GetOfertaEducativaTipo(int PlantelId)
        {
            return Ok(BLLOfertaEducativaTipo.ConsultaOfertaTipo(PlantelId));
        }

        [Route("OFertaEducativa/{PlantelId:int}/{TipoOfertaId:int}")]
        [HttpGet]
        public IHttpActionResult GetOfertasEducativas(int PlantelId, int TipoOfertaId)
        {
            return Ok(BLLOfertaEducativaPortal.ConsultarOfertasEducativas(TipoOfertaId, PlantelId));
        }

        [Route("ConsultarPeriodos")]
        [HttpGet]
        public IHttpActionResult GetPeriodosSiguientes()
        {
            return Ok(BLLPeriodoPortal.ConsultarPeriodos());
        }

        [Route("ConsultarPeriodosPCF")]
        [HttpGet]
        public IHttpActionResult GetPeriodosAn_Ac_Sg()
        {
            object Result = BLLPeriodoPortal.GetPeriodo_An_Ac_Sg();


            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de guardar, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }

        [Route("ConsultarPais")]
        [HttpGet]
        public IHttpActionResult GetPais()
        {
            return Ok(BLLPais.TraerPaises());
        }

        [Route("ConsultarEntidadFederativa")]
        [HttpGet]
        public IHttpActionResult GetEntidadFederativa()
        {
            return Ok(BLLEntidadFederativa.ConsultarEntidadFederativa());
        }

        [Route("ConsultarMunicipios/{EntidadFederativaId:int}")]
        [HttpGet]
        public IHttpActionResult GetMunicipios(int EntidadFederativaId)
        {
            return Ok(BLLMunicipio.ConsultarMunicipios(EntidadFederativaId));
        }

        [Route("ConsultarEstadoCivil")]
        [HttpGet]
        public IHttpActionResult GetEstadoCivil()
        {
            return Ok(BLLEstadoCivil.ConsultarEstadosCiviles());
        }

        [Route("GetDirectory")]
        [HttpGet]
        public IHttpActionResult GetDiractorios()
        {
            return Ok(BLLAlumnoCambio.GetRoot());
        }

        [Route("TraerListaMedios")]
        [HttpGet]
        public IHttpActionResult MediosDifucion()
        {
            return Ok(BLLMedioDifusion.ConsultarListadeMedios());
        }

        [Route("ConsultarParentesco")]
        [HttpGet]
        public IHttpActionResult GetParentesco()
        {
            return Ok(BLLParentesco.ConsultarTodosParentesco());
        }

        [Route("ObtenerAreas")]
        [HttpGet]
        public IHttpActionResult GetAreas()
        {
            return Ok(BLLAreaAcademica.AreasAcademicas());
        }

        [Route("SistemaPagoAlumno/{AlumnoId:int}")]
        [HttpGet]
        public IHttpActionResult SistemaPagoAlumno(int AlumnoId)
        {
            var Result = BLLPagoPlan.ConsultarPagos(AlumnoId);
            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de trer datos, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }
        }
    

        [Route("EnviarMail2")]
        [HttpPost]
        public IHttpActionResult SendMail(List<int> lstAlumnos)
        {
            var Result = BLLEmail.SendMultipleEmail(lstAlumnos);

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                if (((List<string>)Result).Count > 0)
                {
                    return BadRequest(String.Join("/ ", ((List<string>)Result).ToArray()));
                }
                else
                {
                    return Ok(new
                    {
                        Message = "Se envio el email correctamente"
                    });
                }
            }
            else
            {

                return BadRequest(String.Join("/ ", ((List<string>)Result).ToArray()));
            }
        }
        [Route("ConsultarPagosPlan/{TipoOfertaId:int}")]
        [HttpGet]
        public IHttpActionResult PlanPago(int TipoOfertaId)
        {
            var Result = BLLPagoPlan.ConsultarPagosPlanLenguas(TipoOfertaId);

            if (Result.ToString().Contains("System.Collections.Generic.List"))
            {
                return Ok(Result);
            }
            else
            {
                return BadRequest("Fallo al momento de trer datos, " + Result.GetType().GetProperty("Message").GetValue(Result, null));
            }

        }

        [Route("EnviarMailUpdate/{AlumnoId:int}/{UsuarioId:int}")]
        [HttpGet]
        public IHttpActionResult EnviarMailUpdate(int AlumnoId, int UsuarioId)
        {
            string Fallidos = "";
            DTO.DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();
            try
            {
                var refere = new Utilities.ProcessResult();
                DTO.DTOAlumnoPassword objPas = BLLAlumnoPassword.ConsultarAlumnoPassword(AlumnoId);
                string body = "";
                if (objPas == null)
                {
                    BLLAlumnoPassword.GuardarPassword(AlumnoId, Utilities.Seguridad.Encripta(27, Herramientas.ConvertidorT.CrearPass()));
                    objPas = BLLAlumnoPassword.ConsultarAlumnoPassword(AlumnoId);
                }
                DTO.DTOUsuarioDetalle objUsDetalle = BLLUsuarioPortal.ObtenerDetalle(UsuarioId);
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

            return Ok(Fallidos);
        }
    }
}
