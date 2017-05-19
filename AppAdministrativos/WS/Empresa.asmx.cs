using BLL;
using DTO;
using Herramientas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AppAdministrativos.WS
{
    /// <summary>
    /// Descripción breve de Empresa
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Empresa : System.Web.Services.WebService
    {

        [WebMethod]
        public List<DTOEmpresa> ListarEmpresas()
        {
            return BLLEmpresa.ListaEmpresas();
        }

        [WebMethod]
        public string GuardarEmpresa(string Razon, string NombreC, string Paterno, string Materno, string EmailC, string Telefono, string Celular, string Pais, string Estado, string Delegacion,
            string CP, string NoExterior, string NoInterior, string Calle, string Email, string RFC, string Observacion, string Colonia, string CalleF, string NoExteriorF,
            string NoInteriorF, string CPF, string ColoniaF, string PaisF, string EstadoF, string DelegacionF, string ObservacionF, string Igual, string FechaV, string UsuarioId)
        {
            var objEmpresa = (new DTOEmpresa
            {
                RFC = RFC,
                RazonSocial = Razon,
                FechaVigencia = DateTime.ParseExact(FechaV.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Usuarioid = int.Parse(UsuarioId),
                EmpresaDetalle = new DTOEmpresaDetalle
                {
                    Nombre = NombreC,
                    Paterno = Paterno,
                    Materno = Materno,
                    EmailContacto = EmailC,
                    Telefono = Telefono,
                    Celular = Celular,
                    PaisId = int.Parse(Pais),
                    EntidadFederativaId = int.Parse(Estado),
                    MunicipioId = int.Parse(Delegacion),
                    CP = CP,
                    Colonia = Colonia,
                    Calle = Calle,
                    NoExterior = NoExterior,
                    NoInterior = NoInterior,
                    Email = Email,
                    Observacion = Observacion,

                    DatosFiscales = Igual == "false" ? new DTODatosFicales
                    {
                        RFC = RFC,
                        PaisId = int.Parse(PaisF),
                        EntidadFederativaId = int.Parse(EstadoF),
                        MunicipioId = int.Parse(DelegacionF),
                        CP = CPF,
                        Colonia = ColoniaF,
                        Calle = CalleF,
                        NoExterior = NoExteriorF,
                        NoInterior = NoInteriorF,
                        Observacion = ObservacionF,
                        EsEmpresa = true,

                    } : new DTODatosFicales
                    {
                        RFC = RFC,
                        PaisId = int.Parse(Pais),
                        EntidadFederativaId = int.Parse(Estado),
                        MunicipioId = int.Parse(Delegacion),
                        CP = CP,
                        Colonia = Colonia,
                        Calle = Calle,
                        NoExterior = NoExterior,
                        NoInterior = NoInterior,
                        Observacion = Observacion,
                        EsEmpresa = true,
                    }
                }
            });

            return BLLEmpresa.GuardarEmpresa(objEmpresa);
        }

        [WebMethod]
        public string GuardarGrupo(string EmpresaId, string Nombre, string Sede, string Direccion, string TipoOferta, string Oferta,
            string FechaInicio, string CuotaColegiatura, string DescuentoColegiatura, string CuotaInscripcion, string DescuentoInscripcion,
            string Periodo, string NoPagos, string AplicaDescuento, string JustificacionIn, string JustificacionBec)
        {
            ///DTOPeriodo objPer = BLLPeriodo.ConsultarPeriodo(Periodo);
            DTOPeriodo objPer = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.ParseExact(FechaInicio.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture));
            DTOCuota objCuota = BLLCuota.TraerCuotaPagoConcepto(int.Parse(CuotaInscripcion), new DTOAlumnoInscrito
            {
                OfertaEducativaId = int.Parse(Oferta),
                Anio = objPer.Anio,
                PeriodoId = objPer.PeriodoId
            });

            DTOGrupo objGrupo = new DTOGrupo
            {
                RFC = EmpresaId,
                Descripcion = Nombre,
                SucursalId = int.Parse(Sede),
                SucursalDireccion = Direccion,
                OfertaEducativaTipoId = int.Parse(TipoOferta),
                OfertaEducativaId = int.Parse(Oferta),
                FechaInicio = DateTime.ParseExact(FechaInicio.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                GrupoDetalle = new DTOGrupoDetalle
                {
                    //PorcentajeInscripcion = decimal.Parse(DescuentoInscripcion),
                    //SiempreInscripcion = bool.Parse(AplicaDescuento),
                    NoPagos = int.Parse(NoPagos),
                    Cuota = new DTOCuota
                    {
                        Anio = objPer.Anio,
                        PeriodoId = objPer.PeriodoId,
                        OfertaEducativaId = int.Parse(Oferta),
                        PagoConceptoId = objCuota.PagoConceptoId,
                        Monto = int.Parse(CuotaColegiatura)
                    }
                }
            };
            objGrupo.GrupoId = BLLGrupo.GuardarGrupo(objGrupo);
            return objGrupo.GrupoId.ToString();
        }


        [WebMethod]
        public string GuardarGrupo2(int EmpresaId, int GrupoId, string Nombre, string Sede, string Direccion,
            string FechaIni, string UsuarioId)
        {
            /////DTOPeriodo objPer = BLLPeriodo.ConsultarPeriodo(Periodo);
            //DTOPeriodo objPer = BLLPeriodo.TraerPeriodoEntreFechas(DateTime.ParseExact(FechaInicio.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture));
            //DTOCuota objCuota = BLLCuota.TraerCuotaPagoConcepto(int.Parse(CuotaInscripcion), new DTOAlumnoInscrito
            //{
            //    OfertaEducativaId = int.Parse(Oferta),
            //    Anio = objPer.Anio,
            //    PeriodoId = objPer.PeriodoId
            //});

            DTOGrupo objGrupo = new DTOGrupo
            {
                EmpresaId = EmpresaId,
                GrupoId = GrupoId,
                Descripcion = Nombre,
                SucursalId = int.Parse(Sede),
                SucursalDireccion = Direccion,
                FechaInicio = DateTime.ParseExact(FechaIni.Replace('-', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                UsuarioId = int.Parse(UsuarioId)

                //GrupoDetalle = new DTOGrupoDetalle
                //{
                //    //PorcentajeInscripcion = decimal.Parse(DescuentoInscripcion),
                //    //PorcentajeColegiatura = decimal.Parse(DescuentoColegiatura),
                //    //SiempreInscripcion = bool.Parse(AplicaDescuento),
                //    NoPagos = int.Parse(NoPagos),
                //    CuotaId = int.Parse(CuotaId),
                //    EsCongelada = Congelada == "true" ? true : false,
                //}
            };
            objGrupo.GrupoId = BLLGrupo.GuardarGrupo2(objGrupo);
            return objGrupo.GrupoId.ToString();
        }


         [WebMethod]
        public List<DTOEmpresaLigera> ListarEmpresaLigera()
        {
            return BLLEmpresa.ListarEmpresaLigera();
        }

         [WebMethod]
         public string MovimientosAlumnoGrupo(string GrupoId, string AlumnoId, string UsuarioId, string OfertaId, string TipoMovimiento)
         {
             return BLLGrupo.MovimientosAlumnoGrupo(int.Parse(GrupoId), int.Parse(AlumnoId), int.Parse(UsuarioId), int.Parse(OfertaId), int .Parse(TipoMovimiento));
         }

        [WebMethod]
        public List<DTOGrupo> ListarGrupos(string EmpresaId)
        {
            return BLLGrupo.ListaGrupo(int.Parse(EmpresaId));
        }

        [WebMethod]
        public string DireccionEmpresa(string SucursalId)
        {
            DTOSucursal sucursal = BLLSucursal.TraerSucursal(int.Parse(SucursalId));
            if (sucursal.Detalle.Calle != null)
            { return "Calle " + sucursal.Detalle.Calle + " No.Esterior " + sucursal.Detalle.NoExterior + " Colonia " + sucursal.Detalle.Colonia + " Delegación " + sucursal.Detalle.Delegacion; }
            else { return ""; }
        }

        [WebMethod]
        public string GenerarPagos(string Alumnos, string Grupo)
        {
            try
            {
                Alumnos = Alumnos.Remove(Alumnos.Length - 1, 1);
                List<int> Alumnosl = new List<int>(Array.ConvertAll(Alumnos.Split(','), int.Parse));
                return BLLAlumnoDescuento.InsertarDescuento(Alumnosl, int.Parse(Grupo));
            }
            catch (Exception)
            {
                return "";
            }
        }

        [WebMethod]
        public string GenerarPagos2(string Alumnos, string Grupo, string Usuario)
        {
            try
            {
                Alumnos = Alumnos.Remove(Alumnos.Length - 1, 1);
                List<int> Alumnosl = new List<int>(Array.ConvertAll(Alumnos.Split(','), int.Parse));
                if (BLLAlumnoDescuento.InsertarDescuento2(Alumnosl, int.Parse(Grupo), int.Parse(Usuario)) == "Guardar")
                {
                    Alumnosl.ForEach(delegate (int Alumno)
                    {
                        Descuentos objDescuento = new Descuentos();

                        objDescuento.EnviarMail(Alumno, ConvertidorT.CrearPass());
                    });

                    return "Guardar";
                }
                else { return "Error al Guardar"; }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        [WebMethod]
        public string GuardarDocumentos()
        {
            HttpFileCollection httpFileCollection = Context.Request.Files;
            System.Collections.Specialized.NameValueCollection DescuentosID = Context.Request.Form;
            int Grupoid = int.Parse(DescuentosID["GrupoId"]);
            DTOGrupo objGrupo = BLLGrupo.ObtenerGrupo(Grupoid);

            List<DTOGrupoComprobante> lstDoc = new List<DTOGrupoComprobante>();
            try
            {
                string JustificacionBeca = DescuentosID["JustificacionBe"];
                HttpPostedFile httpColegiatura = httpFileCollection["DocBeca"];
                Stream strCole = httpColegiatura.InputStream;

                lstDoc.Add(new DTOGrupoComprobante
                {
                    GrupoId = Grupoid,
                    //Justificacion = JustificacionBeca,
                    //PagoConceptoId = objGrupo.GrupoDetalle.Cuota.PagoConceptoId,
                    GrupoComprobanteDocumento = new DTOGrupoComprobanteDocumento
                    {
                        Documento = Herramientas.ConvertidorT.ConvertirStream(strCole, httpColegiatura.ContentLength)
                    }
                });
            }
            catch { }
            try
            {
                string JustificacionInsc = DescuentosID["JustificacionIn"];
                HttpPostedFile httpInscripcion = httpFileCollection["DocBeca"];
                Stream strInscrip = httpInscripcion.InputStream;

                lstDoc.Add(new DTOGrupoComprobante
                {
                    GrupoId = Grupoid,
                    //Justificacion = JustificacionInsc,
                    //PagoConceptoId = objGrupo.GrupoDetalle.Cuota.PagoConceptoId,
                    GrupoComprobanteDocumento = new DTOGrupoComprobanteDocumento
                    {
                        Documento = Herramientas.ConvertidorT.ConvertirStream(strInscrip, httpInscripcion.ContentLength)
                    }
                });
            }
            catch { }

            if (lstDoc.Count > 0)
            {
                BLLGrupoComprobante.GuardarComprobante(lstDoc);
                return "1";
            }
            else { return "0"; }


        }
        [WebMethod]
        public List<DTOAlumnoEspecial> ListarAlumnos(string GrupoId)
        {
            return BLLGrupo.TraerAlumnosDeEpresa(int.Parse(GrupoId));
        }

        [WebMethod]
        public bool GuardarConfiguracion(DTOGrupoAlumnoCuotaString AlumnoConfig)
        {
            if (BLLGrupo.GuardarAlumnoConfiguracion(AlumnoConfig))
            {
                BLLGrupo.GenerarCuotas(AlumnoConfig);
                return true;
            }
            else { return false; }
        }
        
    }
}
