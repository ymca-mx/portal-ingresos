using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity;
using System.Globalization;
using System.ComponentModel;
using Universidad.DTO;

namespace BLL
{
    public class BLLEstadoCuenta2
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");
        //public static List<DTOEstadoCuenta> ConsultarEstadodeCuenta(int AlumnoId)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        try
        //        {
        //            //List<DTOEstadoCuenta> lstCuenta = new List<DTOEstadoCuenta>();
        //            //List<Pago> lstPagos = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.EstatusId != 2 && (P.EstatusId == 4 || P.EstatusId == 14)).ToList();
        //            //lstPagos.ForEach(delegate(Pago objP)
        //            //{
        //            //    List<PagoParcial> lstParcial = objP.PagoParcial.Count > 0 ? objP.PagoParcial.ToList() : null;
        //            //    List<ReferenciadoDetalle> lstRDetalle = db.ReferenciadoDetalle.Where(RD => RD.PagoId == objP.PagoId).ToList();
        //            //    DTOEstadoCuenta objEstado = new DTOEstadoCuenta();
        //            //    int MaxParcial = lstParcial.Count > 0 ?
        //            //        lstParcial.Max(P => P.PagoId) : 0;
        //            //    int MaxRefe = lstRDetalle.Count > 0 ?
        //            //        lstRDetalle.Max(RD => RD.PagoId) : 0; 

        //            //    objEstado.Concepto = objP.Cuota1.PagoConcepto.Descripcion;
        //            //    objEstado.ReferenciaId = int.Parse(objP.ReferenciaId).ToString();
        //            //    objEstado.Fecha=MaxParcial>0 && MaxRefe>0?MaxParcial>MaxRefe?lstParcial.Where(P=>P.PagoId==MaxParcial).FirstOrDefault():
        //            //});
        //            return null;
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //}
        //public static List<DTO.EstadoCuenta> ConsultarEstadodeCuenta(int AlumnoId, int PeriodoId, int Anio)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        int PeriodoIdA = PeriodoId == 1 ? 4 : PeriodoId - 1;
        //        int AnioA = PeriodoId == 1 ? Anio - 1 : Anio;
        //        /********************************************************************************/
        //        /*                              Cargos                                          */
        //        /********************************************************************************/

        //        List<DTO.EstadoCuenta> Cargos = (from a in db.Pago
        //                                         join b in db.Cuota on a.CuotaId equals b.CuotaId
        //                                         join c in db.PagoConcepto on new { b.PagoConceptoId, b.OfertaEducativaId } equals new { c.PagoConceptoId, c.OfertaEducativaId }
        //                                         join d in db.Subperiodo on new { a.PeriodoId, a.SubperiodoId } equals new { d.PeriodoId, d.SubperiodoId }
        //                                         join e in db.Mes on d.MesId equals e.MesId
        //                                         where a.Anio == Anio
        //                                             && a.PeriodoId == PeriodoId
        //                                             && a.AlumnoId == AlumnoId
        //                                             && b.PagoConceptoId != 1007
        //                                             && (a.EstatusId != 2 && a.EstatusId != 3)
        //                                         select new DTO.EstadoCuenta
        //                                         {
        //                                             pagoid = a.PagoId,
        //                                             mes = (from y in db.Subperiodo
        //                                                    where y.PeriodoId == a.PeriodoId
        //                                                          && y.SubperiodoId == a.SubperiodoId
        //                                                    select y.MesId).FirstOrDefault(),
        //                                             anio = (from z in db.Periodo
        //                                                     where z.Anio == a.Anio
        //                                                            && z.PeriodoId == a.PeriodoId
        //                                                     select z.FechaInicial.Year).FirstOrDefault(),
        //                                             referenciaId = a.ReferenciaId,
        //                                             anioP = a.Anio,
        //                                             Periodoid = a.PeriodoId,
        //                                             subperiodoid = a.SubperiodoId,
        //                                             fecha = a.FechaGeneracion,
        //                                             subperiodoId = a.SubperiodoId,
        //                                             concepto = a.Anio == AnioA && a.PeriodoId == PeriodoIdA ? "Adeudo, periodos anteriores" :
        //                                                         (b.PagoConceptoId == 802 ? c.Descripcion :
        //                                                         c.Descripcion + " " + e.Descripcion + " " + (a.PeriodoId > 1 ? a.Anio : a.FechaGeneracion.Year)),
        //                                             cargo = a.Cuota

        //                                         }).ToList();


        //        Cargos.ForEach(a =>
        //        {

        //            //a.fecha = a.subperiodoId == 1 ? a.fecha : new DateTime(a.anio, a.mes, 1);
        //            a.fecha = a.subperiodoid != 1 ?
        //                DateTime.Parse("01/" + db.Pago.Where(P => P.PagoId == a.pagoid).FirstOrDefault().Subperiodo.MesId.ToString() +
        //                "/" + (a.Periodoid == 1 ? (Anio - 1).ToString() : Anio.ToString()), Cultura) : a.fecha;


        //            a.referenciaId = "" + int.Parse(a.referenciaId);
        //        });

        //        //Cargos.ForEach(a =>
        //        //{
        //        //    Console.WriteLine(a.fecha.ToShortDateString() + " | " + a.concepto + " | " + a.referenciaId + " | " + a.cargo + " | " + a.abono);
        //        //});


        //        /********************************************************************************/
        //        /*                              Abonos                                          */
        //        /********************************************************************************/

        //        List<DTO.EstadoCuenta> Abonos = (from a in db.ReferenciaProcesada
        //                                         where a.AlumnoId == AlumnoId
        //                                         select new DTO.EstadoCuenta
        //                                         {
        //                                             fecha = a.FechaPago,
        //                                             referenciaId = a.ReferenciaId,
        //                                             concepto = "Su abono... Gracias",
        //                                             abono = a.Importe
        //                                         }).ToList();

        //        Cargos.AddRange(Abonos);


        //        /********************************************************************************/
        //        /*                              Descuentos                                      */
        //        /********************************************************************************/

