using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AppAlumnos.Services
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
        public string NombreCalendario(string Alumno)
        {
            return BLLAlumnoInscrito.NombreCalendario(int.Parse(Alumno));

        }

        [WebMethod]
        public List<DTOEntidadFederativa> ConsultarEntidadFederativa()
        {
            return BLLEntidadFederativa.ConsultarEstadosCiviles();
        }

        [WebMethod]
        public List<DTOMunicipio> ConsultarMunicipios(int EntidadFederativaId)
        {
            return BLLMunicipio.ConsultarMunicipios(EntidadFederativaId);
        }


        [WebMethod]
        public List<DTOPais> ConsultarPaises()
        {
            return BLLPais.TraerPaises();
        }

        [WebMethod]
        public List<Oferta_Costo> Ofertas_costos_Alumno(string AlumnoId, string Anio, string PeriodoId)
        {
            return BLLCuota.TraerOfertasCuotasAlumno(int.Parse(AlumnoId), int.Parse(Anio), int.Parse(PeriodoId));
        }

        [WebMethod]
        public List<DTOCuota> Conceptos(string AlumnoId, string OfertaEducativa)
        {
            return BLLPagoConcepto.ListaPagoConceptos(int.Parse(AlumnoId), int.Parse(OfertaEducativa));
        }

        [WebMethod]
        public DTOPagoConcepto ConsultarPagoConcepto(string OfertaEducativaId, string PagoConceptoId)
        {
            return BLLPagoConcepto.TraerPagoConcepto(int.Parse(OfertaEducativaId), int.Parse(PagoConceptoId));
        }


        [WebMethod]
        public List<DTOGenero> ConsultarGenero()
        {
            return BLLGenero.ConsultaTodosGenero();
        }

        [WebMethod]
        public List<DTOEstadoCivil> ConsultarEstadoCivil()
        {
            return BLLEstadoCivil.ConsultarEstadosCiviles();
        }
        
    }
}
