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
    
    public partial class AlumnoDescuentoPendiente
    {
        public int AlumnoDescuentoId { get; set; }
        public System.DateTime FechaPendiente { get; set; }
        public System.TimeSpan HoraPendiente { get; set; }
        public int UsuarioId { get; set; }
        public System.DateTime FechaAplicacion { get; set; }
        public System.TimeSpan HoraAplicacion { get; set; }
        public int UsuarioIdAplicacion { get; set; }
        public int EstatusId { get; set; }
    
        public virtual AlumnoDescuento AlumnoDescuento { get; set; }
        public virtual Estatus Estatus { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