        //        List<DTO.EstadoCuenta> Descuentos = (from a in db.PagoDescuento
        //                                             join b in db.Pago on a.PagoId equals b.PagoId
        //                                             join d in db.Descuento on a.DescuentoId equals d.DescuentoId

        //                                             join f in db.Cuota on b.CuotaId equals f.CuotaId

        //                                             join g in db.PagoConcepto on new { f.PagoConceptoId, f.OfertaEducativaId } equals new { g.PagoConceptoId, g.OfertaEducativaId }
        //                                             join h in db.Subperiodo on new { b.PeriodoId, b.SubperiodoId } equals new { h.PeriodoId, h.SubperiodoId }
        //                                             join i in db.Mes on h.MesId equals i.MesId
        //                                             where b.AlumnoId == AlumnoId
        //                                                   && b.Anio == Anio
        //                                                   && b.PeriodoId == PeriodoId
        //                                                   && (b.Cuota1.PagoConceptoId == 800 || b.Cuota1.PagoConceptoId == 802)
        //                                             select new DTO.EstadoCuenta
        //                                             {
        //                                                 fecha =

        //                                                 db.AlumnoDescuento
        //                                                   .Where(e => e.AlumnoId == b.AlumnoId && e.PagoConceptoId == b.Cuota1.PagoConceptoId
        //                                                   && e.Anio == Anio && e.PeriodoId == PeriodoId)
        //                                                   .FirstOrDefault().FechaAplicacion ?? DateTime.Now,

        //                                                 concepto = a.Descuento.Descripcion + " " +
        //                                                 (

        //                                                 (d.Descripcion != "Pago Anticipado") ?

        //                                                 db.AlumnoDescuento
        //                                                   .Where(e => e.AlumnoId == b.AlumnoId && e.PagoConceptoId == b.Cuota1.PagoConceptoId &&
        //                                                       e.Anio == Anio && e.PeriodoId == PeriodoId).FirstOrDefault().Monto + "%"
        //                                                   + " " + (b.Anio == AnioA && b.PeriodoId == PeriodoIdA ? "Adeudo, periodos anteriores" :
        //                                                       (f.PagoConceptoId == 802 ? g.Descripcion :
        //                                                       g.Descripcion + " " + i.Descripcion + " " + (b.PeriodoId > 1 ? b.Anio : b.FechaGeneracion.Year)))

        //                                                      : (f.PagoConceptoId == 802 ? g.Descripcion :
        //                                                       g.Descripcion + " " + i.Descripcion + " " + (b.PeriodoId > 1 ? b.Anio : b.FechaGeneracion.Year))
        //                                                       )
        //                                                       ,
        //                                                 referenciaId = b.ReferenciaId,
        //                                                 abono = a.Monto
        //                                             }).ToList();

        //        Cargos.AddRange(Descuentos);

        //        //Cargos.Sort((x, y) => y.fecha.CompareTo(x.fecha));
        //        Cargos = Cargos.OrderBy(C => C.fecha).ToList();
        //        //Cargos.Reverse();
        //        decimal cargos = 0, abonos = 0, Total = 0;
        //        Cargos.ForEach(delegate(EstadoCuenta objes)
        //        {
        //            objes.referenciaId = int.Parse(objes.referenciaId).ToString();
        //            objes.fechaS = (objes.fecha.Day < 10 ? "0" + objes.fecha.Day.ToString() : objes.fecha.Day.ToString()) + "/" +
        //                (objes.fecha.Month < 10 ? "0" + objes.fecha.Month.ToString() : objes.fecha.Month.ToString()) + "/" +
        //                objes.fecha.Year.ToString();
        //            cargos += objes.cargo;
        //            abonos += objes.abono;
        //        });
        //        Total = cargos - abonos;
        //        Cargos[0].totalAbonos = abonos.ToString("C", Cultura);
        //        Cargos[0].totalCargos = cargos.ToString("C", Cultura);
        //        Cargos[0].Total = Total.ToString("C", Cultura);
        //        return Cargos;

        //    }
        //}
        //public static List<DTO.EstadoCuenta> TestEstadoCuenta(int alumnoId, int periodoId, int anio)
        //{
        //    DTO.DTOPeriodo Periodo = new DTO.DTOPeriodo { Anio = anio, PeriodoId = periodoId };

        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        /********************************************************************************/
        //        /*                              Cargos                                          */
        //        /********************************************************************************/

        //        List<DTO.EstadoCuenta> Cargos = new List<DTO.EstadoCuenta>();

        //        Cargos = (from a in db.Pago
        //                  join b in db.Cuota on a.CuotaId equals b.CuotaId
        //                  join c in db.PagoConcepto on new { b.PagoConceptoId, b.OfertaEducativaId } equals new { c.PagoConceptoId, c.OfertaEducativaId }
        //                  join d in db.Subperiodo on new { a.PeriodoId, a.SubperiodoId } equals new { d.PeriodoId, d.SubperiodoId }
        //                  join e in db.Mes on d.MesId equals e.MesId
        //                  where a.Anio == Periodo.Anio
        //                      && a.PeriodoId == Periodo.PeriodoId
        //                      && a.AlumnoId == alumnoId
        //                      && b.PagoConceptoId != 1007
        //                      && (a.EstatusId != 2 && a.EstatusId != 3)
        //                  select new DTO.EstadoCuenta
        //                  {
        //                      mes = (from y in db.Subperiodo
        //                             where y.PeriodoId == a.PeriodoId
        //                                   && y.SubperiodoId == a.SubperiodoId
        //                             select y.MesId).FirstOrDefault(),
        //                      anio = (from z in db.Periodo
        //                              where z.Anio == a.Anio
        //                                     && z.PeriodoId == a.PeriodoId
        //                              select z.FechaInicial.Year).FirstOrDefault(),
        //                      referenciaId = a.ReferenciaId,
        //                      fecha = a.FechaGeneracion,
        //                      subperiodoId = a.SubperiodoId,
        //                      concepto = b.PagoConceptoId == 800 || b.PagoConceptoId == 807 || b.PagoConceptoId == 306
        //                                 ? c.Descripcion + " " + e.Descripcion + " " + (a.PeriodoId > 1 ? a.Anio : a.FechaGeneracion.Year)
        //                                 : c.Descripcion,
        //                      cargo = a.Cuota == 0 ? a.Promesa : a.Cuota

