using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data.Entity;
namespace BLL
{
    public class BLLPagoConcepto
    {
        public static object ListaPagoConceptos(int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {

                    DTOPeriodo PeriodoActual = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);

                    var Conceptos2 = db.Cuota
                         .Where(c => c.OfertaEducativaId == OfertaEducativaId
                                     && c.Anio == PeriodoActual.Anio
                                     && c.PeriodoId == PeriodoActual.PeriodoId
                                     && !c.PagoConcepto.EsVariable
                                     && c.PagoConcepto.EsVisible
                                     && c.PagoConceptoId != 807)
                         .Select(cu => new
                         {
                             cu.CuotaId,
                             cu.PagoConceptoId,
                             cu.PagoConcepto.Descripcion,
                             cu.OfertaEducativaId,
                             cu.Monto,
                             cu.PagoConcepto.EsMultireferencia
                         })
                         .ToList();

                    Conceptos2.Add(
                            db.Cuota
                            .Where(c => c.OfertaEducativaId == OfertaEducativaId
                                        && c.Anio == PeriodoActual.Anio
                                        && c.PeriodoId == PeriodoActual.PeriodoId
                                        && !c.PagoConcepto.EsVariable
                                        && c.PagoConcepto.EsVisible
                                        && c.PagoConceptoId == 807)
                            .Select(cu => new
                            {
                                cu.CuotaId,
                                cu.PagoConceptoId,
                                cu.PagoConcepto.Descripcion,
                                cu.OfertaEducativaId,
                                cu.Monto,
                                cu.PagoConcepto.EsMultireferencia
                            })
                            .ToList()?
                            .OrderByDescending(a => a.Monto)?
                            .FirstOrDefault() ?? null);

                    Conceptos2 = Conceptos2.Where(a => a != null).ToList();

                    return Conceptos2.OrderBy(a => a.Descripcion).ToList();


                }
                catch (Exception error)
                {
                    return new
                    {
                        error.Message,
                        Inner = error?.InnerException?.Message
                    };
                }
            }            
        }

        public static DTOPagoConcepto TraerPagoConcepto(int OfertaEducativaId, int PagoConceptoId)
        {
            using (UniversidadEntities db =new UniversidadEntities())
            {
                DTOPagoConcepto objPagoC = (from a in db.PagoConcepto
                                            where a.OfertaEducativaId == OfertaEducativaId && a.PagoConceptoId == PagoConceptoId
                                            select new DTOPagoConcepto
                                            {
                                                CuentaContable = a.CuentaContable,
                                                Descripcion = a.Descripcion,
                                                EsMultireferencia = a.EsMultireferencia,
                                                EstatusId = a.EstatusId,
                                                OfertaEducativaId = OfertaEducativaId,
                                                PagoConceptoId = a.PagoConceptoId
                                            }).FirstOrDefault();
                return objPagoC;
            }
        }

        public static List<DTOCuota> ListaPagoConceptos2(int AlumnoId, int OfertaEducativaId, int  UsuarioId) 
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<DTOCuota> lstConceptos = null;
                    AlumnoInscrito objAlumno = db.AlumnoInscrito.Where(P => P.AlumnoId == AlumnoId && P.OfertaEducativaId == OfertaEducativaId).AsNoTracking().FirstOrDefault();
                    //List<Pago> lstPagos = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.EstatusId == 1).ToList();
                    //Pago objPagoRe = lstPagos.Where(LP => LP.Anio == 2016 && LP.PeriodoId == 1).FirstOrDefault();
                    //int cuenta = db.Pago.Where(P => P.Anio == 2016 && P.PeriodoId == 1).Count();
                    DTOPeriodo objPeriodo = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);
                    var Conceptosid =
                    db.UsuarioTipoPagoConcepto.Where(K =>
                                            K.UsuarioTipoId == db.Usuario.
                                                                    Where(O => O.UsuarioId == UsuarioId).
                                                                    FirstOrDefault().UsuarioTipoId
                                            && K.OfertaEducativaId == OfertaEducativaId
                                            ).Select(k => k.PagoConceptoId).ToArray();

                    lstConceptos = (from a in db.PagoConcepto
                                    join c in db.Cuota on new { a.PagoConceptoId, a.OfertaEducativaId } equals new { c.PagoConceptoId, c.OfertaEducativaId }
                                    where Conceptosid.Contains(a.PagoConceptoId)
                                        && c.PeriodoId == objPeriodo.PeriodoId && c.Anio == objPeriodo.Anio && c.OfertaEducativaId == OfertaEducativaId
                                        && a.EstatusId==1
                                        orderby a.Descripcion ascending
                                        select new DTOCuota
                                        {
                                            DTOPagoConcepto = new DTOPagoConcepto
                                            {
                                                PagoConceptoId = a.PagoConceptoId,
                                                Descripcion = a.Descripcion,
                                                EsVairable = a.EsVariable
                                            },
                                            Anio = c.Anio,
                                            CuotaId = c.CuotaId,
                                            EsEmpresa = c.EsEmpresa,
                                            Monto = c.Monto,
                                            OfertaEducativaId = c.OfertaEducativaId,
                                            PagoConceptoId = c.PagoConceptoId,
                                            PeriodoId = c.PeriodoId
                                        }).ToList();
                    if(objAlumno.OfertaEducativa.OfertaEducativaTipoId == 4)
                    {

                    }
                    return lstConceptos;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
