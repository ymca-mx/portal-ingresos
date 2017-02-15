using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Universidad.DAL;

namespace Universidad.BLL
{
    public class BLLPago
    {
        public static DTO.DTOPago ComisionTC(DTO.DTOAlumnoDatosGenerales Datos, int ultimoId, decimal comision)
        {
            ultimoId = PagoId(ultimoId);
            DTO.DTOPeriodo Periodo = BLLVarios.PeriodoActual();

            using (UniversidadEntities db = new UniversidadEntities())
            {
                int idComisionTC = BLLSistemaConfiguracion.ComisionTC();

                return (from a in db.PagoConcepto
                        join b in db.Cuota on new {a.OfertaEducativaId, a.PagoConceptoId} equals new {b.OfertaEducativaId, b.PagoConceptoId}
                        where a.OfertaEducativaId == Datos.ofertaEducativaId
                        //Cazar concepto con la configuración del sistema
                              && a.PagoConceptoId == idComisionTC
                              && b.Anio == Periodo.anio
                              && b.PeriodoId == Periodo.periodoId
               
                        select new DTO.DTOPago { 
                            pagoId = ultimoId,
                            conceptoPago = a.Descripcion,
                            conceptoPagoId = a.PagoConceptoId,
                            //cuota,
                            descuento = 0,
                            importe = comision,
                            estatusId = 1,
                            enUso = true,
                            check = true,
                            cuotaId = b.CuotaId,
                            
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
                        }).FirstOrDefault();
            }
        }

        public static string ImporteLetra(string numero, string moneda, bool centavo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return db.fnImporteLetra(numero, moneda, centavo);
            }
        }

        public static int PagoId(int ultimoId)
        {
            if ((ultimoId - 1) < 0)
                return (ultimoId - 1);
            else
                return 0;
        }

        public static string Total(List<DTO.DTOPago> Pagos)
        {
            return "Total a pagar  $ " + Pagos.Where(Pago => Pago.enUso).Sum(Pago => Pago.importe).ToString();
        }

        public static List<DTO.DTOPagoDescuento> ObtenerDescuentos(List<DTO.DTOPago> Pagos, int pagoId)
        {
            return Pagos.FirstOrDefault(pago => pago.pagoId == pagoId).Descuentos;
        }

