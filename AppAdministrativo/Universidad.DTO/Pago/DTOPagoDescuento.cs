using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOPagoDescuento
    {
         [System.ComponentModel.Browsable(false)]
        public int descuentoId { get; set; }
        [DisplayName("Descripción")]
        public string descripcion { get; set; }
        [DisplayName("Tipo de descuento")]
        public string descuentoTipo { get; set; }
        [DisplayName("Monto o Porcentaje")]
        public string porcentaje { get; set; }
        [DisplayName("Importe")]
        public decimal importe { get; set; }
        [DisplayName("Observaciones")]
        public string observacion { get; set; }
        public int alumnoDescuentoId { get; set; }

    }
}
