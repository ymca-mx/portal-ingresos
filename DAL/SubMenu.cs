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
    
    public partial class SubMenu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubMenu()
        {
            this.TipoUsuarioSubmenu = new HashSet<TipoUsuarioSubmenu>();
        }
    
        public int SubmenuId { get; set; }
        public int MenuId { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Incono { get; set; }
    
        public virtual Menu Menu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TipoUsuarioSubmenu> TipoUsuarioSubmenu { get; set; }
    }
}
