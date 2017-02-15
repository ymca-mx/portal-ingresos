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
    public class BLLPerfil
    {
        //public static List<DTOPerfiles> ConsultarTodos()
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        try
        //        {
        //            return (from a in db.Perfiles
        //                    select new DTOPerfiles
        //                    {
        //                        Descripcion = a.Descripcion,
        //                        FechaAlta = a.FechaAlta,
        //                        PerfilId = a.PerfilId
        //                    }).AsNoTracking().ToList();
        //        }
        //        catch (Exception)
        //        {
        //            return null;
        //        }
        //    }
        //}
        //public static Nullable<int> GuardarPerfil(DTOPerfiles objGuardar)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        try
        //        {
        //            db.Perfiles.Add(new Perfiles
        //            {
        //                Descripcion = objGuardar.Descripcion,
        //                FechaAlta = objGuardar.FechaAlta,
        //                //PerfilId = objGuardar.PerfilId
        //            });

        //            db.SaveChanges();

        //            return db.Perfiles.Local[0].PerfilId;
        //        }
        //        catch (Exception) { return null; }
        //    }
        //}

        //public static string ModificarPerfil(DTOPerfiles dTOPerfiles)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        try
        //        {
        //            Perfiles objGuardar = db.Perfiles.Where(P => P.PerfilId == dTOPerfiles.PerfilId).FirstOrDefault();
        //            objGuardar.Descripcion = dTOPerfiles.Descripcion;
        //            objGuardar.FechaAlta = dTOPerfiles.FechaAlta;
        //            db.SaveChanges();

        //            return "Guardado";
        //        }
        //        catch (Exception e)
        //        {
        //            return e.Message;
        //        }
        //    }
        //}

        //public static string EliminarPerfil(int PerfilId)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        try
        //        {
        //            db.Perfiles.Remove(db.Perfiles.Where(P => P.PerfilId == PerfilId).FirstOrDefault());
        //            db.SaveChanges();
        //            return "Guardado";
        //        }
        //        catch (Exception e)
        //        {
        //            return e.Message;
        //        }
        //    }
        //}
    }
}
