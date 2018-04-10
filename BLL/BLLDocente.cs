using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using System.Globalization;

namespace BLL
{
    public class BLLDocente
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");
        public static List<DTODocenteActualizar> ListaDocentesActualizar()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return db.Docente
                                                            .Select(a =>
                                                                new DTODocenteActualizar
                                                                {
                                                                    DocenteId = a.DocenteId,
                                                                    Materno = a.Materno,
                                                                    Nombre = a.Nombre,
                                                                    Paterno = a.Paterno,
                                                                    ListaEstudios = a.DocenteEstudioPeriodo
                                                                                    .Where(le => le.EstatusId == true)
                                                                                            .Select(b => new DTODocenteEstudioPeriodo
                                                                                            {
                                                                                                DocenteEstudioPeriodoId = b.DocenteEstudioPeriodoId,
                                                                                                Anio = b.Anio,
                                                                                                PeriodoId = b.PeriodoId,
                                                                                                EstudioId = b.EstudioId,
                                                                                                TieneVbo = b.VistoBuenoEstudio.Count > 0 ? true : false,
                                                                                                Periodo = new DTOPeriodo
                                                                                                {
                                                                                                    Descripcion = b.Periodo.Descripcion
                                                                                                },
                                                                                                EstudioDocente = new DTODocenteEstudio
                                                                                                {
                                                                                                    Carrera = b.DocenteEstudio.Carrera,
                                                                                                    DocenteId = b.DocenteId,
                                                                                                    EstatusId = b.DocenteEstudio.EstatusId,
                                                                                                    EstudioId = b.DocenteEstudio.EstudioId,
                                                                                                    Fecha = (b.DocenteEstudio.Fecha.Value.Day < 10 ?
                                                                                                    "0" + b.DocenteEstudio.Fecha.Value.Day : "" + b.DocenteEstudio.Fecha.Value.Day) + "/" +
                                                                                                    (b.DocenteEstudio.Fecha.Value.Month < 10 ?
                                                                                                    "0" + b.DocenteEstudio.Fecha.Value.Month : "" + b.DocenteEstudio.Fecha.Value.Month) + "/" + b.DocenteEstudio.Fecha.Value.Year,
                                                                                                    Hora = (b.DocenteEstudio.Hora.Value.Hours < 10 ? "0" + b.DocenteEstudio.Hora.Value.Hours : "" + b.DocenteEstudio.Hora.Value.Hours) + ":" +
                                                                                                (b.DocenteEstudio.Hora.Value.Minutes < 10 ? "0" + b.DocenteEstudio.Hora.Value.Minutes : "" + b.DocenteEstudio.Hora.Value.Minutes),
                                                                                                    Institucion = b.DocenteEstudio.Institucion,
                                                                                                    OfertaEducativaTipoId = b.DocenteEstudio.OfertaEducativaTipoId,
                                                                                                    UsuarioId = b.DocenteEstudio.UsuarioId,
                                                                                                    Documento = new DTODocenteEstudioDocumento
                                                                                                    {
                                                                                                        DocumentoTipoId = b.DocenteEstudio.DocumentoTipoId,
                                                                                                        DocumentoUrl = b.DocenteEstudio.DocenteEstudioDocumento.FirstOrDefault().DocumentoUrl,
                                                                                                    }
                                                                                                }
                                                                                            })
                                                                                            .ToList()
                                                                                            .OrderByDescending(b => b.Anio)
                                                                                            .ThenBy(b => b.PeriodoId)
                                                                                            .ToList(),
                                                                    CursosDocente = a.DocenteCurso
                                                                                        .Where(le =>
                                                                                            le.EstatusId == true)
                                                                                            .Select(b => new DTODocenteCurso
                                                                                            {
                                                                                                Descripcion = b.Descripcion,
                                                                                                DocenteCursoId = b.DocenteCursoId,
                                                                                                Duracion = b.Duracion,
                                                                                                EsCursoYMCA = b.EsCursoYMCA,
                                                                                                FechaFinal = (b.FechaFinal.Day < 10 ?
                                                                                                    "0" + b.FechaFinal.Day : "" + b.FechaFinal.Day) + "/" +
                                                                                                    (b.FechaFinal.Month < 10 ?
                                                                                                    "0" + b.FechaFinal.Month : "" + b.FechaFinal.Month) + "/" + b.FechaFinal.Year,
                                                                                                FechaInicial = (b.FechaInicial.Day < 10 ?
                                                                                                    "0" + b.FechaInicial.Day : "" + b.FechaInicial.Day) + "/" +
                                                                                                    (b.FechaInicial.Month < 10 ?
                                                                                                    "0" + b.FechaInicial.Month : "" + b.FechaInicial.Month) + "/" + b.FechaInicial.Year,
                                                                                                Institucion = b.Institucion,
                                                                                                VoBo = b.VoBo,
                                                                                                Anio = b.Anio,
                                                                                                PeriodoId = b.PeriodoId,
                                                                                                Periodo = new DTOPeriodo
                                                                                                {
                                                                                                    Descripcion = b.Periodo.Descripcion
                                                                                                }
                                                                                            })
                                                                                            .ToList()
                                                                                            .OrderByDescending(b=> b.Anio)
                                                                                            .ThenBy(b=> b.PeriodoId)
                                                                                            .ToList()
                                                                }).ToList();
                }
                catch { return null; }
            }
        }

        public static object GetDocumentoTipo()
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                return db.DocumentoTipo.Select(doc => new
                {
                    doc.DocumentoTipoId,
                    doc.Descripcion
                }).ToList();
            }
        }

        public static List<DTODocenteActualizar> ListaDocentesActualizarVbo()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DTOPeriodo PeriodoActual = TraerPeriodoActSig()[1];
                    return db.Docente
                        .Where(a => (a.DocenteEstudioPeriodo.Where(b => b.EstatusId == true).ToList().Count > 0)
                        || (a.DocenteCurso.Where(b => b.EstatusId == true).ToList().Count > 0))
                                                            .Select(a =>
                                                                new DTODocenteActualizar
                                                                {
                                                                    DocenteId = a.DocenteId,
                                                                    Materno = a.Materno,
                                                                    Nombre = a.Nombre,
                                                                    Paterno = a.Paterno,
                                                                    ListaEstudios = a.DocenteEstudioPeriodo
                                                                                    .Where(le =>
                                                                                            (le.Anio == PeriodoActual.Anio && le.PeriodoId == PeriodoActual.PeriodoId)
                                                                                            && le.EstatusId == true)
                                                                                            .Select(b => new DTODocenteEstudioPeriodo
                                                                                            {
                                                                                                DocenteEstudioPeriodoId = b.DocenteEstudioPeriodoId,
                                                                                                Anio = b.Anio,
                                                                                                PeriodoId = b.PeriodoId,
                                                                                                EstudioId = b.EstudioId,
                                                                                                TieneVbo = b.VistoBuenoEstudio.Count > 0 ? true : false,
                                                                                                Periodo = new DTOPeriodo
                                                                                                {
                                                                                                    Descripcion = b.Periodo.Descripcion
                                                                                                },
                                                                                                EstudioDocente = new DTODocenteEstudio
                                                                                                {
                                                                                                    Carrera = b.DocenteEstudio.Carrera,
                                                                                                    DocenteId = b.DocenteId,
                                                                                                    EstatusId = b.DocenteEstudio.EstatusId,
                                                                                                    EstudioId = b.DocenteEstudio.EstudioId,
                                                                                                    Fecha = (b.DocenteEstudio.Fecha.Value.Day < 10 ?
                                                                                                    "0" + b.DocenteEstudio.Fecha.Value.Day : "" + b.DocenteEstudio.Fecha.Value.Day) + "/" +
                                                                                                    (b.DocenteEstudio.Fecha.Value.Month < 10 ?
                                                                                                    "0" + b.DocenteEstudio.Fecha.Value.Month : "" + b.DocenteEstudio.Fecha.Value.Month) + "/" + b.DocenteEstudio.Fecha.Value.Year,
                                                                                                    Hora = (b.DocenteEstudio.Hora.Value.Hours < 10 ? "0" + b.DocenteEstudio.Hora.Value.Hours : "" + b.DocenteEstudio.Hora.Value.Hours) + ":" +
                                                                                                (b.DocenteEstudio.Hora.Value.Minutes < 10 ? "0" + b.DocenteEstudio.Hora.Value.Minutes : "" + b.DocenteEstudio.Hora.Value.Minutes),
                                                                                                    Institucion = b.DocenteEstudio.Institucion,
                                                                                                    OfertaEducativaTipoId = b.DocenteEstudio.OfertaEducativaTipoId,                                                                                                    
                                                                                                    UsuarioId = b.DocenteEstudio.UsuarioId,
                                                                                                    Documento = new DTODocenteEstudioDocumento
                                                                                                    {
                                                                                                        DocumentoTipoId = b.DocenteEstudio.DocumentoTipoId,
                                                                                                        DocumentoUrl= b.DocenteEstudio.DocenteEstudioDocumento.FirstOrDefault().DocumentoUrl,                                                                                                        
                                                                                                    }
                                                                                                }
                                                                                            })
                                                                                            .ToList(),
                                                                    CursosDocente = a.DocenteCurso
                                                                                        .Where(le =>
                                                                                            (le.Anio == PeriodoActual.Anio && le.PeriodoId == PeriodoActual.PeriodoId) && le.EstatusId == true)
                                                                                            .Select(b => new DTODocenteCurso
                                                                                            {
                                                                                                Descripcion = b.Descripcion,
                                                                                                DocenteCursoId = b.DocenteCursoId,
                                                                                                Duracion = b.Duracion,
                                                                                                EsCursoYMCA = b.EsCursoYMCA,
                                                                                                FechaFinal = (b.FechaFinal.Day < 10 ?
                                                                                                    "0" + b.FechaFinal.Day : "" + b.FechaFinal.Day) + "/" +
                                                                                                    (b.FechaFinal.Month < 10 ?
                                                                                                    "0" + b.FechaFinal.Month : "" + b.FechaFinal.Month) + "/" + b.FechaFinal.Year,
                                                                                                FechaInicial = (b.FechaInicial.Day < 10 ?
                                                                                                    "0" + b.FechaInicial.Day : "" + b.FechaInicial.Day) + "/" +
                                                                                                    (b.FechaInicial.Month < 10 ?
                                                                                                    "0" + b.FechaInicial.Month : "" + b.FechaInicial.Month) + "/" + b.FechaInicial.Year,
                                                                                                Institucion = b.Institucion,
                                                                                                VoBo = b.VoBo,
                                                                                                Anio = b.Anio,
                                                                                                PeriodoId = b.PeriodoId,
                                                                                                Periodo = new DTOPeriodo
                                                                                                {
                                                                                                    Descripcion = b.Periodo.Descripcion
                                                                                                }
                                                                                            }).ToList()
                                                                }).ToList();
                }
                catch { return null; }
            }
        }

        public static bool VboCurso(int cursoId, int usuarioId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    DocenteCurso Curso = db.DocenteCurso.Where(a => a.DocenteCursoId == cursoId).FirstOrDefault();
                    Curso.VoBo = true;

                    db.VistoBuenoCurso.Add(new VistoBuenoCurso
                    {
                        DocenteCursoId = cursoId,
                        Fecha = DateTime.Now,
                        Hora = DateTime.Now.TimeOfDay,
                        UsuarioId = usuarioId
                    });
                    db.SaveChanges();
                    return true;
                }
                catch { return false; }
            }
        }

        public static bool VboEstudio(int estudioId, int usuarioId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    db.VistoBuenoEstudio.Add(new VistoBuenoEstudio
                    {
                        DocenteEstudioPeriodoId = estudioId,
                        Fecha = DateTime.Now,
                        Hora = DateTime.Now.TimeOfDay,
                        UsuarioId = usuarioId
                    });

                    db.SaveChanges();

                    return true;
                }
                catch { return false; }
            }
        }

        public static bool CancelarCurso(int cursoId, string comentario, int usuarioId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    DocenteCurso Curso = db.DocenteCurso.Where(a => a.DocenteCursoId == cursoId).FirstOrDefault();
                    Curso.EstatusId = false;

                    db.CancelacionCursoDocente.Add(new CancelacionCursoDocente
                    {
                        Comentario = comentario,
                        Fecha = DateTime.Now,
                        Hora = DateTime.Now.TimeOfDay,
                        UsuarioId = usuarioId,
                        DocenteCursoId = cursoId
                    });

                    db.SaveChanges();

                    return true;
                }
                catch { return false; }
            }
        }

        public static bool CancelarEstudio(int estudioPeriodoId, string comentario, int usuarioId)
        {
          using(UniversidadEntities db=new UniversidadEntities())
            {
                try
                {
                    DocenteEstudioPeriodo Estudio = db.DocenteEstudioPeriodo.Where(a => a.DocenteEstudioPeriodoId == estudioPeriodoId).FirstOrDefault();

                    Estudio.EstatusId = false;
                    db.CancelacionDocenteEstudio.Add(new CancelacionDocenteEstudio
                    {
                        DocenteEstudioPeriodoId = estudioPeriodoId,
                        Comentario = comentario,
                        Fecha = DateTime.Now,
                        Hora = DateTime.Now.TimeOfDay,
                        UsuarioId = usuarioId
                    });

                    db.SaveChanges();

                    return true;
                }
                catch { return false; }
            }
        }

        public static int GuardarCurso(string NombreInstitucion, string tituloCurso, int anio, int periodoId, int duracion, string fechaFinal, string fechaInicial, bool esCursoYmca, int docenteId, int usuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DateTime FechaInicial = DateTime.ParseExact(fechaInicial, "dd/MM/yyyy", Cultura);
                    DateTime FechaFinal = DateTime.ParseExact(fechaFinal, "dd/MM/yyyy", Cultura);
                    db.DocenteCurso.Add(new DocenteCurso
                    {
                        Anio = anio,
                        Descripcion = tituloCurso,
                        DocenteId = docenteId,
                        Duracion = duracion,
                        EsCursoYMCA = esCursoYmca,
                        FechaFinal = FechaFinal,
                        FechaInicial = FechaInicial,
                        Institucion = NombreInstitucion,
                        PeriodoId = periodoId,
                        VoBo = false,
                        UsuarioId = usuarioId,
                        EstatusId = true,
                    });
                    db.SaveChanges();

                    return db.DocenteCurso.Local.FirstOrDefault().DocenteCursoId;
                }
                catch
                {
                    return -1;
                }
            }
        }

        public static List<DTOPeriodo> TraerPeriodoActSig()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                #region Actual 
                List<Periodo> listPeriodos = new List<Periodo>();

                listPeriodos.Add(db.Periodo.Where(a =>
                                             a.FechaInicial <= DateTime.Now && a.FechaFinal >= DateTime.Now)
                                            .FirstOrDefault());

                int anio = listPeriodos.FirstOrDefault().Anio,
                    periodo = listPeriodos.FirstOrDefault().PeriodoId;
                #endregion
                #region Anterior 
                anio = periodo == 1 ? anio - 1 : anio;
                periodo = periodo == 1 ? 3 : periodo - 1;

                listPeriodos.Insert(0, db.Periodo.Where(per => per.Anio == anio && periodo == per.PeriodoId).FirstOrDefault());

                #endregion

                #region Siguiente 
                anio = listPeriodos[1].Anio;
                periodo = listPeriodos[1].PeriodoId;

                anio = periodo == 3 ? anio + 1 : anio;
                periodo = periodo == 3 ? 1 : periodo + 1;

                listPeriodos.Add(db.Periodo.Where(per => per.Anio == anio && periodo == per.PeriodoId).FirstOrDefault());
                #endregion

                return listPeriodos.Select(per => new DTOPeriodo
                {
                    Anio = per.Anio,
                    PeriodoId = per.PeriodoId,
                    Descripcion = per.Descripcion
                }).ToList();
            }
        }

        public static bool GuardarRelacionDocumento(int estudioId, int tipoDocumentoId, string rutaServe)
        {
            using(UniversidadEntities db=new UniversidadEntities())
            {
                try
                {
                    db.DocenteEstudioDocumento.Add(
                        new DocenteEstudioDocumento
                        {
                            EstudioId = estudioId,
                            DocuentoTipoId = tipoDocumentoId,
                            DocumentoUrl = rutaServe
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

        public static int GuardarFormacionAcademica(int docenteId, string institucion, int oFertaTipo, string carrera, int DocumentoTipoId, int UsuarioId, int anio, int periodoId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    db.DocenteEstudioPeriodo.Add(new DocenteEstudioPeriodo
                    {
                        Anio = anio,
                        DocenteId = docenteId,
                        PeriodoId = periodoId,
                        EstatusId = true,
                        DocenteEstudio = new DocenteEstudio
                        {
                            Carrera = carrera,
                            DocenteId = docenteId,
                            EstatusId = 1,
                            Fecha = DateTime.Now,
                            Hora = DateTime.Now.TimeOfDay,
                            Institucion = institucion,
                            OfertaEducativaTipoId = oFertaTipo,                            
                            UsuarioId = UsuarioId,
                            DocumentoTipoId=DocumentoTipoId
                        }
                    });

                    db.SaveChanges();                                        

                    return db.DocenteEstudio.Local.FirstOrDefault().EstudioId;
                }
                catch
                {
                    return -1;   
                }
            }
        }
    }
}
