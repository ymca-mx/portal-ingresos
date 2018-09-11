using BLL.Tools;
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

        public static object GetAlumnoNuevo(int alumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    var Alumnos = db.AlumnoTitulo.Where(a => a.EstatusId == 1 && a.AlumnoId == alumnoId).ToList().AsQueryable().ToList();



                    return
                    Alumnos
                        .Select(a => new
                        {
                            a.Alumno.AlumnoId,
                            a.Alumno.Nombre,
                            a.Alumno.Paterno,
                            a.Alumno.Materno,
                            a.Alumno.AlumnoDetalle.CURP,
                            a.Alumno.AlumnoDetalle.Email,
                            Autorizado = true,
                            a.EstatusId,
                            Institucion = new
                            {
                                a.AlumnoOfertaEducativa.InstitucionId,
                                a.AlumnoOfertaEducativa.Institucion.Nombre,
                                a.AlumnoOfertaEducativa.Institucion.InstitucionOfertaEducativa.FirstOrDefault().Campus.Clave,
                                a.AlumnoOfertaEducativa.Institucion.InstitucionOfertaEducativa.FirstOrDefault().CampusId
                            },
                            Titulo = new
                            {
                                MedioTitulacionId = a.ModalidadTitulacionId,
                                MedioTitulacion = a.ModalidadTitulacion.TipoModalidad,
                                FExamenProf = a.FechaExamenProfesional.ToString("dd/MM/yyyy"),
                                FExencion = a.FechaExencionExamenProfecional.ToString("dd/MM/yyyy"),
                                FundamentoLegalId = a.FundamentoLegalId,
                                FundamentoLegal = a.FundamentoLegal.Descripcion,
                                EntidadFederativaId = a.EntidadFederativaIdExpedicion,
                                EntidadFederativa = a.EntidadFederativa.Descripcion
                            },
                            Carrera = new
                            {
                                a.AlumnoOfertaEducativa.OfertaEducativaId,
                                OfertaEducativa = a.AlumnoOfertaEducativa.OfertaEducativa.Descripcion,
                                Clave = a.AlumnoOfertaEducativa.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().ClaveOfertaEducativa,
                                FInicio = a.AlumnoOfertaEducativa.FechaInicio.ToString("dd/MM/yyyy"),
                                FFin = a.AlumnoOfertaEducativa.FechaTermino.ToString("dd/MM/yyyy"),
                                AutReconocimientoId = a.AutorizacionReconocimientoId,
                                AutReconocimiento = a.AutorizacionReconocimiento.Descripcion,
                                RVOE = a.AlumnoOfertaEducativa.OfertaEducativa.Rvoe
                            },
                            Antecedente = new
                            {
                                a.AlumnoAntecedente1.EntidadFederativaId,
                                EntidadFederativa = a.AlumnoAntecedente1.EntidadFederativa.Descripcion,
                                TipoAntecedenteId = a.AlumnoAntecedente1.TipoEstudioAntecedenteId,
                                TipoAntecedente = a.AlumnoAntecedente1.TipoEstudioAntecedente.Descripcion,
                                Institucion = a.AlumnoAntecedente1.Nombre,
                                FechaInicio = a.AlumnoAntecedente1.FechaInicio.ToString("dd/MM/yyyy"),
                                FechaFin = a.AlumnoAntecedente1.FechaFin.ToString("dd/MM/yyyy"),
                            },
                            Responsables = a.UsuarioResponsable
                                            .Select(b => new
                                            {
                                                b.UsuarioId,
                                                b.Usuario.Nombre,
                                                b.Usuario.Paterno,
                                                b.Usuario.Materno,
                                                b.Usuario.Cargo.FirstOrDefault().CargoId,
                                                Cargo = b.Usuario.Cargo.FirstOrDefault().Descripcion
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
                        Inner = error?.InnerException?.Message ?? "",
                        Inner2 = error?.InnerException?.InnerException?.Message ?? ""
                    };
                }
            }
        }

        public static object FirmarAlumnos(List<TituloGeneral> alumnos)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    dynamic result;
                    List<object> fallidos = new List<object>();
                    alumnos.ForEach(alumno =>
                    {
                        try
                        {
                            var AlumnoBD = db
                            .AlumnoTitulo
                            .Where(A => A.AlumnoId == alumno.AlumnoId
                                    && A.AlumnoOfertaEducativa.OfertaEducativaId == alumno.Carrera.OfertaEducativaId)
                            .FirstOrDefault();

                            AlumnoBD.EstatusId = (AlumnoBD.UsuarioResponsable.Where(a => a.Aprobo && a.UsuarioId != alumno.UsuarioId).Count()) == 1 && alumno.Autorizado ? 3 : 2;

                            var Responsable = AlumnoBD.UsuarioResponsable.Where(a => a.UsuarioId == alumno.UsuarioId).FirstOrDefault();

                            Responsable.Aprobo = alumno.Autorizado;
                            Responsable.Comentario = alumno?.Comentario ?? "";



                            if (alumno.Autorizado)
                            {
                                result =
                                SEP.CrearXMLTitulo(AlumnoBD);

                                if (result != null)
                                {
                                    fallidos.Add(new
                                    {
                                        alumno,
                                        result
                                    });
                                }
                                else
                                {
                                    db.SaveChanges();
                                }
                            }
                        }
                        catch (Exception Error)
                        {
                            fallidos.Add(new
                            {
                                alumno,
                                Error.Message,
                                Inner = Error?.InnerException?.Message??"",
                                Inner2 = Error?.InnerException?.InnerException?.Message??""
                            });
                        }
                    });

                    return new
                    {
                        Status = true,
                        fallidos
                    };
                }
                catch (Exception error)
                {
                    return new
                    {
                        Status = false,
                        error.Message,
                        Inner = error?.InnerException?.Message ?? "",
                        Inner2 = error?.InnerException?.InnerException?.Message ?? ""
                    };

                }
            }
        }

        public static object UpdateAlumnos(List<TituloGeneral> alumnos)
        {
            using (UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    List<object> fallidos = new List<object>();
                    alumnos.ForEach(alumno =>
                    {
                        try
                        {
                            var Alumnodb =
                            db.AlumnoTitulo
                            .Where(a => a.AlumnoId == alumno.AlumnoId
                                && a.AlumnoOfertaEducativa.OfertaEducativaId == alumno.Carrera.OfertaEducativaId)
                            .FirstOrDefault();

                            //SEP.AlumnoTitulo
                            Alumnodb.AutorizacionReconocimientoId = alumno.Carrera.AutReconocimientoId;
                            Alumnodb.ModalidadTitulacionId = alumno.Titulo.MedioTitulacionId;
                            Alumnodb.FechaExamenProfesional = DateTime.Parse(alumno.Titulo.FExamenProf, Region);
                            Alumnodb.FechaExencionExamenProfecional = DateTime.Parse(alumno.Titulo.FExencion, Region);
                            Alumnodb.FundamentoLegalId = alumno.Titulo.FundamentoLegalId;
                            Alumnodb.EntidadFederativaIdExpedicion = alumno.Titulo.EntidadFederativaId;
                            Alumnodb.UsuarioId = alumno.UsuarioId;
                            Alumnodb.EstatusId = alumno.EstatusId;


                            //Sep.AlumnoAntecedente
                            Alumnodb.AlumnoAntecedente1.Nombre = alumno.Antecedente.Institucion;
                            Alumnodb.AlumnoAntecedente1.TipoEstudioAntecedenteId = alumno.Antecedente.TipoAntecedenteId;
                            Alumnodb.AlumnoAntecedente1.EntidadFederativaId = alumno.Antecedente.EntidadFederativaId;
                            Alumnodb.AlumnoAntecedente1.FechaInicio = DateTime.Parse(alumno.Antecedente.FechaInicio, Region);
                            Alumnodb.AlumnoAntecedente1.FechaFin = DateTime.Parse(alumno.Antecedente.FechaFin, Region);


                            //Sep.AlumnoOfertaEducativa
                            Alumnodb.AlumnoOfertaEducativa.InstitucionId = alumno.Institucion.InstitucionId;
                            Alumnodb.AlumnoOfertaEducativa.OfertaEducativaId = alumno.Carrera.OfertaEducativaId;
                            Alumnodb.AlumnoOfertaEducativa.FechaInicio = DateTime.Parse(alumno.Carrera.FInicio, Region);
                            Alumnodb.AlumnoOfertaEducativa.FechaTermino = DateTime.Parse(alumno.Carrera.FFin, Region);
                            Alumnodb.AlumnoOfertaEducativa.RVOE = alumno.Carrera.RVOE;

                            //Sep.UsuarioResponsable
                            if (Alumnodb.UsuarioResponsable.Count == 0)
                            {

                                alumno.Responsables.ForEach(a =>
                                {
                                    db.UsuarioResponsable.Add(new UsuarioResponsable
                                    {
                                        UsuarioId = a.UsuarioId,
                                        AlumnoTituloId = Alumnodb.AlumnoTituloId,
                                        Aprobo = false
                                    });
                                });
                            }
                            else if (Alumnodb.UsuarioResponsable.Count == 2)
                            {
                                Alumnodb.UsuarioResponsable.First().UsuarioId = alumno.Responsables[0].UsuarioId;
                                Alumnodb.UsuarioResponsable.Last().UsuarioId = alumno.Responsables[1].UsuarioId;
                            }

                            db.SaveChanges();
                        }
                        catch (Exception Error)
                        {
                            fallidos.Add(new
                            {
                                alumno,
                                Error.Message,
                                Inner = Error?.InnerException?.Message,
                                Inner2 = Error?.InnerException?.InnerException?.Message
                            });
                        }
                    });

                    return new
                    {
                        Status = true,
                        fallidos
                    };
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

        public static object GetAlumnos()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    var Alumnos = db.AlumnoTitulo.Where(a => a.EstatusId == 1).ToList().AsQueryable().ToList();



                    return
                    Alumnos
                        .Select(a => new
                        {
                            a.Alumno.AlumnoId,
                            a.Alumno.Nombre,
                            a.Alumno.Paterno,
                            a.Alumno.Materno,
                            a.Alumno.AlumnoDetalle.CURP,
                            a.Alumno.AlumnoDetalle.Email,
                            Autorizado = true,
                            a.EstatusId,
                            Institucion = new
                            {
                                a.AlumnoOfertaEducativa.InstitucionId,
                                a.AlumnoOfertaEducativa.Institucion.Nombre,
                                a.AlumnoOfertaEducativa.Institucion.InstitucionOfertaEducativa.FirstOrDefault().Campus.Clave,
                                a.AlumnoOfertaEducativa.Institucion.InstitucionOfertaEducativa.FirstOrDefault().CampusId
                            },
                            Titulo = new
                            {
                                MedioTitulacionId = a.ModalidadTitulacionId,
                                MedioTitulacion = a.ModalidadTitulacion.TipoModalidad,
                                FExamenProf = a.FechaExamenProfesional.ToString("dd/MM/yyyy"),
                                FExencion = a.FechaExencionExamenProfecional.ToString("dd/MM/yyyy"),
                                FundamentoLegalId = a.FundamentoLegalId,
                                FundamentoLegal = a.FundamentoLegal.Descripcion,
                                EntidadFederativaId = a.EntidadFederativaIdExpedicion,
                                EntidadFederativa = a.EntidadFederativa.Descripcion
                            },
                            Carrera = new
                            {
                                a.AlumnoOfertaEducativa.OfertaEducativaId,
                                OfertaEducativa = a.AlumnoOfertaEducativa.OfertaEducativa.Descripcion,
                                Clave = a.AlumnoOfertaEducativa.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().ClaveOfertaEducativa,
                                FInicio = a.AlumnoOfertaEducativa.FechaInicio.ToString("dd/MM/yyyy"),
                                FFin = a.AlumnoOfertaEducativa.FechaTermino.ToString("dd/MM/yyyy"),
                                AutReconocimientoId = a.AutorizacionReconocimientoId,
                                AutReconocimiento = a.AutorizacionReconocimiento.Descripcion,
                                RVOE = a.AlumnoOfertaEducativa.OfertaEducativa.Rvoe
                            },
                            Antecedente = new
                            {
                                a.AlumnoAntecedente1.EntidadFederativaId,
                                EntidadFederativa = a.AlumnoAntecedente1.EntidadFederativa.Descripcion,
                                TipoAntecedenteId = a.AlumnoAntecedente1.TipoEstudioAntecedenteId,
                                TipoAntecedente = a.AlumnoAntecedente1.TipoEstudioAntecedente.Descripcion,
                                Institucion = a.AlumnoAntecedente1.Nombre,
                                FechaInicio = a.AlumnoAntecedente1.FechaInicio.ToString("dd/MM/yyyy"),
                                FechaFin = a.AlumnoAntecedente1.FechaFin.ToString("dd/MM/yyyy"),
                            },
                            Responsables = a.UsuarioResponsable
                                            .Select(b => new
                                            {
                                                b.UsuarioId,
                                                b.Usuario.Nombre,
                                                b.Usuario.Paterno,
                                                b.Usuario.Materno,
                                                b.Usuario.Cargo.FirstOrDefault().CargoId,
                                                Cargo = b.Usuario.Cargo.FirstOrDefault().Descripcion
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
                        Inner = error?.InnerException?.Message ?? "",
                        Inner2 = error?.InnerException?.InnerException?.Message ?? ""
                    };
                }
            }
        }

        public static object GetAlumnos(int UsuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    var Alumnos = db.AlumnoTitulo.Where(a => a.EstatusId == 2).ToList().AsQueryable().ToList();



                    return
                    Alumnos
                        .Where(a => a.UsuarioResponsable
                                        .Where(b => b.UsuarioId == UsuarioId
                                                && !b.Aprobo)
                                        .ToList()
                                        .Count > 0)
                        .Select(a => new
                        {
                            a.Alumno.AlumnoId,
                            a.Alumno.Nombre,
                            a.Alumno.Paterno,
                            a.Alumno.Materno,
                            a.Alumno.AlumnoDetalle.CURP,
                            a.Alumno.AlumnoDetalle.Email,
                            Autorizado = true,
                            Institucion = new
                            {
                                a.AlumnoOfertaEducativa.InstitucionId,
                                a.AlumnoOfertaEducativa.Institucion.Nombre,
                                a.AlumnoOfertaEducativa.Institucion.InstitucionOfertaEducativa.FirstOrDefault().Campus.Clave,
                                a.AlumnoOfertaEducativa.Institucion.InstitucionOfertaEducativa.FirstOrDefault().CampusId
                            },
                            Titulo = new
                            {
                                MedioTitulacionId = a.ModalidadTitulacionId,
                                MedioTitulacion = a.ModalidadTitulacion.TipoModalidad,
                                FExamenProf = a.FechaExamenProfesional.ToString("dd/MM/yyyy"),
                                FExencion = a.FechaExencionExamenProfecional.ToString("dd/MM/yyyy"),
                                a.FundamentoLegalId,
                                FundamentoLegal = a.FundamentoLegal.Descripcion,
                                EntidadFederativaId = a.EntidadFederativaIdExpedicion,
                                EntidadFederativa = a.EntidadFederativa.Descripcion
                            },
                            Carrera = new
                            {
                                a.AlumnoOfertaEducativa.OfertaEducativaId,
                                OfertaEducativa = a.AlumnoOfertaEducativa.OfertaEducativa.Descripcion,
                                Clave = a.AlumnoOfertaEducativa.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().ClaveOfertaEducativa,
                                FInicio = a.AlumnoOfertaEducativa.FechaInicio.ToString("dd/MM/yyyy"),
                                FFin = a.AlumnoOfertaEducativa.FechaTermino.ToString("dd/MM/yyyy"),
                                AutReconocimientoId = a.AutorizacionReconocimientoId,
                                AutReconocimiento = a.AutorizacionReconocimiento.Descripcion,
                                RVOE = a.AlumnoOfertaEducativa.OfertaEducativa.Rvoe
                            },
                            Antecedente = new
                            {
                                a.AlumnoAntecedente1.EntidadFederativaId,
                                EntidadFederativa = a.AlumnoAntecedente1.EntidadFederativa.Descripcion,
                                TipoAntecedenteId = a.AlumnoAntecedente1.TipoEstudioAntecedenteId,
                                TipoAntecedente = a.AlumnoAntecedente1.TipoEstudioAntecedente.Descripcion,
                                Institucion = a.AlumnoAntecedente1.Nombre,
                                FechaInicio = a.AlumnoAntecedente1.FechaInicio.ToString("dd/MM/yyyy"),
                                FechaFin = a.AlumnoAntecedente1.FechaFin.ToString("dd/MM/yyyy"),
                            },
                            Responsables = a.UsuarioResponsable
                                            .Select(b => new
                                            {
                                                b.UsuarioId,
                                                b.Usuario.Nombre,
                                                b.Usuario.Paterno,
                                                b.Usuario.Materno,
                                                b.Usuario.Cargo.FirstOrDefault().CargoId,
                                                Cargo = b.Usuario.Cargo.FirstOrDefault().Descripcion
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
                        Inner = error?.InnerException?.Message ?? "",
                        Inner2 = error?.InnerException?.InnerException?.Message ?? ""
                    };
                }
            }
        }

        public static object NewSolicitud(TituloGeneral AlumnoAdd)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<TituloGeneral> Fallidos = new List<TituloGeneral>();


                    AlumnoTitulo alumnoadd = new AlumnoTitulo
                    {
                        AlumnoId = AlumnoAdd.AlumnoId,
                        AutorizacionReconocimientoId = 1,
                        AlumnoAntecedente1 = new AlumnoAntecedente1
                        {
                            EntidadFederativaId = 9,
                            FechaInicio = DateTime.Parse("01/01/1900", Region),
                            FechaFin = DateTime.Parse("01/01/1900", Region),
                            TipoEstudioAntecedenteId = 4,
                            Nombre = AlumnoAdd.Antecedente.Institucion
                        },
                        AlumnoOfertaEducativa =
                                    new AlumnoOfertaEducativa
                                    {
                                        InstitucionId = AlumnoAdd.Institucion.InstitucionId,
                                        OfertaEducativaId = AlumnoAdd.Carrera.OfertaEducativaId,
                                        RVOE = AlumnoAdd.Carrera.RVOE,
                                        FechaInicio = DateTime.Parse("01/01/1900", Region),
                                        FechaTermino = DateTime.Parse("01/01/1900", Region),
                                    },
                        EntidadFederativaIdExpedicion = 9,
                        FechaExamenProfesional = DateTime.Parse("01/01/1900", Region),
                        FechaExencionExamenProfecional = DateTime.Parse("01/01/1900", Region),
                        FechaExpedicion = DateTime.Parse("01/01/1900", Region),
                        FundamentoLegalId = 4,
                        ModalidadTitulacionId = 1,
                        UsuarioId = AlumnoAdd.UsuarioId,
                        ServicioSocial = true,
                        EstatusId = 1,
                    };

                    db.AlumnoTitulo.Add(alumnoadd);

                    db.SaveChanges();

                    return new
                    {
                        Status = true
                    };
                }
                catch (Exception err)
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
                .Where(a => a.AutorizacionReconocimientoId == 1)
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
                        Inner = error?.InnerException?.Message ?? ""
                    };
                }
            }
        }

        public static object GetTipoEstudioAntecedente()
        {
            using (UniversidadEntities db = new UniversidadEntities())
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
    }
}
