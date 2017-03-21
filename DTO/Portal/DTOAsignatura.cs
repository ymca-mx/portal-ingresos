using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAsignatura
    {
        public int OfertaEducativaId { get; set; }
        public string AsignaturaId { get; set; }
        public string Descripcion { get; set; }
        public int NivelAcademicoId { get; set; }
        public decimal Creditos { get; set; }
        public string AsignaturaSeriacionId { get; set; }
        public int EstatusId { get; set; }
        public DTOOfertaEducativa OfertaEducativa { get; set; }
    }
}
