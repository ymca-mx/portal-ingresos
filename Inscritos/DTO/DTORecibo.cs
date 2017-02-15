using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTORecibo
    {
        public int ReciboId { get; set; }
        public int SucursalCajaId { get; set; }
        public string Serie { get; set; }
        public string Observaciones { get; set; }
        public decimal Importe { get; set; }
        public int AlumnoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public System.DateTime FechaGeneracion { get; set; }
        public System.TimeSpan HoraGeneracion { get; set; }
        public int UsuarioId { get; set; }
        public int EstatusId { get; set; }
    }
}
