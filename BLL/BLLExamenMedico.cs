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
    public class BLLExamenMedico
    {
        public static DTOExamenMedico TraerAlumno(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return
                    db.AlumnoDetalle
                            .Where(o => o.AlumnoId == AlumnoId)
                            .Select(k => new DTOExamenMedico
                            {
                                AlumnoId = k.AlumnoId,
                                Nombre = k.Alumno.Nombre + " " + k.Alumno.Paterno + " " + k.Alumno.Materno,
                                ExamenMedico = k.TieneExamenMedico
                            }).FirstOrDefault();
                }
                catch { return null; }
            }
        }
        public static bool GuardarExamenMedico(int AlumnoId, bool Examen, int UsuarioId, string Comentario)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    AlumnoDetalle alumno = db.AlumnoDetalle
                        .Where(AL => AL.AlumnoId == AlumnoId)
                        .FirstOrDefault();

                    #region No Tiene Examen Medico 
                    if (db.AlumnoExamenMedico.Where(k => k.AlumnoId == AlumnoId).ToList().Count == 0)
                    {
                        db.AlumnoExamenMedico.Add(new AlumnoExamenMedico
                        {
                            AlumnoId = alumno.AlumnoId,
                            FechaRegistro = DateTime.Now,
                            HoraRegistro = DateTime.Now.TimeOfDay,
                            Observaciones = Comentario,
                            UsuarioId = UsuarioId
                        });
                    }
                    #endregion

                    #region Tiene Examen (Actualizacion)
                    alumno.TieneExamenMedico = Examen;

                    db.AlumnoDetalleBitacora
                        .Add(new AlumnoDetalleBitacora
                        {
                            AlumnoId = alumno.AlumnoId,
                            Calle = alumno.Calle,
                            Celular = alumno.Celular,
                            Colonia = alumno.Colonia,
                            CP = alumno.CP,
                            CURP = alumno.CURP,
                            Email = alumno.Email,
                            EntidadFederativaId = alumno.EntidadFederativaId,
                            EntidadNacimientoId = alumno.EntidadNacimientoId,
                            EstadoCivilId = alumno.EstadoCivilId,
                            Fecha = DateTime.Now,
                            FechaNacimiento = alumno.FechaNacimiento,
                            GeneroId = alumno.GeneroId,
                            MunicipioId = alumno.MunicipioId,
                            NoExterior = alumno.NoExterior,
                            NoInterior = alumno.NoInterior,
                            PaisId = alumno.PaisId,
                            ProspectoId = alumno.PaisId,
                            TelefonoCasa = alumno.TelefonoCasa,
                            TelefonoOficina = alumno.TelefonoOficina,
                            UsuarioId = UsuarioId
                        });
                    #endregion

                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
