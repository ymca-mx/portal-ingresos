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
    
    public partial class Institucion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Institucion()
        {
            this.AlumnoOfertaEducativa = new HashSet<AlumnoOfertaEducativa>();
            this.InstitucionOfertaEducativa = new HashSet<InstitucionOfertaEducativa>();
        }
    
        public string InstitucionId { get; set; }
        public string Nombre { get; set; }
        public string TipoSostenimiento { get; set; }
        public string TipoEducativo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoOfertaEducativa> AlumnoOfertaEducativa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstitucionOfertaEducativa> InstitucionOfertaEducativa { get; set; }
    }
}
