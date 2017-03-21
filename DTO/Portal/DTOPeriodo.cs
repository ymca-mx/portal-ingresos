using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPeriodo
    {
        public string _FechaInicial;
        public string _FechaFinal;
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Descripcion { get; set; }
        public int Meses { get; set; }
        public int SubPeriodoId { get; set; }
        public List<DTOSubPeriodo> lstSubPeriodo { get; set; }
    }
}
