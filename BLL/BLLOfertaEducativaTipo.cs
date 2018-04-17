using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLOfertaEducativaTipo
    {
        public static object ConsultaOfertaTipo(int plantel)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var sucursal = db.Sucursal.Where(a => a.SucursalId == plantel).FirstOrDefault();
                if (sucursal != null)
                {
                    if (sucursal.EsSucursal)
                    {
                        return
                        db.OfertaEducativaTipo
                            .Where(oft => oft.OfertaEducativa.Where(of => of.SucursalId == plantel).ToList().Count > 0)
                            .Select(oft => new
                            {
                                oft.OfertaEducativaTipoId,
                                oft.Descripcion
                            }).ToList();
                    }
                    else
                    {
                        return (db.OfertaEducativaTipo
                                    .Select(a => new
                                    {
                                        a.OfertaEducativaTipoId,
                                        a.Descripcion,
                                    })
                                .ToList());
                    }
                }
                else { return null; }
            }
        }

        public static DTOOfertaEducativaTipo ConsultarOferta(int OfertaEducativaId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                return (from a in db.OfertaEducativaTipo
                        join c in db.OfertaEducativa on a.OfertaEducativaTipoId equals c.OfertaEducativaTipoId
                        where c.OfertaEducativaId == OfertaEducativaId
                        select new DTOOfertaEducativaTipo
                        {
                            Descripcion=a.Descripcion,
                            OfertaEducativaTipoId=a.OfertaEducativaTipoId
                        }).FirstOrDefault();
            }
        }

        public static List<DTOOfertaEducativaTipo> ConsultaOfertaTipo()
        {
            using(UniversidadEntities db=new UniversidadEntities())
            {return
                db.OfertaEducativaTipo
                        .Select(k => new DTOOfertaEducativaTipo
                        {
                            Descripcion = k.Descripcion,
                            OfertaEducativaTipoId = k.OfertaEducativaTipoId,
                            Ofertas = k.OfertaEducativa.Where(x=> x.EstatusId== 1).Select(of => new DTOOfertaEducativa2
                            {
                                descripcion = of.Descripcion,
                                ofertaEducativaId = of.OfertaEducativaId,
                                sucursalid= of.SucursalId
                                
                            }).ToList()
                        }).ToList();
            }
        }
    }
}
