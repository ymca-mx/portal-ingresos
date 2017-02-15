using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universidad.DAL;

namespace Universidad.Test
{
    [TestClass]
    public class MetodosEF
    {
        [TestMethod]
        public void TestAsEnumerable()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var Item = db.Pago.Where(pago => pago.ReferenciaId == "0000002550").AsEnumerable().FirstOrDefault();
            }
 
        }

        [TestMethod]
        public void TestAsList()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var Item = db.Pago.Where(pago => pago.ReferenciaId == "0000002550").ToList().FirstOrDefault();
            }
        }
        
    }
}
