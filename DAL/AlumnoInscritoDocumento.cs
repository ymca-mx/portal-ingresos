//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class AlumnoInscritoDocumento
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