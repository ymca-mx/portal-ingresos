using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPais
    {
        public int PaisId { get; set; }
        public string Iso { get; set; }
        public string Descripcion { get; set; }        
        public int AlumnoDetalle { get; set; }
        public int ProspectoDetalle { get; set; }
    }
}
