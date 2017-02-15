using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DAL
{
    public partial class AlumnoImagenDetalle
    {
        public AlumnoImagenDetalle()
        {
            this.FechaCarga = DateTime.Now;
            this.HoraCarga = DateTime.Now.TimeOfDay;
        }
    }
}
