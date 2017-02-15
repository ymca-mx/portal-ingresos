using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DAL
{
    public enum UniEstatus
    {
        Activo,
        Inactivo,
        Cancelado,
        Pagado
    }

    public enum UniTurno
    {
        Matutino,
        Vespertino,
        Sabatino,
        Nocturno,
        Otro
    }

    public enum Importe
    {
        Menor = 1,
        Igual = 2,
        Mayor = 3
    }
}
