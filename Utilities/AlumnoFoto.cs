using SimpleImpersonation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utilities
{
    public class AlumnoFoto
    {
        public static string GetAlumnoFotoBase64(int AlumnoId)
        {
            string base64Image;
            try
            {
                string alumnoFoto = (AlumnoId.ToString() + "60.jpg").PadLeft(11, '0');

                var webClient = new WebClient();
                byte[] imageArray = webClient.DownloadData("http://108.163.172.122/Fotos/Universidad/"+ alumnoFoto);
                base64Image = Convert.ToBase64String(imageArray);
                //using (Impersonation.LogonUser("172.16.1.204", "Administrador", "41rM43st2011", LogonType.NewCredentials))
                //{
                //    byte[] imageArray = System.IO.File.ReadAllBytes(@"\\172.16.1.204\\Unidades\\Universidad\\Fotos\\" + alumnoFoto);
                //}
            }
            catch (Exception)
            {
                var url = HttpContext.Current.Server.MapPath("../../../Imagenes");
                byte[] imageArray = System.IO.File.ReadAllBytes(url + "/SinFoto.png");
                base64Image = Convert.ToBase64String(imageArray);
            }
            return base64Image;
        }

        

    }
}
