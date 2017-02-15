using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class DTOEstadoCuenta
    {
       public string Fecha { get; set; }
       public string Concepto { get; set; }
       public string ReferenciaId { get; set; }
       public string CargoS { get; set; }
       public decimal CargoD { get; set; }
      public string AbonoS { get; set; }
      public decimal AbonoD { get; set; }
      public string Total { get; set; }
    }
}
