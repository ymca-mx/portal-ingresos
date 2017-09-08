using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data.Entity;
using Herramientas;

namespace BLL
{
    public class BLLAlumnoDescuento
    {
        /// <summary>
        /// Insertar Descuentos de los alumnos
        /// </summary>
        /// <param name="AlumnoId">Numero del Alumno</param>
        /// <param name="DescBeca">Porcentaje de Beca</param>
        /// <param name="DescInsc">Porcentaje de Incripcion</param>
        /// <param name="ComentarioInsc">Comentario Inscripcion</param>
        /// <param name="ComentarioBeca">Comentario Beca</param>
        /// <param name="DescExam">Porcetaje de Examen</param>
        /// <param name="ComentarioExam">Comentario de Examen</param>
        /// <param name="DescCred">Porcentaje de Credencial</param>
        /// <param name="ComentarioCred">Comentario de Credencial</param>
        /// <param name="PagoPlan">Pago plan </param>
        /// <returns></returns>
        public static string[] InsertarDescuentoNormal(int AlumnoId, decimal DescBeca, decimal DescInsc, string ComentarioInsc, string ComentarioBeca,
            decimal DescExam, string ComentarioExam, decimal DescCred, string ComentarioCred, int PagoPlan, int UsuarioId, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTOPagoPlan objPlanpago = (from a in db.PagoPlan
                                           where a.PagoPlanId == PagoPlan
                                           select new DTOPagoPlan
                                           {
                                               Descripcion = a.Descripcion,
                                               EstatusId = a.EstatusId,
                                               PagoPlanId = a.PagoPlanId,
                                               Pagos = a.Pagos,
                                               PlanPago = a.PlanPago
                                           }).FirstOrDefault();
                string[] AlumnoID = null;
                DTOAlumnoInscrito objOferta = OfertaEducativaId == 0 ?
                    BLLAlumnoInscrito.ConsultarAlumnoInscrito(AlumnoId) : 
                    BLLAlumnoInscrito.ConsultarAlumnoInscrito(AlumnoId, OfertaEducativaId);

                DTODescuentos objDescuentoIn = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 802);

                DTODescuentos objDescuentoBec = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 800, "Beca Académica");

