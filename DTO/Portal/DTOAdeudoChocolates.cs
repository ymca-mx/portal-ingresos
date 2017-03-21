using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAdeudoChocolates
    {
        public int AlumnoId { get; set; }
        public System.DateTime FechaOperacion { get; set; }
        public string Observaciones { get; set; }
        public int UsuarioId { get; set; }

        public DTOAlumno Alumno { get; set; }
        public DTOUsuario Usuario { get; set; }
    }
}
