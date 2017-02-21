using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ReferenciasPagadas
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string Caja { get; set; }
        public DateTime FechaPagoD { get; set; }
        public string FechaPago { get; set; }
        public string ReferenciaId { get; set; }
        public string MontoPagado { get; set; }
        public string MontoReferencia { get; set; }
        public string Saldo { get; set; }
        public decimal SaldoD { get; set; }

    }
}
