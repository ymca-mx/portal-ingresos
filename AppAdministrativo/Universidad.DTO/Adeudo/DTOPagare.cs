using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOPagare
    {
        [DisplayName("ID Pagaré")]
        public int pagareId { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int alumnoId { get; set; }
        [DisplayName("Fecha de Generación")]
        public DateTime fechaGeneracion { get; set; }
        [DisplayName("Fecha de Vencimiento")]
        public DateTime fechaVencimiento { get; set; }
        [DisplayName("Importe")]
        public decimal importe { get; set; }
        [DisplayName("Interes %")]
        public decimal interes { get; set; }
        [System.ComponentModel.Browsable(false)]
        public string estatus { get; set; }
        [DisplayName("Observaciones")]
        public string observacion { get; set; }


        [System.ComponentModel.Browsable(false)]
        public byte[] documento { get; set; }
    }
}
