using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOProspectoDetalle
    {
        public int ProspectoId { get; set; }
        public Nullable<System.DateTime> FechaNacimiento { get; set; }
        public string CURP { get; set; }
        public Nullable<int> GeneroId { get; set; }
        public Nullable<int> EstadoCivilId { get; set; }
        public Nullable<int> PaisId { get; set; }
        public Nullable<int> EntidadFederativaId { get; set; }
        public Nullable<int> MunicipioId { get; set; }
        public string CP { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string NoExterior { get; set; }
        public string NoInterior { get; set; }
        public string PrepaProcedencia { get; set; }
        public int PrepaArea { get; set; }
        public Nullable<int> PrepaAnio { get; set; }
        public Nullable<int> PrepaMes { get; set; }
        public Nullable<int> PrepaPaisId { get; set; }
        public Nullable<decimal> PrepaPromedio { get; set; }
        public Nullable<int> PrepaEntidadId { get; set; }
        public Nullable<bool> EsEquivalencia { get; set; }
        public string UniversidadProcedencia { get; set; }
        public Nullable<int> UniversidadPaisId { get; set; }
        public Nullable<int> UniversidadEntidadId { get; set; }
        public string UniversidadMotivo { get; set; }
        public Nullable<bool> EsTitulado { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Telefono { get; set; }
        public Nullable<int> SucursalId { get; set; }
        public Nullable<int> OfertaEducativaTipoId { get; set; }
        public Nullable<int> OfertaEducativaId { get; set; }
        public Nullable<int> TurnoId { get; set; }
        public string Observaciones { get; set; }
        public Nullable<int> MedioDifusionId { get; set; }
    }
}
