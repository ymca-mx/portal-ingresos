using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOMes
    {
        public int MesId { get; set; }
        public string Descripcion { get; set; }
        public DTOLenguasRelacion MontoLengua { get; set; }
    }
}
