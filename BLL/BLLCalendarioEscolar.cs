using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLCalendarioEscolar
    {
        public static List<DTOCalendarioEscolar> TraerCalendarios()
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                return null;
            }
        }
    }
}
