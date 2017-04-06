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
    
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            this.AdeudoBiblioteca = new HashSet<AdeudoBiblioteca>();
            this.Alumno = new HashSet<Alumno>();
            this.AlumnoAntecedenteBitacora = new HashSet<AlumnoAntecedenteBitacora>();
            this.AlumnoDescuento = new HashSet<AlumnoDescuento>();
            this.AlumnoDescuentoPendiente = new HashSet<AlumnoDescuentoPendiente>();
            this.AlumnoExamenMedico = new HashSet<AlumnoExamenMedico>();
            this.AlumnoInscrito = new HashSet<AlumnoInscrito>();
            this.AlumnoInscritoBitacora = new HashSet<AlumnoInscritoBitacora>();
            this.AlumnoMovimiento = new HashSet<AlumnoMovimiento>();
            this.AlumnoPermitido = new HashSet<AlumnoPermitido>();
            this.AlumnoReingresoBitacora = new HashSet<AlumnoReingresoBitacora>();
            this.AlumnoRevision = new HashSet<AlumnoRevision>();
            this.BecaDeportiva = new HashSet<BecaDeportiva>();
            this.CuotaIncremento = new HashSet<CuotaIncremento>();
            this.Docente = new HashSet<Docente>();
            this.Empresa = new HashSet<Empresa>();
            this.Grupo = new HashSet<Grupo>();
            this.GrupoAlumnoConfiguracion = new HashSet<GrupoAlumnoConfiguracion>();
            this.GrupoAlumnoConfiguracionBitacora = new HashSet<GrupoAlumnoConfiguracionBitacora>();
            this.GrupoComprobante = new HashSet<GrupoComprobante>();
            this.Matricula = new HashSet<Matricula>();
            this.OfertaEducativaAntecedente = new HashSet<OfertaEducativaAntecedente>();
            this.Pagare = new HashSet<Pagare>();
            this.PagoCancelacionDetalle = new HashSet<PagoCancelacionDetalle>();
            this.PagoParcialBitacora = new HashSet<PagoParcialBitacora>();
            this.PromocionCasa = new HashSet<PromocionCasa>();
            this.ProspectoSeguimiento = new HashSet<ProspectoSeguimiento>();
            this.Recibo = new HashSet<Recibo>();
            this.ReciboDetalle = new HashSet<ReciboDetalle>();
            this.Reclasificacion = new HashSet<Reclasificacion>();
            this.ReferenciadoCabeceroBitacora = new HashSet<ReferenciadoCabeceroBitacora>();
            this.ReferenciadoDetalleBitacora = new HashSet<ReferenciadoDetalleBitacora>();
            this.ReferenciaGeneradaBitacora = new HashSet<ReferenciaGeneradaBitacora>();
            this.SistemaConfiguracion = new HashSet<SistemaConfiguracion>();
            this.BecaSEPBitacora = new HashSet<BecaSEPBitacora>();
            this.UsuarioBitacora = new HashSet<UsuarioBitacora>();
            this.UsuarioIngresosBitacora = new HashSet<UsuarioIngresosBitacora>();
            this.UsuarioPasswordRecovery = new HashSet<UsuarioPasswordRecovery>();
            this.UsuarioPermiso = new HashSet<UsuarioPermiso>();
            this.UsuarioPermiso1 = new HashSet<UsuarioPermiso>();
            this.UsuarioTipoPagoConcepto = new HashSet<UsuarioTipoPagoConcepto>();
            this.SucursalCaja = new HashSet<SucursalCaja>();
        }
    
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Password { get; set; }
        public int UsuarioTipoId { get; set; }
        public int EstatusId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdeudoBiblioteca> AdeudoBiblioteca { get; set; }
        public virtual AdeudoChocolates AdeudoChocolates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Alumno> Alumno { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoAntecedenteBitacora> AlumnoAntecedenteBitacora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoDescuento> AlumnoDescuento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoDescuentoPendiente> AlumnoDescuentoPendiente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoExamenMedico> AlumnoExamenMedico { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoInscrito> AlumnoInscrito { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoInscritoBitacora> AlumnoInscritoBitacora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoMovimiento> AlumnoMovimiento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoPermitido> AlumnoPermitido { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoReingresoBitacora> AlumnoReingresoBitacora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoRevision> AlumnoRevision { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BecaDeportiva> BecaDeportiva { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CuotaIncremento> CuotaIncremento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Docente> Docente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Empresa> Empresa { get; set; }
        public virtual Estatus Estatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Grupo> Grupo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupoAlumnoConfiguracion> GrupoAlumnoConfiguracion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupoAlumnoConfiguracionBitacora> GrupoAlumnoConfiguracionBitacora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupoComprobante> GrupoComprobante { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Matricula> Matricula { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfertaEducativaAntecedente> OfertaEducativaAntecedente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pagare> Pagare { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PagoCancelacionDetalle> PagoCancelacionDetalle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PagoParcialBitacora> PagoParcialBitacora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PromocionCasa> PromocionCasa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProspectoSeguimiento> ProspectoSeguimiento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recibo> Recibo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReciboDetalle> ReciboDetalle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclasificacion> Reclasificacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenciadoCabeceroBitacora> ReferenciadoCabeceroBitacora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenciadoDetalleBitacora> ReferenciadoDetalleBitacora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenciaGeneradaBitacora> ReferenciaGeneradaBitacora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SistemaConfiguracion> SistemaConfiguracion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BecaSEPBitacora> BecaSEPBitacora { get; set; }
        public virtual UsuarioTipo UsuarioTipo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioBitacora> UsuarioBitacora { get; set; }
        public virtual UsuarioDetalle UsuarioDetalle { get; set; }
        public virtual UsuarioImagen UsuarioImagen { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioIngresosBitacora> UsuarioIngresosBitacora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioPasswordRecovery> UsuarioPasswordRecovery { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioPermiso> UsuarioPermiso { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioPermiso> UsuarioPermiso1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioTipoPagoConcepto> UsuarioTipoPagoConcepto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SucursalCaja> SucursalCaja { get; set; }
    }
}
