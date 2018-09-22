using DAL;
using DAL.ACJ;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLCertificado
    {
        public static CultureInfo Region = new CultureInfo("es-MX", true);

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

        public static object GetAlumno(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    int[] OfertasTipoNt = { 2, 4 };

                    var Alumnodb = db.Alumno
                        .Where(a => a.AlumnoId == AlumnoId)
                        .AsEnumerable()
                        .ToList();

                    var ofertas = Alumnodb.FirstOrDefault()
                            .AlumnoInscrito
                            .Where(a => !OfertasTipoNt.Contains(a.OfertaEducativa.OfertaEducativaTipoId)
                                    && a.OfertaEducativa.InstitucionOfertaEducativa.Count > 0
                                    && a.Alumno.AlumnoTitulo
                                        .Where(b => b.AlumnoOfertaEducativa.OfertaEducativaId == a.OfertaEducativaId)
                                        .ToList()
                                        .Count == 0)
                            .ToList();

                    List<dynamic> sede = new List<dynamic>();

                    ofertas.ForEach(a =>
                    {
                        dynamic item = new ExpandoObject();
                        if (sede.Count == 0)
                        {
                            item.InstitucionId = a.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().InstitucionId;
                            item.SedeId = a.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().CampusId;
                            item.Nombre = a.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().Campus.Descripcion;
                            item.Clave = a.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().Campus.Clave;
                            item.Ofertas = new List<dynamic>();

                            sede.Add(item);
                        }
                        else
                        {
                            object resQuery = sede.Find(b => b.SedeId ==
                                a.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().CampusId);

                            if (sede.Find(b => b.SedeId ==
                              a.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().CampusId) == null)
                            {
                                item.InstitucionId = a.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().InstitucionId;
                                item.SedeId = a.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().CampusId;
                                item.Nombre = a.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().Campus.Descripcion;
                                item.Clave = a.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().Campus.Clave;
                                item.Ofertas = new List<dynamic>();

                                sede.Add(item);
                            }
                        }
                    });

                    sede.ForEach(sed =>
                    {
                        sed.Ofertas = new List<dynamic>();
                        sed.Ofertas.AddRange(
                            ofertas
                            .Where(a => sed.SedeId ==
                                  a.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().CampusId)
                            .Select(a => new
                            {
                                a.OfertaEducativaId,
                                a.OfertaEducativa.Descripcion,
                                a.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().ClaveOfertaEducativa,
                                a.OfertaEducativa.Rvoe,
                                FechaInicio = (db.AlumnoInscritoBitacora
                                                    .Where(c => c.AlumnoId == a.AlumnoId
                                                            && c.OfertaEducativaId == a.OfertaEducativaId)
                                                     .AsEnumerable()
                                                     .OrderByDescending(c => c.FechaInscripcion)
                                                     ?.FirstOrDefault()
                                                     ?.FechaInscripcion
                                                     ?? DateTime.Now)
                                                     .ToString("dd/MM/yyyy"),
                                FechaFin = DateTime.Now.ToString("dd/MM/yyyy"),
                            })
                            .ToList()
                            );

                    });

                    return
                  Alumnodb
                        .Select(a => new
                        {
                            Status = true,
                            a.AlumnoId,
                            a.Nombre,
                            a.Paterno,
                            a.Materno,
                            a.AlumnoDetalle.CURP,
                            a.AlumnoDetalle.Email,
                            Sede = sede,
                        })
                        .FirstOrDefault();
                }
                catch (Exception err)
                {
                    return new
                    {
                        Status = false,
                        err.Message,
                        Inner = err?.InnerException?.Message ?? ""
                    };
                }
            }
        }
    }
}
