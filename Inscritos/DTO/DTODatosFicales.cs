using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTODatosFicales
    {        
        public int EmpresaId { get; set; }
        public string RFC { get; set; }
        public int PaisId { get; set; }
        public int EntidadFederativaId { get; set; }
        public int MunicipioId { get; set; }
        public string CP { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string NoExterior { get; set; }
        public string NoInterior { get; set; }
        public string Observacion { get; set; }
        public Nullable<bool> EsEmpresa { get; set; }
        public DTOMunicipio Municipio { get; set; }
        public DTOPais Pais { get; set; }
        public DTOEmpresaDetalle EmpresaDetalle { get; set; }
    }
}
