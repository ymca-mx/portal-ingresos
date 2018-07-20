using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DTO;
using BLL;
using System.Globalization;
using System.Web.Script.Services;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;

namespace AppAdministrativos.WS
{
    /// <summary>
    /// Descripción breve de Alumno
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class Alumno : System.Web.Services.WebService
    {


        [WebMethod]
        public string InsertarAlumno(string Nombre, string Paterno, string Materno, string Sexo, string EstadoCivil, string FNacimiento,
             string Curp, string Pais, string Entidad, string Delegacion, string CP, string Colonia, string Calle,
            string NoExterior, string NoInterior, string TCasa, string TCelular, string Email, string PANombre, string PAPaterno,
             string PAMaterno, string PATCelular, string PAEmail, string Parentesco, string OfertaEducativa, string Turno, string Periodo,
             string Preparatoria, string Area, string AñoPrepa, string Promedio, string Universidad, string Plantel, string PANombre2,
             string PAPaterno2, string PAMaterno2, string PATCelular2, string PAEmail2, string Parentesco2, string Nacionalidad, string NacionalidadPre,
             string PaisEstadoPre, string NacionalidadUni, string PaisEstadoUni, string Titulado, string Motivo, string Autoriza1, string Autoriza2,
             string TelefonoCasaP, string TelefonoCasaP2, string EsEmpresa, string MedioDifusion, string Usuario, string Observaciones)
        {

            //var obj = JsonConvert.DeserializeObject<List<List<object>>>(lstDatos);
            int? defaul = null;
            try
            {
                AñoPrepa = AñoPrepa.Length < 4 ? "null" : AñoPrepa;
                //int dia =int.Parse( FNacimiento.Substring(0, 2));
                //int Mes = int.Parse(FNacimiento.Substring(3, 2));
                //int ano = int.Parse(FNacimiento.Substring(6, 4));
                //DateTime fNacimientos = new DateTime(ano, Mes, dia);
                DTOAlumnoDetalle objAlumnoDetalle;
                try
                {
                    objAlumnoDetalle = new DTOAlumnoDetalle
                    {
                        GeneroId = int.Parse(Sexo),
                        EstadoCivilId = int.Parse(EstadoCivil),
                        FechaNacimiento = DateTime.ParseExact((FNacimiento.Replace('-', '/')), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        CURP = Curp,
                        PaisId = int.Parse(Nacionalidad) == 1 ? 146 : int.Parse(Pais),
                        EntidadFederativaId = int.Parse(Entidad),
                        MunicipioId = int.Parse(Delegacion),
                        Cp = CP,
                        Colonia = Colonia,
                        Calle = Calle,
                        NoExterior = NoExterior,
                        NoInterior = NoInterior == "null" ? "" : NoInterior,
                        TelefonoCasa = TCasa,
                        Celular = TCelular,
                        Email = Email,
                        TelefonoOficina = "",
                        EntidadNacimientoId = int.Parse(Nacionalidad) == 1 ? int.Parse(Pais) : defaul,
                        Observaciones = Observaciones
                    };
                }
                catch { return "Error AlumnoDetalle " + FNacimiento; }
                DTOAlumno objAlumno = new DTOAlumno
                {
                    Nombre = Nombre,
                    Paterno = Paterno,
                    Materno = Materno,
                    UsuarioId = int.Parse(Usuario)
                };
                List<DTOPersonaAutorizada> lstPersona = new List<DTOPersonaAutorizada>{
                new DTOPersonaAutorizada
             {
                 Nombre = PANombre,
                 Paterno = PAPaterno,
                 Materno = PAMaterno,
                 Celular = PATCelular,
                 Email = PAEmail,
                 ParentescoId = int.Parse(Parentesco),
                 Telefono = TelefonoCasaP,
                 Autoriza=Boolean.Parse(Autoriza1)
             }
           };
                if (PANombre2 != "null")
                {
                    lstPersona.Add(new DTOPersonaAutorizada
                    {
                        Nombre = PANombre2,
                        Paterno = PAPaterno2,
                        Materno = PAMaterno2,
                        Celular = PATCelular2,
                        Email = PAEmail2,
                        ParentescoId = int.Parse(Parentesco2),
                        Telefono = TelefonoCasaP2,
                        Autoriza = Boolean.Parse(Autoriza2)
                    });
                }

                DTOAlumnoInscrito objAlumnoIn = new DTOAlumnoInscrito
                {
                    OfertaEducativaId = int.Parse(OfertaEducativa),
                    TurnoId = int.Parse(Turno),
                    PeriodoId = int.Parse(Periodo.Substring(0, 1)),
                    Anio = BLLPeriodoPortal.ConsultarPeriodo(Periodo).Anio,
                    EsEmpresa = Boolean.Parse(EsEmpresa),
                    UsuarioId = int.Parse(Usuario)
                };
                DTOProspectoDetalle objProspectoDetalle;
                try
                {
                    objProspectoDetalle = new DTOProspectoDetalle
                    {

                        PrepaProcedencia = (Preparatoria == "null") ? " " : Preparatoria,
                        PrepaArea = int.Parse((Area == "null") ? " " : Area),
                        PrepaAnio = int.Parse((AñoPrepa == "null") ? "0" : AñoPrepa.Substring(0, 4)),
                        PrepaPromedio = decimal.Parse((Promedio == "null") ? "0" : Promedio),
                        EsEquivalencia = ((Universidad) == "null") ? false : true,
                        UniversidadProcedencia = ((Universidad) == "null") ? string.Empty : (Universidad),
                        SucursalId = int.Parse(Plantel),
                        PrepaMes = int.Parse((AñoPrepa == "null") ? "0" : AñoPrepa.Substring(5, 2)) == 0 ? defaul : int.Parse((AñoPrepa == "null") ? "0" : AñoPrepa.Substring(5, 2)),
                        PrepaPaisId = ((NacionalidadPre) == "null") ? defaul : int.Parse((NacionalidadPre)) == 1 ? 146 : int.Parse(NacionalidadPre),
                        PrepaEntidadId = ((NacionalidadPre) == "null") ? defaul : int.Parse((NacionalidadPre)) == 1 ? int.Parse(PaisEstadoPre) : defaul,
                        UniversidadPaisId = ((NacionalidadUni) == "null") ? defaul : int.Parse(((NacionalidadUni) == "1") ? "146" : (NacionalidadUni)),
                        UniversidadEntidadId = ((NacionalidadUni) == "null") ? defaul : int.Parse((NacionalidadUni)) == 1 ? int.Parse(PaisEstadoUni) : defaul,
                        EsTitulado = bool.Parse((Titulado) == "null" ? "false" : (Titulado)),
                        UniversidadMotivo = (Motivo) == "null" ? " " : (Motivo),
                        MedioDifusionId = int.Parse(MedioDifusion),

                    };
                }
                catch { return "error Prospecto"; }

                string Alumnoid = BLLAlumnoPortal.InsertarAlumno(objAlumnoDetalle, objAlumno, lstPersona, objAlumnoIn, objProspectoDetalle);

                //if (objAlumnoIn.EsEmpresa == true)
                //{
                //    Descuentos objDescuento = new Descuentos();
                //    objDescuento.EnviarMail(int.Parse(Alumnoid), ConvertidorT.CrearPass());
                //}
                return Alumnoid;
            }
            catch (Exception ex)
            { return ex.Message + " " + AñoPrepa; }
        }

        [WebMethod]
        public List<DTOAlumnoLigero> ConsultarAlumnosNuevos()
        {
            return BLLAlumnoPortal.ConsultarAlumnosNuevos();
        }

        [WebMethod]
        public object ConsultarAlumnos()
        {
            return BLLAlumnoPortal.ListarAlumnos();
        }

        [WebMethod]
        public DTOAlumnoReferencias1 ConsultarAlumnoReferencias(int alumnoInt, int pagoid)
        {
            return BLLAlumnoPortal.ObtenerReferenciasAlumno(alumnoInt, pagoid);
        }

        [WebMethod]
        public List<DTOAlumno> ConsultarAlumnosBeca(string Ingles)
        {
            return BLLAlumnoPortal.ListarAlumnosBeca(Boolean.Parse(Ingles));
        }
        [WebMethod]
        public DTOAlumno ConsultarAlumno(string AlumnoId)
        {
            return BLLAlumnoPortal.ObtenerAlumno(int.Parse(AlumnoId));
        }

        //Promocion en casa
        [WebMethod]
        public object GuardarPromocionCasa(DTOAlumnoPromocionCasa Promocion)
        {
            return BLLAlumnoPortal.GuardarPromocionCasa(Promocion);
        }
        [WebMethod]
        public List<DTOAlumnoPromocionCasa> ConsultarAlumnoPromocionCasa(string Anio, string PeriodoId)
        {
            return BLLAlumnoPortal.ConsultarAlumnoPromocionCasa(int.Parse(Anio), int.Parse(PeriodoId));
        }
        [WebMethod]
        public object ConsultarAlumnoPromocionCasa2(string AlumnoPromocion)
        {
            return BLLAlumnoPortal.ConsultarAlumnoPromocionCasa2(int.Parse(AlumnoPromocion));
        }
        [WebMethod]
        public List<DTOPeriodoPromocionCasa> PeriodosPromocionCasa()
        {
            return BLLAlumnoPortal.PeriodosPromocionCasa();
        }
        [WebMethod]
        public string AplicarPromocionCasa(DTOAlumnoPromocionCasa Promocion)
        {
            return BLLAlumnoPortal.AplicarPromocionCasa(Promocion);
        }
        //Promocion en casa
        
        [WebMethod]
        public DTOAlumno ConsultarAlumnoL(string AlumnoId)
        {
            return BLLAlumnoPortal.ObtenerAlumno1(int.Parse(AlumnoId));
        }
        [WebMethod]
        public DTOAlumnoPermitido1 ConsultarAlumno2(string AlumnoId)
        {
            return BLLAlumnoPortal.ObtenerAlumno2(int.Parse(AlumnoId));
        }
        [WebMethod]
        public object BuscarAlumno(string Nombre, string Paterno, string Materno)
        {
            return BLLAlumnoPortal.ListarAlumnos(Nombre, Paterno, Materno);
        }
        [WebMethod]
        public List<DTOAlumno> ConsultarAlumnosEmpresa(string grupoId)
        {
            return BLLAlumnoPortal.AlumnosEmpresa(int.Parse(grupoId));
        }
        [WebMethod]
        public List<DTOAlumno> ConsultarAlumnosDeEmpresa(string grupoId)
        {
            return BLLAlumnoPortal.AlumnosdeEmpresa(int.Parse(grupoId));
        }
        [WebMethod]
        public List<DTOPagos> ConsultarReferencias(string AlumnoId)
        {
            return BLLPagoPortal.ConsultarReferencias(int.Parse(AlumnoId));
        }
        [WebMethod]
        public List<DTOPagos> ConsultarReferencias2(string AlumnoId, string NoPagados)
        {
            return BLLPagoPortal.ConsultarReferencias(int.Parse(AlumnoId), Boolean.Parse(NoPagados));
        }
        [WebMethod]
        public List<DTOPagoParcial> ConsultarReferenciasPagadas(string AlumnoId)
        {
            return BLLPagoPortal.ConsultarReferenciasPagadas(int.Parse(AlumnoId));
        }
        [WebMethod]
        public object ConsultarReferenciasCP(string AlumnoId)
        {
            return BLLPagoPortal.ConsultarReferenciasConceptos(int.Parse(AlumnoId));
        }
        [WebMethod]
        public List<DTOPagos> ConsultarReferenciasCP2(string AlumnoId, string OfertaEducativaId)
        {
            return BLLPagoPortal.ConsultarReferenciasConceptos2(int.Parse(AlumnoId), int.Parse(OfertaEducativaId));
        }
        [WebMethod]
        public List<DTOPagos> ConsultarReferenciasCancelar(string AlumnoId)
        {
            return BLLPagoPortal.ConsultarReferenciasConceptosCancelar(int.Parse(AlumnoId));
        }
        [WebMethod]
        public List<ReferenciasPagadas> ConsultarReferenciasPagadasCortas(string AlumnoId)
        {
            return BLLPagoPortal.ReferenciasPagadasC(int.Parse(AlumnoId));
        }

        [WebMethod]
        public List<DTOAlumno> BuscarAlumnoFiltro(string filtro)
        {
            return BLLAlumnoPortal.BuscarAlumno(filtro);
        }

        [WebMethod]
        public List<DTOAlumno> BuscarAlumnoString(string Filtro)
        {
            return BLLAlumnoPortal.BuscarAlumnoTexto(Filtro);
        }


        [WebMethod]
        public PantallaPago ConsultaPagosDetalle(string AlumnoId)
        {
            try
            {
                return BLLPagoPortal.ReferenciasPago(int.Parse(AlumnoId));                
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        public doblesobj ConsultaPagosTramites(string AlumnoId)
        {
            doblesobj objr = new doblesobj();

            try
            {
                List<DTOPagoDetallado> lstRes = BLLPagoPortal.ConsultaPagosTramites(int.Parse(AlumnoId));
                bool rest;
                int cout = lstRes.Where(l => l.OtroDescuento.Length > 0).ToList().Count;
                rest = cout > 0 ? true : false;
                objr.item1 = lstRes;
                objr.item2 = rest;

                return objr;
            }
            catch
            {
                return objr;
            }
        }

        public class doblesobj
        {
            public List<DTOPagoDetallado> item1 { get; set; }
            public bool item2 { get; set; }
        }

        [WebMethod]
        public List<DTOPeriodo> ConsultarPeriodosAlumno(string AlumnoId)
        {
            return BLLPeriodoPortal.ConsultarPeriodos(int.Parse(AlumnoId));
        }
        [WebMethod]
        public List<DTOAlumnoPermitido> InsertarPermiso(string AlumnoId, string UsuarioId, string Descripcion)
        {
            return BLLAlumnoPermitido.InsertarAlumno(int.Parse(AlumnoId), int.Parse(UsuarioId), Descripcion);
        }

        [WebMethod]
        public bool GenerarCargos(DTOAlumnoInscrito objAlumno)
        {
            return BLLAlumnoPortal.GenerarCargos(objAlumno);
        }

        [WebMethod]
        public List<DTOAlumnoInscrito> ListaPorAutorizar()
        {
            return
            BLLAlumnoPortal.AlumnosPorAutorizar();
        }

        [WebMethod]
        public object ObenerDatosAlumno(string AlumnoId)
        {
            return BLLAlumnoPortal.ObtenerAlumnoCompleto(int.Parse(AlumnoId));
        }
        
        //datos personales  generados por el alumnos
        [WebMethod]
        public List<ReferenciasPagadas> ReferenciasConsulta(string Dato, string TipoBusqueda)
        {
            return BLLAlumnoPortal.ReferenciasConsulta(Dato, int.Parse(TipoBusqueda));
        }

        [WebMethod]
        public string TraerSede(string AlumnoId)
        {
            return BLLSede.SedeAlumno(int.Parse(AlumnoId));
        }

        [WebMethod]
        public bool UpdateAlumno(string AlumnoId, string UsuarioId,
            string Nombre, string Paterno, string Materno, string Celular, string FNacimiento, string CURP, string Email,
            string TelCasa, string Calle, string NumeroE, string NumeroI, string CP, string Colonia, string EstadoCivil,
            string Sexo, string Estado, string Municipio, string Nacionalidad, string LugarN, string NombrePA1, string PaternoPA1,
            string MaternoPA1, string PArentescoPA1, string EmailPA1, string TelefonoPA1, string Telefono2PA1, string Autoriza1,
            string NombrePA2, string PaternoPA2, string MaternoPA2, string PArentescoPA2, string EmailPA2, string TelefonoPA2,
            string Telefono2PA2, string Autoriza2)
        {

            int? defaul = null;
            DTOAlumno Alumno = new DTOAlumno
            {
                Nombre = Nombre,
                Paterno = Paterno,
                Materno = Materno,
                AlumnoId = int.Parse(AlumnoId),
            };
            Alumno.DTOAlumnoDetalle = new DTOAlumnoDetalle
            {
                AlumnoId = int.Parse(AlumnoId),
                Calle = Calle,
                Celular = Celular,
                Colonia = Colonia,
                Cp = CP,
                CURP = CURP,
                Email = Email,
                PaisId = int.Parse(Nacionalidad) == 1 ? 146 : int.Parse(LugarN),
                EntidadNacimientoId = int.Parse(Nacionalidad) == 1 ? int.Parse(LugarN) : defaul,
                EntidadFederativaId = int.Parse(Estado),
                EstadoCivilId = int.Parse(EstadoCivil),
                FechaNacimiento = DateTime.ParseExact((FNacimiento.Replace('-', '/')), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                GeneroId = int.Parse(Sexo),
                MunicipioId = int.Parse(Municipio),
                NoExterior = NumeroE,
                NoInterior = NumeroI,
                ProspectoId = int.Parse(AlumnoId),
                TelefonoCasa = TelCasa,
                TelefonoOficina = ""
            };
            Alumno.DTOPersonaAutorizada = new List<DTOPersonaAutorizada>();
            Alumno.DTOPersonaAutorizada.Add(
                new DTOPersonaAutorizada
                {
                    AlumnoId = int.Parse(AlumnoId),
                    Autoriza = bool.Parse(Autoriza1),
                    Celular = TelefonoPA1,
                    Email = EmailPA1,
                    Materno = MaternoPA1,
                    Nombre = NombrePA1,
                    ParentescoId = int.Parse(PArentescoPA1),
                    Paterno = PaternoPA1,
                    Telefono = Telefono2PA1
                });

            if (NombrePA2.Length > 1)
            {
                Alumno.DTOPersonaAutorizada.Add(new DTOPersonaAutorizada
                {
                    AlumnoId = int.Parse(AlumnoId),
                    Autoriza = bool.Parse(Autoriza2),
                    Celular = TelefonoPA2,
                    Email = EmailPA2,
                    Materno = MaternoPA2,
                    Nombre = NombrePA2,
                    ParentescoId = int.Parse(PArentescoPA2),
                    Paterno = PaternoPA2,
                    Telefono = Telefono2PA2
                });
            }
            
            return BLLAlumnoPortal.UpdateAlumno(Alumno, int.Parse(UsuarioId));
            
        }

        [WebMethod]
        public object UpdateMail(string AlumnoId, string Email, string UsuarId)
        {
            return BllAlumnoDetalle.UpdateEmail(int.Parse(AlumnoId), int.Parse(UsuarId), Email);
        }
        [WebMethod]
        public object TraerAlumnoDetalle(string AlumnoId)
        {
            return BllAlumnoDetalle.GetAlumnoDetalle(int.Parse(AlumnoId));
        }
        [WebMethod]
        public List<DTOAlumnoOfertas> OfertasAlumno(string AlumnoId)
        {
            return BLLAlumnoPortal.ObtenerOfertasAlumno(int.Parse(AlumnoId));
        }
        [WebMethod]
        public object GuardarAntecedentes(string AlumnoId, string AntecedenteTipoId, string AreaAcademicaId, string Procedencia,
        string Promedio, string Anio, string MesId, string EsEquivalencia, string EscuelaEquivalencia, string PaisId,
        string EntidadFederativaId, string EsTitulado, string TitulacionMedio, string MedioDifusionId, string UsuarioId)
        {

            if (AntecedenteTipoId == "-1") { return true; }
            try
            {
                DTOAlumnoAntecendente objG = new DTOAlumnoAntecendente();
                objG.AlumnoId = int.Parse(AlumnoId);
                objG.Anio = int.Parse(Anio);
                objG.AntecedenteTipoId = int.Parse(AntecedenteTipoId);
                objG.AreaAcademicaId = int.Parse(AreaAcademicaId);
                objG.EntidadFederativaId = int.Parse(EntidadFederativaId);
                objG.EscuelaEquivalencia = EscuelaEquivalencia;
                objG.EsEquivalencia = bool.Parse(EsEquivalencia);
                objG.EsTitulado = bool.Parse(EsTitulado);
                objG.MedioDifusionId = int.Parse(MedioDifusionId);
                objG.MesId = int.Parse(MesId);
                objG.PaisId = int.Parse(PaisId);
                objG.Procedencia = Procedencia;
                objG.Promedio = decimal.Parse(Promedio);
                objG.TitulacionMedio = TitulacionMedio;
                objG.UsuarioId = int.Parse(UsuarioId);

                return BLLAntecedente.GuardarAntecendente(objG);
            }
            catch
            {
                return false;
            }
        }

        [WebMethod]
        public string ConsultarAdeudo(string AlumnoId, string OfertaEducativaId)
        {
            return BLLPagoPortal.TraerAdeudos(int.Parse(AlumnoId), int.Parse(OfertaEducativaId));
        }
        [WebMethod]
        public string ConsultarAdeudoCoordinador(string AlumnoId, string OfertaEducativaId)
        {
            return BLLPagoPortal.TraerAdeudosCoordinadores(int.Parse(AlumnoId), int.Parse(OfertaEducativaId));
        }
        [WebMethod]
        public List<Universidad.DTO.EstadoCuenta.ReferenciaProcesada> EstadoDeCuenta(string AlumnoId, string FechaI, string FechaF)
        {
            return
            BLLEstadoCuenta.ObtenerCargos(new DTOAlumno { AlumnoId = int.Parse(AlumnoId) }, DateTime.Parse(FechaI), DateTime.Parse(FechaF),
            BLLEstadoCuenta.ObtenerAbonos(new DTOAlumno { AlumnoId = int.Parse(AlumnoId) }, DateTime.Parse(FechaI), DateTime.Parse(FechaF)));
        }
        

        //Cambio carrera//
        [WebMethod]
        public DTOAlumnoCambioCarrera ConsultaCambioCarrera(string AlumnoId, int UsuarioId)
        {
           return BLLAlumnoPortal.ConsultaCambioCarrera(int.Parse(AlumnoId), UsuarioId);
        }
        [WebMethod]
        public bool AplicarCambioCarrera(DTOAlumnoCambioCarrera Cambio)
        {
            return BLLAlumnoPortal.AplicarCambioCarrera(Cambio);
        }

        //Baja Academica//

        [WebMethod]
        public DTOCatalogoBaja ConsultaCatalogosBaja()
        {
            return BLLAlumnoPortal.ConsultaCatalogosBaja();
        }

        [WebMethod]
        public DTOAlumnoBaja ConsultaAlumnoBaja(string AlumnoId)
        {
            return BLLAlumnoPortal.ConsultaAlumnoBaja(int.Parse(AlumnoId));
        }

        [WebMethod]
        public int AplicarBaja(DTOAlumnoBaja Alumno)
        {
            return BLLAlumnoPortal.AplicarBaja(Alumno);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool GuardarDocumentoBaja()
        {
            HttpContext Contex = HttpContext.Current;
            HttpFileCollection httpFileCollection = Context.Request.Files;
            System.Collections.Specialized.NameValueCollection Formato = Context.Request.Form;
            try
            {
                int AlumnoMovimientoId = int.Parse(Formato["AlumnoMovimientoId"]);
                HttpPostedFile httpFormato = httpFileCollection["Documento"];
                Stream strFormato = httpFormato.InputStream;
                byte[] FormatoFil = Herramientas.ConvertidorT.ConvertirStream(strFormato, httpFormato.ContentLength);

                return BLLAlumnoPortal.GuardarDocumentoBaja(AlumnoMovimientoId, FormatoFil);
            }
            catch
            {
                return false;
            }
        }
        //actualizar alumno

        [WebMethod]
        public bool AtualizarAlumno(int AlumnoId, string Nombre, string Paterno, string Materno, string Nacimiento, int GeneroId, string CURP, int UsuarioId)
        {
            return BLLAlumnoPortal.UpdateAlumnoRP(AlumnoId, Nombre, Paterno, Materno, Nacimiento, GeneroId, CURP, UsuarioId);
        }
        //con consultar bitacora de acceso del alumno
        [WebMethod]
        public DTOBitacoraAccesoAlumno BitacoraAccesoAlumno(int AlumnoId)
        {
            return BLLAlumnoPortal.BitacoraAccesoAlumno(AlumnoId);
        }

        //Aplicacion de donativos//
        [WebMethod]
        public Universidad.DTO.DTOReferenciaDonativo BuscarReferencia(string Dato,int Buscar)
        {
            return BLL.BLLDonativo.BuscarReferencia(Dato,Buscar);
        }

        [WebMethod]
        public bool AplicarDonativo(Universidad.DTO.DTODonativo AlumnoDonativo)
        {
            return BLL.BLLDonativo.AplicarDonativo(AlumnoDonativo);
        }


        //  actualizar datos personales por el coordinador
        [WebMethod]
        public DTOAlumno ObenerDatosAlumnoCordinador(string AlumnoId)
        {
            return BLLAlumnoPortal.ObenerDatosAlumnoCordinador(int.Parse(AlumnoId));
        }

        [WebMethod]
        public bool UpdateAlumnoDatosCoordinador(DTOAlumnoDetalle AlumnoDatos)
        {
            return BLLAlumnoPortal.UpdateAlumnoDatosCoordinador(AlumnoDatos);
        }



    }
}
