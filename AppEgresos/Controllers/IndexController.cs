using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL;

namespace AppEgresos.Controllers
{
    public class IndexController : ApiController
    {
        [HttpGet]
        [ActionName("Datos")]
        public IHttpActionResult Datos( int usuarioId)
        {
            

            using (UniversidadEntities db = new UniversidadEntities())
            {

                var Ano = (from a in db.Usuario
                           join c in db.UsuarioImagen on a.UsuarioId equals c.UsuarioId
                           join d in db.UsuarioImagenDetalle on a.UsuarioId equals d.UsuarioId
                           where a.UsuarioId == usuarioId
                           select new DTOUsuarioImagen
                           {
                               nombre = (a.Nombre + " " + a.Paterno + " " + a.Materno).Trim(),
                               imagen = c.Imagen,
                               extensionImagen = d.Extension
                           }).FirstOrDefault();

                if (Ano == null)
                {


                    Ano = new DTOUsuarioImagen
                    {
                        imagen = ImagenDefault(),
                        extensionImagen = ".png",
                        nombre = (from a in db.Usuario
                                  where a.UsuarioId == usuarioId
                                  select (a.Nombre + " " + a.Paterno + " " + a.Materno).Trim()).FirstOrDefault()
                    };


                }
                
                return Ok((new DTOUsuarioImagen
                {
                    nombre = Ano.nombre,
                    imagenBase64 = Convert.ToBase64String(Ano.imagen),
                    extensionImagen = Ano.extensionImagen
                }));
                
            }

        }


        [HttpGet]
        [ActionName("ConsultarMenu")]
        public IHttpActionResult ConsultarMenu(int usuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                

                int tipoUsuario = db.Usuario.Where(a => a.UsuarioId == usuarioId).FirstOrDefault().UsuarioTipoId;

                List<TipoUsuarioSubmenu1> lstTipoUsu = db.TipoUsuarioSubmenu1.Where(a=> a.UsuarioTipoId == tipoUsuario).ToList();
                
                List<DTOMenu> lstMenu = new List<DTOMenu>();
                lstTipoUsu.ForEach(delegate (TipoUsuarioSubmenu1 objTipo)
                {
                    if (lstMenu.FindIndex(X => X.MenuId == objTipo.SubMenu1.Menu1.MenuId) < 0)
                    {
                        lstMenu.Add(new DTOMenu
                        {
                            Descripcion = objTipo.SubMenu1.Menu1.Descripcion,
                            MenuId = objTipo.SubMenu1.Menu1.MenuId
                        });
                    }
                });

                lstMenu.ForEach(delegate (DTOMenu objM)
                {
                    objM.SubMenu = (from a in lstTipoUsu
                                    where a.SubMenu1.MenuId == objM.MenuId
                                    orderby a.SubmenuId
                                    select new DTOSubMenu
                                    {
                                        Descripcion = a.SubMenu1.Descripcion,
                                        Direccion = a.SubMenu1.Direccion,
                                        MenuId = a.SubMenu1.MenuId,
                                        SubmenuId = a.SubMenu1.SubmenuId
                                    }).ToList();
                });
                lstMenu = lstMenu.OrderBy(M => M.MenuId).ToList();

                return Ok(lstMenu);
            }

            
        }

            private static byte[] ImagenDefault()
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

    }
}
