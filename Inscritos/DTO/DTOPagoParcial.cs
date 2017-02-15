using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPagoParcial
    {
        public int PagoId { get; set; }
        public int SucursalCajaId { get; set; }
        public int ReciboId { get; set; }
        public string Serie { get; set; }
        public decimal Pago { get; set; }
        public string PagoS { get; set; }
        public System.DateTime FechaPago { get; set; }
        public System.TimeSpan HoraPago { get; set; }
        public bool EsReferencia { get; set; }
        public DTOPagos Pago1 { get; set; }
        public List<DTOPagoDetalle> PagoDetalle { get; set; }
        public DTORecibo Recibo { get; set; }
    }
}
