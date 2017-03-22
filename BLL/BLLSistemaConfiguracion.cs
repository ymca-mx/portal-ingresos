using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace  Universidad.BLL
{
    public class BLLSistemaConfiguracion
    {
        public static int ComisionTC()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return int.Parse((from a in db.SistemaConfiguracion
                                  where a.Parametro == "Comisión TC"
                                  select a.Valor).FirstOrDefault());
            }
        }
    }
}
