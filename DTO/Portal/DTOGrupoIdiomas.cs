using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOGrupoIdiomas
    {
        public int GrupoIdiomaId { get; set; }
        public string Descripcion { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public string Cuatrimestre { get; set; }


        //public System.DateTime FechaRegistro { get; set; }
        //public System.TimeSpan HoraRegistro { get; set; }
       // public int UsuarioId { get; set; }

    }


    public class DTOGrupoIdiomasAlumno
    {
        public int AlumnoId { get; set; }
        public int GrupoIdiomaId { get; set; }
        public string TipoCurso { get; set; }
        //public System.DateTime FechaAsignacion { get; set; }
        //public System.TimeSpan HoraAsignacion { get; set; }
        //public int UsuarioId { get; set; }
        //public int EstatusId { get; set; }

    }

    public class DTOAlumnoIdiomas1
    {
        public int AlumnoId { get; set; }
        public int PagoConceptoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public DateTime FechaGeneracion { get; set; }

        //public System.DateTime FechaAsignacion { get; set; }
        //public System.TimeSpan HoraAsignacion { get; set; }
        //public int UsuarioId { get; set; }
        //public int EstatusId { get; set; }

    }


    public class DTOAlumnoIdiomas
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public int OfertaEducativaId { get; set; }
        public string OfertaEducativa { get; set; }
        public string TipoDeCurso { get; set; }
        public DTOGrupoIdiomasAlumno GrupoAlumno{ get; set; }

    }
}
