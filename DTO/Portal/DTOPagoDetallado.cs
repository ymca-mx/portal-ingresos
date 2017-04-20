using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPagoDetallado
    {
        public int EsSep { get; set; }
        public string Concepto { get; set; }
        public int Pagoid { get; set; }
        public string ReferenciaId { get; set; }
        public string CargoMonto { get; set; }
        public string CargoFechaLimite { get; set; }
        public string DescuentoXAnticipado { get; set; }
        public string Cargo_Descuento { get; set; }
        public string BecaAcademica_Pcj { get; set; }
        public string BecaAcademica_Monto { get; set; }
        public string BecaOpcional_Pcj { get; set; }
        public string BecaOpcional_Monto { get; set; }
        public string TotalMDescuentoMBecas { get; set; }
        public string SaldoPagado { get; set; }
        public decimal Total_a_Pagar { get; set; }
        public string Total_a_PagarS { get; set; }
        public int OfertaEducativaId { get; set; }
        public string DescripcionOferta { get; set; }
        public string BecaSEP { get; set; }
        public Boolean Adeudo { get; set; }
        public decimal SaldoAdeudo { get; set; }
        public string TotalPagado { get; set; }
        public bool esEmpresa { get; set; }
        public bool esEspecial { get; set; }
        public string OtroDescuento { get; set; }
        public string Periodo { get; set; }
        public string Pagado { get; set; }
        public string Usuario { get; set; }


    }
}
