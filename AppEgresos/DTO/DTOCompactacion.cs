using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppEgresos
{
    public class DTOCompactacion
    {
        
    }

    public class DTOOfertaEducativa
    {
        public int ofertaEducativaId { get; set; }
        public int ofertaEducativaTipoId { get; set; }
        public string ofertaEducativa { get; set; }
    }

    public class DTOMateria
    {
        public int materiaId { get; set; }
        public string materia { get; set; }
        public string clave { get; set; }
        public int? ofertaEducativaId { get; set; }
        public string creditos { get; set; }
    }

    public class DTOGrupo
    {
        public int grupoId { get; set; }
        public int grupo { get; set; }
        public int ofertaEducativaId { get; set; }
    }


    public class DTOCatalogoCompactacion
    {
        public List<DTOMateria>  materias { get; set; }
        public List<DTOGrupo>  grupos { get; set; }
    }



}