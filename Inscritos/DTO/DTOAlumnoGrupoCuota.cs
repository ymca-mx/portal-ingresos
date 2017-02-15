using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public partial class DTOAlumnoGrupoCuota
    {
        public int AlumnoId { get; set; }
        public int OFertaEducativaId { get; set; }
        public int PeriodoId { get; set; }
        public int Anio { get; set; }
        public int GrupoId { get; set; }
        public Nullable<decimal> MontoInscripcion { get; set; }
        public Nullable<decimal> MontoColegiatura { get; set; }
    }
}
