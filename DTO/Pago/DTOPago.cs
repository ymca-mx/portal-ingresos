using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universidad.DTO
{
    public class DTOPago
    {
        public DTOPago()
        {
            enUso = false;
            check = false;
        }

        [DisplayName(" ")]
        public bool enUso { get; set; }
        public int pagoId { get; set; }
        [DisplayName("Descripción")]
        public string conceptoPago { get; set; }
        public int conceptoPagoId { get; set; }
        [DisplayName("Cuota")]
        public decimal cuota { get; set; }
        public int cuotaId { get; set; }
        [DisplayName("Descuento")]
        public decimal descuento { get; set; }
        [DisplayName("Importe")]
        public decimal importe { get; set; }
        public int estatusId { get; set;}
        public int mesId { get; set; }
        public bool adeudo { get; set; }
        public bool check { get; set; }
        [System.ComponentModel.Browsable(false)]
        public List<DTOPagoDescuento> Descuentos { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int anio { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int periodoId { get; set; }
        public bool esVariable { get; set; }
    }


    public class DTOAlumnoDonativo
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public decimal Monto { get; set; }
    }

    public class DTODonativo
    {
        public List<DTOAlumnoDonativo> Alumnos { get; set; }
        public string ReferenciaId { get; set; }
        public int UsuarioId { get; set; }
    }

    public class DTOReferenciaDonativo
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");

        public DateTime fechaPago_ { get {return DateTime.Now; } set { fechaPago = value.ToString("dd/MM/yyyy", Cultura); } }
        public string fechaPago { get; set; }
        public string referenciaId { get; set; }
        public decimal importe { get; set; }
    }
}
