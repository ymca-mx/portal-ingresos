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
        public static List<DTOEntidadFederativa> ConsultarEntidadFederativa()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.EntidadFederativa
                                                                   orderby a.Descripcion ascending
                                                                 select new DTOEntidadFederativa
                                                           {
                                                               EntidadFederativaId = a.EntidadFederativaId,
                                                               Descripcion = a.Descripcion
                                                           }).ToList();
            }
        }
    }
}
