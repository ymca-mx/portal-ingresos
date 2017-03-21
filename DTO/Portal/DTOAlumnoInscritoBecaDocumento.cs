using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAlumnoInscritoBecaDocumento
    {
        public int AlumnoInscritoDocumentoId { get; set; }
        public int AlumnoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public bool EsComite { get; set; }
        public int TipoDocumentoId { get; set; }
        public byte[] ArchivoBeca { get; set; }
        public byte[] ArchivoComite { get; set; }
    }
}
