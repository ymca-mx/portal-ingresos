using System.Collections.Generic;

namespace DTO
{
    public class DTODocente
    {
        public int DocenteId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public System.DateTime FechaAlta { get; set; }
        public int UsuarioId { get; set; }

        public DTOUsuario Usuario { get; set; }
        public DTODocenteDetalle DocenteDetalle { get; set; }
    }

    public class DTODocenteActualizar
    {
        public int DocenteId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        List<DTODocenteActualizacion> Actualizaciones { get; set; }
    }
}
