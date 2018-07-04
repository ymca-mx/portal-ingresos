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
        public static object UpdateEmail(int AlumnoId, int UsuarioId, string Mail)
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

                    return new
                    {
                        Estatus = true,
                        AlumnoId
                    };
                }
                catch(Exception error)
                {
                    return new
                    {
                        Estatus = false,
                        error.Message,
                        Inner = error?.InnerException?.Message ?? ""
                    };
                }
            }
        }
        public static object GetAlumnoDetalle(int AlumnoId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    return (from a in db.AlumnoDetalle
                            where a.AlumnoId == AlumnoId
                            select new 
                            {
                                Estatus=true,
                                a.AlumnoId,
                                a.Email,
                                Nombre = a.Alumno.Nombre + " " + a.Alumno.Paterno + " " + a.Alumno.Materno
                            }).FirstOrDefault();
                }
                catch(Exception error)
                {
                    return new
                    {
                        Estatus = true,
                        error.Message,
                        Inner = error?.InnerException?.Message ?? ""
                    };
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
