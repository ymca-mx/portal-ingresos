using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO.Pagare.Visor
{
    public class DTOPagare
    {
        [DisplayName("No. Pagaré")]
        public int pagareId { get; set; }
        [DisplayName("Alumno")]
        public string alumno { get; set; }
        [DisplayName("Fecha de generación")]
        public DateTime FechaGeneracion { get; set; }
        [DisplayName("Importe")]
        public decimal importe { get; set; }
        [DisplayName("Interés")]
        public decimal interes { get; set; }
        [DisplayName("Referencia de pago")]
        public string referenciaId { get; set; }
        [DisplayName("Fecha de vencimiento")]
        public DateTime FechaVencimiento { get; set; }
        [DisplayName("¿Quién lo genero?")]
        public string usuario { get; set; }
        [DisplayName("Estatus")]
        public string estatus { get; set; }
    }
}
