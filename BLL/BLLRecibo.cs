using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Universidad.BLL
{
    public class BLLRecibo
    {
        public static void GeneraArchivo(string directorio, int reciboId, DTO.DTOLogin Credencial)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.ReciboArchivo.Add(new ReciboArchivo { 
                    ReciboId = reciboId,
                    SucursalCajaId = Credencial.sucursalCajaId,
                    Archivo = Utilities.Archivo.Bytes(directorio)
                });

                db.SaveChanges();
            }
        }

        public static byte[] Visualizar(DTO.Recibo Recibo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.ReciboArchivo
                        where a.SucursalCajaId == Recibo.sucursalCajaId
                        && a.ReciboId == Recibo.reciboId
                        select a.Archivo).FirstOrDefault();
            }
        }

        //public static void Cancelar(ref Utilities.ProcessResult Resultado, DTO.Recibo Recibo, DTO.DTOLogin Credencial)
        //{
        //    try
        //    {
        //        using (UniversidadEntities db = new UniversidadEntities())
        //        {
        //            Recibo Cancelado = db.Recibo.Where(recibo => recibo.ReciboId == Recibo.reciboId && recibo.SucursalCajaId == Recibo.sucursalCajaId).FirstOrDefault();
        //            Cancelado.EstatusId = 3;
        //            Cancelado.ReciboDetalle = new ReciboDetalle { 
        //                ReciboId = Recibo.reciboId,
        //                SucursalCajaId = Recibo.sucursalCajaId,
        //                FechaCancelacion = DateTime.Now,
        //                Observaciones = Recibo.observacionesCancelacion,
        //                UsuarioId = Credencial.usuarioId
        //            };

        //            List<Pago> Pagos = db.Pago.Where(pago => pago.SucursalCajaId == Recibo.sucursalCajaId && pago.ReciboId == Recibo.reciboId).ToList();

        //            Pagos.ForEach(pago =>
        //            {
        //                pago.SucursalCajaId = null;
        //                pago.ReciboId = null;
        //                pago.Serie = null;
        //                pago.Pago1 = null;
        //                pago.FechaPago = null;
        //                pago.HoraPago = null;
        //                pago.EstatusId = 1;

        //                db.PagoDetalle.RemoveRange(pago.PagoDetalle);
        //            });

        //            db.SaveChanges();

        //            Resultado.Estatus = true;
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        Resultado.Estatus = false;
        //        Resultado.Mensaje = Ex.Message;
        //        Resultado.MensajeDetalle = (Ex.InnerException != null) ? Ex.InnerException.InnerException.Message : string.Empty;
        //        Resultado.Informacion = "BLLRecibo.Cancelar()";
        //    }
        //}

        public static List<DTO.Recibo> Consulta(string filtro)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    if (filtro == "")
                    {
                        return (from a in db.Recibo
                                join b in db.Alumno on a.AlumnoId equals b.AlumnoId
                                join c in db.SucursalCaja on a.SucursalCajaId equals c.SucursalCajaId
                                join d in db.Caja on c.CajaId equals d.CajaId
                                join e in db.Sucursal on c.SucursalId equals e.SucursalId
                                select new DTO.Recibo
                                {
                                    reciboId = a.ReciboId,
                                    sucursalCajaId = a.SucursalCajaId,
                                    sucursal = e.DescripcionId + " | " + d.Descripcion,
                                    importe = a.Importe,
                                    alumno = (a.AlumnoId + " | " + b.Nombre + " " + b.Paterno + " " + b.Materno).Trim(),
                                    ofertaEducativa = a.OfertaEducativa.Descripcion,
                                    fechaGeneracion = a.FechaGeneracion,
                                    usuario = (a.Usuario.UsuarioId + " | " + a.Usuario.Nombre + " " + a.Usuario.Paterno + " " + a.Usuario.Materno).Trim(),
                                    estatus = a.Estatus.Descripcion,
                                    esCancelable = a.EstatusId == 1 ? true : false
                                }).ToList();
                    }

                    else
                    {

                        int noRecibo = int.Parse(filtro);

                        return (from a in db.Recibo
                                join b in db.Alumno on a.AlumnoId equals b.AlumnoId
                                join c in db.SucursalCaja on a.SucursalCajaId equals c.SucursalCajaId
                                join d in db.Caja on c.CajaId equals d.CajaId
                                join e in db.Sucursal on c.SucursalId equals e.SucursalId
                                where a.ReciboId == noRecibo
                                select new DTO.Recibo
                                {
                                    reciboId = a.ReciboId,
                                    sucursalCajaId = a.SucursalCajaId,
                                    sucursal = e.DescripcionId + " | " + d.Descripcion,
                                    importe = a.Importe,
                                    alumno = (a.AlumnoId + " | " + b.Nombre + " " + b.Paterno + " " + b.Materno).Trim(),
                                    ofertaEducativa = a.OfertaEducativa.Descripcion,
                                    fechaGeneracion = a.FechaGeneracion,
                                    usuario = (a.Usuario.UsuarioId + " | " + a.Usuario.Nombre + " " + a.Usuario.Paterno + " " + a.Usuario.Materno).Trim(),
                                    estatus = a.Estatus.Descripcion,
                                    esCancelable = a.EstatusId == 1 ? true : false
                                }).ToList();
                    }
                }
                catch (Exception) { return null; }
            }
        }

        //public static DTO.DTORecibo CargaDatos (DTO.DTOLogin Credenciales, int reciboId)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        //Establecer si es este concepto de pago hay que traer precio de Pago, no de concepto {Por que es = 0}
        //        //int idComisionTC = BLLSistemaConfiguracion.ComisionTC();

        //        var Conceptos = (from a in db.Pago
        //                         join b in db.Cuota on a.CuotaId equals b.CuotaId
        //                         join c in db.PagoConcepto on new { b.PagoConceptoId, a.OfertaEducativaId } equals new { c.PagoConceptoId, c.OfertaEducativaId }
        //                         join d in db.Subperiodo on new { a.SubperiodoId, a.PeriodoId } equals new { d.SubperiodoId, d.PeriodoId }
        //                         join e in db.Mes on d.MesId equals e.MesId
        //                         where a.SucursalCajaId == Credenciales.sucursalCajaId
        //                               && a.ReciboId == reciboId
        //                         select new DTO.DTOReciboConcepto
        //                         {
        //                             cantidad = "1",
        //                             descripcion = c.Descripcion + " " + e.Descripcion + " " + a.FechaGeneracion.Year.ToString(),
        //                             //importe = "$ " + b.Monto.ToString()
        //                             importe = "$ " + ((c.EsVariable) ? a.Promesa.ToString() : b.Monto.ToString())

        //                         }).ToList();

        //        var Descuentos = (from a in db.Pago
        //                          join b in db.PagoDescuento on a.PagoId equals b.PagoId
        //                          join c in db.Descuento on b.DescuentoId equals c.DescuentoId
        //                          join d in db.Subperiodo on new { a.SubperiodoId, a.PeriodoId } equals new { d.SubperiodoId, d.PeriodoId }
        //                          join e in db.Mes on d.MesId equals e.MesId
        //                          where a.SucursalCajaId == Credenciales.sucursalCajaId
        //                                && a.ReciboId == reciboId
        //                          select new DTO.DTOReciboConcepto
        //                          {
        //                              cantidad = "",
        //                              descripcion = c.Descripcion + " " + e.Descripcion + " " + a.FechaGeneracion.Year.ToString(),
        //                              importe = "$ " + (-b.Monto).ToString()

        //                          }).ToList();

        //        if (Descuentos.Count > 0)
        //            Conceptos.Add(new DTO.DTOReciboConcepto
        //            {
        //                cantidad = "",
        //                descripcion = " ",
        //                importe = " "
        //            });

        //        Conceptos.AddRange(Descuentos);

        //        return new DTO.DTORecibo
        //        {
        //            //Datos recibo
        //            DatosGenerales = (from a in db.Recibo
        //                              join b in db.Alumno on a.AlumnoId equals b.AlumnoId
        //                              join c in db.OfertaEducativa on a.OfertaEducativaId equals c.OfertaEducativaId
        //                              join d in db.Usuario on a.UsuarioId equals d.UsuarioId
        //                              where a.ReciboId == reciboId 
        //                                    && a.SucursalCajaId == Credenciales.sucursalCajaId
        //                              select new DTO.DTOReciboDatos
        //                              {
        //                                  alumno = a.AlumnoId + " | " + (b.Nombre + " " + b.Paterno + " " + b.Materno).Trim(),
        //                                  ofertaEducativa = c.Descripcion,
        //                                  reciboId = "" + a.ReciboId,
        //                                  fechaGeneracion = a.FechaGeneracion,
        //                                  horaGeneracion = a.HoraGeneracion,
        //                                  importeLetra = db.fnImporteLetra(a.Importe.ToString(), "pesos", true),
        //                                  total = a.Importe,
        //                                  observaciones = a.Observaciones,
        //                                  cajero = (d.Nombre + " " + d.Paterno + " " + d.Materno).Trim(),
        //                                  anuncio = (from z in db.SucursalAnuncio
        //                                             join y in db.SucursalCaja on z.SucursalId equals y.SucursalId
        //                                             select z.Descripcion).FirstOrDefault()

        //                              }).ToList(),

        //            //Datos sucursal
        //            Sucursal = (from a in db.SucursalDetalle
        //                        join b in db.Pais on a.PaisId equals b.PaisId
        //                        join c in db.EntidadFederativa on a.EntidadFederativaId equals c.EntidadFederativaId
        //                        where a.SucursalId == (from z in db.SucursalCaja
        //                                               where z.SucursalCajaId == Credenciales.sucursalCajaId
        //                                               select z.SucursalId
        //                                              ).FirstOrDefault()
        //                        select new DTO.DTOReciboSucursal
        //                        {
        //                            pais = b.Descripcion,
        //                            entidadFederativa = c.Descripcion,
        //                            delegacion = a.Delegacion,
        //                            cp = a.Cp,
        //                            colonia = a.Colonia,
        //                            noExterior = a.NoExterior,
        //                            calle = a.Calle,
        //                            telefono = a.Telefono

        //                        }).ToList(),
        //            //Datos conceptos
        //            Conceptos = Conceptos

                    
        //            //Datos descuentos
        //            /*
        //            Descuentos = (from a in db.Pago
        //                          join b in db.PagoDescuento on a.PagoId equals b.PagoId
        //                          join c in db.Descuento on b.DescuentoId equals c.DescuentoId
        //                          join d in db.Subperiodo on new { a.SubperiodoId, a.PeriodoId } equals new { d.SubperiodoId, d.PeriodoId }
        //                          join e in db.Mes on d.MesId equals e.MesId
        //                          where a.SucursalCajaId == Credenciales.sucursalCajaId
        //                                && a.ReciboId == reciboId
        //                          select new DTO.DTOReciboDescuento
        //                          {
        //                              descripcion = c.Descripcion + " " + e.Descripcion + " " + a.Anio.ToString(),
        //                              importe = -(b.Monto)

        //                          }).ToList()
        //             * */
        //        };
        //    }
        //}
    }
}
