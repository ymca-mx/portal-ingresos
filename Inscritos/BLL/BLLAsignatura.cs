using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data.Entity;

namespace BLL
{
    public class BLLAsignatura
    {
        public static List<DTOAsignatura> ListarAsignatura(int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return (from a in db.Asignatura
                            where a.OfertaEducativaId == OfertaEducativaId
                            select new DTOAsignatura
                            {
                                AsignaturaId = a.AsignaturaId,
                                Descripcion = a.Descripcion
                            }).AsNoTracking().ToList();
                            
                }
                catch
                {
                    return null;
                }
            }

        }
    }
}
