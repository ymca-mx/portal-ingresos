using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOSucursal
    {
        public int SucursalId { get; set; }
        public string DescripcionId { get; set; }
        public string Serie { get; set; }
        public DTOSucursalDetalle Detalle { get; set; }
    }
}
