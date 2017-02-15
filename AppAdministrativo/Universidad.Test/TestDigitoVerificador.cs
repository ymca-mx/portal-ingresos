using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Universidad.Test
{
    [TestClass]
    public class TestDigitoVerificador
    {
        [TestMethod]
        public void Comprobacion()
        {
            int v1 = Utilities.DigitoVerificador.Obtener(1);
            int v2 = Utilities.DigitoVerificador.Obtener(88);
            int v3 = Utilities.DigitoVerificador.Obtener(700);
            int v4 = Utilities.DigitoVerificador.Obtener(918273465);
            int v5 = Utilities.DigitoVerificador.Obtener(4879);
            int v6 = Utilities.DigitoVerificador.Obtener(412796);
            int v7 = Utilities.DigitoVerificador.Obtener(45870);
            int v8 = Utilities.DigitoVerificador.Obtener(9999999);
            int v9 = Utilities.DigitoVerificador.Obtener(123456789);
            int v10 = Utilities.DigitoVerificador.Obtener(987654321);
        }
    }
}
