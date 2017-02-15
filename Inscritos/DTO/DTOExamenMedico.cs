using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOExamenMedico
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public bool ExamenMedico { get; set; }
        public string Comentario { get; set; }
        public int UsuarioId { get; set; }
    }
}
