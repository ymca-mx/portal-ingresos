using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTODocenteDetalle
    {
        public int DocenteId { get; set; }
        public int GeneroId { get; set; }
        public int EstadoCivilId { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public string RFC { get; set; }
        public string Email { get; set; }
        public string TelefonoCelular { get; set; }
        public string TelefonoCasa { get; set; }
    }
}
