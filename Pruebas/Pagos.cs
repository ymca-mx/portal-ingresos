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
            BLL.BLLPagoPortal.GenerarSemestre(6704, 44, 2017, 11, 2018, 4, 8358, 0, 3600);
        }
    }
}
