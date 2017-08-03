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
                    DTOPeriodo PeriodoActual = TraerPeriodoActSig().FirstOrDefault();
                    return db.Docente
                                                            .Select(a =>
                                                                new DTODocenteActualizar
                                                                {
                                                                    DocenteId = a.DocenteId,
                                                                    Materno = a.Materno,
                                                                    Nombre = a.Nombre,
                                                                    Paterno = a.Paterno,
                                                                    ListaEstudios = a.DocenteEstudioPeriodo
                                                                                    .Where(le =>
                                                                                            (le.Anio == PeriodoActual.Anio && le.PeriodoId == PeriodoActual.PeriodoId))
                                                                                            .Select(b => new DTODocenteEstudioPeriodo
                                                                                            {
                                                                                                Anio = b.Anio,
                                                                                                PeriodoId = b.PeriodoId,
                                                                                                EstudioId = b.EstudioId,
                                                                                                Periodo= new DTOPeriodo
                                                                                                {
                                                                                                    Descripcion=b.Periodo.Descripcion
                                                                                                },
                                                                                                EstudioDocente = new DTODocenteEstudio
                                                                                                {
                                                                                                    Carrera = b.DocenteEstudio.Carrera,
                                                                                                    Cedula = b.DocenteEstudio.Cedula,
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
                                                                                                    Titulo = b.DocenteEstudio.Titulo,
                                                                                                    UsuarioId = b.DocenteEstudio.UsuarioId,
                                                                                                    Documento = new DTODocenteEstudioDocumento
                                                                                                    {
                                                                                                        DocumentoUrl = b.DocenteEstudio.DocenteEstudioDocumento.FirstOrDefault().DocumentoUrl
                                                                                                    }
                                                                                                }
                                                                                            })
                                                                                            .ToList(),
                                                                    CursosDocente = a.DocenteCurso
                                                                                        .Where(le =>
                                                                                            (le.Anio == PeriodoActual.Anio && le.PeriodoId == PeriodoActual.PeriodoId))
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
                                                                                                Periodo= new DTOPeriodo
                                                                                                {
                                                                                                    Descripcion=b.Periodo.Descripcion
                                                                                                }
                                                                                            }).ToList()
                                                                }).ToList();
                }
                catch { return null; }
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
                        UsuarioId = usuarioId
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
            using(UniversidadEntities db =new UniversidadEntities())
            {
                DTOPeriodo PeriodoActual = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);
                Subperiodo subActual = db.Subperiodo.Where(sp => sp.MesId == DateTime.Now.Month).FirstOrDefault();
                DTOPeriodo PeriodoSiguiente = new DTOPeriodo
                {
                    Anio = PeriodoActual.PeriodoId == 3 ? PeriodoActual.Anio + 1 : PeriodoActual.Anio,
                    PeriodoId = PeriodoActual.PeriodoId == 3 ? 1 : PeriodoActual.PeriodoId + 1,
                };
                if (subActual.SubperiodoId == 4)
                { PeriodoActual = BLLPeriodoPortal.ConsultarPeriodo(PeriodoSiguiente.Anio, PeriodoSiguiente.PeriodoId); }


                return new List<DTOPeriodo>
                {
                    PeriodoActual
                };
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

        public static int GuardarFormacionAcademica(int docenteId, string institucion, int oFertaTipo, string carrera, bool cedula, bool titulo, int UsuarioId, int anio, int periodoId)
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
                        DocenteEstudio = new DocenteEstudio
                        {
                            Carrera = carrera,
                            Cedula = cedula,
                            DocenteId = docenteId,
                            EstatusId = 1,
                            Fecha = DateTime.Now,
                            Hora = DateTime.Now.TimeOfDay,
                            Institucion = institucion,
                            OfertaEducativaTipoId = oFertaTipo,
                            Titulo = titulo,
                            UsuarioId = UsuarioId
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
