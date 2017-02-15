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
                    AlumnoDetalle objAlDetalle= db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault();
                    objAlDetalle.Email = Mail;
                    db.AlumnoDetalleBitacora.Add(new AlumnoDetalleBitacora
                    {
                        AlumnoId = objAlDetalle.AlumnoId,
                        Calle = objAlDetalle.Calle,
                        Celular = objAlDetalle.Celular,
                        Colonia = objAlDetalle.Colonia,
                        CP = objAlDetalle.CP,
                        CURP = objAlDetalle.CURP,
                        Email = objAlDetalle.Email,
                        EntidadFederativaId = objAlDetalle.EntidadFederativaId,
                        EntidadNacimientoId = objAlDetalle.EntidadNacimientoId,
                        EstadoCivilId = objAlDetalle.EstadoCivilId,
                        Fecha = DateTime.Now,
                        FechaNacimiento = objAlDetalle.FechaNacimiento,
                        GeneroId = objAlDetalle.GeneroId,
                        MunicipioId = objAlDetalle.MunicipioId,
                        NoExterior = objAlDetalle.NoExterior,
                        NoInterior = objAlDetalle.NoInterior,
                        PaisId = objAlDetalle.PaisId,
                        
                        TelefonoCasa = objAlDetalle.TelefonoCasa,
                        TelefonoOficina = objAlDetalle.TelefonoOficina,
                        UsuarioId = UsuarioId,
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
