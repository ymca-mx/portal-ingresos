using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOGrupoComprobante
    {
        public int GrupoComprobanteId { get; set; }
        public int GrupoId { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }
        public TimeSpan HoraRegistro { get; set; }
        public int Usuarioid { get; set; }
        public DTOGrupo Grupo { get; set; }
        public DTOGrupoComprobanteDocumento GrupoComprobanteDocumento { get; set; }
    }
}
