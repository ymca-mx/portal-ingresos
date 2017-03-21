using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPagoRecargo
    {
        public int PagoId { get; set; }
        public int PagoIdRecargo { get; set; }
        public DateTime Fecha { get; set; }
    }
}
