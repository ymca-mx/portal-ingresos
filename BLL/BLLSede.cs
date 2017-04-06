using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data.Entity;

namespace BLL
{
    public class BLLSede
    {
        public static string SedeAlumno(int AlumnoId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                var Alumno= db.Alumno
                        .Where(a => a.AlumnoId == AlumnoId)
                        .FirstOrDefault();

                int oferta = Alumno.AlumnoInscrito?.LastOrDefault()?.OfertaEducativa?.SucursalId ?? 0;

                return oferta != 2 ? "../portalAlumno/Documentos/Reglamento.pdf" : "../portalAlumno/Documentos/ReglamentoCamohmila.pdf";
            }
        }
    }
}
