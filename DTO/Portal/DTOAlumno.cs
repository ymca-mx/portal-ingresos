﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DTO
{
    

    public class DTOAlumno
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string FechaRegistro { get; set; }
        public int AlumnoDescuento { get; set; }
        public int AlumnoDetalle { get; set; }
        public int AlumnoOfertaEducativa { get; set; }
        public int PersonaAutorizada { get; set; }
        public List<DTOPersonaAutorizada> DTOPersonaAutorizada { get; set; }
        public int UsuarioId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public int EstatusId { get; set; }
        public DTOAlumnoDetalle DTOAlumnoDetalle { get; set; }
        public DTOAlumnoInscrito AlumnoInscrito { get; set; }
        public DTOAlumnoInscrito2 AlumnoInscrito2 { get; set; }
        public List<DTOAlumnoInscrito> lstAlumnoInscrito { get; set; }
        public DTOUsuario Usuario { get; set; }
        public DTOGrupo Grupo { get; set; }
        public List<DTOAlumnoDescuento> lstAlumnoDescuento { get; set; }
        public string EstatusDescuento { get; set; }
        public string Descripcion { get; set; }
        public List<DTOAlumnoOfertas> lstOfertas { get; set; }
        public Boolean Chocolates { get; set; }
        public Boolean Biblioteca { get; set; }
        public string Matricula { get; set; }
        public List<DTOAlumnoAntecendente> Antecendentes { get; set; }
        public int Plantel { get; set; }
        public string Email { get; set; }
    }

    public class DTOAlumnoDatos
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public int GeneroId { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public string FechaNacimientoC { get; set; }
        public string CURP { get; set; }
        public int PaisId { get; set; }
        public int EntidadFederativaId { get; set; }
        public Nullable<int> EntidadNacimientoId { get; set; }
        public List<DTOAlumnoDatos2> DatosContacto { get; set; }
    }

    public class DTOAlumnoDatos2
    {
        public string Dato { get; set; }
        public string ServiciosEscolares { get; set; }
    }

    public class DTOAlumnoPromocionCasa
    {
        public int AlumnoId { get; set; }
        public string NombreC { get; set; }
        public int OfertaEducativaIdActual { get; set; }
        public string OfertaEducativaActual { get; set; }
        public int AlumnoIdProspecto { get; set; }
        public string NombreCProspecto { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public int SubPeriodoId { get; set; }
        public decimal Monto { get; set; }
        public int UsuarioId { get; set; }
        public int EstatusId { get; set; }
    }

    public class DTOAlumnoCambioCarrera
    {
        public int AlumnoId { get; set; }
        public string NombreC { get; set; }
        public List<DTOOfertaEducativa2> OfertaEducativa { get; set; }
        public int OfertaEducativaIdActual { get; set; }
        public string OfertaEducativaActual { get; set; }
        public int OfertaEducativaIdNueva { get; set; }
        public string OfertaEducativaNueva { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public string DescripcionPeriodo { get; set; }
        public string Observaciones { get; set; }
        public int UsuarioId { get; set; }
        public int EstatusId { get; set; }
    }

    public class DTOAlumnoBaja
    {
        public int AlumnoId { get; set; }
        public string NombreC { get; set; }
        public int OfertaEducativaId { get; set; }
        public string OfertaEducativa { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public string DescripcionPeriodo { get; set; }
        public int TipoMovimientoId { get; set; }
        public int BajaMotivoId { get; set; }
        public string FechaRecepcion { get; set; }
        public int Folio { get; set; }
        public string Observaciones { get; set; }
        public int UsuarioId { get; set; }
        public int EstatusId { get; set; }
    }


    public class DTOTipoMovimiento
    {
        public int TipoMovimientoId { get; set; }
        public string Descripcion { get; set; }
    }

    public class DTOBajaMotivo
    {
        public int BajaMotivoId { get; set; }
        public string Descripcion { get; set; }
    }
    public class DTOCatalogoBaja
    {
        public List<DTOTipoMovimiento> TipoMovimiento { get; set; }
        public List<DTOBajaMotivo> BajaMotivo { get; set; }
    }


    public class DTOPeriodoPromocionCasa
    {
        public string Descripcion { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public List<DTOMes> Meses { get; set; }
    }


    public class DTOAlumnoReferencias
    {
        public string Concepto { get; set; }
        public string ReferenciaBancaria1 { get; set; }
        public int ReferenciaBancaria { get; set; }
        public DateTime FechaGeneracion1 { get; set; }
        public string FechaGeneracion { get; set; }
        public string UsuarioGenero { get; set; }
        public int UsuarioTipo { get; set; }
        public string Promesa { get; set; }
        public string Pagado { get; set; }
        public string Estatus { get; set; }
        public int AlumnoId { get; set; }
        public int EnProcesoSolicitud { get; set; }
    }
    public class DTOAlumnoReferencias1
    {
        public DTOAlumnoLigero1 AlumnoDatos { get; set; }
        public List<DTOAlumnoReferencias> AlumnoReferencias { get; set; }
    }

    public class DTOAlumnoLigero
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string FechaRegistro { get; set; }
        public string Descripcion
        {
            get
            {
                return string.Join(" / ", _OfertasEducativas2.ToArray());
            }
            set
            {
            }
        }
        private List<string> _OfertasEducativas2 = null;
        public List<string> OfertasEducativas
        {
            get
            {
                return null;
            }
            set
            {
                _OfertasEducativas2 = value;
            }
        }
        public string Usuario { get; set; }
    }
    public class DTOAlumnoLigero1
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
    }


    public class DTOAlumnoPermitido1
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public List<DTOAlumnoPermitido> lstBitacora { get; set; }
    }
    public class DTOAlumnoOfertas
    {
        public int OfertaEducativaId { get; set; }
        public string Descripcion { get; set; }
        public int OfertaEducativaTipoId { get; set; }
        public string Mensaje { get; set; }
    }
    public class DTOAlumnoBecaComite
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public bool EsEmpresa { get; set; }
        public List<PeriodoBeca> PeriodosAlumno { get; set; }
        public List<DTOAlumnoOfertas> OfertasAlumnos { get; set; }
        public List<DTOAlumnoDescuento> lstDescuentos { get; set; }
    }
    public class DTOAlumnoBecaDeportiva
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public List<PeriodoBeca> PeriodosAlumno { get; set; }
        public List<DTOAlumnoOfertas> OfertasAlumnos { get; set; }
        public List<DTOAlumnoDescuento> lstDescuentos { get; set; }
    }
    public class PeriodoBeca
    {
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public string Descripcion { get; set; }
        public int OfertaEducativaId { get; set; }
        public bool EsEmprea { get; set; }
    }

    public class DTOBitacoraAccesoAlumno
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public List<DTOBitacoraAcceso> BitacoraAcceso { get; set; }
        public List<DTOBitacoraPassword> BitacoraPassword { get; set; }
    }

    public class DTOBitacoraAcceso
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");
        public string FechaIngreso { get; set; }
        private DateTime FechaIngreso2_ { get; set; }
        public DateTime FechaIngreso2
        {
            get { return FechaIngreso2_; }
            set { FechaIngreso = value.ToString("dd/MM/yyyy", Cultura); }
        }
        public string HoraIngreso { get; set; }
    }

    public class DTOBitacoraPassword
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");
        public string FechaSolicitud { get; set; }
        private DateTime FechaSolicitud2_ { get; set; }
        public DateTime FechaSolicitud2
        {
            get { return FechaSolicitud2_; }
            set { FechaSolicitud = value.ToString("dd/MM/yyyy", Cultura); }
        }
        public string HoraSolicitud { get; set; }
    }

}

