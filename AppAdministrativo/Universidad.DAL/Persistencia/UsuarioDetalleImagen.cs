using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DAL
{
    public partial class UsuarioImagenDetalle
    {
        public UsuarioImagenDetalle()
        {
            this.FechaCarga = DateTime.Now;
            this.HoraCarga = DateTime.Now.TimeOfDay;
        }
    }
}
