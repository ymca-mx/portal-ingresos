using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPagoDetalle
    {
        public int PagoId { get; set; }
        public int SucursalCajaId { get; set; }
        public int ReciboId { get; set; }
        public int PagoMetodoId { get; set; }
        public decimal Monto { get; set; }
        public int ConceptoPagoId { get; set; }
        public DTOPagoMetodo PagoMetodo { get; set; }
        public DTOPagoParcial PagoParcial { get; set; }
        public DTOPagoConcepto PagoConcepto { get; set; }
    }
}
