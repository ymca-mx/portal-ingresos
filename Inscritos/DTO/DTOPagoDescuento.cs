using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPagoDescuento
    {
        public int PagoId { get; set; }
        public int DescuentoId { get; set; }
        public decimal Monto { get; set; }
        public DTOAlumnoDescuento DTOAlumnDes { get; set; }
    }
}
