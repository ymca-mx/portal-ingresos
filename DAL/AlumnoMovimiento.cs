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
    
    public partial class AlumnoMovimiento
    {
        public int AlumnoMovimientoId { get; set; }
        public Nullable<int> AlumnoId { get; set; }
        public Nullable<int> OfertaEducativaId { get; set; }
        public Nullable<int> Anio { get; set; }
        public Nullable<int> PeriodoId { get; set; }
        public Nullable<int> TipoMovimientoId { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<System.TimeSpan> Hora { get; set; }
        public Nullable<int> UsuarioId { get; set; }
        public Nullable<int> EstatusId { get; set; }
    
        public virtual Alumno Alumno { get; set; }
        public virtual Estatus Estatus { get; set; }
        public virtual OfertaEducativa OfertaEducativa { get; set; }
        public virtual TipoMovimiento TipoMovimiento { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual AlumnoMovimientoBaja AlumnoMovimientoBaja { get; set; }
        public virtual AlumnoMovimientoCarrera AlumnoMovimientoCarrera { get; set; }
    }
}
