using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOFinanciamiento
    {
        [System.ComponentModel.Browsable(false)]
        public int financiamientoId { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int alumnoId { get; set; }
        [DisplayName("Fecha de Generación")]
        public DateTime fechaGeneracion { get; set; }
        [DisplayName("Porcentaje %")]
        public decimal porcentaje { get; set; }
        public int cuotaId { get; set; }
        [DisplayName("Concepto de Pago")]
        public string concepto { get; set; }
        [DisplayName("Cuota")]
        public decimal cuota { get; set; }
        [DisplayName("Importe")]
        public decimal importe { get; set; }
        [System.ComponentModel.Browsable(false)]
        public string estatus { get; set; }
        [DisplayName("Observaciones")]
        public string observacion { get; set; }
    }
}
