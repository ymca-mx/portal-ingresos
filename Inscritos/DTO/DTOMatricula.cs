using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOMatricula
    {
        public string MatriculaId { get; set; }
        public int AlumnoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public System.DateTime FechaAsignacion { get; set; }
        public int UsuarioId { get; set; }
    }
}
