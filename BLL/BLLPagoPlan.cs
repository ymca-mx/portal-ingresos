using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLPagoPlan
    {
        public static object ConsultarPagos(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return
                    db.PagoPlan
                        .Where(p => db.OfertaEducativaPlan
                                        .Where(a => a.OfertaEducativaTipoId == db.AlumnoInscrito
                                                                            .Where(c => c.AlumnoId == AlumnoId)
                                                                            .FirstOrDefault().OfertaEducativa.OfertaEducativaTipoId)
                                        .Select(b => b.PagoPlanId)
                                        .ToList()
                                        .Contains(p.PagoPlanId)
                                        && p.EstatusId == 1)
                        .Select(a => new
                        {
                            PlanPago = a.PlanPago + " - " + (a.Pagos > 1 ? a.Pagos + " Pagos" : a.Pagos + " Pago"),
                            a.Descripcion,
                            a.Pagos,
                            a.PagoPlanId
                        })
                        .ToList();
                }
                catch (Exception error)
                {
                    return new
                    {
                        error.Message,
                        Ineer = error?.InnerException?.Message??""
                    };
                }
            }
        }

        public static object ConsultarPagosPlanLenguas(int TipoOfertaId)
        {
            using (UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    return (from a in db.PagoPlan
                            join b in db.OfertaEducativaPlan on a.PagoPlanId equals b.PagoPlanId
                            join c in db.OfertaEducativaTipo on b.OfertaEducativaTipoId equals c.OfertaEducativaTipoId
                            where c.OfertaEducativaTipoId == TipoOfertaId && a.EstatusId == 1
                            select new
                            {
                                PlanPago = a.PlanPago + " - " + (a.Pagos > 1 ? a.Pagos + " Pagos" : a.Pagos + " Pago"),
                                a.Descripcion,
                                a.Pagos,
                                a.PagoPlanId
                            }).ToList();

                }catch(Exception Error)
                {
                    return new
                    {
                        Error.Message,
                        Inner = Error?.InnerException?.Message ?? ""
                    };
                }
            }
        }


        public static List<DTOPagoPlan> ConsultarPagos(int AlumnoId, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTOPagoPlan> lstPagosPlan = (from a in db.PagoPlan
                                                  join b in db.OfertaEducativaPlan on a.PagoPlanId equals b.PagoPlanId
                                                  join c in db.OfertaEducativaTipo on b.OfertaEducativaTipoId equals c.OfertaEducativaTipoId
                                                  join d in db.OfertaEducativa on c.OfertaEducativaTipoId equals d.OfertaEducativaTipoId
                                                  join e in db.AlumnoInscrito on d.OfertaEducativaId equals e.OfertaEducativaId
                                                  where e.AlumnoId == AlumnoId && e.OfertaEducativaId == OfertaEducativaId && a.EstatusId == 1
                                                  select new DTOPagoPlan
                                                  {
                                                      PagoPlanId = a.PagoPlanId,
                                                      PlanPago = a.PlanPago,
                                                      Descripcion = a.Descripcion,
                                                      Pagos = a.Pagos
                                                  }).ToList();
                foreach (DTOPagoPlan objL in lstPagosPlan)
                {
                    string descP = objL.Pagos > 1 ? objL.Pagos + " Pagos" : objL.Pagos + " Pago";
                    objL.PlanPago += " - " + descP;
                }
                return lstPagosPlan;
            }
        }
    }
}
