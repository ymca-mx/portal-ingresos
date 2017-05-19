using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLTurno
    {
        public static List<DTOTurno> ConsultarTurno()
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                return (from a in db.Turno
                                               select new DTOTurno { 
                                               TurnoId=a.TurnoId,
                                               Descripcion=a.Descripcion
                                               }).ToList();
            }
        }
    }
}
