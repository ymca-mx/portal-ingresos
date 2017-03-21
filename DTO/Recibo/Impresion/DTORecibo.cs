using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTORecibo
    {
        public List<DTOReciboDatos> DatosGenerales { get; set; }
        public List<DTOReciboSucursal> Sucursal { get; set; }
        public List<DTOReciboConcepto> Conceptos { get; set; }
    }
}
