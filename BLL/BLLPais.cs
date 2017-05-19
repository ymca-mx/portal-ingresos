using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLPais
    {
        public static List<DTOPais> TraerPaises()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTOPais> paises = (from a in db.Pais
                                                       where a.PaisId != 146
                                                       orderby a.Descripcion ascending
                                                       select new DTOPais
                                                       {
                                                           PaisId = a.PaisId,
                                                           Descripcion = a.Descripcion
                                                       }).ToList();
                return paises;
            }
        }

        public static List<DTOPais> TraerPaisesT()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTOPais> paises = (from a in db.Pais
                                           orderby a.Descripcion ascending
                                           select new DTOPais
                                           {
                                               PaisId = a.PaisId,
                                               Descripcion = a.Descripcion
                                           }).ToList();
                return paises;
            }
        }
    }
}
