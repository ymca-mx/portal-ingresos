using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLUsuario
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
