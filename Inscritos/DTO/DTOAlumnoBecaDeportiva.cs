using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Alumno.Beca
{
    public class DTOAlumnoBecaDeportiva
    {
        public int alumnoId { get; set; }
        public int anio { get; set; }
        public int periodoId { get; set; }
        public decimal porcentajeBeca { get; set; }
        public int ofertaEducativaId { get; set; }
        public int usuarioId { get; set; }
    }
}