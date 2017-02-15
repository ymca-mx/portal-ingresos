using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO.Pagare.Impresion
{
    public class DTODocumento
    {
        public string importe { get; set; }
        public DateTime fechaGeneracion { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public string interes { get; set; }
        public string observaciones { get; set; }
        public string diaGeneracion { get; set; }
        public string mesGeneracion { get; set; }
        public string anioGeneracion { get; set; }
        public string no { get; set; }
        public string diaVencimiento { get; set; }
        public string mesVencimiento { get; set; }
        public string anioVencimiento { get; set; }
        public string importeLetra { get; set; }
        public string referenciaId { get; set; }
    }
}