        //                  }).ToList();

        //        //Fecha de generación de los cargos
        //        //Solo respetar donde subperiodoId = 1, sino hay que ponerla en base al periodo y al subperiodo
        //        Cargos.ForEach(a =>
        //        {

        //            a.fecha = a.subperiodoId == 1 ? a.fecha : new DateTime(a.anio, a.mes, 1);
        //            a.referenciaId = "" + int.Parse(a.referenciaId);
        //        });

        //        //Console.WriteLine("*************************************Cargos****************************************");

        //        //Cargos.ForEach(a =>
        //        //{
        //        //    Console.WriteLine(a.fecha.ToShortDateString() + " | " + a.concepto + " | " + a.referenciaId + " | " + a.cargo + " | " + a.abono);
        //        //});



        //        /********************************************************************************/
        //        /*                    Adeudos Periodos anteriores                         */
        //        /********************************************************************************/

        //        List<DTO.EstadoCuenta> Anteriores = new List<DTO.EstadoCuenta>();

        //        Anteriores = (from a in db.Pago
        //                      where a.Anio == 2016
        //                            && a.PeriodoId == 1
        //                            && (a.EstatusId != 2 && a.EstatusId != 3)
        //                            && a.AlumnoId == alumnoId
        //                      select new DTO.EstadoCuenta
        //                      {
        //                          referenciaId = a.ReferenciaId,
        //                          concepto = "Adeudo, periodos anteriores",
        //                          cargo = a.Promesa
        //                      }).ToList();

        //        DateTime FechaAdeudos = Cargos.Min(a => a.fecha).AddDays(-1);

        //        Anteriores.ForEach(a =>
        //        {
        //            a.fecha = FechaAdeudos;
        //        });

        //        Cargos.AddRange(Anteriores);

        //        //Console.WriteLine("*************************************Adeudos Anteriores****************************************");

        //        //Anteriores.ForEach(a =>
        //        //{
        //        //    Console.WriteLine(a.fecha.ToShortDateString() + " | " + a.concepto + " | " + a.referenciaId + " | " + a.cargo + " | " + a.abono);
        //        //});

        //        /********************************************************************************/
        //        /*                              Abonos                                          */
        //        /********************************************************************************/

        //        List<DTO.EstadoCuenta> AbonosReferencias = new List<DTO.EstadoCuenta>();
        //        List<DTO.EstadoCuenta> AbonosCaja = new List<DTO.EstadoCuenta>();

        //        AbonosReferencias = (from a in db.ReferenciaProcesada
        //                             where a.AlumnoId == alumnoId
        //                             select new DTO.EstadoCuenta
        //                             {
        //                                 fecha = a.FechaPago,
        //                                 referenciaId = a.ReferenciaId,
        //                                 concepto = "Su abono... Gracias",
        //                                 abono = a.Importe
        //                             }).ToList();

        //        AbonosCaja = (from a in db.Pago
        //                      join b in db.PagoParcial on a.PagoId equals b.PagoId
        //                      where a.AlumnoId == alumnoId
        //                            && a.Anio == Periodo.Anio
        //                            && a.PeriodoId == Periodo.PeriodoId
        //                            && (a.EstatusId == 4 || a.EstatusId == 13 || a.EstatusId == 14)
        //                            && !b.EsReferencia
        //                      select new DTO.EstadoCuenta
        //                      {
        //                          fecha = b.FechaPago,
        //                          referenciaId = "-",
        //                          concepto = "Su abono en caja... Gracias",
        //                          abono = b.Pago
        //                      }).ToList();

        //        AbonosReferencias.AddRange(AbonosCaja);
        //        Cargos.AddRange(AbonosReferencias);

        //        //Console.WriteLine("*************************************Abonos****************************************");


        //        //AbonosReferencias.ForEach(a =>
        //        //{
        //        //    Console.WriteLine(a.fecha.ToShortDateString() + " | " + a.concepto + " | " + a.referenciaId + " | " + a.cargo + " | " + a.abono);
        //        //});

        //        /********************************************************************************/
        //        /*                              Descuentos                                      */
        //        /********************************************************************************/

        //        List<DTO.EstadoCuenta> Descuentos = new List<DTO.EstadoCuenta>();


        //        Descuentos = (from a in db.PagoDescuento
        //                      join b in db.Pago on a.PagoId equals b.PagoId
        //                      join d in db.Descuento on a.DescuentoId equals d.DescuentoId
        //                      join f in db.Cuota on b.CuotaId equals f.CuotaId
        //                      join g in db.PagoConcepto on new { f.PagoConceptoId, f.OfertaEducativaId } equals new { g.PagoConceptoId, g.OfertaEducativaId }
        //                      join h in db.Subperiodo on new { b.PeriodoId, b.SubperiodoId } equals new { h.PeriodoId, h.SubperiodoId }
        //                      join i in db.Mes on h.MesId equals i.MesId
        //                      where b.AlumnoId == alumnoId
        //                            && b.Anio == Periodo.Anio
        //                            && b.PeriodoId == Periodo.PeriodoId
        //                      //&& (b.Cuota1.PagoConceptoId == 800 || b.Cuota1.PagoConceptoId == 802)
        //                      select new DTO.EstadoCuenta
        //                      {
        //                          mes = (from y in db.Subperiodo
        //                                 where y.PeriodoId == b.PeriodoId
        //                                       && y.SubperiodoId == b.SubperiodoId
        //                                 select y.MesId).FirstOrDefault(),
        //                          anio = (from z in db.Periodo
        //                                  where z.Anio == b.Anio
        //                                         && z.PeriodoId == b.PeriodoId
        //                                  select z.FechaInicial.Year).FirstOrDefault(),
        //                          fecha =

        //                          db.AlumnoDescuento
        //                            .Where(e => e.AlumnoId == b.AlumnoId && e.PagoConceptoId == b.Cuota1.PagoConceptoId)
        //                            .FirstOrDefault().FechaAplicacion ?? DateTime.Now,

