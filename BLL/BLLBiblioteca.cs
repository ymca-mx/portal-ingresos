using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLBiblioteca
    {
        public static void SendComunicado(int Anio, int PeriodoId, string Asunto, string NombreDocumento)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                if (db.Comunicado.Where(com => com.Anio == Anio && com.PeriodoId == PeriodoId).ToList().Count == 0)
                {
                    var periodo = db.Periodo
                                    .Where(p => DateTime.Now >= p.FechaInicial
                                            && DateTime.Now <= p.FechaFinal)
                                    .FirstOrDefault();

                    var listAlumnos = db.Alumno
                                                .Where(x => x.AlumnoInscrito
                                                                .Where(al => al.OfertaEducativaId != 43
                                                                        && al.Anio == periodo.Anio
                                                                        && al.PeriodoId == periodo.PeriodoId)
                                                                .ToList()
                                                                .Count > 0
                                                             && x.EstatusId == 1
                                                             && x.AlumnoDetalle.Email.Length > 5)
                                                 .Select(x => new
                                                 {
                                                     x.AlumnoId,
                                                     x.Nombre,
                                                     x.Materno,
                                                     x.Paterno,
                                                     x.AlumnoDetalle.Email
                                                 }).ToList();

                    var listDocentes = db.Docente
                                            .Join(db.DocenteDetalle,
                                                    Docente => Docente.DocenteId,
                                                    DocDeta => DocDeta.DocenteId,
                                                    (Docente, DocDeta) => new { Docente, DocenteDetalle = DocDeta })
                                            .Select(d => new
                                            {
                                                d.Docente.DocenteId,
                                                d.Docente.Nombre,
                                                d.Docente.Paterno,
                                                d.Docente.Materno,
                                                d.DocenteDetalle.Email
                                            }).ToList();

                    var listUsuarios = db.Usuario
                                        .Where(us => us.UsuarioId == 100000)
                                        .Select(us => new
                                        {
                                            us.UsuarioId,
                                            us.Nombre,
                                            us.Paterno,
                                            us.Materno,
                                            us.UsuarioDetalle.Email
                                        }).ToList();

                    listAlumnos.ForEach(alumno =>
                    {

                    });

                    listDocentes.ForEach(doce =>
                    {

                    });

                    listUsuarios.ForEach(usu =>
                    {

                    });
                }
            }
        }
    }
}
