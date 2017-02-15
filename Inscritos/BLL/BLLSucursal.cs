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
        public static List<DTOSucursal> ConsultarSucursales()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTOSucursal> lstSucursales = (from a in db.Sucursal
                                                   where a.EsSucursal==true
                                                       select new DTOSucursal { 
                                                           SucursalId=a.SucursalId,
                                                           DescripcionId=a.DescripcionId
                                                       }).ToList();
                return lstSucursales;
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
    }
}
