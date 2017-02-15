using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class BLLAreaAcademica
    {
        public static List<DTOAreaAcademica> AreasAcademicas()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.AreaAcademica
                        select new DTOAreaAcademica
                        {
                            AreaAcademicaId=a.AreaAcademicaId,
                            Descripcion=a.Descripcion
                        }).ToList();
            }
        }
    }
}
