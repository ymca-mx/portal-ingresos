using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLGenero
    {
        public static List<DTOGenero> ConsultaTodosGenero()
        {
            //List<DTO.DTOGenero> lstGenero=new List<DTO.DTOGenero>;
            using(UniversidadEntities db= new UniversidadEntities())
            {
                List<DTOGenero> lstGenero = (from a in db.Genero
                                                 select new DTOGenero
                                                 {
                                                     GeneroId = a.GeneroId,
                                                     Descripcion=a.Descripcion
                                                 }).ToList();
                return lstGenero;
            }
        }
    }
}
