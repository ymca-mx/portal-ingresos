using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOUsuarioTipo
    {
        public int usuarioTipoId { get; set; }
        public string descripcion { get; set; }
        public List<DTOTipoUsuarioSubMenu> TipoUsuarioSubmenu { get; set; }
    }
}
