using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Referencia
{
    public class DTOReferencia
    {
        //public string tipo { get; set; }
        //public string moneda { get; set; }
        //public string plaza { get; set; }
        //public string cuenta { get; set; }
        public DateTime fechaPago { get; set; }
        public string referenciaId { get; set; }
        public decimal importe { get; set; }
        public string movimiento { get; set; }
        //public decimal saldo { get; set; }
        //public string transaccion { get; set; }
        //public string leyenda { get; set; }
        //public string leyenda2 { get; set; }
        public int consecutivo { get; set; }
        public int estatusId { get; set; }
        public int alumnoId { get; set; }
        public decimal restante { get; set; }
    }
    public class DTODescuentoReferencia
    {
        public int pagoId { get; set; }
        public int descuentoId { get; set; }
        public decimal monto { get; set; }
    }
}
