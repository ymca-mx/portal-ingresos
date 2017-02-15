using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOOfertaEducativaTipo
    {
        public int OfertaEducativaTipoId { get; set; }
        public string Descripcion { get; set; }
        public int OfertaEducativa { get; set; }
        public int ProspectoDetalle { get; set; }
        public List<DTOOfertaEducativa1> Ofertas { get; set; }
    }
}
