using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTODocente
    {
        public int DocenteId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public System.DateTime FechaAlta { get; set; }
        public int UsuarioId { get; set; }

        public DTOUsuario Usuario { get; set; }
        public DTODocenteDetalle DocenteDetalle { get; set; }
    }
}
