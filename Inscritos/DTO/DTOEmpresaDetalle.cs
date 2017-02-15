using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOEmpresaDetalle
    {
        public int EmpresaId { get; set; }
        public string RFC { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string EmailContacto { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public int PaisId { get; set; }
        public int EntidadFederativaId { get; set; }
        public int MunicipioId { get; set; }
        public string CP { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string NoExterior { get; set; }
        public string NoInterior { get; set; }
        public string Email { get; set; }
        public string Observacion { get; set; }
        public DTODatosFicales DatosFiscales { get; set; }
        public DTOEmpresa Empresa { get; set; }
        public DTOMunicipio Municipio { get; set; }
        public DTOPais Pais { get; set; }
    }
}
