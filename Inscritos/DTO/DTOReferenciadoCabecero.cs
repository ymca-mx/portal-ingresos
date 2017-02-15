using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public  class DTOReferenciadoCabecero
    {
        public int PagoId { get; set; }
        public string ReferenciaId { get; set; }
        public System.DateTime FechaAplicacion { get; set; }
        public decimal ImporteRestante { get; set; }
        public decimal ImporteTotal { get; set; }
        public int EstatusId { get; set; }
        public List<DTOReferenciadoDetalle> LlstReferenciadoDetalle { get; set; }
    }
}
