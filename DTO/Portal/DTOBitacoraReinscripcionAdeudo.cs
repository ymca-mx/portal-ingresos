using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOBitacoraReinscripcionAdeudo
    {
        public int AlumnoId { get; set; }
        public System.DateTime FechaOperacion { get; set; }
        public System.TimeSpan HoraOperacion { get; set; }
        public bool EsBiblioteca { get; set; }
        public bool EsChocolates { get; set; }
        public int UsuarioId { get; set; }
        public string Observaciones { get; set; }
        public bool EsActivo { get; set; }

        public DTOAlumno Alumno { get; set; }
    }
}
