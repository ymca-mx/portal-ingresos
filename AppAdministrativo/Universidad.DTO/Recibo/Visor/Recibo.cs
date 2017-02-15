using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class Recibo
    {
        [DisplayName("No. Recibo")]
        public int reciboId { get; set; }
        [DisplayName("Fecha de generación")]
        public DateTime fechaGeneracion { get; set; }
        public int sucursalCajaId { get; set; }
         [DisplayName("Sucursal | Caja")]
        public string sucursal { get; set; }
         [DisplayName("Importe")]
        public decimal importe { get; set;}
         [DisplayName("Alumno")]
        public string alumno { get; set; }
         [DisplayName("Oferta Educativa")]
        public string ofertaEducativa { get; set; }
         [DisplayName("¿Quién cobro?")]
        public string usuario { get; set; }
         [DisplayName("Estatus")]
        public string estatus { get; set; }
        [System.ComponentModel.Browsable(false)]
        public string observacionesCancelacion { get; set; }
        public bool esCancelable { get; set; }
    }
}
