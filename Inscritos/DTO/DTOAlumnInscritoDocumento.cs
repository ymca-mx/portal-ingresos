using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAlumnInscritoDocumento
    {
        public int AlumnoInscritoDocumentoId { get; set; }
        public int AlumnoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public int TipoDocumento { get; set; }
        public byte[] Archivo { get; set; }
        public Nullable<int> UsuarioDocumento { get; set; }
        public Nullable<System.DateTime> FechaDocumento { get; set; }
    }
}
