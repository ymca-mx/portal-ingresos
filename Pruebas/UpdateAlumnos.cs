using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL;
using DTO;
using Herramientas;
using System.Linq;

namespace Pruebas
{
    [TestClass]
    public class UpdateAlumnos
    {
        [TestMethod]
        public void ACtualizacionMatricula()
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                DAL.Alumno Alumnodb = db.Alumno.Where(a => a.EstatusId == 2).FirstOrDefault();

                db.Entry(Alumnodb).State = System.Data.Entity.EntityState.Modified;

                var p1= db.Set<DAL.Alumno>().Include(p=> p.Alumno).FirstOrDefault(p=> p.
            }
        }
    }
}
