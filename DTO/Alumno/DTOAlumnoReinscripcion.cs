using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOAlumnoReinscripcion
    {
        public int alumnoId { get; set; }
        public string nombre { get; set; }
        public bool biblioteca { get; set; }
        public bool doctos { get; set; }
        public bool adeudo { get; set; }
        public decimal beca { get; set; }
    }
}