        //                          concepto = a.Descuento.Descripcion + " " +
        //                          (

        //                              (d.Descripcion != "Pago Anticipado") ?

        //                              db.AlumnoDescuento
        //                                .Where(e => e.AlumnoId == b.AlumnoId && e.PagoConceptoId == b.Cuota1.PagoConceptoId && e.Anio == Periodo.Anio && e.PeriodoId == Periodo.PeriodoId).FirstOrDefault().Monto + "%"
        //                                + " " + ((f.PagoConceptoId == 800 || f.PagoConceptoId == 807) ? i.Descripcion + " " + (b.PeriodoId > 1 ? b.Anio : b.FechaGeneracion.Year)
        //                                : "")


        //                                   : (f.PagoConceptoId == 800 || f.PagoConceptoId == 807 ? g.Descripcion + " " + i.Descripcion + " " + (b.PeriodoId > 1 ? b.Anio : b.FechaGeneracion.Year) :
        //                                    g.Descripcion)
        //                                )
        //                                ,
        //                          referenciaId = b.ReferenciaId,
        //                          abono = a.Monto
        //                      }).ToList();

        //        Descuentos.ForEach(a =>
        //        {

        //            a.fecha = a.subperiodoId == 1 ? a.fecha : new DateTime(a.anio, a.mes, 1);
        //            a.referenciaId = "" + int.Parse(a.referenciaId);
        //        });

        //        //Console.WriteLine("*************************************Descuentos****************************************");

        //        //Descuentos.ForEach(a =>
        //        //{
        //        //    Console.WriteLine(a.fecha.ToShortDateString() + " | " + a.concepto + " | " + a.referenciaId + " | " + a.cargo + " | " + a.abono);
        //        //});

        //        Cargos.AddRange(Descuentos);

        //        //Ordernar los movimientos
        //        Cargos.Sort((x, y) => y.fecha.CompareTo(x.fecha));
        //        Cargos.Reverse();

