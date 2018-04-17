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
        public static object ConsultarTurno()
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                return (from a in db.Turno
                        select new
                        {
                            a.TurnoId,
                            a.Descripcion
                        }).ToList();
            }
        }
    }
}
