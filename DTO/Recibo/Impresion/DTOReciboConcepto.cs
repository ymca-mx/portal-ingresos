using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOReciboConcepto
    {
        public DTOReciboConcepto()
        {
            cantidad = "";
            importe = "";
        }

        public string cantidad { get; set; }
        public string descripcion { get; set; }
        public string importe { get; set; }
    }
}
