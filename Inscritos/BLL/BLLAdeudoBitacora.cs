using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using System.Data.Entity;

namespace BLL
{
    public class BLLAdeudoBitacora
    {
        public static void GuardarAdeudo(DTOAlumnoReferenciaBitacora objAdeudo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.AlumnoReferenciaBitacora.Add(new AlumnoReferenciaBitacora
                {
                    AlumnoId = objAdeudo.AlumnoId,
                    FechaGeneracion = DateTime.Now.Date,
                    HoraGeneracion = DateTime.Now.TimeOfDay,
                    Anio = objAdeudo.Anio,
                    PeriodoId = objAdeudo.PeriodoId,
                    OfertaEducativaId = objAdeudo.OfertaEducativaId,
                    PagoConceptoId = objAdeudo.PagoConceptoId,
                    PagoId = objAdeudo.PagoId
                });
                db.SaveChanges();
            }
        }
        public static List<DTOAlumnoReferenciaBitacora> TraerListaBitacora(int AlumnoId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                return (from a in db.AlumnoReferenciaBitacora
                        where a.AlumnoId == AlumnoId
                        select new DTOAlumnoReferenciaBitacora
                        {
                            AlumnoId=a.AlumnoId,
                            Anio=a.Anio,
                            BitacoraId=a.BitacoraId,
                            FechaGeneracion=a.FechaGeneracion,
                            HoraGeneracion=a.HoraGeneracion,
                            OfertaEducativaId=a.OfertaEducativaId,
                            PagoConceptoId=a.PagoConceptoId,
                            PagoId=a.PagoId,
                            PeriodoId=a.PeriodoId
                        }).ToList();
            }
        }
    }
}