        //        decimal cargos = 0, abonos = 0, Total = 0;
        //        Cargos.ForEach(delegate(EstadoCuenta objes)
        //        {
        //            objes.referenciaId = objes.referenciaId.Length < 2 ? objes.referenciaId : int.Parse(objes.referenciaId).ToString();
        //            objes.fechaS = (objes.fecha.Day < 10 ? "0" + objes.fecha.Day.ToString() : objes.fecha.Day.ToString()) + "/" +
        //                (objes.fecha.Month < 10 ? "0" + objes.fecha.Month.ToString() : objes.fecha.Month.ToString()) + "/" +
        //                objes.fecha.Year.ToString();
        //            cargos += objes.cargo;
        //            abonos += objes.abono;
        //        });
        //        Total = cargos - abonos;
        //        Cargos[0].totalAbonos = abonos.ToString("C", Cultura);
        //        Cargos[0].totalCargos = cargos.ToString("C", Cultura);
        //        Cargos[0].Total = Total.ToString("C", Cultura);
        //        return Cargos;
        //    }
        //}
        //public static List<EstadoCuenta> TestEstadoCuenta2(int alumnoId, int periodoId, int anio)
        //{
        //    DTO.DTOPeriodo Periodo = new DTO.DTOPeriodo { Anio = anio, PeriodoId = periodoId };

        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {

        //        List<DTO.EstadoCuenta> Cargos = new List<DTO.EstadoCuenta>();

        //        /********************************************************************************/
        //        /*                              Cargos                                          */
        //        /********************************************************************************/

        //        Cargos = (from a in db.Pago
        //                  join b in db.Cuota on a.CuotaId equals b.CuotaId
        //                  join c in db.PagoConcepto on new { b.PagoConceptoId, b.OfertaEducativaId } equals new { c.PagoConceptoId, c.OfertaEducativaId }
        //                  join d in db.Subperiodo on new { a.PeriodoId, a.SubperiodoId } equals new { d.PeriodoId, d.SubperiodoId }
        //                  join e in db.Mes on d.MesId equals e.MesId
        //                  //join f in db.PagoRecargo on a.PagoId equals f.PagoIdRecargo
        //                  where a.Anio == Periodo.Anio
        //                      && a.PeriodoId == Periodo.PeriodoId
        //                      && a.AlumnoId == alumnoId
        //                      && b.PagoConceptoId != 1007
        //                      && b.PagoConceptoId != 1001
        //                      && (a.EstatusId != 2 && a.EstatusId != 3)
        //                  select new DTO.EstadoCuenta
        //                  {
        //                      mes = (from y in db.Subperiodo
        //                             where y.PeriodoId == a.PeriodoId
        //                                   && y.SubperiodoId == a.SubperiodoId
        //                             select y.MesId).FirstOrDefault(),
        //                      anio = (from z in db.Periodo
        //                              where z.Anio == a.Anio
        //                                     && z.PeriodoId == a.PeriodoId
        //                              select z.FechaInicial.Year).FirstOrDefault(),
        //                      referenciaId = a.ReferenciaId,
        //                      fecha = a.FechaGeneracion,
        //                      subperiodoId = a.SubperiodoId,
        //                      concepto = b.PagoConceptoId == 800 || b.PagoConceptoId == 807
        //                                        ? c.Descripcion + " " + e.Descripcion + " " + (a.PeriodoId > 1 ? a.Anio : a.FechaGeneracion.Year)
        //                                        : (b.PagoConceptoId == 306)
        //                                            ? (from z in db.PagoRecargo where z.PagoIdRecargo == a.PagoId select z).FirstOrDefault().Pago.Cuota1.PagoConceptoId == 800
        //                                                ? (c.Descripcion + " " + (from z in db.PagoRecargo where z.PagoIdRecargo == a.PagoId select z).FirstOrDefault().Pago.Cuota1.PagoConcepto.Descripcion + " " + e.Descripcion + " " + (a.PeriodoId > 1 ? a.Anio : a.FechaGeneracion.Year))
        //                                                : c.Descripcion + " " + (from z in db.PagoRecargo where z.PagoIdRecargo == a.PagoId select z).FirstOrDefault().Pago.Cuota1.PagoConcepto.Descripcion
        //                                            : c.Descripcion,
        //                      cargo = a.Cuota == 0 ? a.Promesa : a.Cuota

        //                  }).ToList();

        //        //Fecha de generación de los cargos
        //        //Solo respetar donde subperiodoId = 1, sino hay que ponerla en base al periodo y al subperiodo
        //        Cargos.ForEach(a =>
        //        {
        //            a.fecha = a.subperiodoId == 1 ? a.fecha : new DateTime(a.anio, a.mes, 1);
        //            a.referenciaId = "" + int.Parse(a.referenciaId);
        //        });

        //        //Console.WriteLine("*************************************Cargos****************************************");

        //        //Cargos.ForEach(a =>
        //        //{
        //        //    Console.WriteLine(a.fecha.ToShortDateString() + " | " + a.concepto + " | " + a.referenciaId + " | " + a.cargo + " | " + a.abono);
        //        //});

        //        if (anio == 2016 && periodoId == 1)
        //            Cargos.Clear();

        //        /********************************************************************************/
        //        /*                              Abonos                                          */
        //        /********************************************************************************/

        //        List<DTO.EstadoCuenta> AbonosReferencias = new List<DTO.EstadoCuenta>();
        //        List<DTO.EstadoCuenta> AbonosReferenciasAnteriores = new List<DTO.EstadoCuenta>();
        //        List<DTO.EstadoCuenta> AbonosReferenciasAnterioresCaja = new List<DTO.EstadoCuenta>();
        //        List<DTO.EstadoCuenta> AbonosReferenciasAnterioresCaja2 = new List<DTO.EstadoCuenta>();
        //        List<DTO.EstadoCuenta> AbonosCaja = new List<DTO.EstadoCuenta>();
        //        List<DTO.EstadoCuenta> AbonosCaja2 = new List<DTO.EstadoCuenta>();

        //        AbonosReferencias = (from a in db.ReferenciaProcesada
        //                             where a.AlumnoId == alumnoId
        //                             && (a.Anio == Periodo.Anio && a.PeriodoId == Periodo.PeriodoId)

        //                             select new DTO.EstadoCuenta
        //                             {
        //                                 fecha = a.FechaPago,
        //                                 referenciaId = a.ReferenciaId,
        //                                 concepto = "Su abono... Gracias",
        //                                 abono = a.Importe,
        //                                 seResalta = true
        //                             }).ToList();

        //        AbonosReferenciasAnteriores = (from a in db.ReferenciaProcesada
        //                                       where a.PagoId == null ? (a.ReferenciaId.Substring(5, 4).Contains("0800")
        //                                                                        || a.ReferenciaId.Substring(5, 4).Contains("0802")
        //                                                                        || a.ReferenciaId.Substring(5, 4).Contains("0807")
        //                                                                        || a.ReferenciaId.Substring(5, 4).Contains("0809"))
        //                                                                        && a.Anio == 2016
        //                                                                        && a.PeriodoId == 1
        //                                                                        && a.AlumnoId == alumnoId
        //                                                                     : a.Anio == 2016
        //                                                                        && a.PeriodoId == 1
        //                                                                        && a.AlumnoId == alumnoId
        //                                       select new DTO.EstadoCuenta
        //                                       {
        //                                           fecha = a.FechaPago,
        //                                           referenciaId = a.ReferenciaId,
        //                                           concepto = "Su abono... Gracias",
        //                                           abono = a.Importe,
        //                                           seResalta = true,
        //                                           adeudo = true
        //                                       }).ToList();
        //        //Referencias anteriores {Caja}



        //        AbonosCaja = (from a in db.Pago
        //                      join b in db.PagoParcial on a.PagoId equals b.PagoId
        //                      where a.AlumnoId == alumnoId
        //                            && a.Anio == Periodo.Anio
        //                            && a.PeriodoId == Periodo.PeriodoId
        //                            && (a.EstatusId == 4 || a.EstatusId == 13 || a.EstatusId == 14)
        //                            && !b.EsReferencia
        //                            && a.Cuota1.PagoConceptoId != 1001
        //                      select new DTO.EstadoCuenta
        //                      {
        //                          fecha = b.FechaPago,
        //                          referenciaId = a.ReferenciaId,
        //                          concepto = "Su abono en caja... Gracias",
        //                          abono = b.Pago,
        //                          seResalta = true,
        //                          pagoid = a.PagoId
        //                      }).ToList();

        //        AbonosCaja2 = (from a in db.Pago
        //                       join b in db.PagoParcial on a.PagoId equals b.PagoId
        //                       where a.AlumnoId == alumnoId
        //                             && a.Anio == Periodo.Anio
        //                             && a.PeriodoId == Periodo.PeriodoId
        //                             && (a.EstatusId == 4 || a.EstatusId == 13 || a.EstatusId == 14)
        //                             && !b.EsReferencia
        //                             && a.Cuota1.PagoConceptoId != 1001
        //                       select new DTO.EstadoCuenta
        //                       {
        //                           fecha = b.FechaPago,
        //                           referenciaId = b.Serie + " " + b.ReciboId,
        //                           concepto = "Su abono en caja... Gracias",
        //                           abono = b.Pago,
        //                           seResalta = true,
        //                           pagoid = a.PagoId
        //                       }).ToList();

        //        AbonosReferenciasAnterioresCaja = (from a in db.Pago
        //                                           join b in db.PagoParcial on a.PagoId equals b.PagoId
        //                                           where a.AlumnoId == alumnoId
        //                                                 && a.Anio == 2016
        //                                                 && a.PeriodoId == 1
        //                                                 && (a.EstatusId == 4 || a.EstatusId == 13 || a.EstatusId == 14)
        //                                                 && !b.EsReferencia
        //                                           select new DTO.EstadoCuenta
        //                                           {
        //                                               fecha = b.FechaPago,
        //                                               referenciaId = a.ReferenciaId,
        //                                               concepto = "Su abono en caja... Gracias",
        //                                               abono = b.Pago,
        //                                               seResalta = true,
        //                                               pagoid = a.PagoId,
        //                                               adeudo = true

        //                                           }).ToList();

        //        AbonosReferenciasAnterioresCaja2 = (from a in db.Pago
        //                                            join b in db.PagoParcial on a.PagoId equals b.PagoId
        //                                            where a.AlumnoId == alumnoId
        //                                                  && a.Anio == 2016
        //                                                  && a.PeriodoId == 1
        //                                                  && (a.EstatusId == 4 || a.EstatusId == 13 || a.EstatusId == 14)
        //                                                  && !b.EsReferencia
        //                                            select new DTO.EstadoCuenta
        //                                            {
        //                                                fecha = b.FechaPago,
        //                                                referenciaId = b.Serie + " " + b.ReciboId,
        //                                                concepto = "Su abono en caja... Gracias",
        //                                                abono = b.Pago,
        //                                                seResalta = true,
        //                                                pagoid = a.PagoId,
        //                                                adeudo = true
        //                                            }).ToList();


        //        AbonosCaja.ForEach(a =>
        //        {
        //            a.referenciaId = "" + int.Parse(a.referenciaId);
        //        });

        //        AbonosCaja.ForEach(a =>
        //        {
        //            AbonosCaja2.ForEach(b =>
        //            {
        //                if (a.pagoid == b.pagoid)
        //                    a.referenciaId = a.referenciaId + " " + b.referenciaId;
        //            });

        //        });

        //        AbonosReferenciasAnterioresCaja.ForEach(a =>
        //        {
        //            a.referenciaId = "" + int.Parse(a.referenciaId);
        //        });

        //        AbonosReferenciasAnterioresCaja.ForEach(a =>
        //        {
        //            AbonosReferenciasAnterioresCaja2.ForEach(b =>
        //            {
        //                if (a.pagoid == b.pagoid)
        //                    a.referenciaId = a.referenciaId + " " + b.referenciaId;
        //            });

        //        });

        //        AbonosReferenciasAnteriores.ForEach(a =>
        //        {
        //            a.referenciaId = "" + int.Parse(a.referenciaId);
        //        });



        //        AbonosReferencias.ForEach(a =>
        //        {
        //            a.referenciaId = "" + int.Parse(a.referenciaId);
        //        });

        //        AbonosReferencias.AddRange(AbonosCaja);
        //        AbonosReferencias.AddRange(AbonosReferenciasAnteriores);
        //        AbonosReferencias.AddRange(AbonosReferenciasAnterioresCaja);
        //        Cargos.AddRange(AbonosReferencias);

        //        //Console.WriteLine("*************************************Abonos****************************************");

        //        //AbonosReferencias.ForEach(a =>
        //        //{
        //        //    Console.WriteLine(a.fecha.ToShortDateString() + " | " + a.concepto + " | " + a.referenciaId + " | " + a.cargo + " | " + a.abono);
        //        //});

        //        /********************************************************************************/
        //        /*                              Descuentos                                      */
        //        /********************************************************************************/

        //        List<DTO.EstadoCuenta> Descuentos = new List<DTO.EstadoCuenta>();

        //        Descuentos = (from a in db.PagoDescuento
        //                      join b in db.Pago on a.PagoId equals b.PagoId
        //                      join d in db.Descuento on a.DescuentoId equals d.DescuentoId
        //                      join f in db.Cuota on b.CuotaId equals f.CuotaId
        //                      join g in db.PagoConcepto on new { f.PagoConceptoId, f.OfertaEducativaId } equals new { g.PagoConceptoId, g.OfertaEducativaId }
        //                      join h in db.Subperiodo on new { b.PeriodoId, b.SubperiodoId } equals new { h.PeriodoId, h.SubperiodoId }
        //                      join i in db.Mes on h.MesId equals i.MesId
        //                      where b.AlumnoId == alumnoId
        //                            && b.Anio == Periodo.Anio
        //                            && b.PeriodoId == Periodo.PeriodoId
        //                             && (b.EstatusId != 2 && b.EstatusId != 3)
        //                      select new DTO.EstadoCuenta
        //                      {
        //                          mes = (from y in db.Subperiodo
        //                                 where y.PeriodoId == b.PeriodoId
        //                                       && y.SubperiodoId == b.SubperiodoId
        //                                 select y.MesId).FirstOrDefault(),
        //                          anio = (from z in db.Periodo
        //                                  where z.Anio == b.Anio
        //                                         && z.PeriodoId == b.PeriodoId
        //                                  select z.FechaInicial.Year).FirstOrDefault(),
        //                          fecha =

        //                          db.AlumnoDescuento
        //                            .Where(e => e.AlumnoId == b.AlumnoId && e.PagoConceptoId == b.Cuota1.PagoConceptoId)
        //                            .FirstOrDefault().FechaAplicacion ?? DateTime.Now,

        //                          concepto = a.Descuento.Descripcion + " " +
        //                          (
        //                              (d.Descripcion != "Pago Anticipado") ?

        //                              db.AlumnoDescuento
        //                                .Where(e => e.AlumnoId == b.AlumnoId &&
        //                                    e.PagoConceptoId == b.Cuota1.PagoConceptoId &&
        //                                    e.Anio == Periodo.Anio &&
        //                                    e.PeriodoId == Periodo.PeriodoId
        //                                    && (e.EstatusId == 1 || e.EstatusId == 2)).FirstOrDefault().Monto + "%"
        //                                + " " + ((f.PagoConceptoId == 800 || f.PagoConceptoId == 807) ? g.Descripcion + " " + i.Descripcion + " " + (b.PeriodoId > 1 ? b.Anio : b.FechaGeneracion.Year)
        //                                : (f.PagoConceptoId == 802) ? "" : "")


        //                                   : (f.PagoConceptoId == 800 || f.PagoConceptoId == 807 ? g.Descripcion + " " + i.Descripcion + " " + (b.PeriodoId > 1 ? b.Anio : b.FechaGeneracion.Year) :
        //                                    g.Descripcion)
        //                                )
        //                                ,
        //                          referenciaId = b.ReferenciaId,
        //                          abono = a.Monto
        //                      }).ToList();

        //        Descuentos.ForEach(a =>
        //        {
        //            a.fecha = a.subperiodoId == 1 ? a.fecha : new DateTime(a.anio, a.mes, 1);
        //            a.referenciaId = "" + int.Parse(a.referenciaId);
        //        });

        //        //Console.WriteLine("*************************************Descuentos****************************************");

        //        //Descuentos.ForEach(a =>
        //        //{
        //        //    Console.WriteLine(a.fecha.ToShortDateString() + " | " + a.concepto + " | " + a.referenciaId + " | " + a.cargo + " | " + a.abono);
        //        //});

        //        Cargos.AddRange(Descuentos);

        //        /********************************************************************************/
        //        /*                          Adeudos Periodos anteriores                         */
        //        /********************************************************************************/
        //        List<DTO.EstadoCuenta> Anteriores = new List<DTO.EstadoCuenta>();

        //        Anteriores = (from a in db.Pago
        //                      where a.Anio == 2016
        //                            && a.PeriodoId == 1
        //                            && (a.EstatusId != 2 && a.EstatusId != 3)
        //                            && a.AlumnoId == alumnoId
        //                      select new DTO.EstadoCuenta
        //                      {
        //                          referenciaId = a.ReferenciaId,
        //                          concepto = "Adeudo, periodos anteriores",
        //                          cargo = a.Promesa,
        //                          adeudo = true
        //                      }).ToList();
        //        if (Cargos.Count > 0)
        //        {
        //            DateTime FechaAdeudos = Cargos.Min(a => a.fecha).AddDays(-1);

        //            Anteriores.ForEach(a =>
        //            {
        //                a.fecha = FechaAdeudos;
        //                a.referenciaId = "" + int.Parse(a.referenciaId);
        //            });
        //        }

        //        Cargos.AddRange(Anteriores);

        //        //Console.WriteLine("*************************************Adeudos Anteriores****************************************");

        //        //Anteriores.ForEach(a =>
        //        //{
        //        //    Console.WriteLine(a.fecha.ToShortDateString() + " | " + a.concepto + " | " + a.referenciaId + " | " + a.cargo + " | " + a.abono);
        //        //});

        //        //Ordernar los movimientos
        //        Cargos.Sort((x, y) => y.fecha.CompareTo(x.fecha));
        //        Cargos.Reverse();

        //        //Console.WriteLine("*************************************Totales****************************************");

        //        //Cargos.ForEach(a =>
        //        //{
        //        //    Console.WriteLine(a.fecha.ToShortDateString() + " | " + a.concepto + " | " + a.referenciaId + " | " + a.cargo + " | " + a.abono);
        //        //});

        //        decimal cargos = 0, abonos = 0, Total = 0;
        //        Cargos.ForEach(delegate(EstadoCuenta objes)
        //        {
        //            objes.fechaS = (objes.fecha.Day < 10 ? "0" + objes.fecha.Day.ToString() : objes.fecha.Day.ToString()) + "/" +
        //                (objes.fecha.Month < 10 ? "0" + objes.fecha.Month.ToString() : objes.fecha.Month.ToString()) + "/" +
        //                objes.fecha.Year.ToString();
        //            cargos += objes.cargo;
        //            abonos += objes.abono;
        //        });

        //        Total = cargos - abonos;
        //        Cargos[0].totalAbonos = abonos.ToString("C", Cultura);
        //        Cargos[0].totalCargos = cargos.ToString("C", Cultura);
        //        Cargos[0].Total = Total.ToString("C", Cultura);
        //        return Cargos;
        //    }
        //}
    }
}

