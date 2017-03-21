using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using Herramientas;
using BLL;
using System.Data.Entity;

namespace BLL
{
    public class BLLDia
    {
        public static List<DTODia> ConsultarDias()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return (from a in db.Dia
                            select new DTODia
                            {
                                DiaId = a.DiaId,
                                Descripcion = a.Descripcion
                            }).ToList();
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
