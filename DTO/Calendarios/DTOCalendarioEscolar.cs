using System.Collections.Generic;

namespace DTO
{
    public class DTOCalendarioEscolar
    {
        public int CalendarioEscolarId { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string FechaAlta { get; set; }
        public string HoraAlta { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public int EstatusId { get; set; }
        public List<DTOOFertaCalendario> OfertasCalendario { get; set; }
    }
}
