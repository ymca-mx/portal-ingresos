using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data.Entity;
using System.Globalization;

namespace BLL
{
    public class BLLGrupoIdioma
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");


        public static List<DTOGrupoIdiomas> ConsultarGruposIdiomas()
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {
                var listagrupos = db.IdiomaGrupo.Select(a => new DTOGrupoIdiomas
                                                            {
                                                                GrupoIdiomaId = a.IdiomaGrupoId,
                                                                Descripcion = a.Descripcion,
                                                                Anio = (int)a.Anio,
                                                                PeriodoId = (int)a.PeriodoId,
                                                                Cuatrimestre = db.Periodo.Where(b => b.Anio == a.Anio && b.PeriodoId == a.PeriodoId).FirstOrDefault().Descripcion
                                                            }).ToList();

                return listagrupos;

            }


        }

        public static string GuardarGrupoIdioma(string nombre, int anio, int periodoid, int usuarioid, int grupoId)
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    if (grupoId == 0)
                    {
                        db.IdiomaGrupo.Add(new IdiomaGrupo
                        {
                            Descripcion = nombre,
                            Anio = anio,
                            PeriodoId = periodoid,
                            FechaRegistro = DateTime.Now,
                            HoraRegistro = DateTime.Now.TimeOfDay,
                            UsuarioId = usuarioid

                        });
                    }
                    else
                    {
                        var ActualizaGrupo = db.IdiomaGrupo.Where(a => a.IdiomaGrupoId == grupoId).FirstOrDefault();

                        ActualizaGrupo.Descripcion = nombre;
                        ActualizaGrupo.Anio = anio;
                        ActualizaGrupo.PeriodoId = periodoid;
                        ActualizaGrupo.FechaRegistro = DateTime.Now;
                        ActualizaGrupo.HoraRegistro = DateTime.Now.TimeOfDay;
                        ActualizaGrupo.UsuarioId = usuarioid;
                    }



                    db.SaveChanges();
                    return "Guardado";
                }
                catch (Exception)
                {

                    return "Error"; ;
                }

            }


        }

        public static string EliminarGrupoIdioma(int grupoId)
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    var GrupoEliminar = db.IdiomaGrupo.Where(a => a.IdiomaGrupoId == grupoId).FirstOrDefault();
                    db.IdiomaGrupoAlumno.RemoveRange(
                    GrupoEliminar.IdiomaGrupoAlumno);
                    db.IdiomaGrupo.Remove(GrupoEliminar);
                    db.SaveChanges();

                    
                    return "Eliminado";
                }
                catch (Exception)
                {

                    return "Error"; ;
                }

            }


        }


        public static List<DTOAlumnoIdiomas> ConsultarAlumnosIdiomas()
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {
                var pagoconcepto = new int[] { 1009, 807 };

                var listaAlumnosIdiomas1 = db.Pago.Where(a => a.OfertaEducativa.OfertaEducativaTipoId == 4
                                                        && pagoconcepto.Contains(a.Cuota1.PagoConceptoId)
                                                        && a.EstatusId != 2)
                                                  .GroupBy(b => new { b.AlumnoId, b.Cuota1.PagoConceptoId, b.OfertaEducativaId, b.Anio, b.PeriodoId, b.FechaGeneracion })
                                                  .Select(d => new DTOAlumnoIdiomas1
                                                  {
                                                        AlumnoId = d.Key.AlumnoId,
                                                        PagoConceptoId  = d.Key.PagoConceptoId,
                                                        OfertaEducativaId=  d.Key.OfertaEducativaId,
                                                        Anio = d.Key.Anio,
                                                        PeriodoId = d.Key.PeriodoId,
                                                        FechaGeneracion = d.Key .FechaGeneracion
                                                  }


                    ).ToList();

                 listaAlumnosIdiomas1 = listaAlumnosIdiomas1.Where(a => a.Anio == (listaAlumnosIdiomas1.Where(a1 => a1.AlumnoId == a.AlumnoId).Max(a2 => a2.Anio))
                                                                       && a.PeriodoId == (listaAlumnosIdiomas1.Where(a1 => a1.AlumnoId == a.AlumnoId
                                                                                                                      && a1.Anio == (listaAlumnosIdiomas1.Where(a2 => a2.AlumnoId == a.AlumnoId).Max(a2 => a2.Anio)))
                                                                                                                      .Max(a3 => a3.PeriodoId))
                                                                       && a.FechaGeneracion == (listaAlumnosIdiomas1.Where(a1 => a1.AlumnoId == a.AlumnoId).Max(a2 => a2.FechaGeneracion))
                                                                       ).ToList();

                 var listaAlumnosIdiomas2 = (from a in listaAlumnosIdiomas1
                                            join b in db.Alumno on a.AlumnoId equals b.AlumnoId
                                            join c in db.OfertaEducativa on a.OfertaEducativaId equals c.OfertaEducativaId
                                            select new DTOAlumnoIdiomas
                 {
                     AlumnoId = a.AlumnoId,
                     Nombre = b.Nombre + " " + b.Paterno + " " + b.Materno,
                     OfertaEducativaId = a.OfertaEducativaId,
                     OfertaEducativa = c.Descripcion,
                     TipoDeCurso = a.PagoConceptoId == 807 ? "Normal" : "Intensivo",
                     GrupoAlumno = db.IdiomaGrupoAlumno.Where(e => e.AlumnoId == a.AlumnoId).Select(r => new DTOGrupoIdiomasAlumno
                     {
                         GrupoIdiomaId = r.IdiomaGrupoId,
                         TipoCurso = r.TipoCurso
                     }).FirstOrDefault()
                 }
                 ).ToList();

                 return listaAlumnosIdiomas2;

            }


        }


        public static string AsignarAlumnosIdiomas(int AlumnoId, int GrupoId, string TipoCurso, int usuarioid, int OfertaId, int TM) 
        {

            using (UniversidadEntities db = new UniversidadEntities()) 
            {
                try
                {

                    if (TM == 1 || TM == 2)
                    {
                         db.IdiomaGrupoAlumno.Add(new IdiomaGrupoAlumno
                    {
                        AlumnoId = AlumnoId,
                        IdiomaGrupoId = GrupoId,
                        OfertaEducativaId = OfertaId,
                        TipoCurso = TipoCurso,
                        FechaAsignacion = DateTime.Now,
                        HoraAsignacion = DateTime.Now.TimeOfDay,
                        UsuarioId = usuarioid,
                        EstatusId = 1
                    });

                    }

                    if (TM == 2)
                    {
                        var eliminar = db.IdiomaGrupoAlumno.Where(a => a.AlumnoId == AlumnoId
                                                                  && a.OfertaEducativaId == OfertaId
                                                                  ).FirstOrDefault();
                        db.IdiomaGrupoAlumno.Remove(eliminar);
                    }
                    if (TM == 3)
                    {
                        var eliminar = db.IdiomaGrupoAlumno.Where(a => a.AlumnoId == AlumnoId
                                                                  && a.IdiomaGrupoId == GrupoId
                                                                  && a.OfertaEducativaId == OfertaId
                                                                  ).FirstOrDefault();
                        db.IdiomaGrupoAlumno.Remove(eliminar);
                    }
                    
                    db.SaveChanges();
                    return "Guardado";
                }
                catch (Exception)
                {
                    return "Error";
                    throw;
                }
              
            }
        }

        

        public static List<DTOAlumnoIdiomas> ConsultarAlumnosIdiomasGrupo(int GrupoId) 
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    var gruposidiomas = db.IdiomaGrupoAlumno.Where(a => a.IdiomaGrupoId == GrupoId)
                                                            .Select(b => new DTOAlumnoIdiomas
                                                            {
                                                                AlumnoId = b.AlumnoId,
                                                                Nombre = b.Alumno.Nombre + " " + b.Alumno.Paterno + " " + b.Alumno.Materno,
                                                                OfertaEducativaId = (int)b.OfertaEducativaId,
                                                                OfertaEducativa = b.OfertaEducativa.Descripcion,
                                                                TipoDeCurso = b.TipoCurso
                                                            }).ToList();

                    return gruposidiomas;
                }
                catch (Exception)
                {
                    return null;
                    throw;
                }

            }
        }

        

    }
}
