using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace Herramientas
{
    public class ConvertidorT
    {
        public static byte[] ConvertirStream(Stream stream, int tamaño)
        {
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(stream))
            {
                fileData = binaryReader.ReadBytes(tamaño);
                return fileData;
            }
        }
        public static string CrearPass()
        {
            Random rnd = new Random();
            int CodigoASC;
            int inicial;
            string Pass="";

            for(int i=0;i<=5;i++)
            {
                inicial = rnd.Next(1, 4);

                CodigoASC = inicial == 1 ? rnd.Next(48, 58) : 
                    inicial == 2 ? rnd.Next(65, 91) : rnd.Next(97, 123);
                Pass += ((char)CodigoASC).ToString();
            }
            return Pass;
        }
    }
}
