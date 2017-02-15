using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using System.Data.Entity;

namespace BLL
{
    public class BLLAlumnoBeca
    {
        public  static List<DTO.Alumno.Beca.DTOAlumnoBeca> listarAlumnos(List<int> Alumnos)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTO.Alumno.Beca.DTOAlumnoBeca> listaAlumnos = new List<DTO.Alumno.Beca.DTOAlumnoBeca>();
                Alumnos.ForEach(a1 =>
                {
                    listaAlumnos.Add((from a in db.AlumnoInscrito
                                      where a.AlumnoId == a1 && a.Anio == 2016 && a.PeriodoId == 3
                                      select new DTO.Alumno.Beca.DTOAlumnoBeca
                                      {
                                          alumnoId = a.AlumnoId,
                                          anio = a.Anio,
                                          ofertaEducativaId = a.OfertaEducativaId,
                                          periodoId = a.PeriodoId
                                      }).FirstOrDefault());
                });

                return listaAlumnos;
            }
        }

        public static bool GuardarEnBitacora(DTO.Alumno.Beca.DTOAlumnoBeca AlumnoBeca)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    AlumnoInscrito objAl = db.AlumnoInscrito.Where(a => a.AlumnoId == AlumnoBeca.alumnoId &&
                    a.OfertaEducativaId == AlumnoBeca.ofertaEducativaId
                    ).OrderBy(s => s.FechaInscripcion).ToList().FirstOrDefault();

                    AlumnoBeca.anio = objAl.Anio;
                    AlumnoBeca.periodoId = objAl.PeriodoId;
                    db.AlumnoInscritoBeca.Add(new AlumnoInscritoBeca
                    {
                        AlumnoId = AlumnoBeca.alumnoId,
                        OfertaEducativaId = AlumnoBeca.ofertaEducativaId,
                        Anio = AlumnoBeca.anio,
                        PeriodoId = AlumnoBeca.periodoId,
                        UsuarioId = AlumnoBeca.usuarioId,
                        FechaAplicacion = DateTime.Now,
                        HoraAplicacion = DateTime.Now.TimeOfDay,
                        EsSEP = AlumnoBeca.esSEP,
                        Porcentaje = AlumnoBeca.porcentajeBeca,
                        //EstatusId = AlumnoBeca.estatusid,
                    });
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
    }
}
