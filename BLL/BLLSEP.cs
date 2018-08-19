using DAL;
using DTO.SEP;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLSEP
    {
       public static CultureInfo Region = new CultureInfo("es-MX", true);

        public static object GetAlumno(int AlumnoId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
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
                                    && a.OfertaEducativa.InstitucionOfertaEducativa.Count>0
                                    && a.Alumno.AlumnoTitulo
                                        .Where(b=> b.OfertaEducativaId == a.OfertaEducativaId)
                                        .ToList()
                                        .Count==0)
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
                                                     ??DateTime.Now)
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
                            Antecedente = a.AlumnoAntecedente
                                            .Select(c => new
                                            {
                                                c.Procedencia,
                                                c.EntidadFederativaId,
                                                FechaTermino = "01/" + c.MesId + "/" + c.Anio,
                                                FechaInicio = "01/01/1900"
                                            })
                                            .FirstOrDefault()
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

        public static object NewSolicitud(List<TituloGeneral> Alumnos)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<TituloGeneral> Fallidos = new List<TituloGeneral>();
                    Alumnos
                    .ForEach(a =>
                    {
                        try
                        {
                            AlumnoTitulo alumnoadd = new AlumnoTitulo
                            {
                                AlumnoId = a.AlumnoId,
                                AutorizacionReconocimientoId = a.Carrera.AutReconocimientoId,
                                EntidadFederativaIdAntecedente = a.Antecedente.EntidadFederativaId,
                                EntidadFederativaIdExpedicion = a.Titulo.EntidadFederativaId,
                                FechaExamenProfesional = DateTime.Parse(a.Titulo.FExamenProf, Region),
                                FechaExencionExamenProfecional = DateTime.Parse(a.Titulo.FExencion),
                                FechaExpedicion = DateTime.Now,
                                FechaFinAntecedente = DateTime.Parse(a.Antecedente.FechaFin, Region),
                                FechaInicioAntecedente = DateTime.Parse(a.Antecedente.FechaInicio, Region),
                                FechaInicio = DateTime.Parse(a.Carrera.FInicio, Region),
                                FechaTermino = DateTime.Parse(a.Antecedente.FechaFin, Region),
                                FundamentoLegalId = a.Titulo.FudamentoLegalId,
                                InstitucionId = a.Institucion.InstitucionId,
                                ModalidadTitulacionId = a.Titulo.MedioTitulacionId,
                                OfertaEducativaId = a.Carrera.OfertaEducativaId,
                                RVOE = a.Carrera.RVOE,
                                TipoEstudioAntecedenteId = a.Antecedente.TipoAntecedenteId,
                                UsuarioId = a.UsuarioId,
                                ServicioSocial = true,
                                UsuarioResponsable = new List<UsuarioResponsable>
                        {
                            new UsuarioResponsable
                            {
                                UsuarioId=a.Responsables[0].UsuarioId
                            },
                            new UsuarioResponsable
                            {
                                UsuarioId=a.Responsables[1].UsuarioId
                            }
                        }
                            };

                            db.AlumnoTitulo.Add(alumnoadd);
                            db.SaveChanges();
                        }
                        catch (Exception error)
                        {
                            a.Error = new TituloError
                            {
                                Message = error.Message,
                                InnerMessage = error?.InnerException?.Message ?? "",
                                InnerInnerMessage = error?.InnerException?.InnerException?.Message ?? ""
                            };

                            Fallidos.Add(a);
                        }
                    });

                    return new
                    {
                        Status = true,
                        Alumnos = Fallidos
                    };
                }
                catch(Exception err)
                {
                    return new
                    {
                        Status = false,
                        err.Message,
                        Inner = err?.InnerException?.Message ?? "",
                        Inner2 = err?.InnerException?.InnerException?.Message ?? ""
                    };
                }
            }
        }

        public static object GetResponsable()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return
                    db.Cargo
                        .Select(a => new
                        {
                            a.CargoId,
                            a.Descripcion,
                            Responsables =
                                a.Usuario
                                .Select(b => new
                                {
                                    b.UsuarioId,
                                    b.Nombre,
                                    b.Paterno,
                                    b.Materno
                                })
                                .ToList()
                        })
                        .ToList();
                }
                catch (Exception error)
                {
                    return new
                    {
                        error.Message,
                        Inner = error?.InnerException?.Message ?? ""
                    };
                }
            }
        }

        public static object GetServicioSocial()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return db
                        .FundamentoLegal
                        .Select(a => new
                        {
                            ServicioId = a.FundamentoLegalId,
                            a.Descripcion
                        })
                        .ToList();
                }
                catch (Exception error)
                {
                    return new
                    {
                        error.Message,
                        Inner = error?.InnerException?.Message ?? ""
                    };
                }
            }
        }

        public static object GetMedioTitulacion()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return db
                        .ModalidadTitulacion
                        .Select(a => new
                        {
                            MedioTitulacionId = a.ModalidadTitulacionId,
                            Descripcion = a.ModalidadTitulacion1,
                            a.TipoModalidad
                        })
                        .ToList();
                }
                catch (Exception error)
                {
                    return new
                    {
                        error.Message,
                        Inner = error?.InnerException?.Message ?? ""
                    };
                }
            }
        }

        public static object GetAutorizacionRec()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return
                db.AutorizacionReconocimiento
                    .Select(a => new
                    {
                        AutRecId = a.AutorizacionReconocimientoId,
                        a.Descripcion,
                    })
                    .ToList();
                }
                catch (Exception error)
                {
                    return new
                    {
                        error.Message,
                        Inner = error?.InnerException?.Message??""
                    };
                }
            }
        }

        public static object GetTipoEstudioAntecedente()
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    return
                db.TipoEstudioAntecedente
                    .Select(a => new
                    {
                        TipoEstudioId = a.TipoEstudioAntecedenteId,
                        a.TipoEstudio,
                    })
                    .ToList();
                }
                catch(Exception error)
                {
                    return new
                    {
                        error.Message,
                        Inner=error?.InnerException?.Message??""
                    };
                }
            }
        }
    }
}
