using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utilities
{
    public class Archivo
    {
        public static void CrearTXT(string directorio, List<string> Lineas)
        {
            using (FileStream strm = File.Create(directorio))
            {

            }
            File.AppendAllLines(directorio, Lineas);
            //TextWriter tw = new StreamWriter(directorio, false);
            //tw.Write(Lineas);

            //--System.IO.FileOptions.None

            //System.Security.AccessControl.FileSecurity a = new System.Security.AccessControl.FileSecurity();
            //a.AccessRightType.
            //FileStream fr = File.Create("",)

        }

        public static bool ModificarTXT(string directorio, List<string> Lineas)
        {
            if (!File.Exists(directorio))
                return false;
            else
            {
                
                return true;
            }
        }

        public static byte[] Bytes(string Directorio)
        {
            MemoryStream ms = new MemoryStream();
            FileStream fs = new FileStream(Directorio, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            ms.SetLength(fs.Length);
            fs.Read(ms.GetBuffer(), 0, (int)fs.Length);

            byte[] arrBytes = ms.GetBuffer();
            ms.Flush();
            fs.Close();

            return arrBytes;
        }

        public static Image Imagen(byte[] bytesImagen)
        {
            Image Archivo = null;
            try
            {
                MemoryStream ms = new MemoryStream(bytesImagen);
                Archivo = Image.FromStream(ms);
                ms.Close();
                return Archivo;
            }
            catch (Exception)
            {
                return Archivo;
            }
        }

        public static List<string> LecturaTXT(string directorio)
        {
            List<string> lineas = new List<string>();

            using (StreamReader Lector = new StreamReader(directorio))
            {
                while (!Lector.EndOfStream)
                    lineas.Add(Lector.ReadLine());

                Lector.Close();
                return lineas;
            }
        }
    }
}
