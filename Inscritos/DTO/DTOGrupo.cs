using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOGrupo 
    {
        public int EmpresaId { get; set; }
        public string RFC { get; set; }
        public int GrupoId { get; set; }
        public string Descripcion { get; set; }
        public int SucursalId { get; set; }
        public string SucursalDireccion { get; set; }
        public int OfertaEducativaTipoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public string FechaInicioS { get; set; }
        public string FechaRegistroS { get; set; }
        public int NumeroDePagos { get; set; }
        public int UsuarioId { get; set; }
        public DTOEmpresa Empresa { get; set; }
        public DTOGrupoDetalle GrupoDetalle { get; set; }
        public DTOOfertaEducativa OfertaEducativa { get; set; }
        public DTOOfertaEducativaTipo OfertaEducativaTipo { get; set; }
        public List<DTOAlumno> Alumno { get; set; }
        public List<DTOGrupoComprobante> GrupoComprobante { get; set; }
    }

    public class DTOGrupoLigero
    {
        public int GrupoId { get; set; }
        public string Descripcion { get; set; }
        public int NumeroDePagos { get; set; }
      
    }
}
