using System;
using System.Collections.Generic;
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
    }

    public class DTOAlumnoPromocionCasa
    {
        public int AlumnoId { get; set; }
        public string NombreC { get; set; }
        public List<DTOOfertaEducativa1> OfertaEducativa { get; set; }
        public int OfertaEducativaIdActual { get; set; }
        public string OfertaEducativaActual { get; set; }
        public int AlumnoIdProspecto { get; set; }
        public string NombreCProspecto { get; set; }
        public int OfertaEducativaIdProspecto { get; set; }
        public string OfertaEducativaProspecto { get; set; }
        public bool AlumnoProspecto { get; set; }
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
        public int EnProcesoSolicitud  { get; set; }
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
        public string Descripcion {
            get
            {
                return string.Join(" / ", _OfertasEducativas2.ToArray());
            }
            set
            {
            }
        }
        private List<string> _OfertasEducativas2 = null;
        public List<string> OfertasEducativas { get
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
        public List<PeridoBeca> PeriodosAlumno { get; set; }
        public List<DTOAlumnoOfertas> OfertasAlumnos { get; set; }
        public List<DTOAlumnoDescuento> lstDescuentos { get; set; }
    }
    public class DTOAlumnoBecaDeportiva
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public List<PeridoBeca> PeriodosAlumno { get; set; }
        public List<DTOAlumnoOfertas> OfertasAlumnos { get; set; }
        public List<DTOAlumnoDescuento> lstDescuentos { get; set; }
    }
    public class PeridoBeca
    {
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public string Descripcion { get; set; }
        public int OfertaEducativaId { get; set; }
        public bool EsEmprea { get; set; }
    }
}

