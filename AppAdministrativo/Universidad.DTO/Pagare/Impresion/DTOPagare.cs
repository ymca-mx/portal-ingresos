using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO.Pagare.Impresion
{
    public class DTOPagare
    {
        public List<DTO.Pagare.Impresion.DTODocumento> Documento { get; set; }
        public List<DTO.Pagare.Impresion.DTOAcreedor> Acreedor { get; set; }
        public List<DTO.Pagare.Impresion.DTODeudor> Deudor { get; set; }
        public List<DTO.Pagare.Impresion.DTOBanco> Banco { get; set; }
    }
}
