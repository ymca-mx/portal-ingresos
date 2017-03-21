using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPersonaAutorizada
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Celular { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int ParentescoId { get; set; }
        public int Alumno { get; set; }
        public int Parentesco { get; set; }
        public Boolean Autoriza { get; set; }
    }
}
