using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOCorteCajaConcepto
    {
        public int reciboId { get; set; }
        public string caja { get; set; }
        public string cajero { get; set; }
        public string alumno { get; set; }
        public string metodoPago { get; set; }
        public decimal importe { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
