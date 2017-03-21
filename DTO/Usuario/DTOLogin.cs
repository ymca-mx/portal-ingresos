using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOLogin
    {
        public int usuarioId { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public int sucursalCajaId { get; set; }
        public Perfil.Acceso TipoAcceso { get; set; }
    }
}
