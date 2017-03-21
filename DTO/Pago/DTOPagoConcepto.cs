using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOPagoConcepto
    {
        public int pagoId { get; set; }
        public string cuatrimestre { get; set; }
        public string descripcion { get; set; }
        public int conceptoPagoId { get; set; }
        public decimal cuota { get; set; }
        public decimal descuento { get; set; }
        public decimal importe { get; set; }
        public int estatus { get; set; }
        public List<DTOPagoDescuento> Descuentos { get; set; }
        public int cuotaId { get; set; }
        public bool esVariable { get; set; }
    }
}
