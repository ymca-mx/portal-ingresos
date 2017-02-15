using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO.Usuario
{
    public class DTOUsuarioImagen
    {
        public byte[] imagen { get; set; }
        public string imagenBase64 { get; set; }
        public string extensionImagen { get; set; }
        public string nombre { get; set; }
    }
}
