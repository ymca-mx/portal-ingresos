using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universidad.DAL;
using System.Drawing;
using System.IO;

namespace Universidad.BLL
{
    public class BLLUsuario
    {

        public static void BitacoraIngresos(DTO.DTOLogin Login, DateTime Entrada)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.UsuarioIngresosBitacora.Add(new UsuarioIngresosBitacora { 
                    UsuarioId = Login.usuarioId,
                    FechaIngreso = Entrada,
                    HoraIngreso = Entrada.TimeOfDay,
                    HoraSalida = DateTime.Now.TimeOfDay,
                    TipoIngreso = Login.TipoAcceso == DTO.Perfil.Acceso.Ingresos ? "Caja autorizada" : "Otra terminal MAC: " + Utilities.Aplicacion.DireccionFisica()
                });

                db.SaveChanges();
            }
        }

        public static List<DTO.Usuario.CorteDeCaja.DTOCajero> CargaCajeros()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                List<DTO.Usuario.CorteDeCaja.DTOCajero> Cajeros = new List<DTO.Usuario.CorteDeCaja.DTOCajero>();
                Cajeros.Add(new DTO.Usuario.CorteDeCaja.DTOCajero{
                    usuarioId = -1,
                    nombre = "Todos"
                });

                List<DTO.Usuario.CorteDeCaja.DTOCajero> Reales = (from x in
                                                                      (from a in db.Recibo.AsNoTracking()
                                                                       join b in db.Usuario.AsNoTracking() on a.UsuarioId equals b.UsuarioId
                                                                       where a.UsuarioId != 0
                                                                       select new { a, b })
                                                                  group x by new
                                                                  {
                                                                      x.a.UsuarioId,
                                                                      Nombre = x.b.Nombre + " " + x.b.Paterno + " " + x.b.Materno
                                                                  } into g

                                                                  orderby g.Key.UsuarioId ascending
                                                                  select new DTO.Usuario.CorteDeCaja.DTOCajero
                                                                  {
                                                                      usuarioId = g.Key.UsuarioId,
                                                                      nombre = g.Key.Nombre
                                                                  }).ToList();

                Cajeros.AddRange(Reales);
                Cajeros.Sort((a, b) => a.usuarioId.CompareTo(b.usuarioId));

                return Cajeros;

                //return (from a in db.Recibo.AsNoTracking()
                //        join b in db.Usuario.AsNoTracking() on a.UsuarioId equals b.UsuarioId
                        
                //        select new DTO.Usuario.CorteDeCaja.DTOCajero { 
                //            usuarioId = a.UsuarioId,
                //            nombre = (b.Nombre + " " + b.Paterno + " " + b.Materno).Trim()
                //        }).ToList();
            }
        }

        public static void BitacoraReingreso(DTO.DTOLogin Credenciales, int alumnoId, int anio, int periodoId, int ofertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.AlumnoReingresoBitacora.Add(new AlumnoReingresoBitacora { 
                    UsuarioId = Credenciales.usuarioId,
                    AlumnoId = alumnoId,
                    Anio = anio,
                    PeriodoId = periodoId,
                    OfertaEducativaId = ofertaEducativaId,
                    FechaGeneracion = DateTime.Now,
                    HoraGeneracion = DateTime.Now.TimeOfDay
                });

                db.SaveChanges();
            }
        }

        public static byte[] ImagenDefault()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                byte[] bytesImagen =
                    (from a in db.UsuarioImagen
                     where a.UsuarioId == 0
                     select a.Imagen)
                    .ToList().FirstOrDefault();

                return bytesImagen;
            }
        }

        public static void GuardaImagen(string directorio, int usuarioId, DTO.DTOLogin Credencial)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.UsuarioImagen.Add(new UsuarioImagen
                {
                    UsuarioId = usuarioId,
                    Imagen = Utilities.Archivo.Bytes(directorio)
                });

                db.UsuarioImagenDetalle.Add(new UsuarioImagenDetalle
                {
                    UsuarioId = usuarioId,
                    Extension = Path.GetExtension(directorio).ToLower(),
                    UsuarioIdCarga = Credencial.usuarioId
                });

                db.SaveChanges();
            }
        }

        public static Image ConsultaImagen(int usuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                byte[] bytesImagen =
                    (from a in db.UsuarioImagen
                     where a.UsuarioId == usuarioId
                     select a.Imagen)
                    .ToList().FirstOrDefault();

                return Utilities.Archivo.Imagen(bytesImagen == null ? ImagenDefault() : bytesImagen);
            }
        }

        public static DTO.Usuario.DTOUsuarioImagen ImagenIndex(int usuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                var Ano = (from a in db.Usuario
                           join c in db.UsuarioImagen on a.UsuarioId equals c.UsuarioId
                           join d in db.UsuarioImagenDetalle on a.UsuarioId equals d.UsuarioId
                           where a.UsuarioId == usuarioId
                           select new DTO.Usuario.DTOUsuarioImagen {
                               nombre = (a.Nombre + " " + a.Paterno + " " + a.Materno).Trim(),
                               imagen = c.Imagen,
                               extensionImagen = d.Extension
                           }).FirstOrDefault();

                if (Ano == null)
                {


                    Ano = new DTO.Usuario.DTOUsuarioImagen
                    {
                        imagen = ImagenDefault(),
                        extensionImagen = ".png",
                        nombre = (from a in db.Usuario
                                  where a.UsuarioId == usuarioId
                                  select (a.Nombre + " " + a.Paterno + " " + a.Materno).Trim()).FirstOrDefault()
                    };

                    
                }
                return new DTO.Usuario.DTOUsuarioImagen {
                    nombre = Ano.nombre,
                    imagenBase64 = Convert.ToBase64String(Ano.imagen),
                    extensionImagen = Ano.extensionImagen
                };

                //return new DTO.Usuario.DTOUsuarioImagen
                //{
                //    imagenBase64 = Convert.ToBase64String((from a in db.UsuarioImagen
                //                                           where a.UsuarioId == usuarioId
                //                                           select a.Imagen).ToList().FirstOrDefault()),
                //    extensionImagen = (from a in db.UsuarioImagenDetalle
                //                       where a.UsuarioId == usuarioId
                //                       select a.Extension).FirstOrDefault(),
                //                       nombre = 
                //};
            }
        }

        public static void InsertaBitacora(int usuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.UsuarioBitacora.Add(new UsuarioBitacora
                {
                    UsuarioId = usuarioId,
                    FechaIngreso = DateTime.Now,
                    HoraIngreso = DateTime.Now.TimeOfDay
                });

                db.SaveChanges();
            }
        }

        public static void RecuperaPassword(string email)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                //Verificar si el email esta asociado a un alumno
                var Emails = (from a in db.UsuarioDetalle
                              where a.Email == email
                              select a);

                if (Emails.Count() > 0)
                {
                    var Email = BLLVarios.RecuperaPassword(Emails.FirstOrDefault().UsuarioId, false);
                    Utilities.ProcessResult resultado = new Utilities.ProcessResult();
                    Utilities.Email.Enviar(Email.email, Email.password, Email.displayName, email, ';', "Solicitud de cambio de contraseña", Email.body, "", ';', Email.smtp, Email.puerto, Email.ssl, true, ref resultado);
                }
            }
        }

        public static DTO.DTOLogin LoginAdministrativo(int usuarioId, string password)
        {
            password = Utilities.Seguridad.Encripta(27, password);
            using (UniversidadEntities db = new UniversidadEntities())
            {    
                return (from a in db.Usuario
                        where a.UsuarioId == usuarioId
                        && a.Password == password
                        && a.EstatusId == 1
                        select new DTO.DTOLogin { 
                            usuarioId = a.UsuarioId
                        }).FirstOrDefault();
            }
        }

        public static DTO.DTOLogin LoginApp(int usuarioId, string password)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                string aux = Utilities.Seguridad.Encripta(27, password);

                int cuenta = db.Usuario.AsNoTracking()
                    .Where(u => u.UsuarioId == usuarioId && u.Password == aux && u.EstatusId == 1).Count();

                if (cuenta > 0)
                {
                    List<spUsuarioLogin_Result> resultado =
                            db.spUsuarioLogin(
                                usuarioId,
                                Utilities.Seguridad.Encripta(27, password),
                                Utilities.Aplicacion.DireccionFisica()
                            ).ToList();

                    if (resultado.Count > 0)
                        return
                        (from a in resultado
                         select new DTO.DTOLogin
                         {
                             usuarioId = a.UsuarioId,
                             nombre = a.Nombre,
                             email = a.Email,
                             sucursalCajaId = a.SucursalCajaId,
                             TipoAcceso = DTO.Perfil.Acceso.Ingresos
                         }).ToList().FirstOrDefault();

                    else
                        return (from a in db.Usuario.AsNoTracking()
                                join b in db.UsuarioDetalle.AsNoTracking() on a.UsuarioId equals b.UsuarioId
                                where a.UsuarioId == usuarioId
                                select new DTO.DTOLogin
                                {
                                    usuarioId = a.UsuarioId,
                                    nombre = (a.Nombre + " " + a.Paterno + " " + a.Materno).Trim(),
                                    email = b.Email,
                                    TipoAcceso = DTO.Perfil.Acceso.General
                                }).FirstOrDefault();
                }

                else
                    return null;
            }
        }

        public static void ActualizaPassword(string password, string token)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var UsuarioToken = db.UsuarioPasswordRecovery
                    .Where(usuario => usuario.Token == token)
                    .FirstOrDefault();

                var UsuarioPassword = db.Usuario
                    .Where(a => a.UsuarioId == UsuarioToken.UsuarioId)
                    .FirstOrDefault();

                //Verificar tamaño de pass en base
                UsuarioPassword.Password = Utilities.Seguridad.Encripta(27, password);

                UsuarioToken.EstatusId = 2;

                db.SaveChanges();
            }
        }
        public static List<DTO.DTOMenu> TraerMenu(int UsuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
               List<ICollection<TipoUsuarioSubmenu>> lstITipoUsu1 = (from m in db.Usuario
                                                               where m.UsuarioId == UsuarioId
                                                               select m.UsuarioTipo.TipoUsuarioSubmenu).ToList();
               List<TipoUsuarioSubmenu> lstTipoUsu = (List<TipoUsuarioSubmenu>)lstITipoUsu1[0].ToList();
               

                List<DTO.DTOMenu> lstMenu= new List<DTO.DTOMenu>();
                lstTipoUsu.ForEach(delegate(TipoUsuarioSubmenu objTipo)
                {
                    if (lstMenu.FindIndex(X => X.MenuId == objTipo.SubMenu.Menu.MenuId) < 0)
                    {
                        lstMenu.Add(new DTO.DTOMenu
                        {
                            Descripcion = objTipo.SubMenu.Menu.Descripcion,
                            MenuId = objTipo.SubMenu.Menu.MenuId
                        });
                    }
                });

                lstMenu.ForEach(delegate(DTO.DTOMenu objM)
                {
                    objM.SubMenu = (from a in lstTipoUsu
                                    where a.SubMenu.MenuId == objM.MenuId
                                    orderby a.SubmenuId
                                    select new DTO.DTOSubMenu
                                    {
                                        Descripcion = a.SubMenu.Descripcion,
                                        Direccion = a.SubMenu.Direccion,
                                        MenuId = a.SubMenu.MenuId,
                                        SubmenuId = a.SubMenu.SubmenuId
                                    }).ToList();
                });
                lstMenu = lstMenu.OrderBy(M => M.MenuId).ToList();
                return lstMenu;
            }
        }
    }
}
