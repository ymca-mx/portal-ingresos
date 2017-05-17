using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data.Entity;
using System.Globalization;
using System.IO;


namespace BLL
{
    public class BLLDescuentos
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");
        public static DTODescuentos obtenerDescuentos(int OfertaEducativa, int PagoConceptoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from c in db.Descuento
                        where c.OfertaEducativaId == OfertaEducativa && c.PagoConceptoId == PagoConceptoId
                        select new DTODescuentos
                        {
                            DescuentoId = c.DescuentoId,
                            PagoConceptoId = c.PagoConceptoId,
                            DescuentoTipoId = c.DescuentoTipoId,
                            OfertaEducativaId = c.OfertaEducativaId,
                            MontoMaximo = c.MontoMaximo,
                            MontoMinimo = c.MontoMinimo,
                            Descripcion = c.Descripcion
                        }).FirstOrDefault();
            }
        }
        public static DTODescuentos obtenerDescuentos(int OfertaEducativa, int PagoConceptoId, string Descripcion)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from c in db.Descuento
                        where c.OfertaEducativaId == OfertaEducativa && c.PagoConceptoId == PagoConceptoId && c.Descripcion == Descripcion
                        select new DTODescuentos
                        {
                            DescuentoId = c.DescuentoId,
                            PagoConceptoId = c.PagoConceptoId,
                            DescuentoTipoId = c.DescuentoTipoId,
                            OfertaEducativaId = c.OfertaEducativaId,
                            MontoMaximo = c.MontoMaximo,
                            MontoMinimo = c.MontoMinimo,
                            Descripcion = c.Descripcion,
                        }).FirstOrDefault();
            }
        }
        public static List<DTOAlumnoDescuento> ObtenerDescuentosAlumno(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTOPeriodo objPeriodo = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);
                DTOAlumno objAlumno = BLLAlumnoPortal.ObtenerAlumno(AlumnoId);
                List<DTOAlumnoDescuento> lstDescuentos = (from b in db.AlumnoDescuento
                                                          where b.AlumnoId == objAlumno.AlumnoId
                                                          select new DTOAlumnoDescuento
                                                          {
                                                              AlumnoDescuentoId = b.AlumnoDescuentoId,
                                                              AlumnoId = b.AlumnoId,
                                                              Anio = b.Anio,
                                                              ConceptoId = b.PagoConceptoId,
                                                              DescuentoId = b.DescuentoId,
                                                              EstatusId = b.EstatusId,
                                                              Monto = b.Monto,
                                                              OfertaEducativaId = b.OfertaEducativaId,
                                                              PeriodoId = b.PeriodoId,
                                                              Descuento = (from a in db.Descuento
                                                                           where a.DescuentoId == b.DescuentoId
                                                                           select new DTODescuentos
                                                                           {
                                                                               CuentaContable = a.CuentaContable,
                                                                               Descripcion = a.Descripcion,
                                                                               DescuentoId = a.DescuentoId,
                                                                               DescuentoTipoId = a.DescuentoTipoId,
                                                                               FechaFinal = a.FechaFinal,
                                                                               FechaInicial = a.FechaInicial,
                                                                               MontoMaximo = a.MontoMaximo,
                                                                               MontoMinimo = a.MontoMinimo,
                                                                               OfertaEducativaId = a.OfertaEducativaId,
                                                                               PagoConceptoId = a.PagoConceptoId
                                                                           }).FirstOrDefault()
                                                          }).AsNoTracking().ToList();

                //lstDescuentos = lstDescuentos.Where(P => P.Anio != objPeriodo.Anio && P.PeriodoId != objPeriodo.PeriodoId).ToList();
                lstDescuentos.RemoveAll(P => P.Anio == objPeriodo.Anio && P.PeriodoId == objPeriodo.PeriodoId);
                // List<DTODescuentos> lstDescuen=new List<DTODescuentos>();
                //lstDescuentos.ForEach(delegate(DTOAlumnoDescuento objAlDes)
                //{
                //    lstDescuen.Add((from a in db.Descuento
                //                    where a.DescuentoId == objAlDes.DescuentoId
                //                    select new DTODescuentos
                //                    {
                //                        CuentaContable = a.CuentaContable,
                //                        Descripcion = a.Descripcion,
                //                        DescuentoId = a.DescuentoId,
                //                        DescuentoTipoId = a.DescuentoTipoId,
                //                        FechaFinal = a.FechaFinal,
                //                        FechaInicial = a.FechaInicial,
                //                        MontoMaximo = a.MontoMaximo,
                //                        MontoMinimo = a.MontoMinimo,
                //                        OfertaEducativaId = a.OfertaEducativaId,
                //                        PagoConceptoId = a.PagoConceptoId
                //                    }).FirstOrDefault());
                //});
                return lstDescuentos;
            }
        }

        public static string GenerarDescuento(int AlumnoId, int UsuarioId, decimal PBeca, Boolean Sep, int DescuentoIdBeca, int DescuentoIdSep)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DTOAlumnoInscrito objAlumnoIn = BLLAlumnoInscrito.ConsultarAlumnoInscrito(AlumnoId);

                    DTODescuentos objDescuento = DescuentoIdSep == 0 ? obtenerDescuentos(objAlumnoIn.OfertaEducativaId, 800, "Beca SEP") :
                        new DTODescuentos { DescuentoId = DescuentoIdSep };

                    DTOPeriodo objPeriodo = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);

                    DTOAlumnoDescuento objalDesc = Sep == false ? BLLAlumnoDescuento.ObtenerDescuentoAlumno(AlumnoId,
                        DescuentoIdBeca == 0 ? obtenerDescuentos(objAlumnoIn.OfertaEducativaId, 800, "Beca Académica").DescuentoId : DescuentoIdBeca) :
                        BLLAlumnoDescuento.ObtenerDescuentoAlumno(AlumnoId, objDescuento.DescuentoId);
                    //List<Pago> lstPagos=db.Pago.Where(P=>P.AlumnoId==AlumnoId && P.PeriodoId==objPeriodo.PeriodoId && P.Anio==objPeriodo.Anio &&P.OfertaEducativaId==)
                    if (PBeca > 0)
                    {
                        db.AlumnoDescuento.Add(new AlumnoDescuento
                        {
                            AlumnoId = objalDesc.AlumnoId,
                            OfertaEducativaId = objalDesc.OfertaEducativaId,
                            Anio = objPeriodo.Anio,
                            PeriodoId = objPeriodo.PeriodoId,
                            DescuentoId = objalDesc.DescuentoId,
                            PagoConceptoId = objalDesc.ConceptoId,
                            Monto = PBeca,
                            UsuarioId = UsuarioId,
                            Comentario = "",
                            EstatusId = 1,
                        });
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

        public static string GenerarDescuento(int AlumnoId, int UsuarioId, decimal MontoInscr, decimal MontoFianc, int DescuentoIdInscr, int DescuentoIdFinan)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTOAlumnoInscrito objAlumnoIn = BLLAlumnoInscrito.ConsultarAlumnoInscrito(AlumnoId);
                DTOPeriodo objPeriodo = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);

                DTOAlumnoDescuento objInscr = db.OfertaEducativa.Where(O => O.OfertaEducativaId == objAlumnoIn.OfertaEducativaId).FirstOrDefault().OfertaEducativaTipoId == 4 ?
                    BLLAlumnoDescuento.ObtenerDescuentoAlumno(AlumnoId, (DescuentoIdInscr == 0 ? obtenerDescuentos(objAlumnoIn.OfertaEducativaId, 807, "Descuento en inscripción").DescuentoId : DescuentoIdInscr)) :
                     BLLAlumnoDescuento.ObtenerDescuentoAlumno(AlumnoId, (DescuentoIdInscr == 0 ? obtenerDescuentos(objAlumnoIn.OfertaEducativaId, 802, "Descuento en inscripción").DescuentoId : DescuentoIdInscr));

                DTOAlumnoDescuento objFinan = BLLAlumnoDescuento.ObtenerDescuentoAlumno(AlumnoId, (DescuentoIdFinan == 0 ? obtenerDescuentos(objAlumnoIn.OfertaEducativaId, 800, "Financiamiento").DescuentoId : DescuentoIdFinan));
                if (MontoInscr > 0)
                {
                    db.AlumnoDescuento.Add(new AlumnoDescuento
                    {
                        AlumnoId = objInscr.AlumnoId,
                        OfertaEducativaId = objInscr.OfertaEducativaId,
                        Anio = objPeriodo.Anio,
                        PeriodoId = objPeriodo.PeriodoId,
                        DescuentoId = objInscr.DescuentoId,
                        PagoConceptoId = objInscr.ConceptoId,
                        Monto = MontoInscr,
                        UsuarioId = UsuarioId,
                        Comentario = "",
                        EstatusId = 1
                    });
                }
                if (DescuentoIdFinan > 0)
                {
                    db.AlumnoDescuento.Add(new AlumnoDescuento
                    {
                        AlumnoId = objFinan.AlumnoId,
                        OfertaEducativaId = objFinan.OfertaEducativaId,
                        Anio = objPeriodo.Anio,
                        PeriodoId = objPeriodo.PeriodoId,
                        DescuentoId = objFinan.DescuentoId,
                        PagoConceptoId = objFinan.ConceptoId,
                        Monto = MontoFianc,
                        UsuarioId = UsuarioId,
                        Comentario = "",
                        EstatusId = 1
                    });
                }

                db.SaveChanges();
                return "Guardado";
            }

            throw new NotImplementedException();
        }

        public static List<DTOAlumnoDescuento> ObtenerDescuentosAlumno(int AlumnoId, int OfertaEducativaid)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTOPeriodo objPeriodo = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);
                //DTOAlumno objAlumno = BLLAlumno.ObtenerAlumno(AlumnoId);
                List<DTOAlumnoDescuento> lstDescuentos = (from b in db.AlumnoDescuento
                                                          where b.AlumnoId == AlumnoId && b.OfertaEducativaId == OfertaEducativaid
                                                          select new DTOAlumnoDescuento
                                                          {
                                                              AlumnoDescuentoId = b.AlumnoDescuentoId,
                                                              AlumnoId = b.AlumnoId,
                                                              Anio = b.Anio,
                                                              ConceptoId = b.PagoConceptoId,
                                                              DescuentoId = b.DescuentoId,
                                                              EstatusId = b.EstatusId,
                                                              Monto = b.Monto,
                                                              OfertaEducativaId = b.OfertaEducativaId,
                                                              PeriodoId = b.PeriodoId,
                                                              Descuento = (from a in db.Descuento
                                                                           where a.DescuentoId == b.DescuentoId
                                                                           select new DTODescuentos
                                                                           {
                                                                               CuentaContable = a.CuentaContable,
                                                                               Descripcion = a.Descripcion,
                                                                               DescuentoId = a.DescuentoId,
                                                                               DescuentoTipoId = a.DescuentoTipoId,
                                                                               FechaFinal = a.FechaFinal,
                                                                               FechaInicial = a.FechaInicial,
                                                                               MontoMaximo = a.MontoMaximo,
                                                                               MontoMinimo = a.MontoMinimo,
                                                                               OfertaEducativaId = a.OfertaEducativaId,
                                                                               PagoConceptoId = a.PagoConceptoId
                                                                           }).FirstOrDefault()
                                                          }).AsNoTracking().ToList();

                //lstDescuentos = lstDescuentos.Where(P => P.Anio != objPeriodo.Anio && P.PeriodoId != objPeriodo.PeriodoId).ToList();
                lstDescuentos.RemoveAll(P => P.Anio == objPeriodo.Anio && P.PeriodoId == objPeriodo.PeriodoId);
                // List<DTODescuentos> lstDescuen=new List<DTODescuentos>();
                //lstDescuentos.ForEach(delegate(DTOAlumnoDescuento objAlDes)
                //{
                //    lstDescuen.Add((from a in db.Descuento
                //                    where a.DescuentoId == objAlDes.DescuentoId
                //                    select new DTODescuentos
                //                    {
                //                        CuentaContable = a.CuentaContable,
                //                        Descripcion = a.Descripcion,
                //                        DescuentoId = a.DescuentoId,
                //                        DescuentoTipoId = a.DescuentoTipoId,
                //                        FechaFinal = a.FechaFinal,
                //                        FechaInicial = a.FechaInicial,
                //                        MontoMaximo = a.MontoMaximo,
                //                        MontoMinimo = a.MontoMinimo,
                //                        OfertaEducativaId = a.OfertaEducativaId,
                //                        PagoConceptoId = a.PagoConceptoId
                //                    }).FirstOrDefault());
                //});
                return lstDescuentos;
            }
        }

        public static string GenerarDescuento(int AlumnoId, int UsuarioId, decimal Beca, Boolean SEP, int DescuentoBeca, int DescuentoSEP, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DTOAlumnoInscrito objAlumnoIn = BLLAlumnoInscrito.ConsultarAlumnoInscrito(AlumnoId, OfertaEducativaId);

                    DTODescuentos objDescuento = DescuentoSEP == 0 ? obtenerDescuentos(objAlumnoIn.OfertaEducativaId, 800, "Beca SEP") :
                        new DTODescuentos { DescuentoId = DescuentoSEP };

                    DTOPeriodo objPeriodo = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);

                    DTOAlumnoDescuento objalDesc = SEP == false ? BLLAlumnoDescuento.ObtenerDescuentoAlumno(AlumnoId,
                        DescuentoBeca == 0 ? obtenerDescuentos(objAlumnoIn.OfertaEducativaId, 800, "Beca Académica").DescuentoId : DescuentoBeca) :
                        BLLAlumnoDescuento.ObtenerDescuentoAlumno(AlumnoId, objDescuento.DescuentoId);
                    if (Beca > 0)
                    {
                        db.AlumnoDescuento.Add(new AlumnoDescuento
                        {
                            AlumnoId = objalDesc.AlumnoId,
                            OfertaEducativaId = objalDesc.OfertaEducativaId,
                            Anio = objPeriodo.Anio,
                            PeriodoId = objPeriodo.PeriodoId,
                            DescuentoId = objalDesc.DescuentoId,
                            PagoConceptoId = objalDesc.ConceptoId,
                            Monto = Beca,
                            UsuarioId = UsuarioId,
                            Comentario = "",
                            EstatusId = 1
                        });

                        db.SaveChanges();

                        //List<Pago> lstPagos = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.PeriodoId == objPeriodo.PeriodoId && P.Anio == objPeriodo.Anio && P.OfertaEducativaId == OfertaEducativaId).ToList();
                        //lstPagos.ForEach(delegate(Pago objPago){
                        //    objPago.Promesa=(Beca/100)*
                        //})
                    }
                    return "Guardado";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }

        }

        public static List<DTOAlumnoDescuento> TraerDescuentos(int AlumnoId, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                try
                {
                    DTOPeriodo objPeriodoActual = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);

                    List< DTOAlumnoInscritoBecaDocumento> lstDocumentosAlumno = db.AlumnoInscritoDocumento
                        .Where(a => a.AlumnoId == AlumnoId
                        && a.OfertaEducativaId == OfertaEducativaId)
                            .Select(l=> new DTOAlumnoInscritoBecaDocumento
                            {
                                AlumnoInscritoDocumentoId=l.AlumnoInscritoDocumentoId,
                                AlumnoId=l.AlumnoId,
                                Anio=l.Anio,
                                OfertaEducativaId=l.OfertaEducativaId,
                                PeriodoId=l.PeriodoId,
                                TipoDocumentoId=l.TipoDocumento
                            })
                            .ToList();
                    
                    List<DTOAlumnoDescuento> lstAlumnoDescuento = new List<DTOAlumnoDescuento>();
                    List<DTOAlumnoInscrito> lstHistorico = db.AlumnoInscrito.Where(a => a.AlumnoId == AlumnoId
                    && a.OfertaEducativaId == OfertaEducativaId )
                        .ToList().Select(s => new DTOAlumnoInscrito
                        {
                            AlumnoId = s.AlumnoId,
                            PeriodoId = s.PeriodoId,
                            OfertaEducativaId = s.OfertaEducativaId,
                            Anio = s.Anio,
                            EsEmpresa = s.EsEmpresa
                        }).ToList();
                    lstHistorico.AddRange(db.AlumnoInscritoBitacora.Where(a => a.AlumnoId == AlumnoId
                    && a.OfertaEducativaId == OfertaEducativaId )
                        .ToList().Select(s => new DTOAlumnoInscrito
                        {
                            AlumnoId = s.AlumnoId,
                            PeriodoId = s.PeriodoId,
                            OfertaEducativaId = s.OfertaEducativaId,
                            Anio = s.Anio,
                            EsEmpresa = s.EsEmpresa
                        }).ToList());

                    lstHistorico = lstHistorico.GroupBy(a => new
                    {
                        a.Anio,
                        a.PeriodoId
                    }).Select(a => a.FirstOrDefault()).ToList();

                                       

                    List<AlumnoDescuento> lstAlDescuento = db.AlumnoDescuento.Where(d => d.PagoConceptoId == 800 && d.AlumnoId == AlumnoId
                     && d.OfertaEducativaId == OfertaEducativaId && d.EstatusId == 2).ToList();

                    List<AlumnoInscritoDetalle> lstAlDetalle = db.AlumnoInscritoDetalle.Where(ad => ad.AlumnoId == AlumnoId &&
                    ad.OfertaEducativaId == OfertaEducativaId).ToList();

                    lstAlDescuento.ForEach(a =>
                    {

                        DTOAlumnoInscrito objAlInscrito = new DTOAlumnoInscrito();
                        objAlInscrito = lstHistorico.Where(O => a.Anio == O.Anio
                                                                   && a.PeriodoId == O.PeriodoId
                                                                   && a.OfertaEducativaId == O.OfertaEducativaId)
                                                                .ToList().Count > 0 ?
                                                                lstHistorico.Where(O => a.Anio == O.Anio
                                                                && a.PeriodoId == O.PeriodoId
                                                                && a.OfertaEducativaId == O.OfertaEducativaId).FirstOrDefault() :
                                                                new DTOAlumnoInscrito
                                                                {
                                                                    EsEmpresa = false,
                                                                    AlumnoId = -1
                                                                };

                        if (objAlInscrito.AlumnoId != -1)
                        {

                            bool esBeca = db.Descuento.Where(i => i.DescuentoId == a.DescuentoId)
                                            .ToList()
                                                .Where(D => D.Descripcion == "Beca Académica" || D.Descripcion == "Beca SEP").ToList().Count > 0
                                                ?
                                                true : false;
                            lstAlumnoDescuento.Add(
                                new DTOAlumnoDescuento
                                {
                                //AlumnoDescuentoId = a.AlumnoDescuentoId,
                                AlumnoId = a.AlumnoId,
                                    Anio = a.Anio,
                                    ConceptoId = a.PagoConceptoId,
                                    DescuentoId = a.DescuentoId,
                                    SMonto = !esBeca || a.EsDeportiva || objAlInscrito.EsEmpresa ? ""
                                                : ("" + a.Monto) + "%",
                                    OtrosDescuentos = objAlInscrito.EsEmpresa ? "" + a.Monto : "",
                                    BecaDeportiva = a.EsDeportiva ? ("" + a.Monto) + "%" : "",
                                    PeriodoId = a.PeriodoId,
                                    AnioPeriodoId = ("" + a.Anio + " " + a.PeriodoId),
                                    BecaSEP = (a.EsSEP ? "Si" : ""),
                                    DescripcionPeriodo = db.Periodo.Where(p1 => p1.Anio == a.Anio && p1.PeriodoId == a.PeriodoId).FirstOrDefault().Descripcion,
                                    Usuario = db.Usuario.Where(u => u.UsuarioId == a.UsuarioId).ToList().
                                              Select(a1 => new DTOUsuario
                                              {
                                                  UsuarioId = a1.UsuarioId,
                                                  Nombre = a1.Nombre
                                              }).FirstOrDefault(),
                                    BecaComite = (a.EsComite ? "Si" : ""),
                                    esEmpresa = objAlInscrito.EsEmpresa,
                                    FechaAplicacionS = a.FechaAplicacion.Value.ToString("dd/MM/yyyy", Cultura),
                                    DocComiteRutaId = "" + lstDocumentosAlumno.Where(o => a.Anio == o.Anio
                                                        && a.PeriodoId == o.PeriodoId
                                                        && o.TipoDocumentoId == 2).FirstOrDefault()?.AlumnoInscritoDocumentoId ?? "-1",

                                    DocAcademicaId = "" + lstDocumentosAlumno.Where(o => a.Anio == o.Anio
                                                           && a.PeriodoId == o.PeriodoId
                                                           && o.TipoDocumentoId == 1).FirstOrDefault()?.AlumnoInscritoDocumentoId ?? "-1"
                                });
                        }
                    });

                    lstAlumnoDescuento = lstAlumnoDescuento.OrderBy(a => a.Anio).ThenBy(s => s.PeriodoId).ToList();
                    return lstAlumnoDescuento;

                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
