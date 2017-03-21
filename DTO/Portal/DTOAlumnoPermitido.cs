using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAlumnoPermitido
    {
        public int AlumnoId { get; set; }
        public int UsuarioId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public System.TimeSpan HoraRegistro { get; set; }
        public string FechaRegistroS { get; set; }
        public string HoraRegistroS { get; set; }
        public string Descripcion { get; set; }
        public DTOAlumno Alumno { get; set; }
        public  DTOUsuario Usuario { get; set; }
    }
}
