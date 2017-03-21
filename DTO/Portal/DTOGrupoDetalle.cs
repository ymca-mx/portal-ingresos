using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOGrupoDetalle
    {
        public int GrupoId { get; set; }
        public int NoPagos { get; set; }
        public int CuotaId { get; set; }
        public bool EsCongelada { get; set; }
        public DTOCuota Cuota { get; set; }
        public DTOCuota CuotaI { get; set; }
        public DTOCuota CuotaB { get; set; }
        public DTOGrupo Grupo { get; set; }        
    }
}
