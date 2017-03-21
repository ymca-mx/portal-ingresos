using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTODatos
    {
        public int alumnoId { get; set; }
        public string nombre { get; set; }
        public string password { get; set; }
        public string descripcion { get; set; }
        public string nombreSolo { get; set; }
        public int estatusId { get; set; }

        public int sexoId { get; set; }

        public DTODatos() { }
    }
}
