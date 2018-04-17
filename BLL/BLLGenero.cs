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
        public static object ConsultaTodosGenero()
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                return (from a in db.Genero
                        select new
                        {
                            a.GeneroId,
                            a.Descripcion
                        }).ToList();
            }
        }
    }
}
