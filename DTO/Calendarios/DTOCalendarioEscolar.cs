using System.Collections.Generic;

namespace DTO
{
    public class DTOCalendarioEscolar
    {
        int CalendarioEscolarId { get; set; }
        string Nombre { get; set; }
        string Direccion { get; set; }
        string FechaAlta { get; set; }
        string HoraAlta { get; set; }
        int UsuarioId { get; set; }
        int EstatusId { get; set; }
        List<DTOOFertaCalendario> OfertasCalendario { get; set; }
    }
}
