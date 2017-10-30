using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLDonativo
    {
        public static Universidad.DTO.DTOReferenciaDonativo BuscarReferencia(string Dato, int Buscar)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    Universidad.DTO.DTOReferenciaDonativo referencia = new Universidad.DTO.DTOReferenciaDonativo();
                    
                    int s = int.Parse(Dato);
                    Dato = string.Format("{0:0000000000}",s);

                    if (Buscar == 1)
                    {
                        referencia = db.Pago.Where(a => a.ReferenciaId == Dato
                                                                  && a.Cuota1.PagoConceptoId == 1014
                                                                  && a.PagoParcial.FirstOrDefault().ReferenciaProcesada.EstatusId == 1)
                                                           .Select(b => new Universidad.DTO.DTOReferenciaDonativo
                                                           {
                                                               referenciaId = b.ReferenciaId,
                                                               importe = b.PagoParcial.FirstOrDefault().ReferenciaProcesada.Importe,
                                                               fechaPago_ = b.PagoParcial.FirstOrDefault().ReferenciaProcesada.FechaPago
                                                           }).FirstOrDefault();
                    }
                    else if (Buscar == 2)
                    {
                        int recibo = int.Parse(Dato);
                        referencia = db.Pago.Where(a => a.PagoParcial.FirstOrDefault().ReciboId == recibo
                                                          && a.PagoParcial.FirstOrDefault().SucursalCajaId == 8
                                                          && a.Cuota1.PagoConceptoId == 1014
                                                          && a.PagoParcial.FirstOrDefault().ReferenciaProcesada.EstatusId == 1)
                                                   .Select(b => new Universidad.DTO.DTOReferenciaDonativo
                                                   {
                                                       referenciaId = b.PagoParcial.FirstOrDefault().ReferenciaProcesada.ReferenciaId,
                                                       importe = b.PagoParcial.FirstOrDefault().ReferenciaProcesada.Importe,
                                                       fechaPago_ = b.PagoParcial.FirstOrDefault().ReferenciaProcesada.FechaPago
                                                   }).FirstOrDefault();
                    }

                    return referencia;
                }
                catch (Exception e)
                {

                    return null;
                }
            }
        }

        public static bool AplicarDonativo(Universidad.DTO.DTODonativo AlumnoDonativo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    ReferenciaProcesada referencia = db.Pago.Where(a => a.ReferenciaId == AlumnoDonativo.ReferenciaId 
                                                                  && a.Cuota1.PagoConceptoId == 1014
                                                                  && a.PagoParcial.FirstOrDefault().ReferenciaProcesada.EstatusId == 1)
                                                           .FirstOrDefault().PagoParcial.FirstOrDefault().ReferenciaProcesada;
                    decimal montoTotal = AlumnoDonativo.Alumnos.Select(a => a.Monto).Sum();

                    if (AlumnoDonativo.Alumnos.Select(a => a.Monto).Sum() <= referencia.Restante)
                    {
                        AlumnoDonativo.Alumnos.ForEach(a =>
                        {
                            db.ReferenciaProcesada.Add(new ReferenciaProcesada
                            {
                                ReferenciaId = "0000000000",
                                FechaPago = DateTime.Now,
                                Importe = a.Monto,
                                Restante = a.Monto,
                                AlumnoId = a.AlumnoId,
                                ReferenciaTipoId = 5,
                                Observaciones = "",
                                SeGasto = false,
                                EsIngles = false,
                                EstatusId = 1
                            });
                        });

                        db.SaveChanges();

                        referencia.EstatusId = 2;
                            

                        db.ReferenciaProcesada.Local.ToList().ForEach(a =>
                        {
                            if (a.ReferenciaId != AlumnoDonativo.ReferenciaId)
                            {
                                db.ReferenciaDonativo.Add(new ReferenciaDonativo
                                {
                                    ReferenciaProcesadaIdOrigen = referencia.ReferenciaProcesadaId,
                                    ReferenciaProcesadaIdDestino = a.ReferenciaProcesadaId,
                                    Fecha = DateTime.Now,
                                    Hora = DateTime.Now.TimeOfDay,
                                    UsuarioId = AlumnoDonativo.UsuarioId
                                });
                            }
                            
                        });
                        db.SaveChanges();
                        return true;

                    }
                    else
                    {
                        return true;
                    }
                    
                }
                catch (Exception e)
                {

                    return false;
                }
            }
        }

    }
}