namespace BLL
{
    public class BLLEstadoCuenta
    {
        static int[] referenciaTipoId = new int[] { 1, 2 };

        public static List<Universidad.DTO.EstadoCuenta.ReferenciaProcesada> ObtenerAbonos(DTO.DTOAlumno Alumno, DateTime FechaInicial, DateTime FechaFinal)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<Universidad.DTO.EstadoCuenta.ReferenciaProcesada> Abonos =
               db.ReferenciaProcesada.AsNoTracking()
               .Where(n => n.AlumnoId == Alumno.AlumnoId &&
                       (n.FechaPago >= FechaInicial && n.FechaPago <= FechaFinal)
                       && n.Importe > 0 && n.EstatusId == 1
                       //&& referenciaTipoId.Contains(n.ReferenciaTipoId)
                       )
                   .Select(n => new Universidad.DTO.EstadoCuenta.ReferenciaProcesada
                   {
                       referenciaProcesadaId = n.ReferenciaProcesadaId,
                       fechaPago = n.FechaPago.ToString(),
                       concepto = n.ReferenciaTipoId != 4 ? (n.ReferenciaTipo.Descripcion) : "Pagos anteriores a 27 - Sep - 2016",
                       referenciaId = n.ReferenciaId,
                       abono = n.Importe.ToString(),
                       cargo = "0",
                       restante = n.Restante.ToString()

                   }).ToList();

