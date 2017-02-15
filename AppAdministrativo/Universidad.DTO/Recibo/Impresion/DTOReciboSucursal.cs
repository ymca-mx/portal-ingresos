using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOReciboSucursal
    {
        public int sucursalID { get; set; }
        public string pais { get; set; }
        public string entidadFederativa { get; set; }
        public string delegacion { get; set; }
        public string cp { get; set; }
        public string colonia { get; set; }
        public string noExterior { get; set; }
        public string calle { get; set; }
        public string telefono { get; set; }
    }
}
