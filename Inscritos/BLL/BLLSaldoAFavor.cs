using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLSaldoAFavor
    {
        public static bool bAnticipoUno;
        public static bool bAnticipoDos;
        public static Universidad.DAL.Importe Validacion;
        public static void AplicacionSaldoAlumno1(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                string cuentaIngXApl = db.SistemaConfiguracion.Where(sc => sc.ParametroId == 5).FirstOrDefault().Valor;
                List<AlumnoSaldo> Alumno = new List<AlumnoSaldo>();
                List<Pago> PagosPendientes = new List<Pago>();
                List<Pago> PagosPendientesAnteriores = new List<Pago>();

                #region Saldo del Alumno

                Alumno.Add((from a in db.AlumnoSaldo
                            where a.Saldo > 0
                                   && a.AlumnoId == AlumnoId
                            select a).FirstOrDefault());

                #endregion Saldo del Alumno

                if (Alumno.FirstOrDefault() != null && Alumno.FirstOrDefault().Saldo > 0)
                {
                    #region Pagos Pendientes
                    PagosPendientes.AddRange(
                        (from a in db.Pago
                         join b in db.AlumnoSaldo on a.AlumnoId equals b.AlumnoId
                         where (a.Cuota1.PagoConceptoId == 800 ||
                                a.Cuota1.PagoConceptoId == 802 ||
                                a.Cuota1.PagoConceptoId == 807 ||
                                a.Cuota1.PagoConceptoId == 304 ||
                                a.Cuota1.PagoConceptoId == 320 ||
                                a.Cuota1.PagoConceptoId == 306 ||
                                a.Cuota1.PagoConceptoId == 15 ||
                                a.Cuota1.PagoConceptoId == 808 ||
                                a.Cuota1.PagoConceptoId == 1010)
                               && (a.EstatusId == 1 || a.EstatusId == 13)
                               && !(a.Anio == 2016 && a.PeriodoId == 1)
                               && (a.Promesa > 0)
                               && (b.Saldo > 0)
                               && a.AlumnoId == AlumnoId
                         select a).ToList());

                    #endregion Pagos Pendientes

                    #region Inscripcion 802

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 802) > 0)
                    {
                        var pagoBD = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 802).FirstOrDefault();
                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Inscripcion 802

                    #region Colegiaturas 800

                    #region Ciclo 1

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 800 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 800
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 1

                    #region Ciclo 2

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 800 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 800
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 2

                    #region Ciclo 3

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 800 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 800
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 3

                    #region Ciclo 4

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 800 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 800
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 4

                    #region Ciclo 5

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 800 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 800
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 5

                    #endregion Colegiaturas 800

                    #region Recargos 306

                    #region Ciclo 1

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 306 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 306
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 1

                    #region Ciclo 2

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 306 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 306
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 2

                    #region Ciclo 3

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 306 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 306
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 3

                    #region Ciclo 4

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 306 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 306
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 4

                    #region Ciclo 5

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 306 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 306
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 5

                    #endregion Recargos 306

                    #region Colegiaturas 807

                    #region Ciclo 1

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 807 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 807
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 1

                    #region Ciclo 2

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 807 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 807
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 2

                    #region Ciclo 3

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 807 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 807
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 3

                    #region Ciclo 4

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 807 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 807
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 4

                    #region Ciclo 5

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 807 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 807
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 5

                    #endregion Colegiaturas 807

                    #region Adelanto de Materia 304

                    #region Ciclo 1

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 304 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 304
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 1

                    #region Ciclo 2

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 304 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 304
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 2

                    #region Ciclo 3

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 304 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 304
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 3

                    #region Ciclo 4

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 304 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 304
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 4

                    #region Ciclo 5

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 304 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 304
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 5

                    #endregion Adelanto de Materia 304

                    #region Adelanto de Materia 320

                    #region Ciclo 1

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 320 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 320
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 1

                    #region Ciclo 2

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 320 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 320
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 2

                    #region Ciclo 3

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 320 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 320
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 3

                    #region Ciclo 4

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 320 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 320
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 4

                    #region Ciclo 5

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 320 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 320
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 5

                    #endregion Adelanto de Materia 320

                    #region Asesoria 15

                    #region Ciclo 1

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 15 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 15
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 1

                    #region Ciclo 2

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 15 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 15
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 2

                    #region Ciclo 3

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 15 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 15
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 3

                    #region Ciclo 4

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 15 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 15
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 4

                    #region Ciclo 5

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 15 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 15
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 5

                    #endregion Asesoria 15

                    #region Material Didactico 808

                    #region Ciclo 1

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 808 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 808
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 1

                    #region Ciclo 2

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 808 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 808
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 2

                    #region Ciclo 3

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 808 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 808
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 3

                    #region Ciclo 4

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 808 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 808
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 4

                    #region Ciclo 5

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 808 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 808
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 5

                    #endregion Material Didactico 808

                    #region Adeudos 2015

                    PagosPendientesAnteriores.AddRange(
                       (from a in db.Pago
                        join b in db.AlumnoSaldo on a.AlumnoId equals b.AlumnoId
                        where (a.EstatusId == 1 || a.EstatusId == 13)
                              && (a.Anio == 2016 && a.PeriodoId == 1)
                              && (a.Promesa > 0)
                              && (b.Saldo > 0)
                              && a.AlumnoId == AlumnoId
                        select a).ToList());

                    if (PagosPendientesAnteriores.Count > 0)
                    {

                        //var pagoBD = db.Pago.Where(a => a.PagoId == PagosPendientesAnteriores.FirstOrDefault().PagoId).FirstOrDefault();
                        //DAL.Pago Variable = db.Pago.Where(z => z.PagoId == PagosPendientesAnteriores.FirstOrDefault().PagoId).ToList().FirstOrDefault();

                        int pId = PagosPendientesAnteriores.FirstOrDefault().PagoId;

                        var Variable = (from a in db.Pago
                                        where a.PagoId == pId
                                        select a).ToList().FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == Variable.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == Variable.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (Variable.Promesa - refcabecero))
                        {
                            Variable.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = Variable.PagoId,
                                AlumnoId = Variable.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Variable.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = Variable.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Variable.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == Variable.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (Variable.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = Variable.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Variable.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Variable.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = Variable.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (Variable.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < Variable.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = Variable.PagoId,
                                AlumnoId = Variable.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = Variable.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == Variable.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = Variable.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = Variable.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }
                    #endregion Adeudos 2015

                    #region Diplomados 1010

                    #region Ciclo 1

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 1010 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 1010 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 1010 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 1010 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 1010
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 1

                    #region Ciclo 2

                    if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 1010 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                    {
                        //Obtenemos la colegiatura mas antigua....
                        int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 1010 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                        int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 1010 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                        int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 1010 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                        var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                        && x.Cuota1.PagoConceptoId == 1010
                                                        && x.Anio == minimoAnio
                                                        && x.PeriodoId == minimoPeriodo
                                                        && x.SubperiodoId == minimoSubperiodo
                                                        && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                        var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId && x.EstatusId == 8).FirstOrDefault().ImporteTotal : 0);

                        #region Podemos Pagar

                        if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - refcabecero))
                        {
                            pagoBD.EstatusId = 4;

                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = pagoBD.Promesa - refcabecero
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - refcabecero);
                                RefCabecero.EstatusId = 7;
                                RefCabecero.FechaAplicacion = DateTime.Now;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                Detalles.Add(new ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = pagoBD.Promesa - refcabecero,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = pagoBD.Promesa - refcabecero,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 7 //Se marca como usado
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - refcabecero);
                        }

                        #endregion PodemosPagar

                        #region No Podemos Pagar

                        else if (Alumno.FirstOrDefault().Saldo < pagoBD.Promesa)
                        {
                            db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                            {
                                PagoId = pagoBD.PagoId,
                                AlumnoId = pagoBD.AlumnoId,
                                FechaGasto = DateTime.Now,
                                HoraGasto = DateTime.Now.TimeOfDay,
                                Importe = Alumno.FirstOrDefault().Saldo
                            });

                            if (refcabecero > 0)
                            {
                                db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                Referenciado.ImporteRestante = 0;
                            }

                            else
                            {
                                List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                Detalles.Add(new DAL.ReferenciadoDetalle
                                {
                                    PagoId = pagoBD.PagoId,
                                    ReferenciaId = "0000000000",
                                    FechaPago = DateTime.Now,
                                    Importe = Alumno.FirstOrDefault().Saldo,
                                    FechaProcesado = DateTime.Now,
                                    HoraProcesado = DateTime.Now.TimeOfDay
                                });

                                db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                {
                                    ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                    ReferenciadoDetalle = Detalles,
                                    ReferenciaId = "0000000000",
                                    PagoId = pagoBD.PagoId,
                                    ImporteRestante = 0,
                                    EstatusId = 8
                                });
                            }

                            Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                        }

                        #endregion No podemos Pagar

                        db.SaveChanges();
                    }

                    #endregion Ciclo 2

                    #endregion Diplomados 1010

                }

            }
        }
        public static void AplicacionSaldoAlumno(int alumnoId, bool aplicaRecargos, bool aplicaIdiomas)
        {
              using (DAL.UniversidadEntities db = new DAL.UniversidadEntities())
            {
                List<DAL.Pago> PagosPendientes = new List<Pago>();
                List<DAL.Pago> PagosPendientesAnteriores = new List<Pago>();
                List<DAL.AlumnoSaldo> Alumno = new List<DAL.AlumnoSaldo>();

                Alumno.Add((from a in db.AlumnoSaldo
                            where a.Saldo > 0
                            && a.AlumnoId == alumnoId
                            select a).FirstOrDefault());

                DTO.DTOReferencia Referencia = new DTO.DTOReferencia
                {
                    referenciaId = "0000000000",
                    fechaPago = DateTime.Now
                };

                if (Alumno.FirstOrDefault() != null && Alumno.FirstOrDefault().Saldo > 0)
                {
                    #region Pagos Pendientes

                    PagosPendientes.AddRange(
                        (from a in db.Pago
                         where (a.Cuota1.PagoConceptoId == 800 ||
                                a.Cuota1.PagoConceptoId == 802 ||
                                a.Cuota1.PagoConceptoId == 807 ||
                                a.Cuota1.PagoConceptoId == 304 ||
                                a.Cuota1.PagoConceptoId == 320 ||
                                a.Cuota1.PagoConceptoId == 306 ||
                                a.Cuota1.PagoConceptoId == 15 ||
                                a.Cuota1.PagoConceptoId == 808 ||
                                a.Cuota1.PagoConceptoId == 1010)
                               && (a.EstatusId == 1 || a.EstatusId == 13)
                               && !(a.Anio == 2016 && a.PeriodoId == 1)
                               && (a.Promesa > 0)
                               && a.AlumnoId == alumnoId
                         select a).ToList());

                    #endregion Pagos Pendientes

                    if (PagosPendientes.Count > 0 && PagosPendientes.FirstOrDefault() != null)
                    {
                        if(aplicaRecargos)
                            #region Recargos 306

                        for (int i = 0; i < 5; i++)
                            #region Ciclo 1

                            if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 306 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                            {
                                //Obtenemos la colegiatura mas antigua....
                                int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                                int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                                int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 306 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                                var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                                && x.Cuota1.PagoConceptoId == 306
                                                                && x.Anio == minimoAnio
                                                                && x.PeriodoId == minimoPeriodo
                                                                && x.SubperiodoId == minimoSubperiodo
                                                                && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                                var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId).FirstOrDefault().ImporteTotal : 0);
                                    var cuenta = db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId);
                                    var parcial = (db.PagoParcial.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.PagoParcial.Where(x => x.PagoId == pagoBD.PagoId).Sum(x => x.Pago) : 0);
                                var total = refcabecero + parcial;

                                #region Podemos Pagar

                                if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - total))
                                {
                                    db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto { 
                                        PagoId = pagoBD.PagoId,
                                        AlumnoId = pagoBD.AlumnoId,
                                        FechaGasto = DateTime.Now,
                                        HoraGasto = DateTime.Now.TimeOfDay,
                                        SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                        SaldoDespues = (Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total)),
                                        Importe = (pagoBD.Promesa - total)
                                    });

                                    pagoBD.EstatusId = total == 0 ? 4 : 14;

                                    if (refcabecero > 0)
                                    {
                                        db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = pagoBD.Promesa - total,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                        RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - total);
                                        RefCabecero.EstatusId = 7;
                                        RefCabecero.FechaAplicacion = DateTime.Now;
                                    }

                                    else
                                    {
                                        List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                        Detalles.Add(new ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = pagoBD.Promesa - total,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                        {
                                            ImporteTotal = pagoBD.Promesa - total,
                                            ReferenciadoDetalle = Detalles,
                                            ReferenciaId = Referencia.referenciaId,
                                            PagoId = pagoBD.PagoId,
                                            ImporteRestante = 0,
                                            FechaAplicacion = DateTime.Now,
                                            EstatusId = 7 //Se marca como usado
                                        });
                                    }

                                    Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total);
                                }

                                    #endregion PodemosPagar

                                    #region No Podemos Pagar

                                    else if ((Alumno.FirstOrDefault().Saldo < (pagoBD.Promesa - total)) && Alumno.FirstOrDefault().Saldo > 0)
                                    {
                                    db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                    {
                                        PagoId = pagoBD.PagoId,
                                        AlumnoId = pagoBD.AlumnoId,
                                        FechaGasto = DateTime.Now,
                                        HoraGasto = DateTime.Now.TimeOfDay,
                                        SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                        SaldoDespues = (Alumno.FirstOrDefault().Saldo - (Alumno.FirstOrDefault().Saldo)),
                                        Importe = Alumno.FirstOrDefault().Saldo
                                    });

                                        if (cuenta > 0)
                                        {
                                            db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                            {
                                                PagoId = pagoBD.PagoId,
                                                ReferenciaId = Referencia.referenciaId,
                                                FechaPago = Referencia.fechaPago,
                                                Importe = Alumno.FirstOrDefault().Saldo,
                                                FechaProcesado = DateTime.Now,
                                                HoraProcesado = DateTime.Now.TimeOfDay,
                                                EsReferenciado = false,
                                                ReferenciaProcesadaId = null
                                            });

                                            var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                            Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                            Referenciado.ImporteRestante = 0;
                                        }

                                        else
                                        {
                                            List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                            Detalles.Add(new DAL.ReferenciadoDetalle
                                            {
                                                PagoId = pagoBD.PagoId,
                                                ReferenciaId = Referencia.referenciaId,
                                                FechaPago = Referencia.fechaPago,
                                                Importe = Alumno.FirstOrDefault().Saldo,
                                                FechaProcesado = DateTime.Now,
                                                HoraProcesado = DateTime.Now.TimeOfDay,
                                                EsReferenciado = false,
                                                ReferenciaProcesadaId = null
                                            });

                                            db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                            {
                                                ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                                ReferenciadoDetalle = Detalles,
                                                ReferenciaId = Referencia.referenciaId,
                                                PagoId = pagoBD.PagoId,
                                                ImporteRestante = 0,
                                                EstatusId = 8
                                            });
                                        }

                                    Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                                }

                                #endregion No podemos Pagar

                                db.SaveChanges();
                            }
                            else
                                break;

                            #endregion Ciclo 1

                        #endregion Recargos 306                

                        #region Colegiaturas 800

                        for (int i = 0; i < 5; i++)
                            #region Ciclo 1

                            if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 800 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                            {
                                //Obtenemos la colegiatura mas antigua....
                                int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                                int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                                int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 800 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                                var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                                && x.Cuota1.PagoConceptoId == 800
                                                                && x.Anio == minimoAnio
                                                                && x.PeriodoId == minimoPeriodo
                                                                && x.SubperiodoId == minimoSubperiodo
                                                                && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                                var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId).FirstOrDefault().ImporteTotal : 0);
                                var cuenta = db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId);
                                var parcial = (db.PagoParcial.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.PagoParcial.Where(x => x.PagoId == pagoBD.PagoId).Sum(x => x.Pago) : 0);
                                var total = refcabecero + parcial;

                                #region Check Anticipo

                                /********************************************************************************/
                                /*                     Anticipados 2016 - 2                                     */
                                /********************************************************************************/

                                ////Si es el primer pago
                                if (pagoBD.SubperiodoId == 1 && pagoBD.Anio == 2016 && pagoBD.PeriodoId == 2)
                                {
                                    //PRIMER PERIODO
                                    if (DateTime.Now >= DateTime.Parse("07/12/2015") && DateTime.Now <= DateTime.Parse("22/12/2015"))
                                        bAnticipoUno = true;

                                    //SEGUNDO PERIODO
                                    else if (DateTime.Now >= DateTime.Parse("23/12/2015") && DateTime.Now <= DateTime.Parse("31/12/2015"))
                                        bAnticipoDos = true;
                                }

                                /********************************************************************************/
                                /*                     Anticipados 2016 - 3                                     */
                                /********************************************************************************/

                                if (pagoBD.SubperiodoId == 1 && pagoBD.Anio == 2016 && pagoBD.PeriodoId == 3)
                                {
                                    if (DateTime.Now >= DateTime.Parse("15/04/2016") && DateTime.Now <= DateTime.Parse("30/04/2016"))
                                        bAnticipoUno = true;
                                }


                                /********************************************************************************/
                                /*                     Anticipados 2017 - 1                                     */
                                /********************************************************************************/

                                if (pagoBD.SubperiodoId == 1 && pagoBD.Anio == 2017 && pagoBD.PeriodoId == 1)
                                {
                                    if (DateTime.Now >= DateTime.Parse("15/08/2016") && DateTime.Now <= DateTime.Parse("31/08/2016"))
                                        bAnticipoUno = true;
                                }

                                #endregion Check Anticipo

                                #region Anticipo

                                if (bAnticipoUno || bAnticipoDos)
                                {
                                    #region Calculo

                                    decimal DMonto, Promesa, Pagar, nuevaPromesa, nueva;
                                    string Monto = BLL.BLLReferencia.DescuentoAnticipado();

                                    if (pagoBD.PeriodoId != 2)
                                    {
                                        DMonto = decimal.Parse(Monto);
                                        Promesa = 100 - DMonto; // 100 - 4 = 96
                                        DMonto = DMonto / 100; // 4/100 = .04
                                        Promesa = Promesa / 100; //  96 / 100 = .96
                                                                 //Importe del descuento que se genera
                                        Pagar = Math.Round(pagoBD.Promesa * DMonto); // .04
                                                                                     //Nueva promesa
                                        nueva = Math.Round(decimal.Parse((pagoBD.Promesa * decimal.Parse(Promesa.ToString())).ToString()));
                                    }

                                    else
                                    {
                                        DMonto = decimal.Parse(Monto);
                                        Promesa = 100 - DMonto; // 100 - 4 = 96
                                        DMonto = DMonto / 100; // 4/100 = .04
                                        Promesa = Promesa / 100; //  96 / 100 = .96

                                        //Obtener cuota anterior
                                        decimal nCuota = Math.Round((pagoBD.Cuota / (100 + int.Parse(Monto))) * 100);

                                        //Si es primer periodo quitar 4% a cuota anterior
                                        //Si es segundo periodo dejar la cuota anterior
                                        nCuota = bAnticipoUno ? Math.Round(nCuota * Promesa) : nCuota;

                                        //Importe del descuento anticipado
                                        decimal pAnticipado = pagoBD.Cuota - nCuota;

                                        //Si hay descuentos ver la proporcion con la cuota REAL
                                        List<DTO.Referencia.DTODescuentoReferencia> Desc = db.PagoDescuento
                                            .Select(a => new DTO.Referencia.DTODescuentoReferencia
                                            {
                                                pagoId = a.PagoId,
                                                descuentoId = a.DescuentoId,
                                                monto = a.Monto
                                            })
                                            .Where(b => b.pagoId == pagoBD.PagoId)
                                            .ToList();

                                        //Recalcular los importes de los descuentos
                                        Desc.ForEach(a =>
                                        {
                                            a.monto = Math.Round(
                                                            Math.Round(pagoBD.Cuota - pAnticipado) *
                                                            Math.Round(((a.monto * 100) / pagoBD.Cuota)) / 100
                                                        );
                                        });

                                        nuevaPromesa = pagoBD.Cuota - (pAnticipado + Desc.Sum(a => a.monto));
                                        Pagar = pAnticipado;
                                        nueva = nuevaPromesa;
                                    }

                                    #endregion Calculo

                                    Validacion = BLLReferencia.VerificaCantidadCantidad(Alumno.FirstOrDefault().Saldo, nueva);

                                    #region Verificación

                                    if (Validacion == Universidad.DAL.Importe.Igual || Validacion == Universidad.DAL.Importe.Mayor)
                                    {
                                        if (pagoBD.PeriodoId != 2)
                                        {
                                            DMonto = decimal.Parse(Monto);
                                            Promesa = 100 - DMonto; // 100 - 4 = 96
                                            DMonto = DMonto / 100; // 4/100 = .04
                                            Promesa = Promesa / 100; //  96 / 100 = .96
                                                                     //Importe del descuento que se genera
                                            Pagar = Math.Round(pagoBD.Promesa * DMonto); // .04
                                                                                         //Nueva promesa
                                            pagoBD.Promesa = Math.Round(decimal.Parse((pagoBD.Promesa * decimal.Parse(Promesa.ToString())).ToString()));
                                        }

                                        else
                                        {
                                            DMonto = decimal.Parse(Monto);
                                            Promesa = 100 - DMonto; // 100 - 4 = 96
                                            DMonto = DMonto / 100; // 4/100 = .04
                                            Promesa = Promesa / 100; //  96 / 100 = .96

                                            //Obtener cuota anterior
                                            decimal nCuota = Math.Round((pagoBD.Cuota / (100 + int.Parse(Monto))) * 100);

                                            //Si es primer periodo quitar 4% a cuota anterior
                                            //Si es segundo periodo dejar la cuota anterior
                                            nCuota = bAnticipoUno ? Math.Round(nCuota * Promesa) : nCuota;

                                            //Importe del descuento anticipado
                                            decimal pAnticipado = pagoBD.Cuota - nCuota;

                                            //Si hay descuentos ver la proporcion con la cuota REAL
                                            List<DAL.PagoDescuento> Desctos = db.PagoDescuento.Where(a => a.PagoId == pagoBD.PagoId).ToList();

                                            Desctos.ForEach(a =>
                                            {
                                                //a.Monto = Math.Round((a.Monto * 100) / pagoBD.Cuota);
                                                a.Monto = Math.Round(
                                                                    Math.Round(pagoBD.Cuota - pAnticipado) *
                                                                    Math.Round(((a.Monto * 100) / pagoBD.Cuota)) / 100
                                                                );
                                            });

                                            nuevaPromesa = pagoBD.Cuota - (pAnticipado + Desctos.Sum(a => a.Monto));
                                            Pagar = pAnticipado;
                                            pagoBD.Promesa = nuevaPromesa;
                                        }

                                        DAL.Descuento objDesc = db.Descuento
                                               .Where(D => D.OfertaEducativaId == pagoBD.OfertaEducativaId && D.Descripcion == "Pago Anticipado" && D.PagoConceptoId == pagoBD.Cuota1.PagoConceptoId)
                                                   .FirstOrDefault();

                                        pagoBD.EsAnticipado = true;

                                        if (bAnticipoUno)
                                            pagoBD.PeriodoAnticipadoId = 1;
                                        else if (bAnticipoDos)
                                            pagoBD.PeriodoAnticipadoId = 2;

                                        db.PagoDescuento.Add(new DAL.PagoDescuento
                                        {
                                            DescuentoId = objDesc.DescuentoId,
                                            Monto = Pagar,
                                            PagoId = pagoBD.PagoId
                                        });
                                    }

                                    #endregion Verificación

                                }

                                #endregion Anticipo

                                #region Podemos Pagar

                                if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - total))
                                {
                                    db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                    {
                                        PagoId = pagoBD.PagoId,
                                        AlumnoId = pagoBD.AlumnoId,
                                        FechaGasto = DateTime.Now,
                                        HoraGasto = DateTime.Now.TimeOfDay,
                                        SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                        SaldoDespues = (Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total)),
                                        Importe = (pagoBD.Promesa - total)
                                    });

                                    pagoBD.EstatusId = total == 0 ? 4 : 14;

                                    if (refcabecero > 0)
                                    {
                                        db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = pagoBD.Promesa - total,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                        RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - total);
                                        RefCabecero.EstatusId = 7;
                                        RefCabecero.FechaAplicacion = DateTime.Now;
                                    }

                                    else
                                    {
                                        List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                        Detalles.Add(new ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = pagoBD.Promesa - total,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                        {
                                            ImporteTotal = pagoBD.Promesa - total,
                                            ReferenciadoDetalle = Detalles,
                                            ReferenciaId = Referencia.referenciaId,
                                            PagoId = pagoBD.PagoId,
                                            ImporteRestante = 0,
                                            FechaAplicacion = DateTime.Now,
                                            EstatusId = 7 //Se marca como usado
                                        });
                                    }

                                    Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total);
                                }

                                #endregion PodemosPagar

                                #region No Podemos Pagar

                                else if ((Alumno.FirstOrDefault().Saldo < (pagoBD.Promesa - total)) && Alumno.FirstOrDefault().Saldo > 0)
                                {
                                    db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                    {
                                        PagoId = pagoBD.PagoId,
                                        AlumnoId = pagoBD.AlumnoId,
                                        FechaGasto = DateTime.Now,
                                        HoraGasto = DateTime.Now.TimeOfDay,
                                        SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                        SaldoDespues = (Alumno.FirstOrDefault().Saldo - (Alumno.FirstOrDefault().Saldo)),
                                        Importe = Alumno.FirstOrDefault().Saldo
                                    });

                                    if (cuenta > 0)
                                    {
                                        db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = Alumno.FirstOrDefault().Saldo,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                        Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                        Referenciado.ImporteRestante = 0;
                                    }

                                    else
                                    {
                                        List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                        Detalles.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = Alumno.FirstOrDefault().Saldo,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                        {
                                            ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                            ReferenciadoDetalle = Detalles,
                                            ReferenciaId = Referencia.referenciaId,
                                            PagoId = pagoBD.PagoId,
                                            ImporteRestante = 0,
                                            EstatusId = 8
                                        });
                                    }

                                    Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                                }

                                #endregion No podemos Pagar

                                db.SaveChanges();
                            }
                            else
                                break;

                            #endregion Ciclo 1

                        #endregion Colegiaturas 800

                        #region Inscripcion 802

                        if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 802) > 0)
                        {
                            var pagoBD = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 802).FirstOrDefault();
                            var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId).FirstOrDefault().ImporteTotal : 0);
                            var cuenta = db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId);
                            var parcial = (db.PagoParcial.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.PagoParcial.Where(x => x.PagoId == pagoBD.PagoId).Sum(x => x.Pago) : 0);
                            var total = refcabecero + parcial;

                            #region Check Anticipo

                            /********************************************************************************/
                            /*                     Anticipados 2016 - 2                                     */
                            /********************************************************************************/

                            ////Si es el primer pago
                            if (pagoBD.SubperiodoId == 1 && pagoBD.Anio == 2016 && pagoBD.PeriodoId == 2)
                            {
                                //PRIMER PERIODO
                                if (DateTime.Now >= DateTime.Parse("07/12/2015") && DateTime.Now <= DateTime.Parse("22/12/2015"))
                                    bAnticipoUno = true;

                                //SEGUNDO PERIODO
                                else if (DateTime.Now >= DateTime.Parse("23/12/2015") && DateTime.Now <= DateTime.Parse("31/12/2015"))
                                    bAnticipoDos = true;
                            }

                            /********************************************************************************/
                            /*                     Anticipados 2016 - 3                                     */
                            /********************************************************************************/

                            if (pagoBD.SubperiodoId == 1 && pagoBD.Anio == 2016 && pagoBD.PeriodoId == 3)
                            {
                                if (DateTime.Now >= DateTime.Parse("15/04/2016") && DateTime.Now <= DateTime.Parse("30/04/2016"))
                                    bAnticipoUno = true;
                            }


                            /********************************************************************************/
                            /*                     Anticipados 2017 - 1                                     */
                            /********************************************************************************/

                            if (pagoBD.SubperiodoId == 1 && pagoBD.Anio == 2017 && pagoBD.PeriodoId == 1)
                            {
                                if (DateTime.Now >= DateTime.Parse("15/08/2016") && DateTime.Now <= DateTime.Parse("31/08/2016"))
                                    bAnticipoUno = true;
                            }

                            #endregion Check Anticipo

                            #region Anticipo

                            if (bAnticipoUno || bAnticipoDos)
                            {
                                #region Calculo

                                decimal DMonto, Promesa, Pagar, nuevaPromesa, nueva;
                                string Monto = BLL.BLLReferencia.DescuentoAnticipado();

                                if (pagoBD.PeriodoId != 2)
                                {
                                    DMonto = decimal.Parse(Monto);
                                    Promesa = 100 - DMonto; // 100 - 4 = 96
                                    DMonto = DMonto / 100; // 4/100 = .04
                                    Promesa = Promesa / 100; //  96 / 100 = .96
                                                             //Importe del descuento que se genera
                                    Pagar = Math.Round(pagoBD.Promesa * DMonto); // .04
                                                                                 //Nueva promesa
                                    nueva = Math.Round(decimal.Parse((pagoBD.Promesa * decimal.Parse(Promesa.ToString())).ToString()));
                                }

                                else
                                {
                                    DMonto = decimal.Parse(Monto);
                                    Promesa = 100 - DMonto; // 100 - 4 = 96
                                    DMonto = DMonto / 100; // 4/100 = .04
                                    Promesa = Promesa / 100; //  96 / 100 = .96

                                    //Obtener cuota anterior
                                    decimal nCuota = Math.Round((pagoBD.Cuota / (100 + int.Parse(Monto))) * 100);

                                    //Si es primer periodo quitar 4% a cuota anterior
                                    //Si es segundo periodo dejar la cuota anterior
                                    nCuota = bAnticipoUno ? Math.Round(nCuota * Promesa) : nCuota;

                                    //Importe del descuento anticipado
                                    decimal pAnticipado = pagoBD.Cuota - nCuota;

                                    //Si hay descuentos ver la proporcion con la cuota REAL
                                    List<DTO.Referencia.DTODescuentoReferencia> Desc = db.PagoDescuento
                                        .Select(a => new DTO.Referencia.DTODescuentoReferencia
                                        {
                                            pagoId = a.PagoId,
                                            descuentoId = a.DescuentoId,
                                            monto = a.Monto
                                        })
                                        .Where(b => b.pagoId == pagoBD.PagoId)
                                        .ToList();

                                    //Recalcular los importes de los descuentos
                                    Desc.ForEach(a =>
                                    {
                                        a.monto = Math.Round(
                                                        Math.Round(pagoBD.Cuota - pAnticipado) *
                                                        Math.Round(((a.monto * 100) / pagoBD.Cuota)) / 100
                                                    );
                                    });

                                    nuevaPromesa = pagoBD.Cuota - (pAnticipado + Desc.Sum(a => a.monto));
                                    Pagar = pAnticipado;
                                    nueva = nuevaPromesa;
                                }

                                #endregion Calculo

                                Validacion = BLLReferencia.VerificaCantidadCantidad(Alumno.FirstOrDefault().Saldo, nueva);

                                #region Verificación

                                if (Validacion == Universidad. DAL.Importe.Igual || Validacion == Universidad.DAL.Importe.Mayor)
                                {
                                    if (pagoBD.PeriodoId != 2)
                                    {
                                        DMonto = decimal.Parse(Monto);
                                        Promesa = 100 - DMonto; // 100 - 4 = 96
                                        DMonto = DMonto / 100; // 4/100 = .04
                                        Promesa = Promesa / 100; //  96 / 100 = .96
                                                                 //Importe del descuento que se genera
                                        Pagar = Math.Round(pagoBD.Promesa * DMonto); // .04
                                                                                     //Nueva promesa
                                        pagoBD.Promesa = Math.Round(decimal.Parse((pagoBD.Promesa * decimal.Parse(Promesa.ToString())).ToString()));
                                    }

                                    else
                                    {
                                        DMonto = decimal.Parse(Monto);
                                        Promesa = 100 - DMonto; // 100 - 4 = 96
                                        DMonto = DMonto / 100; // 4/100 = .04
                                        Promesa = Promesa / 100; //  96 / 100 = .96

                                        //Obtener cuota anterior
                                        decimal nCuota = Math.Round((pagoBD.Cuota / (100 + int.Parse(Monto))) * 100);

                                        //Si es primer periodo quitar 4% a cuota anterior
                                        //Si es segundo periodo dejar la cuota anterior
                                        nCuota = bAnticipoUno ? Math.Round(nCuota * Promesa) : nCuota;

                                        //Importe del descuento anticipado
                                        decimal pAnticipado = pagoBD.Cuota - nCuota;

                                        //Si hay descuentos ver la proporcion con la cuota REAL
                                        List<DAL.PagoDescuento> Desctos = db.PagoDescuento.Where(a => a.PagoId == pagoBD.PagoId).ToList();

                                        Desctos.ForEach(a =>
                                        {
                                            //a.Monto = Math.Round((a.Monto * 100) / pagoBD.Cuota);
                                            a.Monto = Math.Round(
                                                                Math.Round(pagoBD.Cuota - pAnticipado) *
                                                                Math.Round(((a.Monto * 100) / pagoBD.Cuota)) / 100
                                                            );
                                        });

                                        nuevaPromesa = pagoBD.Cuota - (pAnticipado + Desctos.Sum(a => a.Monto));
                                        Pagar = pAnticipado;
                                        pagoBD.Promesa = nuevaPromesa;
                                    }

                                    DAL.Descuento objDesc = db.Descuento
                                           .Where(D => D.OfertaEducativaId == pagoBD.OfertaEducativaId && D.Descripcion == "Pago Anticipado" && D.PagoConceptoId == pagoBD.Cuota1.PagoConceptoId)
                                               .FirstOrDefault();

                                    pagoBD.EsAnticipado = true;

                                    if (bAnticipoUno)
                                        pagoBD.PeriodoAnticipadoId = 1;
                                    else if (bAnticipoDos)
                                        pagoBD.PeriodoAnticipadoId = 2;

                                    db.PagoDescuento.Add(new DAL.PagoDescuento
                                    {
                                        DescuentoId = objDesc.DescuentoId,
                                        Monto = Pagar,
                                        PagoId = pagoBD.PagoId
                                    });
                                }

                                #endregion Verificación
                            }

                            #endregion Anticipo

                            #region Podemos Pagar

                            if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - total))
                            {
                                db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                {
                                    PagoId = pagoBD.PagoId,
                                    AlumnoId = pagoBD.AlumnoId,
                                    FechaGasto = DateTime.Now,
                                    HoraGasto = DateTime.Now.TimeOfDay,
                                    SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                    SaldoDespues = (Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total)),
                                    Importe = (pagoBD.Promesa - total)
                                });

                                pagoBD.EstatusId = total == 0 ? 4 : 14;

                                if (refcabecero > 0)
                                {
                                    db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                    {
                                        PagoId = pagoBD.PagoId,
                                        ReferenciaId = Referencia.referenciaId,
                                        FechaPago = Referencia.fechaPago,
                                        Importe = pagoBD.Promesa - total,
                                        FechaProcesado = DateTime.Now,
                                        HoraProcesado = DateTime.Now.TimeOfDay,
                                        EsReferenciado = false,
                                        ReferenciaProcesadaId = null
                                    });

                                    var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                    RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - total);
                                    RefCabecero.EstatusId = 7;
                                    RefCabecero.FechaAplicacion = DateTime.Now;
                                }

                                else
                                {
                                    List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                    Detalles.Add(new ReferenciadoDetalle
                                    {
                                        PagoId = pagoBD.PagoId,
                                        ReferenciaId = Referencia.referenciaId,
                                        FechaPago = Referencia.fechaPago,
                                        Importe = pagoBD.Promesa - total,
                                        FechaProcesado = DateTime.Now,
                                        HoraProcesado = DateTime.Now.TimeOfDay,
                                        EsReferenciado = false,
                                        ReferenciaProcesadaId = null
                                    });

                                    db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                    {
                                        ImporteTotal = pagoBD.Promesa - total,
                                        ReferenciadoDetalle = Detalles,
                                        ReferenciaId = Referencia.referenciaId,
                                        PagoId = pagoBD.PagoId,
                                        ImporteRestante = 0,
                                        FechaAplicacion = DateTime.Now,
                                        EstatusId = 7 //Se marca como usado
                                    });
                                }

                                Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total);
                            }

                            #endregion PodemosPagar

                            #region No Podemos Pagar

                            else if ((Alumno.FirstOrDefault().Saldo < (pagoBD.Promesa - total)) && Alumno.FirstOrDefault().Saldo > 0)
                            {
                                db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                {
                                    PagoId = pagoBD.PagoId,
                                    AlumnoId = pagoBD.AlumnoId,
                                    FechaGasto = DateTime.Now,
                                    HoraGasto = DateTime.Now.TimeOfDay,
                                    SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                    SaldoDespues = (Alumno.FirstOrDefault().Saldo - (Alumno.FirstOrDefault().Saldo)),
                                    Importe = Alumno.FirstOrDefault().Saldo
                                });

                                if (cuenta > 0)
                                {
                                    db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                    {
                                        PagoId = pagoBD.PagoId,
                                        ReferenciaId = Referencia.referenciaId,
                                        FechaPago = Referencia.fechaPago,
                                        Importe = Alumno.FirstOrDefault().Saldo,
                                        FechaProcesado = DateTime.Now,
                                        HoraProcesado = DateTime.Now.TimeOfDay,
                                        EsReferenciado = false,
                                        ReferenciaProcesadaId = null
                                    });

                                    var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                    Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                    Referenciado.ImporteRestante = 0;
                                }

                                else
                                {
                                    List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                    Detalles.Add(new DAL.ReferenciadoDetalle
                                    {
                                        PagoId = pagoBD.PagoId,
                                        ReferenciaId = Referencia.referenciaId,
                                        FechaPago = Referencia.fechaPago,
                                        Importe = Alumno.FirstOrDefault().Saldo,
                                        FechaProcesado = DateTime.Now,
                                        HoraProcesado = DateTime.Now.TimeOfDay,
                                        EsReferenciado = false,
                                        ReferenciaProcesadaId = null
                                    });

                                    db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                    {
                                        ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                        ReferenciadoDetalle = Detalles,
                                        ReferenciaId = Referencia.referenciaId,
                                        PagoId = pagoBD.PagoId,
                                        ImporteRestante = 0,
                                        EstatusId = 8
                                    });
                                }

                                Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                            }

                            #endregion No podemos Pagar

                            db.SaveChanges();
                        }

                        #endregion Inscripcion 802

                        #region Adelanto de Materia 304

                        for (int i = 0; i < 5; i++)
                            #region Ciclo 1

                            if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 304 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                            {
                                //Obtenemos la colegiatura mas antigua....
                                int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                                int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                                int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 304 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                                var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                                && x.Cuota1.PagoConceptoId == 304
                                                                && x.Anio == minimoAnio
                                                                && x.PeriodoId == minimoPeriodo
                                                                && x.SubperiodoId == minimoSubperiodo
                                                                && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                                var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId).FirstOrDefault().ImporteTotal : 0);
                                var cuenta = db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId);
                                var parcial = (db.PagoParcial.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.PagoParcial.Where(x => x.PagoId == pagoBD.PagoId).Sum(x => x.Pago) : 0);
                                var total = refcabecero + parcial;

                                #region Podemos Pagar

                                if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - total))
                                {
                                    db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                    {
                                        PagoId = pagoBD.PagoId,
                                        AlumnoId = pagoBD.AlumnoId,
                                        FechaGasto = DateTime.Now,
                                        HoraGasto = DateTime.Now.TimeOfDay,
                                        SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                        SaldoDespues = (Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total)),
                                        Importe = (pagoBD.Promesa - total)
                                    });

                                    pagoBD.EstatusId = total == 0 ? 4 : 14;

                                    if (refcabecero > 0)
                                    {
                                        db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = pagoBD.Promesa - total,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                        RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - total);
                                        RefCabecero.EstatusId = 7;
                                        RefCabecero.FechaAplicacion = DateTime.Now;
                                    }

                                    else
                                    {
                                        List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                        Detalles.Add(new ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = pagoBD.Promesa - total,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                        {
                                            ImporteTotal = pagoBD.Promesa - total,
                                            ReferenciadoDetalle = Detalles,
                                            ReferenciaId = Referencia.referenciaId,
                                            PagoId = pagoBD.PagoId,
                                            ImporteRestante = 0,
                                            FechaAplicacion = DateTime.Now,
                                            EstatusId = 7 //Se marca como usado
                                        });
                                    }

                                    Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total);
                                }

                                #endregion PodemosPagar

                                #region No Podemos Pagar

                                else if ((Alumno.FirstOrDefault().Saldo < (pagoBD.Promesa - total)) && Alumno.FirstOrDefault().Saldo > 0)
                                {
                                    db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                    {
                                        PagoId = pagoBD.PagoId,
                                        AlumnoId = pagoBD.AlumnoId,
                                        FechaGasto = DateTime.Now,
                                        HoraGasto = DateTime.Now.TimeOfDay,
                                        SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                        SaldoDespues = (Alumno.FirstOrDefault().Saldo - (Alumno.FirstOrDefault().Saldo)),
                                        Importe = Alumno.FirstOrDefault().Saldo
                                    });

                                    if (cuenta > 0)
                                    {
                                        db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = Alumno.FirstOrDefault().Saldo,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                        Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                        Referenciado.ImporteRestante = 0;
                                    }

                                    else
                                    {
                                        List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                        Detalles.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = Alumno.FirstOrDefault().Saldo,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                        {
                                            ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                            ReferenciadoDetalle = Detalles,
                                            ReferenciaId = Referencia.referenciaId,
                                            PagoId = pagoBD.PagoId,
                                            ImporteRestante = 0,
                                            EstatusId = 8
                                        });
                                    }

                                    Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                                }

                                #endregion No podemos Pagar

                                db.SaveChanges();
                            }
                            else
                                break;

                            #endregion Ciclo 1

                        #endregion Adelanto de Materia 304

                        #region Adelanto de Materia 320

                        for (int i = 0; i < 5; i++)
                            #region Ciclo 1

                            if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 320 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                            {
                                //Obtenemos la colegiatura mas antigua....
                                int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                                int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                                int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 320 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                                var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                                && x.Cuota1.PagoConceptoId == 320
                                                                && x.Anio == minimoAnio
                                                                && x.PeriodoId == minimoPeriodo
                                                                && x.SubperiodoId == minimoSubperiodo
                                                                && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                                var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId).FirstOrDefault().ImporteTotal : 0);
                                var cuenta = db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId);
                                var parcial = (db.PagoParcial.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.PagoParcial.Where(x => x.PagoId == pagoBD.PagoId).Sum(x => x.Pago) : 0);
                                var total = refcabecero + parcial;

                                #region Podemos Pagar

                                if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - total))
                                {
                                    db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                    {
                                        PagoId = pagoBD.PagoId,
                                        AlumnoId = pagoBD.AlumnoId,
                                        FechaGasto = DateTime.Now,
                                        HoraGasto = DateTime.Now.TimeOfDay,
                                        SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                        SaldoDespues = (Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total)),
                                        Importe = (pagoBD.Promesa - total)
                                    });

                                    pagoBD.EstatusId = total == 0 ? 4 : 14;

                                    if (refcabecero > 0)
                                    {
                                        db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = pagoBD.Promesa - total,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                        RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - total);
                                        RefCabecero.EstatusId = 7;
                                        RefCabecero.FechaAplicacion = DateTime.Now;
                                    }

                                    else
                                    {
                                        List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                        Detalles.Add(new ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = pagoBD.Promesa - total,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                        {
                                            ImporteTotal = pagoBD.Promesa - total,
                                            ReferenciadoDetalle = Detalles,
                                            ReferenciaId = Referencia.referenciaId,
                                            PagoId = pagoBD.PagoId,
                                            ImporteRestante = 0,
                                            FechaAplicacion = DateTime.Now,
                                            EstatusId = 7 //Se marca como usado
                                        });
                                    }

                                    Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total);
                                }

                                #endregion PodemosPagar

                                #region No Podemos Pagar

                                else if ((Alumno.FirstOrDefault().Saldo < (pagoBD.Promesa - total)) && Alumno.FirstOrDefault().Saldo > 0)
                                {

                                    db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                    {
                                        PagoId = pagoBD.PagoId,
                                        AlumnoId = pagoBD.AlumnoId,
                                        FechaGasto = DateTime.Now,
                                        HoraGasto = DateTime.Now.TimeOfDay,
                                        SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                        SaldoDespues = (Alumno.FirstOrDefault().Saldo - (Alumno.FirstOrDefault().Saldo)),
                                        Importe = Alumno.FirstOrDefault().Saldo
                                    });

                                    if (cuenta > 0)
                                    {
                                        db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = Alumno.FirstOrDefault().Saldo,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                        Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                        Referenciado.ImporteRestante = 0;
                                    }

                                    else
                                    {
                                        List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                        Detalles.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = Alumno.FirstOrDefault().Saldo,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                        {
                                            ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                            ReferenciadoDetalle = Detalles,
                                            ReferenciaId = Referencia.referenciaId,
                                            PagoId = pagoBD.PagoId,
                                            ImporteRestante = 0,
                                            EstatusId = 8
                                        });
                                    }

                                    Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                                }

                                #endregion No podemos Pagar

                                db.SaveChanges();
                            }
                            else
                                break;

                            #endregion Ciclo 1

                        #endregion Adelanto de Materia 320

                        #region Asesoria 15

                        for (int i = 0; i < 5; i++)
                            #region Ciclo 1

                            if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 15 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                            {
                                //Obtenemos la colegiatura mas antigua....
                                int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                                int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                                int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 15 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                                var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                                && x.Cuota1.PagoConceptoId == 15
                                                                && x.Anio == minimoAnio
                                                                && x.PeriodoId == minimoPeriodo
                                                                && x.SubperiodoId == minimoSubperiodo
                                                                && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                                var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId).FirstOrDefault().ImporteTotal : 0);
                                var cuenta = db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId);
                                var parcial = (db.PagoParcial.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.PagoParcial.Where(x => x.PagoId == pagoBD.PagoId).Sum(x => x.Pago) : 0);
                                var total = refcabecero + parcial;

                                #region Podemos Pagar

                                if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - total))
                                {

                                    db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                    {
                                        PagoId = pagoBD.PagoId,
                                        AlumnoId = pagoBD.AlumnoId,
                                        FechaGasto = DateTime.Now,
                                        HoraGasto = DateTime.Now.TimeOfDay,
                                        SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                        SaldoDespues = (Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total)),
                                        Importe = (pagoBD.Promesa - total)
                                    });

                                    pagoBD.EstatusId = total == 0 ? 4 : 14;

                                    if (refcabecero > 0)
                                    {
                                        db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = pagoBD.Promesa - total,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                        RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - total);
                                        RefCabecero.EstatusId = 7;
                                        RefCabecero.FechaAplicacion = DateTime.Now;
                                    }

                                    else
                                    {
                                        List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                        Detalles.Add(new ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = pagoBD.Promesa - total,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                        {
                                            ImporteTotal = pagoBD.Promesa - total,
                                            ReferenciadoDetalle = Detalles,
                                            ReferenciaId = Referencia.referenciaId,
                                            PagoId = pagoBD.PagoId,
                                            ImporteRestante = 0,
                                            FechaAplicacion = DateTime.Now,
                                            EstatusId = 7 //Se marca como usado
                                        });
                                    }

                                    Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total);
                                }

                                #endregion PodemosPagar

                                #region No Podemos Pagar

                                else if ((Alumno.FirstOrDefault().Saldo < (pagoBD.Promesa - total)) && Alumno.FirstOrDefault().Saldo > 0)
                                {
                                    db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                    {
                                        PagoId = pagoBD.PagoId,
                                        AlumnoId = pagoBD.AlumnoId,
                                        FechaGasto = DateTime.Now,
                                        HoraGasto = DateTime.Now.TimeOfDay,
                                        SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                        SaldoDespues = (Alumno.FirstOrDefault().Saldo - (Alumno.FirstOrDefault().Saldo)),
                                        Importe = Alumno.FirstOrDefault().Saldo
                                    });

                                    if (cuenta > 0)
                                    {
                                        db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = Alumno.FirstOrDefault().Saldo,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                        Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                        Referenciado.ImporteRestante = 0;
                                    }

                                    else
                                    {
                                        List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                        Detalles.Add(new DAL.ReferenciadoDetalle
                                        {
                                            PagoId = pagoBD.PagoId,
                                            ReferenciaId = Referencia.referenciaId,
                                            FechaPago = Referencia.fechaPago,
                                            Importe = Alumno.FirstOrDefault().Saldo,
                                            FechaProcesado = DateTime.Now,
                                            HoraProcesado = DateTime.Now.TimeOfDay,
                                            EsReferenciado = false,
                                            ReferenciaProcesadaId = null
                                        });

                                        db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                        {
                                            ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                            ReferenciadoDetalle = Detalles,
                                            ReferenciaId = Referencia.referenciaId,
                                            PagoId = pagoBD.PagoId,
                                            ImporteRestante = 0,
                                            EstatusId = 8
                                        });
                                    }

                                    Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                                }

                                #endregion No podemos Pagar

                                db.SaveChanges();
                            }
                            else
                                break;

                        #endregion Ciclo 1

                        #endregion Asesoria 15

                        if (aplicaIdiomas)
                        {
                            #region Colegiaturas 807

                            for (int i = 0; i < 5; i++)
                                #region Ciclo 1

                                if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 807 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                                {
                                    //Obtenemos la colegiatura mas antigua....
                                    int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                                    int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                                    int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 807 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                                    var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                                    && x.Cuota1.PagoConceptoId == 807
                                                                    && x.Anio == minimoAnio
                                                                    && x.PeriodoId == minimoPeriodo
                                                                    && x.SubperiodoId == minimoSubperiodo
                                                                    && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                                    var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId).FirstOrDefault().ImporteTotal : 0);
                                    var cuenta = db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId);
                                    var parcial = (db.PagoParcial.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.PagoParcial.Where(x => x.PagoId == pagoBD.PagoId).Sum(x => x.Pago) : 0);
                                    var total = refcabecero + parcial;

                                    #region Podemos Pagar

                                    if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - total))
                                    {

                                        db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                        {
                                            PagoId = pagoBD.PagoId,
                                            AlumnoId = pagoBD.AlumnoId,
                                            FechaGasto = DateTime.Now,
                                            HoraGasto = DateTime.Now.TimeOfDay,
                                            SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                            SaldoDespues = (Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total)),
                                            Importe = (pagoBD.Promesa - total)
                                        });

                                        pagoBD.EstatusId = total == 0 ? 4 : 14;

                                        if (refcabecero > 0)
                                        {
                                            db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                            {
                                                PagoId = pagoBD.PagoId,
                                                ReferenciaId = Referencia.referenciaId,
                                                FechaPago = Referencia.fechaPago,
                                                Importe = pagoBD.Promesa - total,
                                                FechaProcesado = DateTime.Now,
                                                HoraProcesado = DateTime.Now.TimeOfDay,
                                                EsReferenciado = false,
                                                ReferenciaProcesadaId = null
                                            });

                                            var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                            RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - total);
                                            RefCabecero.EstatusId = 7;
                                            RefCabecero.FechaAplicacion = DateTime.Now;
                                        }

                                        else
                                        {
                                            List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                            Detalles.Add(new ReferenciadoDetalle
                                            {
                                                PagoId = pagoBD.PagoId,
                                                ReferenciaId = Referencia.referenciaId,
                                                FechaPago = Referencia.fechaPago,
                                                Importe = pagoBD.Promesa - total,
                                                FechaProcesado = DateTime.Now,
                                                HoraProcesado = DateTime.Now.TimeOfDay,
                                                EsReferenciado = false,
                                                ReferenciaProcesadaId = null
                                            });

                                            db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                            {
                                                ImporteTotal = pagoBD.Promesa - total,
                                                ReferenciadoDetalle = Detalles,
                                                ReferenciaId = Referencia.referenciaId,
                                                PagoId = pagoBD.PagoId,
                                                ImporteRestante = 0,
                                                FechaAplicacion = DateTime.Now,
                                                EstatusId = 7 //Se marca como usado
                                            });
                                        }

                                        Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total);
                                    }

                                    #endregion PodemosPagar

                                    #region No Podemos Pagar

                                    else if ((Alumno.FirstOrDefault().Saldo < (pagoBD.Promesa - total)) && Alumno.FirstOrDefault().Saldo > 0)
                                    {
                                        db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                        {
                                            PagoId = pagoBD.PagoId,
                                            AlumnoId = pagoBD.AlumnoId,
                                            FechaGasto = DateTime.Now,
                                            HoraGasto = DateTime.Now.TimeOfDay,
                                            SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                            SaldoDespues = (Alumno.FirstOrDefault().Saldo - (Alumno.FirstOrDefault().Saldo)),
                                            Importe = Alumno.FirstOrDefault().Saldo
                                        });

                                        if (cuenta > 0)
                                        {
                                            db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                            {
                                                PagoId = pagoBD.PagoId,
                                                ReferenciaId = Referencia.referenciaId,
                                                FechaPago = Referencia.fechaPago,
                                                Importe = Alumno.FirstOrDefault().Saldo,
                                                FechaProcesado = DateTime.Now,
                                                HoraProcesado = DateTime.Now.TimeOfDay,
                                                EsReferenciado = false,
                                                ReferenciaProcesadaId = null
                                            });

                                            var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                            Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                            Referenciado.ImporteRestante = 0;
                                        }

                                        else
                                        {
                                            List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                            Detalles.Add(new DAL.ReferenciadoDetalle
                                            {
                                                PagoId = pagoBD.PagoId,
                                                ReferenciaId = Referencia.referenciaId,
                                                FechaPago = Referencia.fechaPago,
                                                Importe = Alumno.FirstOrDefault().Saldo,
                                                FechaProcesado = DateTime.Now,
                                                HoraProcesado = DateTime.Now.TimeOfDay,
                                                EsReferenciado = false,
                                                ReferenciaProcesadaId = null
                                            });

                                            db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                            {
                                                ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                                ReferenciadoDetalle = Detalles,
                                                ReferenciaId = Referencia.referenciaId,
                                                PagoId = pagoBD.PagoId,
                                                ImporteRestante = 0,
                                                EstatusId = 8
                                            });
                                        }

                                        Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                                    }

                                    #endregion No podemos Pagar

                                    db.SaveChanges();
                                }
                                else
                                    break;

                            #endregion Ciclo 1

                            #endregion Colegiaturas 807

                            #region Material Didactico 808

                            for (int i = 0; i < 5; i++)
                                #region Ciclo 1

                                if (PagosPendientes.Count(p => p.Cuota1.PagoConceptoId == 808 && (p.EstatusId == 1 || p.EstatusId == 13)) > 0 && Alumno.FirstOrDefault().Saldo > 0)
                                {
                                    //Obtenemos la colegiatura mas antigua....
                                    int minimoAnio = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13)).Min(a => a.Anio);
                                    int minimoPeriodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio).Min(a => a.PeriodoId);
                                    int minimoSubperiodo = PagosPendientes.Where(a => a.Cuota1.PagoConceptoId == 808 && a.AlumnoId == Alumno.FirstOrDefault().AlumnoId && (a.EstatusId == 1 || a.EstatusId == 13) && a.Anio == minimoAnio && a.PeriodoId == minimoPeriodo).Min(a => a.SubperiodoId);
                                    var pagoBD = PagosPendientes.Where(x => x.AlumnoId == Alumno.FirstOrDefault().AlumnoId
                                                                    && x.Cuota1.PagoConceptoId == 808
                                                                    && x.Anio == minimoAnio
                                                                    && x.PeriodoId == minimoPeriodo
                                                                    && x.SubperiodoId == minimoSubperiodo
                                                                    && (x.EstatusId == 1 || x.EstatusId == 13)).FirstOrDefault();

                                    var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == pagoBD.PagoId).FirstOrDefault().ImporteTotal : 0);
                                    var cuenta = db.ReferenciadoCabecero.Count(x => x.PagoId == pagoBD.PagoId);
                                    var parcial = (db.PagoParcial.Count(x => x.PagoId == pagoBD.PagoId) > 0 ? db.PagoParcial.Where(x => x.PagoId == pagoBD.PagoId).Sum(x => x.Pago) : 0);
                                    var total = refcabecero + parcial;

                                    #region Podemos Pagar

                                    if (Alumno.FirstOrDefault().Saldo >= (pagoBD.Promesa - total))
                                    {
                                        db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                        {
                                            PagoId = pagoBD.PagoId,
                                            AlumnoId = pagoBD.AlumnoId,
                                            FechaGasto = DateTime.Now,
                                            HoraGasto = DateTime.Now.TimeOfDay,
                                            SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                            SaldoDespues = (Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total)),
                                            Importe = (pagoBD.Promesa - total)
                                        });

                                        pagoBD.EstatusId = total == 0 ? 4 : 14;

                                        if (refcabecero > 0)
                                        {
                                            db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                            {
                                                PagoId = pagoBD.PagoId,
                                                ReferenciaId = Referencia.referenciaId,
                                                FechaPago = Referencia.fechaPago,
                                                Importe = pagoBD.Promesa - total,
                                                FechaProcesado = DateTime.Now,
                                                HoraProcesado = DateTime.Now.TimeOfDay,
                                                EsReferenciado = false,
                                                ReferenciaProcesadaId = null
                                            });

                                            var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == pagoBD.PagoId).FirstOrDefault();
                                            RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (pagoBD.Promesa - total);
                                            RefCabecero.EstatusId = 7;
                                            RefCabecero.FechaAplicacion = DateTime.Now;
                                        }

                                        else
                                        {
                                            List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                            Detalles.Add(new ReferenciadoDetalle
                                            {
                                                PagoId = pagoBD.PagoId,
                                                ReferenciaId = Referencia.referenciaId,
                                                FechaPago = Referencia.fechaPago,
                                                Importe = pagoBD.Promesa - total,
                                                FechaProcesado = DateTime.Now,
                                                HoraProcesado = DateTime.Now.TimeOfDay,
                                                EsReferenciado = false,
                                                ReferenciaProcesadaId = null
                                            });

                                            db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                            {
                                                ImporteTotal = pagoBD.Promesa - total,
                                                ReferenciadoDetalle = Detalles,
                                                ReferenciaId = Referencia.referenciaId,
                                                PagoId = pagoBD.PagoId,
                                                ImporteRestante = 0,
                                                FechaAplicacion = DateTime.Now,
                                                EstatusId = 7 //Se marca como usado
                                            });
                                        }

                                        Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (pagoBD.Promesa - total);
                                    }

                                    #endregion PodemosPagar

                                    #region No Podemos Pagar

                                    else if ((Alumno.FirstOrDefault().Saldo < (pagoBD.Promesa - total)) && Alumno.FirstOrDefault().Saldo > 0)
                                    {
                                        db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                        {
                                            PagoId = pagoBD.PagoId,
                                            AlumnoId = pagoBD.AlumnoId,
                                            FechaGasto = DateTime.Now,
                                            HoraGasto = DateTime.Now.TimeOfDay,
                                            SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                            SaldoDespues = (Alumno.FirstOrDefault().Saldo - (Alumno.FirstOrDefault().Saldo)),
                                            Importe = Alumno.FirstOrDefault().Saldo
                                        });

                                        if (cuenta > 0)
                                        {
                                            db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                            {
                                                PagoId = pagoBD.PagoId,
                                                ReferenciaId = Referencia.referenciaId,
                                                FechaPago = Referencia.fechaPago,
                                                Importe = Alumno.FirstOrDefault().Saldo,
                                                FechaProcesado = DateTime.Now,
                                                HoraProcesado = DateTime.Now.TimeOfDay,
                                                EsReferenciado = false,
                                                ReferenciaProcesadaId = null
                                            });

                                            var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == pagoBD.PagoId).FirstOrDefault();
                                            Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                            Referenciado.ImporteRestante = 0;
                                        }

                                        else
                                        {
                                            List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                            Detalles.Add(new DAL.ReferenciadoDetalle
                                            {
                                                PagoId = pagoBD.PagoId,
                                                ReferenciaId = Referencia.referenciaId,
                                                FechaPago = Referencia.fechaPago,
                                                Importe = Alumno.FirstOrDefault().Saldo,
                                                FechaProcesado = DateTime.Now,
                                                HoraProcesado = DateTime.Now.TimeOfDay,
                                                EsReferenciado = false,
                                                ReferenciaProcesadaId = null
                                            });

                                            db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                            {
                                                ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                                ReferenciadoDetalle = Detalles,
                                                ReferenciaId = Referencia.referenciaId,
                                                PagoId = pagoBD.PagoId,
                                                ImporteRestante = 0,
                                                EstatusId = 8
                                            });
                                        }

                                        Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                                    }

                                    #endregion No podemos Pagar

                                    db.SaveChanges();
                                }
                                else
                                    break;

                            #endregion Ciclo 1

                            #endregion Material Didactico 808
                        }

                        db.SaveChanges();
                    }
                    
                    #region Adeudos 2015

                        PagosPendientesAnteriores.AddRange(
                           (from a in db.Pago
                            where (a.EstatusId == 1 || a.EstatusId == 13)
                                  && (a.Anio == 2016 && a.PeriodoId == 1)
                                  && (a.Promesa > 0)
                                  && a.AlumnoId == alumnoId
                            select a).ToList());

                        if (PagosPendientesAnteriores.Count > 0 && Alumno.FirstOrDefault().Saldo > 0)
                        {
                            int pId = PagosPendientesAnteriores.FirstOrDefault().PagoId;

                            var Variable = (from a in db.Pago
                                            where a.PagoId == pId
                                            select a).ToList().FirstOrDefault();

                            var refcabecero = (db.ReferenciadoCabecero.Count(x => x.PagoId == Variable.PagoId) > 0 ? db.ReferenciadoCabecero.Where(x => x.PagoId == Variable.PagoId).FirstOrDefault().ImporteTotal : 0);
                            var cuenta = db.ReferenciadoCabecero.Count(x => x.PagoId == Variable.PagoId);
                            var parcial = (db.PagoParcial.Count(x => x.PagoId == Variable.PagoId) > 0 ? db.PagoParcial.Where(x => x.PagoId == Variable.PagoId).Sum(x => x.Pago) : 0);
                            var total = refcabecero + parcial;

                            #region Podemos Pagar

                            if (Alumno.FirstOrDefault().Saldo >= (Variable.Promesa - total))
                            {
                                Variable.EstatusId = total == 0 ? 4 : 14;

                                db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                {
                                    PagoId = Variable.PagoId,
                                    AlumnoId = Variable.AlumnoId,
                                    FechaGasto = DateTime.Now,
                                    HoraGasto = DateTime.Now.TimeOfDay,
                                    SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                    SaldoDespues = (Alumno.FirstOrDefault().Saldo - (Variable.Promesa - total)),
                                    Importe = (Variable.Promesa - total)
                                });

                                if (refcabecero > 0)
                                {
                                    db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                    {
                                        PagoId = Variable.PagoId,
                                        ReferenciaId = Referencia.referenciaId,
                                        FechaPago = Referencia.fechaPago,
                                        Importe = Variable.Promesa - total,
                                        FechaProcesado = DateTime.Now,
                                        HoraProcesado = DateTime.Now.TimeOfDay,
                                        EsReferenciado = false,
                                        ReferenciaProcesadaId = null
                                    });

                                    var RefCabecero = db.ReferenciadoCabecero.Where(a => a.PagoId == Variable.PagoId).FirstOrDefault();
                                    RefCabecero.ImporteTotal = RefCabecero.ImporteTotal + (Variable.Promesa - total);
                                    RefCabecero.EstatusId = 7;
                                    RefCabecero.FechaAplicacion = DateTime.Now;
                                }

                                else
                                {
                                    List<DAL.ReferenciadoDetalle> Detalles = new List<ReferenciadoDetalle>();
                                    Detalles.Add(new ReferenciadoDetalle
                                    {
                                        PagoId = Variable.PagoId,
                                        ReferenciaId = Referencia.referenciaId,
                                        FechaPago = Referencia.fechaPago,
                                        Importe = Variable.Promesa - total,
                                        FechaProcesado = DateTime.Now,
                                        HoraProcesado = DateTime.Now.TimeOfDay,
                                        EsReferenciado = false,
                                        ReferenciaProcesadaId = null
                                    });

                                    db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                    {
                                        ImporteTotal = Variable.Promesa - total,
                                        ReferenciadoDetalle = Detalles,
                                        ReferenciaId = Referencia.referenciaId,
                                        PagoId = Variable.PagoId,
                                        ImporteRestante = 0,
                                        FechaAplicacion = DateTime.Now,
                                        EstatusId = 7 //Se marca como usado
                                    });
                                }

                                Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - (Variable.Promesa - total);
                            }

                        #endregion PodemosPagar

                            #region No Podemos Pagar

                        else if ((Alumno.FirstOrDefault().Saldo < (Variable.Promesa - total)) && Alumno.FirstOrDefault().Saldo > 0)
                        {
                                db.AlumnoSaldoGasto.Add(new AlumnoSaldoGasto
                                {
                                    PagoId = Variable.PagoId,
                                    AlumnoId = Variable.AlumnoId,
                                    FechaGasto = DateTime.Now,
                                    HoraGasto = DateTime.Now.TimeOfDay,
                                    SaldoAnterior = Alumno.FirstOrDefault().Saldo,
                                    SaldoDespues = (Alumno.FirstOrDefault().Saldo - (Alumno.FirstOrDefault().Saldo)),
                                    Importe = Alumno.FirstOrDefault().Saldo
                                });

                            if (cuenta > 0)
                            {
                                    db.ReferenciadoDetalle.Add(new DAL.ReferenciadoDetalle
                                    {
                                        PagoId = Variable.PagoId,
                                        ReferenciaId = Referencia.referenciaId,
                                        FechaPago = Referencia.fechaPago,
                                        Importe = Alumno.FirstOrDefault().Saldo,
                                        FechaProcesado = DateTime.Now,
                                        HoraProcesado = DateTime.Now.TimeOfDay,
                                        EsReferenciado = false,
                                        ReferenciaProcesadaId = null
                                    });

                                    var Referenciado = db.ReferenciadoCabecero.Where(pr => pr.PagoId == Variable.PagoId).FirstOrDefault();
                                    Referenciado.ImporteTotal = Referenciado.ImporteTotal + Alumno.FirstOrDefault().Saldo;
                                    Referenciado.ImporteRestante = 0;
                                }

                                else
                                {
                                    List<DAL.ReferenciadoDetalle> Detalles = new List<DAL.ReferenciadoDetalle>();

                                    Detalles.Add(new DAL.ReferenciadoDetalle
                                    {
                                        PagoId = Variable.PagoId,
                                        ReferenciaId = Referencia.referenciaId,
                                        FechaPago = Referencia.fechaPago,
                                        Importe = Alumno.FirstOrDefault().Saldo,
                                        FechaProcesado = DateTime.Now,
                                        HoraProcesado = DateTime.Now.TimeOfDay,
                                        EsReferenciado = false,
                                        ReferenciaProcesadaId = null
                                    });

                                    db.ReferenciadoCabecero.Add(new DAL.ReferenciadoCabecero
                                    {
                                        ImporteTotal = Alumno.FirstOrDefault().Saldo,
                                        ReferenciadoDetalle = Detalles,
                                        ReferenciaId = Referencia.referenciaId,
                                        PagoId = Variable.PagoId,
                                        ImporteRestante = 0,
                                        EstatusId = 8
                                    });
                                }

                                Alumno.FirstOrDefault().Saldo = Alumno.FirstOrDefault().Saldo - Alumno.FirstOrDefault().Saldo;
                            }

                            #endregion No podemos Pagar

                            db.SaveChanges();
                        }
                    #endregion Adeudos 2015
                }
            }
        }
    }
}
