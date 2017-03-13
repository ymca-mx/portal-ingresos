using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAlumnoDetalle
    {
        public int AlumnoId { get; set; }
        public int GeneroId { get; set; }
        public int EstadoCivilId { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public string FechaNacimientoC { get; set; }
        public string CURP { get; set; }
        public int PaisId { get; set; }
        public int EntidadFederativaId { get; set; }
        public Nullable<int> EntidadNacimientoId { get; set; }
        public int MunicipioId { get; set; }
        public string Cp { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string NoExterior { get; set; }
        public string NoInterior { get; set; }
        public string TelefonoCasa { get; set; }
        public string TelefonoOficina { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public int ProspectoId { get; set; }
        public DTOProspectoDetalle ProspectoO { get; set; }
        public int UsuarioId { get; set; }
    }
}
