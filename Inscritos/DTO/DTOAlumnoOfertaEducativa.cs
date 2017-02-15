using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAlumnoOfertaEducativa
    {
        public int AlumnoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public System.DateTime FechaAsignacion { get; set; }
        public int EstatusId { get; set; }
        public int Alumno { get; set; }        
        public int AlumnoInscrito { get; set; }
        public int OfertaEducativa { get; set; }
    }
}
