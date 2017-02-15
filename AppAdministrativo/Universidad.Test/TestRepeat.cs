using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.Test
{
    [TestClass]
    public class TestRepeat
    {
        [TestMethod]
        public void Comprobacion()
        {
            string a = string.Concat(Enumerable.Repeat("0", 10));
        }
    }
}
