using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Globalization;

namespace Pruebas
{
    [TestClass]
    public class Pagos
    {
        [TestMethod]
        public void PagosSemestrales()
        {
            BLL.BLLPagoPortal.GenerarSemestre(803, 44, 11, 6, 100000, 0, 2000);
        }
    }
}
