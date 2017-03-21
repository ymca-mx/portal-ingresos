using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOSubPeriodo
    {
        public int SubperiodoId { get; set; }
        public int PeriodoId { get; set; }
        public int MesId { get; set; }
        public DTOMes Mes { get; set; }
    }
}
