using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class ProcessResult
    {
        public ProcessResult() { }
        public bool Estatus { get; set; }
        public string Mensaje { get; set; }
        public string MensajeDetalle { get; set; }
        public string Informacion { get; set; }
    }
}
