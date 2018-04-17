using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLSucursal
    {
        public static object ConsultarSucursales()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.Sucursal
                        where a.EsSucursal == true
                        select new
                        {
                            a.SucursalId,
                            Descripcion= a.DescripcionId
                        }).ToList();
            }
        }
        public static DTOSucursal TraerSucursal(int SucursalId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.Sucursal
                        where a.SucursalId == SucursalId
                        select new DTOSucursal
                        {
                            SucursalId = a.SucursalId,
                            Serie = a.Serie,
                            DescripcionId = a.DescripcionId,
                            Detalle = new DTOSucursalDetalle
                            {
                                Calle = a.SucursalDetalle.Calle,
                                Colonia = a.SucursalDetalle.Colonia,
                                Cp = a.SucursalDetalle.Cp,
                                Delegacion = a.SucursalDetalle.Delegacion,
                                NoExterior = a.SucursalDetalle.NoExterior
                            }
                        }).FirstOrDefault();
            }
        }

        public static List<DTOSucursal> ConsultarSucursalesEmpresa()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTOSucursal> lstSucursales = (from a in db.Sucursal
                                                   select new DTOSucursal
                                                   {
                                                       SucursalId = a.SucursalId,
                                                       DescripcionId = a.DescripcionId
                                                   }).ToList();
                return lstSucursales;
            }
        }

        public static List<DTOSucursalTree> TraerTodas()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTOSucursalTree> Sucursales = db.OfertaEducativa
                        .GroupBy(of => of.Sucursal)
                        .Select(of => of.FirstOrDefault().Sucursal)
                        .Select(of =>
                            new DTOSucursalTree
                            {
                                Descripcion = of.DescripcionId,
                                SucursalId = of.SucursalId,
                            }).ToList();

                Sucursales.ForEach(a =>
                {
                    a.OFertaEducativaTipo = db.OfertaEducativa
                                            .Where(of => of.SucursalId == a.SucursalId)
                                            .GroupBy(ot => ot.OfertaEducativaTipo)
                                                        .Select(ot => ot.FirstOrDefault().OfertaEducativaTipo)
                                                        .Where(ot => ot.OfertaEducativa.Where(of => of.SucursalId == a.SucursalId).ToList().Count > 0)
                                                        .Select(ot =>
                                                            new DTOOfertaEducativaTipo
                                                            {
                                                                Descripcion = ot.Descripcion,
                                                                OfertaEducativaTipoId = ot.OfertaEducativaTipoId,
                                                                Ofertas = ot.OfertaEducativa
                                                                            .Where(of1 => of1.SucursalId == a.SucursalId)
                                                                            .Select(of1 =>
                                                                                new DTOOfertaEducativa2
                                                                                {
                                                                                    descripcion = of1.Descripcion,
                                                                                    ofertaEducativaId = of1.OfertaEducativaId
                                                                                }).ToList()
                                                            })
                                                            .ToList();
                });

                return Sucursales;
            }
        }
    }
}
