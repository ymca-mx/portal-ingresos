using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOTipoUsuarioSubMenu
    {
        public int TipoUsuarioSubmenuId { get; set; }
        public int SubmenuId { get; set; }
        public int UsuarioTipoId { get; set; }
        public DTOSubMenu SubMenu { get; set; }
        public DTOUsuarioTipo UsuarioTipo { get; set; }
    }
}
