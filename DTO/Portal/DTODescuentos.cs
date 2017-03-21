using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTODescuentos
    {
        public int DescuentoId { get; set; }
        public int PagoConceptoId { get; set; }
        public int DescuentoTipoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public string Descripcion { get; set; }
        public decimal MontoMinimo { get; set; }
        public decimal MontoMaximo { get; set; }
        public System.DateTime FechaInicial { get; set; }
        public System.DateTime FechaFinal { get; set; }
        public string CuentaContable { get; set; }
    
    }
}
