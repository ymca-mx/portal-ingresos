using SimpleImpersonation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                using (Impersonation.LogonUser("172.16.1.204", "Administrador", "41rM43st2011", LogonType.NewCredentials))
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(@"\\172.16.1.204\\Unidades\\Universidad\\Fotos\\" + alumnoFoto);
                    base64Image = Convert.ToBase64String(imageArray);
                }
            }
            catch (Exception)
            {
                using (Impersonation.LogonUser("172.16.1.204", "Administrador", "41rM43st2011", LogonType.NewCredentials))
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(@"\\172.16.1.204\\Unidades\\Universidad\\Fotos\\SINFOTO.jpg");
                    base64Image = Convert.ToBase64String(imageArray);
                }
            }
            return base64Image;
        }
    }
}
