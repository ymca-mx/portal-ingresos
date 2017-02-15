using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTODetalleAlmacen
    {
        public string descripcion { get; set; }
        public string valorDefault { get; set; }
        public int longitud { get; set; }
        public bool tieneEspacio { get; set; }
    }
}
