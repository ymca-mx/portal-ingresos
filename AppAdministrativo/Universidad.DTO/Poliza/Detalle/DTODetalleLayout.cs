using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTODetalleLayout
    {
        public DTOPropiedad Movimiento { get; set; }
        public DTOPropiedad CuentaId { get; set; }
        public DTOPropiedad Referencia { get; set; }
        public DTOPropiedad TipoMovimiento { get; set; }
        public DTOPropiedad Importe { get; set; }
        public DTOPropiedad DiarioId { get; set; }
        public DTOPropiedad ImporteME { get; set; }
        public DTOPropiedad Concepto { get; set; }
    }
}
