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
        public static List<DTOOfertaEducativaTipo> ConsultaOfertaTipo(int plantel)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                bool Sucursal = (from s in db.Sucursal
                                where s.SucursalId == plantel
                                select s.EsSucursal).FirstOrDefault();
                if (Sucursal == true)
                {
                    return (from a in db.OfertaEducativaTipo
                            join b in db.OfertaEducativa on a.OfertaEducativaTipoId equals b.OfertaEducativaTipoId
                            where b.SucursalId == plantel
                            select new DTOOfertaEducativaTipo
                            {
                                OfertaEducativaTipoId = a.OfertaEducativaTipoId,
                                Descripcion = a.Descripcion,
                            }).Distinct().ToList();
                }
                else
                {
                    return (from a in db.OfertaEducativaTipo
                            select new DTOOfertaEducativaTipo
                            {
                                OfertaEducativaTipoId = a.OfertaEducativaTipoId,
                                Descripcion = a.Descripcion,
                            }).Distinct().ToList();
                }
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
