using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universidad.DAL;

namespace Universidad.BLL
{
    public class BLLCajaConfiguracion
    {
        public static DTO.DTOConfiguracion Consulta(DTO.DTOLogin Credencial)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.CajaConfiguracion
                        where a.SucursalCajaId == Credencial.sucursalCajaId
                        select new DTO.DTOConfiguracion
                        {
                            configuracionId = a.ConfiguracionId,
                            impresoraRecibo = a.ImpresoraRecibo != null ? a.ImpresoraRecibo : "",
                            impresoraReporteria = a.ImpresoraReporteria != null ? a.ImpresoraReporteria : ""
                        }).FirstOrDefault();
            }
        }

        public static void Actualiza(DTO.DTOConfiguracion Configuracion)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                CajaConfiguracion Settings = db.CajaConfiguracion
                    .Where(configuracion => configuracion.ConfiguracionId == Configuracion.configuracionId).FirstOrDefault();

                Settings.ImpresoraRecibo = Configuracion.impresoraRecibo;
                Settings.ImpresoraReporteria = Configuracion.impresoraReporteria;

                db.SaveChanges();
            }
        }
    }
}
