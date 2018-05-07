using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using BLL;
using System.Drawing;
using System.IO;
using System.ComponentModel;
using System.Threading;
using Utilities;

namespace Universidad.BLL
{
 
    public class BLLAlumno
    {

        #region Generacion de Cargos
 public static void CargoExamen(ref List<Pago> Cabecero, int alumnoId, int anio, int periodo, int ofertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                int descuentoId = db.AlumnoDescuento.AsNoTracking()
                    .Where(alumno => alumno.AlumnoId == alumnoId
                                     && alumno.Anio == anio
                                     && alumno.PeriodoId == periodo
                                     && alumno.OfertaEducativaId == ofertaEducativaId
                                     && alumno.PagoConceptoId == 1).Count();

                descuentoId = descuentoId > 0 ? db.AlumnoDescuento.AsNoTracking()
                    .Where(alumno => alumno.AlumnoId == alumnoId
                                     && alumno.Anio == anio
                                     && alumno.PeriodoId == periodo
                                     && alumno.OfertaEducativaId == ofertaEducativaId
                                     && alumno.PagoConceptoId == 1).FirstOrDefault().DescuentoId : 0;

                Cabecero.Add(new Pago
                {
                    AlumnoId = alumnoId,
                    Anio = anio,
                    PeriodoId = periodo,
                    SubperiodoId = 1,
                    OfertaEducativaId = ofertaEducativaId,
                    FechaGeneracion = DateTime.Now,
                    CuotaId = (from a in db.Cuota
                               where
                               a.PagoConceptoId == 1
                               && a.OfertaEducativaId == ofertaEducativaId
                               && a.Anio == anio
                               && a.PeriodoId == periodo
                               && !a.EsEmpresa

                               select a.CuotaId).FirstOrDefault(),
                    Cuota = (from a in db.Cuota
                             where
                             a.PagoConceptoId == 1
                             && a.OfertaEducativaId == ofertaEducativaId
                             && a.Anio == anio
                             && a.PeriodoId == periodo
                             && !a.EsEmpresa
                             select a.Monto).FirstOrDefault(),
                    Promesa = (from a in db.Cuota
                               where
                               a.PagoConceptoId == 1
                               && a.OfertaEducativaId == ofertaEducativaId
                               && a.Anio == anio
                               && a.PeriodoId == periodo
                               && !a.EsEmpresa
                               select a.Monto).FirstOrDefault() - db.spConceptoDescuento(1, alumnoId, periodo, anio).FirstOrDefault() ?? 0,
                    ReferenciaId = "",
                    PagoId = 0,
                    EstatusId = 1,
                    PagoDescuento = descuentoId == 0 ? null : new List<PagoDescuento> { new PagoDescuento { DescuentoId = descuentoId, Monto = db.spConceptoDescuento(1, alumnoId, periodo, anio).FirstOrDefault() ?? 0 } }


                });
            }
        }

