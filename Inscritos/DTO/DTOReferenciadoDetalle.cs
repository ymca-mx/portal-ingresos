using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO
{
    public class DTOReferenciadoDetalle
    {
        public int PagoId { get; set; }
        public string ReferenciaId { get; set; }
        public System.DateTime FechaPago { get; set; }
        public decimal Importe { get; set; }
        public System.TimeSpan HoraProcesado { get; set; }
        public System.DateTime FechaProcesado { get; set; }
        public bool EsReferenciado { get; set; }
        public Nullable<int> ReferenciaProcesadaId { get; set; }
        public  DTOReferenciaProcesada ReferenciaProcesada { get; set; }
    }
}
