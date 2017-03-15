using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebServices.WS
{
    /// <summary>
    /// Descripción breve de General
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class General : System.Web.Services.WebService
    {
        [WebMethod]
        public string ActivarPago(string PagoId)
        {
           return BLLPago.ActivarPago(int.Parse(PagoId));
        }

        [WebMethod]
        public List<DTOGenero> ConsultarGenero()
        {
            return BLLGenero.ConsultaTodosGenero();
        }

        [WebMethod]
        public List<DTOParentesco> ConsultarParentesco()
        {
            return BLLParentesco.ConsultarTodosParentesco();
        }

        [WebMethod]
        public List<DTOPais> ConsultarPaises()
        {
            return BLLPais.TraerPaises();
        }
        [WebMethod]
        public List<DTOPais> ConsultarPaisesT()
        {
            return BLLPais.TraerPaisesT();
        }
        [WebMethod]
        public List<DTOOfertaEducativaTipo> ConsultarOfertaEducativaTipo(string Plantel)
        {
            return BLLOfertaEducativaTipo.ConsultaOfertaTipo(int.Parse(Plantel));
        }
        [WebMethod]
        public List<DTOOfertaEducativa> ConsultarOfertaEducativa(string tipoOferta, string Plantel)
        {
            return BLLOfertaEducativa.ConsultarOfertasEducativas(int.Parse(tipoOferta), int.Parse(Plantel));
        }
        [WebMethod]
        public List<DTOOfertaEducativa> ConsultarOfertaEducativaAlumno(string AlumnoId)
        {
            return BLLOfertaEducativa.OfertaEducativaAlumno(int.Parse(AlumnoId));
        }
        [WebMethod]
        public List<DTOTurno> ConsultarTurnos()
        {
            return BLLTurno.ConsultarTurno();
        }
        [WebMethod]
        public List<DTOPeriodo> ConsultarPeriodos()
        {
            return BLLPeriodo.ConsultarPeriodos();
        }
        [WebMethod]
        public List<DTOEstadoCivil> ConsultarEstadoCivil()
        {
            return BLLEstadoCivil.ConsultarEstadosCiviles();
        }
        [WebMethod]
        public List<DTOEntidadFederativa> ConsultarEntidadFederativa()
        {
            return BLLEntidadFederativa.ConsultarEstadosCiviles();
        }
        [WebMethod]
        public List<DTOSucursal> ConsultarPlantel()
        {
            return BLLSucursal.ConsultarSucursales();
        }
        [WebMethod]
        public List<DTOSucursal> ConsultarPlantelEmpresas()
        {
            return BLLSucursal.ConsultarSucursalesEmpresa();
        }
        [WebMethod]
        public List<DTOPagoPlan> ConsultarPagosPlan(string AlumnoId)
        {
            return BLLPagoPlan.ConsultarPagos(int.Parse(AlumnoId));
        }
        [WebMethod]
        public List<DTOPagoPlan> ConsultarPagosPlan2(string AlumnoId, string OfertaEducativaId)
        {
            return BLLPagoPlan.ConsultarPagos(int.Parse(AlumnoId), int.Parse(OfertaEducativaId));
        }
        [WebMethod]
        public List<DTOMunicipio> ConsultarMunicipios(int EntidadFederativaId)
        {
            return BLLMunicipio.ConsultarMunicipios(EntidadFederativaId);
        }
        [WebMethod]
        public List<DTOPagoPlan> ConsultarPagosPlanLenguas(int Oferta)
        {
            return BLLPagoPlan.ConsultarPagosPlanLenguas(Oferta);
        }
        [WebMethod]
        public string BuscarLengua(string AlumnoId, string Idioma, string Periodo)
        {
            return BLLOfertaEducativa.BuscarOfertaEnAlumnos(int.Parse(AlumnoId), int.Parse(Idioma), new DTOPeriodo
            {
                Anio = BLLPeriodo.ConsultarPeriodo(Periodo).Anio,
                PeriodoId = int.Parse(Periodo.Substring(0, 1))
            });
        }
        [WebMethod]
        public DTOPeriodo PeriodosCompletos(string Periodo, string ofertaId)
        {
            return BLLPeriodo.TraerPeriodoCompleto(BLLPeriodo.ConsultarPeriodo(Periodo).Anio, int.Parse(Periodo.Substring(0, 1)), int.Parse(ofertaId));
        }
        [WebMethod]
        public List<DTOAreaAcademica> ObtenerAreas()
        {
            return BLLAreaAcademica.AreasAcademicas();
        }
        [WebMethod]
        public DTOCuota TraerCuotaUnPago(string OfertaEducativa, string PagoPlan, string Periodo)
        {
            return BLLCuota.CuotaUnPago(int.Parse(OfertaEducativa), int.Parse(PagoPlan), Periodo);
        }
        [WebMethod]
        public List<DTOMedioDifusion> TraerListaMedios()
        {
            return BLLMedioDifusion.ConsultarListadeMedios();
        }
        [WebMethod]
        public List<DTOCuota> Conceptos(string AlumnoId, string OfertaEducativa)
        {
            return BLLPagoConcepto.ListaPagoConceptos(int.Parse(AlumnoId), int.Parse(OfertaEducativa));
        }
        [WebMethod]
        public List<DTOCuota> Conceptos2(string AlumnoId, string OfertaEducativa, string UsuarioId)
        {
            return BLLPagoConcepto.ListaPagoConceptos2(int.Parse(AlumnoId), int.Parse(OfertaEducativa), int.Parse(UsuarioId));
        }
        [WebMethod]
        public string NombreCalendario(string Alumno)
        {
            return BLLAlumnoInscrito.NombreCalendario(int.Parse(Alumno));

        }
        [WebMethod]
        public string GenerarPagoZS()
        {
            return BLLAlumnoDescuento.GenerarReferenciasPagos();
        }
        [WebMethod]
        public DTOPagoConcepto ConsultarPagoConcepto(string OfertaEducativaId, string PagoConceptoId)
        {
            return BLLPagoConcepto.TraerPagoConcepto(int.Parse(OfertaEducativaId), int.Parse(PagoConceptoId));
        }
        [WebMethod]
        public DTOPagoConcepto ConsultarPagoConcepto2(string OfertaEducativaId, string PagoConceptoId)
        {
            return BLLPagoConcepto.TraerPagoConcepto(int.Parse(OfertaEducativaId), int.Parse(PagoConceptoId));
        }
        [WebMethod]
        public List<DTODia> Dias()
        {
            return BLLDia.ConsultarDias();
        }
        [WebMethod]
        public List<Oferta_Costo> Ofertas_costos_Alumno(string AlumnoId, string Anio, string PeriodoId)
        {
            return BLLCuota.TraerOfertasCuotasAlumno(int.Parse(AlumnoId), int.Parse(Anio), int.Parse(PeriodoId));
        }
        [WebMethod]
        public void EstadodeCuenta(string AlumnoId, string Anio, string PeriodoId)
        {
            //return BLLEstadoCuenta.ConsultarEstadodeCuenta(int.Parse(AlumnoId), int.Parse(PeriodoId), int.Parse(Anio));
            //return BLLEstadoCuenta.TestEstadoCuenta2(int.Parse(AlumnoId), int.Parse(PeriodoId), int.Parse(Anio));*/

        }
        [WebMethod]
        public DTOUsuario ObtenerUsuario(string UsuarioId)
        {
            return BLLUsuario.ObtenerUsuario(int.Parse(UsuarioId));
        }
        [WebMethod]
        public string CancelarPago(string PagoId, string Comentario, string UsuarioId, string Estatus)
        {
            if (Estatus == "Pendiente")
            {
                return BLLPago.CancelarPago(int.Parse(PagoId), Comentario, int.Parse(UsuarioId));
            }
            else
            {
                try { BLLCargo.CancelarTotal(int.Parse(PagoId), int.Parse(UsuarioId), Comentario);  return "Guardado"; }
                catch (Exception d) { return d.Message; }
            }
        }


        //PagoCancelacionSolicitud DE CANCELACION

        [WebMethod]
        public string PagoCancelacionSolicitud(string PagoId, string Comentario, string UsuarioId)
        {

            PagoId = PagoId.Remove(PagoId.Length - 1);
            return BLLPago.PagoCancelacionSolicitud(int.Parse(PagoId), Comentario, int.Parse(UsuarioId));
        }

        [WebMethod]
        public List<DTOPagoCancelacionSolicitud> ConsultarPagoCancelacionSolicitud( string UsuarioId,string Tipo)
        {
            return BLLPago.ConsultarPagoCancelacionSolicitud(int.Parse(UsuarioId), int.Parse(Tipo));
        }

        [WebMethod]
        public string CambiarPagoCancelacionSolicitud(string SolicitudId, string UsuarioId, string Tipo)
        {

            return BLLPago.CambiarPagoCancelacionSolicitud(int.Parse(SolicitudId), int.Parse(UsuarioId), int.Parse(Tipo));
        }

        // FIN PagoCancelacionSolicitud DE CANCELACION

        [WebMethod]
        public List<DTOPeriodo> PeriodosAlumno(string AlumnoId, string OfertaEducativaId)
        {
            return BLLPeriodo.ConsutlarPeriodosAlumno(int.Parse(AlumnoId), int.Parse(OfertaEducativaId));

        }
        [WebMethod]
        public string GenerarInscripcion(string Alumnoid, string OFerta, string anio, string periodo, string Pagoconcepto, string subPEriodo)
        {

            return BLLPago.GenerarPagoInscripcion(int.Parse(Alumnoid), int.Parse(OFerta), int.Parse(anio), int.Parse(periodo), int.Parse(Pagoconcepto), int.Parse(subPEriodo));
        }
        [WebMethod]
        public DTOPeriodo GetPeriodoActual()
        {
            return BLLPeriodo.TraerPeriodoEntreFechas(DateTime.Now);
        }

        [WebMethod]
        public List<DTOOfertaEducativaTipo> OfertaEducativaTipo()
        {
           return BLLOfertaEducativaTipo.ConsultaOfertaTipo();
        }
    }
}
