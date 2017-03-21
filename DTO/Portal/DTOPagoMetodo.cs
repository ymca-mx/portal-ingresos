using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPagoMetodo
    {
        public int PagoMetodoId { get; set; }
        public string Descripcion { get; set; }
        public string CuentaContable { get; set; }
        public decimal Comision { get; set; }
        public bool EsVisible { get; set; }
        public List<DTOPagoDetalle> PagoDetalle { get; set; }
    }
}
