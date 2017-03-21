using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOCuota
    {
        public int CuotaId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public int PagoConceptoId { get; set; }
        public decimal Monto { get; set; }
        public string MontoS { get; set; }
        public DTOLenguasRelacion Lenguas { get; set; }
        public int CuotaLentuaId { get; set; }
        public bool EsEmpresa { get; set; }
        public DTODescuentos Descuento { get; set; }
        public string PeridoAnio { get; set; }
        public DTOPagoConcepto DTOPagoConcepto { get; set; }
    }
}
