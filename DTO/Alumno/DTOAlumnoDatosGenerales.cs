using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOAlumnoDatosGenerales
    {
        public int alumnoId { get; set; }
        public string nombre { get; set; }
        public string ofertaEducativa { get; set; }
        public int anio { get; set; }
        public int periodo { get; set; }
        public int ofertaEducativaId { get; set; }
        public List<DTO.DTOAlumnoOfertaEducativa> Ofertas { get; set; }
    }
}
