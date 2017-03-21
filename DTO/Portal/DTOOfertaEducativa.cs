using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOOfertaEducativa
    {
        public int OfertaEducativaId { get; set; }
        public int OfertaEducativaTipoId { get; set; }
        public string Descripcion { get; set; }
        public string Rvoe { get; set; }
        public string FechaRvoe { get; set; }
        public int EstatusId { get; set; }
        public int AlumnoDescuento { get; set; }
        public int AlumnoOfertaEducativa { get; set; }
        public int ConceptoPago { get; set; }
        public int Cuota { get; set; }
        public int Descuento { get; set; }
        public int OfertaEducativaTipo { get; set; }
        public int Pago { get; set; }
        public int ProspectoDetalle { get; set; }
        public int SucursalId { get; set; }
        public DTOOfertaEducativaTipo DTOOfertaEducativaTipo { get; set; }
    }
}
