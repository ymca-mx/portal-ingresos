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
    public class BLLDocente
    {
        public static string NuevoDocente(DTODocente objDocente)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    db.Docente.Add(
                        new Docente
                        {
                            FechaAlta = DateTime.Now,
                            Materno = objDocente.Materno,
                            Nombre = objDocente.Materno,
                            Paterno = objDocente.Paterno,
                            UsuarioId = objDocente.UsuarioId
                        });
                    db.SaveChanges();
                    db.DocenteDetalle.Add(
                        new DocenteDetalle
                            {
                                DocenteId = db.Docente.Local[0].DocenteId,
                                Email = objDocente.DocenteDetalle.Email,
                                EstadoCivilId = objDocente.DocenteDetalle.EstadoCivilId,
                                FechaNacimiento = objDocente.DocenteDetalle.FechaNacimiento,
                                GeneroId = objDocente.DocenteDetalle.GeneroId,
                                RFC = objDocente.DocenteDetalle.RFC,
                                TelefonoCasa = objDocente.DocenteDetalle.TelefonoCasa,
                                TelefonoCelular = objDocente.DocenteDetalle.TelefonoCelular
                            });
                    db.SaveChanges();
                    return db.Docente.Local[0].DocenteId.ToString();
                }
                catch
                {
                    return "-1";
                }
            }
        }

        public static List<DTODocente> ConsultarDocentes()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return (from a in db.Docente
                            select new DTODocente
                            {
                                DocenteId = a.DocenteId,
                                FechaAlta = a.FechaAlta,
                                Materno = a.Materno,
                                Paterno = a.Paterno,
                                Nombre = a.Nombre,
                                UsuarioId = a.UsuarioId,
                                DocenteDetalle = new DTODocenteDetalle
                                {
                                    DocenteId = a.DocenteDetalle.DocenteId,
                                    Email = a.DocenteDetalle.Email,
                                    EstadoCivilId = a.DocenteDetalle.EstadoCivilId,
                                    FechaNacimiento = a.DocenteDetalle.FechaNacimiento,
                                    GeneroId = a.DocenteDetalle.GeneroId,
                                    RFC = a.DocenteDetalle.RFC,
                                    TelefonoCasa = a.DocenteDetalle.TelefonoCasa,
                                    TelefonoCelular = a.DocenteDetalle.TelefonoCelular
                                }
                            }).AsNoTracking().ToList();
                }catch
                {
                    return null;
                }
            }
        }
        public static List<DTODocente> ListarDocentesNormal()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return (from a in db.Docente
                            select new DTODocente
                            {
                                DocenteId = a.DocenteId,
                                Nombre = a.Nombre + " " + a.Paterno + " " + a.Materno
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
