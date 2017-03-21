using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOPago
    {
        public DTOPago()
        {
            enUso = false;
            check = false;
        }

        [DisplayName(" ")]
        public bool enUso { get; set; }
        public int pagoId { get; set; }
        [DisplayName("Descripción")]
        public string conceptoPago { get; set; }
        public int conceptoPagoId { get; set; }
        [DisplayName("Cuota")]
        public decimal cuota { get; set; }
        public int cuotaId { get; set; }
        [DisplayName("Descuento")]
        public decimal descuento { get; set; }
        [DisplayName("Importe")]
        public decimal importe { get; set; }
        public int estatusId { get; set;}
        public int mesId { get; set; }
        public bool adeudo { get; set; }
        public bool check { get; set; }
        [System.ComponentModel.Browsable(false)]
        public List<DTOPagoDescuento> Descuentos { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int anio { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int periodoId { get; set; }
        public bool esVariable { get; set; }
    }
}
