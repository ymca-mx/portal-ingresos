using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTORegistroPoliza
    {
        public string cuentaContable { get; set; }
        public decimal importe { get; set; }
        public int tipoMovimiento { get; set; }
    }
}
