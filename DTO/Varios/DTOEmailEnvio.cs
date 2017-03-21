using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO.Varios
{
    public class DTOEmailEnvio
    {
        public string email { get; set; }
        public string password { get; set; }
        public string displayName { get; set; }
        public string smtp { get; set; }
        public int puerto { get; set; }
        public bool ssl { get; set; }
        public string body { get; set; }
    }
}
