using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOSubMenu
    {
        public int SubmenuId { get; set; }
        public int MenuId { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
    }
}
