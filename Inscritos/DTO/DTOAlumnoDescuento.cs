using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAlumnoDescuento
    {
        public int AlumnoDescuentoId { get; set; }
        public int AlumnoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public int DescuentoId { get; set; }
        public int ConceptoId { get; set; }
        public decimal Monto { get; set; }
        public decimal MontoDep { get; set; }
        public string SMonto { get; set; }
        public int EstatusId { get; set; }
        public DTODescuentos Descuento { get; set; }
        public DTOUsuario Usuario { get; set; }
        public System.DateTime FechaGeneracion { get; set; }
        public Nullable<System.DateTime> FechaAplicacion { get; set; }
        public string FechaAplicacionS { get; set; }
        public string BecaSEP { get; set; }
        public string OtrosDescuentos { get; set; }
        public string BecaDeportiva { get; set; }
        public string DescripcionPeriodo { get; set; }
        public string AnioPeriodoId { get; set; }
        public string BecaComite { get; set; }
        public string DocAcademicaId { get; set; }
        public string DocComiteRutaId { get; set; }
        public string pMensaje { get; set; }
        public bool esEmpresa { get; set; }
    }
}
