using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLEntidadFederativa
    {
        public static List<DTOEntidadFederativa> ConsultarEstadosCiviles()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTOEntidadFederativa> lstEntidadFederativa = (from a in db.EntidadFederativa
                                                                   orderby a.Descripcion ascending
                                                                 select new DTOEntidadFederativa
                                                           {
                                                               EntidadFederativaId = a.EntidadFederativaId,
                                                               Descripcion = a.Descripcion
                                                           }).ToList();
                return lstEntidadFederativa;
            }
        }
    }
}
