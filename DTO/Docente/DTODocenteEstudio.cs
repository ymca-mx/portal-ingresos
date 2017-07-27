using System;

namespace DTO
{
   public class DTODocenteEstudio
    {
        public int EstudioId { get; set; }
        public Nullable<int> DocenteId { get; set; }
        public string Institucion { get; set; }
        public Nullable<int> OfertaEducativaTipoId { get; set; }
        public string Carrera { get; set; }
        public Nullable<bool> Cedula { get; set; }
        public Nullable<bool> Titulo { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public Nullable<int> UsuarioId { get; set; }
        public Nullable<int> EstatusId { get; set; }
        public DTODocenteEstudioDocumento Documento { get; set; }
    }
    public class DTODocenteEstudioDocumento
    {
        public int EstudioId { get; set; }
        public int DocuentoTipoId { get; set; }
        public string DocumentoUrl { get; set; }
        public DTODocumentoTipo TipoDocumento { get; set; }
    }
    public class DTODocumentoTipo
    {
        public int DocumentoTipoId { get; set; }
        public string Descripcion { get; set; }
    }
}
