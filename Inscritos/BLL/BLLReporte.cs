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
    public class BLLReporte
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");

        public static List<DTOReporteAlumnoOferta> ObtenerReporteAlumnoOferta()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                
                return (db.AlumnoInscrito.Where(a => a.OfertaEducativa.Descripcion != "Desconocida").Select(
                   b => new DTOReporteAlumnoOferta
                   {
                       alumnoId = b.AlumnoId,
                       nombreAlumno = b.Alumno.Paterno + " " + b.Alumno.Materno + " " + b.Alumno.Nombre,
                       oferta = b.OfertaEducativa.Descripcion,
                       ultimoAnio = b.FechaInscripcion.Year.ToString()
                   }).OrderBy(d => d.alumnoId).ToList());

            }
        }//ObtenerReporteAlumnoOferta()

        public static DTOFIltros CargarCuatrimestre()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var per = db.Periodo.Select(a => new DTOCuatrimestre
                {
                    anio = a.Anio,
                    periodoId = a.PeriodoId,
                    descripcion = a.Descripcion,
                    fechaInicial1 = a.FechaInicial,
                    fechaFinal1 = a.FechaFinal,
                }
                       ).Where(b => b.fechaInicial1 <= DateTime.Today).OrderByDescending(c => c.anio).Take(3).ToList();

                per = per.Select(
                   a => new DTOCuatrimestre
                   {
                       anio = a.anio,
                       periodoId = a.periodoId,
                       descripcion = a.descripcion,
                       fechaInicial = a.fechaInicial1.ToString("dd/MM/yyyy", Cultura),
                       fechaFinal = a.fechaFinal1.ToString("dd/MM/yyyy", Cultura)
                   }
                   ).ToList();

                var ofe = db.OfertaEducativa.Where(w => w.OfertaEducativaTipoId != 4 && w.Descripcion != "Desconocida").OrderByDescending(g => g.OfertaEducativaTipoId).Select(o =>
                    new DTOOfertaEducativa1
                    {
                        ofertaEducativaId = o.OfertaEducativaId,
                        descripcion = o.Descripcion
                    }
                    ).ToList();

                var filtro = new DTOFIltros { item1 = per, item2 = ofe };

                return (filtro);
            }
        }

        public static List<DTOReporteBecasCuatrimestre> CargaReporteBecaCuatrimestre(int anio, int periodo)
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {
                var AlumnoInscrito = db.spAlumnoInscritoCompleto(anio, periodo).ToList();

                var AlumnosPagoPlanNull = AlumnoInscrito.Where(a => a.PagoPlanId != 0).Select(b => b.AlumnoId).ToList();

                var becas = (from a in db.AlumnoDescuento
                             join b in db.Alumno on a.AlumnoId equals b.AlumnoId
                             join c in db.OfertaEducativa on a.OfertaEducativaId equals c.OfertaEducativaId
                             join d in db.Usuario on a.UsuarioId equals d.UsuarioId
                             join e in db.Descuento on a.DescuentoId equals e.DescuentoId
                             join g in db.AlumnoDetalle on a.AlumnoId equals g.AlumnoId
                             where a.Anio == anio
                             && a.PeriodoId == periodo
                             && a.PagoConceptoId == 800
                             && a.EstatusId == 2
                             && AlumnosPagoPlanNull.Contains(a.AlumnoId)
                             orderby a.AlumnoId

                             select new DTOReporteBecasCuatrimestre
                             {
                                 alumnoId = a.AlumnoId,
                                 nombreAlumno = b.Paterno + " " + b.Materno + " " + b.Nombre,
                                 especialidad = c.Descripcion,
                                 especialidadId = c.OfertaEducativaId,
                                 becaDescuento = e.Descripcion,
                                 porcentajeDescuento = a.Monto + "%",
                                 comentario = a.Comentario,
                                 fechaGeneracion1 = a.FechaGeneracion,
                                 horaGeneracion = a.HoraGeneracion.ToString(),
                                 usuarioAplico = d.Nombre + " " + d.Paterno + " " + d.Materno

                             }
                        ).ToList();



                becas = becas.Select(a =>
                     new DTOReporteBecasCuatrimestre
                     {
                         alumnoId = a.alumnoId,
                         nombreAlumno = a.nombreAlumno,
                         especialidad = a.especialidad,
                         especialidadId = a.especialidadId,
                         becaDescuento = a.becaDescuento,
                         porcentajeDescuento = a.porcentajeDescuento,
                         comentario = a.comentario,
                         fechaGeneracion = a.fechaGeneracion1.ToString("dd/MM/yyyy", Cultura),
                         fechaGeneracion1 = a.fechaGeneracion1,
                         horaGeneracion = a.horaGeneracion,
                         usuarioAplico = a.usuarioAplico
                     }).ToList();

                return (becas);

            }

        }//CargaReporteBecaCuatrimestre

        public static List<DTOReporteInscrito> CargaReporteInscrito(int anio, int periodo)
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {

                db.Configuration.LazyLoadingEnabled = true;

                var pagoconcepto = new int[] { 304, 320, 15 };
                var estatus = new int[] { 1, 4, 14 };

                var AlumnoInscrito = db.spAlumnoInscritoCompleto(anio, periodo).ToList();

                var listFechas = db.AlumnoInscritoBitacora.Where(k => anio == k.Anio && periodo == k.PeriodoId).ToList();
                List<DTOReporteInscrito> ins = new List<DTOReporteInscrito>();
                List<DTOReporteInscrito> temp = new List<DTOReporteInscrito>();
                #region alumnoInscrito
                if (anio < 2017 || (anio == 2017 && periodo == 1))
                {
                    var temp0 = (from a in AlumnoInscrito
                                 join b in db.Alumno on a.AlumnoId equals b.AlumnoId
                                 join c in db.Usuario on a.UsuarioId equals c.UsuarioId
                                 join d in db.OfertaEducativa on a.OfertaEducativaId equals d.OfertaEducativaId 
                                 where a.Anio == anio
                                 && a.PeriodoId == periodo
                                 && a.EstatusId == 1
                                 && d.OfertaEducativaTipoId != 4
                                 select new DTOReporteInscrito
                                 {
                                     alumnoId = a.AlumnoId,
                                     nombreAlumno = b.Paterno + " " + b.Materno + " " + b.Nombre,
                                     especialidad = d.Descripcion,
                                     especialidadId = a.OfertaEducativaId,
                                     fechaInscripcion1 = listFechas.Where(w=> w.AlumnoId==a.AlumnoId && w.OfertaEducativaId == a.OfertaEducativaId).OrderBy(k => k.FechaInscripcion).FirstOrDefault()?.FechaInscripcion ?? a.FechaInscripcion,
                                     porcentajeDescuento = a.Descuento,
                                     tipoAlumno = a.TipoAlumno,
                                     esEmpresa = a.EsEmpresa == true ? "SI" : "NO",
                                     usuarioAplico = c.Paterno + " " + c.Materno + " " + c.Nombre
                                 }
                                              ).ToList();


                    ///alumnos con  materias sueltas
                    var temp1 = (from a in db.Pago
                                 join c in db.Usuario on a.UsuarioId equals c.UsuarioId
                                 join d in db.Alumno on a.AlumnoId equals d.AlumnoId
                                 where pagoconcepto.Contains(a.Cuota1.PagoConceptoId)
                                 && a.Anio == anio
                                 && a.PeriodoId == periodo
                                 && estatus.Contains(a.EstatusId)
                                 && (db.Pago.Where(o =>
                                              o.AlumnoId == a.AlumnoId
                                              && o.Cuota1.PagoConceptoId == 800
                                              && o.Anio == anio
                                              && o.PeriodoId == periodo
                                              && o.EstatusId != 2).ToList().Count) == 0
                                 select new DTOReporteInscrito
                                 {
                                     alumnoId = a.AlumnoId,
                                     nombreAlumno = d.Paterno + " " + d.Materno + " " + d.Nombre,
                                     especialidad = a.OfertaEducativa.Descripcion,
                                     especialidadId = a.OfertaEducativaId,
                                     fechaInscripcion1 = a.FechaGeneracion,
                                     porcentajeDescuento = "0.00%",
                                     tipoAlumno = db.AlumnoInscritoBitacora.Where(f =>
                                                                                    (f.Anio < a.Anio || (f.Anio == a.Anio && f.PeriodoId < a.PeriodoId)) && f.OfertaEducativaId == a.OfertaEducativaId
                                                                                  && f.AlumnoId == a.AlumnoId).FirstOrDefault() == null ? "Nuevo Ingreso" : "Reinscrito ",
                                     esEmpresa = a.EsEmpresa == true ? "SI" : "NO",
                                     usuarioAplico = c.Nombre + " " + c.Paterno + " " + c.Materno
                                 }
                                   ).Distinct().ToList();
                    ins.AddRange(temp0);
                    ins.AddRange(temp1);
                }
                else
                {
                    var temp0 = (from a in AlumnoInscrito
                                 join b in db.Usuario on a.UsuarioId equals b.UsuarioId
                                 join c in db.Alumno on a.AlumnoId equals c.AlumnoId
                                 join d in db.OfertaEducativa on a.OfertaEducativaId equals d.OfertaEducativaId 
                                 where a.Anio == anio
                                 && a.PeriodoId == periodo
                                 && a.EstatusId == 1
                                 && d.OfertaEducativaTipoId != 4
                                 select new DTOReporteInscrito
                                 {
                                     alumnoId = a.AlumnoId,
                                     nombreAlumno = c.Paterno + " " + c.Materno + " " + c.Nombre,
                                     especialidad = d.Descripcion,
                                     especialidadId = a.OfertaEducativaId,
                                     fechaInscripcion1 = listFechas.Where(w => w.AlumnoId == a.AlumnoId && w.OfertaEducativaId == a.OfertaEducativaId).OrderBy(k => k.FechaInscripcion).FirstOrDefault()?.FechaInscripcion ?? a.FechaInscripcion,
                                     porcentajeDescuento = a.Descuento,
                                     tipoAlumno = a.TipoAlumno,
                                     esEmpresa = a.EsEmpresa == true ? "SI" : "NO",
                                     usuarioAplico = b.Paterno + " " + b.Materno + " " + b.Nombre
                                 }
                                              ).ToList();
                    
                    ins.AddRange(temp0);
                }
                #endregion

                return (ins.GroupBy(c => c.alumnoId).Select(i => i.FirstOrDefault()).ToList().Select(a => new DTOReporteInscrito
                {
                    alumnoId = a.alumnoId,
                    nombreAlumno = a.nombreAlumno,
                    especialidad = a.especialidad,
                    especialidadId = a.especialidadId,
                    fechaInscripcion = a.fechaInscripcion1.ToString("dd/MM/yyyy", Cultura),
                    porcentajeDescuento = a.porcentajeDescuento,
                    tipoAlumno = a.tipoAlumno,
                    esEmpresa = a.esEmpresa,
                    usuarioAplico = a.usuarioAplico
                }).OrderBy(b => b.alumnoId).ToList());

            }//using

        }//CargaReporteInscrito

        public static List<DTOReporteBecaSep> CargaReporteBecaSep(int anio, int periodo)
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {
                var AlumnoInscrito = db.spAlumnoInscritoCompleto(anio, periodo).ToList();
                var AlumnosPagoPlanNull = AlumnoInscrito.Where(a => a.PagoPlanId != 0).Select(b => b.AlumnoId).ToList();
                List<DTOReporteBecaSep> FiltroDos = new List<DTOReporteBecaSep>();

                //Descuentos que tienen beca sep y beca comite
                List<DTOReporteBecaSep> Uno = (from a in db.AlumnoDescuento
                                               where a.Anio == anio
                                               && a.PeriodoId == periodo
                                               && a.PagoConceptoId == 800
                                               && a.EstatusId == 2
                                               && a.EsSEP
                                               && a.EsComite
                                               && AlumnosPagoPlanNull.Contains(a.AlumnoId)
                                               select new DTOReporteBecaSep
                                              {
                                                  alumnoId = a.AlumnoId,
                                                  especialidadId = a.OfertaEducativaId,
                                                  porcentajeDescuento = a.Monto + "%",
                                                  comentario = a.Comentario,
                                                  fechaGeneracion1 = a.FechaGeneracion,
                                                  horaGeneracion = a.HoraGeneracion.ToString(),
                                                  usuarioAplicoId = a.UsuarioId,
                                                  alumnoDescuentoId = a.AlumnoDescuentoId,
                                                  esSEP = a.EsSEP,
                                                  esComite = a.EsComite
                                              }).ToList();


                List<DTOReporteBecaSep> Tres = (from a in db.AlumnoDescuentoBitacora
                                                where a.Anio == anio
                                               && a.PeriodoId == periodo
                                               && a.PagoConceptoId == 800
                                               && a.EsSEP
                                               && !a.EsComite
                                                select new DTOReporteBecaSep
                                               {
                                                   alumnoId = a.AlumnoId,
                                                   especialidadId = a.OfertaEducativaId,
                                                   porcentajeDescuento = a.Monto + "%",
                                                   comentario = a.Comentario,
                                                   fechaGeneracion1 = a.FechaGeneracion,
                                                   horaGeneracion = a.HoraGeneracion.ToString(),
                                                   usuarioAplicoId = a.UsuarioId,
                                                   alumnoDescuentoId = a.AlumnoDescuentoId,
                                                   esSEP = a.EsSEP,
                                                   esComite = a.EsComite
                                               }).ToList();

                List<DTOReporteBecaSep> Dos = new List<DTOReporteBecaSep>();


                Dos.AddRange(Uno);
                Dos.AddRange(Tres);
                Dos.Reverse();

                Dos.ForEach(n =>
                {
                    int cuenta = FiltroDos.Where(s => s.alumnoDescuentoId == n.alumnoDescuentoId).Count();

                    if (cuenta > 0)
                    {
                        DTOReporteBecaSep LoQueHay = FiltroDos.Where(s => s.alumnoDescuentoId == n.alumnoDescuentoId && s.esSEP && !s.esComite).FirstOrDefault();

                        if (LoQueHay != null)
                            if ((LoQueHay.fechaGeneracion1 <= n.fechaGeneracion1) && n.esSEP && !n.esComite)
                            {
                                LoQueHay.borrar = true;
                                FiltroDos.Add(new DTOReporteBecaSep
                                {
                                    alumnoId = n.alumnoId,
                                    especialidadId = n.especialidadId,
                                    porcentajeDescuento = n.porcentajeDescuento,
                                    comentario = n.comentario,
                                    fechaGeneracion1 = n.fechaGeneracion1,
                                    horaGeneracion = n.horaGeneracion,
                                    usuarioAplicoId = n.usuarioAplicoId,
                                    alumnoDescuentoId = n.alumnoDescuentoId,
                                    esSEP = n.esSEP,
                                    esComite = n.esComite,
                                    borrar = false
                                });
                            }
                    }

                    else
                    {
                        FiltroDos.Add(new DTOReporteBecaSep
                        {
                            alumnoId = n.alumnoId,
                            especialidadId = n.especialidadId,
                            porcentajeDescuento = n.porcentajeDescuento,
                            comentario = n.comentario,
                            fechaGeneracion1 = n.fechaGeneracion1,
                            horaGeneracion = n.horaGeneracion,
                            usuarioAplicoId = n.usuarioAplicoId,
                            alumnoDescuentoId = n.alumnoDescuentoId,
                            esSEP = n.esSEP,
                            esComite = n.esComite,
                            borrar = false
                        });
                    }
                });

                FiltroDos.RemoveAll(n => n.borrar);

                var Aux = new List<DTOReporteBecaSep>(FiltroDos);

                FiltroDos.ForEach(n =>
                {
                    var cuenta = Uno.Where(s => s.alumnoDescuentoId == n.alumnoDescuentoId).FirstOrDefault();

                    if (cuenta == null)

                        Aux.Remove(n);
                });


                List<DTOReporteBecaSep> Cuatro = (from a in db.AlumnoDescuento
                                                  where a.Anio == anio
                                                  && a.PeriodoId == periodo
                                                  && a.PagoConceptoId == 800
                                                  && AlumnosPagoPlanNull.Contains(a.AlumnoId)
                                                  && a.EsSEP
                                                  && !a.EsComite
                                                  select new DTOReporteBecaSep
                                                 {
                                                     alumnoId = a.AlumnoId,
                                                     especialidadId = a.OfertaEducativaId,
                                                     porcentajeDescuento = a.Monto + "%",
                                                     comentario = a.Comentario,
                                                     fechaGeneracion1 = a.FechaGeneracion,
                                                     horaGeneracion = a.HoraGeneracion.ToString(),
                                                     usuarioAplicoId = a.UsuarioId,
                                                     alumnoDescuentoId = a.AlumnoDescuentoId,
                                                     esSEP = a.EsSEP,
                                                     esComite = a.EsComite
                                                 }).ToList();


                Aux.AddRange(Cuatro);

                return (from a in Aux
                        join b in db.Alumno on a.alumnoId equals b.AlumnoId
                        join c in db.OfertaEducativa on a.especialidadId equals c.OfertaEducativaId
                        join d in db.Usuario on a.usuarioAplicoId equals d.UsuarioId
                        orderby a.alumnoId
                        select
                        new DTOReporteBecaSep
                        {
                            alumnoId = a.alumnoId,
                            nombreAlumno = b.Paterno + " " + b.Materno + " " + b.Nombre,
                            especialidad = c.Descripcion,
                            especialidadId = a.especialidadId,
                            porcentajeDescuento = a.porcentajeDescuento,
                            comentario = a.comentario,
                            fechaGeneracion = a.fechaGeneracion1.ToString("dd/MM/yyyy", Cultura),
                            horaGeneracion = a.horaGeneracion,
                            usuarioAplico = d.Nombre + " " + d.Paterno + " " + d.Materno
                        }
                     ).ToList();


            }


        }//CargaReporteBecaSep

        public static List<DTOReporteInegi> CargaReporteIneg(int anio, int periodo)
        {

            
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var AlumnoInscrito = db.spAlumnoInscritoCompleto(anio, periodo);
               
                db.Configuration.LazyLoadingEnabled = true;

                var pagoconcepto = new int[] { 304, 320, 15 };
                var estatus = new int[] { 1, 4, 14 };
                List<DTOReporteInegi> ins = new List<DTOReporteInegi>();
                //#region alumnoInscrito
                if (anio < 2017 || (anio == 2017 && periodo == 1))
                {

                    var temp = (from a in AlumnoInscrito
                                join b in db.Alumno on a.AlumnoId equals b.AlumnoId
                                join c in db.OfertaEducativa on a.OfertaEducativaId equals c.OfertaEducativaId
                                join d in db.AlumnoDetalle on a.AlumnoId equals d.AlumnoId
                                where a.Anio == anio
                                && a.PeriodoId == periodo
                                && a.EstatusId == 1
                                && c.OfertaEducativaTipoId != 4
                                select new DTOReporteInegi
                                {
                                    alumnoId = a.AlumnoId,
                                    nombreAlumno = b.Paterno + " " + b.Materno + " " + b.Nombre,
                                    ciclo = a.Anio + "-" + a.PeriodoId,
                                    especialidad = c.Descripcion,
                                    especialidadId = a.OfertaEducativaId,
                                    sexo = d.Genero.Descripcion,
                                    fechaNacimiento1 = d.FechaNacimiento,
                                    lugarNacimiento = d.EntidadFederativa.Descripcion,
                                    Cuatrimestre=""
                                }
                            ).ToList();

                    var temp1 = (from a in db.Pago
                                 join c in db.Alumno on a.AlumnoId equals c.AlumnoId
                                 join d in db.AlumnoDetalle on a.AlumnoId equals d.AlumnoId
                                 where pagoconcepto.Contains(a.Cuota1.PagoConceptoId)
                                 && a.Anio == anio
                                 && a.PeriodoId == periodo
                                 && estatus.Contains(a.EstatusId)
                                 && (db.Pago.Where(o =>
                                              o.AlumnoId == a.AlumnoId
                                              && o.Cuota1.PagoConceptoId == 802
                                              && o.Anio == anio
                                              && o.PeriodoId == periodo
                                              && o.EstatusId != 2).ToList().Count) == 0
                                 select new DTOReporteInegi
                                 {
                                     alumnoId = a.AlumnoId,
                                     nombreAlumno = c.Paterno + " " + c.Materno + " " + c.Nombre,
                                     ciclo = a.Anio + "-" + a.PeriodoId,
                                     especialidad = a.OfertaEducativa.Descripcion,
                                     especialidadId = a.OfertaEducativaId,
                                     sexo = d.Genero.Descripcion,
                                     fechaNacimiento1 = d.FechaNacimiento,
                                     lugarNacimiento = d.EntidadFederativa.Descripcion,
                                     Cuatrimestre = ""
                                 }
                                   ).Distinct().ToList();



                    ins.AddRange(temp);
                    ins.AddRange(temp1);
                }
                else
                {
                    

                    var temp0 = (from a in AlumnoInscrito
                                 join b in db.Alumno on a.AlumnoId equals b.AlumnoId
                                 join c in db.OfertaEducativa on a.OfertaEducativaId equals c.OfertaEducativaId
                                 join d in db.AlumnoDetalle on a.AlumnoId equals d.AlumnoId
                                 join e in db.AlumnoCuatrimestre on new {a.AlumnoId,a.OfertaEducativaId,a.Anio,a.PeriodoId } equals 
                                                                    new {e.AlumnoId, e.OfertaEducativaId, e.Anio, e.PeriodoId }
                                 where a.Anio == anio
                                 && a.PeriodoId == periodo
                                 && a.EstatusId == 1
                                 && c.OfertaEducativaTipoId != 4
                                 select new DTOReporteInegi
                                 {
                                     alumnoId = a.AlumnoId,
                                     nombreAlumno = b.Paterno + " " + b.Materno + " " + b.Nombre,
                                     ciclo = a.Anio + "-" + a.PeriodoId,
                                     especialidad = c.Descripcion,
                                     especialidadId = a.OfertaEducativaId,
                                     sexo = d.Genero.Descripcion,
                                     fechaNacimiento1 = d.FechaNacimiento,
                                     lugarNacimiento = d.EntidadFederativa.Descripcion,
                                     tipoAlumno = a.TipoAlumno,
                                     Cuatrimestre = e.Cuatrimestre + " Cuatrimestre"
                                 }
                                              ).ToList();

                    ins.AddRange(temp0);

                }

                DateTime ahora = DateTime.Now;

                return (ins.GroupBy(c => c.alumnoId).Select(i => i.FirstOrDefault()).ToList().Select(a => new DTOReporteInegi
                {
                    alumnoId = a.alumnoId,
                    nombreAlumno = a.nombreAlumno,
                    ciclo = a.ciclo,
                    especialidad = a.especialidad,
                    especialidadId = a.especialidadId,
                    sexo = a.sexo,
                    edad = (ahora.Month < a.fechaNacimiento1.Month || (ahora.Month == a.fechaNacimiento1.Month && ahora.Day < a.fechaNacimiento1.Day)) ? (ahora.Year - a.fechaNacimiento1.Year) - 1 + " Años" : ahora.Year - a.fechaNacimiento1.Year + " Años",
                    fechaNacimiento = a.fechaNacimiento1.ToString("dd/MM/yyyy", Cultura),
                    lugarNacimiento = a.lugarNacimiento,
                    Cuatrimestre = a.Cuatrimestre
                }).OrderBy(b => b.alumnoId).ToList());


            }//using


        }//CargaReporteIneg

        public static List<DTOReporteAlumnoReferencia> CargaReporteAlumnoReferencia(int anio, int periodo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var pagosid = new int[] { 802, 304, 320, 800, 15 };
                var pagocolegiatura = new int[] { 800 };
                var pagomateria = new int[] { 304, 320 };
                var estatus = new int[] { 1, 4, 14 };
                //Alumnos que estan inscritos y  tienen referencias

                var AlumnosInscrito = db.spAlumnoInscritoCompleto(anio, periodo).ToList();


                var referencias = (from a in db.Pago
                                   join c in db.Cuota on a.CuotaId equals c.CuotaId
                                   where a.Anio == anio && a.PeriodoId == periodo && pagosid.Contains(c.PagoConceptoId) && estatus.Contains(a.EstatusId)
                                   orderby a.AlumnoId
                                   group a by new { c.PagoConceptoId, a.AlumnoId, a.OfertaEducativaId } into grp
                                   select new DTOReporteAlumnoReferencia1
                                   {
                                       alumnoId = grp.Key.AlumnoId,
                                       ofertaId = grp.Key.OfertaEducativaId,
                                       pagoConcepto = grp.Key.PagoConceptoId,
                                       suma = grp.Count()
                                   }).ToList();


                // var alumno = referencias.Where(a => a.alumnoId == 7697).ToList();

                List<DTOReporteAlumnoReferencia> union = new List<DTOReporteAlumnoReferencia>();

                union.AddRange(referencias.GroupBy(a => new { a.alumnoId, a.ofertaId }).Select(a => new DTOReporteAlumnoReferencia
                {
                    alumnoId = a.Key.alumnoId,
                    especialidadId = a.Key.ofertaId
                }));

                var calificacionesantecedente = db.CalificacionesAntecedente.Where(v => v.Anio == anio
                                                                                          && v.PeriodoId == periodo).ToList();
                ///ins
                union = union.Select(a => new DTOReporteAlumnoReferencia
                {
                    alumnoId = a.alumnoId,
                    especialidadId = a.especialidadId,
                    inscripcion = "" + referencias.Where(b => b.alumnoId == a.alumnoId && b.ofertaId == a.especialidadId && b.pagoConcepto == 802).FirstOrDefault()?.suma ,
                    colegiatura = "" + referencias.Where(b => b.alumnoId == a.alumnoId && b.ofertaId == a.especialidadId && b.pagoConcepto == 800).FirstOrDefault()?.suma ,
                    materiaSuelta = "" + referencias.Where(b => b.alumnoId == a.alumnoId && b.ofertaId == a.especialidadId && pagomateria.Contains(b.pagoConcepto)).FirstOrDefault()?.suma,
                    asesoriaEspecial = "" + referencias.Where(b => b.alumnoId == a.alumnoId && b.ofertaId == a.especialidadId && b.pagoConcepto == 15).FirstOrDefault()?.suma ,
                    noMaterias = calificacionesantecedente.Where(v => v.AlumnoId == a.alumnoId && v.OfertaEducativaId == a.especialidadId).FirstOrDefault()?.NoMaterias ?? 0,
                    calificacionMaterias = "" + calificacionesantecedente.Where(v => v.AlumnoId == a.alumnoId && v.OfertaEducativaId == a.especialidadId).FirstOrDefault()?.CalificacionMaterias ?? "",
                    noBaja = calificacionesantecedente.Where(v => v.AlumnoId == a.alumnoId && v.OfertaEducativaId == a.especialidadId).FirstOrDefault()?.NoBajas ?? 0,
                    bajaMaterias = "" + calificacionesantecedente.Where(v => v.AlumnoId == a.alumnoId && v.OfertaEducativaId == a.especialidadId).FirstOrDefault()?.BajaMaterias ?? ""
                }
                ).ToList();


                //Alumnos que estan inscritos y no tienen referencias
                var parte2 = AlumnosInscrito.Where(a => a.Anio == anio && a.PeriodoId == periodo && union.Where(f => f.alumnoId == a.AlumnoId && f.especialidadId == a.OfertaEducativaId).ToList().Count == 0)
                                             .Select(d => new DTOReporteAlumnoReferencia
                                             {
                                                 alumnoId = d.AlumnoId,
                                                 especialidadId = d.OfertaEducativaId,
                                                 inscripcion = "0",
                                                 colegiatura = "0",
                                                 materiaSuelta = "0",
                                                 asesoriaEspecial = "0",
                                                 noMaterias = calificacionesantecedente.Where(v => v.AlumnoId == d.AlumnoId && v.OfertaEducativaId == d.OfertaEducativaId).FirstOrDefault()?.NoMaterias ?? 0,
                                                 calificacionMaterias = "" + calificacionesantecedente.Where(v => v.AlumnoId == d.AlumnoId && v.OfertaEducativaId == d.OfertaEducativaId).FirstOrDefault()?.CalificacionMaterias ?? "",
                                                 noBaja = calificacionesantecedente.Where(v => v.AlumnoId == d.AlumnoId && v.OfertaEducativaId == d.OfertaEducativaId).FirstOrDefault()?.NoBajas ?? 0,
                                                 bajaMaterias = "" + calificacionesantecedente.Where(v => v.AlumnoId == d.AlumnoId && v.OfertaEducativaId == d.OfertaEducativaId).FirstOrDefault()?.BajaMaterias ?? ""
                                             }
                                             ).ToList();

            

                union.AddRange(parte2);



                //union

                union = (from a in union
                         join b in db.Alumno on a.alumnoId equals b.AlumnoId
                         join c in db.OfertaEducativa on a.especialidadId equals c.OfertaEducativaId
                         select new DTOReporteAlumnoReferencia
                         {
                             alumnoId = a.alumnoId,
                             nombreAlumno = b.Paterno + " " + b.Materno + " " + b.Nombre,
                             especialidad = c.Descripcion,
                             inscripcion = a.inscripcion == "" ? "0" : a.inscripcion,
                             colegiatura = a.colegiatura == "" ? "0" : a.colegiatura,
                             materiaSuelta = a.materiaSuelta == "" ? "0" : a.materiaSuelta,
                             asesoriaEspecial = a.asesoriaEspecial == "" ? "0" : a.asesoriaEspecial,
                             noMaterias = a.noMaterias,
                             calificacionMaterias = a.calificacionMaterias,
                             noBaja = a.noBaja,
                             bajaMaterias = a.bajaMaterias,
                             tipo = a.noMaterias > 0 && a.noBaja > 0 && a.noBaja < a.noMaterias ? 1
                             : a.noMaterias == 0 && a.noBaja > 0 ? 2
                             : int.Parse(a.inscripcion + a.colegiatura + a.materiaSuelta + a.asesoriaEspecial) > 0 && a.noMaterias == 0 && a.noBaja == 0 ? 3 
                             : int.Parse(a.inscripcion + a.colegiatura + a.materiaSuelta + a.asesoriaEspecial) == 0 && a.noMaterias > 0 || a.noBaja > 0 ? 4 : 0,
                         }).Distinct().ToList();

                return  union;


            }//using


        }//CargaReporteIneg

        public static DTOVoBo ReporteVoBo(int anio, int periodoid, int usuarioid)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    var todos = db.Alumno.Where(a => a.AlumnoRevision.Where(ar => ar.Anio == anio
                                                                        && ar.PeriodoId == periodoid
                                                                        && ar.OfertaEducativa.OfertaEducativaTipoId != 4).Count() > 0

                                              || a.AlumnoInscrito.Where(ai => ai.Anio == anio
                                                                        && ai.PeriodoId == periodoid
                                                                        && ai.OfertaEducativa.OfertaEducativaTipoId != 4
                                                                        && db.AlumnoInscritoBitacora.Where(aib => aib.AlumnoId == ai.AlumnoId
                                                                                                           && aib.OfertaEducativaId == ai.OfertaEducativaId
                                                                                                           && (aib.Anio != anio || (aib.Anio == anio && aib.PeriodoId != periodoid))).Count() > 0
                                                                        ).Count() > 0)
                                     .Select(b => new DTOAlumnosVoBo
                                     {
                                         AlumnoId = b.AlumnoId,
                                         Nombre = b.Paterno + " " + b.Materno + " " + b.Nombre,
                                         AlumnoInscrito = b.AlumnoInscrito.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault(),
                                         AlumnoInscritoBitacora = b.AlumnoInscritoBitacora.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault(),
                                         AlumnoRevision = b.AlumnoRevision.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault()
                                     }).ToList();


                    var todos1 = todos.Select(td => new DTOReporteVoBo
                    {
                        AlumnoId = td.AlumnoId,
                        Nombre = td.Nombre,
                        OfertaEducativaid = td.AlumnoInscrito?.OfertaEducativaId ?? td.AlumnoRevision.OfertaEducativaId,
                        OfertaEducativa = td.AlumnoInscrito?.OfertaEducativa.Descripcion ?? td.AlumnoRevision.OfertaEducativa.Descripcion,
                        Inscrito = td.AlumnoInscrito != null ? "Si" : "No",
                        FechaInscrito = td.AlumnoInscritoBitacora?.FechaInscripcion.ToString("dd/MM/yyyy", Cultura) ?? td.AlumnoInscrito?.FechaInscripcion.ToString("dd/MM/yyyy", Cultura) ?? "-",
                        HoraInscrito = td.AlumnoInscritoBitacora?.HoraInscripcion.ToString() ?? td.AlumnoInscrito?.HoraInscripcion.ToString() ?? "-",
                        UsuarioInscribio = td.AlumnoInscritoBitacora != null ? td.AlumnoInscritoBitacora.Usuario.Paterno + " " + td.AlumnoInscritoBitacora.Usuario.Materno + " " + td.AlumnoInscritoBitacora.Usuario.Nombre
                           : td.AlumnoInscrito != null ? td.AlumnoInscrito.Usuario.Paterno + " " + td.AlumnoInscrito.Usuario.Materno + " " + td.AlumnoInscrito.Usuario.Nombre : "-",
                        FechaVoBo = td.AlumnoRevision?.FechaRevision.ToString("dd/MM/yyyy", Cultura) ?? "-",
                        HoraVoBo = td.AlumnoRevision?.HoraRevision.ToString() ?? "-",
                        InscripcionCompleta = td.AlumnoRevision?.InscripcionCompleta == true ? "Si" : td.AlumnoRevision?.InscripcionCompleta == false ? "No" : "-",
                        Asesorias = td.AlumnoRevision?.AsesoriaEspecial.ToString() ?? "-",
                        Materias = td.AlumnoRevision?.AdelantoMateria.ToString() ?? "-",
                        UsuarioVoBo = td.AlumnoRevision != null ? td.AlumnoRevision.Usuario.Paterno + " " + td.AlumnoRevision.Usuario.Materno + " " + td.AlumnoRevision.Usuario.Nombre : "-",
                    }).ToList();

                    bool sw = false;  
                    int usuariotipo = db.Usuario.Where(a => a.UsuarioId == usuarioid).FirstOrDefault().UsuarioTipoId;
                    if (usuariotipo == 12 || usuariotipo == 10)
                    {
                        sw = true;
                    }

                    var VoVo = new DTOVoBo { lstVoBo = todos1, Sw = sw } ;
                    return VoVo;
                }
                catch (Exception)
                {

                    return null;
                }
            }
        }
        

    }
}
