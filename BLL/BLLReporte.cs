using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Universidad.BLL
{
    public class BLLReporte
    {
        //public static List<DTO.DTOReporteCobro> CorteCobro()
        //{
        //    try
        //    {
        //        using (UniversidadEntities db = new UniversidadEntities())
        //        {
        //            return (from a in db.Pago
        //                    join b in db.Recibo on new { A = a.SucursalCajaId ?? 0, B = a.ReciboId, C = a.Serie }
        //                                        equals new { A = b.SucursalCajaId, B = b.ReciboId, C = b.Serie }
        //                    join c in db.Usuario on b.UsuarioId equals c.UsuarioId
        //                    join d in db.Alumno on b.AlumnoId equals d.AlumnoId
        //                    join e in db.Cuota on a.CuotaId equals e.CuotaId
        //                    join f in db.PagoConcepto on new { e.PagoConceptoId, a.OfertaEducativaId } equals new { f.PagoConceptoId, f.OfertaEducativaId }
        //                    join g in db.Subperiodo on new { a.SubperiodoId, a.PeriodoId } equals new { g.SubperiodoId, g.PeriodoId }
        //                    join h in db.Mes on g.MesId equals h.MesId
        //                    select new DTO.DTOReporteCobro
        //                    {
        //                        pagoId = a.PagoId,
        //                        cajero = (c.UsuarioId + " " + c.Nombre + " " + c.Paterno + " " + c.Materno).Trim(),
        //                        alumno = (d.AlumnoId + " " + d.Nombre + " " + d.Paterno + " " + d.Materno).Trim(),
        //                        concepto = f.Descripcion + " " + h.Descripcion + " " + a.Anio.ToString(),
        //                        importe = a.Pago1 ?? 0,
        //                        descuento = a.Cuota - a.Promesa

        //                    }).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public static DTO.DTOCorteCaja CorteDeCaja(DateTime FechaInicial, DateTime FechaFinal, DTO.Usuario.CorteDeCaja.DTOCajero Cajero)
        //{
        //    List<DTO.DTOCorteCajaConcepto> Otros = new List<DTO.DTOCorteCajaConcepto>();
        //    List<DTO.DTOCorteCajaConcepto> Conceptos = new List<DTO.DTOCorteCajaConcepto>();
        //    List<DTO.DTOCorteCajaTotal> Totales= new List<DTO.DTOCorteCajaTotal>();
        //    DTO.Reporte.CorteDeCaja.DTOFirmas Firmas = new DTO.Reporte.CorteDeCaja.DTOFirmas();

        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {

        //        if (Cajero == null)
        //        {
        //            Otros = (from a in db.Recibo
        //                     join b in db.Usuario on a.UsuarioId equals b.UsuarioId
        //                     join c in db.Alumno on a.AlumnoId equals c.AlumnoId
        //                     where a.EstatusId == 3
        //                        && a.FechaGeneracion >= FechaInicial && a.FechaGeneracion <= FechaFinal
        //                        && a.UsuarioId != 0
        //                     select new DTO.DTOCorteCajaConcepto
        //                     {
        //                         reciboId = a.ReciboId,
        //                         caja = a.SucursalCaja.Caja.Descripcion,
        //                         alumno = (c.AlumnoId + " | " + c.Nombre).Trim(),
        //                         cajero = (b.UsuarioId + " | " + b.Nombre).Trim(),
        //                         importe = 0,
        //                         metodoPago = "Cancelado",
        //                         FechaPago = a.FechaGeneracion
        //                     }).ToList();

        //            Conceptos = (from x in
        //                             (from a in db.Pago
        //                              join b in db.PagoDetalle on a.PagoId equals b.PagoId
        //                              join c in db.Recibo on new { A = a.SucursalCajaId ?? 0, B = a.ReciboId ?? 0, C = a.Serie }
        //                                      equals new { A = c.SucursalCajaId, B = c.ReciboId, C = c.Serie }
        //                              join d in db.Usuario on c.UsuarioId equals d.UsuarioId
        //                              join e in db.Alumno on c.AlumnoId equals e.AlumnoId
        //                              join f in db.PagoMetodo on b.PagoMetodoId equals f.PagoMetodoId
        //                              where c.FechaGeneracion >= FechaInicial && c.FechaGeneracion <= FechaFinal
        //                                && c.UsuarioId != 0
        //                              select new { b, c, d, e, f })

        //                         group x by new
        //                         {
        //                             x.b.PagoMetodoId,
        //                             Usuario = (x.c.UsuarioId + " | " + x.d.Nombre).Trim(),
        //                             Alumno = (x.c.AlumnoId + " | " + x.e.Nombre).Trim(),
        //                             Metodo = (x.f.Descripcion),
        //                             Recibo = (x.c.ReciboId),
        //                             DescripcionCaja = (x.c.SucursalCaja.Caja.Descripcion),
        //                             FechaPago = (x.c.FechaGeneracion)
        //                         } into g

        //                         orderby g.Key.Recibo ascending
        //                         select new DTO.DTOCorteCajaConcepto
        //                         {
        //                             reciboId = g.Key.Recibo,
        //                             caja = g.Key.DescripcionCaja,
        //                             alumno = g.Key.Alumno,
        //                             cajero = g.Key.Usuario,
        //                             importe = g.Sum(metodo => metodo.b.Monto),
        //                             metodoPago = g.Key.Metodo,
        //                             FechaPago = g.Key.FechaPago
        //                         }).ToList();


        //            Totales = (from u in
        //                           (from y in Conceptos
        //                            select new { y })

        //                       group u by new
        //                       {
        //                           u.y.metodoPago
        //                       } into g

        //                       select new DTO.DTOCorteCajaTotal
        //                       {
        //                           metodoPago = g.Key.metodoPago,
        //                           importe = g.Sum(metodo => metodo.y.importe)
        //                       }).ToList();


        //            Firmas = (from a in db.Usuario.AsNoTracking()
        //                      join b in db.UsuarioTipo.AsNoTracking() on a.UsuarioTipoId equals b.UsuarioTipoId
        //                      where a.UsuarioTipoId == 3
        //                             && a.EstatusId == 1
        //                      select new DTO.Reporte.CorteDeCaja.DTOFirmas
        //                      {
        //                          primerNombre = (a.Nombre + " " + a.Paterno + " " + a.Materno).Trim(),
        //                          primerPuesto = b.Descripcion,
        //                          segundoNombre = "",
        //                          segundoPuesto = ""
        //                      }).ToList().FirstOrDefault();

        //            Conceptos.AddRange(Otros);
        //            Conceptos.Sort((a, b) => a.reciboId.CompareTo(b.reciboId));
        //        }

        //        else
        //        {
        //            Otros = (from a in db.Recibo
        //                     join b in db.Usuario on a.UsuarioId equals b.UsuarioId
        //                     join c in db.Alumno on a.AlumnoId equals c.AlumnoId
        //                     where a.EstatusId == 3
        //                        && a.FechaGeneracion >= FechaInicial && a.FechaGeneracion <= FechaFinal
        //                        && a.UsuarioId == Cajero.usuarioId
        //                        && a.UsuarioId != 0
        //                     select new DTO.DTOCorteCajaConcepto
        //                     {
        //                         reciboId = a.ReciboId,
        //                         caja = a.SucursalCaja.Caja.Descripcion,
        //                         alumno = (c.AlumnoId + " | " + c.Nombre).Trim(),
        //                         cajero = (b.UsuarioId + " | " + b.Nombre).Trim(),
        //                         importe = 0,
        //                         metodoPago = "Cancelado",
        //                         FechaPago = a.FechaGeneracion
        //                     }).ToList();

        //            Conceptos = (from x in
        //                             (from a in db.Pago
        //                              join b in db.PagoDetalle on a.PagoId equals b.PagoId
        //                              join c in db.Recibo on new { A = a.SucursalCajaId ?? 0, B = a.ReciboId ?? 0, C = a.Serie }
        //                                      equals new { A = c.SucursalCajaId, B = c.ReciboId, C = c.Serie }
        //                              join d in db.Usuario on c.UsuarioId equals d.UsuarioId
        //                              join e in db.Alumno on c.AlumnoId equals e.AlumnoId
        //                              join f in db.PagoMetodo on b.PagoMetodoId equals f.PagoMetodoId
        //                              where c.FechaGeneracion >= FechaInicial && c.FechaGeneracion <= FechaFinal
        //                                && c.UsuarioId == Cajero.usuarioId
        //                                && c.UsuarioId != 0
        //                              select new { b, c, d, e, f })

        //                         group x by new
        //                         {
        //                             x.b.PagoMetodoId,
        //                             Usuario = (x.c.UsuarioId + " | " + x.d.Nombre).Trim(),
        //                             Alumno = (x.c.AlumnoId + " | " + x.e.Nombre).Trim(),
        //                             Metodo = (x.f.Descripcion),
        //                             Recibo = (x.c.ReciboId),
        //                             DescripcionCaja = (x.c.SucursalCaja.Caja.Descripcion),
        //                             FechaPago = (x.c.FechaGeneracion)
        //                         } into g

        //                         orderby g.Key.Recibo ascending
        //                         select new DTO.DTOCorteCajaConcepto
        //                         {
        //                             reciboId = g.Key.Recibo,
        //                             caja = g.Key.DescripcionCaja,
        //                             alumno = g.Key.Alumno,
        //                             cajero = g.Key.Usuario,
        //                             importe = g.Sum(metodo => metodo.b.Monto),
        //                             metodoPago = g.Key.Metodo,
        //                             FechaPago = g.Key.FechaPago
        //                         }).ToList();

        //            Totales = (from u in
        //                           (from y in Conceptos
        //                            select new { y })

        //                       group u by new
        //                       {
        //                           u.y.metodoPago
        //                       } into g

        //                       select new DTO.DTOCorteCajaTotal
        //                       {
        //                           metodoPago = g.Key.metodoPago,
        //                           importe = g.Sum(metodo => metodo.y.importe)
        //                       }).ToList();


        //            //Primera Firma

                    

        //           Firmas = (from a in db.Usuario.AsNoTracking()
        //                      join b in db.UsuarioTipo.AsNoTracking() on a.UsuarioTipoId equals b.UsuarioTipoId
        //                      where a.UsuarioTipoId == 3
        //                             && a.EstatusId == 1
        //                      select new DTO.Reporte.CorteDeCaja.DTOFirmas { 
        //                          primerNombre = (a.Nombre + " " + a.Paterno + " " + a.Materno).Trim(),
        //                          primerPuesto = b.Descripcion,
        //                          segundoNombre = (from p in db.Usuario
        //                                               where p.UsuarioId == Cajero.usuarioId
        //                                               select p.Nombre + " " + p.Paterno +  " " + p.Materno).FirstOrDefault(),
        //                          segundoPuesto = (from p in db.Usuario
        //                                           join q in db.UsuarioTipo on p.UsuarioTipoId equals q.UsuarioTipoId
        //                                               where p.UsuarioId == Cajero.usuarioId
        //                                               select q.Descripcion
        //                                               ).FirstOrDefault()
        //                      }).ToList().FirstOrDefault();

                  

        //            Conceptos.AddRange(Otros);
        //            Conceptos.Sort((a, b) => a.reciboId.CompareTo(b.reciboId));
        //        }

        //        return new DTO.DTOCorteCaja
        //        {
        //            Concepto = Conceptos,
        //            Total = Totales,
        //            Firma = new List<DTO.Reporte.CorteDeCaja.DTOFirmas>() { Firmas }
        //        };
        //    }
        //}

        public static void Pagos(DateTime FechaInicial, DateTime FechaFinal)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                //(from a in db.Recibo
                //     new DTO)
            }

        }
    }
}