        public static List<DTO.DTOPagoMetodo> Metodos()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.PagoMetodo
                        where a.EsVisible == true
                        select new DTO.DTOPagoMetodo
                        {
                            pagoMetodoId = a.PagoMetodoId,
                            descripcion = a.Descripcion,
                            cuentaContable = a.CuentaContable,
                            comision = a.Comision
                        })
                        .OrderBy(metodo => metodo.pagoMetodoId)
                        .ToList();
            }
        }

        public static string GnerarDescuento(int PagoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    Pago objPago = db.Pago.Where(P => P.PagoId == PagoId).FirstOrDefault();
                    Descuento objDesc = db.Descuento.Where(D => D.OfertaEducativaId == objPago.OfertaEducativaId && D.Descripcion == "Pago Anticipado" && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).FirstOrDefault();

                    objPago.Promesa = Math.Round(decimal.Parse((objPago.Promesa * decimal.Parse("0.96")).ToString()));

                    db.PagoDescuento.Add(new PagoDescuento
                    {
                        DescuentoId = objDesc.DescuentoId,
                        Monto = Math.Round(objPago.Cuota - (objPago.Cuota * decimal.Parse("0.04"))),
                        PagoId = objPago.PagoId
                    });
                    
                    db.SaveChanges();
                    ///Regresa Guardado en Succes
                    return "Guardado";
                }
                catch (Exception e)
                {
                    ///Mensaje de la Excepcion
                    return e.Message;
                }
            }

        }
     
        //public static int? Aplicar(ref Utilities.ProcessResult Resultado, DTO.DTOAlumnoDatosGenerales Datos, List<DTO.DTOPagoMetodo> Metodos, List<DTO.DTOPago> Pagos, DTO.DTOLogin Credencial, string observacion)
        //{
        //    List<Pago> Cabecero = new List<Pago>();

        //    //Aquí se guardan los metodos
        //    List<PagoDetalle> Detalles = new List<PagoDetalle>();
            
        //    List<DTO.DTOPago> Auxiliar = Pagos;
        //    int? reciboId;

        //    try
        //    {
        //        using (UniversidadEntities db = new UniversidadEntities())
        //        {
        //            var ParametrosRecibo =
        //                (from a in db.SucursalCaja
        //                 join b in db.Sucursal on a.SucursalId equals b.SucursalId
        //                 join c in db.Caja on a.CajaId equals c.CajaId
        //                 where a.SucursalCajaId == Credencial.sucursalCajaId
        //                 select new
        //                 {
        //                     c.ReciboId,
        //                     b.Serie
        //                 }).ToList().FirstOrDefault();

        //            reciboId = ParametrosRecibo.ReciboId + 1;

        //            var Caja =
        //            (from a in db.SucursalCaja
        //             join b in db.Sucursal on a.SucursalId equals b.SucursalId
        //             join c in db.Caja on a.CajaId equals c.CajaId
        //             where a.SucursalCajaId == Credencial.sucursalCajaId
        //             select c).FirstOrDefault();

        //            Caja.ReciboId = reciboId ?? 0;

        //            //Cabecero
        //            var periodo = (from a in db.Periodo
        //                                     where DateTime.Now >= a.FechaInicial && DateTime.Now <= a.FechaFinal
        //                                     select a).FirstOrDefault();

        //            foreach (DTO.DTOPago Item in Pagos)
        //            {
        //                if (Item.pagoId <= 0)
        //                {
        //                    Cabecero.Add(new Pago
        //                    {
        //                        AlumnoId = Datos.alumnoId,
        //                        Anio = periodo.Anio,
        //                        PeriodoId = periodo.PeriodoId,
        //                        SubperiodoId = (from a in db.Subperiodo
        //                                            where a.PeriodoId == periodo.PeriodoId
        //                                            && a.MesId == DateTime.Now.Month
        //                                            select a.SubperiodoId).FirstOrDefault(),
        //                        OfertaEducativaId = Datos.ofertaEducativaId,
        //                        FechaGeneracion = DateTime.Now,
        //                        CuotaId = Item.cuotaId,
        //                        Cuota = Item.cuota,
        //                        Promesa = Item.importe,
        //                        ReferenciaId = "",
        //                        PagoId = Item.pagoId
        //                    });
        //                }
        //                else
        //                    Cabecero.Add(db.Pago.Where(pago => pago.PagoId == Item.pagoId).FirstOrDefault<Pago>());
        //            }

        //            //Armado de metodos y pagos
        //            foreach (DTO.DTOPago p in Auxiliar)
        //            {
        //                foreach (DTO.DTOPagoMetodo m in Metodos.FindAll(metodo => metodo.importe > 0))
        //                {
        //                    if (m.importe <= p.importe)
        //                    {
        //                        Detalles.Add(new PagoDetalle
        //                        {
        //                            PagoId = p.pagoId,
        //                            PagoMetodoId = m.pagoMetodoId,
        //                            Monto = m.importe,
        //                            ConceptoPagoId = p.conceptoPagoId,
        //                            FechaPago = DateTime.Now,
        //                            HoraPago = DateTime.Now.TimeOfDay
        //                        });

        //                        p.importe = p.importe - m.importe;
        //                        m.importe = m.importe - m.importe;

        //                        if (p.importe == 0)
        //                            break;

        //                        continue;
        //                    }

        //                    else
        //                    {
        //                        Detalles.Add(new PagoDetalle
        //                        {
        //                            PagoId = p.pagoId,
        //                            PagoMetodoId = m.pagoMetodoId,
        //                            Monto = p.importe,
        //                            ConceptoPagoId = p.conceptoPagoId
        //                        });

        //                        m.importe = m.importe - p.importe;
        //                        p.importe = p.importe - p.importe;

        //                        break;
        //                    }
        //                }
        //            }


        //            //Verificar si es un pago parcial o total

                    

        //            Cabecero.ForEach(pago =>
        //            {
        //                ////Parcial
        //                //if (Detalles.Where(z => z.PagoId == pago.PagoId).Sum(x => x.Monto) < pago.Promesa)
        //                //{
        //                //    pago.EstatusId = 13;
        //                //    pago.PagoDetalle = Detalles.FindAll(detalle => detalle.PagoId == pago.PagoId);
        //                //}

        //                ////Total
        //                //else
        //                //{
        //                    pago.SucursalCajaId = Credencial.sucursalCajaId;
        //                    pago.ReciboId = reciboId;
        //                    pago.Serie = ParametrosRecibo.Serie;
        //                    pago.Pago1 = pago.Promesa;
        //                    pago.FechaPago = DateTime.Now;
        //                    pago.HoraPago = DateTime.Now.TimeOfDay;
        //                    pago.EstatusId = 4;
        //                    pago.PagoDetalle = Detalles.FindAll(detalle => detalle.PagoId == pago.PagoId);

        //                    if (pago.PagoId <= 0)
        //                    {
        //                        pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId <= 0 ? 0 : pago.PagoId).FirstOrDefault();

        //                        List<PagoDescuento> Descuentos = new List<PagoDescuento>();

        //                        Pagos.Where(p => p.pagoId == pago.PagoId).ToList().FirstOrDefault().Descuentos.ForEach(descuento =>
        //                        {
        //                            Descuentos.Add(new PagoDescuento
        //                            {
        //                                PagoId = pago.PagoId,
        //                                DescuentoId = descuento.descuentoId,
        //                                Monto = descuento.importe
        //                            });
        //                        });

        //                        pago.PagoDescuento = Descuentos;
        //                        db.Pago.Add(pago);

        //                        Pagos.Where(p => p.pagoId == pago.PagoId && p.Descuentos.Count > 0).ToList().ForEach(a =>
        //                        {
        //                            a.Descuentos.ForEach(s =>
        //                            {
        //                                db.AlumnoDescuento.Where(u => u.AlumnoDescuentoId == s.alumnoDescuentoId).FirstOrDefault().EstatusId = 2;
        //                                db.AlumnoDescuento.Where(p => p.AlumnoDescuentoId == s.alumnoDescuentoId).FirstOrDefault().FechaAplicacion = DateTime.Now;
        //                            });
        //                        });
        //                    }
        //                //}
        //            });

        //            db.Recibo.Add(new Recibo
        //            {
        //                ReciboId = reciboId ?? 0,
        //                SucursalCajaId = Credencial.sucursalCajaId,
        //                Serie = ParametrosRecibo.Serie,
        //                Observaciones = observacion,
        //                Importe = Detalles.Sum(detalle => detalle.Monto),
        //                AlumnoId = Datos.alumnoId,
        //                OfertaEducativaId = Datos.ofertaEducativaId,
        //                FechaGeneracion = DateTime.Now,
        //                HoraGeneracion = DateTime.Now.TimeOfDay,
        //                UsuarioId = Credencial.usuarioId,
        //                EstatusId = 1
        //            });

        //            db.SaveChanges();

        //            //Si son pagos en vuelo
        //            if (Cabecero.Count(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000") > 0)
        //            {
        //                //Generación de referencia
        //                Cabecero.Where(pago => pago.PagoId <= 0 || pago.ReferenciaId == "0000000000").ToList().ForEach(pago =>
        //                {
        //                    pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId).FirstOrDefault();
        //                });

        //                db.SaveChanges();
        //            }
        //        }

        //        return reciboId = reciboId > 0 ? reciboId : null;
        //    }
        //    catch (Exception Ex)
        //    {
        //        Resultado.Estatus = false;
        //        Resultado.Mensaje = Ex.Message;
        //        Resultado.MensajeDetalle = (Ex.InnerException != null) ? Ex.InnerException.InnerException.Message : string.Empty;
        //        Resultado.Informacion = "BLLPago.Aplicar()";
                
        //        return null;
        //    }
        //}
    }
}
