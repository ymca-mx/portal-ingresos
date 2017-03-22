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
    
    public partial class AlumnoSaldoDetalle
    {
        public int ConsecutivoId { get; set; }
        public string ReferenciaId { get; set; }
        public int AlumnoId { get; set; }
        public int Rubro { get; set; }
        public decimal Importe { get; set; }
        public System.DateTime FechaPago { get; set; }
        public System.DateTime FechaAplicacion { get; set; }
        public System.TimeSpan HoraAplicacion { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoDespues { get; set; }
        public bool EsReferenciado { get; set; }
        public Nullable<int> ReferenciaProcesadaId { get; set; }
    
        public virtual AlumnoSaldo AlumnoSaldo { get; set; }
        public virtual ReferenciaProcesada ReferenciaProcesada { get; set; }
    }
}