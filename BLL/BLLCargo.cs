using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using System.Data.Entity;

namespace BLL
{
    public class BLLCargo
    {
        public static void CancelarTotal(int pagoId, int usuarioId, string observaciones)
        {
            List<DTO.ReciboDatos> recibos = new List<ReciboDatos>();
            List<DAL.Recibo> Recibos = new List<DAL.Recibo>();

            using (DAL.UniversidadEntities db = new DAL.UniversidadEntities())
            {
                var Pagos = db.PagoParcial.Where(n => n.PagoId == pagoId && n.EstatusId == 4).ToList();

                Pagos.ForEach(n =>
                {
                    #region PagoParcial Bitacora

                    db.PagoParcialBitacora.Add(new PagoParcialBitacora
                    {
                        PagoParcialId = n.PagoParcialId,
                        PagoId = n.PagoId,
                        SucursalCajaId = n.SucursalCajaId,
                        ReciboId = n.ReciboId,
                        Pago = n.Pago,
                        FechaPago = n.FechaPago,
                        HoraPago = n.HoraPago,
                        EstatusId = n.EstatusId,
                        TieneMovimientos = n.TieneMovimientos,
                        PagoTipoId = n.PagoTipoId,
                        ReferenciaProcesadaId = n.ReferenciaProcesadaId,
                        FechaBitacora = DateTime.Now.Date,
                        HoraBitacora = DateTime.Now.TimeOfDay,
                        UsuarioId = usuarioId
                    });

                    #endregion PagoParcial Bitacora

                    #region Reclasificacion

                    n.Reclasificacion = new List<Reclasificacion> {
                                                new Reclasificacion {
                                                    UsuarioId = usuarioId,
                                                    FechaReclasificacion = DateTime.Now,
                                                    HoraReclasificacion = DateTime.Now.TimeOfDay,
                                                    Importe = n.Pago,
                                                    ReclasificacionTipoId = 3
                                                }
                                            };

                    #endregion Reclasificacion

                    #region No Caja

                    if (n.PagoTipoId != 1)
                    {
                        n.ReferenciaProcesada.SeGasto = false;
                        n.ReferenciaProcesada.Restante = n.ReferenciaProcesada.Restante + n.Pago;
                        n.ReferenciaProcesada.Importe = n.ReferenciaProcesada.ReferenciaTipoId == 4 ? n.ReferenciaProcesada.Restante : n.ReferenciaProcesada.Importe;
                        n.EstatusId = 2;
                    }

                    #endregion No Caja

                    #region Caja

                    else if (n.PagoTipoId == 1)
                    {
                        n.ReferenciaProcesada.SeGasto = false;
                        n.ReferenciaProcesada.Restante = n.ReferenciaProcesada.Restante + n.Pago;
                        n.EstatusId = 2;
                    }

                    #endregion Caja

                    #region Cancelacion

                    var PagoCancelacion = db.PagoCancelacion.Where(s => s.PagoId == pagoId).FirstOrDefault();

                    if (PagoCancelacion == null)
                    {
                        db.PagoCancelacion.Add(new PagoCancelacion
                        {
                            PagoId = n.PagoId,
                            EstatusId = n.EstatusId,
                            PagoCancelacionDetalle = new List<PagoCancelacionDetalle> {
                                    new PagoCancelacionDetalle {
                                        PagoId = n.PagoId,
                                        PagoParcialId = n.PagoParcialId,
                                        UsuarioId = usuarioId,
                                        FechaCancelacion = DateTime.Now,
                                        HoraCancelacion = DateTime.Now.TimeOfDay,
                                        Observaciones = observaciones
                                    }
                                }
                        });
                    }

                    else
                    {
                        PagoCancelacion.PagoCancelacionDetalle.Add(new PagoCancelacionDetalle
                        {
                            PagoId = n.PagoId,
                            PagoParcialId = n.PagoParcialId,
                            UsuarioId = usuarioId,
                            FechaCancelacion = DateTime.Now,
                            HoraCancelacion = DateTime.Now.TimeOfDay,
                            Observaciones = observaciones
                        });
                    }

                    #endregion Cancelacion

                    db.SaveChanges();
                });

                var Pago = db.Pago.Where(n => n.PagoId == pagoId).FirstOrDefault();
                Pago.EstatusId = 2;
                Pago.Restante = Pago.Promesa;

                db.SaveChanges();
            }
        }
    }
}
