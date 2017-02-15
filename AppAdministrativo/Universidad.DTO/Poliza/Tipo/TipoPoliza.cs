using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO.Poliza.Tipo
{
    public enum Tipo
    {
        Ingresos = 1,
        Egresos = 2,
        Diario = 3,
        Orden = 4,
        Estadisticas = 5
    }

    public enum Subtipo
    {
        Caja = 1,
        Referenciados = 2
    }
}
