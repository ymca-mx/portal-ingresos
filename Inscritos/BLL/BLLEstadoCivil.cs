using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLEstadoCivil
    {
        public static List< DTOEstadoCivil> ConsultarEstadosCiviles()
        {
            using(UniversidadEntities db=new UniversidadEntities())
            {
                List<DTOEstadoCivil> lstEstadoCivil = (from a in db.EstadoCivil
                                                           select new DTOEstadoCivil
                                                           {
                                                               EstadoCivilId = a.EstadoCivilId,
                                                               Descripcion = a.Descripcion
                                                           }).ToList();
                return lstEstadoCivil;
            }
        }
    }
}
