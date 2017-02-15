using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using Herramientas;
using BLL;

namespace BLL
{
    public class BLLMedioDifusion
    {
        public static List<DTOMedioDifusion> ConsultarListadeMedios()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.MedioDifusion
                        select new DTOMedioDifusion
                        {
                            Descripcion=a.Descripcion,
                            MedioDifusionId=a.MedioDifusionId
                        }).ToList();
            }
        }
    }
}
