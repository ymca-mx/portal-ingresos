using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO
{
    public class DTOReferenciaProcesada
    {
        public int ReferenciaProcesadaId { get; set; }
        public string ReferenciaId { get; set; }
        public System.DateTime FechaPago { get; set; }
        public decimal Importe { get; set; }
        public Nullable<decimal> Restante { get; set; }
        public int ReferenciaTipoId { get; set; }
        public bool SeGasto { get; set; }
        public string Observaciones { get; set; }
        public bool EsIngles { get; set; }
    }
}
