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
    
    public partial class GrupoBitacora
    {
        public int GrupoId { get; set; }
        public int EmpresaId { get; set; }
        public string Descripcion { get; set; }
        public int SucursalId { get; set; }
        public string SucursalDireccion { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public int NumeroPagos { get; set; }
        public int UsuarioId { get; set; }
        public System.DateTime FechaModificacion { get; set; }
        public System.TimeSpan HoraModificacion { get; set; }
        public int UsuarioIdModificacion { get; set; }
    }
}
