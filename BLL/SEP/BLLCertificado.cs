using DAL;
using DAL.ACJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLCertificado
    {
        public static object GetMateriaAlumno(int AlumnoId)
        {
            using(ACJEntities  db = new ACJEntities())
            {
                try
                {
                    var Result = db.SPS_CALIFICACIONES(AlumnoId);

                    //Result.

                    return new
                    {
                        Status = true,
                        Result = Result.Select(a => new
                        {
                            AlumnoId = a.FIClave,
                            Carrera = a.FCCarrera,
                            Periodo = a.FCPeriodo,
                            Profesor = a.FcProfesor,
                            CalificacionFinal = a.FCFinal,
                            ClaveMateria = a.FIMateria,
                            NombreMateria = a.FCMateria,
                            Nombre = a.FCAlumno,
                            PromedioLetra = a.FCLetra
                        }).ToList()
                    };
                }
                catch (Exception err)
                {
                    return new
                    {
                        Status = false,
                        err.Message,
                        Inner = err?.InnerException?.Message ?? "",
                        Inner2 = err?.InnerException?.Message ?? ""
                    };
                }
            }
        }
    }
}
