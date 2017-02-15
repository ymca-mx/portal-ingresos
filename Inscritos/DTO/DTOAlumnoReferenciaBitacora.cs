using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAlumnoReferenciaBitacora
    {
            public int BitacoraId { get; set; }
            public int AlumnoId { get; set; }
            public int OfertaEducativaId { get; set; }
            public int PagoConceptoId { get; set; }
            public Nullable<int> Anio { get; set; }
            public Nullable<int> PeriodoId { get; set; }
            public System.DateTime FechaGeneracion { get; set; }
            public System.TimeSpan HoraGeneracion { get; set; }
            public int PagoId { get; set; }

            public virtual DTOAlumno Alumno { get; set; }
            public virtual DTOPagos Pago { get; set; }
            public virtual DTOPagoConcepto PagoConcepto { get; set; }
    }
}
