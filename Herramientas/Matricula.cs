using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace Herramientas
{
    public class Matricula
    {
        public static string ObtenerMatricula(DTOAlumnoInscrito objAlumnoInscrito, DTOOfertaEducativa objOfertaEducativa, int AlumnoId)
        {
            //Periodo
            string Cad = objAlumnoInscrito.Anio.ToString();
            Cad = Cad.Substring(2, 2);
            Cad += objAlumnoInscrito.PeriodoId.ToString();
            //RVO
            Cad += objOfertaEducativa.Rvoe.Substring(4);
            //AlumnoId
            Cad += AlumnoId.ToString();
            //Turno
            Cad += "-" + objAlumnoInscrito.TurnoId.ToString();
            return Cad;

        }

        public static string GenerarMatricula(int anio, int periodoId, int alumnoid, string RVOE, int TurnoId)
        {
            //Periodo
            string Cad = anio.ToString();
            Cad = Cad.Substring(2, 2);
            Cad += periodoId.ToString();
            //RVO
            Cad += RVOE.Substring(4);
            //AlumnoId
            Cad += alumnoid.ToString();
            //Turno
            Cad += "-" + TurnoId.ToString();
            return Cad;
        }
    }
}
