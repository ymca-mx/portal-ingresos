using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Seguridad
    {
        public static String Encripta(int clave, String cadena)
        {
            String Valores = "";
            for (int i = 0; i < cadena.Length; i++)
            {
                int iValor = (int)Convert.ToChar(cadena.Substring(i, 1)) * clave;
                String sHexa = String.Format("{0:X}", iValor);
                sHexa = "0" + sHexa;
                Valores += sHexa;
            }

            return Valores;
        }
        public static String Desencripta(int clave, String cadena)
        {
            String Valores = "";
            for (int i = 0; i < cadena.Length; i = i + 4)
            {
                String X = cadena.Substring(i, 4);
                int iValor = Int32.Parse(X, System.Globalization.NumberStyles.HexNumber) / clave;
                Char Caracter = (char)iValor;
                Valores = Valores + Caracter;
            }

            return Valores;
        }       
    }
}
