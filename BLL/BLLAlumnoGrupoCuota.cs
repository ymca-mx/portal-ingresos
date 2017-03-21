using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DAL;
using DTO;

namespace BLL
{
    public class BLLAlumnoGrupoCuota
    {

        public static DTOAlumnoGrupoCuota ObtenerAlumno(int AlumnoId, int OfertaEducativaId, int Anio, int PeriodoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return (from a in db.AlumnoGrupoCuota
                            where a.AlumnoId == AlumnoId && a.Anio == Anio &&
                            a.PeriodoId == PeriodoId && a.OfertaEducativaId == OfertaEducativaId
                            select new DTOAlumnoGrupoCuota
                            {
                                AlumnoId = (int)a.AlumnoId,
                                Anio = (int)a.Anio,
                                GrupoId = a.GrupoId,
                                OFertaEducativaId = (int)a.OfertaEducativaId,
                                PeriodoId = (int)a.PeriodoId,
                                MontoInscripcion = a.MontoInscripcion,
                                MontoColegiatura = a.MontoColegiatura
                            }).FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            }
        }

    }
}