        public static void CargoCredencial(ref List<Pago> Cabecero, int alumnoId, int anio, int periodo, int ofertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                int descuentoId = db.AlumnoDescuento.AsNoTracking()
                    .Where(alumno => alumno.AlumnoId == alumnoId
                                     && alumno.Anio == anio
                                     && alumno.PeriodoId == periodo
                                     && alumno.OfertaEducativaId == ofertaEducativaId
                                     && alumno.PagoConceptoId == 1000).Count();

                descuentoId = descuentoId > 0 ? db.AlumnoDescuento.AsNoTracking()
                    .Where(alumno => alumno.AlumnoId == alumnoId
                                     && alumno.Anio == anio
                                     && alumno.PeriodoId == periodo
                                     && alumno.OfertaEducativaId == ofertaEducativaId
                                     && alumno.PagoConceptoId == 1000).FirstOrDefault().DescuentoId : 0;


                Cabecero.Add(new Pago
                {
                    AlumnoId = alumnoId,
                    Anio = anio,
                    PeriodoId = periodo,
                    SubperiodoId = 1,
                    OfertaEducativaId = ofertaEducativaId,
                    FechaGeneracion = DateTime.Now,
                    CuotaId = (from a in db.Cuota
                               where
                               a.PagoConceptoId == 1000
                               && a.OfertaEducativaId == ofertaEducativaId
                               && a.Anio == anio
                               && a.PeriodoId == periodo
                               && !a.EsEmpresa

                               select a.CuotaId).FirstOrDefault(),
                    Cuota = (from a in db.Cuota
                             where
                             a.PagoConceptoId == 1000
                             && a.OfertaEducativaId == ofertaEducativaId
                             && a.Anio == anio
                             && a.PeriodoId == periodo
                             && !a.EsEmpresa
                             select a.Monto).FirstOrDefault(),
                    Promesa = (from a in db.Cuota
                               where
                               a.PagoConceptoId == 1000
                               && a.OfertaEducativaId == ofertaEducativaId
                               && a.Anio == anio
                               && a.PeriodoId == periodo
                               && !a.EsEmpresa
                               select a.Monto).FirstOrDefault() - db.spConceptoDescuento(1000, alumnoId, periodo, anio).FirstOrDefault() ?? 0,
                    ReferenciaId = "",
                    PagoId = 0,
                    EstatusId = 1,
                    PagoDescuento = descuentoId == 0 ? null : new List<PagoDescuento> { new PagoDescuento { DescuentoId = descuentoId, Monto = db.spConceptoDescuento(1000, alumnoId, periodo, anio).FirstOrDefault() ?? 0 } }
                });
            }
        }

        public static void CargoInscripcion(ref List<Pago> Cabecero, int alumnoId, int anio, int periodo, int ofertaEducativaId)
        {
            
            using (UniversidadEntities db = new UniversidadEntities())
            {

                int descuentoId = db.AlumnoDescuento.AsNoTracking()
                      .Where(alumno => alumno.AlumnoId == alumnoId
                                       && alumno.Anio == anio
                                       && alumno.PeriodoId == periodo
                                       && alumno.OfertaEducativaId == ofertaEducativaId
                                       && alumno.PagoConceptoId == 802).Count();

                descuentoId = descuentoId > 0 ? db.AlumnoDescuento.AsNoTracking()
                    .Where(alumno => alumno.AlumnoId == alumnoId
                                     && alumno.Anio == anio
                                     && alumno.PeriodoId == periodo
                                     && alumno.OfertaEducativaId == ofertaEducativaId
                                     && alumno.PagoConceptoId == 802).FirstOrDefault().DescuentoId : 0;

                Cabecero.Add(new Pago
                {
                    AlumnoId = alumnoId,
                    Anio = anio,
                    PeriodoId = periodo,
                    SubperiodoId = 1,
                    OfertaEducativaId = ofertaEducativaId,
                    FechaGeneracion = DateTime.Now,
                    CuotaId = (from a in db.Cuota
                               where
                               a.PagoConceptoId == 802
                               && a.OfertaEducativaId == ofertaEducativaId
                               && a.Anio == anio
                               && a.PeriodoId == periodo
                               && !a.EsEmpresa

                               select a.CuotaId).FirstOrDefault(),
                    Cuota = (from a in db.Cuota
                             where
                             a.PagoConceptoId == 802
                             && a.OfertaEducativaId == ofertaEducativaId
                             && a.Anio == anio
                             && a.PeriodoId == periodo
                             && !a.EsEmpresa
                             select a.Monto).FirstOrDefault(),
                    Promesa = (from a in db.Cuota
                               where
                               a.PagoConceptoId == 802
                               && a.OfertaEducativaId == ofertaEducativaId
                               && a.Anio == anio
                               && a.PeriodoId == periodo
                               && !a.EsEmpresa
                               select a.Monto).FirstOrDefault() - db.spConceptoDescuento(802, alumnoId, periodo, anio).FirstOrDefault() ?? 0,
                    ReferenciaId = "",
                    PagoId = 0,
                    EstatusId = 1,
                    PagoDescuento = descuentoId == 0 ? null : new List<PagoDescuento> { new PagoDescuento { DescuentoId = descuentoId, Monto = db.spConceptoDescuento(802, alumnoId, periodo, anio).FirstOrDefault() ?? 0 } }
                });
            }
        }

        public static void CargoColegiatura(ref List<Pago> Cabecero, int alumnoId, int anio, int periodo, int ofertaEducativaId, int subperiodoId)
        {
            int pagoConceptoId = ofertaEducativaId >= 32 && ofertaEducativaId <= 41 ? 807 : 800;

            using (UniversidadEntities db = new UniversidadEntities())
            {

                int descuentoId = db.AlumnoDescuento.AsNoTracking()
                     .Where(alumno => alumno.AlumnoId == alumnoId
                                      && alumno.Anio == anio
                                      && alumno.PeriodoId == periodo
                                      && alumno.OfertaEducativaId == ofertaEducativaId
                                      && alumno.PagoConceptoId == pagoConceptoId).Count();

                descuentoId = descuentoId > 0 ? db.AlumnoDescuento.AsNoTracking()
                    .Where(alumno => alumno.AlumnoId == alumnoId
                                     && alumno.Anio == anio
                                     && alumno.PeriodoId == periodo
                                     && alumno.OfertaEducativaId == ofertaEducativaId
                                     && alumno.PagoConceptoId == pagoConceptoId).FirstOrDefault().DescuentoId : 0;

                Cabecero.Add(new Pago
                {
                    AlumnoId = alumnoId,
                    Anio = anio,
                    PeriodoId = periodo,
                    SubperiodoId = subperiodoId,
                    OfertaEducativaId = ofertaEducativaId,
                    FechaGeneracion = DateTime.Now,
                    CuotaId = (from a in db.Cuota
                               where
                               a.PagoConceptoId == pagoConceptoId
                               && a.OfertaEducativaId == ofertaEducativaId
                               && a.Anio == anio
                               && a.PeriodoId == periodo
                               && !a.EsEmpresa

                               select a.CuotaId).FirstOrDefault(),
                    Cuota = (from a in db.Cuota
                             where
                             a.PagoConceptoId == pagoConceptoId
                             && a.OfertaEducativaId == ofertaEducativaId
                             && a.Anio == anio
                             && a.PeriodoId == periodo
                             && !a.EsEmpresa
                             select a.Monto).FirstOrDefault(),
                    Promesa = (from a in db.Cuota
                               where
                               a.PagoConceptoId == pagoConceptoId
                               && a.OfertaEducativaId == ofertaEducativaId
                               && a.Anio == anio
                               && a.PeriodoId == periodo
                               && !a.EsEmpresa
                               select a.Monto).FirstOrDefault() - db.spConceptoDescuento(pagoConceptoId, alumnoId, periodo, anio).FirstOrDefault() ?? 0,
                    ReferenciaId = "",
                    PagoId = 0,
                    EstatusId = 1,
                    PagoDescuento = descuentoId == 0 ? null : new List<PagoDescuento> { new PagoDescuento { DescuentoId = descuentoId, Monto = db.spConceptoDescuento(pagoConceptoId, alumnoId, periodo, anio).FirstOrDefault() ?? 0 } }
                });
            }
        }

        public static void Inscripcion(int alumnoId, bool examen, bool credencial, bool inscripcion, bool colegiatura1, bool colegiatura2, bool colegiatura3, bool colegiatura4)
        {
            int ofertaEducativaId = 0;
             List<Pago> Cabecero = new List<Pago>();
             using (UniversidadEntities db = new UniversidadEntities())
             {
                 ofertaEducativaId = db.AlumnoInscrito.Where(alumno => alumno.AlumnoId == alumnoId).FirstOrDefault().OfertaEducativaId;


                 if (examen)
                 {
                     CargoExamen(ref Cabecero, alumnoId, 2016, 2, ofertaEducativaId);
                 }

                 if (credencial)
                 {
                     CargoCredencial(ref Cabecero, alumnoId, 2016, 2, ofertaEducativaId);
                 }

                 if (inscripcion)
                 {
                     CargoInscripcion(ref Cabecero, alumnoId, 2016, 2, ofertaEducativaId);
                 }

                 if (colegiatura1)
                 {
                     CargoColegiatura(ref Cabecero, alumnoId, 2016, 2, ofertaEducativaId, 1);
                 }

                 if (colegiatura2)
                 {
                     CargoColegiatura(ref Cabecero, alumnoId, 2016, 2, ofertaEducativaId, 2);
                 }

                 if (colegiatura3)
                 {
                     CargoColegiatura(ref Cabecero, alumnoId, 2016, 2, ofertaEducativaId, 3);
                 }

                 if (colegiatura4)
                 {
                     CargoColegiatura(ref Cabecero, alumnoId, 2016, 2, ofertaEducativaId, 4);
                 }


                 Cabecero.ForEach(pago =>
                 {
                     pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId <= 0 ? 0 : pago.PagoId).FirstOrDefault();
                     db.Pago.Add(pago);
                     Console.WriteLine("" + pago.AlumnoId + " - Referencia");
                 });


                 db.SaveChanges();

                 if (Cabecero.Count(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000") > 0)
                 {
                     //Generación de referencia
                     Cabecero.Where(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000").ToList().ForEach(pago =>
                     {
                         pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId).FirstOrDefault();
                         Console.WriteLine("" + pago.AlumnoId + " - Actualiza Referencia");
                     });

                     db.SaveChanges();
                 }
             }
        }
        #endregion Generacion de Cargos.

        #region Generacion de Reingresos

        public static bool GeneraReingreso(int alumnoId, int anio, int periodo, int ofertaEducativaId, ref Utilities.ProcessResult Resultado)
        {
            try
            {
                int conceptoColegiatura = ofertaEducativaId >= 32 && ofertaEducativaId <= 41 ? 807 : 800;

                using (UniversidadEntities db = new UniversidadEntities())
                {
                    List<Pago> Cabecero = new List<Pago>();

                    Cabecero.Add(new Pago
                    {
                        AlumnoId = alumnoId,
                        Anio = anio,
                        PeriodoId = periodo,
                        SubperiodoId = 1,
                        OfertaEducativaId = ofertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = (from a in db.Cuota
                                   where
                                   a.PagoConceptoId == 802
                                   && a.OfertaEducativaId == ofertaEducativaId
                                   && a.Anio == anio
                                   && a.PeriodoId == periodo
                                   && !a.EsEmpresa

                                   select a.CuotaId).FirstOrDefault(),
                        Cuota = (from a in db.Cuota
                                 where
                                 a.PagoConceptoId == 802
                                 && a.OfertaEducativaId == ofertaEducativaId
                                 && a.Anio == anio
                                 && a.PeriodoId == periodo
                                 && !a.EsEmpresa
                                 select a.Monto).FirstOrDefault(),
                        Promesa = (from a in db.Cuota
                                   where
                                   a.PagoConceptoId == 802
                                   && a.OfertaEducativaId == ofertaEducativaId
                                   && a.Anio == anio
                                   && a.PeriodoId == periodo
                                   && !a.EsEmpresa
                                   select a.Monto).FirstOrDefault(),
                        ReferenciaId = "",
                        PagoId = 0,
                        EstatusId = 1
                    });

                    for (int i = 1; i <= 4; i++)
                    {
                        Cabecero.Add(new Pago
                        {
                            AlumnoId = alumnoId,
                            Anio = anio,
                            PeriodoId = periodo,
                            SubperiodoId = i,
                            OfertaEducativaId = ofertaEducativaId,
                            FechaGeneracion = DateTime.Now,
                            CuotaId = (from a in db.Cuota
                                       where
                                       a.PagoConceptoId == conceptoColegiatura
                                       && a.OfertaEducativaId == ofertaEducativaId
                                       && a.Anio == anio
                                       && a.PeriodoId == periodo
                                       && !a.EsEmpresa

                                       select a.CuotaId).FirstOrDefault(),
                            Cuota = (from a in db.Cuota
                                     where
                                     a.PagoConceptoId == conceptoColegiatura
                                     && a.OfertaEducativaId == ofertaEducativaId
                                     && a.Anio == anio
                                     && a.PeriodoId == periodo
                                     && !a.EsEmpresa
                                     select a.Monto).FirstOrDefault(),
                            Promesa = (from a in db.Cuota
                                       where
                                       a.PagoConceptoId == conceptoColegiatura
                                       && a.OfertaEducativaId == ofertaEducativaId
                                       && a.Anio == anio
                                       && a.PeriodoId == periodo
                                       && !a.EsEmpresa
                                       select a.Monto).FirstOrDefault(),
                            ReferenciaId = "",
                            PagoId = 0,
                            EstatusId = 1
                        });

                        Console.WriteLine("" + alumnoId + " - Colegiatura - " + "Subperiodo - " + i);
                    }

                    Cabecero.ForEach(pago =>
                    {
                        pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId <= 0 ? 0 : pago.PagoId).FirstOrDefault();
                        db.Pago.Add(pago);
                        Console.WriteLine("" + pago.AlumnoId + " - Referencia");
                    });


                    db.SaveChanges();

                    if (Cabecero.Count(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000") > 0)
                    {
                        //Generación de referencia
                        Cabecero.Where(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000").ToList().ForEach(pago =>
                        {
                            pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId).FirstOrDefault();
                            Console.WriteLine("" + pago.AlumnoId + " - Actualiza Referencia");
                        });

                        db.SaveChanges();
                    }

                    Console.WriteLine("Termino");
                }

                return true;
            }
            catch (Exception Ex)
            {
                Resultado.Estatus = false;
                Resultado.Mensaje = Ex.Message;
                Resultado.MensajeDetalle = (Ex.InnerException != null) ? Ex.InnerException.InnerException.Message : string.Empty;
                Resultado.Informacion = "BLLPago.Aplicar()";
                return false;
            }
        }
        #endregion Generacion de Reingresos
        
        #region Reinscripción

        public static void GeneraReinscripcionIngles(int menor, int mayor)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<Pago> Cabecero = new List<Pago>();
                List<DTO.DTOReinscripcion> Reinscritos = (from a in db.Alumno.AsNoTracking()
                                                      join b in db.AlumnoInscrito.AsNoTracking() on a.AlumnoId equals b.AlumnoId
                                                      join c in db.OfertaEducativa on b.OfertaEducativaId equals c.OfertaEducativaId
                                                      where a.EstatusId == 1
                                                      && b.AlumnoId <= mayor
                                                      && c.OfertaEducativaTipoId == 4

                                                      select new DTO.DTOReinscripcion
                                                      {
                                                          alumnoId = b.AlumnoId,
                                                          ofertaEducativaId = b.OfertaEducativaId
                                                      }).ToList();

                
                //Colegiaturas Ingles
                Reinscritos.ForEach(alumno =>
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        Cabecero.Add(new Pago
                        {
                            AlumnoId = alumno.alumnoId,
                            Anio = 2016,
                            PeriodoId = 2,
                            SubperiodoId = i,
                            OfertaEducativaId = alumno.ofertaEducativaId,
                            FechaGeneracion = DateTime.Now,
                            CuotaId = (from a in db.Cuota
                                       where
                                       a.PagoConceptoId == 807
                                       && a.OfertaEducativaId == alumno.ofertaEducativaId
                                       && a.Anio == 2016
                                       && a.PeriodoId == 2
                                       && !a.EsEmpresa

                                       select a.CuotaId).FirstOrDefault(),
                            Cuota = (from a in db.Cuota
                                     where
                                     a.PagoConceptoId == 807
                                     && a.OfertaEducativaId == alumno.ofertaEducativaId
                                     && a.Anio == 2016
                                     && a.PeriodoId == 2
                                     && !a.EsEmpresa
                                     select a.Monto).FirstOrDefault(),
                            Promesa = (from a in db.Cuota
                                       where
                                       a.PagoConceptoId == 807
                                       && a.OfertaEducativaId == alumno.ofertaEducativaId
                                       && a.Anio == 2016
                                       && a.PeriodoId == 2
                                       && !a.EsEmpresa
                                       select a.Monto).FirstOrDefault(),
                            ReferenciaId = "",
                            PagoId = 0,
                            EstatusId = 1
                        });

                        Console.WriteLine("" + alumno.alumnoId + " - Colegiatura - " + "Subperiodo - " + i);
                    }
                });

                //Referencias
                Cabecero.ForEach(pago =>
                {
                    pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId <= 0 ? 0 : pago.PagoId).FirstOrDefault();
                    db.Pago.Add(pago);
                    Console.WriteLine("" + pago.AlumnoId + " - Referencia");
                });


                db.SaveChanges();

                if (Cabecero.Count(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000") > 0)
                {
                    //Generación de referencia
                    Cabecero.Where(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000").ToList().ForEach(pago =>
                    {
                        pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId).FirstOrDefault();
                        Console.WriteLine("" + pago.AlumnoId + " - Actualiza Referencia");
                    });

                    db.SaveChanges();
                }

                Console.WriteLine("Termino");
            }
        }


        public static void GeneraReinscripcion(int menor, int mayor)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<Pago> Cabecero = new List<Pago>();

                List<DTO.DTOReinscripcion> Reinscritos = (from a in db.Alumno.AsNoTracking()
                                                      join b in db.AlumnoInscrito.AsNoTracking() on a.AlumnoId equals b.AlumnoId
                                                      join c in db.OfertaEducativa on b.OfertaEducativaId equals c.OfertaEducativaId
                                                      where a.EstatusId == 1
                                                      && b.AlumnoId <= mayor
                                                      && b.AlumnoId >= menor
                                                      && c.OfertaEducativaTipoId != 4

                                                      select new DTO.DTOReinscripcion { 
                                                          alumnoId = b.AlumnoId,
                                                          ofertaEducativaId = b.OfertaEducativaId
                                                      }).ToList();
                
                //Reinscripción
                Reinscritos.ForEach(alumno => {
                    Cabecero.Add(new Pago
                    {
                        AlumnoId = alumno.alumnoId,
                        Anio = 2016,
                        PeriodoId = 2,
                        SubperiodoId = 1,
                        OfertaEducativaId = alumno.ofertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = (from a in db.Cuota
                                   where
                                   a.PagoConceptoId == 802
                                   && a.OfertaEducativaId == alumno.ofertaEducativaId
                                   && a.Anio == 2016
                                   && a.PeriodoId == 2
                                   && !a.EsEmpresa

                                   select a.CuotaId).FirstOrDefault(),
                        Cuota = (from a in db.Cuota
                                 where
                                 a.PagoConceptoId == 802
                                 && a.OfertaEducativaId == alumno.ofertaEducativaId
                                 && a.Anio == 2016
                                 && a.PeriodoId == 2
                                 && !a.EsEmpresa
                                 select a.Monto).FirstOrDefault(),
                        Promesa = (from a in db.Cuota
                                   where
                                   a.PagoConceptoId == 802
                                   && a.OfertaEducativaId == alumno.ofertaEducativaId
                                   && a.Anio == 2016
                                   && a.PeriodoId == 2
                                   && !a.EsEmpresa
                                   select a.Monto).FirstOrDefault(),
                        ReferenciaId = "",
                        PagoId = 0,
                        EstatusId = 1
                    });

                    Console.WriteLine("" + alumno.alumnoId + " - Reinscripción");
                });
                 

                
                //Colegiaturas
                Reinscritos.ForEach(alumno =>
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        Cabecero.Add(new Pago
                        {
                            AlumnoId = alumno.alumnoId,
                            Anio = 2016,
                            PeriodoId = 2,
                            SubperiodoId = i,
                            OfertaEducativaId = alumno.ofertaEducativaId,
                            FechaGeneracion = DateTime.Now,
                            CuotaId = (from a in db.Cuota
                                       where
                                       a.PagoConceptoId == 800
                                       && a.OfertaEducativaId == alumno.ofertaEducativaId
                                       && a.Anio == 2016
                                       && a.PeriodoId == 2
                                       && !a.EsEmpresa

                                       select a.CuotaId).FirstOrDefault(),
                            Cuota = (from a in db.Cuota
                                     where
                                     a.PagoConceptoId == 800
                                     && a.OfertaEducativaId == alumno.ofertaEducativaId
                                     && a.Anio == 2016
                                     && a.PeriodoId == 2
                                     && !a.EsEmpresa
                                     select a.Monto).FirstOrDefault(),
                            Promesa = (from a in db.Cuota
                                       where
                                       a.PagoConceptoId == 800
                                       && a.OfertaEducativaId == alumno.ofertaEducativaId
                                       && a.Anio == 2016
                                       && a.PeriodoId == 2
                                       && !a.EsEmpresa
                                       select a.Monto).FirstOrDefault(),
                            ReferenciaId = "",
                            PagoId = 0,
                            EstatusId = 1
                        });

                        Console.WriteLine("" + alumno.alumnoId + " - Colegiatura - " + "Subperiodo - " + i);
                    }
                });

                //Referencias
                Cabecero.ForEach(pago =>
                {
                    pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId <= 0 ? 0 : pago.PagoId).FirstOrDefault();
                    db.Pago.Add(pago);
                    Console.WriteLine("" + pago.AlumnoId + " - Referencia");
                });


                db.SaveChanges();

                if (Cabecero.Count(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000") > 0)
                {
                    //Generación de referencia
                    Cabecero.Where(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000").ToList().ForEach(pago =>
                    {
                        pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId).FirstOrDefault();
                        Console.WriteLine("" + pago.AlumnoId + " - Actualiza Referencia");
                    });

                    db.SaveChanges();
                }

                Console.WriteLine("Termino");
            }
        }

        #endregion Reinscripción
 
        #region Adeudos

        //public static void GeneraAdeudosNuevo(int menor, int mayor)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        List<Pago> Cabecero = new List<Pago>();

        //        db.TempAdeudo.AsNoTracking().ToList().ForEach(adeudo =>
        //        {
        //            if (adeudo.Tipo == 1 && adeudo.Adeudo > 0)
        //            {
        //                Cabecero.Add(new Pago
        //                {
        //                    AlumnoId = adeudo.AlumnoId,
        //                    Anio = 2016,
        //                    PeriodoId = 1,
        //                    SubperiodoId = 4,
        //                    OfertaEducativaId = adeudo.OfertaEducativaId,
        //                    FechaGeneracion = DateTime.Now,
        //                    CuotaId = (from a in db.Cuota
        //                               where
        //                               a.PagoConceptoId == 800
        //                               && a.OfertaEducativaId == adeudo.OfertaEducativaId
        //                               && a.Anio == 2016
        //                               && a.PeriodoId == 1
        //                               && !a.EsEmpresa

        //                               select a.CuotaId).FirstOrDefault(),
        //                    Cuota = (from a in db.Cuota
        //                             where
        //                             a.PagoConceptoId == 800
        //                             && a.OfertaEducativaId == adeudo.OfertaEducativaId
        //                             && a.Anio == 2016
        //                             && a.PeriodoId == 1
        //                             && !a.EsEmpresa
        //                             select a.Monto).FirstOrDefault(),
        //                    Promesa = System.Math.Abs(adeudo.Adeudo),
        //                    ReferenciaId = "",
        //                    PagoId = 0,
        //                    EstatusId = 1
        //                });
        //            }

        //            if (adeudo.Tipo == 2 && adeudo.Adeudo > 0)
        //            {
        //                Cabecero.Add(new Pago
        //                {
        //                    AlumnoId = adeudo.AlumnoId,
        //                    Anio = 2016,
        //                    PeriodoId = 1,
        //                    SubperiodoId = 4,
        //                    OfertaEducativaId = 32,
        //                    FechaGeneracion = DateTime.Now,
        //                    CuotaId = (from a in db.Cuota
        //                               where
        //                               a.PagoConceptoId == 807
        //                               && a.OfertaEducativaId == 32
        //                               && a.Anio == 2016
        //                               && a.PeriodoId == 1
        //                               && !a.EsEmpresa

        //                               select a.CuotaId).FirstOrDefault(),
        //                    Cuota = (from a in db.Cuota
        //                             where
        //                             a.PagoConceptoId == 807
        //                             && a.OfertaEducativaId == 32
        //                             && a.Anio == 2016
        //                             && a.PeriodoId == 1
        //                             && !a.EsEmpresa
        //                             select a.Monto).FirstOrDefault(),
        //                    Promesa = System.Math.Abs(adeudo.Adeudo),
        //                    ReferenciaId = "",
        //                    PagoId = 0,
        //                    EstatusId = 1
        //                });
        //            }

        //            Console.WriteLine(adeudo.AlumnoId);
        //        });

        //        Cabecero.ForEach(pago =>
        //        {
        //            pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId <= 0 ? 0 : pago.PagoId).FirstOrDefault();
        //            db.Pago.Add(pago);
        //        });



        //        db.SaveChanges();

        //        if (Cabecero.Count(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000") > 0)
        //        {
        //            //Generación de referencia
        //            Cabecero.Where(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000").ToList().ForEach(pago =>
        //            {
        //                pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId).FirstOrDefault();
        //            });

        //            db.SaveChanges();
        //        }
        //    }
        //}

        public static void GeneraAdeudos(int menor, int mayor)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<Pago> Cabecero = new List<Pago>();

                //Cargos Lic.
                db.Adeudo.AsNoTracking().ToList().ForEach(adeudo =>
                {
                    if (adeudo.AlumnoId >= menor && adeudo.AlumnoId <= mayor)
                    {

                        if (adeudo.ColegiaturaIdiomas > 0)
                        {
                            //Colegiatura Idiomas
                            Cabecero.Add(new Pago
                            {
                                AlumnoId = adeudo.AlumnoId,
                                Anio = 2016,
                                PeriodoId = 1,
                                SubperiodoId = 4,
                                OfertaEducativaId = 32,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = (from a in db.Cuota
                                           where
                                           a.PagoConceptoId == 807
                                           && a.OfertaEducativaId == 32
                                           && a.Anio == 2016
                                           && a.PeriodoId == 1
                                           && !a.EsEmpresa

                                           select a.CuotaId).FirstOrDefault(),
                                Cuota = (from a in db.Cuota
                                         where
                                         a.PagoConceptoId == 807
                                         && a.OfertaEducativaId == 32
                                         && a.Anio == 2016
                                         && a.PeriodoId == 1
                                         && !a.EsEmpresa
                                         select a.Monto).FirstOrDefault(),
                                Promesa = System.Math.Abs(adeudo.ColegiaturaIdiomas),
                                ReferenciaId = "",
                                PagoId = 0,
                                EstatusId = 1
                            });
                        }

                        if (adeudo.Colegiatura > 0)
                        {
                            //Colegiatura Lic. Posgrado, Maestria
                            Cabecero.Add(new Pago
                            {
                                AlumnoId = adeudo.AlumnoId,
                                Anio = 2016,
                                PeriodoId = 1,
                                SubperiodoId = 4,
                                OfertaEducativaId = adeudo.OfertaEducativaId ?? 0,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = (from a in db.Cuota
                                           where
                                           a.PagoConceptoId == 800
                                           && a.OfertaEducativaId == adeudo.OfertaEducativaId
                                           && a.Anio == 2016
                                           && a.PeriodoId == 1
                                           && !a.EsEmpresa

                                           select a.CuotaId).FirstOrDefault(),
                                Cuota = (from a in db.Cuota
                                         where
                                         a.PagoConceptoId == 800
                                         && a.OfertaEducativaId == adeudo.OfertaEducativaId
                                         && a.Anio == 2016
                                         && a.PeriodoId == 1
                                         && !a.EsEmpresa
                                         select a.Monto).FirstOrDefault(),
                                Promesa = System.Math.Abs(adeudo.Colegiatura),
                                ReferenciaId = "",
                                PagoId = 0,
                                EstatusId = 1
                            });
                        }

                        if (adeudo.Inscripcion > 0)
                        {
                            //Inscripción Lic. Posgrado, Maestria
                            Cabecero.Add(new Pago
                            {
                                AlumnoId = adeudo.AlumnoId,
                                Anio = 2016,
                                PeriodoId = 1,
                                SubperiodoId = 4,
                                OfertaEducativaId = adeudo.OfertaEducativaId ?? 0,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = (from a in db.Cuota
                                           where
                                           a.PagoConceptoId == 802
                                           && a.OfertaEducativaId == adeudo.OfertaEducativaId
                                           && a.Anio == 2016
                                           && a.PeriodoId == 1
                                           && !a.EsEmpresa

                                           select a.CuotaId).FirstOrDefault(),
                                Cuota = (from a in db.Cuota
                                         where
                                         a.PagoConceptoId == 802
                                         && a.OfertaEducativaId == adeudo.OfertaEducativaId
                                         && a.Anio == 2016
                                         && a.PeriodoId == 1
                                         && !a.EsEmpresa
                                         select a.Monto).FirstOrDefault(),
                                Promesa = System.Math.Abs(adeudo.Inscripcion),
                                ReferenciaId = "",
                                PagoId = 0,
                                EstatusId = 1
                            });
                        }

                        Console.WriteLine(adeudo.AlumnoId); 
                    }
                });

                Cabecero.ForEach(pago => { 
                     pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId <= 0 ? 0 : pago.PagoId).FirstOrDefault();
                     db.Pago.Add(pago);
                });

                db.SaveChanges();

                if (Cabecero.Count(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000") > 0)
                {
                    //Generación de referencia
                    Cabecero.Where(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000").ToList().ForEach(pago =>
                    {
                        pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId).FirstOrDefault();
                    });

                    db.SaveChanges();
                }
            }
        }

        #endregion Adeudos

        #region Cartas y password
        public static List<DTO.DTODatos> DatosCarta()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var Lista =
                (from a in db.AlumnoPassword
                 join b in db.Alumno on a.AlumnoId equals b.AlumnoId
                 join z in db.AlumnoDetalle on b.AlumnoId equals z.AlumnoId
                 join c in db.AlumnoInscrito on b.AlumnoId equals c.AlumnoId
                 join d in db.OfertaEducativa on c.OfertaEducativaId equals d.OfertaEducativaId

                 where d.OfertaEducativaTipoId != 4
                 select new DTO.DTODatos
                 {
                     alumnoId = a.AlumnoId,
                     nombre = b.Nombre + " " + b.Paterno + " " + b.Materno,
                     password = a.Password,
                     descripcion = d.Descripcion,
                     nombreSolo = b.Nombre,
                     estatusId = b.EstatusId,
                     sexoId = z.GeneroId
                 }).ToList();

                Lista.ForEach(a =>
                {
                    a.password = Utilities.Seguridad.Desencripta(27, a.password);
                });

                return Lista.ToList();
            }
        }

        public static List<DTO.DTODatos> DatosCartaIngles()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var Lista =
                (from a in db.AlumnoPassword
                 join b in db.Alumno on a.AlumnoId equals b.AlumnoId
                 join z in db.AlumnoDetalle on b.AlumnoId equals z.AlumnoId
                 join c in db.AlumnoInscrito on b.AlumnoId equals c.AlumnoId
                 join d in db.OfertaEducativa on c.OfertaEducativaId equals d.OfertaEducativaId

                 where d.OfertaEducativaTipoId == 4
                 && b.EstatusId == 1
                 select new DTO.DTODatos
                 {
                     alumnoId = a.AlumnoId,
                     nombre = b.Nombre + " " + b.Paterno + " " + b.Materno,
                     password = a.Password,
                     descripcion = d.Descripcion,
                     nombreSolo = b.Nombre,
                     estatusId = b.EstatusId,
                     sexoId = z.GeneroId
                 }).ToList();

                Lista.ForEach(a =>
                {
                    a.password = Utilities.Seguridad.Desencripta(27, a.password);
                });

                return Lista.ToList();
            }
        }

        public static void GeneraPassword(int menor, int mayor)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.Alumno.AsNoTracking().ToList().ForEach(alumno =>
                {
                    if (alumno.AlumnoId >= menor && alumno.AlumnoId <= mayor)
                    {
                        db.AlumnoPassword.Add(new AlumnoPassword
                        {
                            AlumnoId = alumno.AlumnoId,
                            Password = Utilities.Seguridad.Encripta(27, Utilities.Cadena.GeneraPassword(6))
                        });

                        Thread.Sleep(500);
                        Console.WriteLine("" + alumno.AlumnoId);
                    }
                });

                db.SaveChanges();
            }
        }
        #endregion Cartas

        public static void InsertaBitacora(int alumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.AlumnoAccesoBitacora.Add(new AlumnoAccesoBitacora
                {
                    AlumnoId = alumnoId,
                    FechaIngreso = DateTime.Now,
                    HoraIngreso = DateTime.Now.TimeOfDay
                });

                db.SaveChanges();
            }
        }

       

        public static void ActualizaPassword(string password, string token)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var AlumnoToken = db.AlumnoPasswordRecovery
                    .Where(alumno => alumno.Token == token )
                    .FirstOrDefault();

                var AlumnoPassword = db.AlumnoPassword
                    .Where(a => a.AlumnoId == AlumnoToken.AlumnoId)
                    .FirstOrDefault();

                //Verificar tamaño de pass en base
                AlumnoPassword.Password = Utilities.Seguridad.Encripta(27, password);

                AlumnoToken.EstatusId = 2;

                db.SaveChanges();

                DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();


                var refere = new Utilities.ProcessResult();
                string body = "";
                DTOAlumno opjAl = BLLAlumnoPortal.ObtenerAlumno(AlumnoToken.AlumnoId);

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
                                                    "<div class='col-md-7 footer-gray portlet light '>" +
                                                        "<div class='portlet-title '>" +
                                                            "<div class='caption'>" +
                                                                "<h2 class='caption font-green-sharp bold uppercase'>RECUPERACIÓN CONTRASEÑA</h2>" +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class='col-md-12'>" +
                                                    "<div class='col-md-3'>" +
                                                    "</div>" +
                                                    "<div class='col-md-7 footer-gray portlet light portlet-title'>" +
                                                        "<h3 class='caption font-blue'>" + opjAl.Nombre + " " + opjAl.Paterno + " " + opjAl.Materno + "</h3>" +
                                                        "<hr />" +
                                                        "<h3 class='caption font-blue'>Los siguientes datos son tus credenciales para poder acceder al portal de la universidad</h3>" +
                                                        "<hr />" +
                                                        "<h3 class='caption font-blue'>Usuario</h3>" +
                                                        "<h4 class='caption font-blue-dark'>" + opjAl.AlumnoId.ToString() + "</h4>" +
                                                            "<h3 class='caption font-blue'>Contraseña</h3>" +
                                                        "<h4 class='caption font-blue-dark'>" + password + "</h4>" +
                                                        "<hr />" +
                                                        "<h3 class='caption font-blue'>Puedes acceder a el desde el siguiente enlace.</h3>" +
                                                        "<a class='caption font-blue-dark' href='http://108.163.172.122/portalalumno/login.html'>YMCA</a>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</body>" +
                            "</html>";
                #endregion

                Utilities.Email.Enviar(objCuenta.Email, objCuenta.Password, objCuenta.DisplayName, opjAl.Email, ',', "antoniogalvan@ymcacdmex.org.mx", ';', "Recuperación Contraseña Portal Universidad YMCA", body, "", ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true, ref refere);
            }
        }

        public static void RecuperaPassword(string email)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                //Verificar si el email esta asociado a un alumno
                var Emails = (from a in db.AlumnoDetalle
                          where a.Email == email
                          select a);

                if (Emails.Count() > 0)
                {
                    var Email = BLLVarios.RecuperaPassword(Emails.FirstOrDefault().AlumnoId, true);
                    Utilities.ProcessResult resultado = new Utilities.ProcessResult();
                    Utilities.Email.Enviar(Email.email, Email.password, Email.displayName, email, ';', "Solicitud de cambio de contraseña", Email.body, "", ';', Email.smtp, Email.puerto, Email.ssl, true, ref resultado);
                } 
            }
        }

        public static List<DTO.DTOAlumnoReinscripcion> Concentrado()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                string varl = "Viernes" + "Martes";
                string var2 = "Gay";
                return
                    (from x in
                         (from a in db.AlumnoInscrito
                          join b in db.Alumno on a.AlumnoId equals b.AlumnoId
                          join c in db.AlumnoDescuento on new { a.AlumnoId, a.OfertaEducativaId } equals new { c.AlumnoId, c.OfertaEducativaId }
                          where c.PagoConceptoId == 800
                          && c.OfertaEducativaId < 30
                          select new { a, b, c })
                     group x by new
                     {
                         x.a.AlumnoId,
                         x.a.OfertaEducativaId,
                         x.a.Anio,
                         x.a.PeriodoId,
                         x.c.PagoConceptoId,
                         Alumno = x.a.AlumnoId,
                         Nombre = x.b.Nombre + " " + x.b.Paterno + " M sksks",
                         Beca = x.c.Monto
                     } into g
                     orderby g.Key.AlumnoId ascending

                     select new DTO.DTOAlumnoReinscripcion
                     {
                         alumnoId = g.Key.AlumnoId,
                         nombre = g.Key.Nombre,
                         beca = g.Key.Beca
                     }).ToList();
            }
        }

        public static DTO.DTOAlumnoDatosGenerales LoginAcademico(int alumnoId, string password)
        {
            password = Utilities.Seguridad.Encripta(27, password);

            using (UniversidadEntities db = new UniversidadEntities())
            {
               
                    return (from a in db.AlumnoPassword
                            where a.AlumnoId == alumnoId
                            && a.Password == password
                            select new DTO.DTOAlumnoDatosGenerales
                            {
                                alumnoId = alumnoId
                            }).FirstOrDefault();
            }
        }

        public static DTO.DTOAdeudo Adeudo(DTO.DTOAlumnoDatosGenerales Datos)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (new DTO.DTOAdeudo
                {
                    Pagare = (from a in db.Pagare
                              where a.EstatusId == 1
                              && a.AlumnoId == Datos.alumnoId
                              && a.FechaVencimiento <= DateTime.Now
                              select new DTO.DTOPagare
                              {
                                  pagareId = a.PagareId,
                                  alumnoId = a.AlumnoId,
                                  fechaGeneracion = a.FechaGeneracion,
                                  fechaVencimiento = a.FechaVencimiento,
                                  importe = a.Importe,
                                  interes = a.Interes,
                                  estatus = a.Estatus.Descripcion,
                                  observacion = a.Observacion
                              }).ToList(),
                    Financimiento = (from a in db.Financiamiento
                                     where a.AlumnoId == Datos.alumnoId
                                     && a.EstatusId == 1
                                     select new DTO.DTOFinanciamiento
                                     {
                                         financiamientoId = a.FinanciamientoId,
                                         alumnoId = a.AlumnoId,
                                         fechaGeneracion = a.FechaGeneracion,
                                         porcentaje = a.Porcentaje,
                                         cuotaId = a.CuotaId,
                                         concepto = a.Cuota1.PagoConcepto.Descripcion,
                                         cuota = a.Cuota,
                                         estatus = a.Estatus.Descripcion,
                                         importe = a.Importe ?? 0,
                                         observacion = a.Observacion
                                     }).ToList()
                });
            }
        }

        //public static List<DTO.DTOPago> PagosPendientes(ref Utilities.ProcessResult Resultado, DTO.DTOAlumnoDatosGenerales Datos)
        //{
        //    try {
        //        using (UniversidadEntities db = new UniversidadEntities())
        //        {

        //            DateTime FechaActual = DateTime.Now.Date;

        //            var periodo =
        //                (from a in db.Periodo
        //                 where FechaActual >= a.FechaInicial && FechaActual <= a.FechaFinal
        //                 select new { a.Anio, a.PeriodoId }).FirstOrDefault();

        //            return
        //                ((from a in db.Pago
        //                  //join b in db.Cuota on new { a.CuotaId, a.Anio, a.PeriodoId, a.OfertaEducativaId } equals new { b.CuotaId, b.Anio, b.PeriodoId, b.OfertaEducativaId }
        //                  join b in db.Cuota on new { a.CuotaId } equals new { b.CuotaId }
        //                  join c in db.PagoConcepto on new { b.PagoConceptoId, b.OfertaEducativaId } equals new { c.PagoConceptoId, c.OfertaEducativaId }
        //                  join d in db.Subperiodo on new { a.SubperiodoId, a.PeriodoId } equals new { d.SubperiodoId, d.PeriodoId }
        //                  join e in db.Mes on d.MesId equals e.MesId
        //                  where a.FechaPago == null
        //                        && a.AlumnoId == Datos.alumnoId
        //                        && a.OfertaEducativaId == Datos.ofertaEducativaId
        //                        && a.EstatusId == 1
        //                  orderby a.PagoId ascending
        //                  select new DTO.DTOPago
        //                      {
        //                          pagoId = a.PagoId,
        //                          conceptoPago = a.PagoId <=2588 ? "Adeudo, periodos anteriores" : c.Descripcion + " " + e.Descripcion + " " + (a.PeriodoId > 1 ? a.Anio: a.FechaGeneracion.Year ),
        //                          conceptoPagoId = b.PagoConceptoId,
        //                          cuota = a.Cuota,
        //                          descuento = db.fnPagoDescuento(a.PagoId),
        //                          importe = a.Promesa,
        //                          mesId = e.MesId,
        //                          anio = a.Anio,
        //                          periodoId = a.PeriodoId,
        //                          adeudo = ((a.Anio < periodo.Anio) || (a.Anio == periodo.Anio && a.PeriodoId < periodo.PeriodoId) || a.PagoId <= 2588) ? true : false,
        //                          //esVariable = a.PagoId <= 2588 ? true : false,
        //                          esVariable = c.EsVariable,
        //                          Descuentos = (from f in a.PagoDescuento
        //                                        join g in db.AlumnoDescuento on new { a.AlumnoId, a.Anio, a.PeriodoId, a.OfertaEducativaId, f.DescuentoId } equals new { g.AlumnoId, g.Anio, g.PeriodoId, g.OfertaEducativaId, g.DescuentoId }
        //                                        join h in db.Descuento on new { g.DescuentoId, g.OfertaEducativaId } equals new { h.DescuentoId, h.OfertaEducativaId }
        //                                        select new DTO.DTOPagoDescuento
        //                                        {
        //                                            descuentoId = f.DescuentoId,
        //                                            descripcion = f.Descuento.Descripcion,
        //                                            descuentoTipo = h.DescuentoTipo.Descripcion,
        //                                            porcentaje = (h.DescuentoTipoId == 1) ? g.Monto + "%" : "$" + g.Monto,  /*"$" + g.Monto,*/
        //                                            importe = f.Monto,
        //                                            observacion = g.Comentario,
        //                                            alumnoDescuentoId = g.AlumnoDescuentoId
        //                                        }).ToList(),
                                                
                                                
        //                      }).ToList());
        //        }
        //    }

        //    catch (Exception Ex)
        //    {
        //        Resultado.Estatus = false;
        //        Resultado.Mensaje = Ex.Message;
        //        Resultado.MensajeDetalle = (Ex.InnerException != null) ? Ex.InnerException.Message : string.Empty;
        //        Resultado.Informacion = "BLLAlumno.PagosPendientes()";

        //        return null;
        //    }
        //}

        public static List<DTO.DTOPagoConcepto> ConceptosPago(DTO.DTOAlumnoDatosGenerales Datos)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                //int [] Conceptos = new int []{800, 802};
                DateTime FechaActual = DateTime.Now.Date;

                //Obtener Periodo
                var periodo =
                   (from a in db.Periodo
                    where FechaActual >= a.FechaInicial && FechaActual <= a.FechaFinal
                    select new { a.Anio, a.PeriodoId}).FirstOrDefault();

                return
                    (from a in db.Cuota
                     join b in db.PagoConcepto on new { a.PagoConceptoId, a.OfertaEducativaId } equals new { b.PagoConceptoId, b.OfertaEducativaId }
                     where b.EstatusId == 1 
                     && b.EsVisible 
                     && a.Anio == periodo.Anio
                     && a.PeriodoId == periodo.PeriodoId
                     && a.OfertaEducativaId == Datos.ofertaEducativaId
                     orderby b.Descripcion ascending
                     select new DTO.DTOPagoConcepto
                     {
                         pagoId = 0,
                         cuatrimestre = a.Anio + " " + a.PeriodoId,
                         descripcion = b.Descripcion,
                         conceptoPagoId = a.PagoConceptoId,
                         cuota = a.Monto,
                         cuotaId = a.CuotaId,
                         descuento = db.fnConceptoDescuento(a.PagoConceptoId, Datos.alumnoId, periodo.PeriodoId, periodo.Anio),
                         importe = a.Monto - db.fnConceptoDescuento(a.PagoConceptoId, Datos.alumnoId, periodo.PeriodoId, periodo.Anio),
                         esVariable = b.EsVariable,
                         Descuentos = (from z in db.AlumnoDescuento
                                       join y in db.Descuento on new { z.DescuentoId, z.OfertaEducativaId } equals new { y.DescuentoId, y.OfertaEducativaId }
                                       where z.AlumnoId == Datos.alumnoId
                                             && y.PagoConceptoId == a.PagoConceptoId
                                             && z.EstatusId == 1
                                       select new DTO.DTOPagoDescuento
                                       {
                                           descuentoId = z.DescuentoId,
                                           descripcion = y.Descripcion,
                                           descuentoTipo = y.DescuentoTipo.Descripcion,
                                           porcentaje = (y.DescuentoTipoId == 1) ? z.Monto + "%" : "$" + z.Monto,
                                           importe = db.fnDetalleDescuentoConcepto(z.AlumnoDescuentoId),
                                           alumnoDescuentoId = z.AlumnoDescuentoId,
                                           observacion = z.Comentario
                                       }).ToList()
                     })/*.Where((x) => !(Conceptos.Contains(x.conceptoPagoId)))*/.ToList();
            }
        }

        public static List<DTO.DTOAlumnoOfertaEducativa> OfertasEducativas(int alumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.AlumnoInscrito.AsNoTracking()
                        where a.AlumnoId == alumnoId
                 select new DTO.DTOAlumnoOfertaEducativa { 
                     ofertaEducativa = a.OfertaEducativa.Descripcion,
                     ofertaEducativaId = a.OfertaEducativaId
                 }).ToList();
            }
        }


        public static void GuardaImagen(string directorio, int alumnoId, DTO.DTOLogin Credencial)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.AlumnoImagen.Add(new AlumnoImagen
                {
                    AlumnoId = alumnoId,
                    Imagen = Utilities.Archivo.Bytes(directorio)
                });

                db.AlumnoImagenDetalle.Add(new AlumnoImagenDetalle
                {
                    AlumnoId = alumnoId,
                    Extension = Path.GetExtension(directorio).ToLower(),
                    UsuarioIdCarga = Credencial.usuarioId
                });

                db.SaveChanges();
            }
        }

        public static Image ConsultaImagen(int alumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.Configuration.LazyLoadingEnabled = true;

                byte[] bytesImagen =
                    (from a in db.AlumnoImagen.AsNoTracking()
                     where a.AlumnoId == alumnoId
                     select a.Imagen)
                    .ToList().FirstOrDefault();

                return Utilities.Archivo.Imagen(bytesImagen == null ? BLLUsuario.ImagenDefault() : bytesImagen);
            }
        }

        public static int ObtenerAlumnoId(string entradaTexto)
        {
            try
            {
                if (entradaTexto.Length > 5)
                    return int.Parse(entradaTexto.Substring(0, 5));
                else if (entradaTexto.Length == 4 || entradaTexto.Length == 5)
                    return int.Parse(entradaTexto.Substring(0, entradaTexto.Length));
                else
                    return int.Parse(entradaTexto.Substring(0, entradaTexto.Length));
            }
            catch (Exception) { return 0; }
        }

        public static List<DTO.DTOAlumnoBusqueda> Busqueda(string filtroAlumno)
        {
            filtroAlumno = Utilities.Cadena.SinAcentos(filtroAlumno);

            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTO.DTOAlumnoBusqueda> Resultado =
                    (from a in db.Alumno.AsNoTracking()
                     
                     where (a.Nombre + " " + a.Paterno + " " + a.Materno).Contains(filtroAlumno) | (a.AlumnoId.ToString()).Contains(filtroAlumno)
                     select new DTO.DTOAlumnoBusqueda
                     {
                         alumnoId = a.AlumnoId,
                         nombre = a.Nombre + " " + a.Paterno + " " + a.Materno
                     }).ToList();

                return Resultado;
            }
        }

        public static DTO.DTOAlumnoDatosGenerales DatosGenerales(int alumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTO.DTOAlumnoDatosGenerales Resultado =
                    (from a in db.Alumno.AsNoTracking()
                     join b in db.AlumnoInscrito.AsNoTracking() on a.AlumnoId equals b.AlumnoId
                     join c in db.OfertaEducativa.AsNoTracking() on b.OfertaEducativaId equals c.OfertaEducativaId
                     join d in db.Periodo.AsNoTracking() on new { b.PeriodoId, b.Anio } equals new { d.PeriodoId, d.Anio }
                     where b.AlumnoId == alumnoId
                     select new DTO.DTOAlumnoDatosGenerales
                     {
                         alumnoId = a.AlumnoId,
                         nombre = a.Nombre + " " + a.Paterno + " " + a.Materno,
                         ofertaEducativa = c.Descripcion,
                         anio = d.Anio,
                         periodo = d.PeriodoId,
                         ofertaEducativaId = b.OfertaEducativaId,
                         Ofertas = (from z in db.AlumnoInscrito
                                    join y in db.OfertaEducativa on z.OfertaEducativaId equals y.OfertaEducativaId
                                    where z.AlumnoId == alumnoId
                                    select new DTO.DTOAlumnoOfertaEducativa
                                    {
                                        ofertaEducativaId = z.OfertaEducativaId,
                                        ofertaEducativa = y.Descripcion
                                    }).ToList()

                     }).ToList().FirstOrDefault();

                return Resultado;                                       
            }
        }

        public static DTO.Alumno.DTOAlumnoImagen ImagenIndex(int alumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                var Ano = (from a in db.Alumno
                           join c in db.AlumnoImagen on a.AlumnoId equals c.AlumnoId
                           join d in db.AlumnoImagenDetalle on a.AlumnoId equals d.AlumnoId
                           where a.AlumnoId == alumnoId
                           select new DTO.Alumno.DTOAlumnoImagen
                           {
                               nombre = (a.Nombre + " " + a.Paterno + " " + a.Materno).Trim(),
                               imagen = c.Imagen,
                               extensionImagen = d.Extension
                           }).FirstOrDefault();

                if (Ano == null)
                {


                    Ano = new DTO.Alumno.DTOAlumnoImagen
                    {
                        imagen = BLLUsuario.ImagenDefault(),
                        extensionImagen = ".png",
                        nombre = (from a in db.Alumno
                                  where a.AlumnoId == alumnoId
                                  select (a.Nombre + " " + a.Paterno + " " + a.Materno).Trim()).FirstOrDefault()
                    };
                }
                string fotoAlumno = "";
                try
                {
                    fotoAlumno = AlumnoFoto.GetAlumnoFotoBase64(alumnoId);
                }
                catch
                {
                    fotoAlumno = "";
                }
                return new DTO.Alumno.DTOAlumnoImagen
                {
                    nombre = Ano.nombre,
                    imagenBase64 = fotoAlumno,//Convert.ToBase64String(Ano.imagen),
                    extensionImagen = Ano.extensionImagen
                };

                //return new DTO.Usuario.DTOUsuarioImagen
                //{
                //    imagenBase64 = Convert.ToBase64String((from a in db.UsuarioImagen
                //                                           where a.UsuarioId == usuarioId
                //                                           select a.Imagen).ToList().FirstOrDefault()),
                //    extensionImagen = (from a in db.UsuarioImagenDetalle
                //                       where a.UsuarioId == usuarioId
                //                       select a.Extension).FirstOrDefault(),
                //                       nombre = 
                //};
            }
        }

        //public static List<DTO.DTOPagoInformes> PagosAlumno(DTO.DTOAlumnoDatosGenerales Datos)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {

        //        DateTime FechaActual = DateTime.Now.Date;

        //        var periodo =
        //            (from a in db.Periodo
        //             where FechaActual >= a.FechaInicial && FechaActual <= a.FechaFinal
        //             select new { a.Anio, a.PeriodoId }).FirstOrDefault();

        //        return
        //            ((from a in db.Pago
        //              join b in db.Cuota on new { a.CuotaId, a.Anio, a.PeriodoId, a.OfertaEducativaId } equals new { b.CuotaId, b.Anio, b.PeriodoId, b.OfertaEducativaId }
        //              join c in db.PagoConcepto on new { b.PagoConceptoId, b.OfertaEducativaId } equals new { c.PagoConceptoId, c.OfertaEducativaId }
        //              join d in db.Subperiodo on new { a.SubperiodoId, a.PeriodoId } equals new { d.SubperiodoId, d.PeriodoId }
        //              join e in db.Mes on d.MesId equals e.MesId
        //              where a.FechaPago != null
        //                    && a.AlumnoId == Datos.alumnoId
        //                    && a.OfertaEducativaId == Datos.ofertaEducativaId
        //              orderby a.PagoId ascending
        //              select new DTO.DTOPagoInformes
        //              {
        //                  FechaPago = a.FechaPago ?? DateTime.Now,
        //                  conceptoPago = c.Descripcion + " " + e.Descripcion + " " + a.FechaGeneracion.Year,
        //                  periodo = a.Anio + " - " + a.PeriodoId,
        //                  cuota = a.Cuota == 0 ? a.Promesa : a.Cuota,
        //                  descuento = db.fnPagoDescuento(a.PagoId),
        //                  importe = a.Promesa,
        //                  referenciaId = a.ReferenciaId
        //              }).ToList());
        //    }
        //}

        //public static List<DTO.DTOPagoInformes> PagosPendientesAlumno(DTO.DTOAlumnoDatosGenerales Datos)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {

        //        DateTime FechaActual = DateTime.Now.Date;

        //        var periodo =
        //            (from a in db.Periodo
        //             where FechaActual >= a.FechaInicial && FechaActual <= a.FechaFinal
        //             select new { a.Anio, a.PeriodoId }).FirstOrDefault();

        //        return
        //            ((from a in db.Pago
        //              join b in db.Cuota on new { a.CuotaId, a.Anio, a.PeriodoId, a.OfertaEducativaId } equals new { b.CuotaId, b.Anio, b.PeriodoId, b.OfertaEducativaId }
        //              join c in db.PagoConcepto on new { b.PagoConceptoId, b.OfertaEducativaId } equals new { c.PagoConceptoId, c.OfertaEducativaId }
        //              join d in db.Subperiodo on new { a.SubperiodoId, a.PeriodoId } equals new { d.SubperiodoId, d.PeriodoId }
        //              join e in db.Mes on d.MesId equals e.MesId
        //              where a.FechaPago != null
        //                    && a.AlumnoId == Datos.alumnoId
        //                    && a.OfertaEducativaId == Datos.ofertaEducativaId
        //              //&& a.EstatusId == 1
        //              orderby a.PagoId ascending
        //              select new DTO.DTOPagoInformes
        //              {
        //                  conceptoPago = c.Descripcion + " " + e.Descripcion + " " + a.FechaGeneracion.Year,
        //                  periodo = a.Anio + " - " + a.PeriodoId,
        //                  cuota = a.Cuota == 0 ? a.Promesa : a.Cuota,
        //                  descuento = db.fnPagoDescuento(a.PagoId),
        //                  importe = a.Promesa,
        //                  referenciaId = a.ReferenciaId
        //                  //mesId = e.MesId,
        //                  //adeudo = ((a.Anio < periodo.Anio) || (a.Anio == periodo.Anio && a.PeriodoId < periodo.PeriodoId)) ? true : false,
        //                  //Descuentos = (from f in a.PagoDescuento
        //                  //              join g in db.AlumnoDescuento on new { a.AlumnoId, a.Anio, a.PeriodoId, a.OfertaEducativaId, f.DescuentoId } equals new { g.AlumnoId, g.Anio, g.PeriodoId, g.OfertaEducativaId, g.DescuentoId }
        //                  //              join h in db.Descuento on new { g.DescuentoId, g.OfertaEducativaId } equals new { h.DescuentoId, h.OfertaEducativaId }
        //                  //              select new DTO.DTOPagoDescuento
        //                  //              {
        //                  //                  descuentoId = f.DescuentoId,
        //                  //                  descripcion = f.Descuento.Descripcion,
        //                  //                  descuentoTipo = h.DescuentoTipo.Descripcion,
        //                  //                  porcentaje = (h.DescuentoTipoId == 1) ? g.Monto + "%" : "$" + g.Monto,  /*"$" + g.Monto,*/
        //                  //                  importe = f.Monto,
        //                  //                  observacion = g.Comentario,
        //                  //                  alumnoDescuentoId = g.AlumnoDescuentoId
        //                  //              }).ToList()
        //              }).ToList());
        //    }
        //}    
        
    }
}
