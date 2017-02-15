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

    public class DTOFIltros
    {
        public List<DTOCuatrimestre> item1 { get; set; }
        public List<DTOOfertaEducativa1> item2 { get; set; }
    }
    public class DTOCuatrimestre
    {
        public int anio { get; set; }
        public int periodoId { get; set; }
        public DateTime fechaInicial1 { get; set; }
        public DateTime fechaFinal1 { get; set; }
        public string fechaInicial { get; set; }
        public string fechaFinal { get; set; }
        public string descripcion { get; set; }

    }

    public class DTOOfertaEducativa1
    {
        public int ofertaEducativaId { get; set; }
        public string descripcion { get; set; }
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
        public int edad { get; set; }
        public System.DateTime fechaNacimiento1 { get; set; }
        public string fechaNacimiento { get; set; }
        public string lugarNacimiento { get; set; }
        public string tipoAlumno { get; set; }
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
}
