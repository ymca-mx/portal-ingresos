using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Globalization;
using DTO;
using DAL;

namespace BLL
{
    public class BllAlumnoDetalle
    {
        public static bool UpdateEmail(int AlumnoId, int UsuarioId, string Mail)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    AlumnoDetalle AlumnoDetalleAnterior= db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault();
                    AlumnoDetalleAnterior.Email = Mail;
                    db.AlumnoDetalleBitacora.Add(new AlumnoDetalleBitacora
                    {
                        AlumnoId = AlumnoDetalleAnterior.AlumnoId,
                        Calle = AlumnoDetalleAnterior.Calle,
                        Celular = AlumnoDetalleAnterior.Celular,
                        Colonia = AlumnoDetalleAnterior.Colonia,
                        CP = AlumnoDetalleAnterior.CP,
                        CURP = AlumnoDetalleAnterior.CURP,
                        Email = AlumnoDetalleAnterior.Email,
                        EntidadFederativaId = AlumnoDetalleAnterior.EntidadFederativaId,
                        EntidadNacimientoId = AlumnoDetalleAnterior.EntidadNacimientoId,
                        EstadoCivilId = AlumnoDetalleAnterior.EstadoCivilId,
                        Fecha = DateTime.Now,
                        FechaNacimiento = AlumnoDetalleAnterior.FechaNacimiento,
                        GeneroId = AlumnoDetalleAnterior.GeneroId,
                        MunicipioId = AlumnoDetalleAnterior.MunicipioId,
                        NoExterior = AlumnoDetalleAnterior.NoExterior,
                        NoInterior = AlumnoDetalleAnterior.NoInterior,
                        PaisId = AlumnoDetalleAnterior.PaisId,
                        
                        TelefonoCasa = AlumnoDetalleAnterior.TelefonoCasa,
                        TelefonoOficina = AlumnoDetalleAnterior.TelefonoOficina,
                        UsuarioId = UsuarioId,
                        Observaciones=AlumnoDetalleAnterior.Observaciones,
                    });

                    db.SaveChanges();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public static DTOAlumnoDetallev2 GetAlumnoDetalle(int AlumnoId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    return (from a in db.AlumnoDetalle
                            where a.AlumnoId == AlumnoId
                            select new DTOAlumnoDetallev2
                            {
                                AlumnoId = a.AlumnoId,
                                Email = a.Email,
                                Nombre = a.Alumno.Nombre + " " + a.Alumno.Paterno + " " + a.Alumno.Materno
                            }).FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            }
        }
    }
    public class DTOAlumnoDetallev2
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
    }
}
