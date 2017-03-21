using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOPeriodo
    {
        public int anio { get; set; }
        public int periodoId { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaFinal { get; set; }
    }
}
