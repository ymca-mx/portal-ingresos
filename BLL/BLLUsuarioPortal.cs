using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLUsuarioPortal
    {
        public static DTOUsuario ObtenerUsuario(int UsuarioId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
               return (from a in db.Usuario
                 where a.UsuarioId == UsuarioId
                 select new DTOUsuario
                 {
                     EstatusId = a.EstatusId,
                     Materno = a.Materno,
                     Nombre = a.Nombre,
                     UsuarioId = a.UsuarioId,
                     Paterno = a.Paterno,
                     UsuarioTipoId = a.UsuarioTipoId,
                     Password = a.Password
                 }).FirstOrDefault();
            }
        }

        public static object TraerTodos(int UsuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                
                try
                {
                    return
                          new
                          {
                              Status = true,
                              Usuarios =
                                  db.Usuario.Where(a => a.UsuarioId != UsuarioId
                                                    && a.UsuarioId!=0)
                                  .Select(a => new
                                  {
                                      a.UsuarioId,
                                      a.Nombre,
                                      a.Paterno,
                                      a.Materno,
                                      Estatus = a.Estatus.Descripcion,
                                      a.EstatusId,
                                      a.UsuarioTipo.Descripcion,
                                      a.Password
                                  })
                                  .AsEnumerable()
                                  .Select(a => new
                                  {
                                      a.UsuarioId,
                                      a.Nombre,
                                      a.Paterno,
                                      a.Materno,
                                      a.Estatus,
                                      a.EstatusId,
                                      a.Descripcion,
                                      Password = Utilities.Seguridad.Desencripta(27, a.Password)
                                  })
                                  .ToList()
                          };
                }
                catch (Exception Error)
                {
                    return new
                    {
                        Status = false,
                        Error.Message,
                        Inner = Error?.InnerException?.Message
                    };
                }
            }
        }

        public static object GetUsuario(int usuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return db
                            .Usuario
                            .Where(a => a.UsuarioId == usuarioId)
                            .AsEnumerable()
                            .Select(a => new
                            {
                                Status = true,
                                a.Nombre,
                                a.Paterno,
                                a.Materno,
                                FechaNacimiento = a.UsuarioDetalle.FechaNacimiento.ToString("dd/MM/yyyy"),
                                a.UsuarioDetalle.GeneroId,
                                a.UsuarioDetalle.Email,
                                a.UsuarioDetalle.Telefono,
                                UsuarioTipo = a.UsuarioTipo.Descripcion
                            })
                            .FirstOrDefault();

                }
                catch (Exception err)
                {
                    return new
                    {
                        Status = false,
                        err.Message,
                        Inner = err?.InnerException?.Message
                    };
                }
            }
        }

        public static object UpdateUsuario(DTOUsuario objUsuario)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    Usuario usuariodb = db.Usuario
                        .Where(a => a.UsuarioId == objUsuario.UsuarioId)
                        .FirstOrDefault();

                    if (usuariodb != null)
                    {
                        usuariodb.Nombre = objUsuario.Nombre;
                        usuariodb.Paterno = objUsuario.Paterno;
                        usuariodb.Materno = objUsuario.Materno;
                        usuariodb.EstatusId = objUsuario.EstatusId;

                        usuariodb.Password = Utilities.Seguridad.Encripta(27, objUsuario.Password);

                        db.SaveChanges();

                        return new
                        {
                            Status = true,
                            Message = "No existe el usuario en la base",
                            Inner = ""
                        };
                    }
                    else
                    {
                        return new
                        {
                            Status = false,
                            Message = "No existe el usuario en la base",
                            Inner = ""
                        };
                    }
                }
                catch (Exception Error)
                {
                    return new
                    {
                        Status = false,
                        Error.Message,
                        Inner = Error?.InnerException?.Message ?? ""
                    };
                }
            }
        }

        public static DTOUsuarioDetalle ObtenerDetalle(int UsuarioId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    return (from a in db.UsuarioDetalle
                            where a.UsuarioId == UsuarioId
                            select new DTOUsuarioDetalle
                            {
                                UsuarioId = a.UsuarioId,
                                Email = a.Email
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
