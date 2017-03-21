using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOLenguasRelacion
    {
        public int CuotaId { get; set; }
        public string Descripcion { get; set; }
        public int DiaInicial { get; set; }
        public int DiaFinal { get; set; }
        public DTOCuota Cuota { get; set; }
    }
}
