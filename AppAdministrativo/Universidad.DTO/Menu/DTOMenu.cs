using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
public    class DTOMenu
    {
        public int MenuId { get; set; }
        public string Descripcion { get; set; }
        public virtual List<DTOSubMenu> SubMenu { get; set; }
    }
}
