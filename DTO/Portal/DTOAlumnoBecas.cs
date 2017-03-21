using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAlumnoBecas
    {
        public string AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public int OfertaEducativaId { get; set; }
        public string OfertaEducativa { get; set; }
        public DateTime FechaReinscripcion { get; set; }
        public string FechaReinscripcionS { get; set; }
        public string BecaAcademica { get; set; }
        public string BecaSep { get; set; }
        public string Usuario { get; set; }
    }
    public class AlumnoPagos
    {
        public string AlumnoId { get; set; }
        public string Nombre { get; set; }
        public List<DTOOfertaEducativa> OfertasEducativas { get; set; }
        public string PeriodoId { get; set; }
        public string Anio { get; set; }
        public string PeriodoD { get; set; }
        public List<PagosAlumnos> lstPagos { get; set; }
        public Boolean Inscrito { get; set; }
        public int estatusid { get; set; }
        public bool NuevoIngreso { get; set; }
        public bool SEP { get; set; }
        public bool Academica { get; set; }
        public bool Comite { get; set; }
        public int Materias { get; set; }
        public int Asesorias { get; set; }
        public bool Revision { get; set; }
        public string Grupo { get; set; }
        public bool EsEmpresa { get; set; }
        public bool EsEspecial { get; set; }
        public bool Completa { get; set; }
    }
    public class PagosAlumnos
    {
        public string Concepto { get; set; }
        public string PagoId { get; set; }
        public string ReferenciaId { get; set; }
        public string Cargo { get; set; }
        public decimal CargoD { get; set; }
        public string BecaSEP { get; set; }
        public string BecaAcademica { get; set; }
        public decimal BecaSEPD { get; set; }
        public decimal BecaAcademicaD { get; set; }
        public string TotalPagar { get; set; }
        public int SubPeriodo { get; set; }
    }
}
