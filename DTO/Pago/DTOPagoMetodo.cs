using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOPagoMetodo
    {
        public DTOPagoMetodo()
        {
            importe = 0;
        }

        [System.ComponentModel.Browsable(false)]
        public int pagoMetodoId { get; set; }
        [DisplayName("Metodo de pago")]
        public string descripcion { get; set; }
        [DisplayName("Importe")]
        public decimal importe { get; set; }
         [System.ComponentModel.Browsable(false)]
        public string cuentaContable { get; set; }
        public decimal comision { get; set; }
    }
}
