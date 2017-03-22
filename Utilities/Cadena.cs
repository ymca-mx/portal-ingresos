using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Cadena
    {
        public static String SinAcentos(String Texto)
        {
            String Resultado;
            byte[] Arreglo;
            Arreglo = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(Texto);
            Resultado = System.Text.Encoding.UTF8.GetString(Arreglo);

            return Resultado;
        }

        public static string[] Separar(char separador, string texto)
        {
            char[] separadores = { separador };
            return texto.Split(separadores);
        }

        /// <summary>
        /// De manera aleatoria genera un cadena, formada por numeros, letras minusculas y mayusculas
        /// </summary>
        /// <param name="longitud">Longitud del password que se va a generar</param>
        /// <returns></returns>
        public static string GeneraPassword(int longitud)
        {
            Random Rnd = new Random();
            int digito;
            int tipoDigito;
            string cadena = "";

            for (int i = 0; i <= longitud; i++)
            {
                tipoDigito = Rnd.Next(1, 10);

                digito = tipoDigito < 4 ? Rnd.Next(48, 57) :
                    tipoDigito > 3 && tipoDigito < 7 ? Rnd.Next(65, 90) : Rnd.Next(97, 122);
                cadena += ((char)digito).ToString();
            }
            return cadena;
        }


    }
}
