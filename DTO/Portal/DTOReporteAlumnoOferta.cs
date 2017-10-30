﻿using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DTO
{
    public class DTOReporteAlumnoOferta
    {
        public int alumnoId { get; set; }
        public string nombreAlumno { get; set; }
        public string oferta { get; set; }
        public string ultimoAnio { get; set; }
    }

    public  class DTOReporteBecasDetalle
    {
        public Nullable<int> AlumnoId { get; set; }
        public Nullable<int> OfertaEducativaId { get; set; }
        public string Descripcion { get; set; }
        public string CostoIns { get; set; }
        public string AnticipadoIns { get; set; }
        public string AnticipadoInsPor { get; set; }
        public string BecaIns { get; set; }
        public string BecaInsPor { get; set; }
        public string DesTotalIns { get; set; }
        public string DesTotalInsPor { get; set; }
        public string TotalIns { get; set; }
        public string CostoCol { get; set; }
        public string AnticipadoCol { get; set; }
        public string AnticipadoColPor { get; set; }
        public string BecaCol { get; set; }
        public string BecaColPor { get; set; }
        public string BecaDeportiva { get; set; }
        public string BecaDeportivaPor { get; set; }
        public string PromoCasa { get; set; }
        public string PromoCasaPor { get; set; }
        public string DesTotalCol { get; set; }
        public string DesTotalColPor { get; set; }
        public string TotalCol { get; set; }
        public string EsEmpresa { get; set; }
    }

    public class DTOReporteBecasConcentrado
    {
        public string Descripcion { get; set; }
        public Nullable<int> TotalAlumnos { get; set; }
        public Nullable<int> AlumnosConBeca { get; set; }
        public string CargosInscripcion { get; set; }
        public string PromedioAnticipadoPor { get; set; }
        public string PromedioAnticipado_ { get; set; }
        public string PromedioBecaInscripcionPor { get; set; }
        public string PromedioBecaInscripcion_ { get; set; }
        public string TotalDescuentoInscripcion { get; set; }
        public string CargosColegiatura { get; set; }
        public string PromedioAnticipadoPorColegiatura { get; set; }
        public string PromedioAnticipado_Colegiatura { get; set; }
        public string BecaPromedioPor { get; set; }
        public string BecaPromedio_ { get; set; }
        public string BecaDeportivaPor { get; set; }
        public string BecaDeportivaPromedio_ { get; set; }
        public string PromoCasaPor { get; set; }
        public string PromoCasa_ { get; set; }
        public string TotalDescuentoColegiatura { get; set; }
    }

    public class DTOReporteBecas
    {
        public List<DTOReporteBecasDetalle> Detalle { get; set; }
        public List<DTOReporteBecasConcentrado> Concentrado { get; set;}
        public List< DTOBecasCalculos> Calculos1 { get; set; }
        public List<DTOBecasCalculos> Calculos2 { get; set; }
    }

    public  class DTOReporteSaldos
    {
        public List<DTOSaldoAlumno> Alumnos { get; set; }
        public List<DTOSaldosOfertas> Ofertas { get; set; }
        public List<DTOPeriodoSaldos> Periodos { get; set; }
    }

    public class DTOPeriodoSaldos
    {
        public string Descripcion { get; set; }
        private string DescripcionCorta_ { get; set; }
        public string DescripcionCorta
        {
            get {return DescripcionCorta_; }
            set { DescripcionCorta_= value.Split(' ')[0].Substring(0, 3) + value.Split(' ')[1] + value.Split(' ')[2].Substring(0, 3) + " " + value.Split(' ')[3] ; }
        }
    }


    public class DTOSaldoAlumno
    {
        public Nullable<int> AlumnoId { get; set; }
        public string Nombre { get; set; }
        public List<DTOSaldosDetalle> Detalle { get; set; }
        public string SaldoTotal { get; set; }
    }

    public class DTOSaldosDetalle
    {
        public int AlumnoId { get; set; }
        public string Periodo { get; set; }
        public string Saldo { get; set; }
    }

    public class DTOSaldosOfertas
    {
        public int OfertaEducativaId { get; set; }
        public string Descripcion { get; set; }
        public List<DTOSaldoPeriodos> Periodos { get; set; }
        public string SaldoTotal { get; set; }
    }

    public class DTOSaldoPeriodos
    {
        public string Periodo { get; set; }
        public string Saldo { get; set; }
    }

    


    public class DTOBecasCalculos
    {
        public string valor { get; set; }
        public string valor2 { get; set; }
    }
    

    //public  class DTOBecasDetalle
    //{
    //    public Nullable<int> AlumnoId { get; set; }
    //    public Nullable<int> OfertaEducativaId { get; set; }
    //    public string Descripcion { get; set; }
    //    public Nullable<decimal> CostoIns { get; set; }
    //    public Nullable<int> AnticipadoIns { get; set; }
    //    public Nullable< decimal> AnticipadoInsPor { get; set; }
    //    public Nullable<int> BecaIns { get; set; }
    //    public string BecaInsPor { get; set; }
    //    public Nullable<decimal> DesTotalIns { get; set; }
    //    public string DesTotalInsPor { get; set; }
    //    public Nullable<decimal> TotalIns { get; set; }
    //    public Nullable<decimal> CostoCol { get; set; }
    //    public Nullable<int> AnticipadoCol { get; set; }
    //    public string AnticipadoColPor { get; set; }
    //    public Nullable<int> BecaCol { get; set; }
    //    public string BecaColPor { get; set; }
    //    public Nullable<int> BecaDeportiva { get; set; }
    //    public string BecaDeportivaPor { get; set; }
    //    public Nullable<int> PromoCasa { get; set; }
    //    public string PromoCasaPor { get; set; }
    //    public Nullable<decimal> DesTotalCol { get; set; }
    //    public string DesTotalColPor { get; set; }
    //    public Nullable<decimal> TotalCol { get; set; }
    //    public string EsEmpresa { get; set; }
    //}

    //public class DTOBecasConcentrado
    //{
    //    public string Descripcion { get; set; }
    //    public Nullable<int> TotalAlumnos { get; set; }
    //    public Nullable<int> AlumnosConBeca { get; set; }
    //    public Nullable<decimal> CargosInscripcion { get; set; }
    //    public string PromedioAnticipadoPor { get; set; }
    //    public Nullable<int> PromedioAnticipado_ { get; set; }
    //    public string PromedioBecaInscripcionPor { get; set; }
    //    public Nullable<int> PromedioBecaInscripcion_ { get; set; }
    //    public Nullable<decimal> TotalDescuentoInscripcion { get; set; }
    //    public Nullable<decimal> CargosColegiatura { get; set; }
    //    public string PromedioAnticipadoPorColegiatura { get; set; }
    //    public Nullable<int> PromedioAnticipado_Colegiatura { get; set; }
    //    public string BecaPromedioPor { get; set; }
    //    public Nullable<int> BecaPromedio_ { get; set; }
    //    public string BecaDeportivaPor { get; set; }
    //    public Nullable<int> BecaDeportivaPromedio_ { get; set; }
    //    public string PromoCasaPor { get; set; }
    //    public Nullable<int> PromoCasa_ { get; set; }
    //    public Nullable<decimal> TotalDescuentoColegiatura { get; set; }
    //}

    public class DTOCuatrimestre
    {
        public List<DTOPeriodo2> periodos { get; set; }
        public List<DTOOfertaEducativa2> ofertas { get; set; }
    }
    public class DTOPeriodo2
    {
        public int anio { get; set; }
        public int periodoId { get; set; }
        public DateTime fechaInicial1 { get; set; }
        public DateTime fechaFinal1 { get; set; }
        public string fechaInicial { get; set; }
        public string fechaFinal { get; set; }
        public string descripcion { get; set; }
        public DTOInscripcionDetalle Inscripcion { get; set; }
        public DTOVistoBuenoDetalle VistoBueno { get; set; }
        public DTOSolicitudInscripcion Solicitud { get; set; }
    }

    public class DTOInscripcionDetalle
    {
        public string Usuario { get; set; }
        public string Fecha { get; set; }
    }
    public class DTOVistoBuenoDetalle
    {
        public string Usuario { get; set; }
        public string Fecha { get; set; }
    }


    public class DTOOfertaEducativa2
    {
        public int ofertaEducativaId { get; set; }
        public string descripcion { get; set; }
        public int  sucursalid { get; set; }
    }


    public class DTOReporteBecasCuatrimestre
    {
        public int alumnoId { get; set; }
        public string nombreAlumno { get; set; }
        public string especialidad { get; set; }
        public int especialidadId { get; set; }
        public string becaDescuento { get; set; }
        public string porcentajeDescuento { get; set; }
        public string comentario { get; set; }
        public System.DateTime fechaGeneracion1 { get; set; }
        public string fechaGeneracion { get; set; }
        public string horaGeneracion { get; set; }
        public string usuarioAplico { get; set; }
    }

    

    public class DTOReporteInscrito
    {
        public int alumnoId { get; set; }
        public string nombreAlumno { get; set; }
        public string especialidad { get; set; }
        public int especialidadId { get; set; }
        public System.DateTime fechaInscripcion1 { get; set; }
        public string fechaInscripcion { get; set; }
        public string porcentajeDescuento { get; set; }
        public string tipoAlumno { get; set; }
        public string esEmpresa { get; set; }
        public string usuarioAplico { get; set; }
    }

    public class DTOReporteBecaSep
    {
        public int alumnoId { get; set; }
        public string nombreAlumno { get; set; }
        public string especialidad { get; set; }
        public int especialidadId { get; set; }
        public string porcentajeDescuento { get; set; }
        public string comentario { get; set; }
        public DateTime fechaGeneracion1 { get; set; }
        public string fechaGeneracion { get; set; }
        public string horaGeneracion { get; set; }
        public string usuarioAplico { get; set; }
        public int usuarioAplicoId { get; set; }
        public int alumnoDescuentoId { get; set; }
        public bool borrar { get; set; }
        public bool esSEP { get; set; }
        public bool esComite { get; set; }

    }

    public class DTOReporteInegi
    {
        public int alumnoId { get; set; }
        public string nombreAlumno { get; set; }
        public string ciclo { get; set; }
        public string especialidad { get; set; }
        public int especialidadId { get; set; }
        public string sexo { get; set; }
        public string edad { get; set; }
        public System.DateTime fechaNacimiento1 { get; set; }
        public string fechaNacimiento { get; set; }
        public string lugarNacimiento { get; set; }
        public string lugarEstudio { get; set; }
        public string tipoAlumno { get; set; }
        public string Cuatrimestre { get; set; }
    }

    public class DTOReporteAlumnoReferencia
    {
        public int alumnoId { get; set; }
        public string nombreAlumno { get; set; }
        public string especialidad { get; set; }
        public int especialidadId { get; set; }
        public string inscripcion { get; set; }
        public string colegiatura { get; set; }
        public string materiaSuelta { get; set; }
        public string asesoriaEspecial { get; set; }
        public int noMaterias { get; set; }
        public string calificacionMaterias { get; set; }
        public int noBaja { get; set; }
        public string bajaMaterias { get; set; }
        public int tipo { get; set; }

    }
    public class DTOReporteAlumnoReferencia1
    {
        public int pagoId { get; set; }
        public int alumnoId { get; set; }
        public int ofertaId { get; set; }
        public int pagoConcepto { get; set; }
        public int estatusId { get; set; }
        public int suma { get; set; }
        public int tipo {get; set; }
    }

    public class DTOReporteVoBo
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string OfertaEducativa { get; set; }
        public string Inscrito { get; set; }
        public int OfertaEducativaid{get;set; }
        public string FechaInscrito { get; set; }
        public string HoraInscrito { get; set; }
        public string UsuarioInscribio { get; set; }
        public string FechaVoBo { get; set; }
        public string HoraVoBo { get; set; }
        public string InscripcionCompleta { get; set; }
        public string Asesorias { get; set; }
        public string Materias { get; set; }
        public string UsuarioVoBo { get; set; }
        public string Email { get; set; }
    }

    public class DTOAlumnosVoBo
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public AlumnoInscrito AlumnoInscrito { get; set; }
        public  AlumnoInscritoBitacora AlumnoInscritoBitacora  { get; set; }
        public AlumnoRevision AlumnoRevision { get; set; }
        public string Email { get; set; }
    }

    public class DTOAlumnosVoBo1
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public AlumnoInscritoCompleto AlumnoInscrito { get; set; }
        public AlumnoInscritoBitacora AlumnoInscritoBitacora { get; set; }
        public AlumnoRevision AlumnoRevision { get; set; }
        public string Email { get; set; }
    }



    public class DTOVoBo
    {
        public List<DTOReporteVoBo> AlumnoVoBo { get; set; }
        public bool EsEscolares { get; set; }
    }
}
