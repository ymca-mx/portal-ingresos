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
    
    public partial class PolizaCuadre
    {
        public System.DateTime FechaGeneracion { get; set; }
        public int PolizaTipoId { get; set; }
        public int PolizaSubtipoId { get; set; }
        public decimal ImporteAjuste { get; set; }
        public int AsociacionId { get; set; }
    
        public virtual Asociacion Asociacion { get; set; }
        public virtual PolizaSubtipo PolizaSubtipo { get; set; }
        public virtual PolizaTipo PolizaTipo { get; set; }
    }
}