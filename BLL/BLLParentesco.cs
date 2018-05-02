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
        public static object ConsultarTodosParentesco()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.Parentesco
                                                         select new 
                                                         {
                                                             a.ParentescoId,
                                                             a.Descripcion
                                                         }).ToList();
            }
        }
    }
}
