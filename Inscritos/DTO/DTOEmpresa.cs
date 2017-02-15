using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOEmpresa
    {
        public int EmpresaId { get; set; }
        public string RFC { get; set; }
        public string RazonSocial { get; set; }
        public System.DateTime FechaAlta { get; set; }
        public string FechaAltaS { get; set; }
        public System.DateTime FechaVigencia { get; set; }
        public int Usuarioid { get; set; }
        public DTOEmpresaDetalle EmpresaDetalle { get; set; }
        public List<DTOGrupo> Grupo { get; set; }

    }

    public class DTOEmpresaLigera
    {
        public int EmpresaId { get; set; }
        public string RFC { get; set; }
        public string RazonSocial { get; set; }
        public List<DTOGrupoLigero> Grupo { get; set; }

    }

}
