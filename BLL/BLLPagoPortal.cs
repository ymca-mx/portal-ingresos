using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Utilities;
using System.Data.Entity;

namespace BLL
{
    public class BLLPagoPortal
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");
        public static void ActualizarPagos(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DateTime fIni;
                    DateTime fFin;
                    List<Pago> lstPago = db.Pago.Where(P => P.AlumnoId == AlumnoId &&
                        (P.EstatusId == 1 || P.EstatusId == 13) &&
                        (P.Cuota1.PagoConceptoId == 800 || P.Cuota1.PagoConceptoId == 802) &&
                        P.OfertaEducativa.OfertaEducativaTipoId != 4 && P.EsAnticipado == false &&
                        P.SubperiodoId == 1).ToList();


                    lstPago.ForEach(delegate (Pago objPago)
                    {
                        fIni = objPago.Periodo.FechaInicial;
                        fIni = fIni.AddMonths(-1);
                        fFin = fIni.AddDays(15);

                        if (objPago.FechaGeneracion >= fIni && objPago.FechaGeneracion <= fFin)
                        {
                            BLLPagoPortal.AplicarDescuento(objPago.PagoId);
                        }
                        else if (objPago.FechaGeneracion <= fIni)
                        {
                            BLLPagoPortal.AplicarDescuento(objPago.PagoId);
                        }
                    });

                }
                catch
                {

                }
            }
        }

        public static string ActivarPago(int PagoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    Pago pago = db.Pago.Where(p => p.PagoId == PagoId).FirstOrDefault();

                    pago.EstatusId = pago.Promesa == 0 ? 4 : 1;
                    pago.Restante = pago.Promesa;

                    if ((pago?.PagoCancelacion?.PagoCancelacionDetalle?.FirstOrDefault() ?? null) != null)
                    {
                        db.PagoCancelacionDetalle.Remove(pago?.PagoCancelacion?.PagoCancelacionDetalle?.FirstOrDefault());
                    }
                    if ((pago?.PagoCancelacion ?? null) != null)
                    {
                        db.PagoCancelacion.Remove(pago?.PagoCancelacion);
                    }
                    db.SaveChanges();
                    return "Guardado";
                }
                catch { return "Fallo"; }
            }
        }

        public static string GenerarReferenciaId(int PagoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    Pago objP = db.Pago.Where(a => a.PagoId == PagoId).FirstOrDefault();
                    objP.ReferenciaId = db.spGeneraReferencia(PagoId).FirstOrDefault();
                    db.SaveChanges();
                    return "Generado";
                }
                catch (Exception a1)
                {
                    return a1.Message;
                }
            }
        }



        public static List<DTOPagos> ConsultarReferencias(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    //string Fechaq=(DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString())
                    //    + "/" +( DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + "/" + DateTime.Now.Year.ToString();
                    //DateTime FechaActual = DateTime.ParseExact(Fechaq, "dd/MM/yyyy", Cultura);
                    //int idPago = 2588;
                    string Anticipado;
                    string FechaS;
                    DTOCuota CuotaAnterior;
                    AlumnoInscrito objAlumno = db.AlumnoInscrito.Where(A => A.AlumnoId == AlumnoId).FirstOrDefault();
                    List<DTOAlumnoReferenciaBitacora> lstPagosGenerados = BLLAdeudoBitacora.TraerListaBitacora(AlumnoId);

                    List<DTOPagos> lstPagos = (from p in db.Pago
                                               where p.AlumnoId == AlumnoId && p.EstatusId != 2 && p.Cuota1.PagoConceptoId != 1007
                                               orderby p.PagoId ascending
                                               select new DTOPagos
                                               {
                                                   PagoRecargo = db.PagoRecargo.Select(a => new DTOPagoRecargo
                                                   {
                                                       PagoId = a.PagoId,
                                                       PagoIdRecargo = a.PagoIdRecargo
                                                   }).Where(a => a.PagoIdRecargo == p.PagoId).FirstOrDefault(),
                                                   PagoId = p.PagoId,
                                                   AlumnoId = p.AlumnoId,
                                                   Anio = p.Anio,
                                                   PeriodoId = p.PeriodoId,
                                                   PeriodoAnticipadoId = p.PeriodoAnticipadoId,
                                                   DTOParcial = (from PP in db.PagoParcial
                                                                 where
                                                                     PP.PagoId == p.PagoId
                                                                 select new DTOPagoParcial
                                                                 {
                                                                     PagoId = PP.PagoId,
                                                                     SucursalCajaId = (int)PP.SucursalCajaId,
                                                                     Pago = PP.Pago,
                                                                     FechaPago = PP.FechaPago,

                                                                 }).ToList(),
                                                   DTOPeriodo = new DTOPeriodo
                                                   {
                                                       PeriodoId = p.Periodo.PeriodoId,
                                                       Anio = p.Periodo.Anio,
                                                       Descripcion = p.Periodo.Descripcion,
                                                       FechaInicial = p.Periodo.FechaInicial,
                                                       FechaFinal = p.Periodo.FechaFinal,
                                                       Meses = p.Periodo.Meses
                                                   },
                                                   SubperiodoId = p.SubperiodoId,
                                                   DTOSubPeriodo = new DTOSubPeriodo
                                                   {
                                                       SubperiodoId = p.Subperiodo.SubperiodoId,
                                                       PeriodoId = p.Subperiodo.PeriodoId,
                                                       MesId = p.Subperiodo.MesId,
                                                       Mes = (from c in db.Mes
                                                              where c.MesId == p.Subperiodo.MesId
                                                              select new DTOMes
                                                              {
                                                                  Descripcion = c.Descripcion,
                                                                  MesId = c.MesId,
                                                              }).FirstOrDefault()
                                                   },
                                                   OfertaEducativaId = p.OfertaEducativaId,
                                                   OfertaEducativa = new DTOOfertaEducativa
                                                   {
                                                       OfertaEducativaId = p.OfertaEducativa.OfertaEducativaId,
                                                       OfertaEducativaTipoId = p.OfertaEducativa.OfertaEducativaTipoId,
                                                       Descripcion = p.OfertaEducativa.Descripcion,
                                                       DTOOfertaEducativaTipo = new DTOOfertaEducativaTipo
                                                       {
                                                           Descripcion = p.OfertaEducativa.OfertaEducativaTipo.Descripcion,
                                                           OfertaEducativaTipoId = p.OfertaEducativa.OfertaEducativaTipo.OfertaEducativaTipoId
                                                       }
                                                   },
                                                   FechaGeneracion = p.FechaGeneracion,
                                                   CuotaId = p.CuotaId,
                                                   Cuota = p.Cuota,
                                                   DTOCuota = new DTOCuota
                                                   {
                                                       CuotaId = p.Cuota1.CuotaId,
                                                       Anio = p.Cuota1.Anio,
                                                       PeriodoId = p.Cuota1.PeriodoId,
                                                       OfertaEducativaId = p.Cuota1.OfertaEducativaId,
                                                       PagoConceptoId = p.Cuota1.PagoConceptoId,
                                                       Monto = p.Cuota1.Monto,
                                                       //EsEmpresa = p.Cuota1.EsEmpresa,
                                                       DTOPagoConcepto = new DTOPagoConcepto
                                                       {
                                                           //CuentaContable = p.Cuota1.PagoConcepto.CuentaContable,
                                                           Descripcion = p.Cuota1.PagoConcepto.Descripcion,
                                                           EstatusId = p.Cuota1.PagoConcepto.EstatusId,
                                                           //OfertaEducativaId = p.Cuota1.PagoConcepto.OfertaEducativaId,
                                                           PagoConceptoId = p.Cuota1.PagoConcepto.PagoConceptoId
                                                       }
                                                   },
                                                   Promesa = p.Promesa,
                                                   Referencia = p.ReferenciaId,
                                                   EstatusId = p.EstatusId,
                                                   lstPagoDescuento = (from pd in db.PagoDescuento
                                                                       join ad in db.AlumnoDescuento on pd.DescuentoId equals ad.DescuentoId
                                                                       where pd.PagoId == p.PagoId && ad.AlumnoId == AlumnoId && p.Anio == ad.Anio && p.PeriodoId == ad.PeriodoId && ad.EstatusId != 3
                                                                       select new DTOPagoDescuento
                                                                       {
                                                                           DescuentoId = pd.DescuentoId,
                                                                           Monto = pd.Monto,
                                                                           PagoId = pd.PagoId,
                                                                           DTOAlumnDes = new DTOAlumnoDescuento
                                                                           {
                                                                               AlumnoId = ad.AlumnoId,
                                                                               Anio = ad.Anio,
                                                                               ConceptoId = ad.PagoConceptoId,
                                                                               DescuentoId = ad.DescuentoId,
                                                                               EstatusId = ad.EstatusId,
                                                                               Monto = objAlumno.EsEmpresa == true ? 0 : ad.Monto,
                                                                               SMonto = objAlumno.EsEmpresa == true ? "0%" : ad.Monto.ToString() + "%",
                                                                               OfertaEducativaId = ad.OfertaEducativaId,
                                                                               PeriodoId = ad.PeriodoId
                                                                           }
                                                                       }).ToList()
                                                   //DTOReferenciaBitacora = new DTOAlumnoReferenciaBitacora
                                                   //{
                                                   //    AlumnoId=p.AlumnoReferenciaBitacora.
                                                   //}
                                               }).ToList();

                    lstPagos.ForEach(delegate (DTOPagos objP)
                    {
                        if (objP.Anio == 2016 && objP.PeriodoId == 1)
                        {
                            objP.DTOCuota.PeridoAnio = "";
                            CuotaAnterior = null;
                        }
                        else
                        {
                            objP.DTOCuota.PeridoAnio = objP.DTOCuota.Anio.ToString() + "-" + objP.DTOCuota.PeriodoId.ToString();

                            int Anio, Periodo, Oferta, Concepto;

                            Anio = (objP.DTOPeriodo.PeriodoId == 4 ? objP.DTOPeriodo.Anio - 1 : objP.DTOPeriodo.Anio);

                            Periodo = (objP.DTOPeriodo.PeriodoId == 1 ? 4 : objP.DTOPeriodo.PeriodoId - 1);

                            Oferta = objP.OfertaEducativaId;

                            Concepto = objP.DTOCuota.PagoConceptoId;

                            CuotaAnterior = (from a in db.Cuota
                                             where a.Anio == Anio && a.PeriodoId == Periodo
                                             && a.OfertaEducativaId == Oferta && a.PagoConceptoId == Concepto
                                             select new DTOCuota
                                             {
                                                 CuotaId = a.CuotaId,
                                                 Anio = a.Anio,
                                                 PeriodoId = a.PeriodoId,
                                                 OfertaEducativaId = a.OfertaEducativaId,
                                                 PagoConceptoId = a.PagoConceptoId,
                                                 Monto = a.Monto,
                                                 EsEmpresa = a.EsEmpresa
                                             }).FirstOrDefault();
                        }
                        if (objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800)
                        {

                            objP.DTOCuota.DTOPagoConcepto.Descripcion += ", " + objP.DTOSubPeriodo.Mes.Descripcion + " " + objP.DTOPeriodo.FechaInicial.Year.ToString();
                        }

                        if (objP.PagoRecargo != null)
                        {
                            objP.DTOCuota.DTOPagoConcepto.Descripcion += ", " + lstPagos.Where(LP => LP.PagoId == objP.PagoRecargo.PagoId).FirstOrDefault().DTOCuota.DTOPagoConcepto.Descripcion;
                        }

                        if (objP.lstPagoDescuento.Count == 0)
                        {
                            objP.lstPagoDescuento = new List<DTOPagoDescuento>{
                        new DTOPagoDescuento{
                            DTOAlumnDes= new DTOAlumnoDescuento{
                                Monto=0,
                                SMonto=objP.Anio==2016 && objP.PeriodoId==1? "":"0%"
                            }
                        }
                    };
                        }
                        decimal PTotal = objP.DTOParcial.Select(P => P.Pago).Sum();
                        decimal RTotal = db.ReferenciadoCabecero.Where(RC => RC.PagoId == objP.PagoId).ToList().Select(RC => RC.ImporteTotal).Sum();

                        objP.Restante = objP.DTOParcial.Count > 0 || RTotal > 0 ?
                            (objP.Promesa - (PTotal + RTotal)).ToString("C", Cultura)
                            : objP.Promesa.ToString("C", Cultura);

                        Anticipado = (objP.DTOPeriodo.FechaInicial.Day.ToString().Length < 2 ? "0" + objP.DTOPeriodo.FechaInicial.Day.ToString() : objP.DTOPeriodo.FechaInicial.Day.ToString())
                            + "/" + (objP.DTOSubPeriodo.MesId.ToString().Length < 2 ? "0" + objP.DTOSubPeriodo.MesId.ToString() : objP.DTOSubPeriodo.MesId.ToString())
                            + "/" + (objP.DTOPeriodo.FechaInicial.Year.ToString().Length < 2 ? "0" + objP.DTOPeriodo.FechaInicial.Year.ToString() : objP.DTOPeriodo.FechaInicial.Year.ToString());
                        FechaS = DateTime.ParseExact(Anticipado, "dd/MM/yyyy", Cultura).AddDays(-1).ToString("dd/MM/yyyy", Cultura).ToString();
                        //FechaS = "15" + FechaS.Substring(2, 8);
                        //objP.objNormal = new Pagos_Detalles
                        //{
                        //    FechaLimite = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 && lstPagosGenerados.Find(A => A.PagoId == objP.PagoId) != null ?
                        //    objP.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura) :
                        //    Fecha.Prorroga(objP.DTOPeriodo.FechaFinal.Year, int.Parse(Anticipado.Substring(3, 2)), true, 5).ToString("dd/MM/yyyy", Cultura) : "",
                        //    //FechaLimite = "",
                        //    Monto = objP.PagoId > idPago ? objP.Promesa.ToString("C", Cultura) : "",
                        //    Recargo = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                        //    (objP.Promesa * decimal.Parse("0.05")).ToString("C", Cultura) : "",
                        //    Total = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                        //    (objP.Promesa + (objP.Promesa * decimal.Parse("0.05"))).ToString("C", Cultura) : ""
                        //};
                        if (objP.PeriodoAnticipadoId == 0)
                        {

                            objP.objNormal = new Pagos_Detalles
                            {
                                FechaLimite = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 && lstPagosGenerados.Find(A => A.PagoId == objP.PagoId) != null ?
                             objP.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura) :
                             Fecha.Prorroga(objP.DTOPeriodo.FechaFinal.Year, int.Parse(Anticipado.Substring(3, 2)), true, 5).ToString("dd/MM/yyyy", Cultura) : "",
                                //FechaLimite = "",
                                Monto = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.Promesa.ToString("C", Cultura) : "",
                                Recargo = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                             (objP.Promesa * decimal.Parse("0.05")).ToString("C", Cultura) : "",
                                Total = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                             (objP.Promesa + (objP.Promesa * decimal.Parse("0.05"))).ToString("C", Cultura) : ""
                            };
                            objP.objAnticipado2 = new Pagos_Detalles
                            {
                                Monto = "",
                                FechaLimite = ""
                            };
                            objP.objAnticipado1 = new Pagos_Detalles
                            {
                                Monto = "",
                                FechaLimite = ""
                            };
                        }
                        else if (objP.PeriodoAnticipadoId == 1)
                        {
                            objP.objAnticipado1 = new Pagos_Detalles
                            {
                                FechaLimite = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 && lstPagosGenerados.Find(A => A.PagoId == objP.PagoId) != null ?
                             objP.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura) :
                             Fecha.Prorroga(objP.DTOPeriodo.FechaFinal.Year, int.Parse(Anticipado.Substring(3, 2)), true, 5).ToString("dd/MM/yyyy", Cultura) : "",
                                //FechaLimite = "",
                                Monto = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.Promesa.ToString("C", Cultura) : "0"
                                //Recargo = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                                //(objP.Promesa * decimal.Parse("0.05")).ToString("C", Cultura) : "",
                                //Total = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                                //(objP.Promesa + (objP.Promesa * decimal.Parse("0.05"))).ToString("C", Cultura) : ""
                            };
                            objP.objAnticipado2 = new Pagos_Detalles
                            {
                                Monto = "",
                                FechaLimite = ""
                            };
                            objP.objNormal = new Pagos_Detalles
                            {
                                Monto = "",
                                FechaLimite = ""
                            };
                        }
                        else if (objP.PeriodoAnticipadoId == 2)
                        {
                            objP.objAnticipado2 = new Pagos_Detalles
                            {
                                FechaLimite = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 &&
                                objP.DTOCuota.PagoConceptoId != 807 && lstPagosGenerados.Find(A => A.PagoId == objP.PagoId) != null ?
                                objP.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura) :
                                Fecha.Prorroga(objP.DTOPeriodo.FechaFinal.Year, int.Parse(Anticipado.Substring(3, 2)), true, 5).ToString("dd/MM/yyyy", Cultura) : "",
                                //FechaLimite = "",
                                Monto = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.Promesa.ToString("C", Cultura) : "0"
                                //    Recargo = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                                //    (objP.Promesa * decimal.Parse("0.05")).ToString("C", Cultura) : "",
                                //    Total = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                                //    (objP.Promesa + (objP.Promesa * decimal.Parse("0.05"))).ToString("C", Cultura) : ""
                            };
                            objP.objAnticipado1 = new Pagos_Detalles
                            {
                                Monto = "",
                                FechaLimite = ""
                            };
                            objP.objNormal = new Pagos_Detalles
                            {
                                Monto = "",
                                FechaLimite = ""
                            };
                        }
                        //objP.objAnticipado2 = new Pagos_Detalles
                        //{
                        //    Monto = "",
                        //    //Monto = FechaActual > DateTime.ParseExact(Anticipado, "dd/MM/yyyy", Cultura) ? "" : objP.OfertaEducativa.DTOOfertaEducativaTipo.OfertaEducativaTipoId != 4 ?
                        //    //objP.PagoId > idPago ? objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800 ? (CuotaAnterior.Monto.ToString("C", Cultura)) :
                        //    //objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 802 ? (CuotaAnterior.Monto.ToString("C", Cultura)) : "" : "" : "",
                        //    FechaLimite = ""
                        //    // FechaLimite = FechaActual > DateTime.ParseExact(Anticipado, "dd/MM/yyyy", Cultura) ? "" : objP.OfertaEducativa.DTOOfertaEducativaTipo.OfertaEducativaTipoId != 4 ?
                        //    // objP.PagoId > idPago ? objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800 ?
                        //    //DateTime.ParseExact(Anticipado, "dd/MM/yyyy", Cultura).AddDays(-1).ToString("dd/MM/yyyy", Cultura) :
                        //    //objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 802 ?
                        //    //DateTime.ParseExact(Anticipado, "dd/MM/yyyy", Cultura).AddDays(-1).ToString("dd/MM/yyyy", Cultura) :
                        //    //"" : "" : ""
                        //    // FechaLimite = (objP.DTOPeriodo.FechaInicial.AddDays(-1)).ToShortDateString()
                        //};
                        //objP.objAnticipado1 = new Pagos_Detalles
                        //    {
                        //        Monto = "",
                        //        //Monto = FechaActual > DateTime.ParseExact(FechaS, "dd/MM/yyyy", Cultura) ? "" : objP.OfertaEducativa.DTOOfertaEducativaTipo.OfertaEducativaTipoId != 4 ? objP.objAnticipado2.Monto.Length > 0 ?
                        //        //objP.PagoId > idPago ? objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800 ?
                        //        //(Math.Round(CuotaAnterior.Monto * decimal.Parse("0.96"))).ToString() :
                        //        //objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 802 ? (Math.Round(CuotaAnterior.Monto * decimal.Parse("0.96"))).ToString() : "" : "" : "" : "",
                        //        FechaLimite = ""
                        //        //FechaLimite = FechaActual > DateTime.ParseExact(FechaS, "dd/MM/yyyy", Cultura) ? "" : objP.OfertaEducativa.DTOOfertaEducativaTipo.OfertaEducativaTipoId != 4 ?
                        //        //objP.PagoId > idPago ? objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800 ?
                        //        //DateTime.ParseExact(FechaS, "dd/MM/yyyy", Cultura).ToString("dd/MM/yyyy", Cultura) :
                        //        //objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 802 ?
                        //        //DateTime.ParseExact(FechaS, "dd/MM/yyyy", Cultura).ToString("dd/MM/yyyy", Cultura) :
                        //        //"" : "" : ""

                        //        // FechaLimite = (objP.DTOPeriodo.FechaInicial.AddDays(-1)).ToShortDateString()
                        //    };
                        //if (objP.objAnticipado1.Monto != "")
                        //{
                        //    objP.objAnticipado1.Monto = int.Parse(objP.objAnticipado1.Monto.Substring(objP.objAnticipado1.Monto.Length - 1, 1)) > 5 ?
                        //        ((decimal.Parse(objP.objAnticipado1.Monto) + 10) - int.Parse(objP.objAnticipado1.Monto.Substring(objP.objAnticipado1.Monto.Length - 1, 1))).ToString("C", Cultura) :
                        //        int.Parse(objP.objAnticipado1.Monto.Substring(objP.objAnticipado1.Monto.Length - 1, 1)) == 5 ? decimal.Parse(objP.objAnticipado1.Monto).ToString("C", Cultura) :
                        //        ((decimal.Parse(objP.objAnticipado1.Monto)) - int.Parse(objP.objAnticipado1.Monto.Substring(objP.objAnticipado1.Monto.Length - 1, 1))).ToString("C", Cultura);
                        //}
                        //objP.objRetrasado = new Pagos_Detalles
                        //{
                        //    FechaLimite = objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800 ? DateTime.ParseExact(objP.objNormal.FechaLimite, "dd/MM/yyyy", Cultura).AddDays(1).ToString("dd/MM/yyyy", Cultura) : "",
                        //    Monto = objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800 ? (objP.Cuota + ((objP.Cuota * 5) / 100)).ToString() : "",
                        //};



                    });

                    return lstPagos;
                }
                catch (Exception e)
                {
                    return new List<DTOPagos>() {new DTOPagos{
                        objNormal= new Pagos_Detalles{
                            Observaciones=e.Message
}
                    }};
                }
            }
        }

        public static PantallaPago ReferenciasPago(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                List<Pago> ListaPagos = db.Pago.Where(P => P.AlumnoId == AlumnoId
                                                        && P.EstatusId != 2
                                                        && (P.Cuota1.PagoConceptoId != 1007 && P.Cuota1.PagoConceptoId != 1001))
                                            .AsNoTracking().ToList();

                List<Periodo> Periodos = ListaPagos.Where(P => P.AlumnoId == AlumnoId
                                                        && P.EstatusId != 2
                                                        && (P.Cuota1.PagoConceptoId != 1007 && P.Cuota1.PagoConceptoId != 1001)
                                                        && (P.Anio != 2016 || P.PeriodoId != 1 || (P.EstatusId == 14 || P.EstatusId == 4)))
                                            .Select(p => p.Periodo)
                                            .GroupBy(p => new { p.Anio, p.PeriodoId })
                                            .Select(p => p.FirstOrDefault())
                                            .ToList()
                                            .OrderBy(a=> a.Anio).ThenBy(a=> a.PeriodoId)
                                            .ToList();
                List<AlumnoDescuento> ListaDescuentosAlumno = db.AlumnoDescuento
                                                       .Where(ad => ad.AlumnoId == AlumnoId
                                                                    && ad.EstatusId != 3)
                                                        .ToList();
                

                List<Pago> Adeudos = ListaPagos.Where(P => P.AlumnoId == AlumnoId && P.Anio == 2016 && P.PeriodoId == 1
                        && (P.EstatusId == 1 || P.EstatusId == 13)).ToList();

                List<DTOPagoDetallado> PagosDetalles = new List<DTOPagoDetallado>();
                List<DTOPagoDetallado> PagosAdeudos = new List<DTOPagoDetallado>();

                decimal porpagar20161 = 0;
                Adeudos.ForEach(Pago =>
                {


                    decimal total = 0;
                    total = Pago.Promesa - (Pago.Promesa - Pago.Restante);
                    porpagar20161 += total;

                    DTOPagoDetallado PagoAdeudo = new DTOPagoDetallado();
                    PagoAdeudo.Concepto = "Adeudo, periodos anteriores";
                    PagoAdeudo.ReferenciaId = int.Parse(Pago.ReferenciaId).ToString();
                    PagoAdeudo.Cargo_Descuento = Pago.Promesa.ToString("C", Cultura);
                    PagoAdeudo.CargoFechaLimite = "";
                    PagoAdeudo.DescuentoXAnticipado = "";
                    PagoAdeudo.CargoMonto = Pago.Promesa.ToString("C", Cultura);
                    PagoAdeudo.BecaAcademica_Pcj = "";
                    PagoAdeudo.BecaOpcional_Monto = "";
                    PagoAdeudo.Total_a_PagarS = total.ToString("C", Cultura);
                    PagoAdeudo.SaldoPagado = total.ToString("C", Cultura);
                    PagoAdeudo.Pagoid = Pago.EstatusId != 1 ? Pago.PagoId : 0;
                    PagoAdeudo.TotalMDescuentoMBecas = Pago.Promesa.ToString("C", Cultura);
                    PagoAdeudo.Adeudo = true;
                    PagoAdeudo.BecaSEP = null;
                    PagoAdeudo.SaldoAdeudo = total;
                    PagoAdeudo.OfertaEducativaId = Pago.OfertaEducativaId;
                    PagoAdeudo.EsSep = 2;
                    PagoAdeudo.OtroDescuento = "";
                    PagoAdeudo.Anio = Pago.Anio;
                    PagoAdeudo.PeriodoId = Pago.PeriodoId;

                    PagosAdeudos.Add(PagoAdeudo);
                    PagosDetalles.Add(PagoAdeudo);
                });

                if (PagosDetalles.Count > 0)
                {
                    PagosDetalles.Insert(0, new DTOPagoDetallado
                    {
                        Anio = 2016,
                        PeriodoId = 1,
                        Concepto = "Septiembre - Diciembre 2015",
                        Titulo = true,
                        esEmpresa=true
                    });
                }

                if (Periodos.Where(per => per.Anio == 2016 && per.PeriodoId == 1).ToList().Count == 0 && PagosDetalles.Count > 0)
                {
                    Periodos.Insert(0,new Periodo
                    {
                        Anio = 2016,
                        PeriodoId = 1,
                        Descripcion = "Septiembre - Diciembre 2015",
                    });
                }

                List<DTOPeriodoReferencias> PeriodosDTO = Periodos.Select(per =>
                                                                 new DTOPeriodoReferencias
                                                                 {
                                                                     Anio = per.Anio,
                                                                     PeriodoId = per.PeriodoId,
                                                                     Descripcion = per.Descripcion,
                                                                     Total = per.Anio == 2016 && per.PeriodoId == 1 ? porpagar20161 : 0,
                                                                     EsSep = per.Anio == 2016 && per.PeriodoId == 1 ? 2 : 0,
                                                                     BecaSEP= per.Anio == 2016 && per.PeriodoId == 1 ? "" : "",
                                                                     EsEmpresa = per.Anio == 2016 && per.PeriodoId == 1 ? true: false,
                                                                 }).ToList();

                Periodos.ForEach(Periodobd =>
                {
                    int ofertaid = 0;
                    List<Pago> Pagos = ListaPagos.Where(P => P.AlumnoId == AlumnoId
                                                            && P.Anio == Periodobd.Anio
                                                            && P.PeriodoId == Periodobd.PeriodoId
                                                            && P.EstatusId != 2
                                                            && P.EstatusId != 3
                                                            && (P.Cuota1.PagoConceptoId != 1007 && P.Cuota1.PagoConceptoId != 1001)
                                                            && (P.Anio != 2016 || P.PeriodoId != 1 || (P.EstatusId == 14 || P.EstatusId == 4)))
                                                .ToList();

                    Pagos = Pagos.OrderBy(P => P.OfertaEducativaId).ToList();
                    Pagos = Pagos.Where(p => p.EstatusId != 2).ToList();
                    if (Pagos.Count == 0)
                    {
                        if (PagosDetalles.Count > 0)
                        {
                            decimal Total = 0;
                            PagosDetalles.ForEach(delegate (DTOPagoDetallado Pago)
                            {
                                Total += Pago.SaldoAdeudo;
                            });
                            PagosDetalles[0].TotalPagado = Total.ToString("C", Cultura);
                        }
                    }

                    #region Ciclo de Pagos
                    Pagos.ForEach(Pago =>
                    {
                        try
                        {
                            #region Descripcion de Periodo 
                            if (PagosDetalles.Where(pdt => pdt.Titulo == true
                                                     && pdt.Anio == Periodobd.Anio
                                                     && pdt.PeriodoId == Periodobd.PeriodoId)
                                               .ToList().Count == 0)
                            {
                                PagosDetalles.Add(new DTOPagoDetallado
                                {
                                    Anio = Periodobd.Anio,
                                    PeriodoId = Periodobd.PeriodoId,
                                    Concepto = Periodobd.Descripcion,
                                    Titulo = true,

                                });
                            }
                            #endregion
                            #region Descripcion de Ofertas 
                            if (ofertaid == 0)
                            {
                                ofertaid = Pago.OfertaEducativaId;
                                PagosDetalles.Add(new DTOPagoDetallado
                                {
                                    BecaAcademica_Monto = "",
                                    BecaAcademica_Pcj = "",
                                    Cargo_Descuento = "",
                                    CargoFechaLimite = "",
                                    CargoMonto = "",
                                    Concepto = Pago.OfertaEducativa.Descripcion,
                                    OfertaEducativaId = Pago.OfertaEducativaId,
                                    DescripcionOferta = "",
                                    ReferenciaId = "",
                                    SaldoPagado = "",
                                    Total_a_PagarS = "",
                                    TotalMDescuentoMBecas = "",
                                    EsSep = 2,
                                    OtroDescuento = "",
                                    Anio = Pago.Anio,
                                    PeriodoId = Pago.PeriodoId
                                });
                            }
                            else if (ofertaid != Pago.OfertaEducativaId)
                            {
                                ofertaid = Pago.OfertaEducativaId;
                                PagosDetalles.Add(new DTOPagoDetallado
                                {
                                    BecaAcademica_Monto = "",
                                    BecaAcademica_Pcj = "",
                                    Cargo_Descuento = "",
                                    CargoFechaLimite = "",
                                    CargoMonto = "",
                                    Concepto = Pago.OfertaEducativa.Descripcion,
                                    OfertaEducativaId = Pago.OfertaEducativaId,
                                    DescripcionOferta = "",
                                    ReferenciaId = "",
                                    SaldoPagado = "",
                                    Total_a_PagarS = "",
                                    TotalMDescuentoMBecas = "",
                                    EsSep = 2,
                                    OtroDescuento = "",
                                    Anio = Pago.Anio,
                                    PeriodoId = Pago.PeriodoId
                                });
                            }
                            #endregion
                            #region Pagos 2016 - 1 
                            //Adeudos Anteriores
                            if (Pago.Anio == 2016 && Pago.PeriodoId == 1)
                            {
                                DTOPagoDetallado objanterior = new DTOPagoDetallado();

                                objanterior.BecaAcademica_Monto = "";
                                objanterior.BecaAcademica_Pcj = "";
                                objanterior.Cargo_Descuento = "";
                                objanterior.CargoFechaLimite = "";
                                objanterior.CargoMonto = Pago.Promesa.ToString("C", Cultura);
                                objanterior.Concepto = "Adeudo, periodos anteriores";
                                objanterior.OfertaEducativaId = Pago.OfertaEducativaId;
                                objanterior.DescripcionOferta = Pago.OfertaEducativa.Descripcion;
                                objanterior.ReferenciaId = (int.Parse(Pago.ReferenciaId)).ToString();
                                objanterior.SaldoPagado = Pago.Restante.ToString("C", Cultura); ;
                                objanterior.Total_a_PagarS = Pago.Promesa.ToString("C", Cultura);
                                objanterior.TotalMDescuentoMBecas = Pago.Promesa.ToString("C", Cultura);
                                objanterior.EsSep = 2;
                                objanterior.OtroDescuento = "";
                                objanterior.Anio = Pago.Anio;
                                objanterior.PeriodoId = Pago.PeriodoId;
                                objanterior.Pagoid = Pago.EstatusId != 1 ? Pago.PagoId : 0;

                                PeriodosDTO.Where(a => a.Anio == Pago.Anio && a.PeriodoId == Pago.PeriodoId)
                                                .Select(k => k.Total = (k.Total + Pago.Restante));
                                PeriodosDTO.Where(a => a.Anio == Pago.Anio && a.PeriodoId == Pago.PeriodoId)
                                                .FirstOrDefault().EsSep = 2;
                                PeriodosDTO.Where(a => a.Anio == Pago.Anio && a.PeriodoId == Pago.PeriodoId)
                                                .FirstOrDefault().BecaSEP = "";


                                PagosDetalles.Add(objanterior);
                            }
                            #endregion
                            #region Pagos Normales 
                            //Pago Normal
                            else
                            {
                                DTOPagoDetallado PagosDetallesAgregar = new DTOPagoDetallado();
                                List<PagoDescuento> PagosAnticipados = Pago.PagoDescuento.Where(PD => PD.Descuento.Descripcion == "Pago Anticipado").ToList();
                                List<Descuento> ListaDescuentosAP = db
                                                                    .Descuento
                                                                    .Where(D =>
                                                                         (D.Descripcion == "Beca Académica"
                                                                          || D.Descripcion == "Beca SEP"
                                                                          || D.Descripcion == "Descuento en inscripción"
                                                                          || D.Descripcion == "Descuento en colegiatura"
                                                                          || D.Descripcion == "Descuento en examen diagnóstico"
                                                                          || D.Descripcion == "Descuento en credencial nuevo ingreso"
                                                                          || D.Descripcion == "Descuento en credencial nuevo ingreso, idiomas"
                                                                          || D.Descripcion == "Descuento en Adelanto de materia, Licenciatura"
                                                                          || D.Descripcion == "Descuento en Adelanto de materia, Maestría"
                                                                          || D.Descripcion == "Descuento en Examen global")
                                                                          && D.OfertaEducativaId == ofertaid
                                                                          && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId
                                                                        ).ToList();

                                List<AlumnoDescuento> DescuentosAlumno =
                                    ListaDescuentosAlumno.Where(A => A.Anio == Periodobd.Anio && A.PeriodoId == Periodobd.PeriodoId &&
                                       ListaDescuentosAP.Where(lda => lda.DescuentoId == A.DescuentoId).ToList().Count > 0
                                        && A.OfertaEducativaId == Pago.OfertaEducativaId && A.PagoConceptoId == Pago.Cuota1.PagoConceptoId && A.EstatusId == 2).ToList();
                                //BecaDeportiva
                                List<AlumnoDescuento> DescuentosBecaDeportiva = ListaDescuentosAlumno.Where(A => A.AlumnoId == AlumnoId
                                                                                                        && A.Anio == Periodobd.Anio
                                                                                                        && A.PeriodoId == Periodobd.PeriodoId
                                                                                                        && A.OfertaEducativaId == Pago.OfertaEducativaId
                                                                                                        && A.PagoConceptoId == Pago.Cuota1.PagoConceptoId
                                                                                                        && A.EstatusId == 2
                                                                                                        && A.EsDeportiva == true)
                                                                                                        .ToList();

                                decimal Descuento = Pago.PagoDescuento != null ?
                                   PagosAnticipados?.FirstOrDefault()?.Monto ?? 0 : 0;

                                decimal Beca = DescuentosAlumno.Count > 0 ?
                                    Pago.PagoDescuento.Count > 0 ? (from a in Pago.PagoDescuento
                                                                    where DescuentosAlumno.Where(s => s.DescuentoId == a.DescuentoId).ToList().Count > 0
                                                                    select a).FirstOrDefault()?.Monto ?? 0 : 0 : 0;

                                decimal DescuentoBecaDeportiva = DescuentosBecaDeportiva.Count > 0 ?
                                                        Pago.PagoDescuento?.Where(P => P.DescuentoId == DescuentosBecaDeportiva.FirstOrDefault().DescuentoId)?.FirstOrDefault()?.Monto ?? 0
                                                    : 0;
                                int DescuentoSepID = ListaDescuentosAP.Where(lda => lda.Descripcion == "Beca SEP")?.FirstOrDefault()?.DescuentoId ?? 0;

                                int DescuentoAcdID = ListaDescuentosAP.Where(D => D.Descripcion == "Beca Académica")?.FirstOrDefault()?.DescuentoId ?? 0;

                                decimal PromoCasa = Pago.PagoDescuento.Where(O => O.Descuento.Descripcion == "Promoción en Casa").ToList().Sum(O => O.Monto);
                                PagosDetallesAgregar.OtroDescuento = PromoCasa > 0 ? PromoCasa.ToString("C", Cultura) : "";

                                int Comite = DescuentosAlumno.Where(d => d.EsComite == true).ToList().Count > 0 ? 3 : 0;

                                decimal Monto = 0;
                                Monto = Pago.Cuota == 0 ? Pago.Promesa : Pago.Cuota;

                                decimal total = 0;
                                total = ((Monto - Descuento - Beca - DescuentoBecaDeportiva - PromoCasa) - (Pago.Promesa - Pago.Restante));
                                total = total < 0 ? 0 : total;


                                PagosDetallesAgregar.Pagoid = Pago.PagoId;
                                //Concepto
                                PagosDetallesAgregar.Concepto =
                                Pago.Cuota1.PagoConceptoId == 800 ? Pago.Cuota1.PagoConcepto.Descripcion + ", " +
                                    Pago.Subperiodo.Mes.Descripcion + " " + (Pago.PeriodoId == 1 ? (Pago.Anio - 1).ToString() :
                                    Pago.Anio.ToString()) : Pago.Cuota1.PagoConcepto.Descripcion + " " + (
                                    Pago.PagoRecargo1.Count > 0 ?
                                    Pago.PagoRecargo1.Where(P => P.PagoIdRecargo == Pago.PagoId).FirstOrDefault().Pago.Cuota1.PagoConcepto.Descripcion +
                                     (Pago.PagoRecargo1.Where(P => P.PagoIdRecargo == Pago.PagoId).FirstOrDefault().Pago.Cuota1.PagoConceptoId == 800 ?
                                     ", " + Pago.PagoRecargo1.Where(P => P.PagoIdRecargo == Pago.PagoId).FirstOrDefault().Pago.Subperiodo.Mes.Descripcion + " " +
                                     (Pago.PagoRecargo1.Where(P => P.PagoIdRecargo == Pago.PagoId).FirstOrDefault().Pago.PeriodoId == 1 ?
                                     (Pago.PagoRecargo1.Where(P => P.PagoIdRecargo == Pago.PagoId).FirstOrDefault().Pago.Anio - 1).ToString() :
                                     Pago.PagoRecargo1.Where(P => P.PagoIdRecargo == Pago.PagoId).FirstOrDefault().Pago.Anio.ToString()) : "") : "") +
                                     Pago?.PagoDescripcion?.Descripcion;


                                PagosDetallesAgregar.ReferenciaId = int.Parse(Pago.ReferenciaId).ToString();
                                PagosDetallesAgregar.CargoMonto = Pago.Cuota == 0 ? Pago.Promesa.ToString("C", Cultura) : Pago.Cuota.ToString("C", Cultura);
                                PagosDetallesAgregar.CargoFechaLimite = Pago.Cuota1.PagoConceptoId == 800 || Pago.Cuota1.PagoConceptoId == 802 ? Utilities.Fecha.Prorroga((Pago.PeriodoId > 1 ? Pago.Anio : Pago.Anio - 1),
                                    db.Subperiodo.Where(s => s.PeriodoId == Pago.PeriodoId && s.SubperiodoId == Pago.SubperiodoId)
                                    .FirstOrDefault().MesId, true, 5).ToString("dd/MM/yyyy", Cultura) :
                                    Pago.FechaGeneracion.AddDays(5).ToString("dd/MM/yyyy", Cultura);//; Pago.FechaGeneracion.ToString("dd/MM/yyyy", Cultura);

                                PagosDetallesAgregar.DescuentoXAnticipado = Descuento > 0 ? Descuento.ToString("C", Cultura) : " ";

                                PagosDetallesAgregar.Cargo_Descuento = Pago.Cuota == 0 ? (Pago.Promesa - (Descuento > 0 ? Descuento : 0)).ToString("C", Cultura) :
                                    (Pago.Cuota - (Descuento > 0 ? Descuento : 0)).ToString("C", Cultura);

                                PagosDetallesAgregar.BecaAcademica_Pcj = DescuentosAlumno.Count > 0 ? DescuentosAlumno.FirstOrDefault().Monto.ToString() + "%" : " ";

                                PagosDetallesAgregar.BecaAcademica_Monto = DescuentosAlumno.Count > 0 ?
                                    Beca.ToString("C", Cultura) : " ";


                                PagosDetallesAgregar.BecaOpcional_Pcj = DescuentosBecaDeportiva?.FirstOrDefault()?.Monto.ToString() + "%" ?? " ";
                                PagosDetallesAgregar.BecaOpcional_Monto = DescuentosBecaDeportiva.Count > 0 ? DescuentoBecaDeportiva.ToString("C", Cultura) : " ";
                                PagosDetallesAgregar.TotalMDescuentoMBecas = (Monto - Descuento - Beca - DescuentoBecaDeportiva - PromoCasa).ToString("C", Cultura);
                                PagosDetallesAgregar.SaldoPagado = total.ToString("C", Cultura);
                                PagosDetallesAgregar.OfertaEducativaId = Pago.OfertaEducativaId;
                                PagosDetallesAgregar.DescripcionOferta = Pago.OfertaEducativa.Descripcion;
                                PagosDetallesAgregar.Anio = Pago.Anio;
                                PagosDetallesAgregar.PeriodoId = Pago.PeriodoId;

                                PagosDetalles.Add(PagosDetallesAgregar);
                                PagosDetalles[Adeudos.Count > 0 ? 1 : 0].Total_a_Pagar += total;

                                PeriodosDTO.Where(a => a.Anio == Pago.Anio
                                                        && a.PeriodoId == Pago.PeriodoId)
                                                .FirstOrDefault().Total =
                                                                            (PeriodosDTO.Where(a => a.Anio == Pago.Anio
                                                                                                && a.PeriodoId == Pago.PeriodoId)
                                                                            .FirstOrDefault().Total + total);

                                ///////////////////Corregir
                                if (Pago.Cuota1.PagoConceptoId == 800 || Pago.Cuota1.PagoConceptoId == 802)
                                {
                                    PagosDetallesAgregar.EsSep =
                                                    Comite == 3
                                                    ? Comite
                                                    : DescuentosAlumno.Where(d => d.EsSEP).ToList().Count > 0 ? 1
                                                    : 2;
                                    PagosDetalles[0].EsSep = PagosDetallesAgregar.EsSep;
                                    PagosDetalles[0].BecaSEP = PagosDetalles[0].BecaSEP == null ? DescuentosBecaDeportiva.Count > 0 ? "Beca Deportiva" : null : PagosDetalles[0].BecaSEP;

                                    PeriodosDTO.Where(a => a.Anio == Pago.Anio
                                                      && a.PeriodoId == Pago.PeriodoId)
                                              .FirstOrDefault().EsSep = Comite == 3
                                                    ? Comite
                                                    : DescuentosAlumno.Where(d => d.EsSEP).ToList().Count > 0 ? 1
                                                    : 2;
                                    PeriodosDTO.Where(a => a.Anio == Pago.Anio
                                                                                          && a.PeriodoId == Pago.PeriodoId)
                                                                                  .FirstOrDefault().BecaSEP = PagosDetalles[0].BecaSEP == null ? DescuentosBecaDeportiva.Count > 0 ? "Beca Deportiva" : null : PagosDetalles[0].BecaSEP;
                                    PeriodosDTO.Where(a => a.Anio == Pago.Anio
                                                        && a.PeriodoId == Pago.PeriodoId)
                                                    .FirstOrDefault().EsEmpresa = (PeriodosDTO.Where(a => a.Anio == Pago.Anio
                                                         && a.PeriodoId == Pago.PeriodoId)
                                                    .FirstOrDefault().EsEmpresa == false ?
                                                                                  (Pago.Alumno.AlumnoInscrito.Where(a =>
                                                                                                                    a.OfertaEducativaId == Pago.OfertaEducativaId
                                                                                                                    && a.EsEmpresa).ToList().Count > 0 ? true : false)
                                                                                  : true);
                                }
                                if (Pago.OfertaEducativa.OfertaEducativaTipoId == 4)
                                {
                                    PeriodosDTO.Where(a => a.Anio == Pago.Anio
                                                       && a.PeriodoId == Pago.PeriodoId)
                                                   .FirstOrDefault().EsEmpresa = true;
                                }
                            }
                            #endregion
                        }
                        catch
                        {

                        }
                    });
                    #endregion
                });

                int ofert = PagosDetalles[0].OfertaEducativaId;

                PagosDetalles[Adeudos.Count > 0 ? 1 : 0].Total_a_Pagar += Adeudos.Count > 0 ? PagosAdeudos.Sum(P => P.SaldoAdeudo) : 0;
                PagosDetalles[Adeudos.Count > 0 ? 1 : 0].Total_a_PagarS = PagosDetalles[Adeudos.Count > 0 ? 1 : 0].Total_a_Pagar.ToString("C", Cultura);
                PagosDetalles[0].TotalPagado = PagosDetalles[Adeudos.Count > 0 ? 1 : 0].Total_a_PagarS;
                PagosDetalles[0].esEmpresa = db.AlumnoInscrito.Where(a =>
                                            a.AlumnoId == AlumnoId &&
                                            a.EsEmpresa == true).ToList().Count > 0 ? true : false;
                if (PagosDetalles[0].esEmpresa)
                {
                    var alConf = db.GrupoAlumnoConfiguracion.Where(k =>
                                 k.AlumnoId == AlumnoId && k.EsEspecial == true).ToList();
                    PagosDetalles[0].esEspecial = alConf.Count > 0 ? true : false;
                }
                //PagosDetalles[0].BecaSEP = tpBeca == 3 ? "Beca Comite" : "Beca SEP";

                return
                    new PantallaPago
                    {
                        Pagos = PagosDetalles,
                        Estatus = PagosDetalles.Where(l => (l?.OtroDescuento?.Length ?? 0) > 0).ToList().Count > 0 ? true : false,
                        Periodos = PeriodosDTO
                    };

            }
        }

        public static List<DTOPagos> ConsultarReferenciasConceptosCancelar(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DateTime FechaActual = DateTime.Now;
                    List<Pago> Pagos = db.Pago.Where(p => p.AlumnoId == AlumnoId
                    ).OrderByDescending(f => f.FechaGeneracion).ToList();


                    List<DTOPagos> PagosDTO = new List<DTOPagos>();
                    Pagos.ForEach(p =>
                    {
                        try
                        {

                            DTOPagos PagoDTO = new DTOPagos();

                            PagoDTO.PagoId = p.PagoId;
                            PagoDTO.AlumnoId = p.AlumnoId;
                            PagoDTO.Anio = p.Anio;
                            PagoDTO.PeriodoId = p.PeriodoId;
                            PagoDTO.DTOPeriodo = new DTOPeriodo
                            {
                                PeriodoId = p.Periodo.PeriodoId,
                                Anio = p.Periodo.Anio,
                            };
                            PagoDTO.SubperiodoId = p.SubperiodoId;
                            PagoDTO.OfertaEducativaId = p.OfertaEducativaId;
                            PagoDTO.FechaGeneracion = p.FechaGeneracion;
                            PagoDTO.CuotaId = p.CuotaId;
                            PagoDTO.Cuota = p.Cuota;
                            PagoDTO.FechaPago = (DateTime)new DateTime(p.Anio, p.Subperiodo.MesId, 01);
                            PagoDTO.DTOCuota = new DTOCuota
                            {
                                CuotaId = p.Cuota1.CuotaId,
                                Anio = p.Cuota1.Anio,
                                PeriodoId = p.Cuota1.PeriodoId,
                                OfertaEducativaId = p.Cuota1.OfertaEducativaId,
                                PagoConceptoId = p.Cuota1.PagoConceptoId,
                                Monto = p.Cuota1.Monto,
                                EsEmpresa = p.Cuota1.EsEmpresa,
                                DTOPagoConcepto = new DTOPagoConcepto
                                {
                                    CuentaContable = p.Cuota1.PagoConcepto.CuentaContable,
                                    Descripcion = p.Cuota1.PagoConceptoId == 800 || p.Cuota1.PagoConceptoId == 802 ? (p.Cuota1.PagoConcepto.Descripcion +
                                                                                         " " + p.Subperiodo.Mes.Descripcion +
                                                                                         " " + p.Periodo.FechaInicial.Year.ToString())
                                                                                         : p.Cuota1.PagoConcepto.Descripcion
                                               + " " + (p.Cuota1.PagoConceptoId == 306 ?
                                               p.PagoRecargo1?.FirstOrDefault()?.Pago?.Cuota1?.PagoConcepto?.Descripcion ?? ""
                                               + " " + p.PagoRecargo1?.FirstOrDefault()?.Pago?.Subperiodo?.Mes?.Descripcion ?? "" + " " +
                                               p.PagoRecargo1?.FirstOrDefault()?.Pago?.Periodo?.FechaInicial.Year.ToString() ?? ""
                                               : ""),
                                    EstatusId = p.Cuota1.PagoConcepto.EstatusId,
                                    OfertaEducativaId = p.Cuota1.PagoConcepto.OfertaEducativaId,
                                    PagoConceptoId = p.Cuota1.PagoConcepto.PagoConceptoId
                                }
                            };
                            PagoDTO.Promesa = p.Promesa;
                            PagoDTO.Pagado = (p.Promesa - p.Restante).ToString();
                            PagoDTO.Restante = p.Restante.ToString();
                            PagoDTO.Referencia = p.ReferenciaId;
                            PagoDTO.EstatusId = p.EstatusId;
                            PagosDTO.Add(PagoDTO);
                        }
                        catch { }
                    });

                    PagosDTO.ForEach(Pago =>
                    {
                        try
                        {
                            Pago.FechaGeneracionS = Pago.FechaGeneracion.ToString("dd/MM/yyyy", Cultura);
                            Pago.Cancelable = Pago.FechaGeneracion.ToShortDateString() == FechaActual.ToShortDateString() ? true : false;
                            Pago.objNormal = new Pagos_Detalles
                            {
                                FechaLimite = (Utilities.Fecha.Prorroga(Pago.FechaPago.Value.Year, Pago.FechaPago.Value.Month, true, 5).ToString("dd/MM/yyyy", Cultura)),
                                Monto = Pago.Promesa.ToString("C", Cultura),
                                Restante = decimal.Parse(Pago.Pagado).ToString("C", Cultura),
                                Estatus = Pago.EstatusId == 2 ? "Cancelado" : Pago.Promesa == decimal.Parse(Pago.Restante) ? (Pago.Promesa == 0 ? "Pagado" : "Pendiente") : (Pago.Restante == "0.00" ? "Pagado" : "Parcialmente Pagado")
                            };
                            Pago.DTOCuota.PeridoAnio = Pago.Anio == 2016 && Pago.PeriodoId == 1 ? "" :
                            Pago.DTOCuota.Anio.ToString() + "-" + Pago.DTOCuota.PeriodoId.ToString();

                        }
                        catch { }
                    });
                    return PagosDTO;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static string TraerAdeudos(int AlumnoId, int OfertaEducativaid)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    int NumeroPagos = 0;
                    List<Pago> Pagos =
                        db.Pago.Where(P =>
                            P.AlumnoId == AlumnoId
                            && P.EstatusId == 1
                            && P.OfertaEducativaId == OfertaEducativaid
                            && (P.Anio != 2016 || P.PeriodoId != 1)
                            && (P.Cuota1.PagoConceptoId == 800
                                    || P.Cuota1.PagoConceptoId == 802
                                    || P.Cuota1.PagoConceptoId == 807
                                    || P.Cuota1.PagoConceptoId == 306
                                    || P.Cuota1.PagoConceptoId == 304
                                    || P.Cuota1.PagoConceptoId == 320
                                    || P.Cuota1.PagoConceptoId == 15
                                    || P.Cuota1.PagoConceptoId == 1004
                                    || P.Cuota1.PagoConceptoId == 26)).ToList();
                    Pagos.ForEach(Pago =>
                    {
                        DateTime FechaActual = DateTime.Now;
                        FechaActual += new TimeSpan(00, 00, 00);
                        DateTime FechaPago = Pago.Cuota1.PagoConceptoId == 800 ?
                            new DateTime(Pago.Periodo.FechaFinal.Year, Pago.Subperiodo.MesId, 01) : Pago.FechaGeneracion;

                        FechaPago += new TimeSpan(00, 00, 00);
                        FechaPago = FechaPago.AddDays(10);
                        NumeroPagos += FechaPago < FechaActual ? 1 : 0;
                    });
                    return NumeroPagos > 0 ? "Debe" : "";
                }
                catch (Exception a)
                { return a.Message; }
            }
        }

        public static string TraerAdeudosCoordinadores(int AlumnoId, int OfertaEducativaid)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    int nPagos = 0;

                    DateTime FechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                    Periodo objPerActual = db.Periodo.Where(d => FechaActual >= d.FechaInicial
                    && FechaActual <= d.FechaFinal).FirstOrDefault();



                    DateTime dtPeriodo;
                    List<Pago> Pagolist = db.Pago.Where(P => 
                                P.AlumnoId == AlumnoId 
                                && P.OfertaEducativaId == OfertaEducativaid
                                && P.EstatusId == 1 
                                && (P.Anio != 2016 || P.PeriodoId != 1) 
                                && (P.Cuota1.PagoConceptoId == 800
                                        || P.Cuota1.PagoConceptoId == 802 
                                        || P.Cuota1.PagoConceptoId == 807 
                                        || P.Cuota1.PagoConceptoId == 306
                                        || P.Cuota1.PagoConceptoId == 304 
                                        || P.Cuota1.PagoConceptoId == 320)).ToList();
                    Pagolist.ForEach(delegate (Pago objP)
                    {
                        dtPeriodo = BLLPeriodoPortal.TraerPeriodoCompletoS(objP.Anio, objP.PeriodoId, objP.SubperiodoId);
                        objP.FechaGeneracion = Fecha.Prorroga(dtPeriodo.Year, dtPeriodo.Month, true, 5);
                        nPagos += objP.FechaGeneracion < DateTime.Now ? 1 : 0;
                    });
                    nPagos = db.AlumnoPermitido.Where(P => P.AlumnoId == AlumnoId
                                                        && P.Anio == objPerActual.Anio
                                                        && P.PeriodoId == objPerActual.PeriodoId).ToList().Count > 0 ? 0 : nPagos;
                    return nPagos > 0 ? "Debe" : "";
                }
                catch (Exception a)
                { return a.Message; }
            }
        }

        public static List<DTOPagos> ConsultarReferencias(int AlumnoId, Boolean Procesadas)
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    //string Fechaq=(DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString())
                    //    + "/" +( DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + "/" + DateTime.Now.Year.ToString();
                    //DateTime FechaActual = DateTime.ParseExact(Fechaq, "dd/MM/yyyy", Cultura);
                    //int idPago = 2588;
                    string Anticipado;
                    string FechaS;
                    DTOCuota CuotaAnterior;
                    AlumnoInscrito objAlumno = db.AlumnoInscrito.Where(A => A.AlumnoId == AlumnoId).FirstOrDefault();
                    List<DTOAlumnoReferenciaBitacora> lstPagosGenerados = BLLAdeudoBitacora.TraerListaBitacora(AlumnoId);

                    List<DTOPagos> lstPagos;
                    if (Procesadas == false)
                    {
                        lstPagos =
                            ((from p in db.Pago
                              where p.AlumnoId == AlumnoId && p.EstatusId != 2 && p.Cuota1.PagoConceptoId != 1007
                              orderby p.PagoId ascending
                              select new DTOPagos
                              {
                                  PagoRecargo = db.PagoRecargo.Select(a => new DTOPagoRecargo
                                  {
                                      PagoId = a.PagoId,
                                      PagoIdRecargo = a.PagoIdRecargo
                                  }).Where(a => a.PagoIdRecargo == p.PagoId).FirstOrDefault(),
                                  PagoId = p.PagoId,
                                  AlumnoId = p.AlumnoId,
                                  Anio = p.Anio,
                                  PeriodoId = p.PeriodoId,
                                  PeriodoAnticipadoId = p.PeriodoAnticipadoId,
                                  DTOParcial = (from PP in db.PagoParcial
                                                where
                                                    PP.PagoId == p.PagoId
                                                select new DTOPagoParcial
                                                {
                                                    PagoId = PP.PagoId,
                                                    SucursalCajaId = (int)PP.SucursalCajaId,
                                                    Pago = PP.Pago,
                                                    FechaPago = PP.FechaPago,

                                                }).ToList(),
                                  DTOPeriodo = new DTOPeriodo
                                  {
                                      PeriodoId = p.Periodo.PeriodoId,
                                      Anio = p.Periodo.Anio,
                                      Descripcion = p.Periodo.Descripcion,
                                      FechaInicial = p.Periodo.FechaInicial,
                                      FechaFinal = p.Periodo.FechaFinal,
                                      Meses = p.Periodo.Meses
                                  },
                                  SubperiodoId = p.SubperiodoId,
                                  DTOSubPeriodo = new DTOSubPeriodo
                                  {
                                      SubperiodoId = p.Subperiodo.SubperiodoId,
                                      PeriodoId = p.Subperiodo.PeriodoId,
                                      MesId = p.Subperiodo.MesId,
                                      Mes = (from c in db.Mes
                                             where c.MesId == p.Subperiodo.MesId
                                             select new DTOMes
                                             {
                                                 Descripcion = c.Descripcion,
                                                 MesId = c.MesId,
                                             }).FirstOrDefault()
                                  },
                                  OfertaEducativaId = p.OfertaEducativaId,
                                  OfertaEducativa = new DTOOfertaEducativa
                                  {
                                      OfertaEducativaId = p.OfertaEducativa.OfertaEducativaId,
                                      OfertaEducativaTipoId = p.OfertaEducativa.OfertaEducativaTipoId,
                                      Descripcion = p.OfertaEducativa.Descripcion,
                                      DTOOfertaEducativaTipo = new DTOOfertaEducativaTipo
                                      {
                                          Descripcion = p.OfertaEducativa.OfertaEducativaTipo.Descripcion,
                                          OfertaEducativaTipoId = p.OfertaEducativa.OfertaEducativaTipo.OfertaEducativaTipoId
                                      }
                                  },
                                  FechaGeneracion = p.FechaGeneracion,
                                  CuotaId = p.CuotaId,
                                  Cuota = p.Cuota,
                                  DTOCuota = new DTOCuota
                                  {
                                      CuotaId = p.Cuota1.CuotaId,
                                      Anio = p.Cuota1.Anio,
                                      PeriodoId = p.Cuota1.PeriodoId,
                                      OfertaEducativaId = p.Cuota1.OfertaEducativaId,
                                      PagoConceptoId = p.Cuota1.PagoConceptoId,
                                      Monto = p.Cuota1.Monto,
                                      //EsEmpresa = p.Cuota1.EsEmpresa,
                                      DTOPagoConcepto = new DTOPagoConcepto
                                      {
                                          //CuentaContable = p.Cuota1.PagoConcepto.CuentaContable,
                                          Descripcion = p.Cuota1.PagoConcepto.Descripcion,
                                          EstatusId = p.Cuota1.PagoConcepto.EstatusId,
                                          //OfertaEducativaId = p.Cuota1.PagoConcepto.OfertaEducativaId,
                                          PagoConceptoId = p.Cuota1.PagoConcepto.PagoConceptoId
                                      }
                                  },
                                  Promesa = p.Promesa,
                                  Referencia = p.ReferenciaId,
                                  EstatusId = p.EstatusId,
                                  lstPagoDescuento = (from pd in db.PagoDescuento
                                                      join ad in db.AlumnoDescuento on pd.DescuentoId equals ad.DescuentoId
                                                      where pd.PagoId == p.PagoId && ad.AlumnoId == AlumnoId && p.Anio == ad.Anio && p.PeriodoId == ad.PeriodoId && ad.EstatusId != 3
                                                      select new DTOPagoDescuento
                                                      {
                                                          DescuentoId = pd.DescuentoId,
                                                          Monto = pd.Monto,
                                                          PagoId = pd.PagoId,
                                                          DTOAlumnDes = new DTOAlumnoDescuento
                                                          {
                                                              AlumnoId = ad.AlumnoId,
                                                              Anio = ad.Anio,
                                                              ConceptoId = ad.PagoConceptoId,
                                                              DescuentoId = ad.DescuentoId,
                                                              EstatusId = ad.EstatusId,
                                                              Monto = objAlumno.EsEmpresa == true ? 0 : ad.Monto,
                                                              SMonto = objAlumno.EsEmpresa == true ? "0%" : ad.Monto.ToString() + "%",
                                                              OfertaEducativaId = ad.OfertaEducativaId,
                                                              PeriodoId = ad.PeriodoId
                                                          }
                                                      }).ToList()
                                  //DTOReferenciaBitacora = new DTOAlumnoReferenciaBitacora
                                  //{
                                  //    AlumnoId=p.AlumnoReferenciaBitacora.
                                  //}
                              }).ToList());
                    }

                    else
                    {
                        lstPagos = ((from p in db.Pago
                                     where p.AlumnoId == AlumnoId && p.Cuota1.PagoConceptoId != 1007 &&
                            p.EstatusId != 2 && p.EstatusId != 4 && p.EstatusId != 14
                                     orderby p.PagoId ascending
                                     select new DTOPagos
                                     {
                                         PagoRecargo = db.PagoRecargo.Select(a => new DTOPagoRecargo
                                         {
                                             PagoId = a.PagoId,
                                             PagoIdRecargo = a.PagoIdRecargo
                                         }).Where(a => a.PagoIdRecargo == p.PagoId).FirstOrDefault(),
                                         PagoId = p.PagoId,
                                         AlumnoId = p.AlumnoId,
                                         Anio = p.Anio,
                                         PeriodoId = p.PeriodoId,
                                         PeriodoAnticipadoId = p.PeriodoAnticipadoId,
                                         DTOParcial = (from PP in db.PagoParcial
                                                       where
                                                           PP.PagoId == p.PagoId
                                                       select new DTOPagoParcial
                                                       {
                                                           PagoId = PP.PagoId,
                                                           SucursalCajaId = (int)PP.SucursalCajaId,
                                                           Pago = PP.Pago,
                                                           FechaPago = PP.FechaPago,

                                                       }).ToList(),
                                         DTOPeriodo = new DTOPeriodo
                                         {
                                             PeriodoId = p.Periodo.PeriodoId,
                                             Anio = p.Periodo.Anio,
                                             Descripcion = p.Periodo.Descripcion,
                                             FechaInicial = p.Periodo.FechaInicial,
                                             FechaFinal = p.Periodo.FechaFinal,
                                             Meses = p.Periodo.Meses
                                         },
                                         SubperiodoId = p.SubperiodoId,
                                         DTOSubPeriodo = new DTOSubPeriodo
                                         {
                                             SubperiodoId = p.Subperiodo.SubperiodoId,
                                             PeriodoId = p.Subperiodo.PeriodoId,
                                             MesId = p.Subperiodo.MesId,
                                             Mes = (from c in db.Mes
                                                    where c.MesId == p.Subperiodo.MesId
                                                    select new DTOMes
                                                    {
                                                        Descripcion = c.Descripcion,
                                                        MesId = c.MesId,
                                                    }).FirstOrDefault()
                                         },
                                         OfertaEducativaId = p.OfertaEducativaId,
                                         OfertaEducativa = new DTOOfertaEducativa
                                         {
                                             OfertaEducativaId = p.OfertaEducativa.OfertaEducativaId,
                                             OfertaEducativaTipoId = p.OfertaEducativa.OfertaEducativaTipoId,
                                             Descripcion = p.OfertaEducativa.Descripcion,
                                             DTOOfertaEducativaTipo = new DTOOfertaEducativaTipo
                                             {
                                                 Descripcion = p.OfertaEducativa.OfertaEducativaTipo.Descripcion,
                                                 OfertaEducativaTipoId = p.OfertaEducativa.OfertaEducativaTipo.OfertaEducativaTipoId
                                             }
                                         },
                                         FechaGeneracion = p.FechaGeneracion,
                                         CuotaId = p.CuotaId,
                                         Cuota = p.Cuota,
                                         DTOCuota = new DTOCuota
                                         {
                                             CuotaId = p.Cuota1.CuotaId,
                                             Anio = p.Cuota1.Anio,
                                             PeriodoId = p.Cuota1.PeriodoId,
                                             OfertaEducativaId = p.Cuota1.OfertaEducativaId,
                                             PagoConceptoId = p.Cuota1.PagoConceptoId,
                                             Monto = p.Cuota1.Monto,
                                             //EsEmpresa = p.Cuota1.EsEmpresa,
                                             DTOPagoConcepto = new DTOPagoConcepto
                                             {
                                                 //CuentaContable = p.Cuota1.PagoConcepto.CuentaContable,
                                                 Descripcion = p.Cuota1.PagoConcepto.Descripcion,
                                                 EstatusId = p.Cuota1.PagoConcepto.EstatusId,
                                                 //OfertaEducativaId = p.Cuota1.PagoConcepto.OfertaEducativaId,
                                                 PagoConceptoId = p.Cuota1.PagoConcepto.PagoConceptoId
                                             }
                                         },
                                         Promesa = p.Promesa,
                                         Referencia = p.ReferenciaId,
                                         EstatusId = p.EstatusId,
                                         lstPagoDescuento = (from pd in db.PagoDescuento
                                                             join ad in db.AlumnoDescuento on pd.DescuentoId equals ad.DescuentoId
                                                             where pd.PagoId == p.PagoId && ad.AlumnoId == AlumnoId && p.Anio == ad.Anio && p.PeriodoId == ad.PeriodoId && ad.EstatusId != 3
                                                             select new DTOPagoDescuento
                                                             {
                                                                 DescuentoId = pd.DescuentoId,
                                                                 Monto = pd.Monto,
                                                                 PagoId = pd.PagoId,
                                                                 DTOAlumnDes = new DTOAlumnoDescuento
                                                                 {
                                                                     AlumnoId = ad.AlumnoId,
                                                                     Anio = ad.Anio,
                                                                     ConceptoId = ad.PagoConceptoId,
                                                                     DescuentoId = ad.DescuentoId,
                                                                     EstatusId = ad.EstatusId,
                                                                     Monto = objAlumno.EsEmpresa == true ? 0 : ad.Monto,
                                                                     SMonto = objAlumno.EsEmpresa == true ? "0%" : ad.Monto.ToString() + "%",
                                                                     OfertaEducativaId = ad.OfertaEducativaId,
                                                                     PeriodoId = ad.PeriodoId
                                                                 }
                                                             }).ToList()
                                         //DTOReferenciaBitacora = new DTOAlumnoReferenciaBitacora
                                         //{
                                         //    AlumnoId=p.AlumnoReferenciaBitacora.
                                         //}
                                     }).ToList());
                    }

                    lstPagos.ForEach(delegate (DTOPagos objP)
                    {
                        if (objP.Anio == 2016 && objP.PeriodoId == 1)
                        {
                            objP.DTOCuota.PeridoAnio = "";
                            CuotaAnterior = null;
                        }
                        else
                        {
                            objP.DTOCuota.PeridoAnio = objP.DTOCuota.Anio.ToString() + "-" + objP.DTOCuota.PeriodoId.ToString();

                            int Anio, Periodo, Oferta, Concepto;

                            Anio = (objP.DTOPeriodo.PeriodoId == 4 ? objP.DTOPeriodo.Anio - 1 : objP.DTOPeriodo.Anio);

                            Periodo = (objP.DTOPeriodo.PeriodoId == 1 ? 4 : objP.DTOPeriodo.PeriodoId - 1);

                            Oferta = objP.OfertaEducativaId;

                            Concepto = objP.DTOCuota.PagoConceptoId;

                            CuotaAnterior = (from a in db.Cuota
                                             where a.Anio == Anio && a.PeriodoId == Periodo
                                             && a.OfertaEducativaId == Oferta && a.PagoConceptoId == Concepto
                                             select new DTOCuota
                                             {
                                                 CuotaId = a.CuotaId,
                                                 Anio = a.Anio,
                                                 PeriodoId = a.PeriodoId,
                                                 OfertaEducativaId = a.OfertaEducativaId,
                                                 PagoConceptoId = a.PagoConceptoId,
                                                 Monto = a.Monto,
                                                 EsEmpresa = a.EsEmpresa
                                             }).FirstOrDefault();
                        }
                        if (objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800)
                        {

                            objP.DTOCuota.DTOPagoConcepto.Descripcion += ", " + objP.DTOSubPeriodo.Mes.Descripcion + " " + objP.DTOPeriodo.FechaInicial.Year.ToString();
                        }

                        if (objP.PagoRecargo != null)
                        {
                            //objP.DTOCuota.DTOPagoConcepto.Descripcion += ", " + lstPagos.Where(LP => LP.PagoId == objP.PagoRecargo.PagoId).FirstOrDefault().DTOCuota.DTOPagoConcepto.Descripcion;
                            objP.DTOCuota.DTOPagoConcepto.Descripcion += ", " + db.Pago.Where(P => P.PagoId == objP.PagoRecargo.PagoId).FirstOrDefault().Cuota1.PagoConcepto.Descripcion;
                        }

                        if (objP.lstPagoDescuento.Count == 0)
                        {
                            objP.lstPagoDescuento = new List<DTOPagoDescuento>{
                        new DTOPagoDescuento{
                            DTOAlumnDes= new DTOAlumnoDescuento{
                                Monto=0,
                                SMonto=objP.Anio == 2016 && objP.PeriodoId == 1? "":"0%"
                            }
                        }
                    };
                        }
                        decimal PTotal = objP.DTOParcial.Select(P => P.Pago).Sum();
                        decimal RTotal = db.ReferenciadoCabecero.Where(RC => RC.PagoId == objP.PagoId).ToList().Select(RC => RC.ImporteTotal).Sum();

                        objP.Restante = objP.DTOParcial.Count > 0 || RTotal > 0 ?
                            (objP.Promesa - (PTotal + RTotal)).ToString("C", Cultura)
                            : objP.Promesa.ToString("C", Cultura);

                        Anticipado = (objP.DTOPeriodo.FechaInicial.Day.ToString().Length < 2 ? "0" + objP.DTOPeriodo.FechaInicial.Day.ToString() : objP.DTOPeriodo.FechaInicial.Day.ToString())
                            + "/" + (objP.DTOSubPeriodo.MesId.ToString().Length < 2 ? "0" + objP.DTOSubPeriodo.MesId.ToString() : objP.DTOSubPeriodo.MesId.ToString())
                            + "/" + (objP.DTOPeriodo.FechaInicial.Year.ToString().Length < 2 ? "0" + objP.DTOPeriodo.FechaInicial.Year.ToString() : objP.DTOPeriodo.FechaInicial.Year.ToString());
                        FechaS = DateTime.ParseExact(Anticipado, "dd/MM/yyyy", Cultura).AddDays(-1).ToString("dd/MM/yyyy", Cultura).ToString();
                        //FechaS = "15" + FechaS.Substring(2, 8);
                        //objP.objNormal = new Pagos_Detalles
                        //{
                        //    FechaLimite = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 && lstPagosGenerados.Find(A => A.PagoId == objP.PagoId) != null ?
                        //    objP.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura) :
                        //    Fecha.Prorroga(objP.DTOPeriodo.FechaFinal.Year, int.Parse(Anticipado.Substring(3, 2)), true, 5).ToString("dd/MM/yyyy", Cultura) : "",
                        //    //FechaLimite = "",
                        //    Monto = objP.PagoId > idPago ? objP.Promesa.ToString("C", Cultura) : "",
                        //    Recargo = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                        //    (objP.Promesa * decimal.Parse("0.05")).ToString("C", Cultura) : "",
                        //    Total = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                        //    (objP.Promesa + (objP.Promesa * decimal.Parse("0.05"))).ToString("C", Cultura) : ""
                        //};
                        if (objP.PeriodoAnticipadoId == 0)
                        {

                            objP.objNormal = new Pagos_Detalles
                            {
                                FechaLimite = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 &&
                                objP.DTOCuota.PagoConceptoId != 807 && lstPagosGenerados.Find(A => A.PagoId == objP.PagoId) != null ?
                                objP.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura) :
                                Fecha.Prorroga(objP.DTOPeriodo.FechaFinal.Year, int.Parse(Anticipado.Substring(3, 2)), true, 5).ToString("dd/MM/yyyy", Cultura) : "",
                                //FechaLimite = "",
                                Monto = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.Promesa.ToString("C", Cultura) : "",
                                Recargo = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 &&
                                objP.DTOCuota.PagoConceptoId != 807 ? "" :
                                (objP.Promesa * decimal.Parse("0.05")).ToString("C", Cultura) : "",
                                Total = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 &&
                                objP.DTOCuota.PagoConceptoId != 807 ? "" :
                                (objP.Promesa + (objP.Promesa * decimal.Parse("0.05"))).ToString("C", Cultura) : ""
                            };
                            objP.objAnticipado2 = new Pagos_Detalles
                            {
                                Monto = "",
                                FechaLimite = ""
                            };
                            objP.objAnticipado1 = new Pagos_Detalles
                            {
                                Monto = "",
                                FechaLimite = ""
                            };
                        }
                        else if (objP.PeriodoAnticipadoId == 1)
                        {
                            objP.objAnticipado1 = new Pagos_Detalles
                            {
                                FechaLimite = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.
                                PagoConceptoId != 807 && lstPagosGenerados.Find(A => A.PagoId == objP.PagoId) != null ?
                                objP.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura) :
                                Fecha.Prorroga(objP.DTOPeriodo.FechaFinal.Year, int.Parse(Anticipado.Substring(3, 2)), true, 5).ToString("dd/MM/yyyy", Cultura) : "",
                                //FechaLimite = "",
                                Monto = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.Promesa.ToString("C", Cultura) : "0"
                                //Recargo = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                                //(objP.Promesa * decimal.Parse("0.05")).ToString("C", Cultura) : "",
                                //Total = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                                //(objP.Promesa + (objP.Promesa * decimal.Parse("0.05"))).ToString("C", Cultura) : ""
                            };
                            objP.objAnticipado2 = new Pagos_Detalles
                            {
                                Monto = "",
                                FechaLimite = ""
                            };
                            objP.objNormal = new Pagos_Detalles
                            {
                                Monto = "",
                                FechaLimite = ""
                            };
                        }
                        else if (objP.PeriodoAnticipadoId == 2)
                        {
                            objP.objAnticipado2 = new Pagos_Detalles
                            {
                                FechaLimite = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 &&
                                objP.DTOCuota.PagoConceptoId != 807 && lstPagosGenerados.Find(A => A.PagoId == objP.PagoId) != null ?
                                objP.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura) :
                                Fecha.Prorroga(objP.DTOPeriodo.FechaFinal.Year, int.Parse(Anticipado.Substring(3, 2)), true, 5).ToString("dd/MM/yyyy", Cultura) : "",
                                //FechaLimite = "",
                                Monto = objP.Anio != 2016 && objP.PeriodoId != 1 ? objP.Promesa.ToString("C", Cultura) : "0"
                                //    Recargo = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                                //    (objP.Promesa * decimal.Parse("0.05")).ToString("C", Cultura) : "",
                                //    Total = objP.PagoId > idPago ? objP.DTOCuota.PagoConceptoId != 800 && objP.DTOCuota.PagoConceptoId != 802 && objP.DTOCuota.PagoConceptoId != 807 ? "" :
                                //    (objP.Promesa + (objP.Promesa * decimal.Parse("0.05"))).ToString("C", Cultura) : ""
                            };
                            objP.objAnticipado1 = new Pagos_Detalles
                            {
                                Monto = "",
                                FechaLimite = ""
                            };
                            objP.objNormal = new Pagos_Detalles
                            {
                                Monto = "",
                                FechaLimite = ""
                            };
                        }
                        //objP.objAnticipado2 = new Pagos_Detalles
                        //{
                        //    Monto = "",
                        //    //Monto = FechaActual > DateTime.ParseExact(Anticipado, "dd/MM/yyyy", Cultura) ? "" : objP.OfertaEducativa.DTOOfertaEducativaTipo.OfertaEducativaTipoId != 4 ?
                        //    //objP.PagoId > idPago ? objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800 ? (CuotaAnterior.Monto.ToString("C", Cultura)) :
                        //    //objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 802 ? (CuotaAnterior.Monto.ToString("C", Cultura)) : "" : "" : "",
                        //    FechaLimite = ""
                        //    // FechaLimite = FechaActual > DateTime.ParseExact(Anticipado, "dd/MM/yyyy", Cultura) ? "" : objP.OfertaEducativa.DTOOfertaEducativaTipo.OfertaEducativaTipoId != 4 ?
                        //    // objP.PagoId > idPago ? objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800 ?
                        //    //DateTime.ParseExact(Anticipado, "dd/MM/yyyy", Cultura).AddDays(-1).ToString("dd/MM/yyyy", Cultura) :
                        //    //objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 802 ?
                        //    //DateTime.ParseExact(Anticipado, "dd/MM/yyyy", Cultura).AddDays(-1).ToString("dd/MM/yyyy", Cultura) :
                        //    //"" : "" : ""
                        //    // FechaLimite = (objP.DTOPeriodo.FechaInicial.AddDays(-1)).ToShortDateString()
                        //};
                        //objP.objAnticipado1 = new Pagos_Detalles
                        //    {
                        //        Monto = "",
                        //        //Monto = FechaActual > DateTime.ParseExact(FechaS, "dd/MM/yyyy", Cultura) ? "" : objP.OfertaEducativa.DTOOfertaEducativaTipo.OfertaEducativaTipoId != 4 ? objP.objAnticipado2.Monto.Length > 0 ?
                        //        //objP.PagoId > idPago ? objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800 ?
                        //        //(Math.Round(CuotaAnterior.Monto * decimal.Parse("0.96"))).ToString() :
                        //        //objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 802 ? (Math.Round(CuotaAnterior.Monto * decimal.Parse("0.96"))).ToString() : "" : "" : "" : "",
                        //        FechaLimite = ""
                        //        //FechaLimite = FechaActual > DateTime.ParseExact(FechaS, "dd/MM/yyyy", Cultura) ? "" : objP.OfertaEducativa.DTOOfertaEducativaTipo.OfertaEducativaTipoId != 4 ?
                        //        //objP.PagoId > idPago ? objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800 ?
                        //        //DateTime.ParseExact(FechaS, "dd/MM/yyyy", Cultura).ToString("dd/MM/yyyy", Cultura) :
                        //        //objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 802 ?
                        //        //DateTime.ParseExact(FechaS, "dd/MM/yyyy", Cultura).ToString("dd/MM/yyyy", Cultura) :
                        //        //"" : "" : ""

                        //        // FechaLimite = (objP.DTOPeriodo.FechaInicial.AddDays(-1)).ToShortDateString()
                        //    };
                        //if (objP.objAnticipado1.Monto != "")
                        //{
                        //    objP.objAnticipado1.Monto = int.Parse(objP.objAnticipado1.Monto.Substring(objP.objAnticipado1.Monto.Length - 1, 1)) > 5 ?
                        //        ((decimal.Parse(objP.objAnticipado1.Monto) + 10) - int.Parse(objP.objAnticipado1.Monto.Substring(objP.objAnticipado1.Monto.Length - 1, 1))).ToString("C", Cultura) :
                        //        int.Parse(objP.objAnticipado1.Monto.Substring(objP.objAnticipado1.Monto.Length - 1, 1)) == 5 ? decimal.Parse(objP.objAnticipado1.Monto).ToString("C", Cultura) :
                        //        ((decimal.Parse(objP.objAnticipado1.Monto)) - int.Parse(objP.objAnticipado1.Monto.Substring(objP.objAnticipado1.Monto.Length - 1, 1))).ToString("C", Cultura);
                        //}
                        //objP.objRetrasado = new Pagos_Detalles
                        //{
                        //    FechaLimite = objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800 ? DateTime.ParseExact(objP.objNormal.FechaLimite, "dd/MM/yyyy", Cultura).AddDays(1).ToString("dd/MM/yyyy", Cultura) : "",
                        //    Monto = objP.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800 ? (objP.Cuota + ((objP.Cuota * 5) / 100)).ToString() : "",
                        //};



                    });
                    lstPagos = lstPagos.OrderByDescending(l => l.PagoId).ToList();
                    return lstPagos;
                }
                catch (Exception e)
                {
                    return new List<DTOPagos>() {new DTOPagos{
                        objNormal= new Pagos_Detalles{
                            Observaciones=e.Message
}
                    }};
                }
            }
        }
        public static List<DTOPagos> ConsultarReferenciasConceptos(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    //int idPago = 2588;
                    DTOCuota CuotaAnterior;
                    DateTime FechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    FechaActual = FechaActual.AddDays(-7);
                    List<DTOPagos> Pagos = (from p in db.Pago
                                            where p.AlumnoId == AlumnoId
                                                     && p.EstatusId == 1
                                                     && (p.Cuota1.PagoConceptoId != 800 && p.Cuota1.PagoConceptoId != 802)
                                            select new DTOPagos
                                            {
                                                PagoId = p.PagoId,
                                                AlumnoId = p.AlumnoId,
                                                Anio = p.Anio,
                                                PeriodoId = p.PeriodoId,
                                                DTOPeriodo = new DTOPeriodo
                                                {
                                                    PeriodoId = p.Periodo.PeriodoId,
                                                    Anio = p.Periodo.Anio,
                                                    Descripcion = p.Periodo.Descripcion,
                                                    FechaInicial = p.Periodo.FechaInicial,
                                                    FechaFinal = p.Periodo.FechaFinal,
                                                    Meses = p.Periodo.Meses
                                                },
                                                SubperiodoId = p.SubperiodoId,
                                                DTOSubPeriodo = new DTOSubPeriodo
                                                {
                                                    SubperiodoId = p.Subperiodo.SubperiodoId,
                                                    PeriodoId = p.Subperiodo.PeriodoId,
                                                    MesId = p.Subperiodo.MesId,
                                                    Mes = (from c in db.Mes
                                                           where c.MesId == p.Subperiodo.MesId
                                                           select new DTOMes
                                                           {
                                                               Descripcion = c.Descripcion,
                                                               MesId = c.MesId,
                                                           }).FirstOrDefault()
                                                },
                                                OfertaEducativaId = p.OfertaEducativaId,
                                                OfertaEducativa = new DTOOfertaEducativa
                                                {
                                                    OfertaEducativaId = p.OfertaEducativa.OfertaEducativaId,
                                                    OfertaEducativaTipoId = p.OfertaEducativa.OfertaEducativaTipoId,
                                                    Descripcion = p.OfertaEducativa.Descripcion,
                                                    DTOOfertaEducativaTipo = new DTOOfertaEducativaTipo
                                                    {
                                                        Descripcion = p.OfertaEducativa.OfertaEducativaTipo.Descripcion,
                                                        OfertaEducativaTipoId = p.OfertaEducativa.OfertaEducativaTipo.OfertaEducativaTipoId
                                                    }
                                                },
                                                FechaGeneracion = p.FechaGeneracion,
                                                CuotaId = p.CuotaId,
                                                Cuota = p.Cuota,
                                                DTOCuota = new DTOCuota
                                                {
                                                    CuotaId = p.Cuota1.CuotaId,
                                                    Anio = p.Cuota1.Anio,
                                                    PeriodoId = p.Cuota1.PeriodoId,
                                                    OfertaEducativaId = p.Cuota1.OfertaEducativaId,
                                                    PagoConceptoId = p.Cuota1.PagoConceptoId,
                                                    Monto = p.Cuota1.Monto,
                                                    EsEmpresa = p.Cuota1.EsEmpresa,
                                                    DTOPagoConcepto = new DTOPagoConcepto
                                                    {
                                                        CuentaContable = p.Cuota1.PagoConcepto.CuentaContable,
                                                        Descripcion = p.Cuota1.PagoConcepto.Descripcion,
                                                        EstatusId = p.Cuota1.PagoConcepto.EstatusId,
                                                        OfertaEducativaId = p.Cuota1.PagoConcepto.OfertaEducativaId,
                                                        PagoConceptoId = p.Cuota1.PagoConcepto.PagoConceptoId
                                                    }
                                                },
                                                Promesa = p.Promesa,
                                                Referencia = p.ReferenciaId,
                                                EstatusId = p.EstatusId
                                            }).ToList();

                    Pagos.ForEach(Pago =>
                    {
                        Pago.objNormal = new Pagos_Detalles
                        {
                            FechaLimite = Pago.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura),
                            Monto = Pago.Promesa.ToString("C", Cultura)
                        };
                        if (Pago.Anio == 2016 && Pago.PeriodoId == 1)
                        {
                            Pago.DTOCuota.PeridoAnio = "";
                            CuotaAnterior = null;
                        }
                        else
                        {
                            Pago.DTOCuota.PeridoAnio = Pago.DTOCuota.Anio.ToString() + "-" + Pago.DTOCuota.PeriodoId.ToString();
                            int Anio, Periodo, Oferta, Concepto;
                            Anio = (Pago.DTOPeriodo.PeriodoId == 4 ? Pago.DTOPeriodo.Anio - 1 : Pago.DTOPeriodo.Anio);
                            Periodo = (Pago.DTOPeriodo.PeriodoId == 1 ? 4 : Pago.DTOPeriodo.PeriodoId - 1);
                            Oferta = Pago.OfertaEducativaId;
                            Concepto = Pago.DTOCuota.PagoConceptoId;
                            CuotaAnterior = (from a in db.Cuota
                                             where a.Anio == Anio && a.PeriodoId == Periodo
                                             && a.OfertaEducativaId == Oferta && a.PagoConceptoId == Concepto
                                             select new DTOCuota
                                             {
                                                 CuotaId = a.CuotaId,
                                                 Anio = a.Anio,
                                                 PeriodoId = a.PeriodoId,
                                                 OfertaEducativaId = a.OfertaEducativaId,
                                                 PagoConceptoId = a.PagoConceptoId,
                                                 Monto = a.Monto,
                                                 EsEmpresa = a.EsEmpresa
                                             }).FirstOrDefault();
                        }
                        if (Pago.DTOCuota.DTOPagoConcepto.PagoConceptoId == 800)
                        {
                            Pago.DTOCuota.DTOPagoConcepto.Descripcion += " " + Pago.DTOSubPeriodo.Mes.Descripcion + " " +
                                Pago.DTOPeriodo.FechaInicial.Year.ToString();
                        }
                        Pago.lstPagoDescuento = (from pd in db.PagoDescuento
                                                 join ad in db.AlumnoDescuento on pd.DescuentoId equals ad.DescuentoId
                                                 where pd.PagoId == Pago.PagoId && ad.AlumnoId == AlumnoId
                                                 select new DTOPagoDescuento
                                                 {
                                                     DescuentoId = pd.DescuentoId,
                                                     Monto = pd.Monto,
                                                     PagoId = pd.PagoId,
                                                     DTOAlumnDes = new DTOAlumnoDescuento
                                                     {
                                                         AlumnoId = ad.AlumnoId,
                                                         Anio = ad.Anio,
                                                         ConceptoId = ad.PagoConceptoId,
                                                         DescuentoId = ad.DescuentoId,
                                                         EstatusId = ad.EstatusId,
                                                         Monto = ad.Monto,
                                                         SMonto = ad.Monto.ToString() + "%",
                                                         OfertaEducativaId = ad.OfertaEducativaId,
                                                         PeriodoId = ad.PeriodoId
                                                     }
                                                 }).ToList();
                        if (Pago.lstPagoDescuento.Count == 0)
                        {
                            Pago.lstPagoDescuento = new List<DTOPagoDescuento>{
                                new DTOPagoDescuento{
                                    DTOAlumnDes= new DTOAlumnoDescuento{
                                    Monto=0,
                                    SMonto=Pago.Anio == 2016 && Pago.PeriodoId == 1? "":"0%"
                                    }
                                }
                            };
                        }


                    });

                    return Pagos;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static DTOPagos GenerarPago(int AlumnoId, int OfertaEducativaId, int PagoConceptoId, int Cuotaid, int UsuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                try
                {
                    Usuario Usuario = db.Usuario.Where(u => u.UsuarioId == UsuarioId).FirstOrDefault();
                    DTOPagos PagoDTO;
                    DTOAlumnoInscrito AlumnoInscrito = (from a in db.AlumnoInscrito
                                                        where a.AlumnoId == AlumnoId && a.OfertaEducativaId == OfertaEducativaId
                                                        select new DTOAlumnoInscrito
                                                        {
                                                            AlumnoId = a.AlumnoId,
                                                            Anio = a.Anio,
                                                            EsEmpresa = a.EsEmpresa,
                                                            FechaInscripcion = a.FechaInscripcion,
                                                            OfertaEducativaId = a.OfertaEducativaId,
                                                            PagoPlanId = a.PagoPlanId,
                                                            PeriodoId = a.PeriodoId,
                                                            TurnoId = a.TurnoId,
                                                            UsuarioId = a.UsuarioId,
                                                            OfertaEducativa = new DTOOfertaEducativa
                                                            {
                                                                OfertaEducativaId = a.OfertaEducativa.OfertaEducativaId,
                                                                OfertaEducativaTipoId = a.OfertaEducativa.OfertaEducativaTipoId,
                                                                Descripcion = a.OfertaEducativa.Descripcion,
                                                                Rvoe = a.OfertaEducativa.Rvoe
                                                            }
                                                        }).FirstOrDefault();
                    DTOPeriodo PeriodoActual = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);

                    DTODescuentos Descuento = BLLDescuentos.obtenerDescuentos(OfertaEducativaId, PagoConceptoId);
                    DTOCuota Cuota = BLLCuota.TraerCuota(Cuotaid);
                    DTOAlumnoDescuento AlumnoInscritoDescuento = Descuento != null ? BLLAlumnoDescuento.TraerDescuentoAlumno(AlumnoId, OfertaEducativaId, PagoConceptoId, Descuento.DescuentoId, PeriodoActual.Anio, PeriodoActual.PeriodoId) : null;
                    //AlumnoInscritoDescuento = AlumnoInscritoDescuento.EstatusId == 2 ? null : AlumnoInscritoDescuento;
                    db.Pago.Add(new Pago
                    {
                        AlumnoId = AlumnoId,
                        Anio = PeriodoActual.Anio,
                        PeriodoId = PeriodoActual.PeriodoId,
                        SubperiodoId = PeriodoActual.SubPeriodoId,
                        OfertaEducativaId = AlumnoInscrito.OfertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = Cuota.CuotaId,
                        Cuota = Cuota.Monto,
                        Promesa = AlumnoInscritoDescuento != null ? Math.Round(Cuota.Monto - ((AlumnoInscritoDescuento.Monto / 100) * Cuota.Monto)) : Cuota.Monto,
                        Restante = AlumnoInscritoDescuento != null ? Math.Round(Cuota.Monto - ((AlumnoInscritoDescuento.Monto / 100) * Cuota.Monto)) : Cuota.Monto,
                        EstatusId = 1,
                        ReferenciaId = "",
                        UsuarioId = Usuario.UsuarioId,
                        UsuarioTipoId = Usuario.UsuarioTipoId,
                        HoraGeneracion = DateTime.Now.TimeOfDay

                    });
                    db.SaveChanges();
                    Pago PagoGenerado = db.Pago.Local[0];
                    PagoGenerado.ReferenciaId = db.spGeneraReferencia(PagoGenerado.PagoId).FirstOrDefault();

                    if (AlumnoInscritoDescuento != null)
                    {
                        db.PagoDescuento.Add(new PagoDescuento
                        {
                            DescuentoId = AlumnoInscritoDescuento.DescuentoId,
                            PagoId = db.Pago.Local[0].PagoId,
                            Monto = Math.Round((AlumnoInscritoDescuento.Monto / 100) * Cuota.Monto)
                        });
                        AlumnoDescuento objDes = db.AlumnoDescuento.Where(AD => AD.AlumnoDescuentoId == AlumnoInscritoDescuento.AlumnoDescuentoId).FirstOrDefault();
                        objDes.FechaAplicacion = DateTime.Now;
                        objDes.EstatusId = 2;
                    }

                    db.SaveChanges();
                    int pagoid = db.Pago.Local[0].PagoId;
                    PagoDTO = (from p in db.Pago
                               where p.PagoId == pagoid
                               select new DTOPagos
                               {
                                   PagoId = p.PagoId,
                                   AlumnoId = p.AlumnoId,
                                   Anio = p.Anio,
                                   Referencia = p.ReferenciaId,
                                   Promesa = p.Promesa,
                                   FechaGeneracion = p.FechaGeneracion,
                                   DTOCuota = new DTOCuota
                                   {
                                       CuotaId = p.Cuota1.CuotaId,
                                       Anio = p.Cuota1.Anio,
                                       PeriodoId = p.Cuota1.PeriodoId,
                                       OfertaEducativaId = p.Cuota1.OfertaEducativaId,
                                       PagoConceptoId = p.Cuota1.PagoConceptoId,
                                       Monto = p.Cuota1.Monto,
                                       EsEmpresa = p.Cuota1.EsEmpresa,
                                       DTOPagoConcepto = new DTOPagoConcepto
                                       {
                                           CuentaContable = p.Cuota1.PagoConcepto.CuentaContable,
                                           Descripcion = p.Cuota1.PagoConcepto.Descripcion,
                                           EstatusId = p.Cuota1.PagoConcepto.EstatusId,
                                           OfertaEducativaId = p.Cuota1.PagoConcepto.OfertaEducativaId,
                                           PagoConceptoId = p.Cuota1.PagoConcepto.PagoConceptoId
                                       }
                                   },
                               }).FirstOrDefault();
                    PagoDTO.objNormal = new Pagos_Detalles
                    {
                        FechaLimite = DateTime.ParseExact(PagoDTO.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura), "dd/MM/yyyy", Cultura).ToString("dd/MM/yyyy", Cultura),
                        Monto = PagoDTO.Promesa.ToString("C", Cultura)
                    };
                    PagoDTO.lstPagoDescuento = (from pd in db.PagoDescuento
                                                join ad in db.AlumnoDescuento on pd.DescuentoId equals ad.DescuentoId
                                                where pd.PagoId == PagoDTO.PagoId && ad.AlumnoId == AlumnoId
                                                select new DTOPagoDescuento
                                                {
                                                    DescuentoId = pd.DescuentoId,
                                                    Monto = pd.Monto,
                                                    PagoId = pd.PagoId,
                                                    DTOAlumnDes = new DTOAlumnoDescuento
                                                    {
                                                        AlumnoId = ad.AlumnoId,
                                                        Anio = ad.Anio,
                                                        ConceptoId = ad.PagoConceptoId,
                                                        DescuentoId = ad.DescuentoId,
                                                        EstatusId = ad.EstatusId,
                                                        Monto = ad.Monto,
                                                        SMonto = ad.Monto.ToString() + "%",
                                                        OfertaEducativaId = ad.OfertaEducativaId,
                                                        PeriodoId = ad.PeriodoId
                                                    }
                                                }).ToList();
                    try
                    {
                        BLLAdeudoBitacora.GuardarAdeudo(new DTOAlumnoReferenciaBitacora
                        {
                            AlumnoId = AlumnoId,
                            OfertaEducativaId = OfertaEducativaId,
                            PagoConceptoId = PagoConceptoId,
                            Anio = PeriodoActual.Anio,
                            PeriodoId = PeriodoActual.PeriodoId,
                            PagoId = pagoid
                        });
                    }
                    catch
                    {

                    }
                    return PagoDTO;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static DTOPagos GenerarPagoC(int AlumnoId, int OfertaEducativaId, int PagoConceptoId, int Cuotaid, int UsuarioId, int Anio, int PeriodoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                try
                {
                    Usuario objUser = db.Usuario.Where(u => u.UsuarioId == UsuarioId).FirstOrDefault();
                    DTOPagos dtoPago;
                    DTOAlumnoInscrito objAlumno = (from a in db.AlumnoInscrito
                                                   where a.AlumnoId == AlumnoId && a.OfertaEducativaId == OfertaEducativaId
                                                   select new DTOAlumnoInscrito
                                                   {
                                                       AlumnoId = a.AlumnoId,
                                                       Anio = a.Anio,
                                                       EsEmpresa = a.EsEmpresa,
                                                       FechaInscripcion = a.FechaInscripcion,
                                                       OfertaEducativaId = a.OfertaEducativaId,
                                                       PagoPlanId = a.PagoPlanId,
                                                       PeriodoId = a.PeriodoId,
                                                       TurnoId = a.TurnoId,
                                                       UsuarioId = a.UsuarioId,
                                                       OfertaEducativa = new DTOOfertaEducativa
                                                       {
                                                           OfertaEducativaId = a.OfertaEducativa.OfertaEducativaId,
                                                           OfertaEducativaTipoId = a.OfertaEducativa.OfertaEducativaTipoId,
                                                           Descripcion = a.OfertaEducativa.Descripcion,
                                                           Rvoe = a.OfertaEducativa.Rvoe
                                                       }
                                                   }).FirstOrDefault();

                    DTOPeriodo objPeriodo = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);

                    string per1 = objPeriodo.Anio.ToString() + objPeriodo.PeriodoId.ToString();
                    string per2 = Anio.ToString() + PeriodoId.ToString();

                    if (per1 != per2)
                    {
                        objPeriodo.Anio = Anio;
                        objPeriodo.PeriodoId = PeriodoId;
                        objPeriodo.SubPeriodoId = 1;
                    }

                    DTODescuentos objDescuentoConcepto = BLLDescuentos.obtenerDescuentos(OfertaEducativaId, PagoConceptoId);
                    DTOCuota objCuota = BLLCuota.TraerCuota(Cuotaid);
                    DTOAlumnoDescuento objAlumnoDescuento = objDescuentoConcepto != null ? BLLAlumnoDescuento.TraerDescuentoAlumno(AlumnoId, OfertaEducativaId, PagoConceptoId, objDescuentoConcepto.DescuentoId, objPeriodo.Anio, objPeriodo.PeriodoId) : null;
                    //objAlumnoDescuento = objAlumnoDescuento.EstatusId == 2 ? null : objAlumnoDescuento;
                    db.Pago.Add(new Pago
                    {
                        AlumnoId = AlumnoId,
                        Anio = objPeriodo.Anio,
                        PeriodoId = objPeriodo.PeriodoId,
                        SubperiodoId = objPeriodo.SubPeriodoId,
                        OfertaEducativaId = objAlumno.OfertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = objCuota.CuotaId,
                        Cuota = objCuota.Monto,
                        Promesa = objAlumnoDescuento != null ? Math.Round(objCuota.Monto - ((objAlumnoDescuento.Monto / 100) * objCuota.Monto)) : objCuota.Monto,
                        Restante = objAlumnoDescuento != null ? Math.Round(objCuota.Monto - ((objAlumnoDescuento.Monto / 100) * objCuota.Monto)) : objCuota.Monto,
                        EstatusId = 1,
                        ReferenciaId = "",
                        UsuarioId = objUser.UsuarioId,
                        UsuarioTipoId = objUser.UsuarioTipoId,
                        HoraGeneracion = DateTime.Now.TimeOfDay

                    });
                    db.SaveChanges();
                    Pago objPago = db.Pago.Local[0];
                    objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();

                    if (objAlumnoDescuento != null)
                    {
                        db.PagoDescuento.Add(new PagoDescuento
                        {
                            DescuentoId = objAlumnoDescuento.DescuentoId,
                            PagoId = db.Pago.Local[0].PagoId,
                            Monto = Math.Round((objAlumnoDescuento.Monto / 100) * objCuota.Monto)
                        });
                        AlumnoDescuento objDes = db.AlumnoDescuento.Where(AD => AD.AlumnoDescuentoId == objAlumnoDescuento.AlumnoDescuentoId).FirstOrDefault();
                        objDes.FechaAplicacion = DateTime.Now;
                        objDes.EstatusId = 2;
                    }

                    db.SaveChanges();
                    int pagoid = db.Pago.Local[0].PagoId;
                    dtoPago = (from p in db.Pago
                               where p.PagoId == pagoid
                               select new DTOPagos
                               {
                                   PagoId = p.PagoId,
                                   AlumnoId = p.AlumnoId,
                                   Anio = p.Anio,
                                   Referencia = p.ReferenciaId,
                                   Promesa = p.Promesa,
                                   FechaGeneracion = p.FechaGeneracion,
                                   DTOCuota = new DTOCuota
                                   {
                                       CuotaId = p.Cuota1.CuotaId,
                                       Anio = p.Cuota1.Anio,
                                       PeriodoId = p.Cuota1.PeriodoId,
                                       OfertaEducativaId = p.Cuota1.OfertaEducativaId,
                                       PagoConceptoId = p.Cuota1.PagoConceptoId,
                                       Monto = p.Cuota1.Monto,
                                       EsEmpresa = p.Cuota1.EsEmpresa,
                                       DTOPagoConcepto = new DTOPagoConcepto
                                       {
                                           CuentaContable = p.Cuota1.PagoConcepto.CuentaContable,
                                           Descripcion = p.Cuota1.PagoConcepto.Descripcion,
                                           EstatusId = p.Cuota1.PagoConcepto.EstatusId,
                                           OfertaEducativaId = p.Cuota1.PagoConcepto.OfertaEducativaId,
                                           PagoConceptoId = p.Cuota1.PagoConcepto.PagoConceptoId
                                       }
                                   },
                               }).FirstOrDefault();
                    dtoPago.objNormal = new Pagos_Detalles
                    {
                        FechaLimite = DateTime.ParseExact(dtoPago.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura), "dd/MM/yyyy", Cultura).ToString("dd/MM/yyyy", Cultura),
                        Monto = dtoPago.Promesa.ToString("C", Cultura)
                    };
                    dtoPago.lstPagoDescuento = (from pd in db.PagoDescuento
                                                join ad in db.AlumnoDescuento on pd.DescuentoId equals ad.DescuentoId
                                                where pd.PagoId == dtoPago.PagoId && ad.AlumnoId == AlumnoId
                                                select new DTOPagoDescuento
                                                {
                                                    DescuentoId = pd.DescuentoId,
                                                    Monto = pd.Monto,
                                                    PagoId = pd.PagoId,
                                                    DTOAlumnDes = new DTOAlumnoDescuento
                                                    {
                                                        AlumnoId = ad.AlumnoId,
                                                        Anio = ad.Anio,
                                                        ConceptoId = ad.PagoConceptoId,
                                                        DescuentoId = ad.DescuentoId,
                                                        EstatusId = ad.EstatusId,
                                                        Monto = ad.Monto,
                                                        SMonto = ad.Monto.ToString() + "%",
                                                        OfertaEducativaId = ad.OfertaEducativaId,
                                                        PeriodoId = ad.PeriodoId
                                                    }
                                                }).ToList();
                    try
                    {
                        BLLAdeudoBitacora.GuardarAdeudo(new DTOAlumnoReferenciaBitacora
                        {
                            AlumnoId = AlumnoId,
                            OfertaEducativaId = OfertaEducativaId,
                            PagoConceptoId = PagoConceptoId,
                            Anio = objPeriodo.Anio,
                            PeriodoId = objPeriodo.PeriodoId,
                            PagoId = pagoid
                        });
                    }
                    catch
                    {

                    }
                    return dtoPago;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static DTOPagos GenerarPago(int AlumnoId, int OfertaEducativaId, int PagoConceptoId, int Cuotaid)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                try
                {
                    DTOPagos PagoNuevo;
                    DTOAlumnoInscrito Alumno = (from a in db.AlumnoInscrito
                                                where a.AlumnoId == AlumnoId && a.OfertaEducativaId == OfertaEducativaId
                                                select new DTOAlumnoInscrito
                                                {
                                                    AlumnoId = a.AlumnoId,
                                                    Anio = a.Anio,
                                                    EsEmpresa = a.EsEmpresa,
                                                    FechaInscripcion = a.FechaInscripcion,
                                                    OfertaEducativaId = a.OfertaEducativaId,
                                                    PagoPlanId = a.PagoPlanId,
                                                    PeriodoId = a.PeriodoId,
                                                    TurnoId = a.TurnoId,
                                                    UsuarioId = a.UsuarioId,
                                                    OfertaEducativa = new DTOOfertaEducativa
                                                    {
                                                        OfertaEducativaId = a.OfertaEducativa.OfertaEducativaId,
                                                        OfertaEducativaTipoId = a.OfertaEducativa.OfertaEducativaTipoId,
                                                        Descripcion = a.OfertaEducativa.Descripcion,
                                                        Rvoe = a.OfertaEducativa.Rvoe
                                                    }
                                                }).FirstOrDefault();
                    DTOPeriodo PeriodoActual = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);

                    DTODescuentos Descuento = BLLDescuentos.obtenerDescuentos(OfertaEducativaId, PagoConceptoId);
                    DTOCuota Cuota = BLLCuota.TraerCuota(Cuotaid);
                    DTOAlumnoDescuento AlumnoDescuento = Descuento != null ? BLLAlumnoDescuento.TraerDescuentoAlumno(AlumnoId, OfertaEducativaId, PagoConceptoId, Descuento.DescuentoId, PeriodoActual.Anio, PeriodoActual.PeriodoId) : null;
                    //AlumnoDescuento = AlumnoDescuento.EstatusId == 2 ? null : AlumnoDescuento;
                    db.Pago.Add(new Pago
                    {
                        AlumnoId = AlumnoId,
                        Anio = PeriodoActual.Anio,
                        PeriodoId = PeriodoActual.PeriodoId,
                        SubperiodoId = PeriodoActual.SubPeriodoId,
                        OfertaEducativaId = Alumno.OfertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = Cuota.CuotaId,
                        Cuota = Cuota.Monto,
                        Promesa = AlumnoDescuento != null ? Math.Round(Cuota.Monto - ((AlumnoDescuento.Monto / 100) * Cuota.Monto)) : Cuota.Monto,
                        Restante = AlumnoDescuento != null ? Math.Round(Cuota.Monto - ((AlumnoDescuento.Monto / 100) * Cuota.Monto)) : Cuota.Monto,
                        EstatusId = 1,
                        ReferenciaId = "",
                        UsuarioId = AlumnoId,
                        UsuarioTipoId = 2,
                        HoraGeneracion = DateTime.Now.TimeOfDay

                    });
                    db.SaveChanges();
                    Pago objPago = db.Pago.Local[0];
                    objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();

                    if (AlumnoDescuento != null)
                    {
                        db.PagoDescuento.Add(new PagoDescuento
                        {
                            DescuentoId = AlumnoDescuento.DescuentoId,
                            PagoId = db.Pago.Local[0].PagoId,
                            Monto = Math.Round((AlumnoDescuento.Monto / 100) * Cuota.Monto)
                        });
                        AlumnoDescuento DescuentoNuevo = db.AlumnoDescuento.Where(AD => AD.AlumnoDescuentoId == AlumnoDescuento.AlumnoDescuentoId).FirstOrDefault();
                        DescuentoNuevo.FechaAplicacion = DateTime.Now;
                        DescuentoNuevo.EstatusId = 2;
                    }

                    db.SaveChanges();
                    int pagoid = db.Pago.Local[0].PagoId;
                    PagoNuevo = (from p in db.Pago
                                 where p.PagoId == pagoid
                                 select new DTOPagos
                                 {
                                     PagoId = p.PagoId,
                                     AlumnoId = p.AlumnoId,
                                     Anio = p.Anio,
                                     Referencia = p.ReferenciaId,
                                     Promesa = p.Promesa,
                                     FechaGeneracion = p.FechaGeneracion,
                                     DTOCuota = new DTOCuota
                                     {
                                         CuotaId = p.Cuota1.CuotaId,
                                         Anio = p.Cuota1.Anio,
                                         PeriodoId = p.Cuota1.PeriodoId,
                                         OfertaEducativaId = p.Cuota1.OfertaEducativaId,
                                         PagoConceptoId = p.Cuota1.PagoConceptoId,
                                         Monto = p.Cuota1.Monto,
                                         EsEmpresa = p.Cuota1.EsEmpresa,
                                         DTOPagoConcepto = new DTOPagoConcepto
                                         {
                                             CuentaContable = p.Cuota1.PagoConcepto.CuentaContable,
                                             Descripcion = p.Cuota1.PagoConcepto.Descripcion,
                                             EstatusId = p.Cuota1.PagoConcepto.EstatusId,
                                             OfertaEducativaId = p.Cuota1.PagoConcepto.OfertaEducativaId,
                                             PagoConceptoId = p.Cuota1.PagoConcepto.PagoConceptoId
                                         }
                                     },
                                 }).FirstOrDefault();
                    PagoNuevo.objNormal = new Pagos_Detalles
                    {
                        FechaLimite = DateTime.ParseExact(PagoNuevo.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura), "dd/MM/yyyy", Cultura).ToString("dd/MM/yyyy", Cultura),
                        Monto = PagoNuevo.Promesa.ToString("C", Cultura)
                    };
                    PagoNuevo.lstPagoDescuento = (from pd in db.PagoDescuento
                                                  join ad in db.AlumnoDescuento on pd.DescuentoId equals ad.DescuentoId
                                                  where pd.PagoId == PagoNuevo.PagoId && ad.AlumnoId == AlumnoId
                                                  select new DTOPagoDescuento
                                                  {
                                                      DescuentoId = pd.DescuentoId,
                                                      Monto = pd.Monto,
                                                      PagoId = pd.PagoId,
                                                      DTOAlumnDes = new DTOAlumnoDescuento
                                                      {
                                                          AlumnoId = ad.AlumnoId,
                                                          Anio = ad.Anio,
                                                          ConceptoId = ad.PagoConceptoId,
                                                          DescuentoId = ad.DescuentoId,
                                                          EstatusId = ad.EstatusId,
                                                          Monto = ad.Monto,
                                                          SMonto = ad.Monto.ToString() + "%",
                                                          OfertaEducativaId = ad.OfertaEducativaId,
                                                          PeriodoId = ad.PeriodoId
                                                      }
                                                  }).ToList();
                    try
                    {
                        BLLAdeudoBitacora.GuardarAdeudo(new DTOAlumnoReferenciaBitacora
                        {
                            AlumnoId = AlumnoId,
                            OfertaEducativaId = OfertaEducativaId,
                            PagoConceptoId = PagoConceptoId,
                            Anio = PeriodoActual.Anio,
                            PeriodoId = PeriodoActual.PeriodoId,
                            PagoId = pagoid
                        });
                    }
                    catch
                    {

                    }
                    return PagoNuevo;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static string ConsultarAdeudo(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                int nPagos = 0;

                DateTime FechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                
                Periodo objPerActual = db.Periodo.Where(d =>FechaActual >= d.FechaInicial
                && FechaActual <= d.FechaFinal).FirstOrDefault();



                DateTime dtPeriodo;
                List<Pago> Pagolist = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.EstatusId == 1 && (P.Anio != 2016 || P.PeriodoId != 1) &&
                    (P.Cuota1.PagoConceptoId == 800 || P.Cuota1.PagoConceptoId == 802 || P.Cuota1.PagoConceptoId == 807 || P.Cuota1.PagoConceptoId == 306
                     || P.Cuota1.PagoConceptoId == 304 || P.Cuota1.PagoConceptoId == 320)).ToList();
                Pagolist.ForEach(delegate (Pago objP)
                {
                    dtPeriodo = BLLPeriodoPortal.TraerPeriodoCompletoS(objP.Anio, objP.PeriodoId, objP.SubperiodoId);
                    objP.FechaGeneracion = Fecha.Prorroga(dtPeriodo.Year, dtPeriodo.Month, true, 5);
                    nPagos += objP.FechaGeneracion < DateTime.Now ? 1 : 0;
                });
                nPagos = db.AlumnoPermitido.Where(P => P.AlumnoId == AlumnoId
                                                    && P.Anio == objPerActual.Anio
                                                    && P.PeriodoId == objPerActual.PeriodoId).ToList().Count > 0 ? 0 : nPagos;
                return nPagos > 0 ? "Debe" : "";
            }
        }
        public static List<DTOPagoParcial> ConsultarReferenciasPagadas(int Alumno)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<DTOPagoParcial> lstPParcial = (from PP in db.PagoParcial
                                                        where PP.Pago1.AlumnoId == Alumno // && (PP.Pago1.EstatusId == 4 || PP.Pago1.EstatusId == 14)
                                                        select new DTOPagoParcial
                                                        {
                                                            //Psgo original
                                                            PagoId = PP.PagoId,
                                                            SucursalCajaId = (int)PP.SucursalCajaId,
                                                            Pago = PP.Pago,
                                                            FechaPago = PP.FechaPago,

                                                            //PagoId de Recargo
                                                            Pago1 = new DTOPagos
                                                            {
                                                                EstatusId = PP.Pago1.EstatusId,
                                                                Referencia = PP.Pago1.ReferenciaId,
                                                                PagoId = PP.Pago1.PagoId,
                                                                PagoRecargo = (from pr in db.PagoRecargo
                                                                               where pr.PagoId == PP.PagoId
                                                                               select new DTOPagoRecargo
                                                                               {
                                                                                   Fecha = pr.Fecha,
                                                                                   PagoId = pr.PagoId,
                                                                                   PagoIdRecargo = pr.PagoIdRecargo
                                                                               }).FirstOrDefault(),
                                                                DTOCuota = new DTOCuota
                                                                {
                                                                    CuotaId = PP.Pago1.Cuota1.CuotaId,
                                                                    PagoConceptoId = PP.Pago1.Cuota1.PagoConceptoId,
                                                                    DTOPagoConcepto = new DTOPagoConcepto
                                                                    {
                                                                        Descripcion = PP.Pago1.Cuota1.PagoConcepto.Descripcion,
                                                                        EstatusId = PP.Pago1.Cuota1.PagoConcepto.EstatusId,
                                                                        OfertaEducativaId = PP.Pago1.Cuota1.PagoConcepto.OfertaEducativaId
                                                                    }
                                                                }
                                                            }
                                                        }).ToList();
                    List<ReferenciadoDetalle> lstReferenciado = new List<ReferenciadoDetalle>();

                    db.Pago.Where(P => P.AlumnoId == Alumno).ToList().ForEach(delegate (Pago obPago)
                    {
                        lstReferenciado.AddRange(db.ReferenciadoDetalle.Where(RD => RD.PagoId == obPago.PagoId).ToList());
                    });

                    lstReferenciado.ForEach(delegate (ReferenciadoDetalle objDetalle)
                    {
                        lstPParcial.Add(new DTOPagoParcial
                        {
                            PagoId = objDetalle.PagoId,
                            Pago = objDetalle.Importe,
                            FechaPago = objDetalle.FechaPago,
                            EsReferencia = true,
                            Pago1 = (from P in db.Pago
                                     where P.PagoId == objDetalle.PagoId
                                     select new DTOPagos
                                     {
                                         Referencia = P.ReferenciaId,
                                         DTOCuota = new DTOCuota
                                         {
                                             CuotaId = P.Cuota1.CuotaId,
                                             PagoConceptoId = P.Cuota1.PagoConceptoId,
                                             DTOPagoConcepto = new DTOPagoConcepto
                                             {
                                                 Descripcion = P.Cuota1.PagoConcepto.Descripcion,
                                                 EstatusId = P.Cuota1.PagoConcepto.EstatusId,
                                                 OfertaEducativaId = P.Cuota1.PagoConcepto.OfertaEducativaId
                                             }
                                         }
                                     }).FirstOrDefault()
                        });
                    });

                    lstPParcial.ForEach(delegate (DTOPagoParcial objPago)
                    {
                        objPago.PagoS = objPago.Pago.ToString("C", Cultura);
                        //if (objPago.Pago1.PagoRecargo != null)
                        //{
                        //    string NuevaDescripcio = (from a in db.Pago
                        //                              where a.PagoId == objPago.PagoId
                        //                              select a.Cuota1.PagoConcepto.Descripcion + " " + objPago.Pago1.).FirstOrDefault();

                        //    lstPParcial.Where(LP => LP.PagoId == objPago.Pago1.PagoRecargo.PagoIdRecargo).FirstOrDefault().Pago1.DTOCuota.DTOPagoConcepto.Descripcion =
                        //        lstPParcial.Where(LP => LP.PagoId == objPago.Pago1.PagoRecargo.PagoIdRecargo).FirstOrDefault().Pago1.DTOCuota.DTOPagoConcepto.Descripcion + " " + objPago.Pago1.DTOCuota.DTOPagoConcepto.Descripcion;
                        //}
                    });
                    lstPParcial = lstPParcial.OrderBy(PP => PP.FechaPago).ToList();
                    return lstPParcial;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public static void ActualizarReferencia(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<Pago> lstPago = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.ReferenciaId.Length < 2).ToList();
                lstPago.ForEach(delegate (Pago objPago)
                {
                    objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                });

                db.SaveChanges();

            }
        }
        public static string AplicarDescuento(int PagoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    Pago objPago = db.Pago.Where(P => P.PagoId == PagoId).FirstOrDefault();
                    Descuento objDesc = db.Descuento.Where(D => D.OfertaEducativaId == objPago.OfertaEducativaId && D.Descripcion == "Pago Anticipado" && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).FirstOrDefault();
                    string Monto = db.SistemaConfiguracion.Where(S => S.ParametroId == 4).FirstOrDefault().Valor;
                    decimal DMonto = decimal.Parse(Monto);
                    decimal Promesa = 100 - DMonto;
                    DMonto = DMonto / 100;
                    Promesa = Promesa / 100;
                    decimal Pagar = Math.Round(objPago.Promesa * DMonto);

                    objPago.Promesa = Math.Round(decimal.Parse((objPago.Promesa * decimal.Parse(Promesa.ToString())).ToString()));
                    objPago.EsAnticipado = true;
                    db.PagoDescuento.Add(new PagoDescuento
                    {
                        DescuentoId = objDesc.DescuentoId,
                        Monto = Pagar,
                        PagoId = objPago.PagoId
                    });

                    db.SaveChanges();
                    return "Guardado";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }

        }
        public static int GenerarRecargo(int PagoId, DateTime Fecha, int UsuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    string Monto = db.SistemaConfiguracion.Where(S => S.ParametroId == 3).FirstOrDefault().Valor;

                    decimal Promesa = decimal.Parse(Monto);
                    Promesa = Promesa / 100;
                    DateTime fHoy = new DateTime(Fecha.Year, Fecha.Month, Fecha.Day, 0, 0, 0, 0);

                    Periodo objPeriodo = db.Periodo.Where(P => fHoy >= P.FechaInicial && fHoy <= P.FechaFinal).FirstOrDefault();

                    Pago objPago = db.Pago.Where(P => P.PagoId == PagoId).FirstOrDefault();

                    Cuota objCuota = db.Cuota.Where(C => C.OfertaEducativaId == objPago.OfertaEducativaId
                        && C.PeriodoId == objPeriodo.PeriodoId && C.Anio == objPeriodo.Anio && C.PagoConceptoId == 306).FirstOrDefault();

                    Promesa = Math.Round(Promesa * objPago.Promesa);
                    db.Pago.Add(new Pago
                    {
                        ReferenciaId = "",
                        AlumnoId = objPago.AlumnoId,
                        Anio = objPeriodo.Anio,
                        PeriodoId = objPeriodo.PeriodoId,
                        SubperiodoId = 1,
                        OfertaEducativaId = objPago.OfertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = objCuota.CuotaId,
                        Cuota = Promesa,
                        Promesa = Promesa,
                        Restante = Promesa,
                        EstatusId = 1,
                        EsEmpresa = false,
                        EsAnticipado = false,
                        UsuarioId = UsuarioId,
                        UsuarioTipoId = 1,
                        HoraGeneracion = DateTime.Now.TimeOfDay
                    });

                    db.SaveChanges();
                    Pago objPagoRecargo = db.Pago.Local.Where(PL => PL.ReferenciaId == "").FirstOrDefault();

                    objPagoRecargo.ReferenciaId = db.spGeneraReferencia(objPagoRecargo.PagoId).FirstOrDefault();

                    db.PagoRecargo.Add(new PagoRecargo
                    {
                        PagoId = objPago.PagoId,
                        PagoIdRecargo = objPagoRecargo.PagoId,
                        Fecha = DateTime.Now
                    });

                    db.SaveChanges();
                    return objPagoRecargo.PagoId;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public static List<ReferenciasPagadas> ReferenciasPagadasC(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<Pago> lstPagos = db.Pago.Where(P => P.AlumnoId == AlumnoId).ToList();
                    List<ReferenciasPagadas> lstReferencias = new List<ReferenciasPagadas>();
                    List<ReferenciaProcesada> lstProcesadas;
                    List<PagoParcial> lstPagoParcial;

                    lstPagos.ForEach(delegate (Pago objPago)
                    {
                        lstProcesadas = db.ReferenciaProcesada.Where(RP => RP.ReferenciaId == objPago.ReferenciaId).ToList();
                        lstPagoParcial = objPago.PagoParcial.ToList();
                        if (lstProcesadas.Count > 0)
                        {
                            lstProcesadas.ForEach(delegate (ReferenciaProcesada objRef)
                            {
                                string fecha = objRef.FechaPago.ToString("dd/MM/yyyy", Cultura);
                                lstReferencias.Add(new ReferenciasPagadas
                                {
                                    Caja = "Scotiabank",
                                    FechaPago = fecha,
                                    ReferenciaId = objPago.ReferenciaId,
                                    MontoPagado = ((decimal)objRef.Importe).ToString("C", Cultura),
                                    MontoReferencia = (objPago.Promesa).ToString("C", Cultura),
                                    Saldo = (objPago.Promesa - ((decimal)objRef.Importe)).ToString("C", Cultura),
                                    SaldoD = (objPago.Promesa - ((decimal)objRef.Importe))
                                });
                                objPago.Promesa = (objPago.Promesa - objRef.Importe) < 0 ? 0 : decimal.Parse((objPago.Promesa - objRef.Importe).ToString());
                            });
                        }
                        if (lstPagoParcial.Count > 0)
                        {
                            lstPagoParcial.ForEach(delegate (PagoParcial objParcial)
                            {
                                string fecha = objParcial.FechaPago.ToString("dd/MM/yyyy", Cultura);
                                lstReferencias.Add(new ReferenciasPagadas
                                {
                                    Caja = "Caja",
                                    FechaPago = fecha,
                                    ReferenciaId = objPago.ReferenciaId,
                                    MontoPagado = ((decimal)objParcial.Pago).ToString("C", Cultura),
                                    MontoReferencia = ((decimal)objPago.Promesa).ToString("C", Cultura),
                                    Saldo = (objPago.Promesa - (decimal)objParcial.Pago).ToString("C", Cultura),
                                    SaldoD = (objPago.Promesa - (decimal)objParcial.Pago)
                                });
                            });
                        }
                    });
                    return lstReferencias;
                }
                catch
                {
                    return null;
                }
            }
        }
        public static string GenerarInscripcionColegiatura(int AlumnoId, int OfertaEducativaId, DateTime Fecha)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    //Usuario objUser = db.Usuario.Where(u => u.UsuarioId == UsuarioId).FirstOrDefault();
                    AlumnoInscrito objAlumno = db.AlumnoInscrito.Where(A => A.AlumnoId == AlumnoId && A.OfertaEducativaId == OfertaEducativaId).FirstOrDefault();
                    Periodo objPeriodo = db.Periodo.Where(P => Fecha >= P.FechaInicial && Fecha <= P.FechaFinal).FirstOrDefault();
                    Subperiodo objSub = db.Subperiodo.Where(S => S.MesId == Fecha.Month).FirstOrDefault();
                    #region "Mover a Bitacora"
                    //////////////////////////////Mover a Bitacora si ya existe el Inscrito/////////////////////////////////////
                    if (objAlumno.Anio != objPeriodo.Anio || objAlumno.PeriodoId != objPeriodo.PeriodoId)
                    {
                        db.AlumnoInscrito.Add(new AlumnoInscrito
                        {
                            AlumnoId = AlumnoId,
                            OfertaEducativaId = OfertaEducativaId,
                            Anio = objPeriodo.Anio,
                            PeriodoId = objPeriodo.PeriodoId,
                            FechaInscripcion = DateTime.Now,
                            PagoPlanId = objAlumno.PagoPlanId,
                            TurnoId = objAlumno.TurnoId,
                            EsEmpresa = objAlumno.EsEmpresa,
                            UsuarioId = objAlumno.UsuarioId,
                            EstatusId = objAlumno.EstatusId,
                            HoraInscripcion = DateTime.Now.TimeOfDay
                        });
                        db.AlumnoInscritoBitacora.Add(new AlumnoInscritoBitacora
                        {
                            AlumnoId = objAlumno.AlumnoId,
                            Anio = objAlumno.Anio,
                            EsEmpresa = objAlumno.EsEmpresa,
                            FechaInscripcion = objAlumno.FechaInscripcion,
                            OfertaEducativaId = objAlumno.OfertaEducativaId,
                            PagoPlanId = (int)objAlumno.PagoPlanId,
                            PeriodoId = objAlumno.PeriodoId,
                            TurnoId = objAlumno.TurnoId,
                            UsuarioId = objAlumno.UsuarioId,
                            HoraInscripcion = DateTime.Now.TimeOfDay,
                        });
                        db.AlumnoInscrito.Remove(objAlumno);
                        db.SaveChanges();
                    }
                    ///////////////////////////////////////////////////////////////////////
                    #endregion
                    Cuota CuotaColegiatura = BLLCuota.TraerPeriodoParcialIngles(OfertaEducativaId, objPeriodo.Anio, objPeriodo.PeriodoId, 807, Fecha);

                    if (CuotaColegiatura != null)
                    {

                        db.Pago.Add(
                            new Pago
                            {
                                ReferenciaId = "",
                                AlumnoId = AlumnoId,
                                Anio = objPeriodo.Anio,
                                PeriodoId = objPeriodo.PeriodoId,
                                SubperiodoId = objSub.SubperiodoId,
                                OfertaEducativaId = OfertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = CuotaColegiatura.CuotaId,
                                Cuota = CuotaColegiatura.Monto,
                                Promesa = CuotaColegiatura.Monto,
                                EstatusId = 1,
                                EsEmpresa = false,
                                UsuarioId = AlumnoId,
                                UsuarioTipoId = 2,
                                HoraGeneracion = DateTime.Now.TimeOfDay
                            });
                    }
                    db.SaveChanges();

                    db.Pago.Local.ToList().ForEach(delegate (Pago objPago)
                    {
                        objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                    });
                    db.SaveChanges();

                    return "Guardado";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }

        public static bool GenerarSemestre(int alumnoId, int ofertaEducativaId,int AnioInicial, int SubPeriodoInicial, int AnioFinal, int SubPeriodoFinal, int usuarioId, decimal Inscripcion, decimal Colegiatura)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<Subperiodo> ListaSubPeriodos = new List<Subperiodo>();
                    if (SubPeriodoInicial <= 7)
                    {
                        ListaSubPeriodos.AddRange(db.Subperiodo
                                                            .Where(s => s.MesId >= SubPeriodoInicial
                                                                    && s.MesId <= SubPeriodoFinal)
                                                             .ToList());
                    }
                    else
                    {
                        ListaSubPeriodos.AddRange(db.Subperiodo
                                                            .Where(s => s.MesId >= SubPeriodoInicial
                                                                    && s.MesId <= 12)
                                                             .ToList());
                        ListaSubPeriodos.AddRange(db.Subperiodo
                                                            .Where(s => s.MesId >= 1
                                                                    && s.MesId <= SubPeriodoFinal)
                                                             .ToList());
                    }

                    //ListaSubPeriodos = ListaSubPeriodos.OrderBy(m => m.MesId).ToList();

                    List<Periodo> Periodos = new List<Periodo>();
                    ListaSubPeriodos.ForEach(sbp =>
                    {
                        int anioc = SubPeriodoInicial >= 9 ? AnioFinal : (sbp.MesId < 9 ?AnioInicial : AnioFinal);

                        if (Periodos.Where(pe => pe.PeriodoId == sbp.PeriodoId
                                                                && pe.Anio ==anioc ).ToList().Count == 0)
                        {
                            var periodoadd = db.Periodo.Where(pe => pe.PeriodoId == sbp.PeriodoId
                                                                  && pe.Anio == anioc).FirstOrDefault();
                            Periodos.Add(periodoadd);
                        }
                    });

                    Periodos = Periodos.OrderBy(k => k.Anio).ThenBy(P => P.PeriodoId).ToList();
                    Alumno AlumnoSem = db.Alumno.Where(o => o.AlumnoId == alumnoId).FirstOrDefault();

                    Descuento DescuentoColegiatura = db.Descuento.Where(o => o.OfertaEducativaId == ofertaEducativaId
                                                                              && o.PagoConceptoId == 800
                                                                              && o.Descripcion == "Descuento en colegiatura")
                                                                .FirstOrDefault();

                    Descuento DescuentoInscripcion = db.Descuento.Where(o => o.OfertaEducativaId == ofertaEducativaId
                                                                              && o.PagoConceptoId == 802
                                                                              && o.Descripcion == "Descuento en inscripción")
                                                                .FirstOrDefault();
                    List<Cuota> CuotaColegiatura = new List<Cuota>();
                    List<Cuota> CuotaInscripcion = new List<Cuota>();

                    #region Pagos & Cuotas
                    List<Pago> PagosAlumnoTotales = AlumnoSem.Pago.Where(P => P.OfertaEducativaId == ofertaEducativaId
                                                                              && (P.Cuota1.PagoConceptoId == 802
                                                                              || P.Cuota1.PagoConceptoId == 800)
                                                                              && P.EstatusId != 2).ToList();
                    List<Pago> PagosAlumno = new List<Pago>();
                    bool AlmunDescIns = false, PagoAlumnInsc = false;

                    Periodos.ForEach(PeriodoP =>
                    {
                        CuotaColegiatura.Add(db.Cuota.Where(c => c.OfertaEducativaId == ofertaEducativaId
                                                                && c.Anio == PeriodoP.Anio
                                                                && c.PeriodoId == PeriodoP.PeriodoId
                                                                && c.PagoConceptoId == 800)
                                                     .FirstOrDefault());

                        CuotaInscripcion.Add(db.Cuota.Where(c => c.OfertaEducativaId == ofertaEducativaId
                                                                && c.Anio == PeriodoP.Anio
                                                                && c.PeriodoId == PeriodoP.PeriodoId
                                                                && c.PagoConceptoId == 802)
                                                     .FirstOrDefault());

                        ListaSubPeriodos.Where(K => K.PeriodoId == PeriodoP.PeriodoId)
                        .ToList().ForEach(sb =>
                            {
                                PagosAlumno.Add(PagosAlumnoTotales.Where(p => p.Anio == PeriodoP.Anio
                                                                               && p.PeriodoId == PeriodoP.PeriodoId
                                                                               && (p.SubperiodoId == sb.SubperiodoId
                                                                               || p.Cuota1.PagoConceptoId==802))
                                                                    .FirstOrDefault());
                            });
                    });

                    PagosAlumno.RemoveAll(P => P == null);
                    #endregion

                    Periodos.ForEach(PeriodoP =>
                    {
                        ///////Obtenemos el descuento en decimales 
                        decimal DescuentoColegiaturaD = (CuotaColegiatura.Where(P => P.PeriodoId == PeriodoP.PeriodoId
                                                                                                     && P.Anio == PeriodoP.Anio).FirstOrDefault()?.Monto ?? 0) - Colegiatura;

                        decimal DescuentoColegiaturaP = (DescuentoColegiaturaD * 100) / CuotaColegiatura.Where(P => P.PeriodoId == PeriodoP.PeriodoId
                                                                                                     && P.Anio == PeriodoP.Anio).FirstOrDefault()?.Monto ?? 0;
                        DescuentoColegiaturaP = decimal.Round(DescuentoColegiaturaP, 2);

                        decimal DescuentoInscripcionD = (CuotaInscripcion.Where(P => P.PeriodoId == PeriodoP.PeriodoId
                                                                                                && P.Anio == PeriodoP.Anio).FirstOrDefault()?.Monto ?? 0) - Inscripcion;

                        decimal DescunetoInscripcionP = (DescuentoInscripcionD * 100) / CuotaInscripcion.Where(P => P.PeriodoId == PeriodoP.PeriodoId
                                                                                                  && P.Anio == PeriodoP.Anio).FirstOrDefault()?.Monto ?? 0;
                        DescunetoInscripcionP = decimal.Round(DescunetoInscripcionP, 2);



                        //Buscar PagoDescuento de Inscripcion
                        #region AlumnoDescuento
                        if ((db.AlumnoDescuento.Where(a => a.AlumnoId == alumnoId
                                                            && a.OfertaEducativaId == ofertaEducativaId
                                                            && a.Anio == PeriodoP.Anio
                                                            && a.PeriodoId == PeriodoP.PeriodoId
                                                            && a.PagoConceptoId == 802
                                                            && a.DescuentoId == DescuentoInscripcion.DescuentoId)
                                                 .FirstOrDefault()?.AlumnoDescuentoId ?? 0) == 0 && !AlmunDescIns)
                        {
                            //Alumno descuento Inscripcion
                            db.AlumnoDescuento.Add(new AlumnoDescuento
                            {
                                AlumnoId = alumnoId,
                                OfertaEducativaId = ofertaEducativaId,
                                Anio = PeriodoP.Anio,
                                PeriodoId = PeriodoP.PeriodoId,
                                DescuentoId = DescuentoInscripcion.DescuentoId,
                                PagoConceptoId = 802,
                                Monto = DescunetoInscripcionP,
                                UsuarioId = usuarioId,
                                Comentario = "",
                                FechaGeneracion = DateTime.Now,
                                HoraGeneracion = DateTime.Now.TimeOfDay,
                                EsSEP = false,
                                EsComite = false,
                                EsDeportiva = false,
                                FechaAplicacion = DateTime.Now,
                                EstatusId = 2
                            });
                            AlmunDescIns = true;
                        }
                        else if ((db.AlumnoDescuento.Where(a => a.AlumnoId == alumnoId
                                                       && a.OfertaEducativaId == ofertaEducativaId
                                                       && a.Anio == PeriodoP.Anio
                                                       && a.PeriodoId == PeriodoP.PeriodoId
                                                       && a.PagoConceptoId == 802
                                                       && a.DescuentoId == DescuentoInscripcion.DescuentoId)
                                            .FirstOrDefault()?.AlumnoDescuentoId ?? 0) > 0 && !AlmunDescIns)
                        {
                            AlumnoDescuento AlumnoDescuentoInscripcion = db.AlumnoDescuento.Where(a => a.AlumnoId == alumnoId
                                                          && a.OfertaEducativaId == ofertaEducativaId
                                                          && a.Anio == PeriodoP.Anio
                                                          && a.PeriodoId == PeriodoP.PeriodoId
                                                          && a.PagoConceptoId == 802
                                                          && a.DescuentoId == DescuentoInscripcion.DescuentoId)
                                             .FirstOrDefault();

                            AlumnoDescuentoInscripcion.Monto = DescunetoInscripcionP;
                            AlmunDescIns = true;
                        }
                        //alumno Descuento colegiatura
                        if ((db.AlumnoDescuento.Where(a => a.AlumnoId == alumnoId
                                                          && a.OfertaEducativaId == ofertaEducativaId
                                                          && a.Anio == PeriodoP.Anio
                                                          && a.PeriodoId == PeriodoP.PeriodoId
                                                          && a.PagoConceptoId == 800
                                                          && a.DescuentoId == DescuentoColegiatura.DescuentoId)
                                               .FirstOrDefault()?.AlumnoDescuentoId ?? 0) == 0)
                        {
                            db.AlumnoDescuento.Add(new AlumnoDescuento
                            {
                                AlumnoId = alumnoId,
                                OfertaEducativaId = ofertaEducativaId,
                                Anio = PeriodoP.Anio,
                                PeriodoId = PeriodoP.PeriodoId,
                                DescuentoId = DescuentoColegiatura.DescuentoId,
                                PagoConceptoId = 800,
                                Monto = DescuentoColegiaturaP,
                                UsuarioId = usuarioId,
                                Comentario = "",
                                FechaGeneracion = DateTime.Now,
                                HoraGeneracion = DateTime.Now.TimeOfDay,
                                EsSEP = false,
                                EsComite = false,
                                EsDeportiva = false,
                                FechaAplicacion = DateTime.Now,
                                EstatusId = 2
                            });
                        }
                        else if ((db.AlumnoDescuento.Where(a => a.AlumnoId == alumnoId
                                                     && a.OfertaEducativaId == ofertaEducativaId
                                                     && a.Anio == PeriodoP.Anio
                                                     && a.PeriodoId == PeriodoP.PeriodoId
                                                     && a.PagoConceptoId == 800
                                                     && a.DescuentoId == DescuentoColegiatura.DescuentoId)
                                          .FirstOrDefault()?.AlumnoDescuentoId ?? 0) > 0)
                        {
                            AlumnoDescuento AlumnoDescuentoColegiatura = db.AlumnoDescuento.Where(a => a.AlumnoId == alumnoId
                                                      && a.OfertaEducativaId == ofertaEducativaId
                                                      && a.Anio == PeriodoP.Anio
                                                      && a.PeriodoId == PeriodoP.PeriodoId
                                                      && a.PagoConceptoId == 800
                                                      && a.DescuentoId == DescuentoColegiatura.DescuentoId)
                                           .FirstOrDefault();

                            AlumnoDescuentoColegiatura.Monto = DescuentoColegiaturaP;
                        }
                        #endregion

                        #region Pagos
                        if (PagosAlumno.Where(pa => pa.Cuota1.PagoConceptoId == 802
                                                         && pa.Anio == PeriodoP.Anio
                                                         && pa.PeriodoId == PeriodoP.PeriodoId)
                                            .ToList().Count == 0 && !PagoAlumnInsc)
                        {

                            db.Pago.Add(new Pago
                            {
                                ReferenciaId = "",
                                AlumnoId = alumnoId,
                                Anio = PeriodoP.Anio,
                                PeriodoId = PeriodoP.PeriodoId,
                                SubperiodoId = ListaSubPeriodos.FirstOrDefault()?.SubperiodoId ?? 0,
                                OfertaEducativaId = ofertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                HoraGeneracion = DateTime.Now.TimeOfDay,
                                CuotaId = CuotaInscripcion.Where(P => P.PeriodoId == PeriodoP.PeriodoId
                                                                                                        && P.Anio == PeriodoP.Anio).FirstOrDefault()?.CuotaId ?? 0,
                                Cuota = CuotaInscripcion.Where(P => P.PeriodoId == PeriodoP.PeriodoId
                                                                                                        && P.Anio == PeriodoP.Anio).FirstOrDefault()?.Monto ?? 0,
                                Promesa = Inscripcion,
                                Restante = Inscripcion,
                                EstatusId = DescuentoInscripcionD == 100 ? 4 : 1,
                                UsuarioId = usuarioId,
                                UsuarioTipoId = 1,
                                EsEmpresa = false,
                                EsAnticipado = false,
                                PeriodoAnticipadoId = 0,
                                PagoDescuento = DescuentoInscripcionD > 0 ?
                                                    new List<PagoDescuento>
                                                    {
                                                    new PagoDescuento
                                                    {
                                                        DescuentoId =DescuentoInscripcion.DescuentoId,
                                                        Monto= (CuotaInscripcion.Where(P => P.PeriodoId == PeriodoP.PeriodoId
                                                        && P.Anio == PeriodoP.Anio).FirstOrDefault()?.Monto ?? 0 ) - Inscripcion
                                                    }
                                                    } : null
                            });
                            PagoAlumnInsc = true;
                        }

                        #region aun falta componer
                        //else if (PagosAlumno.Where(pa => pa.Cuota1.PagoConceptoId == 802
                        //                             && pa.Anio == PeriodoP.Anio
                        //                             && pa.PeriodoId == PeriodoP.PeriodoId).ToList().Count > 0 && !PagoAlumnInsc)
                        //{
                        //    Pago AlumnoPagoInscripcion = PagosAlumno.Where(pa => pa.Cuota1.PagoConceptoId == 802
                        //                               && pa.Anio == PeriodoP.Anio
                        //                               && pa.PeriodoId == PeriodoP.PeriodoId).FirstOrDefault();

                        //    decimal TotalRegresar = AlumnoPagoInscripcion.Promesa - AlumnoPagoInscripcion.Restante;
                        //    AlumnoPagoInscripcion.Promesa = Inscripcion;
                        //    if (TotalRegresar >= Inscripcion)
                        //    {
                        //        AlumnoPagoInscripcion.EstatusId = 4;
                        //        AlumnoPagoInscripcion.Restante = 0;
                        //        AlumnoPagoInscripcion.Promesa = Inscripcion;
                        //    }
                        //    else
                        //    {
                        //        AlumnoPagoInscripcion.EstatusId = 1;
                        //        AlumnoPagoInscripcion.Restante = Inscripcion - TotalRegresar;
                        //        AlumnoPagoInscripcion.Promesa = Inscripcion;
                        //    }

                        //    if(AlumnoPagoInscripcion.PagoDescuento.Where(P=> P.DescuentoId == DescuentoInscripcion.DescuentoId).ToList().Count > 0)
                        //    {
                        //        PagoDescuento PagoDescuentoColegiatura = AlumnoPagoInscripcion.PagoDescuento.Where(P => P.DescuentoId == DescuentoInscripcion.DescuentoId).FirstOrDefault();
                        //        PagoDescuentoColegiatura.Monto = AlumnoPagoInscripcion.Cuota - Inscripcion;
                        //    }

                        //    if (TotalRegresar > 0)
                        //    {

                        //        if (AlumnoPagoInscripcion.PagoParcial.ToList().Count > 0)
                        //        {
                        //            AlumnoPagoInscripcion.PagoParcial.ToList().ForEach(pp =>
                        //            {
                        //                if (pp.Pago <= TotalRegresar)
                        //                {
                        //                    pp.ReferenciaProcesada.Restante += pp.Pago;
                        //                    pp.ReferenciaProcesada.SeGasto = false;
                        //                    TotalRegresar = TotalRegresar - pp.Pago;
                        //                    pp.EstatusId = 2;
                        //                }
                        //                else
                        //                {
                        //                    pp.ReferenciaProcesada.Restante += (pp.Pago-TotalRegresar);
                        //                    pp.ReferenciaProcesada.SeGasto = false;
                        //                    pp.
                        //                }
                        //            });

                        //        }
                        //    }
                        //}
                        #endregion
                        
                            ListaSubPeriodos.Where(k => k.PeriodoId == PeriodoP.PeriodoId)
                            .ToList().ForEach(subp =>
                            {
                                if (PagosAlumno.Where(pa => pa.Cuota1.PagoConceptoId == 800
                                                    && pa.Anio == PeriodoP.Anio
                                                    && pa.PeriodoId == PeriodoP.PeriodoId
                                                    && pa.SubperiodoId == subp.SubperiodoId).ToList().Count == 0)
                                {
                                    db.Pago.Add(new Pago
                                    {
                                        ReferenciaId = "",
                                        AlumnoId = alumnoId,
                                        Anio = PeriodoP.Anio,
                                        PeriodoId = PeriodoP.PeriodoId,
                                        SubperiodoId = subp.SubperiodoId,
                                        OfertaEducativaId = ofertaEducativaId,
                                        FechaGeneracion = DateTime.Now,
                                        HoraGeneracion = DateTime.Now.TimeOfDay,
                                        CuotaId = CuotaColegiatura.Where(P => P.PeriodoId == PeriodoP.PeriodoId
                                                                                                                && P.Anio == PeriodoP.Anio).FirstOrDefault()?.CuotaId ?? 0,
                                        Cuota = CuotaColegiatura.Where(P => P.PeriodoId == PeriodoP.PeriodoId
                                                                                                                && P.Anio == PeriodoP.Anio).FirstOrDefault()?.Monto ?? 0,
                                        Promesa = Colegiatura,
                                        Restante = Colegiatura,
                                        EstatusId = DescuentoColegiaturaP == 100 ? 4 : 1,
                                        UsuarioId = usuarioId,
                                        UsuarioTipoId = 1,
                                        EsEmpresa = false,
                                        EsAnticipado = false,
                                        PeriodoAnticipadoId = 0,
                                        PagoDescuento = DescuentoColegiaturaP > 0 ?
                                                    new List<PagoDescuento>
                                                    {
                                                    new PagoDescuento
                                                    {
                                                        DescuentoId =DescuentoColegiatura.DescuentoId,
                                                        Monto= (CuotaColegiatura.Where(P => P.PeriodoId == PeriodoP.PeriodoId
                                                        && P.Anio == PeriodoP.Anio).FirstOrDefault()?.Monto ?? 0 ) - Colegiatura
                                                    }
                                                    } : null
                                    });
                                }
                            });
                        

                        #endregion
                    });

                    db.SaveChanges();

                    db.Pago.Local.ToList()
                        .ForEach(p =>
                        {
                            p.ReferenciaId = db.spGeneraReferencia(p.PagoId).FirstOrDefault();
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

        public static object BuscarPagosActuales(int alumnoId, int ofertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                if (db.OfertaEducativa
                        .Where(ofe => ofe.OfertaEducativaId == ofertaEducativaId)
                        .FirstOrDefault().OfertaEducativaTipoId == 5)
                {

                    int SubPeriodoActual = BLLPeriodoPortal.TraerSubPeriodoEntreFechas(DateTime.Now);
                    DTOPeriodo PeriodoActual = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);
                    List<Pago> Pagos = db.Pago
                                        .Where(p => p.AlumnoId == alumnoId
                                                && p.OfertaEducativaId == ofertaEducativaId
                                                && ((p.Anio == PeriodoActual.Anio
                                                    && p.PeriodoId == PeriodoActual.PeriodoId && p.SubperiodoId >= SubPeriodoActual) || (
                                                    (PeriodoActual.PeriodoId == 3 ? 1 : PeriodoActual.PeriodoId) == p.PeriodoId &&
                                                    (PeriodoActual.PeriodoId == 3 ? PeriodoActual.Anio + 1 : PeriodoActual.Anio) == p.Anio))
                                                && (p.Cuota1.PagoConceptoId == 800 || p.Cuota1.PagoConceptoId == 802))
                                        .Take(7)
                                        .ToList();
                    Pagos = Pagos.OrderBy(p => p.Subperiodo.MesId).ToList(); 

                    return Pagos.Select(p =>
                                new DTOPagos
                                {
                                    Anio = p.Anio,
                                    PeriodoId = p.PeriodoId,
                                    EstatusId = p.EstatusId,
                                    PagoId = p.PagoId,
                                    Referencia = p.ReferenciaId,
                                    objNormal = new Pagos_Detalles
                                    {
                                        Monto = "$" + p.Promesa,
                                        Restante = "$" + p.Restante
                                    },
                                    FechaGeneracionS = (p.FechaGeneracion.Day < 10 ? "0" + p.FechaGeneracion.Day : "" + p.FechaGeneracion.Day) +
                                                    (p.FechaGeneracion.Month < 10 ? "0" + p.FechaGeneracion.Month : "" + p.FechaGeneracion.Month) +
                                                    "" + p.FechaGeneracion.Year,
                                    DTOCuota = new DTOCuota
                                    {
                                        CuotaId = p.Cuota1.CuotaId,
                                        DTOPagoConcepto = new DTOPagoConcepto
                                        {
                                            PagoConceptoId = p.Cuota1.PagoConceptoId,
                                            Descripcion = p.Cuota1.PagoConceptoId == 800 ?
                                                        p.Cuota1.PagoConcepto.Descripcion + " " + p.Subperiodo.Mes.Descripcion + " " + p.Periodo.Anio + " - " + p.Periodo.PeriodoId :
                                                        p.Cuota1.PagoConcepto.Descripcion
                                        }
                                    }

                                }).ToList();
                }
                else
                {
                    return new
                    {
                        Code="500",
                        Message = "La ofertaEducativa no corresponde a plan Semestral."
                    };
                }

            }
        }

        public static string GenerarInscripcionColegiatura(int AlumnoId, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DateTime fHoy = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
                    Boolean Anticipado = db.Subperiodo.Where(SP => SP.MesId == fHoy.Month).FirstOrDefault().SubperiodoId == 4 ? true : false;
                    AlumnoInscrito objAlumno = db.AlumnoInscrito.Where(A => A.AlumnoId == AlumnoId && A.OfertaEducativaId == OfertaEducativaId).FirstOrDefault();
                    Periodo ObjPeriodoDB = db.Periodo.Where(P => fHoy >= P.FechaInicial && fHoy <= P.FechaFinal).FirstOrDefault();
                    DateTime FechaRecargo = Utilities.Fecha.Prorroga(fHoy.Year, fHoy.Month, true, 5);
                    Boolean Recargo = Anticipado == false ? fHoy > FechaRecargo ? true : false : false;
                    decimal DesIns = 0, DesCol = 0;
                    DTOPeriodo objPeriodo = new DTOPeriodo
                    {
                        Anio = ObjPeriodoDB.Anio,
                        PeriodoId = ObjPeriodoDB.PeriodoId,
                        Descripcion = ObjPeriodoDB.Descripcion
                    };

                    objPeriodo = Anticipado == true ? PeridoSiguiente(ObjPeriodoDB) : new DTOPeriodo
                    {
                        Anio = objPeriodo.Anio,
                        PeriodoId = objPeriodo.PeriodoId
                    };
                    List<Pago> lstPagos = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.PeriodoId == objPeriodo.PeriodoId &&
                       P.Anio == objPeriodo.Anio && P.OfertaEducativaId == OfertaEducativaId && (P.Cuota1.PagoConceptoId == 800 || P.Cuota1.PagoConceptoId == 802)
                       && P.EstatusId != 2 && P.EstatusId != 3).ToList();
                    if (lstPagos.Count > 0) { return "Ya tiene Cargos"; }

                    Cuota CuotaInscripcion = db.Cuota.Where(C => C.Anio == objPeriodo.Anio && C.PeriodoId == objPeriodo.PeriodoId && C.OfertaEducativaId == OfertaEducativaId && C.PagoConceptoId == 802).FirstOrDefault();
                    Cuota CuotaColegiatura = db.Cuota.Where(C => C.Anio == objPeriodo.Anio && C.PeriodoId == objPeriodo.PeriodoId && C.OfertaEducativaId == OfertaEducativaId && C.PagoConceptoId == 800).FirstOrDefault();
                    //objAlumno.Anio = objPeriodo.Anio;
                    //objAlumno.PeriodoId = objPeriodo.PeriodoId;

                    decimal DesInscM = 0;
                    decimal DesColM = 0;
                    #region Alumnos - Empresa - Especiales 
                    BLLGrupo.UpdateAlumnoConfiguracion(objPeriodo.Anio, objPeriodo.PeriodoId, AlumnoId, OfertaEducativaId, 7878); //Usuario de Luis Daniel
                    List<DTOAlumnoGrupoCuota> lstAlumnoGrupoDesc = db.GrupoAlumnoConfiguracion.Where(a => a.AlumnoId == AlumnoId && a.OfertaEducativaId == OfertaEducativaId).Select(a => new DTOAlumnoGrupoCuota
                    {
                        AlumnoId = (int)a.AlumnoId,
                        Anio = (int)a.Anio,
                        GrupoId = (int)a.GrupoId,
                        OFertaEducativaId = (int)a.OfertaEducativaId,
                        PeriodoId = (int)a.PeriodoId,
                        MontoInscripcion = a.CuotaInscripcion,
                        MontoColegiatura = a.CuotaColegiatura
                    }).ToList();

                    int DescuentoInsc = 0, DescuentoColg = 0;
                    //si hay algo en la configuracion se trae 
                    if (lstAlumnoGrupoDesc.Count > 0)
                    {
                        #region AlumnoDescuento
                        Recargo = false;

                        DescuentoInsc = db.Descuento.Where(d => d.PagoConceptoId == 802 && d.Descripcion == "Descuento en inscripción" && d.OfertaEducativaId == OfertaEducativaId).FirstOrDefault().DescuentoId;
                        DescuentoColg = db.Descuento.Where(d => d.PagoConceptoId == 800 && d.Descripcion == "Descuento en colegiatura" && d.OfertaEducativaId == OfertaEducativaId).FirstOrDefault().DescuentoId;

                        DTOAlumnoGrupoCuota objAlDesc = lstAlumnoGrupoDesc.Last();
                        DesInscM = (decimal)objAlDesc.MontoInscripcion;
                        DesColM = (decimal)objAlDesc.MontoColegiatura;
                        DesIns = 100 - ((decimal)objAlDesc.MontoInscripcion * 100) / CuotaInscripcion.Monto;
                        DesCol = 100 - ((decimal)objAlDesc.MontoColegiatura * 100) / CuotaColegiatura.Monto;
                        //Inscripcion
                        db.AlumnoDescuento.Add(new AlumnoDescuento
                        {
                            AlumnoId = objAlumno.AlumnoId,
                            Anio = objPeriodo.Anio,
                            Comentario = "",
                            DescuentoId = DescuentoInsc,
                            OfertaEducativaId = OfertaEducativaId,
                            PagoConceptoId = 802,
                            EstatusId = 2,
                            FechaAplicacion = DateTime.Now,
                            FechaGeneracion = DateTime.Now,
                            Monto = 100 - ((decimal)objAlDesc.MontoInscripcion * 100) / CuotaInscripcion.Monto,
                            PeriodoId = objPeriodo.PeriodoId,
                            UsuarioId = 7878,
                            HoraGeneracion = DateTime.Now.TimeOfDay,
                        });
                        //Colegiatura
                        db.AlumnoDescuento.Add(new AlumnoDescuento
                        {
                            AlumnoId = objAlumno.AlumnoId,
                            Anio = objPeriodo.Anio,
                            Comentario = "",
                            DescuentoId = DescuentoColg,
                            OfertaEducativaId = OfertaEducativaId,
                            PagoConceptoId = 800,
                            EstatusId = 2,
                            FechaAplicacion = DateTime.Now,
                            FechaGeneracion = DateTime.Now,
                            Monto = 100 - ((decimal)objAlDesc.MontoColegiatura * 100) / CuotaColegiatura.Monto,
                            PeriodoId = objPeriodo.PeriodoId,
                            UsuarioId = 7878,
                            HoraGeneracion = DateTime.Now.TimeOfDay,

                        });

                        #endregion
                    }
                    #endregion
                    #region Pagos
                    if (CuotaInscripcion != null)
                    {
                        db.Pago.Add(
                            new Pago
                            {
                                ReferenciaId = "",
                                AlumnoId = AlumnoId,
                                Anio = objPeriodo.Anio,
                                PeriodoId = objPeriodo.PeriodoId,
                                SubperiodoId = 1,
                                OfertaEducativaId = OfertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = CuotaInscripcion.CuotaId,
                                Cuota = CuotaInscripcion.Monto,
                                Promesa = DesIns > 0 ? DesIns == 100 ? 0 : DesInscM : CuotaInscripcion.Monto,
                                Restante = DesIns > 0 ? DesIns == 100 ? 0 : DesInscM : CuotaInscripcion.Monto,
                                EstatusId = DesIns == 100 ? 4 : 1,
                                EsEmpresa = false,
                                UsuarioId = AlumnoId,
                                UsuarioTipoId = 2,
                                HoraGeneracion = DateTime.Now.TimeOfDay,
                                PagoDescuento = DesIns > 0 ?
                                new List<PagoDescuento> {
                                    new PagoDescuento
                                    {
                                        DescuentoId = DescuentoInsc,
                                        Monto = CuotaInscripcion.Monto - DesInscM
                                    }
                                } : null
                            });
                    }
                    if (CuotaColegiatura != null)
                    {
                        int nTantos = 1;// Anticipado == true ? 1 : 4;

                        for (int i = 1; i <= nTantos; i++)
                        {
                            db.Pago.Add(
                                new Pago
                                {
                                    ReferenciaId = "",
                                    AlumnoId = AlumnoId,
                                    Anio = objPeriodo.Anio,
                                    PeriodoId = objPeriodo.PeriodoId,
                                    SubperiodoId = i,
                                    OfertaEducativaId = OfertaEducativaId,
                                    FechaGeneracion = DateTime.Now,
                                    CuotaId = CuotaColegiatura.CuotaId,
                                    Cuota = CuotaColegiatura.Monto,
                                    Promesa = DesCol > 0 ? DesCol == 100 ? 0 : DesColM : CuotaColegiatura.Monto,
                                    Restante = DesCol > 0 ? DesCol == 100 ? 0 : DesColM : CuotaColegiatura.Monto,
                                    EstatusId = DesCol == 100 ? 4 : 1,
                                    EsEmpresa = false,
                                    UsuarioId = AlumnoId,
                                    UsuarioTipoId = 2,
                                    HoraGeneracion = DateTime.Now.TimeOfDay,
                                    PagoDescuento = DesCol > 0 ?
                                        new List<PagoDescuento> {
                                            new PagoDescuento
                                            {
                                                DescuentoId = DescuentoColg,
                                                Monto = CuotaColegiatura.Monto - DesColM
                                            }
                                        } : null
                                });
                        }
                    }
                    #endregion

                    db.SaveChanges();

                    #region Adeudos
                    if (Recargo)
                    {
                        db.Pago.Local.ToList().ForEach(delegate (Pago objPago)
                        {
                            if (objPago.EstatusId != 4)
                            {
                                Cuota objCuotaRecargo = db.Cuota.Where(C => C.PagoConceptoId == 306
                                    && C.OfertaEducativaId == objPago.OfertaEducativaId && C.Anio == objPago.Anio && C.PeriodoId == objPago.PeriodoId).FirstOrDefault();
                                decimal Cuota = objPago.Cuota * decimal.Parse("0.05");
                                Cuota = Math.Round(Cuota);

                                db.PagoRecargo.Add(new PagoRecargo
                                {
                                    Pago1 = new Pago
                                    {
                                        ReferenciaId = "",
                                        AlumnoId = objPago.AlumnoId,
                                        Anio = objPago.Anio,
                                        PeriodoId = objPago.PeriodoId,
                                        SubperiodoId = objPago.SubperiodoId,
                                        OfertaEducativaId = objPago.OfertaEducativaId,
                                        FechaGeneracion = DateTime.Now,
                                        CuotaId = objCuotaRecargo.CuotaId,
                                        Cuota = Cuota,
                                        Promesa = Cuota,
                                        Restante = Cuota,
                                        EstatusId = 1,
                                        EsEmpresa = false,
                                        UsuarioId = AlumnoId,
                                        UsuarioTipoId = 2,
                                        HoraGeneracion = DateTime.Now.TimeOfDay
                                    },
                                    PagoId = objPago.PagoId,
                                    Fecha = DateTime.Now
                                });
                            }
                        });
                        db.SaveChanges();
                    }
                    #endregion
                    db.Pago.Local.ToList().ForEach(delegate (Pago objPago)
                    {
                        objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                    });

                    db.SaveChanges();

                    return "Guardado";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }
        public static string GenerarInscripcionColegiatura(int AlumnoId, int OfertaEducativaId, int Anio, int PeriodoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    AlumnoInscrito Alumno = db.AlumnoInscrito.Where(A => A.AlumnoId == AlumnoId && A.OfertaEducativaId == OfertaEducativaId).FirstOrDefault();

                    DateTime FechaRecargo = Utilities.Fecha.Prorroga(DateTime.Now.Year, DateTime.Now.Month, true, 5);
                    Boolean Recargo = DateTime.Now > FechaRecargo && Alumno.EsEmpresa == false ? true : false;

                    List<Pago> Pagos = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.PeriodoId == PeriodoId &&
                       P.Anio == Anio && P.OfertaEducativaId == OfertaEducativaId && (P.Cuota1.PagoConceptoId == 800 || P.Cuota1.PagoConceptoId == 802)
                       && P.EstatusId != 2 && P.EstatusId != 3).ToList();

                    if (Pagos.Count > 0) { return "Ya tiene Cargos"; }

                    Cuota CuotaInscripcion = db.Cuota.Where(C => C.Anio == Anio && C.PeriodoId == PeriodoId && C.OfertaEducativaId == OfertaEducativaId && C.PagoConceptoId == 802).FirstOrDefault();

                    Cuota CuotaColegiatura = db.Cuota.Where(C => C.Anio == Anio && C.PeriodoId == PeriodoId && C.OfertaEducativaId == OfertaEducativaId && C.PagoConceptoId == 800).FirstOrDefault();

                    if (CuotaInscripcion != null)
                    {
                        db.Pago.Add(
                            new Pago
                            {
                                ReferenciaId = "",
                                AlumnoId = AlumnoId,
                                Anio = Anio,
                                PeriodoId = PeriodoId,
                                SubperiodoId = 1,
                                OfertaEducativaId = OfertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = CuotaInscripcion.CuotaId,
                                Cuota = CuotaInscripcion.Monto,
                                Promesa = CuotaInscripcion.Monto,
                                Restante = CuotaInscripcion.Monto,
                                EstatusId = 1,
                                EsEmpresa = false,
                                UsuarioId = AlumnoId,
                                UsuarioTipoId = 2,
                                HoraGeneracion = DateTime.Now.TimeOfDay
                            });
                    }
                    if (CuotaColegiatura != null)
                    {
                        int nTantos = 1;// Anticipado == true ? 1 : 4;

                        for (int i = 1; i <= nTantos; i++)
                        {
                            db.Pago.Add(
                                new Pago
                                {
                                    ReferenciaId = "",
                                    AlumnoId = AlumnoId,
                                    Anio = Anio,
                                    PeriodoId = PeriodoId,
                                    SubperiodoId = i,
                                    OfertaEducativaId = OfertaEducativaId,
                                    FechaGeneracion = DateTime.Now,
                                    CuotaId = CuotaColegiatura.CuotaId,
                                    Cuota = CuotaColegiatura.Monto,
                                    Promesa = CuotaColegiatura.Monto,
                                    Restante = CuotaColegiatura.Monto,
                                    EstatusId = 1,
                                    EsEmpresa = false,
                                    UsuarioId = AlumnoId,
                                    UsuarioTipoId = 2,
                                    HoraGeneracion = DateTime.Now.TimeOfDay
                                });
                        }
                    }
                    db.SaveChanges();
                    #region Adeudos
                    if (Recargo)
                    {
                        db.Pago.Local.ToList().ForEach(delegate (Pago objPago)
                        {
                            Cuota objCuotaRecargo = db.Cuota.Where(C => C.PagoConceptoId == 306
                                && C.OfertaEducativaId == objPago.OfertaEducativaId && C.Anio == objPago.Anio && C.PeriodoId == objPago.PeriodoId).FirstOrDefault();
                            db.PagoRecargo.Add(new PagoRecargo
                            {
                                Pago1 = new Pago
                                {
                                    ReferenciaId = "",
                                    AlumnoId = objPago.AlumnoId,
                                    Anio = objPago.Anio,
                                    PeriodoId = objPago.PeriodoId,
                                    SubperiodoId = objPago.SubperiodoId,
                                    OfertaEducativaId = objPago.OfertaEducativaId,
                                    FechaGeneracion = DateTime.Now,
                                    CuotaId = objCuotaRecargo.CuotaId,
                                    Cuota = Math.Round(objPago.Cuota * decimal.Parse("0.05")),
                                    Promesa = Math.Round(objPago.Cuota * decimal.Parse("0.05")),
                                    Restante = Math.Round(objPago.Cuota * decimal.Parse("0.05")),
                                    EstatusId = 1,
                                    EsEmpresa = false,
                                    UsuarioId = AlumnoId,
                                    UsuarioTipoId = 2,
                                    HoraGeneracion = DateTime.Now.TimeOfDay
                                },
                                PagoId = objPago.PagoId,
                                Fecha = DateTime.Now
                            });
                        });
                        db.SaveChanges();
                    }
                    #endregion
                    db.Pago.Local.ToList().ForEach(delegate (Pago objPago)
                    {
                        objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                    });

                    db.SaveChanges();


                    return "Guardado";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }
        public static string GenerarInscripcionColegiatura2(int AlumnoId, int OfertaEducativaId, int Anio, int PeriodoId, decimal BecaCol, decimal BecaInsc)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    AlumnoInscrito objAlumno = db.AlumnoInscrito.Where(A => A.AlumnoId == AlumnoId && A.OfertaEducativaId == OfertaEducativaId).FirstOrDefault();
                    Periodo ObjPeriodoDB = db.Periodo.Where(P => P.Anio == Anio && P.PeriodoId == PeriodoId).FirstOrDefault();
                    DateTime FechaRecargo = Utilities.Fecha.Prorroga(DateTime.Now.Year, DateTime.Now.Month, true, 5);
                    Boolean Recargo = DateTime.Now > FechaRecargo ? true : false;

                    List<Pago> lstPagos = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.PeriodoId == PeriodoId &&
                       P.Anio == Anio && P.OfertaEducativaId == OfertaEducativaId && (P.Cuota1.PagoConceptoId == 800 || P.Cuota1.PagoConceptoId == 802)
                       && P.EstatusId != 2 && P.EstatusId != 3).ToList();
                    if (lstPagos.Count > 0) { return "Ya tiene Cargos"; }

                    Cuota CuotaInscripcion = db.Cuota.Where(C => C.Anio == Anio && C.PeriodoId == PeriodoId && C.OfertaEducativaId == OfertaEducativaId && C.PagoConceptoId == 802).FirstOrDefault();
                    //Descuento DescuentoIns = db.Descuento.Where(d => d.OfertaEducativaId == OfertaEducativaId && d.PagoConceptoId == 802 && d.Descripcion == "Descuento en inscripción").FirstOrDefault();

                    Cuota CuotaColegiatura = db.Cuota.Where(C => C.Anio == Anio && C.PeriodoId == PeriodoId && C.OfertaEducativaId == OfertaEducativaId && C.PagoConceptoId == 800).FirstOrDefault();
                    //Descuento DescuentoIns = db.Descuento.Where(d => d.OfertaEducativaId == OfertaEducativaId && d.PagoConceptoId == 800 && d.Descripcion == "Descuento en inscripción").FirstOrDefault();
                    //objAlumno.Anio = objPeriodo.Anio
                    //objAlumno.PeriodoId = objPeriodo.PeriodoId;
                    #region "Mover a Bitacora"
                    //////////////////////////////Mover a Bitacora si ya existe el Inscrito/////////////////////////////////////
                    if (db.AlumnoInscrito.Where(AL => AL.AlumnoId == AlumnoId && AL.OfertaEducativaId == OfertaEducativaId && AL.Anio == ObjPeriodoDB.Anio && AL.PeriodoId == ObjPeriodoDB.PeriodoId)
                        .AsNoTracking().ToList().Count == 0)
                    {
                        db.AlumnoInscrito.Add(new AlumnoInscrito
                        {
                            AlumnoId = AlumnoId,
                            OfertaEducativaId = OfertaEducativaId,
                            Anio = Anio,
                            PeriodoId = PeriodoId,
                            FechaInscripcion = DateTime.Now,
                            PagoPlanId = objAlumno.PagoPlanId,
                            TurnoId = objAlumno.TurnoId,
                            EsEmpresa = objAlumno.EsEmpresa,
                            UsuarioId = objAlumno.UsuarioId,
                            EstatusId = objAlumno.EstatusId,
                            HoraInscripcion = DateTime.Now.TimeOfDay,

                        });
                        db.AlumnoInscritoBitacora.Add(new AlumnoInscritoBitacora
                        {
                            AlumnoId = objAlumno.AlumnoId,
                            Anio = objAlumno.Anio,
                            EsEmpresa = objAlumno.EsEmpresa,
                            FechaInscripcion = objAlumno.FechaInscripcion,
                            OfertaEducativaId = objAlumno.OfertaEducativaId,
                            PagoPlanId = (int)objAlumno.PagoPlanId,
                            PeriodoId = objAlumno.PeriodoId,
                            TurnoId = objAlumno.TurnoId,
                            UsuarioId = objAlumno.UsuarioId,
                            HoraInscripcion = DateTime.Now.TimeOfDay,
                        });
                        db.AlumnoInscrito.Remove(objAlumno);
                        db.SaveChanges();
                    }
                    ///////////////////////////////////////////////////////////////////////
                    #endregion
                    if (CuotaInscripcion != null)
                    {
                        db.Pago.Add(
                            new Pago
                            {
                                ReferenciaId = "",
                                AlumnoId = AlumnoId,
                                Anio = Anio,
                                PeriodoId = PeriodoId,
                                SubperiodoId = 1,
                                OfertaEducativaId = OfertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                CuotaId = CuotaInscripcion.CuotaId,
                                Cuota = CuotaInscripcion.Monto,
                                Promesa = CuotaInscripcion.Monto,
                                Restante = CuotaInscripcion.Monto,
                                EstatusId = 1,
                                EsEmpresa = false,
                                UsuarioId = AlumnoId,
                                UsuarioTipoId = 2,
                                HoraGeneracion = DateTime.Now.TimeOfDay
                            });
                    }
                    if (CuotaColegiatura != null)
                    {
                        //db.AlumnoDescuento.Add(new AlumnoDescuento
                        //{
                        //    AlumnoId = AlumnoId,
                        //    Anio = Anio,
                        //    Comentario = "",
                        //    DescuentoId
                        //});
                        int nTantos = 4;// Anticipado == true ? 1 : 4;

                        for (int i = 1; i <= nTantos; i++)
                        {
                            db.Pago.Add(
                                new Pago
                                {
                                    ReferenciaId = "",
                                    AlumnoId = AlumnoId,
                                    Anio = Anio,
                                    PeriodoId = PeriodoId,
                                    SubperiodoId = i,
                                    OfertaEducativaId = OfertaEducativaId,
                                    FechaGeneracion = DateTime.Now,
                                    CuotaId = CuotaColegiatura.CuotaId,
                                    Cuota = CuotaColegiatura.Monto,
                                    Promesa = CuotaColegiatura.Monto,
                                    Restante = CuotaColegiatura.Monto,
                                    EstatusId = 1,
                                    EsEmpresa = false,
                                    UsuarioId = AlumnoId,
                                    UsuarioTipoId = 2,
                                    HoraGeneracion = DateTime.Now.TimeOfDay
                                });
                        }
                    }
                    db.SaveChanges();
                    #region Adeudos
                    //if (Recargo)
                    //{
                    //    db.Pago.Local.ToList().ForEach(delegate (Pago objPago)
                    //    {
                    //        if (objPago.Cuota1.PagoConceptoId == 800 && (objPago.SubperiodoId == 1 || objPago.SubperiodoId == 2))
                    //        {
                    //            Cuota objCuotaRecargo = db.Cuota.Where(C => C.PagoConceptoId == 306
                    //                && C.OfertaEducativaId == objPago.OfertaEducativaId && C.Anio == objPago.Anio && C.PeriodoId == objPago.PeriodoId).FirstOrDefault();
                    //            db.PagoRecargo.Add(new PagoRecargo
                    //            {
                    //                Pago1 = new Pago
                    //                {
                    //                    ReferenciaId = "",
                    //                    AlumnoId = objPago.AlumnoId,
                    //                    Anio = objPago.Anio,
                    //                    PeriodoId = objPago.PeriodoId,
                    //                    SubperiodoId = objPago.SubperiodoId,
                    //                    OfertaEducativaId = objPago.OfertaEducativaId,
                    //                    FechaGeneracion = DateTime.Now,
                    //                    CuotaId = objCuotaRecargo.CuotaId,
                    //                    Cuota = Math.Round(objPago.Cuota * decimal.Parse("0.05")),
                    //                    Promesa = Math.Round(objPago.Cuota * decimal.Parse("0.05")),
                    //                    EstatusId = 1,
                    //                    EsEmpresa = false
                    //                },
                    //                PagoId = objPago.PagoId,
                    //                Fecha = DateTime.Now
                    //            });
                    //        }
                    //    });

                    //    db.SaveChanges(); 
                    //}
                    #endregion
                    db.Pago.Local.ToList().ForEach(delegate (Pago objPago)
                    {
                        objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                    });

                    db.SaveChanges();


                    return "Guardado";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }
        public static List<string> BuscarPagoIngles(int AlumnoId, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<string> Resultados = new List<string>();

                DateTime Fhoy = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
                Boolean Anticipado = db.Subperiodo.Where(SP => SP.MesId == Fhoy.Month).FirstOrDefault().SubperiodoId == 4 ? true : false;
                Periodo PeriodoActual = db.Periodo.Where(P => Fhoy >= P.FechaInicial && Fhoy <= P.FechaFinal).FirstOrDefault();
                DTOPeriodo objPeriodo = new DTOPeriodo
                {
                    Anio = PeriodoActual.Anio,
                    PeriodoId = PeriodoActual.PeriodoId,
                    Descripcion = PeriodoActual.Descripcion
                };
                DTOPeriodo PeriodoSiguiente = Anticipado == true ? PeridoSiguiente(PeriodoActual) : new DTOPeriodo
                {
                    Anio = objPeriodo.Anio,
                    PeriodoId = objPeriodo.PeriodoId,
                    Descripcion = objPeriodo.Descripcion
                };
                List<Pago> Pagos = db.Pago
                                        .Where(
                                            P =>
                                                P.Anio == PeriodoSiguiente.Anio
                                                && P.PeriodoId == PeriodoSiguiente.PeriodoId
                                                && P.AlumnoId == AlumnoId
                                                && P.OfertaEducativaId == OfertaEducativaId
                                                && P.EstatusId != 2
                                                && (P.Cuota1.PagoConceptoId == 800
                                                        || P.Cuota1.PagoConceptoId == 802
                                                        || P.Cuota1.PagoConceptoId == 807))
                                        .ToList();

                Resultados.Add(
                        Pagos.Where(P => P.SubperiodoId == 1).FirstOrDefault() != null ? Pagos.Max(P => P.SubperiodoId) == 4 ? "Completo" :
                    Pagos.Where(P => P.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault() != null ? "No es Idioma" : "Tiene" : "Generar");

                if (Resultados[0].ToString() == "Generar") { Resultados.Add(PeriodoSiguiente.Descripcion); }

                return Resultados;
            }
        }
        public static DTOPeriodo PeridoSiguiente(Periodo objPeriodo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                Periodo objPeriodo2;

                int PeriodoId = objPeriodo.PeriodoId == 3 ? 1 : objPeriodo.PeriodoId + 1,
                    Anio = objPeriodo.PeriodoId == 3 ? objPeriodo.Anio + 1 : objPeriodo.Anio;
                objPeriodo2 = db.Periodo.Where(P => P.PeriodoId == PeriodoId && Anio == P.Anio).FirstOrDefault();

                return new DTOPeriodo
                {
                    PeriodoId = objPeriodo2.PeriodoId,
                    Anio = objPeriodo2.Anio,
                    Descripcion = objPeriodo2.Descripcion
                };

            }

        }
        public static List<DTOMes> EsteMesvsSiguiente(int AlumnoId, int OfertaEducativa, DateTime fhoy)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<DTOMes> lstMeses = new List<DTOMes>();
                    //DateTime fhoy = DateTime.Now;
                    while (lstMeses.Count < 1)
                    {
                        Subperiodo objSubActual = db.Subperiodo.Where(S => S.MesId == fhoy.Month).FirstOrDefault();

                        Periodo objPerActual = db.Periodo.Where(P => fhoy >= P.FechaInicial && fhoy <= P.FechaFinal).FirstOrDefault();
                        DTOPeriodo objPerSig = objSubActual.SubperiodoId == 4 ? PeridoSiguiente(objPerActual) : null;


                        Subperiodo objSubSiguiente = db.Subperiodo.Where(S => S.MesId == fhoy.Month + 1).FirstOrDefault();

                        Pago objPagActual = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.OfertaEducativaId == OfertaEducativa && P.PeriodoId == objPerActual.PeriodoId
                            && P.Anio == objPerActual.Anio && P.Cuota1.PagoConceptoId == 807 && objSubActual.SubperiodoId == P.SubperiodoId && P.EstatusId != 2).FirstOrDefault();

                        Pago objPagSig = objSubActual.SubperiodoId != 4 ? db.Pago.Where(P => P.AlumnoId == AlumnoId && P.OfertaEducativaId == OfertaEducativa && P.PeriodoId == objPerActual.PeriodoId
                            && P.Anio == objPerActual.Anio && P.Cuota1.PagoConceptoId == 807 && objSubSiguiente.SubperiodoId == P.SubperiodoId && P.EstatusId != 2).FirstOrDefault() :
                            db.Pago.Where(P => P.AlumnoId == AlumnoId && P.OfertaEducativaId == OfertaEducativa && P.PeriodoId == objPerSig.PeriodoId &&
                                P.Anio == objPerSig.Anio && P.Cuota1.PagoConceptoId == 807 && objSubSiguiente.SubperiodoId == P.SubperiodoId && P.EstatusId != 2).FirstOrDefault();
                        if (objPagActual == null)
                        {
                            lstMeses.Add(new DTOMes
                            {
                                MesId = objSubActual.MesId,
                                Descripcion = objSubActual.Mes.Descripcion
                            });
                        }
                        if (objPagSig == null)
                        {
                            lstMeses.Add(new DTOMes
                            {
                                MesId = objSubSiguiente.MesId,
                                Descripcion = objSubSiguiente.Mes.Descripcion
                            });
                        }
                        fhoy = fhoy.AddMonths(1).AddDays((fhoy.Day * -1) + 1);
                    }

                    return lstMeses;
                }
                catch (Exception e)
                {
                    return new List<DTOMes>
                    { new DTOMes
                        {
                            Descripcion = e.Message,
                            MesId = 0
                        }
                    };
                }
            }
        }
        public static List<Flujo> FlujoReinscripcion(int AlumnoId, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DateTime fHoy = DateTime.Now;
                    Alumno objAlumno = db.Alumno.Where(A => A.AlumnoId == AlumnoId).FirstOrDefault();
                    Periodo objPeriodoSig = db.Periodo.Where(P => P.PeriodoId == 3 && P.Anio == 2016).FirstOrDefault();

                    List<Pago> lstPagos = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.PeriodoId == objPeriodoSig.PeriodoId && P.Anio == objPeriodoSig.Anio && P.EstatusId != 2
                        && (P.Cuota1.PagoConceptoId == 800 || P.Cuota1.PagoConceptoId == 802 || P.Cuota1.PagoConceptoId == 807) && P.SubperiodoId == 1 && P.OfertaEducativaId == OfertaEducativaId).ToList();

                    List<Flujo> lstFlujo = new List<Flujo>{
                       new Flujo{
                           Nombre="",//objAlumno.Nombre +" "+objAlumno.Paterno+" "+objAlumno.Materno,objAlumno.Nombre +" "+objAlumno.Paterno+" "+objAlumno.Materno,
                           Movimiento="Solicitud de Re-Inscripción",
                           Estatus="PENDIENTE"
                       },
                       new Flujo{
                           Nombre="",//objAlumno.Nombre +" "+objAlumno.Paterno+" "+objAlumno.Materno,
                           Movimiento="Pago de Reinscripción",
                           Estatus="PENDIENTE"
                       },
                       new Flujo{
                           Nombre="",//objAlumno.Nombre +" "+objAlumno.Paterno+" "+objAlumno.Materno,
                           Movimiento="Asignación de Grupo o Materias",
                           Estatus="PENDIENTE"
                       },
                       new Flujo{
                           Nombre="",//objAlumno.Nombre +" "+objAlumno.Paterno+" "+objAlumno.Materno,
                           Movimiento="Incluir en listas y asignación de Salón",
                           Estatus="PENDIENTE"
                       }
                    };
                    lstPagos.ForEach(LP =>
                    {
                        if (LP.Cuota1.PagoConceptoId == 802 || LP.Cuota1.PagoConceptoId == 800)
                        {
                            lstFlujo[0].Nombre = objAlumno.Nombre + " " + objAlumno.Paterno + " " + objAlumno.Materno;
                            lstFlujo[0].Fecha = LP.FechaGeneracion.ToString("dd/MM/yyyy", Cultura);
                            lstFlujo[0].Hora = "10:00";
                            lstFlujo[0].Estatus = "OK";
                            if (LP.EstatusId == 14 || LP.EstatusId == 4)
                            {
                                lstFlujo[1].Nombre = objAlumno.Nombre + " " + objAlumno.Paterno + " " + objAlumno.Materno;
                                lstFlujo[1].Fecha = LP.FechaGeneracion.ToString("dd/MM/yyyy", Cultura);
                                lstFlujo[1].Hora = "10:00";
                                lstFlujo[1].Estatus = "OK";
                            }
                        }
                        else if (LP.Cuota1.PagoConceptoId == 807)
                        {
                            lstFlujo[0].Nombre = objAlumno.Nombre + " " + objAlumno.Paterno + " " + objAlumno.Materno;
                            lstFlujo[0].Fecha = LP.FechaGeneracion.ToString("dd/MM/yyyy", Cultura);
                            lstFlujo[0].Hora = "10:00";
                            lstFlujo[0].Estatus = "OK";
                            if (LP.EstatusId == 14 || LP.EstatusId == 4)
                            {
                                lstFlujo[1].Nombre = objAlumno.Nombre + " " + objAlumno.Paterno + " " + objAlumno.Materno;
                                lstFlujo[1].Fecha = LP.FechaGeneracion.ToString("dd/MM/yyyy", Cultura);
                                lstFlujo[1].Hora = "10:00";
                                lstFlujo[1].Estatus = "OK";
                            }
                        }
                    });
                    return lstFlujo;
                }
                catch (Exception ve)
                {
                    return new List<Flujo>{new Flujo{
                    Nombre=ve.Message
                }};
                }
            }
        }
        /// <summary>
        /// Metodo Para Calcular El monto de las referencias tanto para pagar como lo pagado
        /// </summary>
        /// <param name="AlumnoId"></param>
        /// <param name="Anio"></param>
        /// <param name="PeriodoId"></param>
        /// <returns></returns>
        public static List<DTOPagoDetallado> ReferenciasPago(int AlumnoId, int Anio, int PeriodoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    int ofertaid = 0;
                    List<Pago> Pagos = db.Pago.Where(P => P.AlumnoId == AlumnoId
                                                        && P.Anio == Anio
                                                        && P.PeriodoId == PeriodoId
                                                        && P.EstatusId != 2
                                                        && P.EstatusId != 3
                                                        && (P.Cuota1.PagoConceptoId != 1007 && P.Cuota1.PagoConceptoId != 1001)
                                                        && (P.Anio != 2016 || P.PeriodoId != 1 || (P.EstatusId == 14 || P.EstatusId == 4)))
                                            .AsNoTracking().ToList();

                    Pagos = Pagos.OrderBy(P => P.OfertaEducativaId).ToList();
                    Pagos = Pagos.Where(p => p.EstatusId != 2).ToList();

                    List<Pago> Adeudos = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.Anio == 2016 && P.PeriodoId == 1
                        && (P.EstatusId == 1 || P.EstatusId == 13)).AsNoTracking().ToList();
                    List<DTOPagoDetallado> PagosDetalles = new List<DTOPagoDetallado>();
                    List<DTOPagoDetallado> PagosAdeudos = new List<DTOPagoDetallado>();
                    Adeudos.ForEach(Pago =>
                   {
                       decimal total = 0;
                       total = Pago.Promesa - (Pago.Promesa - Pago.Restante);

                       DTOPagoDetallado PagoAdeudo = new DTOPagoDetallado();
                       PagoAdeudo.Concepto = "Adeudo, periodos anteriores";
                       PagoAdeudo.ReferenciaId = int.Parse(Pago.ReferenciaId).ToString();
                       PagoAdeudo.Cargo_Descuento = Pago.Promesa.ToString("C", Cultura);
                       PagoAdeudo.CargoFechaLimite = "";
                       PagoAdeudo.DescuentoXAnticipado = "";
                       PagoAdeudo.CargoMonto = Pago.Promesa.ToString("C", Cultura);
                       PagoAdeudo.BecaAcademica_Pcj = "";
                       PagoAdeudo.BecaOpcional_Monto = "";
                       PagoAdeudo.Total_a_PagarS = total.ToString("C", Cultura);
                       PagoAdeudo.SaldoPagado = total.ToString("C", Cultura);
                       PagoAdeudo.Pagoid = Pago.PagoId;
                       PagoAdeudo.TotalMDescuentoMBecas = Pago.Promesa.ToString("C", Cultura);
                       PagoAdeudo.Adeudo = true;
                       PagoAdeudo.BecaSEP = null;
                       PagoAdeudo.SaldoAdeudo = total;
                       PagoAdeudo.OfertaEducativaId = Pago.OfertaEducativaId;
                       PagoAdeudo.EsSep = 2;
                       PagoAdeudo.OtroDescuento = "";

                       PagosAdeudos.Add(PagoAdeudo);
                       PagosDetalles.Add(PagoAdeudo);
                   });
                    if (Pagos.Count == 0)
                    {
                        decimal Total = 0;
                        PagosDetalles.ForEach(delegate (DTOPagoDetallado Pago)
                        {
                            Total += Pago.SaldoAdeudo;
                        });
                        PagosDetalles[0].TotalPagado = Total.ToString("C", Cultura);
                        return PagosDetalles;
                    }
                    #region Ciclo de Pagos
                    Pagos.ForEach(Pago =>
                    {
                        try
                        {
                            if (ofertaid == 0)
                            {
                                ofertaid = Pago.OfertaEducativaId;
                                PagosDetalles.Add(new DTOPagoDetallado
                                {
                                    BecaAcademica_Monto = "",
                                    BecaAcademica_Pcj = "",
                                    Cargo_Descuento = "",
                                    CargoFechaLimite = "",
                                    CargoMonto = "",
                                    Concepto = Pago.OfertaEducativa.Descripcion,
                                    OfertaEducativaId = Pago.OfertaEducativaId,
                                    DescripcionOferta = "",
                                    ReferenciaId = "",
                                    SaldoPagado = "",
                                    Total_a_PagarS = "",
                                    TotalMDescuentoMBecas = "",
                                    EsSep = 2,
                                    OtroDescuento = ""
                                });
                            }
                            else if (ofertaid != Pago.OfertaEducativaId)
                            {
                                ofertaid = Pago.OfertaEducativaId;
                                PagosDetalles.Add(new DTOPagoDetallado
                                {
                                    BecaAcademica_Monto = "",
                                    BecaAcademica_Pcj = "",
                                    Cargo_Descuento = "",
                                    CargoFechaLimite = "",
                                    CargoMonto = "",
                                    Concepto = Pago.OfertaEducativa.Descripcion,
                                    OfertaEducativaId = Pago.OfertaEducativaId,
                                    DescripcionOferta = "",
                                    ReferenciaId = "",
                                    SaldoPagado = "",
                                    Total_a_PagarS = "",
                                    TotalMDescuentoMBecas = "",
                                    EsSep = 2,
                                    OtroDescuento = ""
                                });
                            }
                            //Adeudos Anteriores
                            if (Pago.Anio == 2016 && Pago.PeriodoId == 1)
                            {
                                DTOPagoDetallado objanterior = new DTOPagoDetallado();

                                objanterior.BecaAcademica_Monto = "";
                                objanterior.BecaAcademica_Pcj = "";
                                objanterior.Cargo_Descuento = "";
                                objanterior.CargoFechaLimite = "";
                                objanterior.CargoMonto = Pago.Promesa.ToString("C", Cultura);
                                objanterior.Concepto = "Adeudo, periodos anteriores";
                                objanterior.OfertaEducativaId = Pago.OfertaEducativaId;
                                objanterior.DescripcionOferta = Pago.OfertaEducativa.Descripcion;
                                objanterior.ReferenciaId = (int.Parse(Pago.ReferenciaId)).ToString();
                                objanterior.SaldoPagado = Pago.Restante.ToString("C", Cultura); ;
                                objanterior.Total_a_PagarS = Pago.Promesa.ToString("C", Cultura);
                                objanterior.TotalMDescuentoMBecas = Pago.Promesa.ToString("C", Cultura);
                                objanterior.EsSep = 2;
                                objanterior.OtroDescuento = "";
                                PagosDetalles.Add(objanterior);
                            }
                            //Pago Normal
                            else
                            {
                                DTOPagoDetallado PagosDetallesAgregar = new DTOPagoDetallado();
                                List<PagoDescuento> PagosAnticipados = Pago.PagoDescuento.Where(PD => PD.Descuento.Descripcion == "Pago Anticipado").ToList();

                                List<AlumnoDescuento> DescuentosAlumno =
                                    db.AlumnoDescuento.Where(A => A.AlumnoId == AlumnoId && A.Anio == Anio && A.PeriodoId == PeriodoId &&
                                       (A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Beca Académica"
                                            && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                        A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Beca SEP"
                                            && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                            A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en inscripción"
                                            && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                            A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en colegiatura"
                                            && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                            A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en examen diagnóstico"
                                            && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                            A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en credencial nuevo ingreso"
                                            && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                            A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en credencial nuevo ingreso, idiomas"
                                            && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||

                                             A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en Adelanto de materia, Licenciatura"
                                            && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                             A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en Adelanto de materia, Maestría"
                                            && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||

                                             A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en Examen global"
                                            && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId)
                                        && A.OfertaEducativaId == Pago.OfertaEducativaId && A.PagoConceptoId == Pago.Cuota1.PagoConceptoId && A.EstatusId == 2).AsNoTracking().ToList();
                                //BecaDeportiva
                                List<AlumnoDescuento> DescuentosBecaDeportiva = db.AlumnoDescuento.Where(A => A.AlumnoId == AlumnoId
                                                                                                        && A.Anio == Anio
                                                                                                        && A.PeriodoId == PeriodoId
                                                                                                        && A.OfertaEducativaId == Pago.OfertaEducativaId
                                                                                                        && A.PagoConceptoId == Pago.Cuota1.PagoConceptoId
                                                                                                        && A.EstatusId == 2
                                                                                                        && A.EsDeportiva == true)
                                                                                                        .AsNoTracking().ToList();

                                decimal Descuento = Pago.PagoDescuento != null ?
                                   PagosAnticipados?.FirstOrDefault()?.Monto ?? 0 : 0;

                                decimal Beca = DescuentosAlumno.Count > 0 ?
                                    Pago.PagoDescuento.Count > 0 ? (from a in Pago.PagoDescuento
                                                                    where DescuentosAlumno.Where(s => s.DescuentoId == a.DescuentoId).ToList().Count > 0
                                                                    select a).FirstOrDefault()?.Monto ?? 0 : 0 : 0;

                                decimal DescuentoBecaDeportiva = DescuentosBecaDeportiva.Count > 0 ?
                                                        Pago.PagoDescuento?.Where(P => P.DescuentoId == DescuentosBecaDeportiva.FirstOrDefault().DescuentoId)?.FirstOrDefault()?.Monto ?? 0
                                                    : 0;
                                int DescuentoSepID = db.Descuento.Where(D => D.Descripcion == "Beca SEP"
                                            && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId)?.FirstOrDefault()?.DescuentoId ?? 0;

                                int DescuentoAcdID = db.Descuento.Where(D => D.Descripcion == "Beca Académica"
                                            && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == Pago.Cuota1.PagoConceptoId)?.FirstOrDefault()?.DescuentoId ?? 0;

                                decimal PromoCasa = Pago.PagoDescuento.Where(O => O.Descuento.Descripcion == "Promoción en Casa").ToList().Sum(O => O.Monto);
                                PagosDetallesAgregar.OtroDescuento = PromoCasa > 0 ? PromoCasa.ToString("C", Cultura) : "";

                                int Comite = DescuentosAlumno.Where(d => d.EsComite == true).ToList().Count > 0 ? 3 : 0;

                                decimal Monto = 0;
                                Monto = Pago.Cuota == 0 ? Pago.Promesa : Pago.Cuota;

                                decimal total = 0;
                                total = ((Monto - Descuento - Beca - DescuentoBecaDeportiva - PromoCasa) - (Pago.Promesa - Pago.Restante));
                                total = total < 0 ? 0 : total;


                                PagosDetallesAgregar.Pagoid = Pago.PagoId;
                                //Concepto
                                PagosDetallesAgregar.Concepto =
                                Pago.Cuota1.PagoConceptoId == 800 ? Pago.Cuota1.PagoConcepto.Descripcion + ", " +
                                    Pago.Subperiodo.Mes.Descripcion + " " + (Pago.PeriodoId == 1 ? (Pago.Anio - 1).ToString() :
                                    Pago.Anio.ToString()) : Pago.Cuota1.PagoConcepto.Descripcion + " " + (
                                    Pago.PagoRecargo1.Count > 0 ?
                                    Pago.PagoRecargo1.Where(P => P.PagoIdRecargo == Pago.PagoId).FirstOrDefault().Pago.Cuota1.PagoConcepto.Descripcion +
                                     (Pago.PagoRecargo1.Where(P => P.PagoIdRecargo == Pago.PagoId).FirstOrDefault().Pago.Cuota1.PagoConceptoId == 800 ?
                                     ", " + Pago.PagoRecargo1.Where(P => P.PagoIdRecargo == Pago.PagoId).FirstOrDefault().Pago.Subperiodo.Mes.Descripcion + " " +
                                     (Pago.PagoRecargo1.Where(P => P.PagoIdRecargo == Pago.PagoId).FirstOrDefault().Pago.PeriodoId == 1 ?
                                     (Pago.PagoRecargo1.Where(P => P.PagoIdRecargo == Pago.PagoId).FirstOrDefault().Pago.Anio - 1).ToString() :
                                     Pago.PagoRecargo1.Where(P => P.PagoIdRecargo == Pago.PagoId).FirstOrDefault().Pago.Anio.ToString()) : "") : "") +
                                     Pago?.PagoDescripcion?.Descripcion;


                                PagosDetallesAgregar.ReferenciaId = int.Parse(Pago.ReferenciaId).ToString();
                                PagosDetallesAgregar.CargoMonto = Pago.Cuota == 0 ? Pago.Promesa.ToString("C", Cultura) : Pago.Cuota.ToString("C", Cultura);
                                PagosDetallesAgregar.CargoFechaLimite = Pago.Cuota1.PagoConceptoId == 800 || Pago.Cuota1.PagoConceptoId == 802 ? Utilities.Fecha.Prorroga((Pago.PeriodoId > 1 ? Pago.Anio : Pago.Anio - 1),
                                    db.Subperiodo.Where(s => s.PeriodoId == Pago.PeriodoId && s.SubperiodoId == Pago.SubperiodoId)
                                    .FirstOrDefault().MesId, true, 5).ToString("dd/MM/yyyy", Cultura) :
                                    Pago.FechaGeneracion.AddDays(5).ToString("dd/MM/yyyy", Cultura);//; Pago.FechaGeneracion.ToString("dd/MM/yyyy", Cultura);

                                PagosDetallesAgregar.DescuentoXAnticipado = Descuento > 0 ? Descuento.ToString("C", Cultura) : " ";

                                PagosDetallesAgregar.Cargo_Descuento = Pago.Cuota == 0 ? (Pago.Promesa - (Descuento > 0 ? Descuento : 0)).ToString("C", Cultura) :
                                    (Pago.Cuota - (Descuento > 0 ? Descuento : 0)).ToString("C", Cultura);

                                PagosDetallesAgregar.BecaAcademica_Pcj = DescuentosAlumno.Count > 0 ? DescuentosAlumno.FirstOrDefault().Monto.ToString() + "%" : " ";

                                PagosDetallesAgregar.BecaAcademica_Monto = DescuentosAlumno.Count > 0 ?
                                    Beca.ToString("C", Cultura) : " ";


                                PagosDetallesAgregar.BecaOpcional_Pcj = DescuentosBecaDeportiva?.FirstOrDefault()?.Monto.ToString() + "%" ?? " ";
                                PagosDetallesAgregar.BecaOpcional_Monto = DescuentosBecaDeportiva.Count > 0 ? DescuentoBecaDeportiva.ToString("C", Cultura) : " ";
                                PagosDetallesAgregar.TotalMDescuentoMBecas = (Monto - Descuento - Beca - DescuentoBecaDeportiva - PromoCasa).ToString("C", Cultura);
                                PagosDetallesAgregar.SaldoPagado = total.ToString("C", Cultura);
                                PagosDetallesAgregar.OfertaEducativaId = Pago.OfertaEducativaId;
                                PagosDetallesAgregar.DescripcionOferta = Pago.OfertaEducativa.Descripcion;


                                PagosDetalles.Add(PagosDetallesAgregar);
                                PagosDetalles[Adeudos.Count > 0 ? 1 : 0].Total_a_Pagar += total;
                                ///////////////////Corregir
                                if (Pago.Cuota1.PagoConceptoId == 800 || Pago.Cuota1.PagoConceptoId == 802)
                                {
                                    PagosDetallesAgregar.EsSep =
                                                    Comite == 3
                                                    ? Comite
                                                    : DescuentosAlumno.Where(d => d.EsSEP).ToList().Count > 0 ? 1
                                                    : 2;
                                    PagosDetalles[0].EsSep = PagosDetallesAgregar.EsSep;
                                    PagosDetalles[0].BecaSEP = PagosDetalles[0].BecaSEP == null ? DescuentosBecaDeportiva.Count > 0 ? "Beca Deportiva" : null : PagosDetalles[0].BecaSEP;

                                }
                            }
                            /////////////////////////
                        }
                        catch
                        {

                        }
                    });
                    #endregion
                    int ofert = PagosDetalles[0].OfertaEducativaId;

                    PagosDetalles[Adeudos.Count > 0 ? 1 : 0].Total_a_Pagar += Adeudos.Count > 0 ? PagosAdeudos.Sum(P => P.SaldoAdeudo) : 0;
                    PagosDetalles[Adeudos.Count > 0 ? 1 : 0].Total_a_PagarS = PagosDetalles[Adeudos.Count > 0 ? 1 : 0].Total_a_Pagar.ToString("C", Cultura);
                    PagosDetalles[0].TotalPagado = PagosDetalles[Adeudos.Count > 0 ? 1 : 0].Total_a_PagarS;
                    PagosDetalles[0].esEmpresa = db.AlumnoInscrito.Where(a =>
                                                a.AlumnoId == AlumnoId &&
                                                a.EsEmpresa == true).ToList().Count > 0 ? true : false;
                    if (PagosDetalles[0].esEmpresa)
                    {
                        var alConf = db.GrupoAlumnoConfiguracion.Where(k =>
                                     k.AlumnoId == AlumnoId && k.EsEspecial == true).ToList();
                        PagosDetalles[0].esEspecial = alConf.Count > 0 ? true : false;
                    }
                    //PagosDetalles[0].BecaSEP = tpBeca == 3 ? "Beca Comite" : "Beca SEP";
                    return PagosDetalles;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static List<DTOPagoDetallado> ConsultaPagosTramites(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    int ofertaid = 0;
                    List<Pago> lstPagos = db.Pago.Where(P => P.AlumnoId == AlumnoId
                          && (P.Anio != 2016 || P.PeriodoId != 1) && P.Cuota1.PagoConcepto.EsTramite == true && P.OfertaEducativa.OfertaEducativaTipoId != 4).AsNoTracking().ToList();
                    lstPagos = lstPagos.OrderBy(P => P.OfertaEducativaId).ToList();


                    List<DTOPagoDetallado> lstPagosD = new List<DTOPagoDetallado>();
                    List<DTOPagoDetallado> lstPagosAdeudos = new List<DTOPagoDetallado>();
                    if (lstPagos.Count == 0)
                    {
                        decimal Total = 0;
                        lstPagosD.ForEach(delegate (DTOPagoDetallado objPago)
                        {
                            Total += objPago.SaldoAdeudo;
                        });
                        lstPagosD[0].TotalPagado = Total.ToString("C", Cultura);
                        return lstPagosD;
                    }
                    #region Ciclo de Pagos
                    lstPagos.ForEach(objPago =>
                    {
                        try
                        {
                            if (ofertaid == 0)
                            {
                                ofertaid = objPago.OfertaEducativaId;
                                lstPagosD.Add(new DTOPagoDetallado
                                {
                                    BecaAcademica_Monto = "",
                                    BecaAcademica_Pcj = "",
                                    Cargo_Descuento = "",
                                    CargoFechaLimite = "",
                                    CargoMonto = "",
                                    Concepto = objPago.OfertaEducativa.Descripcion,
                                    OfertaEducativaId = objPago.OfertaEducativaId,
                                    DescripcionOferta = "",
                                    ReferenciaId = "",
                                    SaldoPagado = "",
                                    Total_a_PagarS = "",
                                    TotalMDescuentoMBecas = "",
                                    EsSep = 2,
                                    OtroDescuento = "",
                                    Usuario = "",
                                    Estatus = ""

                                });
                            }
                            else if (ofertaid != objPago.OfertaEducativaId)
                            {
                                ofertaid = objPago.OfertaEducativaId;
                                lstPagosD.Add(new DTOPagoDetallado
                                {
                                    BecaAcademica_Monto = "",
                                    BecaAcademica_Pcj = "",
                                    Cargo_Descuento = "",
                                    CargoFechaLimite = "",
                                    CargoMonto = "",
                                    Concepto = objPago.OfertaEducativa.Descripcion,
                                    OfertaEducativaId = objPago.OfertaEducativaId,
                                    DescripcionOferta = "",
                                    ReferenciaId = "",
                                    SaldoPagado = "",
                                    Total_a_PagarS = "",
                                    TotalMDescuentoMBecas = "",
                                    EsSep = 2,
                                    OtroDescuento = "",
                                    Usuario = "",
                                    Estatus = ""
                                });
                            }
                            DTOPagoDetallado objPagoAdd = new DTOPagoDetallado();
                            List<PagoDescuento> listAnticipados = objPago.PagoDescuento.Where(PD => PD.Descuento.Descripcion == "Pago Anticipado").ToList();

                            List<AlumnoDescuento> lstAlDescuentos =
                                db.AlumnoDescuento.Where(A => A.AlumnoId == AlumnoId &&
                                   (A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Beca Académica"
                                        && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                    A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Beca SEP"
                                        && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                        A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en inscripción"
                                        && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                        A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en colegiatura"
                                        && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                        A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en examen diagnóstico"
                                        && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                        A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en credencial nuevo ingreso"
                                        && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId ||
                                        A.DescuentoId == db.Descuento.Where(D => D.Descripcion == "Descuento en credencial nuevo ingreso, idiomas"
                                        && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId)
                                    && A.OfertaEducativaId == objPago.OfertaEducativaId && A.PagoConceptoId == objPago.Cuota1.PagoConceptoId && A.EstatusId == 2).AsNoTracking().ToList();
                            //BecaDeportiva
                            List<AlumnoDescuento> lstAlDescuentos3 = db.AlumnoDescuento.Where(A => A.AlumnoId == AlumnoId
                                                                                                    && A.OfertaEducativaId == objPago.OfertaEducativaId
                                                                                                    && A.PagoConceptoId == objPago.Cuota1.PagoConceptoId
                                                                                                    && A.EstatusId == 2
                                                                                                    && A.EsDeportiva == true)
                                                                                                    .AsNoTracking().ToList();

                            decimal Descuento = objPago.PagoDescuento != null ?
                               listAnticipados.Count > 0 ?
                               listAnticipados.FirstOrDefault().Monto : 0 : 0;

                            decimal Beca = lstAlDescuentos.Count > 0 ?
                                objPago.PagoDescuento.Count > 0 ? (from a in objPago.PagoDescuento
                                                                   where lstAlDescuentos.Where(s => s.DescuentoId == a.DescuentoId).ToList().Count > 0
                                                                   select a).FirstOrDefault().Monto : 0 : 0;

                            decimal Beca3 = lstAlDescuentos3.Count > 0 ?
                                                objPago.PagoDescuento.Count > 0 ?
                                                    objPago.PagoDescuento.Where(P => P.DescuentoId == lstAlDescuentos3.FirstOrDefault().DescuentoId).FirstOrDefault().Monto
                                                    : 0
                                                : 0;
                            int DescuentoSepID = db.Descuento.Where(D => D.Descripcion == "Beca SEP"
                                          && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).ToList().Count > 0 ?
                            db.Descuento.Where(D => D.Descripcion == "Beca SEP"
                                        && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId : 0;

                            int DescuentoAcdID = db.Descuento.Where(D => D.Descripcion == "Beca Académica"
                                          && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).ToList().Count > 0 ?
                            db.Descuento.Where(D => D.Descripcion == "Beca Académica"
                                        && D.OfertaEducativaId == ofertaid && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).FirstOrDefault().DescuentoId : 0;

                            int Comite = lstAlDescuentos.Where(d => d.EsComite == true).ToList().Count > 0 ? 3 : 0;

                            decimal Monto = 0;
                            Monto = objPago.Cuota == 0 ? objPago.Promesa : objPago.Cuota;

                            decimal total = 0;
                            total = ((Monto - Descuento - Beca - Beca3) - (objPago.Promesa - objPago.Restante));
                            total = total < 0 ? 0 : total;


                            objPagoAdd.Pagoid = objPago.PagoId;
                            //Concepto
                            objPagoAdd.Concepto = objPago.Cuota1.PagoConceptoId == 800 ? objPago.Cuota1.PagoConcepto.Descripcion + ", " +
                                objPago.Subperiodo.Mes.Descripcion + " " + (objPago.PeriodoId == 1 ? (objPago.Anio - 1).ToString() :
                                objPago.Anio.ToString()) : objPago.Cuota1.PagoConcepto.Descripcion + " " + (
                                objPago.PagoRecargo1.Count > 0 ?
                                objPago.PagoRecargo1.Where(P => P.PagoIdRecargo == objPago.PagoId).FirstOrDefault().Pago.Cuota1.PagoConcepto.Descripcion +
                                 (objPago.PagoRecargo1.Where(P => P.PagoIdRecargo == objPago.PagoId).FirstOrDefault().Pago.Cuota1.PagoConceptoId == 800 ?
                                 ", " + objPago.PagoRecargo1.Where(P => P.PagoIdRecargo == objPago.PagoId).FirstOrDefault().Pago.Subperiodo.Mes.Descripcion + " " +
                                 (objPago.PagoRecargo1.Where(P => P.PagoIdRecargo == objPago.PagoId).FirstOrDefault().Pago.PeriodoId == 1 ?
                                 (objPago.PagoRecargo1.Where(P => P.PagoIdRecargo == objPago.PagoId).FirstOrDefault().Pago.Anio - 1).ToString() :
                                 objPago.PagoRecargo1.Where(P => P.PagoIdRecargo == objPago.PagoId).FirstOrDefault().Pago.Anio.ToString()) : "") : "");


                            objPagoAdd.ReferenciaId = int.Parse(objPago.ReferenciaId).ToString();
                            objPagoAdd.CargoMonto = objPago.Cuota == 0 ? objPago.Promesa.ToString("C", Cultura) : objPago.Cuota.ToString("C", Cultura);
                            objPagoAdd.CargoFechaLimite = objPago.Cuota1.PagoConceptoId == 800 || objPago.Cuota1.PagoConceptoId == 802 ? Utilities.Fecha.Prorroga(objPago.Anio,
                                db.Subperiodo.Where(s => s.PeriodoId == objPago.PeriodoId && s.SubperiodoId == objPago.SubperiodoId)
                                .FirstOrDefault().MesId, true, 5).ToString("dd/MM/yyyy", Cultura) :
                                objPago.FechaGeneracion.AddDays(5).ToString("dd/MM/yyyy", Cultura);//; objPago.FechaGeneracion.ToString("dd/MM/yyyy", Cultura);

                            objPagoAdd.DescuentoXAnticipado = Descuento > 0 ? Descuento.ToString("C", Cultura) : " ";

                            objPagoAdd.Cargo_Descuento = objPago.Cuota == 0 ? (objPago.Promesa - (Descuento > 0 ? Descuento : 0)).ToString("C", Cultura) :
                                (objPago.Cuota - (Descuento > 0 ? Descuento : 0)).ToString("C", Cultura);

                            objPagoAdd.BecaAcademica_Pcj = lstAlDescuentos.Count > 0 ? lstAlDescuentos.FirstOrDefault().Monto.ToString() + "%" : " ";

                            objPagoAdd.BecaAcademica_Monto = lstAlDescuentos.Count > 0 ?
                                Beca.ToString("C", Cultura) : " ";
                            decimal PromoCasa = objPago.PagoDescuento.Where(O => O.Descuento.Descripcion == "Promoción en Casa").ToList().Sum(O => O.Monto);
                            objPagoAdd.OtroDescuento = PromoCasa > 0 ? PromoCasa.ToString("C", Cultura) : "";

                            objPagoAdd.BecaOpcional_Pcj = lstAlDescuentos3.Count > 0 ? lstAlDescuentos3.FirstOrDefault().Monto.ToString() + "%" : " ";
                            objPagoAdd.BecaOpcional_Monto = lstAlDescuentos3.Count > 0 ? Beca3.ToString("C", Cultura) : " ";
                            objPagoAdd.TotalMDescuentoMBecas = (Monto - Descuento - Beca - Beca3 - PromoCasa).ToString("C", Cultura);
                            objPagoAdd.SaldoPagado = total.ToString("C", Cultura);
                            objPagoAdd.OfertaEducativaId = objPago.OfertaEducativaId;
                            objPagoAdd.DescripcionOferta = objPago.OfertaEducativa.Descripcion;
                            objPagoAdd.Periodo = db.Periodo.Where(a => a.Anio == objPago.Anio && a.PeriodoId == objPago.PeriodoId).FirstOrDefault().Descripcion;
                            objPagoAdd.Pagado = (objPago.Promesa - objPago.Restante).ToString("C", Cultura);
                            objPagoAdd.Usuario = objPago.UsuarioTipoId == 2
                                                 ? db.Alumno.Where(alu => alu.AlumnoId == objPago.UsuarioId).Select(sel => sel.Nombre + " " + sel.Paterno).FirstOrDefault()
                                                 : db.Usuario.Where(alu => alu.UsuarioId == objPago.UsuarioId).Select(sel => sel.Nombre + " " + sel.Paterno).FirstOrDefault();
                            objPagoAdd.Estatus = objPago.Estatus.EstatusId == 2 ? "Cancelado" : objPago.Estatus.Descripcion;

                            lstPagosD.Add(objPagoAdd);
                            lstPagosD[0].Total_a_Pagar += total;
                            ///////////////////Corregir
                            if (objPago.Cuota1.PagoConceptoId == 800 || objPago.Cuota1.PagoConceptoId == 802)
                            {
                                objPagoAdd.EsSep =
                                                Comite == 3
                                                ? Comite
                                                : lstAlDescuentos.Where(d => d.EsSEP).ToList().Count > 0 ? 1
                                                : 2;
                                lstPagosD[0].EsSep = objPagoAdd.EsSep;
                                lstPagosD[0].BecaSEP = lstPagosD[0].BecaSEP == null ? lstAlDescuentos3.Count > 0 ? "Beca Deportiva" : null : lstPagosD[0].BecaSEP;

                            }
                            /////////////////////////
                        }
                        catch
                        {

                        }
                    });
                    #endregion
                    int ofert = lstPagosD[0].OfertaEducativaId;

                    lstPagosD[0].Total_a_Pagar += 0;
                    lstPagosD[0].Total_a_PagarS = lstPagosD[0].Total_a_Pagar.ToString("C", Cultura);
                    lstPagosD[0].TotalPagado = lstPagosD[0].Total_a_PagarS;
                    lstPagosD[0].esEmpresa = db.AlumnoInscrito.Where(a =>
                                                a.AlumnoId == AlumnoId &&
                                                a.EsEmpresa == true).ToList().Count > 0 ? true : false;
                    if (lstPagosD[0].esEmpresa)
                    {
                        var alConf = db.GrupoAlumnoConfiguracion.Where(k =>
                                     k.AlumnoId == AlumnoId && k.EsEspecial == true).ToList();
                        lstPagosD[0].esEspecial = alConf.Count > 0 ? true : false;
                    }
                    //lstPagosD[0].BecaSEP = tpBeca == 3 ? "Beca Comite" : "Beca SEP";
                    return lstPagosD;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static List<DTOPagos> ConsultarReferenciasConceptos2(int AlumnoId, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DTOCuota CuotaAnterior;
                    DateTime FechaActual = DateTime.Now;
                    List<Pago> Pagos = db.Pago.Where(p => p.AlumnoId == AlumnoId
                    && p.OfertaEducativaId == OfertaEducativaId
                    && p.EstatusId != 2
                    ).ToList();


                    Pagos.ForEach(a =>
                    {
                        if (a.PagoParcial.Count > 1)
                        {
                            Pagos.Remove(a);
                        }
                    });

                    List<DTOPagos> PagosDTO = new List<DTOPagos>();
                    Pagos.ForEach(p =>
                    {

                        PagosDTO.Add(new DTOPagos
                        {
                            PagoId = p.PagoId,
                            AlumnoId = p.AlumnoId,
                            Anio = p.Anio,
                            PeriodoId = p.PeriodoId,
                            DTOPeriodo = new DTOPeriodo
                            {
                                PeriodoId = p.Periodo.PeriodoId,
                                Anio = p.Periodo.Anio,
                                Descripcion = p.Periodo.Descripcion,
                                FechaInicial = p.Periodo.FechaInicial,
                                FechaFinal = p.Periodo.FechaFinal,
                                Meses = p.Periodo.Meses
                            },
                            SubperiodoId = p.SubperiodoId,
                            DTOSubPeriodo = new DTOSubPeriodo
                            {
                                SubperiodoId = p.Subperiodo.SubperiodoId,
                                PeriodoId = p.Subperiodo.PeriodoId,
                                MesId = p.Subperiodo.MesId,
                                Mes = (from c in db.Mes
                                       where c.MesId == p.Subperiodo.MesId
                                       select new DTOMes
                                       {
                                           Descripcion = c.Descripcion,
                                           MesId = c.MesId,
                                       }).FirstOrDefault()
                            },
                            OfertaEducativaId = p.OfertaEducativaId,
                            OfertaEducativa = new DTOOfertaEducativa
                            {
                                OfertaEducativaId = p.OfertaEducativa.OfertaEducativaId,
                                OfertaEducativaTipoId = p.OfertaEducativa.OfertaEducativaTipoId,
                                Descripcion = p.OfertaEducativa.Descripcion,
                                DTOOfertaEducativaTipo = new DTOOfertaEducativaTipo
                                {
                                    Descripcion = p.OfertaEducativa.OfertaEducativaTipo.Descripcion,
                                    OfertaEducativaTipoId = p.OfertaEducativa.OfertaEducativaTipo.OfertaEducativaTipoId
                                }
                            },
                            FechaGeneracion = p.FechaGeneracion,
                            CuotaId = p.CuotaId,
                            Cuota = p.Cuota,
                            FechaPago = (DateTime)new DateTime(p.Anio, p.Subperiodo.MesId, 01),
                            DTOCuota = new DTOCuota
                            {
                                CuotaId = p.Cuota1.CuotaId,
                                Anio = p.Cuota1.Anio,
                                PeriodoId = p.Cuota1.PeriodoId,
                                OfertaEducativaId = p.Cuota1.OfertaEducativaId,
                                PagoConceptoId = p.Cuota1.PagoConceptoId,
                                Monto = p.Cuota1.Monto,
                                EsEmpresa = p.Cuota1.EsEmpresa,
                                DTOPagoConcepto = new DTOPagoConcepto
                                {
                                    CuentaContable = p.Cuota1.PagoConcepto.CuentaContable,
                                    Descripcion = p.Cuota1.PagoConceptoId == 800 ? (p.Cuota1.PagoConcepto.Descripcion +
                                                                                      " " + p.Subperiodo.Mes.Descripcion +
                                                                                      " " + p.Periodo.FechaInicial.Year.ToString())
                                                                                      : p.Cuota1.PagoConcepto.Descripcion
                                            + " " + (p.Cuota1.PagoConceptoId == 306 ?
                                            p.PagoRecargo1.FirstOrDefault().Pago.Cuota1.PagoConcepto.Descripcion
                                            + " " + p.PagoRecargo1.FirstOrDefault().Pago.Subperiodo.Mes.Descripcion + " " +
                                            p.PagoRecargo1.FirstOrDefault().Pago.Periodo.FechaInicial.Year.ToString()
                                            : ""),
                                    EstatusId = p.Cuota1.PagoConcepto.EstatusId,
                                    OfertaEducativaId = p.Cuota1.PagoConcepto.OfertaEducativaId,
                                    PagoConceptoId = p.Cuota1.PagoConcepto.PagoConceptoId
                                }
                            },
                            Promesa = p.Promesa,
                            Pagado = (p.Promesa - p.Restante).ToString(),
                            Restante = p.Restante.ToString(),
                            Referencia = p.ReferenciaId,
                            EstatusId = p.EstatusId,
                        });
                    });

                    PagosDTO.ForEach(Pago =>
                   {
                       Pago.FechaGeneracionS = Pago.FechaGeneracion.ToString("dd/MM/yyyy", Cultura);
                       Pago.Cancelable = Pago.FechaGeneracion.ToShortDateString() == FechaActual.ToShortDateString() ? true : false;
                       Pago.objNormal = new Pagos_Detalles
                       {
                           FechaLimite = (Utilities.Fecha.Prorroga(Pago.FechaPago.Value.Year, Pago.FechaPago.Value.Month, true, 5).ToString("dd/MM/yyyy", Cultura)),
                           Monto = Pago.Promesa.ToString("C", Cultura),
                           Restante = decimal.Parse(Pago.Pagado).ToString("C", Cultura)
                       };
                       if (Pago.Anio == 2016 && Pago.PeriodoId == 1)
                       {
                           Pago.DTOCuota.PeridoAnio = "";
                           CuotaAnterior = null;
                       }
                       else
                       {
                           Pago.DTOCuota.PeridoAnio = Pago.DTOCuota.Anio.ToString() + "-" + Pago.DTOCuota.PeriodoId.ToString();
                           int Anio, Periodo, Oferta, Concepto;
                           Anio = (Pago.DTOPeriodo.PeriodoId == 4 ? Pago.DTOPeriodo.Anio - 1 : Pago.DTOPeriodo.Anio);
                           Periodo = (Pago.DTOPeriodo.PeriodoId == 1 ? 4 : Pago.DTOPeriodo.PeriodoId - 1);
                           Oferta = Pago.OfertaEducativaId;
                           Concepto = Pago.DTOCuota.PagoConceptoId;
                           CuotaAnterior = (from a in db.Cuota
                                            where a.Anio == Anio && a.PeriodoId == Periodo
                                            && a.OfertaEducativaId == Oferta && a.PagoConceptoId == Concepto
                                            select new DTOCuota
                                            {
                                                CuotaId = a.CuotaId,
                                                Anio = a.Anio,
                                                PeriodoId = a.PeriodoId,
                                                OfertaEducativaId = a.OfertaEducativaId,
                                                PagoConceptoId = a.PagoConceptoId,
                                                Monto = a.Monto,
                                                EsEmpresa = a.EsEmpresa
                                            }).FirstOrDefault();
                       }
                       Pago.lstPagoDescuento = (from pd in db.PagoDescuento
                                                join ad in db.AlumnoDescuento on pd.DescuentoId equals ad.DescuentoId
                                                where pd.PagoId == Pago.PagoId && ad.AlumnoId == AlumnoId
                                                select new DTOPagoDescuento
                                                {
                                                    DescuentoId = pd.DescuentoId,
                                                    Monto = pd.Monto,
                                                    PagoId = pd.PagoId,
                                                    DTOAlumnDes = new DTOAlumnoDescuento
                                                    {
                                                        AlumnoId = ad.AlumnoId,
                                                        Anio = ad.Anio,
                                                        ConceptoId = ad.PagoConceptoId,
                                                        DescuentoId = ad.DescuentoId,
                                                        EstatusId = ad.EstatusId,
                                                        Monto = ad.Monto,
                                                        SMonto = ad.Monto.ToString() + "%",
                                                        OfertaEducativaId = ad.OfertaEducativaId,
                                                        PeriodoId = ad.PeriodoId
                                                    }
                                                }).ToList();
                       if (Pago.lstPagoDescuento.Count == 0)
                       {
                           Pago.lstPagoDescuento = new List<DTOPagoDescuento>{
                                new DTOPagoDescuento{
                                    DTOAlumnDes= new DTOAlumnoDescuento{
                                    Monto=0,
                                    SMonto=Pago.Anio == 2016 && Pago.PeriodoId == 1? "":"0%"
                                    }
                                }
                           };
                       }


                   });

                    PagosDTO.ForEach(i =>
                    {
                        i.objNormal.Estatus = i.Promesa == decimal.Parse(i.Restante)
                                            ? "Pendiente" : (i.Restante == "0.00" ? "Pagado" : "Parcialmente Pagado");

                    });

                    return PagosDTO;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static DTOPagos ObtenerPago(int PagoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTOPagos objPago = (from p in db.Pago
                                    where p.PagoId == PagoId
                                    select new DTOPagos
                                    {
                                        AlumnoId = p.AlumnoId,
                                        Anio = p.Anio,
                                        Referencia = p.ReferenciaId,
                                        Promesa = p.Promesa,
                                        FechaGeneracion = p.FechaGeneracion,
                                        DTOCuota = new DTOCuota
                                        {
                                            CuotaId = p.Cuota1.CuotaId,
                                            Anio = p.Cuota1.Anio,
                                            PeriodoId = p.Cuota1.PeriodoId,
                                            OfertaEducativaId = p.Cuota1.OfertaEducativaId,
                                            PagoConceptoId = p.Cuota1.PagoConceptoId,
                                            Monto = p.Cuota1.Monto,
                                            EsEmpresa = p.Cuota1.EsEmpresa,
                                            DTOPagoConcepto = new DTOPagoConcepto
                                            {
                                                CuentaContable = p.Cuota1.PagoConcepto.CuentaContable,
                                                Descripcion = p.Cuota1.PagoConcepto.Descripcion,
                                                EstatusId = p.Cuota1.PagoConcepto.EstatusId,
                                                OfertaEducativaId = p.Cuota1.PagoConcepto.OfertaEducativaId,
                                                PagoConceptoId = p.Cuota1.PagoConcepto.PagoConceptoId
                                            }
                                        }
                                    }).FirstOrDefault();

                objPago.objNormal = new Pagos_Detalles
                {
                    Monto = objPago.Promesa.ToString("C", Cultura)
                };
                return objPago;
            }
        }

        public static DTOPagos GenerarPago(int AlumnoId, int OfertaEducativaId, int PagoConceptoId, int Cuotaid, decimal Monto)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                try
                {
                    DTOPagos dtoPago;
                    DTOAlumnoInscrito objAlumno = (from a in db.AlumnoInscrito
                                                   where a.AlumnoId == AlumnoId && a.OfertaEducativaId == OfertaEducativaId
                                                   select new DTOAlumnoInscrito
                                                   {
                                                       AlumnoId = a.AlumnoId,
                                                       Anio = a.Anio,
                                                       EsEmpresa = a.EsEmpresa,
                                                       FechaInscripcion = a.FechaInscripcion,
                                                       OfertaEducativaId = a.OfertaEducativaId,
                                                       PagoPlanId = a.PagoPlanId,
                                                       PeriodoId = a.PeriodoId,
                                                       TurnoId = a.TurnoId,
                                                       UsuarioId = a.UsuarioId,
                                                       OfertaEducativa = new DTOOfertaEducativa
                                                       {
                                                           OfertaEducativaId = a.OfertaEducativa.OfertaEducativaId,
                                                           OfertaEducativaTipoId = a.OfertaEducativa.OfertaEducativaTipoId,
                                                           Descripcion = a.OfertaEducativa.Descripcion,
                                                           Rvoe = a.OfertaEducativa.Rvoe
                                                       }
                                                   }).FirstOrDefault();
                    DTOPeriodo objPeriodo = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);

                    DTODescuentos objDescuentoConcepto = BLLDescuentos.obtenerDescuentos(OfertaEducativaId, PagoConceptoId);
                    DTOCuota objCuota = BLLCuota.TraerCuota(Cuotaid);
                    DTOAlumnoDescuento objAlumnoDescuento = objDescuentoConcepto != null ? BLLAlumnoDescuento.TraerDescuentoAlumno(AlumnoId, OfertaEducativaId, PagoConceptoId, objDescuentoConcepto.DescuentoId, objPeriodo.Anio, objPeriodo.PeriodoId) : null;
                    //objAlumnoDescuento = objAlumnoDescuento.EstatusId == 2 ? null : objAlumnoDescuento;
                    db.Pago.Add(new Pago
                    {
                        AlumnoId = AlumnoId,
                        Anio = objPeriodo.Anio,
                        PeriodoId = objPeriodo.PeriodoId,
                        SubperiodoId = objPeriodo.SubPeriodoId,
                        OfertaEducativaId = objAlumno.OfertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = objCuota.CuotaId,
                        Cuota = Monto,
                        Promesa = objAlumnoDescuento != null ? Math.Round(Monto - ((objAlumnoDescuento.Monto / 100) * objCuota.Monto)) : Monto,
                        EstatusId = 1,
                        ReferenciaId = "",
                        UsuarioId = AlumnoId,
                        UsuarioTipoId = 2,
                        HoraGeneracion = DateTime.Now.TimeOfDay
                    });
                    db.SaveChanges();
                    Pago objPago = db.Pago.Local[0];
                    objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();

                    if (objAlumnoDescuento != null)
                    {
                        db.PagoDescuento.Add(new PagoDescuento
                        {
                            DescuentoId = objAlumnoDescuento.DescuentoId,
                            PagoId = db.Pago.Local[0].PagoId,
                            Monto = Math.Round((objAlumnoDescuento.Monto / 100) * Monto)
                        });
                        AlumnoDescuento objDes = db.AlumnoDescuento.Where(AD => AD.AlumnoDescuentoId == objAlumnoDescuento.AlumnoDescuentoId).FirstOrDefault();
                        objDes.FechaAplicacion = DateTime.Now;
                        objDes.EstatusId = 2;
                    }

                    db.SaveChanges();
                    int pagoid = db.Pago.Local[0].PagoId;

                    dtoPago = (from p in db.Pago
                               where p.PagoId == pagoid
                               select new DTOPagos
                               {
                                   PagoId = p.PagoId,
                                   AlumnoId = p.AlumnoId,
                                   Anio = p.Anio,
                                   Referencia = p.ReferenciaId,
                                   Promesa = p.Promesa,
                                   FechaGeneracion = p.FechaGeneracion,
                                   DTOCuota = new DTOCuota
                                   {
                                       CuotaId = p.Cuota1.CuotaId,
                                       Anio = p.Cuota1.Anio,
                                       PeriodoId = p.Cuota1.PeriodoId,
                                       OfertaEducativaId = p.Cuota1.OfertaEducativaId,
                                       PagoConceptoId = p.Cuota1.PagoConceptoId,
                                       Monto = p.Cuota1.Monto,
                                       EsEmpresa = p.Cuota1.EsEmpresa,
                                       DTOPagoConcepto = new DTOPagoConcepto
                                       {
                                           CuentaContable = p.Cuota1.PagoConcepto.CuentaContable,
                                           Descripcion = p.Cuota1.PagoConcepto.Descripcion,
                                           EstatusId = p.Cuota1.PagoConcepto.EstatusId,
                                           OfertaEducativaId = p.Cuota1.PagoConcepto.OfertaEducativaId,
                                           PagoConceptoId = p.Cuota1.PagoConcepto.PagoConceptoId
                                       }
                                   },
                               }).FirstOrDefault();
                    dtoPago.objNormal = new Pagos_Detalles
                    {
                        FechaLimite = DateTime.ParseExact(dtoPago.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura), "dd/MM/yyyy", Cultura).ToString("dd/MM/yyyy", Cultura),
                        Monto = dtoPago.Promesa.ToString("C", Cultura)
                    };
                    dtoPago.lstPagoDescuento = (from pd in db.PagoDescuento
                                                join ad in db.AlumnoDescuento on pd.DescuentoId equals ad.DescuentoId
                                                where pd.PagoId == dtoPago.PagoId && ad.AlumnoId == AlumnoId
                                                select new DTOPagoDescuento
                                                {
                                                    DescuentoId = pd.DescuentoId,
                                                    Monto = pd.Monto,
                                                    PagoId = pd.PagoId,
                                                    DTOAlumnDes = new DTOAlumnoDescuento
                                                    {
                                                        AlumnoId = ad.AlumnoId,
                                                        Anio = ad.Anio,
                                                        ConceptoId = ad.PagoConceptoId,
                                                        DescuentoId = ad.DescuentoId,
                                                        EstatusId = ad.EstatusId,
                                                        Monto = ad.Monto,
                                                        SMonto = ad.Monto.ToString() + "%",
                                                        OfertaEducativaId = ad.OfertaEducativaId,
                                                        PeriodoId = ad.PeriodoId
                                                    }
                                                }).ToList();
                    try
                    {
                        BLLAdeudoBitacora.GuardarAdeudo(new DTOAlumnoReferenciaBitacora
                        {
                            AlumnoId = AlumnoId,
                            OfertaEducativaId = OfertaEducativaId,
                            PagoConceptoId = PagoConceptoId,
                            Anio = objPeriodo.Anio,
                            PeriodoId = objPeriodo.PeriodoId,
                            PagoId = pagoid
                        });
                    }
                    catch
                    {

                    }
                    return dtoPago;
                }
                catch
                {
                    return null;
                }
            }
        }
        public static DTOPagos GenerarPago(int AlumnoId, int OfertaEducativaId, int PagoConceptoId, int Cuotaid, decimal Monto, int UsuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                try
                {
                    Usuario objUser = db.Usuario.Where(u => u.UsuarioId == UsuarioId).FirstOrDefault();
                    DTOPagos dtoPago;
                    DTOAlumnoInscrito objAlumno = (from a in db.AlumnoInscrito
                                                   where a.AlumnoId == AlumnoId && a.OfertaEducativaId == OfertaEducativaId
                                                   select new DTOAlumnoInscrito
                                                   {
                                                       AlumnoId = a.AlumnoId,
                                                       Anio = a.Anio,
                                                       EsEmpresa = a.EsEmpresa,
                                                       FechaInscripcion = a.FechaInscripcion,
                                                       OfertaEducativaId = a.OfertaEducativaId,
                                                       PagoPlanId = a.PagoPlanId,
                                                       PeriodoId = a.PeriodoId,
                                                       TurnoId = a.TurnoId,
                                                       UsuarioId = a.UsuarioId,
                                                       OfertaEducativa = new DTOOfertaEducativa
                                                       {
                                                           OfertaEducativaId = a.OfertaEducativa.OfertaEducativaId,
                                                           OfertaEducativaTipoId = a.OfertaEducativa.OfertaEducativaTipoId,
                                                           Descripcion = a.OfertaEducativa.Descripcion,
                                                           Rvoe = a.OfertaEducativa.Rvoe
                                                       }
                                                   }).FirstOrDefault();
                    DTOPeriodo objPeriodo = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);

                    DTODescuentos objDescuentoConcepto = BLLDescuentos.obtenerDescuentos(OfertaEducativaId, PagoConceptoId);
                    DTOCuota objCuota = BLLCuota.TraerCuota(Cuotaid);
                    DTOAlumnoDescuento objAlumnoDescuento = objDescuentoConcepto != null ? BLLAlumnoDescuento.TraerDescuentoAlumno(AlumnoId, OfertaEducativaId, PagoConceptoId, objDescuentoConcepto.DescuentoId, objPeriodo.Anio, objPeriodo.PeriodoId) : null;
                    //objAlumnoDescuento = objAlumnoDescuento.EstatusId == 2 ? null : objAlumnoDescuento;
                    db.Pago.Add(new Pago
                    {
                        AlumnoId = AlumnoId,
                        Anio = objPeriodo.Anio,
                        PeriodoId = objPeriodo.PeriodoId,
                        SubperiodoId = objPeriodo.SubPeriodoId,
                        OfertaEducativaId = objAlumno.OfertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = objCuota.CuotaId,
                        Cuota = Monto,
                        Promesa = objAlumnoDescuento != null ? Math.Round(Monto - ((objAlumnoDescuento.Monto / 100) * objCuota.Monto)) : Monto,
                        Restante = objAlumnoDescuento != null ? Math.Round(Monto - ((objAlumnoDescuento.Monto / 100) * objCuota.Monto)) : Monto,
                        EstatusId = 1,
                        ReferenciaId = "",
                        UsuarioId = objUser.UsuarioId,
                        UsuarioTipoId = objUser.UsuarioTipoId,
                        HoraGeneracion = DateTime.Now.TimeOfDay

                    });
                    db.SaveChanges();
                    Pago objPago = db.Pago.Local[0];
                    objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();

                    if (objAlumnoDescuento != null)
                    {
                        db.PagoDescuento.Add(new PagoDescuento
                        {
                            DescuentoId = objAlumnoDescuento.DescuentoId,
                            PagoId = db.Pago.Local[0].PagoId,
                            Monto = Math.Round((objAlumnoDescuento.Monto / 100) * Monto)
                        });
                        AlumnoDescuento objDes = db.AlumnoDescuento.Where(AD => AD.AlumnoDescuentoId == objAlumnoDescuento.AlumnoDescuentoId).FirstOrDefault();
                        objDes.FechaAplicacion = DateTime.Now;
                        objDes.EstatusId = 2;
                    }

                    db.SaveChanges();
                    int pagoid = db.Pago.Local[0].PagoId;

                    dtoPago = (from p in db.Pago
                               where p.PagoId == pagoid
                               select new DTOPagos
                               {
                                   PagoId = p.PagoId,
                                   AlumnoId = p.AlumnoId,
                                   Anio = p.Anio,
                                   Referencia = p.ReferenciaId,
                                   Promesa = p.Promesa,
                                   FechaGeneracion = p.FechaGeneracion,
                                   DTOCuota = new DTOCuota
                                   {
                                       CuotaId = p.Cuota1.CuotaId,
                                       Anio = p.Cuota1.Anio,
                                       PeriodoId = p.Cuota1.PeriodoId,
                                       OfertaEducativaId = p.Cuota1.OfertaEducativaId,
                                       PagoConceptoId = p.Cuota1.PagoConceptoId,
                                       Monto = p.Cuota1.Monto,
                                       EsEmpresa = p.Cuota1.EsEmpresa,
                                       DTOPagoConcepto = new DTOPagoConcepto
                                       {
                                           CuentaContable = p.Cuota1.PagoConcepto.CuentaContable,
                                           Descripcion = p.Cuota1.PagoConcepto.Descripcion,
                                           EstatusId = p.Cuota1.PagoConcepto.EstatusId,
                                           OfertaEducativaId = p.Cuota1.PagoConcepto.OfertaEducativaId,
                                           PagoConceptoId = p.Cuota1.PagoConcepto.PagoConceptoId
                                       }
                                   },
                               }).FirstOrDefault();
                    dtoPago.objNormal = new Pagos_Detalles
                    {
                        FechaLimite = DateTime.ParseExact(dtoPago.FechaGeneracion.AddDays(7).ToString("dd/MM/yyyy", Cultura), "dd/MM/yyyy", Cultura).ToString("dd/MM/yyyy", Cultura),
                        Monto = dtoPago.Promesa.ToString("C", Cultura)
                    };
                    dtoPago.lstPagoDescuento = (from pd in db.PagoDescuento
                                                join ad in db.AlumnoDescuento on pd.DescuentoId equals ad.DescuentoId
                                                where pd.PagoId == dtoPago.PagoId && ad.AlumnoId == AlumnoId
                                                select new DTOPagoDescuento
                                                {
                                                    DescuentoId = pd.DescuentoId,
                                                    Monto = pd.Monto,
                                                    PagoId = pd.PagoId,
                                                    DTOAlumnDes = new DTOAlumnoDescuento
                                                    {
                                                        AlumnoId = ad.AlumnoId,
                                                        Anio = ad.Anio,
                                                        ConceptoId = ad.PagoConceptoId,
                                                        DescuentoId = ad.DescuentoId,
                                                        EstatusId = ad.EstatusId,
                                                        Monto = ad.Monto,
                                                        SMonto = ad.Monto.ToString() + "%",
                                                        OfertaEducativaId = ad.OfertaEducativaId,
                                                        PeriodoId = ad.PeriodoId
                                                    }
                                                }).ToList();
                    try
                    {
                        BLLAdeudoBitacora.GuardarAdeudo(new DTOAlumnoReferenciaBitacora
                        {
                            AlumnoId = AlumnoId,
                            OfertaEducativaId = OfertaEducativaId,
                            PagoConceptoId = PagoConceptoId,
                            Anio = objPeriodo.Anio,
                            PeriodoId = objPeriodo.PeriodoId,
                            PagoId = pagoid
                        });
                    }
                    catch
                    {

                    }
                    return dtoPago;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static string PagoCancelacionSolicitud(int PagoId, string Comentario, int UsuarioId)
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    db.PagoCancelacionSolicitud.Add(new PagoCancelacionSolicitud
                    {
                        PagoId = PagoId,
                        FechaSolicitud = DateTime.Now,
                        HoraSolicitud = DateTime.Now.TimeOfDay,
                        Comentario = Comentario,
                        UsuarioIdSolicitud = UsuarioId,
                        FechaAplicacion = DateTime.Now,
                        HoraAplicacion = DateTime.Now.TimeOfDay,
                        UsuarioIdAutoriza = 0,
                        EstatusId = 1

                    });

                    db.SaveChanges();
                    return "Solicitud Enviada";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }


        }


        public static List<DTOPagoCancelacionSolicitud> ConsultarPagoCancelacionSolicitud(int UsuarioId, int Tipo)
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {


                if (Tipo == 1)
                {
                    var objSolicitudes = (from a in db.PagoCancelacionSolicitud
                                          join b in db.Estatus on a.EstatusId equals b.EstatusId
                                          where a.UsuarioIdSolicitud == UsuarioId
                                          select new DTOPagoCancelacionSolicitud
                                          {
                                              solicitudId = a.SolicitudId,
                                              pagoId = (int)a.PagoId,
                                              fechaSolicitud1 = (DateTime)a.FechaSolicitud,
                                              comentario = a.Comentario,
                                              fechaAplicacion1 = (DateTime)a.FechaAplicacion,
                                              estatusId = b.Descripcion

                                          }).ToList();



                    objSolicitudes = objSolicitudes.Select(b => new DTOPagoCancelacionSolicitud
                    {
                        solicitudId = b.solicitudId,
                        pagoId = int.Parse(db.Pago.Where(c => c.PagoId == b.pagoId).FirstOrDefault().ReferenciaId),
                        fechaSolicitud = b.fechaSolicitud1.ToString("dd/MM/yyyy", Cultura),
                        comentario = b.comentario,
                        fechaAplicacion = b.estatusId == "Activo" ? "N/A" : b.fechaAplicacion1.ToString("dd/MM/yyyy", Cultura),
                        estatusId = b.estatusId

                    }).ToList();

                    return objSolicitudes;
                }
                else
                {
                    var objSolicitudes = (from a in db.PagoCancelacionSolicitud
                                          join b in db.Usuario on a.UsuarioIdSolicitud equals b.UsuarioId
                                          where a.EstatusId == 1
                                          select new DTOPagoCancelacionSolicitud
                                          {
                                              solicitudId = a.SolicitudId,
                                              pagoId = (int)a.PagoId,
                                              comentario = a.Comentario,
                                              fechaSolicitud1 = (DateTime)a.FechaSolicitud,
                                              usuarioIdSolicitud = b.Nombre + " " + b.Paterno + " " + b.Materno
                                          }).ToList();



                    objSolicitudes = objSolicitudes.Select(b => new DTOPagoCancelacionSolicitud
                    {
                        solicitudId = b.solicitudId,
                        pagoId = int.Parse(db.Pago.Where(c => c.PagoId == b.pagoId).FirstOrDefault().ReferenciaId),
                        fechaSolicitud = b.fechaSolicitud1.ToString("dd/MM/yyyy", Cultura),
                        comentario = b.comentario,
                        usuarioIdSolicitud = b.usuarioIdSolicitud
                    }).ToList();

                    return objSolicitudes;
                }


            }

        }


        public static string CambiarPagoCancelacionSolicitud(int SolicitudId, int UsuarioId, int Tipo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {

                    PagoCancelacionSolicitud actualizarSolicitud = db.PagoCancelacionSolicitud.First(a => a.SolicitudId == SolicitudId);

                    //actualizar estatus solicitud
                    actualizarSolicitud.FechaAplicacion = DateTime.Now;
                    actualizarSolicitud.HoraAplicacion = DateTime.Now.TimeOfDay;
                    actualizarSolicitud.EstatusId = Tipo;
                    db.SaveChanges();
                    return "Solicitud Rechazada";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }

        }



        public static string CancelarPago(int PagoId, string Comentario, int UsuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    PagoCancelacion PagoCancelacion = db.PagoCancelacion.Where(a => a.PagoId == PagoId).FirstOrDefault();
                    Pago Pago = db.Pago.Where(P => P.PagoId == PagoId).FirstOrDefault();
                    Pago.Restante = Pago.Promesa;
                    if (PagoCancelacion != null)
                    {
                        Pago.EstatusId = 2;
                        PagoCancelacion.EstatusId = 2;
                    }
                    else
                    {
                        db.PagoCancelacion.Add(new PagoCancelacion
                        {
                            PagoId = PagoId,
                            EstatusId = 2,
                            PagoCancelacionDetalle = new List<PagoCancelacionDetalle> { new PagoCancelacionDetalle
                            {
                                FechaCancelacion = DateTime.Now,
                                HoraCancelacion = DateTime.Now.TimeOfDay,
                                Observaciones = Comentario,
                                PagoId = PagoId,
                                PagoParcialId = null,
                                UsuarioId = UsuarioId
                            } }
                        });

                        Pago.EstatusId = 2;
                        if (Pago.PagoRecargo != null)
                        {
                            List<Pago> lstRecargos = Pago.PagoRecargo.Where(kl => kl.Pago1.EstatusId == 1).ToList().Select(k => k.Pago1).ToList();

                            lstRecargos.ForEach(ok =>
                            {


                                if (ok.Promesa == ok.Restante)
                                {
                                    db.PagoCancelacion.Add(new PagoCancelacion
                                    {
                                        PagoId = ok.PagoId,
                                        EstatusId = 2,
                                        PagoCancelacionDetalle = new List<PagoCancelacionDetalle> { new PagoCancelacionDetalle
                                        {
                                            FechaCancelacion = DateTime.Now,
                                            HoraCancelacion = DateTime.Now.TimeOfDay,
                                            Observaciones = Comentario,
                                            PagoId = ok.PagoId,
                                            PagoParcialId = null,
                                            UsuarioId = UsuarioId
                                        } }
                                    });
                                    ok.EstatusId = 2;
                                }

                            });
                        }
                    }

                    db.SaveChanges();

                    return "Guardado";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }

        public static string[] MesPendiente(int AlumnoId, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    Periodo objPerActual = db.Periodo.Where(d => DateTime.Now >= d.FechaInicial && DateTime.Now <= d.FechaFinal).FirstOrDefault();

                    AlumnoInscrito objInscrito = db.AlumnoInscrito.Where(al =>
                                                        al.AlumnoId == AlumnoId
                                                        && al.OfertaEducativaId == OfertaEducativaId
                                                        && al.Anio == objPerActual.Anio
                                                        && al.PeriodoId == objPerActual.PeriodoId).FirstOrDefault();

                    if (objInscrito != null)
                    {
                        return new string[]
                    {
                        "Siguiente"
                    };

                    }
                    else
                    {
                        return new string[] { "Periodo Actual", objPerActual.Descripcion };
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
        public static string GenerarPagoInscripcion(int AlumnoId, int OfertaEducativa, int Anio, int Periodo, int PagoConceptoId, int SubPEriodo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    Periodo objPer = db.Periodo.Where(p => p.Anio == Anio && p.PeriodoId == Periodo).FirstOrDefault();
                    Cuota objCuota = db.Cuota.Where(c => c.Anio == Anio
                                                    && c.PeriodoId == Periodo
                                                    && c.OfertaEducativaId == OfertaEducativa
                                                    && c.PagoConceptoId == PagoConceptoId).FirstOrDefault();

                    db.Pago.Add(new Pago
                    {
                        AlumnoId = AlumnoId,
                        Anio = objPer.Anio,
                        PeriodoId = objPer.PeriodoId,
                        SubperiodoId = SubPEriodo,
                        OfertaEducativaId = OfertaEducativa,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = objCuota.CuotaId,
                        Cuota = objCuota.Monto,
                        Promesa = objCuota.Monto,
                        Restante = objCuota.Monto,
                        EstatusId = 1,
                        ReferenciaId = "",
                        UsuarioId = AlumnoId,
                        UsuarioTipoId = 2,
                        HoraGeneracion = DateTime.Now.TimeOfDay

                    });

                    db.SaveChanges();
                    Pago objPago = db.Pago.Local[0];
                    objPago.ReferenciaId = db.spGeneraReferencia(objPago.PagoId).FirstOrDefault();
                    db.SaveChanges();
                    return "guardado";
                }
                catch (Exception e)
                { return e.Message; }
            }
        }
    }
}

