using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universidad.DAL;

namespace Universidad.BLL
{
    public class BLLDescuento
    {
        public static bool AplicarAutorizado(DTO.DTOAlumnoDatosGenerales DatosAlumno, DTO.DTOPeriodo Periodo, DTO.DTOPagoConcepto Concepto, DTO.DTOLogin Credenciales, DTO.DTOPagoDescuento Descuento)
        {
            try
            {
                using (DAL.UniversidadEntities db = new DAL.UniversidadEntities())
                {
                    db.AlumnoDescuento.Add(new AlumnoDescuento
                    {
                        AlumnoId = DatosAlumno.alumnoId,
                        OfertaEducativaId = DatosAlumno.ofertaEducativaId,
                        Anio = Periodo.anio,
                        PeriodoId = Periodo.periodoId,
                        DescuentoId = (db.Descuento.AsNoTracking().Where(d => d.PagoConceptoId == Concepto.conceptoPagoId && d.OfertaEducativaId == DatosAlumno.ofertaEducativaId).FirstOrDefault().DescuentoId),
                        PagoConceptoId = Concepto.conceptoPagoId,
                        Monto = Descuento.importe,
                        UsuarioId = Credenciales.usuarioId,
                        Comentario = Descuento.observacion,
                        FechaGeneracion = DateTime.Now,
                        EstatusId = 1
                    });

                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception Ex) { return false; }
        }
    }
}
