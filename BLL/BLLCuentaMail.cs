using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLCuentaMail
    {
        public static DTOCuentaMail ConsultarCuentaMail()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.CuentaMail
                        where a.CuentaMailId == 1
                        select new DTOCuentaMail
                        {
                            CuentaMailId = a.CuentaMailId,
                            Descripcion = a.Descripcion,
                            DisplayName = a.DisplayName,
                            Email = a.Email,
                            EstatusId = a.EstatusId,
                            Password = a.Password,
                            Puerto = a.Puerto,
                            Smtp = a.Smtp,
                            SSL = a.SSL
                        }).FirstOrDefault();
            }
        }
    }
}
