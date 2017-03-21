using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPagoConcepto
    {
        public int PagoConceptoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public string Descripcion { get; set; }
        public string CuentaContable { get; set; }
        public int EstatusId { get; set; }
        public bool EsMultireferencia { get; set; }
        public Boolean EsVairable { get; set; }
    }
}