                Abonos.ForEach(n => {
                    n.referenciaId = int.Parse(n.referenciaId).ToString();
                    n.fechaPago = DateTime.Parse(n.fechaPago).ToShortDateString();
                });

                return Abonos;
            }
        }

        public static List<Universidad.DTO.EstadoCuenta.ReferenciaProcesada> ObtenerCargos(DTO.DTOAlumno Alumno, DateTime FechaInicial, DateTime FechaFinal, List<Universidad.DTO.EstadoCuenta.ReferenciaProcesada> Abonos)
        {
            int[] ConceptosDescriptivos = new int[] { 800, 802, 807 };

            using (UniversidadEntities db = new UniversidadEntities())
            {
                Abonos.ForEach(n =>
                {
                    n.Pagos = (db.PagoParcial.Where(s => s.ReferenciaProcesadaId == n.referenciaProcesadaId && s.EstatusId == 4 && s.Pago1.Cuota1.PagoConceptoId != 1007)
                        .Select(s => new Universidad.DTO.EstadoCuenta.PagoParcial
                        {
                            referenciaProcesadaId = s.ReferenciaProcesadaId,
                            fechaPago = s.Pago1.FechaGeneracion.ToString(),
                            concepto = ConceptosDescriptivos.Contains(s.Pago1.Cuota1.PagoConceptoId)
                                       ? s.Pago1.Cuota1.PagoConcepto.Descripcion + " " + s.Pago1.Subperiodo.Mes.Descripcion + " " + (s.Pago1.PeriodoId > 1 ? s.Pago1.Anio : s.Pago1.FechaGeneracion.Year)
                                       : s.Pago1.Cuota1.PagoConceptoId == 306
                                                ? db.PagoRecargo.Where(r => r.PagoIdRecargo == s.PagoId).FirstOrDefault().Pago.Cuota1.PagoConceptoId == 800
                                                            ? s.Pago1.Cuota1.PagoConcepto.Descripcion + " " + db.PagoRecargo.Where(r => r.PagoIdRecargo == s.PagoId).FirstOrDefault().Pago.Cuota1.PagoConcepto.Descripcion + " " + s.Pago1.Subperiodo.Mes.Descripcion + " " + (s.Pago1.PeriodoId > 1 ? s.Pago1.Anio : s.Pago1.FechaGeneracion.Year)
                                                            : s.Pago1.Cuota1.PagoConcepto.Descripcion + " " + db.PagoRecargo.Where(r => r.PagoIdRecargo == s.PagoId).FirstOrDefault().Pago.Cuota1.PagoConcepto.Descripcion
                                                : s.Pago1.Cuota1.PagoConcepto.Descripcion
                            ,
                            pagoId = s.PagoId.ToString(),
                            reciboId = (s.ReciboId ?? 0).ToString(),
                            referenciaId = s.Pago1.ReferenciaId,
                            abono = "0",
                            cargo = s.Pago.ToString(),
                            restante = "0",
                            anio = (db.Periodo.Where(p => p.Anio == s.Pago1.Anio && p.PeriodoId == s.Pago1.PeriodoId).FirstOrDefault().FechaFinal.Year),
                            mesId = s.Pago1.Subperiodo.Mes.MesId,
                            conceptodescriptivo = ConceptosDescriptivos.Contains(s.Pago1.Cuota1.PagoConceptoId) || s.Pago1.Cuota1.PagoConceptoId == 306 ? true : false
                        }).ToList());
                });

                Abonos.ForEach(n =>
                {
                    n.Pagos.ForEach(s =>
                    {
                        s.referenciaId = int.Parse(s.referenciaId).ToString();
                        s.fechaPago = s.conceptodescriptivo ? new DateTime(s.anio, s.mesId, 1).ToShortDateString() : DateTime.Parse(s.fechaPago).ToShortDateString();
                        s.concepto = s.anio < 2016 ? "Adeudos de 2015 y anteriores" : s.concepto;
                    });
                });

                Abonos.Add(new Universidad.DTO.EstadoCuenta.ReferenciaProcesada
                {
                    referenciaProcesadaId = 0,
                    fechaPago = "",
                    concepto = "Saldo pendiente por aplicar",
                    referenciaId = "",
                    abono = "0",
                    cargo = "0",
                    restante = db.ReferenciaProcesada.Where(n => n.AlumnoId == Alumno.AlumnoId).Sum(n => n.Restante).ToString(),
                    Pagos = null
                });

                Abonos.ForEach(n => {

                    n.abono = decimal.Parse(n.abono) == 0 ? "" : "$ " + n.abono;
                    n.cargo = decimal.Parse(n.cargo) == 0 ? "" : "$ " + n.cargo;
                    n.restante = decimal.Parse(n.restante) == 0 ? "" : "$ " + n.restante;

                    if (n.Pagos != null)
                        n.Pagos.ForEach(s =>
                        {
                            s.abono = decimal.Parse(s.abono) == 0 ? "" : "$ " + s.abono;
                            s.cargo = decimal.Parse(s.cargo) == 0 ? "" : "$ " + s.cargo;
                            s.restante = decimal.Parse(s.restante) == 0 ? "" : "$ " + s.restante;
                        });
                });

                Abonos.OrderByDescending(n => n.referenciaProcesadaId);
                Abonos.Reverse();

                #region Impresión
                /*
                Abonos.ForEach(n =>
                {
                    string titulos = "";
                    string registro = "";
                    string detalle = "";
                    string[] Columnas = new string[] { "referenciaProcesadaId", "fechaPago", "concepto", "referenciaId", "abono", "cargo", "restante" };

                    n.abono = (n.referenciaProcesadaId != 0) ? (n.abono) : "";
                    n.cargo = (n.referenciaProcesadaId != 0) ? n.cargo : "";
                    n.restante = (n.referenciaProcesadaId != 0) ? n.restante: n.restante;

                    foreach (PropertyDescriptor d in TypeDescriptor.GetProperties(n))
                    {
                        if (d.Name != "Pagos" && (Columnas.Contains(d.Name)))
                        {
                            titulos += d.Name + string.Concat(Enumerable.Repeat(" ", 50 - d.Name.Length));
                            if (d.Name == "concepto")
                                registro += d.GetValue(n) + string.Concat(Enumerable.Repeat(" ", 50 - d.GetValue(n).ToString().Length));
                            else
                                registro += d.GetValue(n) + string.Concat(Enumerable.Repeat(" ", 50 - d.GetValue(n).ToString().Length));
                        }
                       
                    }

                    Console.WriteLine(titulos);
                    Console.WriteLine(registro);
                    Console.WriteLine(detalle);
                    Console.WriteLine("*********************************");
                    Console.WriteLine("\n");

                });
                */
                #endregion Impresion

                #region Impresión 2

                Abonos.ForEach(n => {
                    Console.WriteLine(n.referenciaProcesadaId + " " + n.fechaPago + " " + n.concepto + " " + n.referenciaId + " " + n.abono + " " + n.cargo + " " + n.restante);
                    if (n.Pagos != null)
                        n.Pagos.ForEach(s =>
                        {
                            Console.WriteLine("      " + s.referenciaProcesadaId + " " + s.fechaPago + " " + s.concepto + " " + s.referenciaId + " " + s.abono + " " + s.cargo + " " + s.restante);
                        });

                    Console.WriteLine("*********************************************************");
                });

                #endregion Impresión 2
                return Abonos;
            }
        }
    }
}
