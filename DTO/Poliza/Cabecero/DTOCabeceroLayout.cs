using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOCabeceroLayout
    {
        public DTOPropiedad Poliza { get; set; }
        public DTOPropiedad Fecha { get; set; }
        public DTOPropiedad TipoPoliza { get; set; }
        public DTOPropiedad Folio { get; set; }
        public DTOPropiedad Clase { get; set; }
        public DTOPropiedad DiarioId { get; set; }
        public DTOPropiedad Concepto { get; set; }
        public DTOPropiedad SistOrig { get; set; }
        public DTOPropiedad Impresa { get; set; }
    }
}
