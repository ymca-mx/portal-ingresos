using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAlumnoEspecial
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public int OfertaEducativaId { get; set; }
        public string OfertaEducativaS { get; set; }
        public int Anio { get; set; }
        public int Estatus { get; set; }
        public int PeriodoId { get; set; }
        public int? PagoPlanId { get; set; }
        public DTOGrupoAlumnoCuota AlumnoCuota { get; set; }
    }
    public class DTOGrupoAlumnoCuota
    {
        public int AlumnoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public int GrupoId { get; set; }
        public int EmpresaId { get; set; }
        public decimal CuotaColegiatura { get; set; }
        public decimal CuotaInscripcion { get; set; }
        public bool CuotaCongelada { get; set; }
        public bool InscripcionCongelada { get; set; }
        public bool EsEspecial { get; set; }
        public int UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public int AnioGrupo { get; set; }
        public int PeriodoIdGrupo { get; set; }
        public String DescipcionPeriodo { get; set; }
        public int NoPagos { get; set; }
        public int? SucuralGrupo { get; set; }
    }
    public class DTOGrupoAlumnoCuotaString
    {
        public string AlumnoId { get; set; }
        public string OfertaEducativaId { get; set; }
        public string OfertaEducativaIdAnterior { get; set; }
        public string CuotaColegiatura { get; set; }
        public string CuotaInscripcion { get; set; }
        public string EsCuotaCongelada { get; set; }
        public string EsInscripcionCongelada { get; set; }
        public string EsEspecial { get; set; }
        public string UsuarioId { get; set; }
        public string PagoPlanId { get; set; }
        public string GrupoId { get; set; }
        public string Anio { get; set; }
        public string PeriodoId { get; set; }
        public bool Credenciales { get; set; }
       public int NoPagos { get; set; }

    }
}
