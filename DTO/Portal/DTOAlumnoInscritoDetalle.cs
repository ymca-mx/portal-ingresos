using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAlumnoInscritoDetalle
    {
        public int AlumnoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public bool NuevoIngreso { get; set; }
        public int EstatusId { get; set; }
        public bool CargosIniciales { get; set; }
        public int UsuarioCargosIniciales { get; set; }
        public System.DateTime FechaCargosIniciales { get; set; }
        public bool Inscripcion { get; set; }
        public int UsuarioInscripcion { get; set; }
        public System.DateTime FechaInscripcion { get; set; }
        public bool BecaAcademica { get; set; }
        public decimal Porcentaje { get; set; }
        public int UsuarioBecaAcademica { get; set; }
        public System.DateTime FechaBecaAcademica { get; set; }
        public bool BecaSEP { get; set; }
        public int UsuarioBecaSEP { get; set; }
        public System.DateTime FechaBecaSEP { get; set; }
        public bool BecaComite { get; set; }
        public int UsuarioBecaComite { get; set; }
        public System.DateTime FechaBecaComite { get; set; }
        public bool BecaDeportiva { get; set; }
        public decimal PorcentajeBecaDeportiva { get; set; }
        public int UsuarioBecaDeportiva { get; set; }
        public System.DateTime FechaBecaDeportiva { get; set; }
        public bool Pagare { get; set; }
        public int UsuarioPagare { get; set; }
        public System.DateTime FechaPagare { get; set; }
        public bool AdeudoBiblioteca { get; set; }
        public int UsuarioAdeudoBiblioteca { get; set; }
        public System.DateTime FechaAdeudoBiblioteca { get; set; }
        public bool Financiamiento { get; set; }
        public int UsuarioFinanciamiento { get; set; }
        public System.DateTime FechaFinanciamiento { get; set; }

       public DTOAlumnoInscritoDetalle()
        {
            DateTime fDefault = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            FechaAdeudoBiblioteca = fDefault;
            FechaBecaAcademica = fDefault;
            FechaBecaComite= fDefault;
            FechaBecaDeportiva = fDefault;
            FechaBecaSEP = fDefault;
            FechaCargosIniciales = fDefault;
            FechaFinanciamiento = fDefault;
            FechaInscripcion = fDefault;
            FechaPagare = fDefault;
            
        }
    }  
}
