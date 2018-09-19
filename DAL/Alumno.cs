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
    
    public partial class Alumno
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Alumno()
        {
            this.AlumnoReferenciaBitacora = new HashSet<AlumnoReferenciaBitacora>();
            this.AlumnoAntecedente = new HashSet<AlumnoAntecedente>();
            this.AlumnoAutorizacion = new HashSet<AlumnoAutorizacion>();
            this.AlumnoCalificacion = new HashSet<AlumnoCalificacion>();
            this.AlumnoCertificado = new HashSet<AlumnoCertificado>();
            this.AlumnoCuatrimestre = new HashSet<AlumnoCuatrimestre>();
            this.AlumnoDescuento = new HashSet<AlumnoDescuento>();
            this.AlumnoDetalleBitacora = new HashSet<AlumnoDetalleBitacora>();
            this.AlumnoInscrito = new HashSet<AlumnoInscrito>();
            this.AlumnoInscritoBitacora = new HashSet<AlumnoInscritoBitacora>();
            this.AlumnoMovimiento = new HashSet<AlumnoMovimiento>();
            this.AlumnoPasswordRecovery = new HashSet<AlumnoPasswordRecovery>();
            this.AlumnoPermitido = new HashSet<AlumnoPermitido>();
            this.AlumnoRevision = new HashSet<AlumnoRevision>();
            this.AlumnoTitulo = new HashSet<AlumnoTitulo>();
            this.BecaDeportiva = new HashSet<BecaDeportiva>();
            this.AlumnoAccesoBitacora = new HashSet<AlumnoAccesoBitacora>();
            this.BitacoraReinscripcionAdeudo = new HashSet<BitacoraReinscripcionAdeudo>();
            this.Financiamiento = new HashSet<Financiamiento>();
            this.GrupoAlumnoConfiguracion = new HashSet<GrupoAlumnoConfiguracion>();
            this.IdiomaGrupoAlumno = new HashSet<IdiomaGrupoAlumno>();
            this.Matricula = new HashSet<Matricula>();
            this.OfertaEducativaAntecedente = new HashSet<OfertaEducativaAntecedente>();
            this.Pagare = new HashSet<Pagare>();
            this.Pago = new HashSet<Pago>();
            this.AlumnoReingresoBitacora = new HashSet<AlumnoReingresoBitacora>();
            this.PersonaAutorizada = new HashSet<PersonaAutorizada>();
            this.PromocionCasa = new HashSet<PromocionCasa>();
            this.PromocionCasa1 = new HashSet<PromocionCasa>();
            this.Recibo = new HashSet<Recibo>();
            this.ReferenciaProcesada = new HashSet<ReferenciaProcesada>();
            this.Respuesta = new HashSet<Respuesta>();
            this.SolicitudInscripcion = new HashSet<SolicitudInscripcion>();
        }
    
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public int UsuarioId { get; set; }
        public string MatriculaId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public int EstatusId { get; set; }
    
        public virtual Adeudo Adeudo { get; set; }
        public virtual AdeudoBiblioteca AdeudoBiblioteca { get; set; }
        public virtual AdeudoChocolates AdeudoChocolates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoReferenciaBitacora> AlumnoReferenciaBitacora { get; set; }
        public virtual Estatus Estatus { get; set; }
        public virtual Usuario Usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoAntecedente> AlumnoAntecedente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoAutorizacion> AlumnoAutorizacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoCalificacion> AlumnoCalificacion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoCertificado> AlumnoCertificado { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoCuatrimestre> AlumnoCuatrimestre { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoDescuento> AlumnoDescuento { get; set; }
        public virtual AlumnoDetalle AlumnoDetalle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoDetalleBitacora> AlumnoDetalleBitacora { get; set; }
        public virtual AlumnoImagen AlumnoImagen { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoInscrito> AlumnoInscrito { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoInscritoBitacora> AlumnoInscritoBitacora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoMovimiento> AlumnoMovimiento { get; set; }
        public virtual AlumnoPassword AlumnoPassword { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoPasswordRecovery> AlumnoPasswordRecovery { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoPermitido> AlumnoPermitido { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoRevision> AlumnoRevision { get; set; }
        public virtual AlumnoSaldo AlumnoSaldo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoTitulo> AlumnoTitulo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BecaDeportiva> BecaDeportiva { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoAccesoBitacora> AlumnoAccesoBitacora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BitacoraReinscripcionAdeudo> BitacoraReinscripcionAdeudo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Financiamiento> Financiamiento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupoAlumnoConfiguracion> GrupoAlumnoConfiguracion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdiomaGrupoAlumno> IdiomaGrupoAlumno { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Matricula> Matricula { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfertaEducativaAntecedente> OfertaEducativaAntecedente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pagare> Pagare { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pago> Pago { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlumnoReingresoBitacora> AlumnoReingresoBitacora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaAutorizada> PersonaAutorizada { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PromocionCasa> PromocionCasa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PromocionCasa> PromocionCasa1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recibo> Recibo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenciaProcesada> ReferenciaProcesada { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Respuesta> Respuesta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SolicitudInscripcion> SolicitudInscripcion { get; set; }
    }
}
