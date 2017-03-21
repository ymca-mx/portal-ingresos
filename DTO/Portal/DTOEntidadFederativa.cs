using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOEntidadFederativa
    {
        public int EntidadFederativaId { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public string Abreviatura { get; set; }
        public int ProspectoDetalle { get; set; }
    }
}
