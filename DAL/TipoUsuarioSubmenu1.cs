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
    
    public partial class TipoUsuarioSubmenu1
    {
        public int TipoUsuarioSubmenuId { get; set; }
        public int SubmenuId { get; set; }
        public int UsuarioTipoId { get; set; }
    
        public virtual UsuarioTipo UsuarioTipo { get; set; }
        public virtual SubMenu1 SubMenu1 { get; set; }
    }
}