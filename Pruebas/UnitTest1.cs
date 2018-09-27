using System;
using System.Linq;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pruebas
{
    [TestClass]
    public class PruebaBiblioteca
    {
        [TestMethod]
        public void SendComunicado()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var listUsuarios = db.Usuario
                                                    .Where(us => us.UsuarioId == 8696
                                                                || us.UsuarioId == 100000)
                                                    .Select(us => new
                                                    {
                                                        us.UsuarioId,
                                                        us.Nombre,
                                                        us.Paterno,
                                                        us.Materno,
                                                        us.UsuarioDetalle.Email
                                                    }).ToList();
            }            
        }

        [TestMethod]
        public void PruebaAlumnosTitulo()
        {
                var list =
                BLL.BLLSEP.AlumnosFirmados();

            Console.Write(list);
        }
    }
}
