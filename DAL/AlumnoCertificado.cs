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
    
    public partial class AlumnoCertificado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AlumnoCertificado()
        {
            this.CertificadoResponsable = new HashSet<CertificadoResponsable>();
            this.AlumnoCalificacion = new HashSet<AlumnoCalificacion>();
        }
    
        public int AlumnoCertificadoId { get; set; }
        public int AlumnoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public System.DateTime Fecha { get; set; }
        public System.TimeSpan Hora { get; set; }
        public int UsuarioId { get; set; }
        public int MovimientoId { get; set; }
    
        public virtual Alumno Alumno { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CertificadoResponsable> CertificadoResponsable { get; set; }
        public virtual OfertaEducativa OfertaEducativa { get; set; }
        public virtual Usuario Usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoCalificacion> AlumnoCalificacion { get; set; }
    }
}
