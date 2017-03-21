using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOUsuario
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Password { get; set; }
        public int UsuarioTipoId { get; set; }
        public int EstatusId { get; set; }
    }

    public class DTOUsuarioDetalle
    {
        public int UsuarioId { get; set; }
        public int GeneroId { get; set; }
        public string Celular { get; set; }
        public string Telefono { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public string Email { get; set; }
        public string Observaciones { get; set; }
    }
}
namespace DTO.Usuario
{
    public class DTOUsuario
    {
        public int usuarioId { get; set; }
        public int usuarioTipoId { get; set; }
    }
}
namespace DTO.Varios
{
    public class DTOEstatus
    {
        public int estatusId { get; set; }
        public string descripcion { get; set; }
    }
}
