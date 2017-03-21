using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOCuentaMail
    {
        public int CuentaMailId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Smtp { get; set; }
        public int Puerto { get; set; }
        public bool SSL { get; set; }
        public string Descripcion { get; set; }
        public int EstatusId { get; set; }
    }
}
