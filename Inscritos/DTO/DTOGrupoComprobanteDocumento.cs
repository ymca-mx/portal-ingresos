using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOGrupoComprobanteDocumento
    {
        public int GrupoComprobanteId { get; set; }
        public byte[] Documento { get; set; }

        public DTOGrupoComprobante GrupoComprobante { get; set; }
    }
}