                DTODescuentos objDescuentoExam = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 1);
                //Descuento en Credencial
                DTODescuentos objDescuentoCred = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 1000);

                DTOCuota objCuotaIn = BLLCuota.traerCuotaParametros(objOferta, objDescuentoIn);

                DTOCuota objCuotaBec = BLLCuota.traerCuotaParametros(objOferta, objDescuentoBec);

                DTOCuota objCuotaExam = BLLCuota.traerCuotaParametros(objOferta, objDescuentoExam);
                //Cuota Credencial
                DTOCuota objCuotaCred = BLLCuota.traerCuotaParametros(objOferta, objDescuentoCred);

                Usuario objUser = db.Usuario.Where(u => u.UsuarioId == UsuarioId).FirstOrDefault();
                if (objPlanpago.Pagos == 1)
                {
                    objCuotaBec.Monto = objCuotaBec.Monto * 4;
                }

                try
                {
                    //Beca 
                    if (DescBeca > 0)
                    {
                        db.AlumnoDescuento.Add(new AlumnoDescuento
                        {
                            AlumnoId = AlumnoId,
                            OfertaEducativaId = objOferta.OfertaEducativaId,
                            Anio = objOferta.Anio,
                            PeriodoId = objOferta.PeriodoId,
                            DescuentoId = objDescuentoBec.DescuentoId,
                            PagoConceptoId = objDescuentoBec.PagoConceptoId,
                            Monto = DescBeca,
                            //UsuarioId = 7255,
                            UsuarioId = UsuarioId,
                            Comentario = ComentarioBeca,
                            EstatusId = 2,
                            FechaGeneracion = DateTime.Now,
                            FechaAplicacion = DateTime.Now,
                            HoraGeneracion = DateTime.Now.TimeOfDay,
                            EsSEP = false,
                            EsComite = false,
                            EsDeportiva = false

                        });
                    }
                    //Inscripcion
                    if (DescInsc > 0)
                    {
                        db.AlumnoDescuento.Add(new AlumnoDescuento
                        {
                            AlumnoId = AlumnoId,
                            OfertaEducativaId = objOferta.OfertaEducativaId,
                            Anio = objOferta.Anio,
                            PeriodoId = objOferta.PeriodoId,
                            DescuentoId = objDescuentoIn.DescuentoId,
                            PagoConceptoId = objDescuentoIn.PagoConceptoId,
                            Monto = DescInsc,
                            //UsuarioId = 7255,
                            UsuarioId = UsuarioId,
                            Comentario = ComentarioInsc,
                            EstatusId = 2,
                            FechaGeneracion = DateTime.Now,
                            FechaAplicacion = DateTime.Now,
                            HoraGeneracion = DateTime.Now.TimeOfDay,
                            EsSEP = false,
                            EsComite = false,
                            EsDeportiva = false
                        });
                    }
                    //Examen de Admision
                    if (DescExam > 0)
                    {
                        db.AlumnoDescuento.Add(new AlumnoDescuento
                        {
                            AlumnoId = AlumnoId,
                            OfertaEducativaId = objOferta.OfertaEducativaId,
                            Anio = objOferta.Anio,
                            PeriodoId = objOferta.PeriodoId,
                            DescuentoId = objDescuentoExam.DescuentoId,
                            PagoConceptoId = objDescuentoExam.PagoConceptoId,
                            Monto = DescExam,
                            //UsuarioId = 7255,
                            UsuarioId = UsuarioId,
                            Comentario = ComentarioExam,
                            EstatusId = 2,
                            FechaGeneracion = DateTime.Now,
                            FechaAplicacion = DateTime.Now,
                            HoraGeneracion = DateTime.Now.TimeOfDay,
                            EsSEP = false,
                            EsComite = false,
                            EsDeportiva = false
                        });
                    }
                    //Credencial
                    if (DescCred > 0)
                    {
                        db.AlumnoDescuento.Add(new AlumnoDescuento
                        {
                            AlumnoId = AlumnoId,
                            OfertaEducativaId = objOferta.OfertaEducativaId,
                            Anio = objOferta.Anio,
                            PeriodoId = objOferta.PeriodoId,
                            DescuentoId = objDescuentoCred.DescuentoId,
                            PagoConceptoId = objDescuentoCred.PagoConceptoId,
                            Monto = DescCred,
                            //UsuarioId = 7255,
                            UsuarioId = UsuarioId,
                            Comentario = ComentarioCred,
                            EstatusId = 2,
                            FechaGeneracion = DateTime.Now,
                            FechaAplicacion = DateTime.Now,
                            HoraGeneracion = DateTime.Now.TimeOfDay,
                            EsSEP = false,
                            EsComite = false,
                            EsDeportiva = false
                        });
                    }
                    db.SaveChanges();

                    //SubPeriodo
                    int subPeriodo = db.OfertaEducativa.Where(o => o.OfertaEducativaId == objOferta.OfertaEducativaId).ToList().Where(a => a.OfertaEducativaTipoId == 5).ToList().Count > 0 ? 2 : 1;
                    //Pago Examen 
                    if (DescExam != -1)
                    {
                        db.Pago.Add(new Pago
                        {
                            AlumnoId = AlumnoId,
                            Anio = objOferta.Anio,
                            PeriodoId = objOferta.PeriodoId,
                            SubperiodoId = subPeriodo,
                            OfertaEducativaId = objOferta.OfertaEducativaId,
                            FechaGeneracion = DateTime.Now,
                            HoraGeneracion = DateTime.Now.TimeOfDay,
                            CuotaId = objCuotaExam.CuotaId,
                            Cuota = objCuotaExam.Monto,
                            Promesa = objCuotaExam.Monto - ((DescExam / 100) * objCuotaExam.Monto),
                            Restante = objCuotaExam.Monto - ((DescExam / 100) * objCuotaExam.Monto),
                            EstatusId = DescExam == 100 ? 4 : 1,
                            ReferenciaId = "",
                            EsEmpresa = false,
                            UsuarioId = objUser.UsuarioId,
                            UsuarioTipoId = objUser.UsuarioTipoId
                            //PagoParcial =new List<PagoParcial>{ new PagoParcial
                            //{

                            //}}
                        });
                    }
                    //Credencial
                    db.Pago.Add(new Pago
                    {
                        AlumnoId = AlumnoId,
                        Anio = objOferta.Anio,
                        PeriodoId = objOferta.PeriodoId,
                        SubperiodoId = subPeriodo,
                        OfertaEducativaId = objOferta.OfertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        HoraGeneracion = DateTime.Now.TimeOfDay,
                        CuotaId = objCuotaCred.CuotaId,
                        Cuota = objCuotaCred.Monto,
                        Promesa = objCuotaCred.Monto - ((DescCred / 100) * objCuotaCred.Monto),
                        Restante = objCuotaCred.Monto - ((DescCred / 100) * objCuotaCred.Monto),
                        EstatusId = DescCred == 100 ? 4 : 1,
                        ReferenciaId = "",
                        EsEmpresa = false,
                        UsuarioId = objUser.UsuarioId,
                        UsuarioTipoId = objUser.UsuarioTipoId
                    });
                    //Pagos Inscr
                    db.Pago.Add(new Pago
                    {
                        AlumnoId = AlumnoId,
                        Anio = objOferta.Anio,
                        PeriodoId = objOferta.PeriodoId,
                        SubperiodoId = subPeriodo,
                        OfertaEducativaId = objOferta.OfertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        HoraGeneracion = DateTime.Now.TimeOfDay,
                        CuotaId = objCuotaIn.CuotaId,
                        Cuota = objCuotaIn.Monto,
                        Promesa = objCuotaIn.Monto - ((DescInsc / 100) * objCuotaIn.Monto),
                        Restante = objCuotaIn.Monto - ((DescInsc / 100) * objCuotaIn.Monto),
                        EstatusId = DescInsc == 100 ? 4 : 1,
                        ReferenciaId = "",
                        EsEmpresa = false,
                        UsuarioId = objUser.UsuarioId,
                        UsuarioTipoId = objUser.UsuarioTipoId
                    });
                    //Obtener Periodo
                    int periodo =
                        (from a in db.Periodo
                         where DateTime.Now >= a.FechaInicial && DateTime.Now <= a.FechaFinal
                         select a.Meses).FirstOrDefault();
                    objPlanpago.Pagos = objPlanpago.Pagos == 6 ? 3 : objPlanpago.Pagos;
                    int subperiodo2 = subPeriodo == 2 ? 1 : 0;
                    for (int i = 1; i <= objPlanpago.Pagos; i++)
                    {
                        db.Pago.Add(new Pago
                        {
                            AlumnoId = AlumnoId,
                            Anio = objOferta.Anio,
                            PeriodoId = objOferta.PeriodoId,
                            SubperiodoId = subperiodo2 + i,
                            OfertaEducativaId = objOferta.OfertaEducativaId,
                            FechaGeneracion = DateTime.Now,
                            HoraGeneracion = DateTime.Now.TimeOfDay,
                            CuotaId = objCuotaBec.CuotaId,
                            Cuota = objCuotaBec.Monto,
                            Promesa = objCuotaBec.Monto - ((DescBeca / 100) * objCuotaBec.Monto),
                            Restante = objCuotaBec.Monto - ((DescBeca / 100) * objCuotaBec.Monto),
                            EstatusId = DescBeca == 100 ? 4 : 1,
                            ReferenciaId = "",
                            EsEmpresa = false,
                            UsuarioId = objUser.UsuarioId,
                            UsuarioTipoId = objUser.UsuarioTipoId
                        });
                    }
                    db.SaveChanges();
                    List<DTOPagos> lstPagos = new List<DTOPagos>();
                    foreach (Pago objPago in db.Pago.Local)
                    {
                        objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                        lstPagos.Add((from a in db.Pago
                                      where a.PagoId == objPago.PagoId
                                      select new DTOPagos
                                      {
                                          PagoId = a.PagoId,
                                          DTOCuota = new DTOCuota
                                          {
                                              PagoConceptoId = a.Cuota1.PagoConceptoId
                                          }
                                      }).AsNoTracking().FirstOrDefault());
                    }

                    //Pago Descuento
                    if (DescInsc > 0)
                    {

                        db.PagoDescuento.Add(new PagoDescuento
                        {
                            DescuentoId = db.AlumnoDescuento.Local.Where(A => A.PagoConceptoId == 802).FirstOrDefault().DescuentoId,
                            PagoId = lstPagos.Where(X => X.DTOCuota.PagoConceptoId == 802).FirstOrDefault().PagoId,
                            Monto = (DescInsc / 100) * objCuotaIn.Monto

                        });
                    }
                    //PAgo Examen
                    if (DescExam > 0)
                    {
                        db.PagoDescuento.Add(new PagoDescuento
                        {
                            DescuentoId = db.AlumnoDescuento.Local.Where(A => A.PagoConceptoId == 1).FirstOrDefault().DescuentoId,
                            PagoId = lstPagos.Where(X => X.DTOCuota.PagoConceptoId == 1).FirstOrDefault().PagoId,
                            Monto = (DescExam / 100) * objCuotaExam.Monto

                        });
                    }
                    if (DescCred > 0)
                    {
                        //pago Credencial
                        db.PagoDescuento.Add(new PagoDescuento
                        {
                            DescuentoId = db.AlumnoDescuento.Local.Where(A => A.PagoConceptoId == 1000).FirstOrDefault().DescuentoId,
                            PagoId = lstPagos.Where(X => X.DTOCuota.PagoConceptoId == 1000).FirstOrDefault().PagoId,
                            Monto = (DescCred / 100) * objCuotaCred.Monto
                        });
                    }
                    if (DescBeca > 0)
                    {
                        List<DTOPagos> lsPagos2 = lstPagos.Where(X => X.DTOCuota.PagoConceptoId == 800).ToList();
                        //Pagos Periodos
                        lsPagos2.ForEach(delegate (DTOPagos objPago)
                        {
                            db.PagoDescuento.Add(new PagoDescuento
                            {
                                DescuentoId = db.AlumnoDescuento.Local.Where(A => A.PagoConceptoId == 800).FirstOrDefault().DescuentoId,
                                PagoId = objPago.PagoId,
                                Monto = (DescBeca / 100) * objCuotaBec.Monto
                            });
                        });


                    }
                    db.SaveChanges();
                    List<string> lstAlDes = new List<string>();
                    foreach (AlumnoDescuento objAl in db.AlumnoDescuento.Local)
                    {
                        lstAlDes.Add(objAl.AlumnoDescuentoId.ToString());
                    }
                    AlumnoID = lstAlDes.ToArray();
                    return AlumnoID;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    return null;
                }

            }
        }

        //public static string[] InsertarDescuento(int AlumnoId, decimal DescBeca, decimal DescInsc, string ComentarioInsc, string ComentarioBeca,
        //    decimal DescExam, string ComentarioExam, decimal DescCred, string ComentarioCred, int OfertaEducativaId)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        string[] AlumnoID;
        //        DTOAlumnoInscrito objOferta = BLLAlumnoInscrito.ConsultarAlumnoInscrito(AlumnoId, OfertaEducativaId);

        //        DTODescuentos objDescuentoIn = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 802);

        //        DTODescuentos objDescuentoBec = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 800, "Beca Académica");

        //        DTODescuentos objDescuentoExam = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 1);
        //        //Descuento en Credencial
        //        DTODescuentos objDescuentoCred = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 1000);

        //        DTOCuota objCuotaIn = BLLCuota.traerCuotaParametros(objOferta, objDescuentoIn);

        //        DTOCuota objCuotaBec = BLLCuota.traerCuotaParametros(objOferta, objDescuentoBec);

        //        DTOCuota objCuotaExam = BLLCuota.traerCuotaParametros(objOferta, objDescuentoExam);
        //        //Cuota Credencial
        //        DTOCuota objCuotaCred = BLLCuota.traerCuotaParametros(objOferta, objDescuentoCred);

        //        try
        //        {
        //            //Beca 
        //            if (DescBeca > 0)
        //            {
        //                db.AlumnoDescuento.Add(new AlumnoDescuento
        //                {
        //                    AlumnoId = AlumnoId,
        //                    OfertaEducativaId = objOferta.OfertaEducativaId,
        //                    Anio = objOferta.Anio,
        //                    PeriodoId = objOferta.PeriodoId,
        //                    DescuentoId = objDescuentoBec.DescuentoId,
        //                    PagoConceptoId = objDescuentoBec.PagoConceptoId,
        //                    Monto = DescBeca,
        //                    UsuarioId = 7255,
        //                    Comentario = ComentarioBeca,
        //                    EstatusId = 2,
        //                    FechaGeneracion = DateTime.Now,
        //                    FechaAplicacion = DateTime.Now
        //                });
        //            }
        //            //Inscripcion
        //            if (DescInsc > 0)
        //            {
        //                db.AlumnoDescuento.Add(new AlumnoDescuento
        //                {
        //                    AlumnoId = AlumnoId,
        //                    OfertaEducativaId = objOferta.OfertaEducativaId,
        //                    Anio = objOferta.Anio,
        //                    PeriodoId = objOferta.PeriodoId,
        //                    DescuentoId = objDescuentoIn.DescuentoId,
        //                    PagoConceptoId = objDescuentoIn.PagoConceptoId,
        //                    Monto = DescInsc,
        //                    UsuarioId = 7255,
        //                    Comentario = ComentarioInsc,
        //                    EstatusId = 2,
        //                    FechaGeneracion = DateTime.Now,
        //                    FechaAplicacion = DateTime.Now
        //                });
        //            }
        //            //Examen de Admision
        //            if (DescExam > 0)
        //            {
        //                db.AlumnoDescuento.Add(new AlumnoDescuento
        //                {
        //                    AlumnoId = AlumnoId,
        //                    OfertaEducativaId = objOferta.OfertaEducativaId,
        //                    Anio = objOferta.Anio,
        //                    PeriodoId = objOferta.PeriodoId,
        //                    DescuentoId = objDescuentoExam.DescuentoId,
        //                    PagoConceptoId = objDescuentoExam.PagoConceptoId,
        //                    Monto = DescExam,
        //                    UsuarioId = 7255,
        //                    Comentario = ComentarioExam,
        //                    EstatusId = 2,
        //                    FechaGeneracion = DateTime.Now,
        //                    FechaAplicacion = DateTime.Now
        //                });
        //            }
        //            //Credencial
        //            if (DescCred > 0)
        //            {
        //                db.AlumnoDescuento.Add(new AlumnoDescuento
        //                {
        //                    AlumnoId = AlumnoId,
        //                    OfertaEducativaId = objOferta.OfertaEducativaId,
        //                    Anio = objOferta.Anio,
        //                    PeriodoId = objOferta.PeriodoId,
        //                    DescuentoId = objDescuentoCred.DescuentoId,
        //                    PagoConceptoId = objDescuentoCred.PagoConceptoId,
        //                    Monto = DescCred,
        //                    UsuarioId = 7255,
        //                    Comentario = ComentarioCred,
        //                    EstatusId = 2,
        //                    FechaGeneracion = DateTime.Now,
        //                    FechaAplicacion = DateTime.Now
        //                });
        //            }
        //            db.SaveChanges();
        //            //Pago Examen 
        //            db.Pago.Add(new Pago
        //            {
        //                AlumnoId = AlumnoId,
        //                Anio = objOferta.Anio,
        //                PeriodoId = objOferta.PeriodoId,
        //                SubperiodoId = 1,
        //                OfertaEducativaId = objOferta.OfertaEducativaId,
        //                FechaGeneracion = DateTime.Now,
        //                CuotaId = objCuotaExam.CuotaId,
        //                Cuota = objCuotaExam.Monto,
        //                Promesa = objCuotaExam.Monto - ((DescExam / 100) * objCuotaExam.Monto),
        //                EstatusId = 1,
        //                ReferenciaId = "",
        //                EsEmpresa = false
        //            });
        //            //Credencial
        //            db.Pago.Add(new Pago
        //            {
        //                AlumnoId = AlumnoId,
        //                Anio = objOferta.Anio,
        //                PeriodoId = objOferta.PeriodoId,
        //                SubperiodoId = 1,
        //                OfertaEducativaId = objOferta.OfertaEducativaId,
        //                FechaGeneracion = DateTime.Now,
        //                CuotaId = objCuotaCred.CuotaId,
        //                Cuota = objCuotaCred.Monto,
        //                Promesa = objCuotaCred.Monto - ((DescCred / 100) * objCuotaCred.Monto),
        //                EstatusId = 1,
        //                ReferenciaId = "",
        //                EsEmpresa = false
        //            });
        //            //Pagos Inscr
        //            db.Pago.Add(new Pago
        //            {
        //                AlumnoId = AlumnoId,
        //                Anio = objOferta.Anio,
        //                PeriodoId = objOferta.PeriodoId,
        //                SubperiodoId = 1,
        //                OfertaEducativaId = objOferta.OfertaEducativaId,
        //                FechaGeneracion = DateTime.Now,
        //                CuotaId = objCuotaIn.CuotaId,
        //                Cuota = objCuotaIn.Monto,
        //                Promesa = objCuotaIn.Monto - ((DescInsc / 100) * objCuotaIn.Monto),
        //                EstatusId = 1,
        //                ReferenciaId = "",
        //                EsEmpresa = false
        //            });
        //            //Obtener Periodo
        //            int periodo =
        //                (from a in db.Periodo
        //                 where DateTime.Now >= a.FechaInicial && DateTime.Now <= a.FechaFinal
        //                 select a.Meses).FirstOrDefault();
        //            for (int i = 1; i <= periodo; i++)
        //            {
        //                db.Pago.Add(new Pago
        //                {
        //                    AlumnoId = AlumnoId,
        //                    Anio = objOferta.Anio,
        //                    PeriodoId = objOferta.PeriodoId,
        //                    SubperiodoId = i,
        //                    OfertaEducativaId = objOferta.OfertaEducativaId,
        //                    FechaGeneracion = DateTime.Now,
        //                    CuotaId = objCuotaBec.CuotaId,
        //                    Cuota = objCuotaBec.Monto,
        //                    Promesa = objCuotaBec.Monto - ((DescBeca / 100) * objCuotaBec.Monto),
        //                    EstatusId = 1,
        //                    ReferenciaId = "",
        //                    EsEmpresa = false
        //                });
        //            }
        //            db.SaveChanges();
        //            foreach (Pago objPago in db.Pago.Local)
        //            {
        //                objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
        //            }
        //            //Pago Descuento
        //            if (DescInsc > 0)
        //            {
        //                db.PagoDescuento.Add(new PagoDescuento
        //                {
        //                    DescuentoId = db.AlumnoDescuento.Local[1].DescuentoId,
        //                    PagoId = db.Pago.Local[2].PagoId,
        //                    Monto = (DescInsc / 100) * objCuotaIn.Monto

        //                });
        //            }
        //            //PAgo Examen
        //            if (DescExam > 0)
        //            {
        //                db.PagoDescuento.Add(new PagoDescuento
        //                {
        //                    DescuentoId = db.AlumnoDescuento.Local[2].DescuentoId,
        //                    PagoId = db.Pago.Local[0].PagoId,
        //                    Monto = (DescExam / 100) * objCuotaExam.Monto

        //                });
        //            }
        //            if (DescCred > 0)
        //            {
        //                //pago Credencial
        //                db.PagoDescuento.Add(new PagoDescuento
        //                {
        //                    DescuentoId = db.AlumnoDescuento.Local[3].DescuentoId,
        //                    PagoId = db.Pago.Local[1].PagoId,
        //                    Monto = (DescCred / 100) * objCuotaCred.Monto
        //                });
        //            }
        //            if (DescBeca > 0)
        //            {
        //                //Pagos Periodos
        //                for (int i = 1; i <= periodo; i++)
        //                {
        //                    db.PagoDescuento.Add(new PagoDescuento
        //                    {
        //                        DescuentoId = db.AlumnoDescuento.Local[0].DescuentoId,
        //                        PagoId = db.Pago.Local[i + 2].PagoId,
        //                        Monto = (DescBeca / 100) * objCuotaBec.Monto
        //                    });
        //                }
        //            }
        //            db.SaveChanges();
        //            if (DescCred > 0)
        //            {
        //                return AlumnoID = new string[] { db.AlumnoDescuento.Local[0].AlumnoDescuentoId.ToString(),
        //            db.AlumnoDescuento.Local[1].AlumnoDescuentoId.ToString(),db.AlumnoDescuento.Local[2].AlumnoDescuentoId.ToString(),
        //            db.AlumnoDescuento.Local[3].AlumnoDescuentoId.ToString()};
        //            }
        //            else
        //            {
        //                return AlumnoID = new string[] { db.AlumnoDescuento.Local[0].AlumnoDescuentoId.ToString(),
        //            db.AlumnoDescuento.Local[1].AlumnoDescuentoId.ToString(),db.AlumnoDescuento.Local[2].AlumnoDescuentoId.ToString()};
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.Write(ex.Message);
        //            return null;
        //        }

        //    }
        //}

        //public static string[] InsertarDescuento(int AlumnoId, decimal DescBeca, decimal DescInsc, string ComentarioInsc, string ComentarioBeca, int anio, int periodoid)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        string[] AlumnoID;
        //        DTOAlumnoInscrito objOferta = BLLAlumnoInscrito.ConsultarAlumnoInscrito(AlumnoId, anio, periodoid);

        //        DTODescuentos objDescuentoIn = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 802);

        //        DTODescuentos objDescuentoBec = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 800, "Beca Académica");


        //        DTOCuota objCuotaIn = BLLCuota.traerCuotaParametros(objOferta, objDescuentoIn);

        //        DTOCuota objCuotaBec = BLLCuota.traerCuotaParametros(objOferta, objDescuentoBec);


        //        try
        //        {
        //            //Beca 
        //            if (DescBeca > 0)
        //            {
        //                db.AlumnoDescuento.Add(new AlumnoDescuento
        //                {
        //                    AlumnoId = AlumnoId,
        //                    OfertaEducativaId = objOferta.OfertaEducativaId,
        //                    Anio = objOferta.Anio,
        //                    PeriodoId = objOferta.PeriodoId,
        //                    DescuentoId = objDescuentoBec.DescuentoId,
        //                    PagoConceptoId = objDescuentoBec.PagoConceptoId,
        //                    Monto = DescBeca,
        //                    UsuarioId = 7255,
        //                    Comentario = ComentarioBeca,
        //                    FechaGeneracion = DateTime.Now,
        //                    FechaAplicacion = DateTime.Now
        //                });
        //            }
        //            //Inscripcion
        //            if (DescInsc > 0)
        //            {
        //                db.AlumnoDescuento.Add(new AlumnoDescuento
        //                {
        //                    AlumnoId = AlumnoId,
        //                    OfertaEducativaId = objOferta.OfertaEducativaId,
        //                    Anio = objOferta.Anio,
        //                    PeriodoId = objOferta.PeriodoId,
        //                    DescuentoId = objDescuentoIn.DescuentoId,
        //                    PagoConceptoId = objDescuentoIn.PagoConceptoId,
        //                    Monto = DescInsc,
        //                    UsuarioId = 7255,
        //                    Comentario = ComentarioInsc,
        //                    FechaGeneracion = DateTime.Now,
        //                    FechaAplicacion = DateTime.Now
        //                });
        //            }

        //            db.SaveChanges();

        //            //Pagos Inscr
        //            db.Pago.Add(new Pago
        //            {
        //                AlumnoId = AlumnoId,
        //                Anio = objOferta.Anio,
        //                PeriodoId = objOferta.PeriodoId,
        //                SubperiodoId = 1,
        //                OfertaEducativaId = objOferta.OfertaEducativaId,
        //                FechaGeneracion = DateTime.Now,
        //                CuotaId = objCuotaIn.CuotaId,
        //                Cuota = objCuotaIn.Monto,
        //                Promesa = objCuotaIn.Monto - ((DescInsc / 100) * objCuotaIn.Monto),
        //                ReferenciaId = " ",
        //                EstatusId = 1,
        //                EsEmpresa = false
        //            });
        //            //Obtener Periodo
        //            int periodo =
        //                (from a in db.Periodo
        //                 where a.Anio == anio && a.PeriodoId == periodoid
        //                 select a.Meses).FirstOrDefault();
        //            for (int i = 1; i <= periodo; i++)
        //            {
        //                db.Pago.Add(new Pago
        //                {
        //                    AlumnoId = AlumnoId,
        //                    Anio = objOferta.Anio,
        //                    PeriodoId = objOferta.PeriodoId,
        //                    SubperiodoId = i,
        //                    OfertaEducativaId = objOferta.OfertaEducativaId,
        //                    FechaGeneracion = DateTime.Now,
        //                    CuotaId = objCuotaBec.CuotaId,
        //                    Cuota = objCuotaBec.Monto,
        //                    Promesa = objCuotaBec.Monto - ((DescBeca / 100) * objCuotaBec.Monto),
        //                    ReferenciaId = " ",
        //                    EstatusId = 1,
        //                    EsEmpresa = false
        //                });
        //            }
        //            db.SaveChanges();
        //            foreach (Pago objPago in db.Pago.Local)
        //            {
        //                objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
        //            }
        //            //Pago Descuento
        //            if (DescInsc > 0)
        //            {
        //                db.PagoDescuento.Add(new PagoDescuento
        //                {
        //                    DescuentoId = db.AlumnoDescuento.Local[1].DescuentoId,
        //                    PagoId = db.Pago.Local[0].PagoId,
        //                    Monto = (DescInsc / 100) * objCuotaIn.Monto

        //                });
        //            }
        //            if (DescBeca > 0)
        //            {
        //                //Pagos Periodos
        //                for (int i = 1; i <= periodo; i++)
        //                {
        //                    db.PagoDescuento.Add(new PagoDescuento
        //                    {
        //                        DescuentoId = db.AlumnoDescuento.Local[0].DescuentoId,
        //                        PagoId = db.Pago.Local[i].PagoId,
        //                        Monto = (DescBeca / 100) * objCuotaBec.Monto
        //                    });
        //                }
        //            }
        //            db.SaveChanges();
        //            return AlumnoID = new string[] { db.AlumnoDescuento.Local[0].AlumnoDescuentoId.ToString(),
        //            db.AlumnoDescuento.Local[1].AlumnoDescuentoId.ToString()};
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.Write(ex.Message);
        //            return null;
        //        }

        //    }
        //}

        public static string[] InsertarDescuento(int AlumnoId, int OfertaEducativaId, decimal MontoInscripcion, decimal MontoColegiatura, int Anio, int PeriodoId, Boolean Inscripcion, Boolean Descuentos, int UsuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTOAlumnoInscrito objOferta = BLLAlumnoInscrito.ConsultarAlumnoInscrito(AlumnoId, OfertaEducativaId);
                objOferta.Anio = Anio;
                objOferta.PeriodoId = PeriodoId;

                DTODescuentos objDescuentoIn = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 802, "Descuento en inscripción");

                DTODescuentos objDescuentoBec = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 800, "Descuento en colegiatura");


                DTOCuota objCuotaIn = BLLCuota.traerCuotaParametros(objOferta, objDescuentoIn);

                DTOCuota objCuotaBec = BLLCuota.traerCuotaParametros(objOferta, objDescuentoBec);

                decimal DescBeca = 100 - ((MontoColegiatura * 100) / objCuotaBec.Monto);
                decimal DescInsc = 100 - ((MontoInscripcion * 100) / objCuotaIn.Monto);
                DescBeca = Math.Round(DescBeca, 2);
                DescInsc = Math.Round(DescInsc, 2);

                List<Pago> LstPagos = db.Pago.Where(a => a.AlumnoId == AlumnoId
                                               && a.OfertaEducativaId == OfertaEducativaId
                                               && a.EstatusId != 2
                                               && a.Anio == objOferta.Anio
                                               && a.PeriodoId == objOferta.PeriodoId
                                               && (a.Cuota1.PagoConceptoId == 800
                                               || a.Cuota1.PagoConceptoId == 802)).ToList();
                List<AlumnoDescuento> lstDescuento = db.AlumnoDescuento.Where(k => k.AlumnoId == AlumnoId
                                                                              && k.OfertaEducativaId == OfertaEducativaId
                                                                              && k.EstatusId == 2
                                                                              && (k.PagoConceptoId == 800
                                                                              || k.PagoConceptoId == 802)
                                                                              && k.Anio == objOferta.Anio
                                                                              && k.PeriodoId == objOferta.PeriodoId).ToList();
                db.AlumnoDescuentoBitacora.Local.Clear();
                try
                {
                    if (Descuentos)
                    {
                        //Beca 
                        if (DescBeca > 0)
                        {
                            if (lstDescuento.Where(k => k.PagoConceptoId == objDescuentoBec.PagoConceptoId).ToList().Count == 0)
                            {
                                db.AlumnoDescuento.Add(new AlumnoDescuento
                                {
                                    AlumnoId = AlumnoId,
                                    OfertaEducativaId = objOferta.OfertaEducativaId,
                                    Anio = objOferta.Anio,
                                    PeriodoId = objOferta.PeriodoId,
                                    DescuentoId = objDescuentoBec.DescuentoId,
                                    PagoConceptoId = objDescuentoBec.PagoConceptoId,
                                    Monto = DescBeca,
                                    UsuarioId = UsuarioId,
                                    Comentario = "",
                                    FechaGeneracion = DateTime.Now,
                                    FechaAplicacion = DateTime.Now,
                                    HoraGeneracion = DateTime.Now.TimeOfDay,
                                    EstatusId = 2
                                });
                            }
                            else
                            {
                                AlumnoDescuento objDesc1 = lstDescuento.Where(k =>
                                       k.PagoConceptoId == objDescuentoBec.PagoConceptoId)
                                     .LastOrDefault();
                                db.AlumnoDescuentoBitacora.Add(new
                                AlumnoDescuentoBitacora
                                {
                                    AlumnoDescuentoId = objDesc1.AlumnoDescuentoId,
                                    AlumnoId = AlumnoId,
                                    Monto = objDesc1.Monto,
                                    Anio = Anio,
                                    Comentario = objDesc1.Comentario,
                                    DescuentoId = objDesc1.DescuentoId,
                                    EsComite = objDesc1.EsComite,
                                    EsDeportiva = objDesc1.EsDeportiva,
                                    EsSEP = objDesc1.EsSEP,
                                    EstatusId = objDesc1.EstatusId,
                                    FechaAplicacion = objDesc1.FechaAplicacion,
                                    FechaGeneracion = objDesc1.FechaGeneracion,
                                    HoraGeneracion = objDesc1.HoraGeneracion,
                                    OfertaEducativaId = OfertaEducativaId,
                                    PagoConceptoId = objDesc1.PagoConceptoId,
                                    UsuarioId = objDesc1.UsuarioId,
                                    PeriodoId = objDesc1.PeriodoId
                                });
                                objDesc1.Monto = DescBeca;
                                objDesc1.UsuarioId = UsuarioId;
                                objDesc1.FechaGeneracion = DateTime.Now;
                                objDesc1.HoraGeneracion = DateTime.Now.TimeOfDay;
                            }
                        }
                        //Inscripcion
                        if (DescInsc > 0)
                        {
                            if (lstDescuento.Where(k => k.PagoConceptoId == objDescuentoIn.PagoConceptoId).ToList().Count == 0)
                            {
                                db.AlumnoDescuento.Add(new AlumnoDescuento
                                {
                                    AlumnoId = AlumnoId,
                                    OfertaEducativaId = objOferta.OfertaEducativaId,
                                    Anio = objOferta.Anio,
                                    PeriodoId = objOferta.PeriodoId,
                                    DescuentoId = objDescuentoIn.DescuentoId,
                                    PagoConceptoId = objDescuentoIn.PagoConceptoId,
                                    Monto = DescInsc,
                                    UsuarioId = UsuarioId,
                                    Comentario = "",
                                    FechaGeneracion = DateTime.Now,
                                    FechaAplicacion = DateTime.Now,
                                    HoraGeneracion = DateTime.Now.TimeOfDay,
                                    EstatusId = 2
                                });
                            }
                            else
                            {
                                AlumnoDescuento objDesc1 = lstDescuento.Where(k =>
                                       k.PagoConceptoId == objDescuentoIn.PagoConceptoId)
                                     .LastOrDefault();
                                db.AlumnoDescuentoBitacora.Add(new
                                AlumnoDescuentoBitacora
                                {
                                    AlumnoDescuentoId = objDesc1.AlumnoDescuentoId,
                                    AlumnoId = AlumnoId,
                                    Monto = objDesc1.Monto,
                                    Anio = Anio,
                                    Comentario = objDesc1.Comentario,
                                    DescuentoId = objDesc1.DescuentoId,
                                    EsComite = objDesc1.EsComite,
                                    EsDeportiva = objDesc1.EsDeportiva,
                                    EsSEP = objDesc1.EsSEP,
                                    EstatusId = objDesc1.EstatusId,
                                    FechaAplicacion = objDesc1.FechaAplicacion,
                                    FechaGeneracion = objDesc1.FechaGeneracion,
                                    HoraGeneracion = objDesc1.HoraGeneracion,
                                    OfertaEducativaId = OfertaEducativaId,
                                    PagoConceptoId = objDesc1.PagoConceptoId,
                                    UsuarioId = objDesc1.UsuarioId,
                                    PeriodoId = objDesc1.PeriodoId
                                });
                                objDesc1.Monto = DescBeca;
                                objDesc1.UsuarioId = UsuarioId;
                                objDesc1.FechaGeneracion = DateTime.Now;
                                objDesc1.HoraGeneracion = DateTime.Now.TimeOfDay;
                            }
                        }

                        db.SaveChanges();
                    }
                    //Pagos Inscr
                    if (Inscripcion)
                    {
                        if (LstPagos.Where(k => k.SubperiodoId == 1
                                              && k.Cuota1.PagoConceptoId == 802)
                                       .ToList().Count == 0)
                        {
                            db.Pago.Add(new Pago
                            {
                                AlumnoId = AlumnoId,
                                Anio = objOferta.Anio,
                                PeriodoId = objOferta.PeriodoId,
                                SubperiodoId = 1,
                                OfertaEducativaId = objOferta.OfertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = objCuotaIn.CuotaId,
                                Cuota = objCuotaIn.Monto,
                                Promesa = MontoInscripcion,
                                Restante = MontoInscripcion,
                                ReferenciaId = " ",
                                EstatusId = DescInsc == 100 ? 4 : 1,
                                EsEmpresa = false,
                                UsuarioId = UsuarioId,
                                UsuarioTipoId = 1,
                                HoraGeneracion = DateTime.Now.TimeOfDay,
                            });
                        }
                    }
                    //Obtener Periodo
                    int periodo =
                        (from a in db.Periodo
                         where a.Anio == Anio && a.PeriodoId == PeriodoId
                         select a.Meses).FirstOrDefault();

                    for (int i = 1; i <= periodo; i++)
                    {

                        if (LstPagos.Where(k => k.SubperiodoId == i
                                            && k.Cuota1.PagoConceptoId == 800)
                                     .ToList().Count == 0)
                        {
                            db.Pago.Add(new Pago
                            {
                                AlumnoId = AlumnoId,
                                Anio = objOferta.Anio,
                                PeriodoId = objOferta.PeriodoId,
                                SubperiodoId = i,
                                OfertaEducativaId = objOferta.OfertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = objCuotaBec.CuotaId,
                                Cuota = objCuotaBec.Monto,
                                Promesa = MontoColegiatura,
                                Restante = MontoColegiatura,
                                ReferenciaId = " ",
                                EstatusId = DescBeca == 100 ? 4 : 1,
                                EsEmpresa = false,
                                UsuarioId = UsuarioId,
                                UsuarioTipoId = 1,
                                HoraGeneracion = DateTime.Now.TimeOfDay,
                            });
                        }
                    }
                    db.SaveChanges();
                    foreach (Pago objPago in db.Pago.Local)
                    {
                        objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                    }
                    //Pago Descuento
                    if (Inscripcion)
                    {
                        if (DescInsc > 0)
                        {
                            if (db.Pago.Local[0].PagoDescuento.Where(k=>    
                                                    k.DescuentoId== db.AlumnoDescuento.Local.Where(P => 
                                                        P.PagoConceptoId == 802).FirstOrDefault().DescuentoId).ToList().Count == 0)
                            {
                                db.PagoDescuento.Add(new PagoDescuento
                                {
                                    DescuentoId = db.AlumnoDescuento.Local.Where(P => P.PagoConceptoId == 802).FirstOrDefault().DescuentoId,
                                    PagoId = db.Pago.Local[0].PagoId,
                                    Monto = Math.Round((DescInsc / 100) * objCuotaIn.Monto)

                                });
                            }
                        }
                    }
                    if (DescBeca > 0)
                    {
                        //Pagos Periodos
                        for (int i = 1; i <= periodo; i++)
                        {
                            if (db.Pago.Local[i].PagoDescuento.Where(K=>
                                                        K.DescuentoId== db.AlumnoDescuento.Local.Where(P => 
                                                            P.PagoConceptoId == 800).FirstOrDefault().DescuentoId).ToList().Count == 0)
                            {
                                db.PagoDescuento.Add(new PagoDescuento
                                {
                                    DescuentoId = db.AlumnoDescuento.Local.Where(P => P.PagoConceptoId == 800).FirstOrDefault().DescuentoId,
                                    PagoId = db.Pago.Local[i].PagoId,
                                    Monto = Math.Round((DescBeca / 100) * objCuotaBec.Monto)
                                });
                            }
                        }
                    }
                    db.SaveChanges();
                    return  new string[] {"Guadado Correctamente"};
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    return null;
                }

            }
        }

        public static void GenerarReferencia(int Alumnoid)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<Pago> lstPago = db.Pago.Where(P => P.AlumnoId == Alumnoid).ToList();

                lstPago.ForEach(delegate (Pago objpago)
                {
                    if (objpago.ReferenciaId == "")
                    {
                        db.PagoDescuento.Add(new PagoDescuento
                        {
                            DescuentoId = 26,
                            PagoId = objpago.PagoId,
                            Monto = 3498
                        });
                    }
                    objpago.ReferenciaId = db.spGeneraReferencia(objpago.PagoId).FirstOrDefault();
                });

                db.SaveChanges();
            }
        }

        public static List<DTOCuota> TraerDescuentos(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {

                    DTOAlumnoInscrito objOferta = BLLAlumnoInscrito.ConsultarAlumnoInscrito(AlumnoId);
                    List<DTODescuentos> listDescuentos = new List<DTODescuentos> { BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 802),
                BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 800,"Beca Académica"),
                BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId,1),
                BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 1000)};

                    List<DTOCuota> lstCuotas = new List<DTOCuota>();
                    foreach (DTODescuentos objDescuento in listDescuentos)
                    {
                        lstCuotas.Add(BLLCuota.traerCuotaParametros(objOferta, objDescuento));
                        lstCuotas.Last().Descuento = objDescuento;
                    }
                    return lstCuotas;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static List<DTOCuota> TraerDescuentos(int OfertaEducativaId, string periodo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTOPeriodo periodos = BLLPeriodoPortal.ConsultarPeriodo(periodo);
                List<DTODescuentos> descuentos = new List<DTODescuentos> { BLLDescuentos.obtenerDescuentos(OfertaEducativaId, 802),
                BLLDescuentos.obtenerDescuentos(OfertaEducativaId, 800,"Beca Académica"),
                BLLDescuentos.obtenerDescuentos(OfertaEducativaId,1),
                BLLDescuentos.obtenerDescuentos(OfertaEducativaId, 1000)};

                List<DTOCuota> cuotas = new List<DTOCuota>();
                foreach (DTODescuentos descuento in descuentos)
                {
                    if (descuento != null)
                    {
                        cuotas.Add(BLLCuota.traerCuotaParametros(new DTOAlumnoInscrito { OfertaEducativaId = OfertaEducativaId, Anio = periodos.Anio, PeriodoId = periodos.PeriodoId }, descuento));
                        cuotas.Last().Descuento = descuento;
                    }
                }
                return cuotas;
            }
        }

        public static string InsertarDescuentosIngles(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTOAlumnoInscrito objOferta = BLLAlumnoInscrito.ConsultarAlumnoInscrito(AlumnoId, 32);

                //Obtener Periodo
                DTOPeriodo objPeriodo = BLLPeriodoPortal.ConsultarPeriodo(objOferta.Anio, objOferta.PeriodoId);

                List<DTOPeriodo> lstPeiodosAlumno = BLLMes.CalcularMesesDeFechas(objPeriodo.FechaInicial, objPeriodo.FechaFinal);

                DTODescuentos objDescuentoBec = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 807, "Beca Académica");
                //Descuento en Credencial
                DTODescuentos objDescuentoCred = BLLDescuentos.obtenerDescuentos(objOferta.OfertaEducativaId, 1000);

                DTOCuota objCuotaBec = BLLCuota.traerCuotaParametros(objOferta, objDescuentoBec);
                //Cuota Credencial
                DTOCuota objCuotaCred = BLLCuota.traerCuotaParametros(objOferta, objDescuentoCred);

                try
                {
                    //Credencial
                    db.Pago.Add(new Pago
                    {
                        AlumnoId = AlumnoId,
                        Anio = objOferta.Anio,
                        PeriodoId = objOferta.PeriodoId,
                        SubperiodoId = 1,
                        OfertaEducativaId = objOferta.OfertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = objCuotaCred.CuotaId,
                        Cuota = objCuotaCred.Monto,
                        Promesa = objCuotaCred.Monto,
                        EstatusId = 1,
                        ReferenciaId = "",
                        EsEmpresa = false,

                    });
                    for (int i = 1; i <= lstPeiodosAlumno.Count; i++)
                    {
                        db.Pago.Add(new Pago
                        {
                            AlumnoId = AlumnoId,
                            Anio = objOferta.Anio,
                            PeriodoId = objOferta.PeriodoId,
                            SubperiodoId = lstPeiodosAlumno[i - 1].SubPeriodoId,
                            OfertaEducativaId = objOferta.OfertaEducativaId,
                            FechaGeneracion = DateTime.Now,
                            CuotaId = objCuotaBec.CuotaId,
                            Cuota = objCuotaBec.Monto,
                            Promesa = objCuotaBec.Monto,
                            EstatusId = 1,
                            ReferenciaId = "",
                            EsEmpresa = false
                        });
                    }
                    db.SaveChanges();
                    foreach (Pago objPago in db.Pago.Local)
                    {
                        objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                    }
                    db.SaveChanges();
                    return "Guardado";
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    return null;
                }
            }
        }

        public static string InsertarDescuentosIdiomas(int AlumnoId, int Idioma, decimal MontoBeca, string ComBeca, decimal MontoCredencial,
            string ComCredencial, Boolean Material, int UsuarioId, int Anio, int PeriodoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            { 
                try
                {
                    DTOAlumnoInscrito objOferta = BLLAlumnoInscrito.ConsultarAlumnoInscrito(AlumnoId, Idioma, Anio, PeriodoId);

                    //Obtener Periodo
                    DTOPeriodo objPeriodo = new DTOPeriodo
                    {
                        Anio = Anio,
                        PeriodoId = PeriodoId,
                    };
                                       

                    DTOCuota objCuotaMaterial = BLLCuota.TraerCuotaPagoConcepto(808, objOferta);
                    Usuario objUser = db.Usuario.Where(u => u.UsuarioId == UsuarioId).FirstOrDefault();
                    try
                    {
                        if (Material == true)
                        {
                            db.Pago.Add(new Pago
                            {
                                AlumnoId = AlumnoId,
                                Anio = objOferta.Anio,
                                PeriodoId = objOferta.PeriodoId,
                                SubperiodoId = 1,
                                OfertaEducativaId = objOferta.OfertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = objCuotaMaterial.CuotaId,
                                Cuota = objCuotaMaterial.Monto,
                                Promesa = objCuotaMaterial.Monto,
                                Restante = objCuotaMaterial.Monto,
                                EstatusId = 1,
                                ReferenciaId = "",
                                EsEmpresa = false,
                                UsuarioId = objUser.UsuarioId,
                                UsuarioTipoId = objUser.UsuarioTipoId,
                                HoraGeneracion = DateTime.Now.TimeOfDay
                            });
                        }
                        db.SaveChanges();
                        foreach (Pago objPago in db.Pago.Local)
                        {
                            objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                        }
                        db.SaveChanges();
                        return "Guardado";
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
                catch (Exception a)
                {
                    return a.Message;
                }
            }
        }

        public static List<DTOCuota> TraerCuotaIdiomas(int Idioma, string periodo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTOPeriodo objP = BLLPeriodoPortal.ConsultarPeriodo(periodo);
                List<DTODescuentos> listDescuentos = new List<DTODescuentos> {
                BLLDescuentos.obtenerDescuentos(Idioma, 807,"Beca Académica"),
                BLLDescuentos.obtenerDescuentos(Idioma, 1000)};

                List<DTOCuota> lstCuotas = new List<DTOCuota>();
                foreach (DTODescuentos objDescuento in listDescuentos)
                {
                    if (objDescuento != null)
                    {
                        lstCuotas.Add(BLLCuota.traerCuotaParametros(new DTOAlumnoInscrito { OfertaEducativaId = Idioma, Anio = objP.Anio, PeriodoId = objP.PeriodoId }, objDescuento));
                        lstCuotas.Last().Descuento = objDescuento;
                    }
                }
                return lstCuotas;
            }
        }
        public static string InsertarDescuento(List<int> Alumnos, int GrupoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                AlumnoInscrito OBJal;
                DTOGrupo objGrupo = BLLGrupo.ObtenerGrupo(GrupoId);

                List<AlumnoInscrito> listAlumnos = new List<AlumnoInscrito>();
                DTODescuentos objDescuentoIn = BLLDescuentos.obtenerDescuentos(objGrupo.OfertaEducativaId, 802);
                DTOCuota objCuotaIn;
                DTODescuentos objDescuentoCred = BLLDescuentos.obtenerDescuentos(objGrupo.OfertaEducativaId, 1000);
                DTOCuota objCuotaCred;
                //DTODescuentos objDescuentoBec = BLLDescuentos.obtenerDescuentos(objGrupo.OfertaEducativaId, 800);
                //DTOCuota objCuotaBec;

                Alumnos.ForEach(delegate (int id)
                {
                    OBJal = new AlumnoInscrito();
                    OBJal = (from a in db.AlumnoInscrito
                             where a.AlumnoId == id && a.OfertaEducativaId == objGrupo.OfertaEducativaId
                             select a).FirstOrDefault();
                    listAlumnos.Add(OBJal);
                });
                DAL.Periodo periodo = db.Periodo.Where(P => objGrupo.FechaInicio >= P.FechaInicial && objGrupo.FechaInicio <= P.FechaFinal).FirstOrDefault();

                objCuotaIn = BLLCuota.traerCuotaParametros(new DTOAlumnoInscrito { OfertaEducativaId = objGrupo.OfertaEducativaId, Anio = periodo.Anio, PeriodoId = periodo.PeriodoId }, objDescuentoIn);
                objCuotaCred = BLLCuota.traerCuotaParametros(new DTOAlumnoInscrito { OfertaEducativaId = objGrupo.OfertaEducativaId, Anio = periodo.Anio, PeriodoId = periodo.PeriodoId }, objDescuentoCred);

                listAlumnos.ForEach(delegate (AlumnoInscrito Alumno)
                {
                    //objCuotaBec = BLLCuota.traerCuotaParametros(BLLAlumnoInscrito.ConsultarAlumnoInscrito(Alumno.AlumnoId), objDescuentoBec);

                    Alumno.PagoPlanId = 4;
                    //Descuento Inscripcion
                    //if (objGrupo.GrupoDetalle.PorcentajeInscripcion > 0)
                    //{
                    //    Alumno.Alumno.AlumnoDescuento.Add(new AlumnoDescuento
                    //    {
                    //        OfertaEducativaId = objGrupo.OfertaEducativaId,
                    //        Anio = objGrupo.GrupoDetalle.Cuota.Anio,
                    //        PeriodoId = objGrupo.GrupoDetalle.Cuota.PeriodoId,
                    //        DescuentoId = objDescuentoIn.DescuentoId,
                    //        PagoConceptoId = objDescuentoIn.PagoConceptoId,
                    //        Monto = objGrupo.GrupoDetalle.PorcentajeInscripcion ?? 0,
                    //        //UsuarioId = 7255,
                    //        UsuarioId = Alumno.UsuarioId,
                    //        Comentario = "",
                    //        EstatusId = 2,
                    //        HoraGeneracion = DateTime.Now.TimeOfDay,
                    //        EsSEP = false,
                    //        EsComite = false,
                    //        EsDeportiva = false
                    //    });
                    //}
                    //Inscripcion
                    db.Pago.Add(new Pago
                    {
                        AlumnoId = Alumno.AlumnoId,
                        Anio = objGrupo.GrupoDetalle.Cuota.Anio,
                        PeriodoId = objGrupo.GrupoDetalle.Cuota.PeriodoId,
                        SubperiodoId = 1,
                        OfertaEducativaId = objGrupo.OfertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = objCuotaIn.CuotaId,
                        Cuota = objCuotaIn.Monto,
                        //Promesa = objCuotaIn.Monto - (((objGrupo.GrupoDetalle.PorcentajeInscripcion ?? 0) / 100) * objCuotaIn.Monto),
                        //EstatusId = objGrupo.GrupoDetalle.PorcentajeInscripcion == 100 ? 4 : 1,
                        ReferenciaId = "",
                        EsEmpresa = false
                    });
                    //Credencial
                    db.Pago.Add(new Pago
                    {
                        AlumnoId = Alumno.AlumnoId,
                        Anio = objGrupo.GrupoDetalle.Cuota.Anio,
                        PeriodoId = objGrupo.GrupoDetalle.Cuota.PeriodoId,
                        SubperiodoId = 1,
                        OfertaEducativaId = objGrupo.OfertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = objCuotaCred.CuotaId,
                        Cuota = objCuotaCred.Monto,
                        Promesa = objCuotaCred.Monto,
                        EstatusId = 1,
                        ReferenciaId = "",
                        EsEmpresa = false
                    });
                    for (int i = 1; i <= 4; i++)
                    {
                        db.Pago.Add(new Pago
                        {
                            AlumnoId = Alumno.AlumnoId,
                            Anio = objGrupo.GrupoDetalle.Cuota.Anio,
                            PeriodoId = objGrupo.GrupoDetalle.Cuota.PeriodoId,
                            SubperiodoId = i,
                            OfertaEducativaId = objGrupo.OfertaEducativaId,
                            FechaGeneracion = DateTime.Now,
                            CuotaId = objGrupo.GrupoDetalle.Cuota.CuotaId,
                            Cuota = objGrupo.GrupoDetalle.Cuota.Monto,
                            Promesa = objGrupo.GrupoDetalle.Cuota.Monto,
                            EstatusId = 1,
                            ReferenciaId = "",
                            EsEmpresa = false
                        });
                    }
                });
                db.SaveChanges();
                foreach (Pago objPago in db.Pago.Local)
                {
                    objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                }

                listAlumnos.ForEach(delegate (AlumnoInscrito Alumno)
               {
                   //if (objGrupo.GrupoDetalle.PorcentajeInscripcion > 0)
                   //{
                       db.PagoDescuento.Add(new PagoDescuento
                       {
                           DescuentoId = db.AlumnoDescuento.Local.FirstOrDefault(DP => DP.AlumnoId == Alumno.AlumnoId).DescuentoId,
                           PagoId = db.Pago.Local.FirstOrDefault(P => P.AlumnoId == Alumno.AlumnoId).PagoId,
                           Monto = db.Pago.Local.FirstOrDefault(P => P.AlumnoId == Alumno.AlumnoId).Cuota - db.Pago.Local.FirstOrDefault(P => P.AlumnoId == Alumno.AlumnoId).Promesa
                       });
                   //}
                   //Alumno.Alumno.GrupoAlumno.FirstOrDefault().Grupo = (from g in db.Grupo
                   //                       where g.GrupoId == objGrupo.GrupoId
                   //                       select g).FirstOrDefault();
               });

                db.SaveChanges();
                return "";
            }
        }

        public static string InsertarDescuento2(List<int> Alumnos, int GrupoId, int UsuarioId)
        {
            int Anio = 0, PeriodoId = 0;
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {

                    AlumnoInscrito OBJal;
                    DTOGrupo objGrupo = BLLGrupo.ObtenerGrupo(GrupoId);
                    Usuario objUser = db.Usuario.Where(u => u.UsuarioId == UsuarioId).FirstOrDefault();
                    if (objGrupo.GrupoDetalle.EsCongelada)
                    {
                        #region Colegiatura Congelada
                        List<AlumnoInscrito> listAlumnos = new List<AlumnoInscrito>();

                        Alumnos.ForEach(delegate (int id)
                        {
                            OBJal = new AlumnoInscrito();
                            OBJal = (from a in db.AlumnoInscrito
                                     where a.AlumnoId == id && a.EsEmpresa == true
                                     select a).FirstOrDefault();
                            listAlumnos.Add(OBJal);
                        });

                        listAlumnos.ForEach(delegate (AlumnoInscrito Alumno)
                        {
                            objGrupo.GrupoDetalle.Cuota.Descuento = BLLDescuentos.obtenerDescuentos(Alumno.OfertaEducativaId, 800, "Descuento en colegiatura");
                            DTODescuentos objDescuentoIn = BLLDescuentos.obtenerDescuentos(Alumno.OfertaEducativaId, 802);
                            Cuota objCuotaColegiatura = db.Cuota.Where(C => C.Anio == Anio && C.PeriodoId == PeriodoId && C.OfertaEducativaId == Alumno.OfertaEducativaId && C.PagoConceptoId == 800).AsNoTracking().FirstOrDefault();
                            Cuota objCuotaIn;
                            DTODescuentos objDescuentoCred = BLLDescuentos.obtenerDescuentos(Alumno.OfertaEducativaId, 1000);
                            Cuota objCuotaCred;
                            Periodo periodo = db.Periodo.Where(P => P.Anio == Anio && P.PeriodoId == PeriodoId).FirstOrDefault();

                            objCuotaIn = db.Cuota.Where(C => C.Anio == Anio && C.PeriodoId == PeriodoId && C.OfertaEducativaId == Alumno.OfertaEducativaId && C.PagoConceptoId == 802).AsNoTracking().FirstOrDefault();
                            objCuotaCred = db.Cuota.Where(C => C.Anio == Anio && C.PeriodoId == PeriodoId && C.OfertaEducativaId == Alumno.OfertaEducativaId && C.PagoConceptoId == 1000).AsNoTracking().FirstOrDefault();


                            Alumno.PagoPlanId = 4;
                            //Descuento Inscripcion
                            //if (objGrupo.GrupoDetalle.PorcentajeInscripcion > 0)
                            //{
                                Alumno.Alumno.AlumnoDescuento.Add(new AlumnoDescuento
                                {
                                    OfertaEducativaId = objGrupo.OfertaEducativaId,
                                    Anio = Anio,
                                    PeriodoId = PeriodoId,
                                    DescuentoId = objDescuentoIn.DescuentoId,
                                    PagoConceptoId = objDescuentoIn.PagoConceptoId,
                                    //Monto = objGrupo.GrupoDetalle.PorcentajeInscripcion ?? 0,
                                    //UsuarioId = 7255,
                                    UsuarioId = Alumno.UsuarioId,
                                    Comentario = "",
                                    EstatusId = 2,
                                    FechaGeneracion = DateTime.Now,
                                    FechaAplicacion = DateTime.Now,
                                    HoraGeneracion = DateTime.Now.TimeOfDay,
                                    EsSEP = false,
                                    EsComite = false,
                                    EsDeportiva = false
                                });
                            //}
                            // Descuento en Colegiatura
                            //if (objGrupo.GrupoDetalle.PorcentajeColegiatura > 0)
                            //{
                                Alumno.Alumno.AlumnoDescuento.Add(
                                    new AlumnoDescuento
                                    {
                                        OfertaEducativaId = objGrupo.OfertaEducativaId,
                                        Anio = Anio,
                                        PeriodoId = PeriodoId,
                                        DescuentoId = objGrupo.GrupoDetalle.Cuota.Descuento.DescuentoId,
                                        PagoConceptoId = objGrupo.GrupoDetalle.Cuota.PagoConceptoId,
                                        //Monto = objGrupo.GrupoDetalle.PorcentajeColegiatura ?? 0,
                                        //UsuarioId = 7255,
                                        UsuarioId = Alumno.UsuarioId,
                                        Comentario = "",
                                        EstatusId = 2,
                                        FechaGeneracion = DateTime.Now,
                                        FechaAplicacion = DateTime.Now,
                                        HoraGeneracion = DateTime.Now.TimeOfDay,
                                        EsSEP = false,
                                        EsComite = false,
                                        EsDeportiva = false
                                    });
                            //}
                            //Inscripcion
                            db.Pago.Add(new Pago
                            {
                                AlumnoId = Alumno.AlumnoId,
                                Anio = Anio,
                                PeriodoId = PeriodoId,
                                SubperiodoId = 1,
                                OfertaEducativaId = objGrupo.OfertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = objCuotaIn.CuotaId,
                                Cuota = objCuotaIn.Monto,
                                //Promesa = Math.Round(objCuotaIn.Monto - (((objGrupo.GrupoDetalle.PorcentajeInscripcion ?? 0) / 100) * objCuotaIn.Monto)),
                                //Restante = Math.Round(objCuotaIn.Monto - (((objGrupo.GrupoDetalle.PorcentajeInscripcion ?? 0) / 100) * objCuotaIn.Monto)),
                                //EstatusId = objGrupo.GrupoDetalle.PorcentajeInscripcion == 100 ? 4 : 1,
                                ReferenciaId = "",
                                EsEmpresa = false,
                                UsuarioId = objUser.UsuarioId,
                                UsuarioTipoId = objUser.UsuarioTipoId,
                                HoraGeneracion = DateTime.Now.TimeOfDay
                            });
                            //Credencial
                            db.Pago.Add(new Pago
                            {
                                AlumnoId = Alumno.AlumnoId,
                                Anio = Anio,
                                PeriodoId = PeriodoId,
                                SubperiodoId = 1,
                                OfertaEducativaId = objGrupo.OfertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = objCuotaCred.CuotaId,
                                Cuota = objCuotaCred.Monto,
                                Promesa = objCuotaCred.Monto,
                                Restante = objCuotaCred.Monto,
                                EstatusId = 1,
                                ReferenciaId = "",
                                EsEmpresa = false,
                                UsuarioId = objUser.UsuarioId,
                                UsuarioTipoId = objUser.UsuarioTipoId,
                                HoraGeneracion = DateTime.Now.TimeOfDay
                            });
                            decimal Promesa = 0;// ((objGrupo.GrupoDetalle.PorcentajeColegiatura ?? 0) / 100);
                            Promesa = Promesa * objCuotaColegiatura.Monto;
                            for (int i = 1; i <= 4; i++)
                            {

                                db.Pago.Add(new Pago
                                {
                                    AlumnoId = Alumno.AlumnoId,
                                    Anio = Anio,
                                    PeriodoId = PeriodoId,
                                    SubperiodoId = i,
                                    OfertaEducativaId = objGrupo.OfertaEducativaId,
                                    FechaGeneracion = DateTime.Now,
                                    CuotaId = objCuotaColegiatura.CuotaId,
                                    Cuota = objCuotaColegiatura.Monto,
                                    Promesa = Math.Round(objCuotaColegiatura.Monto - Promesa),
                                    Restante = Math.Round(objCuotaColegiatura.Monto - Promesa),
                                    EstatusId = 1,
                                    ReferenciaId = "",
                                    EsEmpresa = false,
                                    UsuarioId = objUser.UsuarioId,
                                    UsuarioTipoId = objUser.UsuarioTipoId,
                                    HoraGeneracion = DateTime.Now.TimeOfDay
                                });
                            }
                        });
                        db.SaveChanges();
                        foreach (Pago objPago in db.Pago.Local)
                        {
                            objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                        }
                        
                        listAlumnos.ForEach(delegate (AlumnoInscrito Alumno)
                        {
                            Cuota objCuotaIn;
                            objCuotaIn = db.Cuota.Where(C => C.Anio == Anio && C.PeriodoId == PeriodoId && C.OfertaEducativaId == Alumno.OfertaEducativaId && C.PagoConceptoId == 802).AsNoTracking().FirstOrDefault();
                            Cuota objCuotaColegiatura = db.Cuota.Where(C => C.Anio == Anio && C.PeriodoId == PeriodoId && C.OfertaEducativaId == Alumno.OfertaEducativaId && C.PagoConceptoId == 800).AsNoTracking().FirstOrDefault();

                            //if (objGrupo.GrupoDetalle.PorcentajeInscripcion > 0)
                            //{
                            //    db.PagoDescuento.Add(new PagoDescuento
                            //    {
                            //        DescuentoId = db.AlumnoDescuento.Local.Where(P => P.PagoConceptoId == 802 && P.AlumnoId == Alumno.AlumnoId).FirstOrDefault().DescuentoId,
                            //        PagoId = db.Pago.Local.Where(P => P.Cuota1.PagoConceptoId == 802 && P.AlumnoId == Alumno.AlumnoId).FirstOrDefault().PagoId,
                            //        Monto = Math.Round(((objGrupo.GrupoDetalle.PorcentajeInscripcion ?? 0) / 100) * objCuotaIn.Monto)
                            //        // Monto = Math.Round(db.Pago.Local.Where(P => P.Cuota1.PagoConceptoId == 802 && P.AlumnoId == Alumno.AlumnoId).FirstOrDefault().Cuota -
                            //        //db.Pago.Local.Where(P => P.Cuota1.PagoConceptoId == 802 && P.AlumnoId == Alumno.AlumnoId).FirstOrDefault().Promesa)
                            //    });
                            //}
                            //if (objGrupo.GrupoDetalle.PorcentajeColegiatura > 0)
                            //{
                            //    decimal Promesa = (((objGrupo.GrupoDetalle.PorcentajeColegiatura ?? 0) / 100));
                            //    Promesa = Math.Round(Promesa * objCuotaColegiatura.Monto);//Aplicar Descuento
                            //    lstPagosG = db.Pago.Local.Where(P => P.Cuota1.PagoConceptoId == 800 && P.AlumnoId == Alumno.AlumnoId).ToList();
                            //    lstPagosG.ForEach(delegate (Pago objPago)
                            //    {
                            //        db.PagoDescuento.Add(new PagoDescuento
                            //        {
                            //            DescuentoId = db.AlumnoDescuento.Local.Where(P => P.PagoConceptoId == 800 && P.AlumnoId == Alumno.AlumnoId).FirstOrDefault().DescuentoId,
                            //            PagoId = objPago.PagoId,
                            //            Monto = Promesa
                            //            // Monto = Math.Round(db.Pago.Local.Where(P => P.Cuota1.PagoConceptoId == 800).FirstOrDefault().Cuota -
                            //            //db.Pago.Local.Where(P => P.Cuota1.PagoConceptoId == 800).FirstOrDefault().Promesa)
                            //        });
                            //    });
                            //}
                            //Alumno.Alumno.GrupoAlumno.FirstOrDefault().Grupo = (from g in db.Grupo
                            //                       where g.GrupoId == objGrupo.GrupoId
                            //                       select g).FirstOrDefault();
                        });

                        db.SaveChanges();
                        return "Guardar";
                        #endregion
                    }
                    else
                    {
                        #region Colegiatura No Congelada
                        objGrupo.GrupoDetalle.Cuota.Descuento = BLLDescuentos.obtenerDescuentos(objGrupo.OfertaEducativaId, 800, "Descuento en colegiatura");
                        List<AlumnoInscrito> listAlumnos = new List<AlumnoInscrito>();
                        DTODescuentos objDescuentoIn = BLLDescuentos.obtenerDescuentos(objGrupo.OfertaEducativaId, 802, "Descuento en inscripción");
                        DTOCuota objCuotaIn;
                        DTODescuentos objDescuentoCred = BLLDescuentos.obtenerDescuentos(objGrupo.OfertaEducativaId, 1000);
                        DTOCuota objCuotaCred;
                        //DTODescuentos objDescuentoBec = BLLDescuentos.obtenerDescuentos(objGrupo.OfertaEducativaId, 800);
                        //DTOCuota objCuotaBec;

                        Alumnos.ForEach(delegate (int id)
                        {
                            OBJal = new AlumnoInscrito();
                            OBJal = (from a in db.AlumnoInscrito
                                     where a.AlumnoId == id && a.OfertaEducativaId == objGrupo.OfertaEducativaId
                                     select a).FirstOrDefault();
                            listAlumnos.Add(OBJal);
                        });
                        DAL.Periodo periodo = db.Periodo.Where(P => objGrupo.FechaRegistro >= P.FechaInicial && objGrupo.FechaRegistro <= P.FechaFinal).FirstOrDefault();

                        objCuotaIn = BLLCuota.traerCuotaParametros(new DTOAlumnoInscrito { OfertaEducativaId = objGrupo.OfertaEducativaId, Anio = periodo.Anio, PeriodoId = periodo.PeriodoId }, objDescuentoIn);
                        objCuotaCred = BLLCuota.traerCuotaParametros(new DTOAlumnoInscrito { OfertaEducativaId = objGrupo.OfertaEducativaId, Anio = periodo.Anio, PeriodoId = periodo.PeriodoId }, objDescuentoCred);

                        listAlumnos.ForEach(delegate (AlumnoInscrito Alumno)
                        {

                            //objCuotaBec = BLLCuota.traerCuotaParametros(BLLAlumnoInscrito.ConsultarAlumnoInscrito(Alumno.AlumnoId), objDescuentoBec);

                            Alumno.PagoPlanId = 4;
                            //Descuento Inscripcion
                            //if (objGrupo.GrupoDetalle.PorcentajeInscripcion > 0)
                            //{
                                Alumno.Alumno.AlumnoDescuento.Add(new AlumnoDescuento
                                {
                                    OfertaEducativaId = objGrupo.OfertaEducativaId,
                                    Anio = objGrupo.GrupoDetalle.Cuota.Anio,
                                    PeriodoId = objGrupo.GrupoDetalle.Cuota.PeriodoId,
                                    DescuentoId = objDescuentoIn.DescuentoId,
                                    PagoConceptoId = objDescuentoIn.PagoConceptoId,
                                    //Monto = objGrupo.GrupoDetalle.PorcentajeInscripcion ?? 0,
                                    //UsuarioId = 7255,
                                    UsuarioId = Alumno.UsuarioId,
                                    Comentario = "",
                                    EstatusId = 2,
                                    FechaGeneracion = DateTime.Now,
                                    FechaAplicacion = DateTime.Now,
                                    HoraGeneracion = DateTime.Now.TimeOfDay,
                                    EsSEP = false,
                                    EsComite = false,
                                    EsDeportiva = false
                                });
                            //}
                            // Descuento en Colegiatura
                            //if (objGrupo.GrupoDetalle.PorcentajeColegiatura > 0)
                            //{
                                Alumno.Alumno.AlumnoDescuento.Add(
                                    new AlumnoDescuento
                                    {
                                        OfertaEducativaId = objGrupo.OfertaEducativaId,
                                        Anio = objGrupo.GrupoDetalle.Cuota.Anio,
                                        PeriodoId = objGrupo.GrupoDetalle.Cuota.PeriodoId,
                                        DescuentoId = objGrupo.GrupoDetalle.Cuota.Descuento.DescuentoId,
                                        PagoConceptoId = objGrupo.GrupoDetalle.Cuota.PagoConceptoId,
                                        //Monto = objGrupo.GrupoDetalle.PorcentajeColegiatura ?? 0,
                                        //UsuarioId = 7255,
                                        UsuarioId = Alumno.UsuarioId,
                                        Comentario = "",
                                        EstatusId = 2,
                                        FechaGeneracion = DateTime.Now,
                                        FechaAplicacion = DateTime.Now,
                                        HoraGeneracion = DateTime.Now.TimeOfDay,
                                        EsSEP = false,
                                        EsComite = false,
                                        EsDeportiva = false
                                    });
                            //}
                            //Inscripcion
                            db.Pago.Add(new Pago
                            {
                                AlumnoId = Alumno.AlumnoId,
                                Anio = objGrupo.GrupoDetalle.Cuota.Anio,
                                PeriodoId = objGrupo.GrupoDetalle.Cuota.PeriodoId,
                                SubperiodoId = 1,
                                OfertaEducativaId = objGrupo.OfertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = objCuotaIn.CuotaId,
                                Cuota = objCuotaIn.Monto,
                                //Promesa = Math.Round(objCuotaIn.Monto - (((objGrupo.GrupoDetalle.PorcentajeInscripcion ?? 0) / 100) * objCuotaIn.Monto)),
                                //Restante = Math.Round(objCuotaIn.Monto - (((objGrupo.GrupoDetalle.PorcentajeInscripcion ?? 0) / 100) * objCuotaIn.Monto)),
                                //EstatusId = objGrupo.GrupoDetalle.PorcentajeInscripcion == 100 ? 4 : 1,
                                ReferenciaId = "",
                                EsEmpresa = false,
                                UsuarioId = objUser.UsuarioId,
                                UsuarioTipoId = objUser.UsuarioTipoId,
                                HoraGeneracion = DateTime.Now.TimeOfDay
                            });
                            //Credencial
                            db.Pago.Add(new Pago
                            {
                                AlumnoId = Alumno.AlumnoId,
                                Anio = objGrupo.GrupoDetalle.Cuota.Anio,
                                PeriodoId = objGrupo.GrupoDetalle.Cuota.PeriodoId,
                                SubperiodoId = 1,
                                OfertaEducativaId = objGrupo.OfertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = objCuotaCred.CuotaId,
                                Cuota = objCuotaCred.Monto,
                                Promesa = objCuotaCred.Monto,
                                Restante = objCuotaCred.Monto,
                                EstatusId = 1,
                                ReferenciaId = "",
                                EsEmpresa = false,
                                UsuarioId = objUser.UsuarioId,
                                UsuarioTipoId = objUser.UsuarioTipoId,
                                HoraGeneracion = DateTime.Now.TimeOfDay
                            });
                            decimal Promesa = 0;// ((objGrupo.GrupoDetalle.PorcentajeColegiatura ?? 0) / 100);
                            Promesa = Promesa * objGrupo.GrupoDetalle.Cuota.Monto;
                            for (int i = 1; i <= 4; i++)
                            {

                                db.Pago.Add(new Pago
                                {
                                    AlumnoId = Alumno.AlumnoId,
                                    Anio = objGrupo.GrupoDetalle.Cuota.Anio,
                                    PeriodoId = objGrupo.GrupoDetalle.Cuota.PeriodoId,
                                    SubperiodoId = i,
                                    OfertaEducativaId = objGrupo.OfertaEducativaId,
                                    FechaGeneracion = DateTime.Now,
                                    CuotaId = objGrupo.GrupoDetalle.Cuota.CuotaId,
                                    Cuota = objGrupo.GrupoDetalle.Cuota.Monto,
                                    Promesa = Math.Round(objGrupo.GrupoDetalle.Cuota.Monto - Promesa),
                                    Restante = Math.Round(objGrupo.GrupoDetalle.Cuota.Monto - Promesa),
                                    EstatusId = 1,
                                    ReferenciaId = "",
                                    EsEmpresa = false,
                                    UsuarioId = objUser.UsuarioId,
                                    UsuarioTipoId = objUser.UsuarioTipoId,
                                    HoraGeneracion = DateTime.Now.TimeOfDay
                                });
                            }
                        });
                        db.SaveChanges();
                        foreach (Pago objPago in db.Pago.Local)
                        {
                            objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                            objPago.Cuota1 = db.Cuota.Where(C => C.CuotaId == objPago.CuotaId).FirstOrDefault();
                        }

                        List<Pago> lstPagosG;
                        listAlumnos.ForEach(delegate (AlumnoInscrito Alumno)
                        {
                            //if (objGrupo.GrupoDetalle.PorcentajeInscripcion > 0)
                            //{
                            //    db.PagoDescuento.Add(new PagoDescuento
                            //    {
                            //        DescuentoId = objDescuentoIn.DescuentoId,
                            //        PagoId = db.Pago.Local.Where(P => P.Cuota1.PagoConceptoId == 802 && P.AlumnoId == Alumno.AlumnoId).FirstOrDefault().PagoId,
                            //        Monto = Math.Round(((objGrupo.GrupoDetalle.PorcentajeInscripcion ?? 0) / 100) * objCuotaIn.Monto)
                            //        // Monto = Math.Round(db.Pago.Local.Where(P => P.Cuota1.PagoConceptoId == 802 && P.AlumnoId == Alumno.AlumnoId).FirstOrDefault().Cuota -
                            //        //db.Pago.Local.Where(P => P.Cuota1.PagoConceptoId == 802 && P.AlumnoId == Alumno.AlumnoId).FirstOrDefault().Promesa)
                            //    });
                            //}
                            //if (objGrupo.GrupoDetalle.PorcentajeColegiatura > 0)
                            //{
                            //    decimal Promesa = (((objGrupo.GrupoDetalle.PorcentajeColegiatura ?? 0) / 100));
                            //    Promesa = Math.Round(Promesa * objGrupo.GrupoDetalle.Cuota.Monto);//Aplicar Descuento
                            //    lstPagosG = db.Pago.Local.Where(P => P.Cuota1.PagoConceptoId == 800 && P.AlumnoId == Alumno.AlumnoId).ToList();
                            //    lstPagosG.ForEach(delegate (Pago objPago)
                            //    {
                            //        db.PagoDescuento.Add(new PagoDescuento
                            //        {
                            //            DescuentoId = objGrupo.GrupoDetalle.Cuota.Descuento.DescuentoId,
                            //            PagoId = objPago.PagoId,
                            //            Monto = Promesa
                            //            // Monto = Math.Round(db.Pago.Local.Where(P => P.Cuota1.PagoConceptoId == 800).FirstOrDefault().Cuota -
                            //            //db.Pago.Local.Where(P => P.Cuota1.PagoConceptoId == 800).FirstOrDefault().Promesa)
                            //        });
                            //    });
                            //}
                            //Alumno.Alumno.GrupoAlumno.FirstOrDefault().Grupo = (from g in db.Grupo
                            //                       where g.GrupoId == objGrupo.GrupoId
                            //                       select g).FirstOrDefault();
                        });

                        db.SaveChanges();
                        return "Guardar";
                        #endregion
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }

        public static DTOAlumnoDescuento TraerDescuentoAlumno(int AlumnoId, int OfertaEducativaId, int PagoConceptoId, int DescuentoId, int Anio, int PeriodoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return (from a in db.AlumnoDescuento
                            where a.AlumnoId == AlumnoId && a.OfertaEducativaId == OfertaEducativaId && a.PagoConceptoId == PagoConceptoId
                            && a.DescuentoId == DescuentoId && a.Anio == Anio && a.PeriodoId == PeriodoId && a.EstatusId == 1
                            select new DTOAlumnoDescuento
                            {
                                AlumnoDescuentoId = a.AlumnoDescuentoId,
                                AlumnoId = a.AlumnoId,
                                Anio = a.Anio,
                                ConceptoId = a.PagoConceptoId,
                                DescuentoId = a.DescuentoId,
                                EstatusId = a.EstatusId,
                                Monto = a.Monto,
                                OfertaEducativaId = a.OfertaEducativaId,
                                PeriodoId = a.PeriodoId
                            }).FirstOrDefault();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static DTOAlumnoDescuento TraerDescuentos(int AlumnoId, int OfertaEducativaid, int PagoConceptoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.AlumnoDescuento
                        where a.AlumnoId == AlumnoId && a.OfertaEducativaId == OfertaEducativaid && a.PagoConceptoId == PagoConceptoId
                        && a.EstatusId == 2
                        select new DTOAlumnoDescuento
                        {
                            AlumnoDescuentoId = a.AlumnoDescuentoId,
                            AlumnoId = a.AlumnoId,
                            Anio = a.Anio,
                            ConceptoId = a.PagoConceptoId,
                            DescuentoId = a.DescuentoId,
                            EstatusId = a.EstatusId,
                            Monto = a.Monto,
                            OfertaEducativaId = a.OfertaEducativaId,
                            PeriodoId = a.PeriodoId,
                            SMonto = a.Monto.ToString()
                        }).AsNoTracking().FirstOrDefault();
            }
        }

        public static string GenerarReferenciasPagos()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<Pago> lstPagos = db.Pago.Where(P => P.PagoId >= 8191 && P.PagoId <= 8197).ToList();
                foreach (Pago objPago in lstPagos)
                {
                    objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                }

                db.SaveChanges();

                return "Guardado";
            }
        }

        internal static DTOAlumnoDescuento ObtenerDescuentoAlumno(int AlumnoId, int DescuentoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.AlumnoDescuento
                        where a.AlumnoId == AlumnoId && a.DescuentoId == DescuentoId
                        select new DTOAlumnoDescuento
                        {
                            AlumnoDescuentoId = a.AlumnoDescuentoId,
                            AlumnoId = a.AlumnoId,
                            Anio = a.Anio,
                            ConceptoId = a.PagoConceptoId,
                            DescuentoId = a.DescuentoId,
                            EstatusId = a.EstatusId,
                            Monto = a.Monto,
                            OfertaEducativaId = a.OfertaEducativaId,
                            PeriodoId = a.PeriodoId
                        }).AsNoTracking().FirstOrDefault();
            }
        }

        public static List<DTOCuota> TraerCuotasOfertaEducativaPeriodo(int OfertaEducativaId, string Periodo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DTOPeriodo objPeriodo = BLLPeriodoPortal.ConsultarPeriodo(Periodo);
                    return db.Cuota.Where(C => C.OfertaEducativaId == OfertaEducativaId && C.Anio == objPeriodo.Anio && C.PeriodoId == objPeriodo.PeriodoId).Select(C => new DTOCuota
                    {
                        CuotaId = C.CuotaId,
                        PagoConceptoId = C.PagoConceptoId,
                        DTOPagoConcepto = new DTOPagoConcepto { Descripcion = C.PagoConcepto.Descripcion }
                    }).ToList();
                }
                catch
                {
                    return null;
                }
            }
        }

        //public static string GenerarCargos_Becas(int AlumnoId, int OfertaEducatovaId, decimal Monto, bool SEP, int Anio, int PeriodoId, int UsuarioId)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        string Descripcion = SEP == true ? "Beca SEP" : "Beca Académica";
        //        try
        //        {

        //            Descuento objDescuentoIns = new Descuento();
        //            Descuento objDescuentoColeg = new Descuento();
        //            List<Pago> lstPagosAlumno = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.OfertaEducativaId == OfertaEducatovaId && P.Anio == Anio && P.PeriodoId == PeriodoId
        //              && (P.Cuota1.PagoConceptoId == 800 || P.Cuota1.PagoConceptoId == 802) && (P.EstatusId == 1 || P.EstatusId == 4 || P.EstatusId == 13 || P.EstatusId == 14)).ToList();

        //            #region Pagos Restantes
        //            if (lstPagosAlumno.Count < 3)
        //            {
        //                Cuota objCuotaColegiatura = db.Cuota.Where(C => C.OfertaEducativaId == OfertaEducatovaId && C.PeriodoId == PeriodoId && C.Anio == Anio
        //                && C.PagoConceptoId == 800).FirstOrDefault();

        //                for (int o = 2; o <= 4; o++)
        //                {
        //                    db.Pago.Add(new Pago
        //                    {
        //                        ReferenciaId = "",
        //                        AlumnoId = AlumnoId,
        //                        Anio = Anio,
        //                        PeriodoId = PeriodoId,
        //                        SubperiodoId = o,
        //                        OfertaEducativaId = OfertaEducatovaId,
        //                        FechaGeneracion = DateTime.Now,
        //                        CuotaId = objCuotaColegiatura.CuotaId,
        //                        Cuota = objCuotaColegiatura.Monto,
        //                        Promesa = objCuotaColegiatura.Monto,
        //                        EstatusId = 1,
        //                        EsEmpresa = false,
        //                        EsAnticipado = false
        //                    });
        //                }
        //                db.SaveChanges();

        //                db.Pago.Local.ToList().ForEach(delegate(Pago objPago)
        //                {
        //                    objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
        //                });
        //                db.SaveChanges();

        //                lstPagosAlumno = db.Pago.Local.Where(P => P.AlumnoId == AlumnoId && P.OfertaEducativaId == OfertaEducatovaId && P.Anio == Anio && P.PeriodoId == PeriodoId
        //                                      && (P.Cuota1.PagoConceptoId == 800 || P.Cuota1.PagoConceptoId == 802) && (P.EstatusId == 1 || P.EstatusId == 4 || P.EstatusId == 13
        //                                      || P.EstatusId == 14)).ToList();
        //            }
        //            #endregion
        //            /* Sacamos los decuentos*/
        //            if (SEP == true)
        //            { objDescuentoIns = db.Descuento.Where(D => D.PagoConceptoId == 802 && D.OfertaEducativaId == OfertaEducatovaId && D.Descripcion == Descripcion).FirstOrDefault(); }
        //            objDescuentoColeg = db.Descuento.Where(D => D.PagoConceptoId == 800 && D.OfertaEducativaId == OfertaEducatovaId && D.Descripcion == Descripcion).FirstOrDefault();

        //            #region Inscripcion
        //            if (SEP == true)
        //            {
        //                db.AlumnoDescuento.Add(new AlumnoDescuento
        //                {
        //                    AlumnoId = AlumnoId,
        //                    OfertaEducativaId = OfertaEducatovaId,
        //                    Anio = Anio,
        //                    PeriodoId = PeriodoId,
        //                    DescuentoId = objDescuentoIns.DescuentoId,
        //                    PagoConceptoId = 802,
        //                    Monto = Monto,
        //                    UsuarioId = UsuarioId,
        //                    Comentario = "",
        //                    FechaGeneracion = DateTime.Now,
        //                    FechaAplicacion = DateTime.Now,
        //                    EstatusId = 2
        //                });

        //            }
        //            #endregion
        //            #region Colegiatura
        //            db.AlumnoDescuento.Add(new AlumnoDescuento
        //            {
        //                AlumnoId = AlumnoId,
        //                OfertaEducativaId = OfertaEducatovaId,
        //                Anio = Anio,
        //                PeriodoId = PeriodoId,
        //                DescuentoId = objDescuentoColeg.DescuentoId,
        //                PagoConceptoId = 800,
        //                Monto = Monto,
        //                UsuarioId = UsuarioId,
        //                Comentario = "",
        //                FechaGeneracion = DateTime.Now,
        //                FechaAplicacion = DateTime.Now,
        //                EstatusId = 2
        //            });
        //            #endregion
        //            #region Modificacion de Pagos

        //            decimal Saldo = 0;
        //            lstPagosAlumno.ForEach(delegate(Pago objPago)
        //            {
        //                decimal PromesaSM = objPago.Promesa;
        //                objPago.Promesa = Math.Round((objPago.Cuota1.PagoConceptoId == 800 ? (objPago.Promesa - (objPago.Promesa * (Monto / 100))) :
        //                SEP == true ? (objPago.Promesa - (objPago.Promesa * (Monto / 100))) : objPago.Promesa));

        //                if (SEP == true && objPago.Cuota1.PagoConceptoId == 802)
        //                {
        //                    db.PagoDescuento.Add(
        //                        new PagoDescuento
        //                        {
        //                            DescuentoId = objDescuentoIns.DescuentoId,
        //                            Monto = Math.Round((PromesaSM * (Monto / 100))),
        //                            PagoId = objPago.PagoId
        //                        });
        //                    if (objPago.EstatusId == 4 || objPago.EstatusId == 14)
        //                    {
        //                        Saldo += Math.Round((PromesaSM * (Monto / 100)));
        //                    }
        //                }
        //                else if (objPago.Cuota1.PagoConceptoId == 800)
        //                {
        //                    db.PagoDescuento.Add(
        //                       new PagoDescuento
        //                       {
        //                           DescuentoId = objDescuentoColeg.DescuentoId,
        //                           Monto = Math.Round((PromesaSM * (Monto / 100))),
        //                           PagoId = objPago.PagoId
        //                       });
        //                    if (objPago.EstatusId == 4 || objPago.EstatusId == 14)
        //                    {
        //                        Saldo += Math.Round((PromesaSM * (Monto / 100)));
        //                    }
        //                }

        //            });
        //            ///Si hay pagos que fueron Pagados
        //            if (Saldo > 0)
        //            {
        //                if (db.AlumnoSaldo.Where(A => A.AlumnoId == AlumnoId).ToList().Count > 1)
        //                {
        //                    AlumnoSaldo objASaldo = db.AlumnoSaldo.Where(A => A.AlumnoId == AlumnoId).FirstOrDefault();
        //                    objASaldo.Saldo += Saldo;
        //                }
        //                else
        //                {
        //                    db.AlumnoSaldo.Add(new AlumnoSaldo
        //                    {
        //                        AlumnoId = AlumnoId,
        //                        Saldo = Saldo,
        //                    });
        //                }
        //            }
        //            #endregion

        //            #region Insertar en AlumnoInscritoBeca
        //            db.AlumnoInscritoBeca.Add(new AlumnoInscritoBeca
        //            {
        //                AlumnoId = AlumnoId,
        //                Anio = Anio,
        //                FechaAplicacion = DateTime.Now,
        //                OfertaEducativaId = OfertaEducatovaId,
        //                PeriodoId = PeriodoId,
        //                UsuarioId = UsuarioId
        //            });
        //            #endregion
        //            db.SaveChanges();
        //            return "Guardado";

        //        }
        //        catch (Exception d)
        //        {
        //            return d.Message;
        //        }
        //    }
        //}
    }
}
