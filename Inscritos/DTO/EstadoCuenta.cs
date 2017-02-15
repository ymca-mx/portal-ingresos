using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO.EstadoCuenta
{
    public class ReferenciaProcesada
    {
        public int referenciaProcesadaId { get; set; }
        public string fechaPago { get; set; }
        public string concepto { get; set; }
        public string referenciaId { get; set; }
        public string abono { get; set; }
        public string cargo { get; set; }
        public string restante { get; set; }
        public List<DTO.EstadoCuenta.PagoParcial> Pagos { get; set; }
    }
}

namespace Universidad.DTO.EstadoCuenta
{
    public class PagoParcial
    {
        public int referenciaProcesadaId { get; set; }
        public string fechaPago { get; set; }
        public string concepto { get; set; }
        //Para formar Pago.Concepto
        public string pagoId { get; set; }
        //Para formar Pago.Concepto
        public string reciboId { get; set; }
        public string referenciaId { get; set; }
        public string abono { get; set; }
        //PagoParcial.Pago
        public string cargo { get; set; }
        public string restante { get; set; }
        public int anio { get; set; }
        public int mesId { get; set; }
        public bool conceptodescriptivo { get; set; }

    }
}