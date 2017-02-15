using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Alumno.Beca
{
    public class DTOAlumnoBeca
    {
        public int alumnoId { get; set; }
        public int anio { get; set; }
        public int periodoId { get; set; }
        public decimal porcentajeBeca { get; set; }
        public decimal porcentajeInscripcion { get; set; }
        public bool esSEP { get; set; }
        public int ofertaEducativaId { get; set; }
        public int usuarioId { get; set; }
        public bool esComite { get; set; }
        public bool esEmpresa { get; set; }
        public string fecha { get; set; }
        public bool genera { get; set; }
    }
}
namespace DTO
{
    public class ReciboDatos
    {
        public int? reciboId { get; set; }
        public int? sucursalCajaId { get; set; }
        public decimal importe { get; set; }
    }
}