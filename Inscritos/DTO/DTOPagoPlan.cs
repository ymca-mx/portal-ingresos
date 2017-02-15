using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPagoPlan
    {
        public int PagoPlanId { get; set; }
        public string PlanPago { get; set; }
        public string Descripcion { get; set; }
        public int Pagos { get; set; }
        public int EstatusId { get; set; }
    }
}
