﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class UniversidadEntities : DbContext
    {
        public UniversidadEntities()
            : base("name=UniversidadEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AlumnoPermitido20162> AlumnoPermitido20162 { get; set; }
        public virtual DbSet<Adeudo> Adeudo { get; set; }
        public virtual DbSet<AdeudoBiblioteca> AdeudoBiblioteca { get; set; }
        public virtual DbSet<AdeudoChocolates> AdeudoChocolates { get; set; }
        public virtual DbSet<Alumno> Alumno { get; set; }
        public virtual DbSet<AlumnoAccesoBitacora> AlumnoAccesoBitacora { get; set; }
        public virtual DbSet<AlumnoAntecedente> AlumnoAntecedente { get; set; }
        public virtual DbSet<AlumnoAntecedenteBitacora> AlumnoAntecedenteBitacora { get; set; }
        public virtual DbSet<AlumnoAutorizacion> AlumnoAutorizacion { get; set; }
        public virtual DbSet<AlumnoBajaDocumento> AlumnoBajaDocumento { get; set; }
        public virtual DbSet<AlumnoBitacora> AlumnoBitacora { get; set; }
        public virtual DbSet<AlumnoCuatrimestre> AlumnoCuatrimestre { get; set; }
        public virtual DbSet<AlumnoCuatrimestreBitacora> AlumnoCuatrimestreBitacora { get; set; }
        public virtual DbSet<AlumnoDescuento> AlumnoDescuento { get; set; }
        public virtual DbSet<AlumnoDescuentoBitacora> AlumnoDescuentoBitacora { get; set; }
        public virtual DbSet<AlumnoDescuentoDocumento> AlumnoDescuentoDocumento { get; set; }
        public virtual DbSet<AlumnoDescuentoPendiente> AlumnoDescuentoPendiente { get; set; }
        public virtual DbSet<AlumnoDetalle> AlumnoDetalle { get; set; }
        public virtual DbSet<AlumnoDetalleAlumno> AlumnoDetalleAlumno { get; set; }
        public virtual DbSet<AlumnoDetalleBitacora> AlumnoDetalleBitacora { get; set; }
        public virtual DbSet<AlumnoExamenMedico> AlumnoExamenMedico { get; set; }
        public virtual DbSet<AlumnoGrupoCuota> AlumnoGrupoCuota { get; set; }
        public virtual DbSet<AlumnoImagen> AlumnoImagen { get; set; }
        public virtual DbSet<AlumnoImagenDetalle> AlumnoImagenDetalle { get; set; }
        public virtual DbSet<AlumnoInscrito> AlumnoInscrito { get; set; }
        public virtual DbSet<AlumnoInscritoBeca> AlumnoInscritoBeca { get; set; }
        public virtual DbSet<AlumnoInscritoBecaDeportiva> AlumnoInscritoBecaDeportiva { get; set; }
        public virtual DbSet<AlumnoInscritoBecaDocumento> AlumnoInscritoBecaDocumento { get; set; }
        public virtual DbSet<AlumnoInscritoBitacora> AlumnoInscritoBitacora { get; set; }
        public virtual DbSet<AlumnoInscritoDetalle> AlumnoInscritoDetalle { get; set; }
        public virtual DbSet<AlumnoInscritoDetalleBitacora> AlumnoInscritoDetalleBitacora { get; set; }
        public virtual DbSet<AlumnoInscritoDocumento> AlumnoInscritoDocumento { get; set; }
        public virtual DbSet<AlumnoMovimiento> AlumnoMovimiento { get; set; }
        public virtual DbSet<AlumnoMovimientoBaja> AlumnoMovimientoBaja { get; set; }
        public virtual DbSet<AlumnoMovimientoCarrera> AlumnoMovimientoCarrera { get; set; }
        public virtual DbSet<AlumnoPassword> AlumnoPassword { get; set; }
        public virtual DbSet<AlumnoPasswordRecovery> AlumnoPasswordRecovery { get; set; }
        public virtual DbSet<AlumnoPermitido> AlumnoPermitido { get; set; }
        public virtual DbSet<AlumnoReferenciaBitacora> AlumnoReferenciaBitacora { get; set; }
        public virtual DbSet<AlumnoReingresoBitacora> AlumnoReingresoBitacora { get; set; }
        public virtual DbSet<AlumnoRevision> AlumnoRevision { get; set; }
        public virtual DbSet<AlumnoSaldo> AlumnoSaldo { get; set; }
        public virtual DbSet<AlumnoSaldoDetalle> AlumnoSaldoDetalle { get; set; }
        public virtual DbSet<AlumnoSaldoGasto> AlumnoSaldoGasto { get; set; }
        public virtual DbSet<AlumnosEmpresa> AlumnosEmpresa { get; set; }
        public virtual DbSet<AntecedenteTipo> AntecedenteTipo { get; set; }
        public virtual DbSet<AreaAcademica> AreaAcademica { get; set; }
        public virtual DbSet<Asignatura> Asignatura { get; set; }
        public virtual DbSet<Asociacion> Asociacion { get; set; }
        public virtual DbSet<AsociacionDetalle> AsociacionDetalle { get; set; }
        public virtual DbSet<BajaMotivo> BajaMotivo { get; set; }
        public virtual DbSet<BancoTerminal> BancoTerminal { get; set; }
        public virtual DbSet<BecaDeportiva> BecaDeportiva { get; set; }
        public virtual DbSet<BitacoraReinscripcionAdeudo> BitacoraReinscripcionAdeudo { get; set; }
        public virtual DbSet<Caja> Caja { get; set; }
        public virtual DbSet<CajaConfiguracion> CajaConfiguracion { get; set; }
        public virtual DbSet<CalendarioEscolar> CalendarioEscolar { get; set; }
        public virtual DbSet<CalificacionesAntecedente> CalificacionesAntecedente { get; set; }
        public virtual DbSet<CuentaMail> CuentaMail { get; set; }
        public virtual DbSet<Cuota> Cuota { get; set; }
        public virtual DbSet<CuotaIncremento> CuotaIncremento { get; set; }
        public virtual DbSet<DatosFiscales> DatosFiscales { get; set; }
        public virtual DbSet<Defectos> Defectos { get; set; }
        public virtual DbSet<Descuento> Descuento { get; set; }
        public virtual DbSet<DescuentoTipo> DescuentoTipo { get; set; }
        public virtual DbSet<Dia> Dia { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<EmpresaDetalle> EmpresaDetalle { get; set; }
        public virtual DbSet<EntidadFederativa> EntidadFederativa { get; set; }
        public virtual DbSet<EspecialidadMaestriaRelacion> EspecialidadMaestriaRelacion { get; set; }
        public virtual DbSet<EstadoCivil> EstadoCivil { get; set; }
        public virtual DbSet<Estatus> Estatus { get; set; }
        public virtual DbSet<Financiamiento> Financiamiento { get; set; }
        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<GrupoAlumno> GrupoAlumno { get; set; }
        public virtual DbSet<GrupoAlumnoBitacora> GrupoAlumnoBitacora { get; set; }
        public virtual DbSet<GrupoAlumnoConfiguracion> GrupoAlumnoConfiguracion { get; set; }
        public virtual DbSet<GrupoAlumnoConfiguracionBitacora> GrupoAlumnoConfiguracionBitacora { get; set; }
        public virtual DbSet<GrupoBitacora> GrupoBitacora { get; set; }
        public virtual DbSet<GrupoComprobante> GrupoComprobante { get; set; }
        public virtual DbSet<GrupoComprobanteDocumento> GrupoComprobanteDocumento { get; set; }
        public virtual DbSet<GrupoDetalle> GrupoDetalle { get; set; }
        public virtual DbSet<IdiomaGrupo> IdiomaGrupo { get; set; }
        public virtual DbSet<IdiomaGrupoAlumno> IdiomaGrupoAlumno { get; set; }
        public virtual DbSet<IngresosPermiso> IngresosPermiso { get; set; }
        public virtual DbSet<LenguasRelacion> LenguasRelacion { get; set; }
        public virtual DbSet<Matricula> Matricula { get; set; }
        public virtual DbSet<MedioDifusion> MedioDifusion { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Mes> Mes { get; set; }
        public virtual DbSet<Municipio> Municipio { get; set; }
        public virtual DbSet<NivelAcademico> NivelAcademico { get; set; }
        public virtual DbSet<OfertaCalendario> OfertaCalendario { get; set; }
        public virtual DbSet<OfertaEducativa> OfertaEducativa { get; set; }
        public virtual DbSet<OfertaEducativaAntecedente> OfertaEducativaAntecedente { get; set; }
        public virtual DbSet<OfertaEducativaPlan> OfertaEducativaPlan { get; set; }
        public virtual DbSet<OfertaEducativaRequerimiento> OfertaEducativaRequerimiento { get; set; }
        public virtual DbSet<OfertaEducativaTipo> OfertaEducativaTipo { get; set; }
        public virtual DbSet<Pagare> Pagare { get; set; }
        public virtual DbSet<PagareDocumento> PagareDocumento { get; set; }
        public virtual DbSet<Pago> Pago { get; set; }
        public virtual DbSet<PagoBitacora> PagoBitacora { get; set; }
        public virtual DbSet<PagoCancelacion> PagoCancelacion { get; set; }
        public virtual DbSet<PagoCancelacionDetalle> PagoCancelacionDetalle { get; set; }
        public virtual DbSet<PagoCancelacionSolicitud> PagoCancelacionSolicitud { get; set; }
        public virtual DbSet<PagoConcepto> PagoConcepto { get; set; }
        public virtual DbSet<PagoConceptoCarrera> PagoConceptoCarrera { get; set; }
        public virtual DbSet<PagoConceptoMaestro> PagoConceptoMaestro { get; set; }
        public virtual DbSet<PagoDescripcion> PagoDescripcion { get; set; }
        public virtual DbSet<PagoDescuento> PagoDescuento { get; set; }
        public virtual DbSet<PagoDetalle> PagoDetalle { get; set; }
        public virtual DbSet<PagoMetodo> PagoMetodo { get; set; }
        public virtual DbSet<PagoPagare> PagoPagare { get; set; }
        public virtual DbSet<PagoParcial> PagoParcial { get; set; }
        public virtual DbSet<PagoParcialBitacora> PagoParcialBitacora { get; set; }
        public virtual DbSet<PagoPlan> PagoPlan { get; set; }
        public virtual DbSet<PagoRecargo> PagoRecargo { get; set; }
        public virtual DbSet<PagoTipo> PagoTipo { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Parentesco> Parentesco { get; set; }
        public virtual DbSet<Periodo> Periodo { get; set; }
        public virtual DbSet<PeriodoAnticipado> PeriodoAnticipado { get; set; }
        public virtual DbSet<PersonaAutorizada> PersonaAutorizada { get; set; }
        public virtual DbSet<PersonaAutorizadaBitacora> PersonaAutorizadaBitacora { get; set; }
        public virtual DbSet<PolizaAjuste> PolizaAjuste { get; set; }
        public virtual DbSet<PolizaCabeceroLayout> PolizaCabeceroLayout { get; set; }
        public virtual DbSet<PolizaConfiguracion> PolizaConfiguracion { get; set; }
        public virtual DbSet<PolizaConsecutivo> PolizaConsecutivo { get; set; }
        public virtual DbSet<PolizaCuadre> PolizaCuadre { get; set; }
        public virtual DbSet<PolizaDetalleLayout> PolizaDetalleLayout { get; set; }
        public virtual DbSet<PolizaMovimiento> PolizaMovimiento { get; set; }
        public virtual DbSet<PolizaNumeracion> PolizaNumeracion { get; set; }
        public virtual DbSet<PolizaSubtipo> PolizaSubtipo { get; set; }
        public virtual DbSet<PolizaTipo> PolizaTipo { get; set; }
        public virtual DbSet<Pregunta> Pregunta { get; set; }
        public virtual DbSet<PreguntaCompuesta> PreguntaCompuesta { get; set; }
        public virtual DbSet<PreguntaConfiguracion> PreguntaConfiguracion { get; set; }
        public virtual DbSet<PreguntaTipo> PreguntaTipo { get; set; }
        public virtual DbSet<PreguntaTipoValores> PreguntaTipoValores { get; set; }
        public virtual DbSet<PreguntaValor> PreguntaValor { get; set; }
        public virtual DbSet<PromocionCasa> PromocionCasa { get; set; }
        public virtual DbSet<Prospecto> Prospecto { get; set; }
        public virtual DbSet<ProspectoBitacora> ProspectoBitacora { get; set; }
        public virtual DbSet<ProspectoDetalle> ProspectoDetalle { get; set; }
        public virtual DbSet<ProspectoDetalleBitacora> ProspectoDetalleBitacora { get; set; }
        public virtual DbSet<ProspectoSeguimiento> ProspectoSeguimiento { get; set; }
        public virtual DbSet<Recibo> Recibo { get; set; }
        public virtual DbSet<ReciboArchivo> ReciboArchivo { get; set; }
        public virtual DbSet<ReciboDetalle> ReciboDetalle { get; set; }
        public virtual DbSet<ReciboMSI> ReciboMSI { get; set; }
        public virtual DbSet<Reclasificacion> Reclasificacion { get; set; }
        public virtual DbSet<ReclasificacionTipo> ReclasificacionTipo { get; set; }
        public virtual DbSet<ReferenciadoArchivoBitacora> ReferenciadoArchivoBitacora { get; set; }
        public virtual DbSet<ReferenciadoBitacora> ReferenciadoBitacora { get; set; }
        public virtual DbSet<ReferenciadoCabecero> ReferenciadoCabecero { get; set; }
        public virtual DbSet<ReferenciadoCabeceroBitacora> ReferenciadoCabeceroBitacora { get; set; }
        public virtual DbSet<ReferenciadoDetalle> ReferenciadoDetalle { get; set; }
        public virtual DbSet<ReferenciadoDetalleBitacora> ReferenciadoDetalleBitacora { get; set; }
        public virtual DbSet<ReferenciadoLayout> ReferenciadoLayout { get; set; }
        public virtual DbSet<ReferenciaEspecial> ReferenciaEspecial { get; set; }
        public virtual DbSet<ReferenciaGeneradaBitacora> ReferenciaGeneradaBitacora { get; set; }
        public virtual DbSet<ReferenciaProcesada> ReferenciaProcesada { get; set; }
        public virtual DbSet<ReferenciaTipo> ReferenciaTipo { get; set; }
        public virtual DbSet<Respuesta> Respuesta { get; set; }
        public virtual DbSet<SistemaConfiguracion> SistemaConfiguracion { get; set; }
        public virtual DbSet<SubMenu> SubMenu { get; set; }
        public virtual DbSet<Subperiodo> Subperiodo { get; set; }
        public virtual DbSet<Sucursal> Sucursal { get; set; }
        public virtual DbSet<SucursalAnuncio> SucursalAnuncio { get; set; }
        public virtual DbSet<SucursalCaja> SucursalCaja { get; set; }
        public virtual DbSet<SucursalDetalle> SucursalDetalle { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<TipoMovimiento> TipoMovimiento { get; set; }
        public virtual DbSet<TipoMovimientoCRUD> TipoMovimientoCRUD { get; set; }
        public virtual DbSet<TipoUsuarioSubmenu> TipoUsuarioSubmenu { get; set; }
        public virtual DbSet<Turno> Turno { get; set; }
        public virtual DbSet<UniversidadValidacion> UniversidadValidacion { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioBitacora> UsuarioBitacora { get; set; }
        public virtual DbSet<UsuarioDetalle> UsuarioDetalle { get; set; }
        public virtual DbSet<UsuarioImagen> UsuarioImagen { get; set; }
        public virtual DbSet<UsuarioImagenDetalle> UsuarioImagenDetalle { get; set; }
        public virtual DbSet<UsuarioIngresosBitacora> UsuarioIngresosBitacora { get; set; }
        public virtual DbSet<UsuarioOrigen> UsuarioOrigen { get; set; }
        public virtual DbSet<UsuarioPasswordRecovery> UsuarioPasswordRecovery { get; set; }
        public virtual DbSet<UsuarioPermiso> UsuarioPermiso { get; set; }
        public virtual DbSet<UsuarioTipo> UsuarioTipo { get; set; }
        public virtual DbSet<UsuarioTipoPagoConcepto> UsuarioTipoPagoConcepto { get; set; }
        public virtual DbSet<CancelacionCursoDocente> CancelacionCursoDocente { get; set; }
        public virtual DbSet<CancelacionDocenteEstudio> CancelacionDocenteEstudio { get; set; }
        public virtual DbSet<Docente> Docente { get; set; }
        public virtual DbSet<DocenteCurso> DocenteCurso { get; set; }
        public virtual DbSet<DocenteDetalle> DocenteDetalle { get; set; }
        public virtual DbSet<DocenteEstudio> DocenteEstudio { get; set; }
        public virtual DbSet<DocenteEstudioDocumento> DocenteEstudioDocumento { get; set; }
        public virtual DbSet<DocenteEstudioPeriodo> DocenteEstudioPeriodo { get; set; }
        public virtual DbSet<DocentePublicacion> DocentePublicacion { get; set; }
        public virtual DbSet<DocumentoTipo> DocumentoTipo { get; set; }
        public virtual DbSet<Grupo1> Grupo1 { get; set; }
        public virtual DbSet<Hora> Hora { get; set; }
        public virtual DbSet<Materia> Materia { get; set; }
        public virtual DbSet<MateriaApertura> MateriaApertura { get; set; }
        public virtual DbSet<MateriaCompartida> MateriaCompartida { get; set; }
        public virtual DbSet<MateriaCompartidaDetalle> MateriaCompartidaDetalle { get; set; }
        public virtual DbSet<MateriaTipo> MateriaTipo { get; set; }
        public virtual DbSet<Menu1> Menu1 { get; set; }
        public virtual DbSet<Salon> Salon { get; set; }
        public virtual DbSet<SalonAsignacion> SalonAsignacion { get; set; }
        public virtual DbSet<Sede> Sede { get; set; }
        public virtual DbSet<SubMenu1> SubMenu1 { get; set; }
        public virtual DbSet<TipoUsuarioSubmenu1> TipoUsuarioSubmenu1 { get; set; }
        public virtual DbSet<VistoBuenoCurso> VistoBuenoCurso { get; set; }
        public virtual DbSet<VistoBuenoEstudio> VistoBuenoEstudio { get; set; }
        public virtual DbSet<AlumnoBeca> AlumnoBeca { get; set; }
        public virtual DbSet<AlumnoInscritoBitacoraBKSalome> AlumnoInscritoBitacoraBKSalome { get; set; }
        public virtual DbSet<AlumnoMatricula> AlumnoMatricula { get; set; }
        public virtual DbSet<BecaSEPBitacora> BecaSEPBitacora { get; set; }
        public virtual DbSet<COLEGIATURA> COLEGIATURA { get; set; }
        public virtual DbSet<IdiomasInscrito> IdiomasInscrito { get; set; }
        public virtual DbSet<PagoDescuentoBitacora> PagoDescuentoBitacora { get; set; }
        public virtual DbSet<ReporteEstadoCuentaDescuento> ReporteEstadoCuentaDescuento { get; set; }
        public virtual DbSet<ReporteEstadoCuentaPagosConcepto> ReporteEstadoCuentaPagosConcepto { get; set; }
        public virtual DbSet<Saldos> Saldos { get; set; }
        public virtual DbSet<UniversidadAdeudo> UniversidadAdeudo { get; set; }
        public virtual DbSet<UniversidadNewAdeudo> UniversidadNewAdeudo { get; set; }
    
        public virtual ObjectResult<AlumnoInscritoCompleto> spAlumnoInscritoCompleto(Nullable<int> anio, Nullable<int> periodo)
        {
            var anioParameter = anio.HasValue ?
                new ObjectParameter("Anio", anio) :
                new ObjectParameter("Anio", typeof(int));
    
            var periodoParameter = periodo.HasValue ?
                new ObjectParameter("Periodo", periodo) :
                new ObjectParameter("Periodo", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AlumnoInscritoCompleto>("spAlumnoInscritoCompleto", anioParameter, periodoParameter);
        }
    
        public virtual ObjectResult<string> spGeneraReferencia(Nullable<int> pagoId)
        {
            var pagoIdParameter = pagoId.HasValue ?
                new ObjectParameter("pagoId", pagoId) :
                new ObjectParameter("pagoId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("spGeneraReferencia", pagoIdParameter);
        }
    
        public virtual ObjectResult<spUsuarioLogin_Result> spUsuarioLogin(Nullable<int> usuarioId, string password, string mac)
        {
            var usuarioIdParameter = usuarioId.HasValue ?
                new ObjectParameter("usuarioId", usuarioId) :
                new ObjectParameter("usuarioId", typeof(int));
    
            var passwordParameter = password != null ?
                new ObjectParameter("password", password) :
                new ObjectParameter("password", typeof(string));
    
            var macParameter = mac != null ?
                new ObjectParameter("mac", mac) :
                new ObjectParameter("mac", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spUsuarioLogin_Result>("spUsuarioLogin", usuarioIdParameter, passwordParameter, macParameter);
        }
    
        public virtual ObjectResult<Nullable<decimal>> spConceptoDescuento(Nullable<int> conceptoPagoId, Nullable<int> alumnoId, Nullable<int> periodo, Nullable<int> anio)
        {
            var conceptoPagoIdParameter = conceptoPagoId.HasValue ?
                new ObjectParameter("conceptoPagoId", conceptoPagoId) :
                new ObjectParameter("conceptoPagoId", typeof(int));
    
            var alumnoIdParameter = alumnoId.HasValue ?
                new ObjectParameter("alumnoId", alumnoId) :
                new ObjectParameter("alumnoId", typeof(int));
    
            var periodoParameter = periodo.HasValue ?
                new ObjectParameter("periodo", periodo) :
                new ObjectParameter("periodo", typeof(int));
    
            var anioParameter = anio.HasValue ?
                new ObjectParameter("anio", anio) :
                new ObjectParameter("anio", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("spConceptoDescuento", conceptoPagoIdParameter, alumnoIdParameter, periodoParameter, anioParameter);
        }
    
        public virtual int SPU_Pagod(Nullable<int> pagoId, Nullable<int> estatusId, Nullable<int> fCUSUARIO, string fCOBSERVACIONES)
        {
            var pagoIdParameter = pagoId.HasValue ?
                new ObjectParameter("PagoId", pagoId) :
                new ObjectParameter("PagoId", typeof(int));
    
            var estatusIdParameter = estatusId.HasValue ?
                new ObjectParameter("EstatusId", estatusId) :
                new ObjectParameter("EstatusId", typeof(int));
    
            var fCUSUARIOParameter = fCUSUARIO.HasValue ?
                new ObjectParameter("FCUSUARIO", fCUSUARIO) :
                new ObjectParameter("FCUSUARIO", typeof(int));
    
            var fCOBSERVACIONESParameter = fCOBSERVACIONES != null ?
                new ObjectParameter("FCOBSERVACIONES", fCOBSERVACIONES) :
                new ObjectParameter("FCOBSERVACIONES", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SPU_Pagod", pagoIdParameter, estatusIdParameter, fCUSUARIOParameter, fCOBSERVACIONESParameter);
        }
    
        public virtual ObjectResult<spReporteBecasConcentrado> spReporteBecasConcentrado(Nullable<int> anio, Nullable<int> periodoId)
        {
            var anioParameter = anio.HasValue ?
                new ObjectParameter("anio", anio) :
                new ObjectParameter("anio", typeof(int));
    
            var periodoIdParameter = periodoId.HasValue ?
                new ObjectParameter("periodoId", periodoId) :
                new ObjectParameter("periodoId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spReporteBecasConcentrado>("spReporteBecasConcentrado", anioParameter, periodoIdParameter);
        }
    
        public virtual ObjectResult<spReporteBecasDetalle> spReporteBecasDetalle(Nullable<int> anio, Nullable<int> periodoId)
        {
            var anioParameter = anio.HasValue ?
                new ObjectParameter("anio", anio) :
                new ObjectParameter("anio", typeof(int));
    
            var periodoIdParameter = periodoId.HasValue ?
                new ObjectParameter("periodoId", periodoId) :
                new ObjectParameter("periodoId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spReporteBecasDetalle>("spReporteBecasDetalle", anioParameter, periodoIdParameter);
        }
    
        public virtual ObjectResult<spReporteCarteraVencida_Result> spReporteCarteraVencida(Nullable<int> anio, Nullable<int> periodoId, Nullable<System.DateTime> fechaInicial, Nullable<System.DateTime> fechaFinal)
        {
            var anioParameter = anio.HasValue ?
                new ObjectParameter("anio", anio) :
                new ObjectParameter("anio", typeof(int));
    
            var periodoIdParameter = periodoId.HasValue ?
                new ObjectParameter("periodoId", periodoId) :
                new ObjectParameter("periodoId", typeof(int));
    
            var fechaInicialParameter = fechaInicial.HasValue ?
                new ObjectParameter("fechaInicial", fechaInicial) :
                new ObjectParameter("fechaInicial", typeof(System.DateTime));
    
            var fechaFinalParameter = fechaFinal.HasValue ?
                new ObjectParameter("fechaFinal", fechaFinal) :
                new ObjectParameter("fechaFinal", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spReporteCarteraVencida_Result>("spReporteCarteraVencida", anioParameter, periodoIdParameter, fechaInicialParameter, fechaFinalParameter);
        }
    
        public virtual ObjectResult<spSaldoAlumno_Result> spSaldoAlumno(Nullable<int> alumnoId, Nullable<bool> detalle)
        {
            var alumnoIdParameter = alumnoId.HasValue ?
                new ObjectParameter("alumnoId", alumnoId) :
                new ObjectParameter("alumnoId", typeof(int));
    
            var detalleParameter = detalle.HasValue ?
                new ObjectParameter("detalle", detalle) :
                new ObjectParameter("detalle", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spSaldoAlumno_Result>("spSaldoAlumno", alumnoIdParameter, detalleParameter);
        }
    
        public virtual ObjectResult<SP_ReporteSaldoAlumno_Result> SP_ReporteSaldoAlumno()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_ReporteSaldoAlumno_Result>("SP_ReporteSaldoAlumno");
        }
    }
}
