using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using System.Globalization;
using Utilities;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.Entity.Core.Objects;

namespace BLL
{
    public class BLLReportePortal
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");

        public static List<DTOReporteAlumnoOferta> ObtenerReporteAlumnoOferta()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                try
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
                catch (Exception)
                {
                    return null;
                }


            }
        }//ObtenerReporteAlumnoOferta()

        public static object CargarReporteSaldosOfertaEducativa(int ofertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                List<SP_ReporteSaldoAlumno_Result> alumnoSaldo0 = db.SP_ReporteSaldoAlumno().ToList();

                var periodos0 = alumnoSaldo0.Select(a => a.Descripcion).Distinct().ToList();

                var periodos = db.Periodo.Where(a => periodos0.Select(w => w).Contains(a.Descripcion))
                                                  .OrderByDescending(b => b.Anio)
                                                  .Select(c => new
                                                  {
                                                      c.Descripcion,
                                                      DescripcionCorta = c.Descripcion,
                                                      c.Anio,
                                                      c.PeriodoId
                                                  }).ToList();

                var alumnos = alumnoSaldo0.Where(a => a.OfertaEducativaId == ofertaEducativaId)
                                .Select(a => new { a.AlumnoId, a.Nombre }).Distinct().ToList();


                List<DTOSaldoAlumno> SaldoAlumno = alumnos.Select(a => new DTOSaldoAlumno
                {
                    AlumnoId = a.AlumnoId,
                    Nombre = a.Nombre,
                    Detalle = periodos.Select(c => new DTOSaldosDetalle
                    {
                        AlumnoId = (int)a.AlumnoId,
                        Periodo = c.Descripcion,
                        Saldo = String.Format("{0:C}", alumnoSaldo0.Where(e => e.AlumnoId == a.AlumnoId && e.Descripcion == c.Descripcion && e.OfertaEducativaId==ofertaEducativaId).FirstOrDefault()?.Saldo ?? 0)
                    }).ToList(),
                    SaldoTotal = String.Format("{0:C}", alumnoSaldo0.Where(e => e.AlumnoId == a.AlumnoId && e.OfertaEducativaId == ofertaEducativaId).Select(f => f.Saldo).ToList().Sum())
                }).ToList();

                return new
                {
                    Alumnos = SaldoAlumno,
                    Estatus = true
                };
            }
        }
        public static DTOCuatrimestre CargarCuatrimestreHistorico()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<DTOPeriodo2> periodo = db.AlumnoInscrito.Where(d => (d.Anio == 2016 && d.PeriodoId > 1) || d.Anio > 2016)
                                                                 .GroupBy(b => new { b.Anio, b.PeriodoId, b.Periodo })
                                                                 .Select(a => new DTOPeriodo2
                                                                 {
                                                                     anio = a.Key.Anio,
                                                                     periodoId = a.Key.PeriodoId,
                                                                     descripcion = a.Key.Periodo.Descripcion
                                                                 })
                                                                  .OrderBy(c => new { c.anio, c.periodoId }).ToList();

                    List<DTOOfertaEducativa2> ofertaEducativa = db.OfertaEducativa.Where(w => w.OfertaEducativaTipoId != 4
                                                                                           && w.Descripcion != "Desconocida"
                                                                                           && w.SucursalId != 4
                                                                                           && w.OfertaEducativaId != 48).OrderBy(g => g.OfertaEducativaTipoId).Select(o =>
                        new DTOOfertaEducativa2
                        {
                            ofertaEducativaId = o.OfertaEducativaId,
                            descripcion = o.Descripcion
                        }
                        ).ToList();

                    return (new DTOCuatrimestre { periodos = periodo, ofertas = ofertaEducativa });
                }
                catch (Exception)
                {
                    return null;
                }

            }
        }

        public static List<DTOReporteCarteraVencida> CarteraVencida(int anio, int periodoId, string fechaInicial, string fechaFinal)
        {
            DateTime fini = DateTime.ParseExact(fechaInicial, "dd/MM/yyyy", Cultura);
            DateTime ffin = DateTime.ParseExact(fechaFinal, "dd/MM/yyyy", Cultura);

            fechaInicial = (fini.Day < 10 ? "0" + fini.Day : "" + fini.Day) + "/" +
                (fini.Month < 10 ? "0" + fini.Month : "" + fini.Month) + "/" + fini.Year;

            fechaFinal = (ffin.Day < 10 ? "0" + ffin.Day : "" + ffin.Day) + "/" +
                (ffin.Month < 10 ? "0" + ffin.Month : "" + ffin.Month) + "/" + ffin.Year;


            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {

                    List<DTOReporteCarteraVencida> listaresult = db.spReporteCarteraVencida(anio, periodoId, fini, ffin).ToList()
                                    .Select(a =>
                                    new DTOReporteCarteraVencida
                                    {
                                        Alumno = a.Alumno,
                                        Concepto = a.Concepto,
                                        FechaPago = a.FechaPago,
                                        _Pago = a.Pago,
                                        _Restante = a.Restante, 
                                        Tipo_de_pago = a.Tipo_de_pago
                                    }).ToList();


                    return listaresult;
                }
                catch { return null; }
            }
        }

        public static DTOCuatrimestre CargarCuatrimestre()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<DTOPeriodo2> periodo = db.Periodo.Select(a => new DTOPeriodo2
                    {
                        anio = a.Anio,
                        periodoId = a.PeriodoId,
                        descripcion = a.Descripcion,
                        fechaInicial1 = a.FechaInicial,
                        fechaFinal1 = a.FechaFinal,
                    }
                ).Where(b => b.fechaInicial1 <= DateTime.Today).OrderByDescending(c => c.anio).Take(2).ToList();


                    int Anio = periodo[0].periodoId == 3 ? periodo[0].anio + 1 : periodo[0].anio;
                    int PeriodoId = periodo[0].periodoId == 3 ? 1 : periodo[0].periodoId + 1;

                    periodo.Add(db.Periodo.Select(a => new DTOPeriodo2
                    {
                        anio = a.Anio,
                        periodoId = a.PeriodoId,
                        descripcion = a.Descripcion,
                        fechaInicial1 = a.FechaInicial,
                        fechaFinal1 = a.FechaFinal,
                    }
                    ).Where(b => b.anio == Anio && b.periodoId == PeriodoId).FirstOrDefault());


                    periodo = periodo.Select(
                       a => new DTOPeriodo2
                       {
                           anio = a.anio,
                           periodoId = a.periodoId,
                           descripcion = a.descripcion,
                           fechaInicial = a.fechaInicial1.ToString("dd/MM/yyyy", Cultura),
                           fechaFinal = a.fechaFinal1.ToString("dd/MM/yyyy", Cultura)
                       }
                       ).OrderByDescending(b => b.anio).ThenByDescending(c => c.periodoId).ToList();

                    List<DTOOfertaEducativa2> ofertaEducativa = db.OfertaEducativa.Where(w => w.OfertaEducativaTipoId != 4
                                                                                           && w.Descripcion != "Desconocida"
                                                                                           && w.SucursalId != 4
                                                                                           && w.OfertaEducativaId != 48).OrderBy(g => g.OfertaEducativaTipoId).Select(o =>
                        new DTOOfertaEducativa2
                        {
                            ofertaEducativaId = o.OfertaEducativaId,
                            descripcion = o.Descripcion
                        }
                        ).ToList();

                    return (new DTOCuatrimestre { periodos = periodo, ofertas = ofertaEducativa });
                }
                catch (Exception)
                {
                    return null;
                }

            }
        }

        public static DTOReporteBecas CargarReporteBecas(int anio, int periodo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<spReporteBecasDetalle> detalle = db.spReporteBecasDetalle(anio, periodo).ToList();

                List<spReporteBecasConcentrado> concentrado = db.spReporteBecasConcentrado(anio, periodo).ToList();
                
                List<DTOReporteBecasDetalle> detalle1 = detalle.Select(a => new DTOReporteBecasDetalle
                {
                    AlumnoId = a.AlumnoId,
                    OfertaEducativaId = a.OfertaEducativaId,
                    Descripcion = a.Descripcion,
                    CostoIns = String.Format("{0:C}",  a.CostoIns),
                    AnticipadoIns = String.Format("{0:C}", a.AnticipadoIns),
                    AnticipadoInsPor = String.Format("{0:P2}", a.AnticipadoInsPor),
                    BecaIns = String.Format("{0:C}", a.BecaIns),
                    BecaInsPor = String.Format("{0:P2}", a.BecaInsPor),
                    DesTotalIns = String.Format("{0:C}", a.DesTotalIns),
                    DesTotalInsPor = String.Format("{0:P2}", a.DesTotalInsPor),
                    TotalIns = String.Format("{0:C}", a.TotalIns),
                    CostoCol = String.Format("{0:C}", a.CostoCol),
                    AnticipadoCol = String.Format("{0:C}", a.AnticipadoCol),
                    AnticipadoColPor = String.Format("{0:P2}", a.AnticipadoColPor),
                    BecaCol = String.Format("{0:C}", a.BecaCol),
                    BecaColPor = String.Format("{0:P2}", a.BecaColPor),
                    BecaDeportiva = String.Format("{0:C}", a.BecaDeportiva),
                    BecaDeportivaPor = String.Format("{0:P2}", a.BecaDeportivaPor),
                    PromoCasa = String.Format("{0:C}", a.PromoCasa),
                    PromoCasaPor = String.Format("{0:P2}", a.PromoCasaPor),
                    DesTotalCol = String.Format("{0:C}", a.DesTotalCol),
                    DesTotalColPor = String.Format("{0:P2}", a.DesTotalColPor),
                    TotalCol = String.Format("{0:C}", a.TotalCol),
                    EsEmpresa = a.EsEmpresa
                }).ToList();

                List<DTOReporteBecasConcentrado> concentrado1 = concentrado.Select(a => new DTOReporteBecasConcentrado
                {
                    Descripcion = a.Descripcion,
                    TotalAlumnos = a.TotalAlumnos,
                    AlumnosConBeca = a.AlumnosConBeca,
                    CargosInscripcion = String.Format("{0:C}", a.CargosInscripcion),
                    PromedioAnticipadoPor = String.Format("{0:P2}", a.PromedioAnticipadoPor),
                    PromedioAnticipado_ = String.Format("{0:C}", a.PromedioAnticipado_),
                    PromedioBecaInscripcionPor = String.Format("{0:P2}", a.PromedioBecaInscripcionPor),
                    PromedioBecaInscripcion_ = String.Format("{0:C}", a.PromedioBecaInscripcion_),
                    TotalDescuentoInscripcion = String.Format("{0:C}", a.TotalDescuentoInscripcion),
                    CargosColegiatura = String.Format("{0:C}", a.CargosColegiatura),
                    PromedioAnticipadoPorColegiatura = String.Format("{0:P2}", a.PromedioAnticipadoPorColegiatura),
                    PromedioAnticipado_Colegiatura = String.Format("{0:C}", a.PromedioAnticipado_Colegiatura),
                    BecaPromedioPor = String.Format("{0:P2}", a.BecaPromedioPor),
                    BecaPromedio_ = String.Format("{0:C}", a.BecaPromedio_),
                    BecaDeportivaPor = String.Format("{0:P2}", a.BecaDeportivaPor),
                    BecaDeportivaPromedio_ = String.Format("{0:C}", a.BecaDeportivaPromedio_),
                    PromoCasaPor = String.Format("{0:P2}", a.PromoCasaPor),
                    PromoCasa_ = String.Format("{0:C}", a.PromoCasa_),
                    TotalDescuentoColegiatura = String.Format("{0:C}", a.TotalDescuentoColegiatura)

                }).ToList();

                #region CALCULOS

                List<DTOBecasCalculos> calculos1 = new List<DTOBecasCalculos>();
                List<DTOBecasCalculos> calculos2 = new List<DTOBecasCalculos>();

                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:C}", detalle.Select(a => a.CostoIns).Sum()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:C}", detalle.Select(a => a.AnticipadoIns).Sum()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:P4}", detalle.Select(a => a.AnticipadoInsPor).Average()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:C}", detalle.Select(a => a.BecaIns).Sum()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:P4}", detalle.Select(a => a.BecaInsPor).Average()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:C}", detalle.Select(a => a.DesTotalIns).Sum()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:P4}", detalle.Select(a => a.DesTotalInsPor).Average()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:C}", detalle.Select(a => a.TotalIns).Sum()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:C}", detalle.Select(a => a.CostoCol).Sum()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:C}", detalle.Select(a => a.AnticipadoCol).Sum()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:P4}", detalle.Select(a => a.AnticipadoColPor).Average()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:C}", detalle.Select(a => a.BecaCol).Sum()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:P4}", detalle.Select(a => a.BecaColPor).Average()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:C}", detalle.Select(a => a.BecaDeportiva).Sum()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:P4}", detalle.Select(a => a.BecaDeportivaPor).Average()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:C}", detalle.Select(a => a.PromoCasa).Sum()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:P4}", detalle.Select(a => a.PromoCasaPor).Average()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:C}", detalle.Select(a => a.DesTotalCol).Sum()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:P4}", detalle.Select(a => a.DesTotalColPor).Average()) });
                calculos1.Add(new DTOBecasCalculos { valor = String.Format("{0:C}", detalle.Select(a => a.TotalCol).Sum()) });

                ////////////////////////////////////
                calculos2.Add(new DTOBecasCalculos { valor2 = "Total:" });
                calculos2.Add(new DTOBecasCalculos { valor2 = concentrado.Select(a => a.TotalAlumnos).Sum().ToString() });
                calculos2.Add(new DTOBecasCalculos { valor2 = concentrado.Select(a => a.AlumnosConBeca).Sum().ToString() });
                calculos2.Add(new DTOBecasCalculos { valor2 = String.Format("{0:C}", concentrado.Select(a => a.CargosInscripcion).Sum()) });
                calculos2.Add(new DTOBecasCalculos { });
                calculos2.Add(new DTOBecasCalculos { valor2 = String.Format("{0:C}", concentrado.Select(a => a.PromedioAnticipado_).Sum()) });
                calculos2.Add(new DTOBecasCalculos { });
                calculos2.Add(new DTOBecasCalculos { valor2 = String.Format("{0:C}", concentrado.Select(a => a.PromedioBecaInscripcion_).Sum()) });
                calculos2.Add(new DTOBecasCalculos { valor2 = String.Format("{0:C}", concentrado.Select(a => a.TotalDescuentoInscripcion).Sum()) });
                calculos2.Add(new DTOBecasCalculos { valor2 = String.Format("{0:C}", concentrado.Select(a => a.CargosColegiatura).Sum()) });
                calculos2.Add(new DTOBecasCalculos { });
                calculos2.Add(new DTOBecasCalculos { valor2 = String.Format("{0:C}", concentrado.Select(a => a.PromedioAnticipado_Colegiatura).Sum()) });
                calculos2.Add(new DTOBecasCalculos { });
                calculos2.Add(new DTOBecasCalculos { valor2 = String.Format("{0:C}", concentrado.Select(a => a.BecaPromedio_).Sum()) });
                calculos2.Add(new DTOBecasCalculos { });
                calculos2.Add(new DTOBecasCalculos { valor2 = String.Format("{0:C}", concentrado.Select(a => a.BecaDeportivaPromedio_).Sum()) });
                calculos2.Add(new DTOBecasCalculos { });
                calculos2.Add(new DTOBecasCalculos { valor2 = String.Format("{0:C}", concentrado.Select(a => a.PromoCasa_).Sum()) });
                calculos2.Add(new DTOBecasCalculos { valor2 = String.Format("{0:C}", concentrado.Select(a => a.TotalDescuentoColegiatura).Sum()) });


                #endregion

                DTOReporteBecas reporteBecas = new DTOReporteBecas();

                reporteBecas.Detalle = detalle1;
                reporteBecas.Concentrado = concentrado1;
                reporteBecas.Calculos1 = calculos1;
                reporteBecas.Calculos2 = calculos2;
                return reporteBecas;

            }
        }

        public static object CargarReporteSaldos()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                List<SP_ReporteSaldoAlumno_Result> alumnoSaldo0 = db.SP_ReporteSaldoAlumno().ToList();

                var periodos0 = alumnoSaldo0.Select(a => a.Descripcion).Distinct().ToList();

                var periodos = db.Periodo.Where(a => periodos0.Select(w => w).Contains(a.Descripcion))
                                                  .OrderByDescending(b => b.Anio)
                                                  .Select(c => new
                                                  {
                                                      c.Descripcion,
                                                      DescripcionCorta = c.Descripcion,
                                                      c.Anio,
                                                      c.PeriodoId
                                                  }).ToList();

                var alumnos = alumnoSaldo0.Select(a => new { a.AlumnoId, a.Nombre }).Distinct().ToList();


                List<DTOSaldoAlumno> SaldoAlumno = alumnos.Select(a => new DTOSaldoAlumno
                {
                    AlumnoId = a.AlumnoId,
                    Nombre = a.Nombre,
                    Detalle = periodos.Select(c => new DTOSaldosDetalle
                    {
                        AlumnoId = (int)a.AlumnoId,
                        Periodo = c.Descripcion,
                        Saldo = String.Format("{0:C}", alumnoSaldo0.Where(e => e.AlumnoId == a.AlumnoId && e.Descripcion == c.Descripcion).FirstOrDefault()?.Saldo ?? 0)
                    }).ToList(),
                    SaldoTotal = String.Format("{0:C}", alumnoSaldo0.Where(e => e.AlumnoId == a.AlumnoId).Select(f => f.Saldo).ToList().Sum())
                }).ToList();

                List<DTOSaldosOfertas> saldoOfertas = alumnoSaldo0.GroupBy(a => new { a.OfertaEducativaId })
                                      .Select(b => new DTOSaldosOfertas
                                      {
                                          OfertaEducativaId = (int)b.Key.OfertaEducativaId,
                                          Descripcion = db.OfertaEducativa.Where(c => c.OfertaEducativaId == b.Key.OfertaEducativaId).FirstOrDefault().Descripcion,
                                          Periodos = periodos.Select(f => new DTOSaldoPeriodos
                                          {
                                              Periodo = f.Descripcion,
                                              Saldo = String.Format("{0:C}", (decimal)alumnoSaldo0.Where(g => g.OfertaEducativaId == b.Key.OfertaEducativaId
                                                                             && g.Descripcion == f.Descripcion)
                                                                           .Select(h => h.Saldo).ToList().Sum())
                                          }).ToList(),
                                          SaldoTotal = String.Format("{0:C}", alumnoSaldo0.Where(p => p.OfertaEducativaId == b.Key.OfertaEducativaId).Select(l => l.Saldo).Sum())

                                      }).OrderBy(j => j.OfertaEducativaId).ToList();

                saldoOfertas.Add(new DTOSaldosOfertas
                {
                    Descripcion = "Total",
                    Periodos = alumnoSaldo0.GroupBy(e => new { e.Descripcion })
                                                     .Select(f => new DTOSaldoPeriodos
                                                     {
                                                         Periodo = f.Key.Descripcion,
                                                         Saldo = String.Format("{0:C}", (decimal)alumnoSaldo0.Where(g => g.Descripcion == f.Key.Descripcion)
                                                                                                             .Select(h => h.Saldo).ToList().Sum())
                                                     }).ToList(),
                    SaldoTotal = String.Format("{0:C}", alumnoSaldo0.Select(l => l.Saldo).Sum())
                });

                var ofertas = alumnoSaldo0.Select(a => a.OfertaEducativaId).Distinct().ToList();

                var TipoOfertas = db.OfertaEducativa
                                    .Where(a => ofertas.Contains(a.OfertaEducativaId))
                                    .Select(b => new
                                    {
                                        b.OfertaEducativaTipoId,
                                        b.OfertaEducativaTipo.Descripcion
                                    })
                                    .Distinct()
                                    .ToList()
                                    .Select(a => new
                                    {
                                        a.OfertaEducativaTipoId,
                                        a.Descripcion,
                                        OfertasEducativas = db.OfertaEducativa
                                                            .Where(b => ofertas.Contains(b.OfertaEducativaId) && b.OfertaEducativaTipoId == a.OfertaEducativaTipoId)
                                                            .Select(Ab => new
                                                            {
                                                                Ab.OfertaEducativaId,
                                                                Ab.Descripcion
                                                            })
                                                            .ToList()
                                    })
                                    .ToList();
                TipoOfertas.Insert(0, new
                {
                    OfertaEducativaTipoId = -1,
                    Descripcion = "Todas",
                    OfertasEducativas = new[]
                    {
                        new {
                            OfertaEducativaId =-1,
                            Descripcion="Todas"
                        }

                    }
                    .ToList()
                });

                //DTOReporteSaldos Reporte = new DTOReporteSaldos();
                //Reporte.Periodos = periodos;
                //Reporte.Alumnos = SaldoAlumno;
                //Reporte.Ofertas = saldoOfertas;

                return new
                {
                    Periodos = periodos,
                    Alumnos = SaldoAlumno,
                    Ofertas = saldoOfertas,
                    TipoOfertas
                };
            }
        }

        public static List<DTOSaldosDetalle> CargarReporteSaldoDetalle(int alumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTOSaldosDetalle> detalle = db.spSaldoAlumno(alumnoId, true).Select(a => new DTOSaldosDetalle
                {
                    AlumnoId = a.AlumnoId,
                    Periodo = db.Periodo.Where(c => c.Anio == a.Anio && c.PeriodoId == a.PeriodoId).FirstOrDefault().Descripcion,
                    Saldo = String.Format("{0:C}", a.Saldo)
                }).ToList();

                return detalle;
            }
        }

        public static List<DTOReporteBecasCuatrimestre> CargaReporteBecaCuatrimestre(int anio, int periodo)
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<AlumnoInscritoCompleto> alumnoInscrito = db.spAlumnoInscritoCompleto(anio, periodo).ToList();

                    List<int> alumnosPagoPlanNull = alumnoInscrito.Where(a => a.PagoPlanId != 0).Select(b => b.AlumnoId).ToList();

                    List<DTOReporteBecasCuatrimestre> becas = (from a in db.AlumnoDescuento
                                                               join b in db.Alumno on a.AlumnoId equals b.AlumnoId
                                                               join c in db.OfertaEducativa on a.OfertaEducativaId equals c.OfertaEducativaId
                                                               join d in db.Usuario on a.UsuarioId equals d.UsuarioId
                                                               join e in db.Descuento on a.DescuentoId equals e.DescuentoId
                                                               join g in db.AlumnoDetalle on a.AlumnoId equals g.AlumnoId
                                                               where a.Anio == anio
                                                               && a.PeriodoId == periodo
                                                               && a.PagoConceptoId == 800
                                                               && a.EstatusId == 2
                                                               && alumnosPagoPlanNull.Contains(a.AlumnoId)
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
                catch (Exception)
                {

                    return null;
                }


            }

        }//CargaReporteBecaCuatrimestre

        public static List<DTOReporteInscrito> CargaReporteInscrito(int anio, int periodo)
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {

                try
                {
                    int[] pagoConcepto = new int[] { 304, 320, 15 };
                    int[] estatus = new int[] { 1, 4, 14 };

                    List<AlumnoInscritoCompleto> alumnoInscrito = db.spAlumnoInscritoCompleto(anio, periodo).ToList();

                    List<AlumnoInscritoBitacora> alumnoInscritoBitacora = db.AlumnoInscritoBitacora.Where(k => anio == k.Anio && periodo == k.PeriodoId).ToList();
                    List<DTOReporteInscrito> inscritos = new List<DTOReporteInscrito>();
                    #region alumnoInscrito

                    List<DTOReporteInscrito> alumnoInscrito2 = (from a in alumnoInscrito
                                                                join b in db.Usuario on a.UsuarioId equals b.UsuarioId
                                                                join c in db.Alumno on a.AlumnoId equals c.AlumnoId
                                                                join d in db.OfertaEducativa on a.OfertaEducativaId equals d.OfertaEducativaId
                                                                where a.Anio == anio
                                                                && a.PeriodoId == periodo
                                                                && a.EstatusId != 3
                                                                && d.OfertaEducativaTipoId != 4
                                                                select new DTOReporteInscrito
                                                                {
                                                                    alumnoId = a.AlumnoId,
                                                                    nombreAlumno = c.Paterno + " " + c.Materno + " " + c.Nombre,
                                                                    especialidad = d.Descripcion,
                                                                    especialidadId = a.OfertaEducativaId,
                                                                    fechaInscripcion1 = alumnoInscritoBitacora.Where(w => w.AlumnoId == a.AlumnoId && w.OfertaEducativaId == a.OfertaEducativaId).OrderBy(k => k.FechaInscripcion).FirstOrDefault()?.FechaInscripcion ?? a.FechaInscripcion,
                                                                    porcentajeDescuento = a.Descuento,
                                                                    tipoAlumno = a.TipoAlumno,
                                                                    esEmpresa = a.EsEmpresa == true ? "SI" : "NO",
                                                                    usuarioAplico = b.Paterno + " " + b.Materno + " " + b.Nombre
                                                                }
                                              ).ToList();

                    inscritos.AddRange(alumnoInscrito2);

                    #endregion

                    return (inscritos.GroupBy(c => c.alumnoId).Select(i => i.FirstOrDefault()).ToList().Select(a => new DTOReporteInscrito
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
                }
                catch (Exception)
                {

                    return null;
                }



            }//using

        }//CargaReporteInscrito

        public static List<DTOReporteBecaSep> CargaReporteBecaSep(int anio, int periodo)
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {

                List<AlumnoInscritoCompleto> alumnoInscrito = db.spAlumnoInscritoCompleto(anio, periodo).ToList();
                List<int> alumnosPagoPlanNull = alumnoInscrito.Where(a => a.PagoPlanId != 0).Select(b => b.AlumnoId).ToList();
                List<DTOReporteBecaSep> FiltroDos = new List<DTOReporteBecaSep>();

                //Descuentos que tienen beca sep y beca comite
                List<DTOReporteBecaSep> Uno = (from a in db.AlumnoDescuento
                                               where a.Anio == anio
                                               && a.PeriodoId == periodo
                                               && a.PagoConceptoId == 800
                                               && a.EstatusId == 2
                                               && a.EsSEP
                                               && a.EsComite
                                               && alumnosPagoPlanNull.Contains(a.AlumnoId)
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
                                                  && alumnosPagoPlanNull.Contains(a.AlumnoId)
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
                try
                {
                    List<AlumnoInscritoCompleto> alumnoInscrito = db.spAlumnoInscritoCompleto(anio, periodo).ToList();

                    int[] pagoConcepto = new int[] { 304, 320, 15 };
                    int[] estatus = new int[] { 1, 4, 14 };
                    List<DTOReporteInegi> inscritoInegi = new List<DTOReporteInegi>();
                    //#region alumnoInscrito

                    List<DTOReporteInegi> alumnoInegi = (from a in alumnoInscrito
                                                         join b in db.Alumno on a.AlumnoId equals b.AlumnoId
                                                         join c in db.OfertaEducativa on a.OfertaEducativaId equals c.OfertaEducativaId
                                                         join d in db.AlumnoDetalle on a.AlumnoId equals d.AlumnoId
                                                         join e in db.AlumnoCuatrimestre on new { a.AlumnoId, a.OfertaEducativaId, a.Anio, a.PeriodoId } equals
                                                                                           new { e.AlumnoId, e.OfertaEducativaId, e.Anio, e.PeriodoId }
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
                                                             lugarEstudio = d.Alumno.AlumnoAntecedente.FirstOrDefault()?.PaisId == 146 ?
                                                             d.Alumno.AlumnoAntecedente.FirstOrDefault()?.EntidadFederativa.Descripcion :
                                                             d.Alumno.AlumnoAntecedente.FirstOrDefault()?.Pais.Descripcion,
                                                             tipoAlumno = a.TipoAlumno,
                                                             Cuatrimestre = e.Cuatrimestre + " Cuatrimestre"
                                                             //Cuatrimestre = db.AlumnoCuatrimestre.Where(e=> a.AlumnoId == e.AlumnoId && a.OfertaEducativaId == e.OfertaEducativaId && a.Anio == e.Anio && a.PeriodoId == e.PeriodoId).FirstOrDefault()?.Cuatrimestre + " Cuatrimestre" ?? ""
                                                         }
                                              ).ToList();

                    inscritoInegi.AddRange(alumnoInegi);



                    DateTime ahora = DateTime.Now;

                    return (inscritoInegi.GroupBy(c => c.alumnoId).Select(i => i.FirstOrDefault()).ToList().Select(a => new DTOReporteInegi
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
                        lugarEstudio = a.lugarEstudio,
                        Cuatrimestre = a.Cuatrimestre
                    }).OrderBy(b => b.alumnoId).ToList());

                }
                catch (Exception)
                {

                    return null;
                }



            }//using


        }//CargaReporteIneg

        public static List<DTOReporteAlumnoReferencia> CargaReporteAlumnoReferencia(int anio, int periodo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                try
                {
                    int[] pagosId = new int[] { 802, 304, 320, 800, 15 };
                    int[] pagoColegiatura = new int[] { 800 };
                    int[] pagoMateria = new int[] { 304, 320 };
                    int[] estatus = new int[] { 1, 4, 14 };
                    //Alumnos que estan inscritos y  tienen referencias

                    List<AlumnoInscritoCompleto> alumnosInscrito = db.spAlumnoInscritoCompleto(anio, periodo).ToList();


                    List<DTOReporteAlumnoReferencia1> referencias = (from a in db.Pago
                                                                     join c in db.Cuota on a.CuotaId equals c.CuotaId
                                                                     where a.Anio == anio && a.PeriodoId == periodo && pagosId.Contains(c.PagoConceptoId) && estatus.Contains(a.EstatusId)
                                                                     orderby a.AlumnoId
                                                                     group a by new { c.PagoConceptoId, a.AlumnoId, a.OfertaEducativaId } into grp
                                                                     select new DTOReporteAlumnoReferencia1
                                                                     {
                                                                         alumnoId = grp.Key.AlumnoId,
                                                                         ofertaId = grp.Key.OfertaEducativaId,
                                                                         pagoConcepto = grp.Key.PagoConceptoId,
                                                                         suma = grp.Count()
                                                                     }).ToList();



                    List<DTOReporteAlumnoReferencia> alumnoRefencias = new List<DTOReporteAlumnoReferencia>();

                    alumnoRefencias.AddRange(referencias.GroupBy(a => new { a.alumnoId, a.ofertaId }).Select(a => new DTOReporteAlumnoReferencia
                    {
                        alumnoId = a.Key.alumnoId,
                        especialidadId = a.Key.ofertaId
                    }));

                    List<CalificacionesAntecedente> calificacionesAntecedente = db.CalificacionesAntecedente.Where(v => v.Anio == anio
                                                                                              && v.PeriodoId == periodo).ToList();

                    alumnoRefencias = alumnoRefencias.Select(a => new DTOReporteAlumnoReferencia
                    {
                        alumnoId = a.alumnoId,
                        especialidadId = a.especialidadId,
                        inscripcion = "" + referencias.Where(b => b.alumnoId == a.alumnoId && b.ofertaId == a.especialidadId && b.pagoConcepto == 802).FirstOrDefault()?.suma,
                        colegiatura = "" + referencias.Where(b => b.alumnoId == a.alumnoId && b.ofertaId == a.especialidadId && b.pagoConcepto == 800).FirstOrDefault()?.suma,
                        materiaSuelta = "" + referencias.Where(b => b.alumnoId == a.alumnoId && b.ofertaId == a.especialidadId && pagoMateria.Contains(b.pagoConcepto)).FirstOrDefault()?.suma,
                        asesoriaEspecial = "" + referencias.Where(b => b.alumnoId == a.alumnoId && b.ofertaId == a.especialidadId && b.pagoConcepto == 15).FirstOrDefault()?.suma,
                        noMaterias = calificacionesAntecedente.Where(v => v.AlumnoId == a.alumnoId && v.OfertaEducativaId == a.especialidadId).FirstOrDefault()?.NoMaterias ?? 0,
                        calificacionMaterias = "" + calificacionesAntecedente.Where(v => v.AlumnoId == a.alumnoId && v.OfertaEducativaId == a.especialidadId).FirstOrDefault()?.CalificacionMaterias ?? "",
                        noBaja = calificacionesAntecedente.Where(v => v.AlumnoId == a.alumnoId && v.OfertaEducativaId == a.especialidadId).FirstOrDefault()?.NoBajas ?? 0,
                        bajaMaterias = "" + calificacionesAntecedente.Where(v => v.AlumnoId == a.alumnoId && v.OfertaEducativaId == a.especialidadId).FirstOrDefault()?.BajaMaterias ?? ""
                    }
                    ).ToList();


                    //Alumnos que estan inscritos y no tienen referencias
                    var alumnoInscrito2 = alumnosInscrito.Where(a => a.Anio == anio && a.PeriodoId == periodo && alumnoRefencias.Where(f => f.alumnoId == a.AlumnoId && f.especialidadId == a.OfertaEducativaId).ToList().Count == 0)
                                                 .Select(d => new DTOReporteAlumnoReferencia
                                                 {
                                                     alumnoId = d.AlumnoId,
                                                     especialidadId = d.OfertaEducativaId,
                                                     inscripcion = "0",
                                                     colegiatura = "0",
                                                     materiaSuelta = "0",
                                                     asesoriaEspecial = "0",
                                                     noMaterias = calificacionesAntecedente.Where(v => v.AlumnoId == d.AlumnoId && v.OfertaEducativaId == d.OfertaEducativaId).FirstOrDefault()?.NoMaterias ?? 0,
                                                     calificacionMaterias = "" + calificacionesAntecedente.Where(v => v.AlumnoId == d.AlumnoId && v.OfertaEducativaId == d.OfertaEducativaId).FirstOrDefault()?.CalificacionMaterias ?? "",
                                                     noBaja = calificacionesAntecedente.Where(v => v.AlumnoId == d.AlumnoId && v.OfertaEducativaId == d.OfertaEducativaId).FirstOrDefault()?.NoBajas ?? 0,
                                                     bajaMaterias = "" + calificacionesAntecedente.Where(v => v.AlumnoId == d.AlumnoId && v.OfertaEducativaId == d.OfertaEducativaId).FirstOrDefault()?.BajaMaterias ?? ""
                                                 }
                                                 ).ToList();



                    alumnoRefencias.AddRange(alumnoInscrito2);



                    //consentrar todas las referencias

                    alumnoRefencias = (from a in alumnoRefencias
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

                    return alumnoRefencias;

                }
                catch (Exception)
                {
                    return null;
                }

            }//using


        }//CargaReporteIneg

        public static DTOVoBo CargarReporteVoBo(int anio, int periodoid, int usuarioid)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<int> alumnoId = db.spAlumnoInscritoCompleto(anio, periodoid).Where(a => a.TipoAlumno == "Reinscrito").Select(b => b.AlumnoId).ToList();
                    alumnoId.AddRange(db.AlumnoRevision.Where(a => a.Anio == anio && a.PeriodoId == periodoid && a.OfertaEducativa.OfertaEducativaTipoId != 4).Select(b => b.AlumnoId).ToList());
                    alumnoId = alumnoId.Distinct().ToList();


                    List<DTOAlumnosVoBo1> alumnoRevision = db.Alumno.Where(a => alumnoId.Contains(a.AlumnoId))

                                                                     .Select(b => new DTOAlumnosVoBo1
                                                                     {
                                                                         AlumnoId = b.AlumnoId,
                                                                         Nombre = b.Paterno + " " + b.Materno + " " + b.Nombre,
                                                                         AlumnoInscritoBitacora = b.AlumnoInscritoBitacora.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault(),
                                                                         AlumnoRevision = b.AlumnoRevision.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault(),
                                                                         Email = b.AlumnoDetalle.Email
                                                                     }).ToList();

                    List<AlumnoInscritoCompleto> alumnoInscrito = db.spAlumnoInscritoCompleto(anio, periodoid).Where(a => a.TipoAlumno == "Reinscrito").ToList();

                    alumnoRevision.ForEach(n =>
                    {
                        n.AlumnoInscrito = alumnoInscrito.Where(c => c.AlumnoId == n.AlumnoId)?.FirstOrDefault();
                    });


                    List<DTOReporteVoBo> alumnoVoBo = alumnoRevision.Select(td => new DTOReporteVoBo
                    {
                        AlumnoId = td.AlumnoId,
                        Nombre = td.Nombre,
                        OfertaEducativaid = td.AlumnoInscrito?.OfertaEducativaId ?? td.AlumnoRevision.OfertaEducativaId,
                        OfertaEducativa = td.AlumnoInscrito?.OfertaEducativa ?? td.AlumnoRevision.OfertaEducativa.Descripcion,
                        Inscrito = td.AlumnoInscrito != null ? "Si" : "No",
                        FechaInscrito = td.AlumnoInscritoBitacora?.FechaInscripcion.ToString("dd/MM/yyyy", Cultura) ?? td.AlumnoInscrito?.FechaInscripcion.ToString("dd/MM/yyyy", Cultura) ?? "-",
                        HoraInscrito = td.AlumnoInscritoBitacora?.HoraInscripcion.ToString() ?? td.AlumnoInscrito?.HoraInscripcion.ToString() ?? "-",
                        UsuarioInscribio = td.AlumnoInscritoBitacora != null ? td.AlumnoInscritoBitacora.Usuario.Paterno + " " + td.AlumnoInscritoBitacora.Usuario.Materno + " " + td.AlumnoInscritoBitacora.Usuario.Nombre
                           : td.AlumnoInscrito != null ? td.AlumnoInscrito.Usuario : "-",
                        FechaVoBo = td.AlumnoRevision?.FechaRevision.ToString("dd/MM/yyyy", Cultura) ?? "-",
                        HoraVoBo = td.AlumnoRevision?.HoraRevision.ToString() ?? "-",
                        InscripcionCompleta = td.AlumnoRevision?.InscripcionCompleta == true ? "Si" : td.AlumnoRevision?.InscripcionCompleta == false ? "No" : "-",
                        Asesorias = td.AlumnoRevision?.AsesoriaEspecial.ToString() ?? "-",
                        Materias = td.AlumnoRevision?.AdelantoMateria.ToString() ?? "-",
                        UsuarioVoBo = td.AlumnoRevision != null ? td.AlumnoRevision.Usuario.Paterno + " " + td.AlumnoRevision.Usuario.Materno + " " + td.AlumnoRevision.Usuario.Nombre : "-",
                        Email = td.Email
                    }).ToList();

                    bool esEscolares = false;
                    int usuarioTipo = db.Usuario.Where(a => a.UsuarioId == usuarioid).FirstOrDefault().UsuarioTipoId;
                    if (usuarioTipo == 12 || usuarioTipo == 10)
                    {
                        esEscolares = true;
                    }

                    return (new DTOVoBo { AlumnoVoBo = alumnoVoBo.Where(q=> q.OfertaEducativaid != 44 ).ToList(), EsEscolares = esEscolares });
                }
                catch (Exception)
                {

                    return null;
                }
            }
        }

        public static bool ReporteVoBoEnviarEmail(int AlumnoId, string EmailAlumno)
        {
            DTOCuentaMail cuentaEmail = BLLCuentaMail.ConsultarCuentaMail();

            try
            {
                ProcessResult respuesta = new Utilities.ProcessResult();
                string body = "";
                DTOAlumno alumno = BLLAlumnoPortal.ObtenerAlumno(AlumnoId);

                #region "HTML"
                body = "<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>" +
                            "<head>" +
                            "<meta charset='utf-8' />" +
                            "<title>Bienvenida Alumnos</title>" +
                            "<meta http-equiv='X-UA-Compatible' content='IE=edge' />" +
                            "<meta content='width=device-width, initial-scale=1.0' name='viewport' />" +
                            "<meta http-equiv='Content-type' content='text/html; charset=utf-8' />" +
                            "<meta content='' name='description' />" +
                            "<meta content='' name='author' />" +
                           "<style>" +
                                "body {" +
                                    "color: #333333;" +
                                    "font-family: 'Open Sans', sans-serif;" +
                                    "padding: 0px !important;" +
                                    "margin: 0px !important;" +
                                    "font-size: 13px;" +
                                    "direction: ltr;" +
                                "}" +
                                "body {" +
                                    "font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif;" +
                                    "font-size: 14px;" +
                                    "line-height: 1.42857143;" +
                                    "color: #333;" +
                                    "background-color: #fff;" +
                                "}" +
                                "Inherited from html html {" +
                                    "font-size: 10px;" +
                                    "-webkit-tap-highlight-color: rgba(0,0,0,0);" +
                                "}" +
                                "html {" +
                                    "font-family: sans-serif;" +
                                    "-webkit-text-size-adjust: 100%;" +
                                    "-ms-text-size-adjust: 100%;" +
                                "}" +
                                "div, input, select, textarea, span, img, table, label, td, th, p, a, button, ul, code, pre, li {" +
                                    "-webkit-border-radius: 0 !important;" +
                                    "-moz-border-radius: 0 !important;" +
                                    "border-radius: 0 !important;" +
                                "}" +
                                "* {" +
                                    "-webkit-box-sizing: border-box;" +
                                    "-moz-box-sizing: border-box;" +
                                    "box-sizing: border-box;" +
                                "}" +
                                "@media (min-width: 1200px) {" +
                                    ".container {" +
                                        "width: 1170px;" +
                                    "}" +
                                "}" +
                                "@media (min-width: 992px) {" +
                                    ".container {" +
                                        "width: 970px;" +
                                    "}" +
                                "}" +
                                "@media (min-width: 768px) {" +
                                    ".container {" +
                                        "width: 750px;" +
                                    "}" +
                                "}" +
                                ".container {" +
                                    "padding-right: 15px;" +
                                    "padding-left: 15px;" +
                                    "margin-right: auto;" +
                                    "margin-left: auto;" +
                                "}" +
                                        ".row {" +
                                            "margin-right: -15px;" +
                                            "margin-left: -15px;" +
                                        "}" +
                                        ".col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9 {" +
                                    "float: left;" +
                                "}" +
                                ".col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {" +
                                    "position: relative;" +
                                    "min-height: 1px;" +
                                    "padding-right: 15px;" +
                                    "padding-left: 15px;" +
                                "}" +
                                ".portlet.light {" +
                                    "padding: 12px 20px 15px 20px;" +
                                    "background-color: #fff;" +
                                "}" +
                                ".portlet {" +
                                    "margin-top: 0px;" +
                                    "margin-bottom: 25px;" +
                                    "padding: 0px;" +
                                    "-webkit-border-radius: 4px;" +
                                    "-moz-border-radius: 4px;" +
                                    "-ms-border-radius: 4px;" +
                                    "-o-border-radius: 4px;" +
                                    "border-radius: 4px;" +
                                "}" +
                                ".portlet.light > .portlet-title {" +
                                    "padding: 0;" +
                                    "min-height: 48px;" +
                                "}" +
                                ".portlet > .portlet-title {" +
                                    "border-bottom: 1px solid #eee;" +
                                    "padding: 0;" +
                                    "margin-bottom: 10px;" +
                                    "min-height: 41px;" +
                                    "-webkit-border-radius: 4px 4px 0 0;" +
                                    "-moz-border-radius: 4px 4px 0 0;" +
                                    "-ms-border-radius: 4px 4px 0 0;" +
                                    "-o-border-radius: 4px 4px 0 0;" +
                                    "border-radius: 4px 4px 0 0;" +
                                "}" +
                                ".portlet.light > .portlet-title > .caption {" +
                                    "color: #666;" +
                                    "padding: 10px 0;" +
                                "}" +
                                ".portlet > .portlet-title > .caption {" +
                                    "float: left;" +
                                    "display: inline-block;" +
                                    "font-size: 18px;" +
                                    "line-height: 18px;" +
                                    "padding: 10px 0;" +
                                "}" +
                                ".uppercase {" +
                                    "text-transform: uppercase !important;" +
                                "}" +
                                ".bold {" +
                                    "font-weight: 700 !important;" +
                                "}" +
                                "h2 {" +
                                    "font-size: 27px;" +
                                "}" +
                                "h3 {" +
                                    "font-size: 23px;" +
                                "}" +
                                "h4 {" +
                                    "font-size: 17px;" +
                                "}" +
                                ".h4, h4 {" +
                                    "font-size: 18px;" +
                                "}" +
                                "h1, h2, h3, h4, h5, h6 {" +
                                    "font-family: 'Open Sans', sans-serif;" +
                                    "font-weight: 300;" +
                                "}" +
                                ".h2, h2 {" +
                                    "font-size: 30px;" +
                                "}" +
                                ".h1, .h2, .h3, h1, h2, h3 {" +
                                    "margin-top: 20px;" +
                                    "margin-bottom: 10px;" +
                                "}" +
                                ".h1, .h2, .h3, .h4, .h5, .h6, h1, h2, h3, h4, h5, h6 {" +
                                    "font-family: inherit;" +
                                    "font-weight: 500;" +
                                    "line-height: 1.1;" +
                                    "color: inherit;" +
                                "}" +
                                "* {" +
                                    "-webkit-box-sizing: border-box;" +
                                    "-moz-box-sizing: border-box;" +
                                    "box-sizing: border-box;" +
                                "}" +
                                "user agent stylesheeth2 {" +
                                    "display: block;" +
                                    "font-size: 1.5em;" +
                                    "-webkit-margin-before: 0.83em;" +
                                    "-webkit-margin-after: 0.83em;" +
                                    "-webkit-margin-start: 0px;" +
                                    "-webkit-margin-end: 0px;" +
                                    "font-weight: bold;" +
                                "}" +
                                ".table {" +
                                    "width: 100%;" +
                                    "max-width: 100%;" +
                                    "margin-bottom: 20px;" +
                                "}" +
                                ".font-green-sharp {" +
                                    "color: #4DB3A2 !important;" +
                                "}" +
                                ".uppercase {" +
                                    "text-transform: uppercase !important;" +
                                "}" +
                                ".bold {" +
                                    "font-weight: 700 !important;" +
                                "}" +
                                ".font-blue {" +
                                    "color: #3598dc !important;" +
                                "}" +
                                "hr {" +
                                    "margin: 20px 0;" +
                                    "border: 0;" +
                                    "border-top: 1px solid #eee;" +
                                    "border-bottom: 0;" +
                                "}" +
                                "hr {" +
                                    "margin-top: 20px;" +
                                    "margin-bottom: 20px;" +
                                    "border: 0;" +
                                    "border-top: 1px solid #eee;" +
                                "}" +
                                "hr {" +
                                    "height: 0;" +
                                    "-webkit-box-sizing: content-box;" +
                                    "-moz-box-sizing: content-box;" +
                                    "box-sizing: content-box;" +
                                "}" +
                                "* {" +
                                    "-webkit-box-sizing: border-box;" +
                                    "-moz-box-sizing: border-box;" +
                                    "box-sizing: border-box;" +
                                "}" +
                                "user agent stylesheethr {" +
                                    "display: block;" +
                                    "-webkit-margin-before: 0.5em;" +
                                    "-webkit-margin-after: 0.5em;" +
                                    "-webkit-margin-start: auto;" +
                                    "-webkit-margin-end: auto;" +
                                    "border-style: inset;" +
                                    "border-width: 1px;" +
                                "}" +
                            "</style>" +
                            "</head>" +
                            "<body>" +

                            "<div class='page-head'>" +
    "<div class='container'>" +
        "<div class='table'>" +
            "<div class='row'>" +
                "<div class='col-md-12'>" +
                    "<div class='col-md-3'>" +
                    "</div>" +
                "</div>" +
                "<div class='col-md-12'>" +
                    "<table cellpadding='0' cellspacing='0' border='0' height='100%' width='100%' bgcolor='#f0f1f1' style='border-collapse:collapse'>" +
                        "<tbody>" +
                            "<tr>" +
                                "<td>" +
                                    "<center style='width:100%'>" +
                                        "<div style='max-width:2000px; margin-left:auto; margin-right:auto'>" +
                                            "<table cellspacing='0' cellpadding='0' border='0' align='center' bgcolor='#ffffff' width='680' style='max-width:680px; border:solid 1px #cfd1d2'>" +
                                                "<tbody>" +
                                                    "<tr><td> &nbsp;</td></tr>" +
                                                    "<tr style='padding:3%'>" +
                                                        "<td dir='ltr' bgcolor='#ffffff' ; align='center' height='100%' valign='top' width='100%' style='padding:0 3% 3% 3%; line-height:25px'>" +
                                                            "<div class='col-md-12'>" +
                                                                "<table border='0' cellspacing='0' cellpadding='0' align='center' width='100%' class='x_body-content' style='background-color:#CCCCCC; padding:3%'>" +
                                                                    "<tbody>" +
                                                                        "<tr>" +
                                                                            "<td align='center' valign='top'>" +
                                                                                "<div class='col-md-12' style='font-size:16pt; font-weight:bolder; color:#FFFFFF;  text-transform:uppercase; text-align:center'>" +
                                                                                    "<img height='113px' width='80px' src='http://108.163.172.122/portalAdministrativo/Imagenes/UYMCA-500px.png' />" +
                                                                                "</div>" +
                                                                            "</td>" +
                                                                            "<td align='center' valign='top'>" +
                                                                                "<div class='col-md-12' style='font-size:17pt; font-weight:bolder; color:#CC0000;  text-transform:uppercase; text-align:center'>" +
                                                                                    "<br>" +
                                                                                    " Aviso importante universidad YMCA " +
                                                                                    "<br>" +
                                                                                "</div>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                    "</tbody>" +
                                                                "</table>" +
                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr style='padding:3%'>" +
                                                        "<td dir='ltr' bgcolor='#ffffff' align='center' height='100%' valign='top' width='100%' style='padding:0 3% 3% 3%; line-height:25px'>" +
                                                            "<div class='col-md-12'>" +
                                                                "<table border='0' cellspacing='0' cellpadding='0' align='center' width='100%' class='x_body-content' style='background-color:#f3f3f3; padding:3%'>" +
                                                                    "<tbody>" +
                                                                        "<tr>" +
                                                                            "<td align='center' valign='top'>" +
                                                                                "<div style='color:#009966; font-size:30px; text-align:center; padding:10px'>Estimado,&nbsp; " + alumno.Nombre + " " + alumno.Paterno + " " + alumno.Materno + " </div>" +
                                                                                "<div>" +
                                                                                    "<h3 class='caption font-blue'>" +
                                                                                        "No has concluido tu proceso de inscripción, para finalizarlo es necesario que te presentes inmediatamente al área de servicios escolares." +
                                                                                    "</h3>" +
                                                                                    "<h4 class='caption font-blue-dark'>" +
                                                                                        "Atentamente: Coordinación Académica" +
                                                                                    "</h4>" +
                                                                                    "<br />" +
                                                                                    "<h4 class='caption font-blue-dark'> Universidad YMCA México. </h4>" +
                                                                                "</div>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                    "</tbody>" +
                                                                "</table>" +
                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</tbody>" +
                                            "</table>" +
                                        "</div>" +
                                    "</center>" +
                                "</td>" +
                            "</tr>" +
                        "</tbody>" +
                    "</table>" +
                "</div>" +
            "</div>" +
        "</div>" +
    "</div>" +
"</div>" +

                            "</body>" +
                            "</html>";
                #endregion

                Email.Enviar(cuentaEmail.Email, cuentaEmail.Password, cuentaEmail.DisplayName, EmailAlumno, ',', "antoniogalvan@ymcacdmex.org.mx", ';', "Aviso Portal Universidad YMCA", body, "", ',', cuentaEmail.Smtp, cuentaEmail.Puerto, cuentaEmail.SSL, true, ref respuesta);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public static bool EnviarSolicitudSep(string FechaInicial, string FechaFinal)
        {
            DTOCuentaMail cuentaEmail = BLLCuentaMail.ConsultarCuentaMail();
            
            try
            {
                ProcessResult respuesta = new Utilities.ProcessResult();
                string body = "";

                #region "HTML"
                body = "<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>" +
                            "<head>" +
    "<meta charset='utf-8' />" +
    "<title>Bienvenida Alumnos</title>" +
    "<meta http-equiv='X-UA-Compatible' content='IE=edge' />" +
    "<meta content='width=device-width, initial-scale=1.0' name='viewport' />" +
    "<meta http-equiv='Content-type' content='text/html; charset=utf-8' />" +
    "<meta content='' name='description' />" +
    "<meta content='' name='author' />" +
    "<style>" + "body {" + "color: #333333;" + "font-family: 'Open Sans', sans-serif;" + "padding: 0px !important;" + "margin: 0px !important;" + "font-size: 13px;" + "direction: ltr;" + "}" + "body {" + "font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif;" + "font-size: 14px;" + "line-height: 1.42857143;" + "color: #333;" + "background-color: #fff;" + "}" + "Inherited from html html {" + "font-size: 10px;" + "-webkit-tap-highlight-color: rgba(0,0,0,0);" + "}" + "html {" + "font-family: sans-serif;" + "-webkit-text-size-adjust: 100%;" + "-ms-text-size-adjust: 100%;" + "}" + "div, input, select, textarea, span, img, table, label, td, th, p, a, button, ul, code, pre, li {" + "-webkit-border-radius: 0 !important;" + "-moz-border-radius: 0 !important;" + "border-radius: 0 !important;" + "}" + "* {" + "-webkit-box-sizing: border-box;" + "-moz-box-sizing: border-box;" + "box-sizing: border-box;" + "}" + "@media (min-width: 1200px) {" + ".container {" + "width: 1170px;" + "}" + "}" + "@media (min-width: 992px) {" + ".container {" + "width: 970px;" + "}" + "}" + "@media (min-width: 768px) {" + ".container {" + "width: 750px;" + "}" + "}" + ".container {" + "padding-right: 15px;" + "padding-left: 15px;" + "margin-right: auto;" + "margin-left: auto;" + "}" + ".row {" + "margin-right: -15px;" + "margin-left: -15px;" + "}" + ".col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9 {" + "float: left;" + "}" + ".col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {" + "position: relative;" + "min-height: 1px;" + "padding-right: 15px;" + "padding-left: 15px;" + "}" + ".portlet.light {" + "padding: 12px 20px 15px 20px;" + "background-color: #fff;" + "}" + ".portlet {" + "margin-top: 0px;" + "margin-bottom: 25px;" + "padding: 0px;" + "-webkit-border-radius: 4px;" + "-moz-border-radius: 4px;" + "-ms-border-radius: 4px;" + "-o-border-radius: 4px;" + "border-radius: 4px;" + "}" + ".portlet.light > .portlet-title {" + "padding: 0;" + "min-height: 48px;" + "}" + ".portlet > .portlet-title {" + "border-bottom: 1px solid #eee;" + "padding: 0;" + "margin-bottom: 10px;" + "min-height: 41px;" + "-webkit-border-radius: 4px 4px 0 0;" + "-moz-border-radius: 4px 4px 0 0;" + "-ms-border-radius: 4px 4px 0 0;" + "-o-border-radius: 4px 4px 0 0;" + "border-radius: 4px 4px 0 0;" + "}" + ".portlet.light > .portlet-title > .caption {" + "color: #666;" + "padding: 10px 0;" + "}" + ".portlet > .portlet-title > .caption {" + "float: left;" + "display: inline-block;" + "font-size: 18px;" + "line-height: 18px;" + "padding: 10px 0;" + "}" + ".uppercase {" + "text-transform: uppercase !important;" + "}" + ".bold {" + "font-weight: 700 !important;" + "}" + "h2 {" + "font-size: 27px;" + "}" + "h3 {" + "font-size: 23px;" + "}" + "h4 {" + "font-size: 17px;" + "}" + ".h4, h4 {" + "font-size: 18px;" + "}" + "h1, h2, h3, h4, h5, h6 {" + "font-family: 'Open Sans', sans-serif;" + "font-weight: 300;" + "}" + ".h2, h2 {" + "font-size: 30px;" + "}" + ".h1, .h2, .h3, h1, h2, h3 {" + "margin-top: 20px;" + "margin-bottom: 10px;" + "}" + ".h1, .h2, .h3, .h4, .h5, .h6, h1, h2, h3, h4, h5, h6 {" + "font-family: inherit;" + "font-weight: 500;" + "line-height: 1.1;" + "color: inherit;" + "}" + "* {" + "-webkit-box-sizing: border-box;" + "-moz-box-sizing: border-box;" + "box-sizing: border-box;" + "}" + "user agent stylesheeth2 {" + "display: block;" + "font-size: 1.5em;" + "-webkit-margin-before: 0.83em;" + "-webkit-margin-after: 0.83em;" + "-webkit-margin-start: 0px;" + "-webkit-margin-end: 0px;" + "font-weight: bold;" + "}" + ".table {" + "width: 100%;" + "max-width: 100%;" + "margin-bottom: 20px;" + "}" + ".font-green-sharp {" + "color: #4DB3A2 !important;" + "}" + ".uppercase {" + "text-transform: uppercase !important;" + "}" + ".bold {" + "font-weight: 700 !important;" + "}" + ".font-blue {" + "color: #3598dc !important;" + "}" + "hr {" + "margin: 20px 0;" + "border: 0;" + "border-top: 1px solid #eee;" + "border-bottom: 0;" + "}" + "hr {" + "margin-top: 20px;" + "margin-bottom: 20px;" + "border: 0;" + "border-top: 1px solid #eee;" + "}" + "hr {" + "height: 0;" + "-webkit-box-sizing: content-box;" + "-moz-box-sizing: content-box;" + "box-sizing: content-box;" + "}" + "* {" + "-webkit-box-sizing: border-box;" + "-moz-box-sizing: border-box;" + "box-sizing: border-box;" + "}" + "user agent stylesheethr {" + "display: block;" + "-webkit-margin-before: 0.5em;" + "-webkit-margin-after: 0.5em;" + "-webkit-margin-start: auto;" + "-webkit-margin-end: auto;" + "border-style: inset;" + "border-width: 1px;" + "}" + "</style>" +
    "</head>" +
                            "<body>" +
    "<div class='page-head'>" +
        "<div class='container'>" +
            "<div class='table'>" +
                "<div class='row'>" +
                    "<div class='col-md-12'>" +
                        "<div class='col-md-3'>" +
                            "</div>" +
                    "<div class='col-md-12'>" +
                        "<div class='col-md-3'>" +
                            "</div>" +
                        "<div class='col-md-7 footer-gray portlet light portlet-title'>" +
                            "<h3 class='caption font-blue'> Alejandra Sandra Cruz Flores ha solicitado el reporte de los alumnos inscritos del  " + FechaInicial  + "  al " + FechaFinal + " ,Favor de anexar la columna de promedios.</h3>" +
                            "<hr />" +
                            "<h3 class='caption font-blue'>Puedes acceder a el desde el siguiente enlace.</h3>" +
                            "<a class='caption font-blue-dark' href='http://108.163.172.122/portalAdministrativo/index.html#Views/17'>YMCA</a>" +
                            " </div>" +
                        " </div>" +
                    " </div>" +
                "</div>" +
            " </div>" +
        "</div>" +
    "</body>" +
   "</html>";
                #endregion

                Email.Enviar(cuentaEmail.Email, cuentaEmail.Password, cuentaEmail.DisplayName, "ricardomunoz@ymcacdmex.org.mx", ',', "sistemas@ymcacdmex.org.mx", ';', "SOLICITUD DE REPORTE ALUMNOS INSCRITOS PARA BECA SEP", body, "", ',', cuentaEmail.Smtp, cuentaEmail.Puerto, cuentaEmail.SSL, true, ref respuesta);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
