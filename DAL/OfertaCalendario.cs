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
    
    public partial class OfertaCalendario
    {
        public int OfertaCalendarioId { get; set; }
        public int CalendarioEscolarId { get; set; }
        public int OfertaEducativaId { get; set; }
    
        public virtual CalendarioEscolar CalendarioEscolar { get; set; }
        public virtual OfertaEducativa OfertaEducativa { get; set; }
    }
}
