using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLParentesco
    {
        public static List<DTOParentesco> ConsultarTodosParentesco()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.Parentesco
                                                         select new DTOParentesco
                                                         {
                                                             ParentescoId=a.ParentescoId,
                                                             Descripcion=a.Descripcion
                                                         }).ToList();
            }
        }
    }
}
