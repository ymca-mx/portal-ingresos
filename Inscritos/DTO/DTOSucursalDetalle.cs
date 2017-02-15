using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOSucursalDetalle
    {
        public int SucursalId { get; set; }
        public string Calle { get; set; }
        public string NoExterior { get; set; }
        public string Colonia { get; set; }
        public string Cp { get; set; }
        public string Delegacion { get; set; }
        public int PaisId { get; set; }
        public int EntidadFederativaId { get; set; }
        public string Telefono { get; set; }
        public DTOPais Pais { get; set; }
    }
}
