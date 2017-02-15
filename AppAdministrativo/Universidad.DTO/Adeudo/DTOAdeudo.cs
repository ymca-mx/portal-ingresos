using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOAdeudo
    {
        public List<DTO.DTOPagare> Pagare { get; set; }
        public List<DTO.DTOFinanciamiento> Financimiento { get; set; }
    }
}
