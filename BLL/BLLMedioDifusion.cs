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
        public static object ConsultarListadeMedios()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.MedioDifusion
                        select new 
                        {
                            a.Descripcion,
                            a.MedioDifusionId
                        }).ToList();
            }
        }
    }
}
