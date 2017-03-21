using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLMunicipio
    {
        public static List<DTOMunicipio> ConsultarMunicipios(int EntidadFederativaId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                return (from a in db.Municipio
                        where a.EntidadFederativaId == EntidadFederativaId
                        orderby a.Descripcion ascending
                        select new DTOMunicipio { 
                        Descripcion=a.Descripcion,
                        MunicipioId=a.EntidadFederativaId,
                        EntidadFederativaId = a.MunicipioId
                        }).ToList();
            }
        }
    }
}
