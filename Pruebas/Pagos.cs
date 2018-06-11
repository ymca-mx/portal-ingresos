using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Globalization;

namespace Pruebas
{
    [TestClass]
    public class Pagos
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");

        [TestMethod]
        public void PagosSemestrales()
        {
            BLL.BLLPagoPortal.GenerarSemestre(6704, 44, 2017, 11, 2018, 4, 8358, 0, 3600);
        }


        [TestMethod]
        public void GetPagos()
        {
            int AlumnoId = 7626;

            using (UniversidadEntities db = new UniversidadEntities())
            {

                var ListaPagos = db.Pago.Where(P => P.AlumnoId == AlumnoId
                                                        && P.EstatusId != 2
                                                        && (P.Cuota1.PagoConceptoId != 1007 && P.Cuota1.PagoConceptoId != 1001))
                                            .AsNoTracking()
                                            .Select(pago => new
                                            {
                                                Alumno = new
                                                {
                                                    AlumnoInscrito = pago.Alumno.AlumnoInscrito.Select(a => new
                                                    {
                                                        a.AlumnoId,
                                                        a.OfertaEducativaId,
                                                        a.EsEmpresa
                                                    })
                                                },
                                                pago.FechaGeneracion,
                                                pago.AlumnoId,
                                                pago.PagoId,
                                                pago.ReferenciaId,
                                                pago.Anio,
                                                pago.PeriodoId,
                                                pago.SubperiodoId,
                                                Subperiodo = new
                                                {
                                                    Mes = new { pago.Subperiodo.Mes.Descripcion }
                                                },
                                                pago.EstatusId,
                                                pago.Promesa,
                                                pago.Restante,
                                                pago.Cuota,
                                                Cuota1 = new
                                                {
                                                    pago.Cuota1.CuotaId,
                                                    pago.Cuota1.PagoConceptoId,
                                                    PagoConcepto = new
                                                    {
                                                        pago.Cuota1.PagoConcepto.Descripcion
                                                    }
                                                },
                                                Periodo = new
                                                {
                                                    pago.PeriodoId,
                                                    pago.Anio,
                                                    pago.Periodo.Descripcion
                                                },
                                                PagoDescripcion=new 
                                                {
                                                    pago.PagoDescripcion.Descripcion,
                                                },
                                                pago.OfertaEducativaId,
                                                OfertaEducativa = new
                                                {
                                                    pago.OfertaEducativa.Descripcion,
                                                    pago.OfertaEducativa.OfertaEducativaTipoId
                                                },
                                                PagoDescuento = pago.PagoDescuento
                                                       .Select(pa => new
                                                       {
                                                           pa.PagoId,
                                                           pa.DescuentoId,
                                                           pa.Monto,
                                                           Descuento = new { pa.Descuento.Descripcion }
                                                       }).ToList(),
                                                PagoRecargo1 = pago.PagoRecargo1.Select(pa => new
                                                {
                                                    pa.PagoId,
                                                    pa.PagoRecargoId,
                                                    pa.PagoIdRecargo,
                                                    Pago = new
                                                    {
                                                        pa.Pago.SubperiodoId,
                                                        pa.Pago.PeriodoId,
                                                        pa.Pago.Anio,
                                                        pa.Pago.PagoId,
                                                        Subperiodo= new
                                                        {
                                                            pa.Pago.SubperiodoId,
                                                            Mes= new
                                                            {
                                                                pa.Pago.Subperiodo.Mes.Descripcion,
                                                                pa.Pago.Subperiodo.Mes.MesId
                                                            }
                                                        },
                                                        Cuota1 = new
                                                        {
                                                            pa.Pago.Cuota1,
                                                            pa.Pago.Cuota1.PagoConceptoId,
                                                            PagoConcepto =  new
                                                            {
                                                                pa.Pago.Cuota1.PagoConcepto.Descripcion,
                                                                pa.Pago.Cuota1.PagoConcepto.PagoConceptoId,
                                                            }
                                                        }
                                                    }
                                                }).ToList()
                                            })
                                            .OrderByDescending(a=> a.Anio)
                                            .ThenBy(a=> a.PeriodoId)
                                            .ToList();

                var Periodos = ListaPagos.Where(P => P.AlumnoId == AlumnoId
                                                        && P.EstatusId != 2
                                                        && (P.Cuota1.PagoConceptoId != 1007 && P.Cuota1.PagoConceptoId != 1001)
                                                        && (P.Anio != 2016 || P.PeriodoId != 1 || (P.EstatusId == 14 || P.EstatusId == 4)))
                                            .Select(p => p.Periodo)
                                            .GroupBy(p => new { p.Anio, p.PeriodoId })
                                            .Select(p => p.FirstOrDefault())
                                            .ToList()
                                            .OrderBy(a => a.Anio).ThenBy(a => a.PeriodoId)
                                            .ToList();

                List<AlumnoDescuento> ListaDescuentosAlumno = db.AlumnoDescuento
                                                       .Where(ad => ad.AlumnoId == AlumnoId
                                                                    && ad.EstatusId != 3)
                                                        .ToList();


                var Adeudos = ListaPagos.Where(P => P.AlumnoId == AlumnoId && P.Anio == 2016 && P.PeriodoId == 1
                        && (P.EstatusId == 1 || P.EstatusId == 13))
                        .ToList();

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
                        esEmpresa = true
                    });
                }

                if (Periodos.Where(per => per.Anio == 2016 && per.PeriodoId == 1).ToList().Count == 0 && PagosDetalles.Count > 0)
                {
                    Periodos.Insert(0, new 
                    {
                        PeriodoId = 1,
                        Anio = 2016,                        
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
                                                                     BecaSEP = per.Anio == 2016 && per.PeriodoId == 1 ? "" : "",
                                                                     EsEmpresa = per.Anio == 2016 && per.PeriodoId == 1 ? true : false,
                                                                 }).ToList();

                Periodos.ForEach(Periodobd =>
                {
                    int ofertaid = 0;
                    var Pagos = ListaPagos.Where(P => P.AlumnoId == AlumnoId
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
                                var PagosAnticipados = Pago.PagoDescuento.Where(PD => PD.Descuento.Descripcion == "Pago Anticipado").ToList();
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

                if (PagosDetalles.Count > 0)
                {
                    int ofert = PagosDetalles?[0]?.OfertaEducativaId ?? 0;

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
                    PagosDetalles.ForEach(p => Console.WriteLine(p.ReferenciaId + " " + p.DescripcionOferta + " " + p.CargoFechaLimite + p.CargoMonto));
                    PeriodosDTO.ForEach(p => Console.WriteLine(p.Descripcion));
                    Console.Write(
                        new PantallaPago
                        {
                            Pagos = PagosDetalles,
                            Estatus = PagosDetalles.Where(l => (l?.OtroDescuento?.Length ?? 0) > 0).ToList().Count > 0 ? true : false,
                            Periodos = PeriodosDTO
                        }
                    );
                }
                else
                {
                    Console.Write(
                       new PantallaPago
                       {
                           Pagos = new List<DTOPagoDetallado>(),
                           Estatus = false,
                           Periodos = new List<DTOPeriodoReferencias>()
                       });
                }
            }
        }

        [TestMethod]
        public void GetPagosV2()
        {
            int AlumnoId = 7626;
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
                                            .OrderBy(a => a.Anio).ThenBy(a => a.PeriodoId)
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
                        esEmpresa = true
                    });
                }

                if (Periodos.Where(per => per.Anio == 2016 && per.PeriodoId == 1).ToList().Count == 0 && PagosDetalles.Count > 0)
                {
                    Periodos.Insert(0, new Periodo
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
                                                                     BecaSEP = per.Anio == 2016 && per.PeriodoId == 1 ? "" : "",
                                                                     EsEmpresa = per.Anio == 2016 && per.PeriodoId == 1 ? true : false,
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

                if (PagosDetalles.Count > 0)
                {
                    int ofert = PagosDetalles?[0]?.OfertaEducativaId ?? 0;

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

                    PagosDetalles.ForEach(p => Console.WriteLine(p.ReferenciaId + " " + p.DescripcionOferta + " " + p.CargoFechaLimite + p.CargoMonto));
                    PeriodosDTO.ForEach(p => Console.WriteLine(p.Descripcion));

                    Console.Write(
                        new PantallaPago
                        {
                            Pagos = PagosDetalles,
                            Estatus = PagosDetalles.Where(l => (l?.OtroDescuento?.Length ?? 0) > 0).ToList().Count > 0 ? true : false,
                            Periodos = PeriodosDTO
                        });
                }
                else
                {
                    Console.Write(
                       new PantallaPago
                       {
                           Pagos = new List<DTOPagoDetallado>(),
                           Estatus = false,
                           Periodos = new List<DTOPeriodoReferencias>()
                       });
                }
            }
        }
    }

    
}
