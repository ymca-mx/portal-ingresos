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
    
    public partial class SalonAsignacion
    {
        public int SalonAsignacionId { get; set; }
        public Nullable<int> MateriaAperturaId { get; set; }
        public Nullable<int> DiaId { get; set; }
        public Nullable<int> HoraInicioId { get; set; }
        public Nullable<int> HoraFinId { get; set; }
        public Nullable<int> SalonId { get; set; }
        public Nullable<int> Anio { get; set; }
        public Nullable<int> PeriodoId { get; set; }
        public Nullable<System.DateTime> FechaAsignacion { get; set; }
        public Nullable<System.TimeSpan> HoraAsignacion { get; set; }
        public Nullable<int> UsuarioId { get; set; }
    }
}