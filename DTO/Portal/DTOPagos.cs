using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPagos
    {
        public int PagoId { get; set; }
        public int AlumnoId { get; set; }
        public DTOAlumno DTOAlumno { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public int SubperiodoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public System.DateTime FechaGeneracion { get; set; }
        public System.TimeSpan HoraGeneracion { get; set; }
        public string FechaGeneracionS { get; set; }
        public string HoraGeneracionS { get; set; }
        public int CuotaId { get; set; }
        public decimal Cuota { get; set; }
        public decimal Promesa { get; set; }
        public string Restante { get; set; }
        public string Pagado { get; set; }
        public Nullable<decimal> Pago1 { get; set; }
        public Nullable<System.DateTime> FechaPago { get; set; }
        public Nullable<System.TimeSpan> HoraPago { get; set; }
        private string referencia;
        public List<DTOPagoParcial> DTOParcial { get; set; }
        public string Referencia
        {
            get { return referencia; }
            set { referencia = int.Parse(value).ToString(); } 
        }
        public int EstatusId { get; set; }
        public bool EsEmpresa { get; set; }
        public DTOCuota DTOCuota { get; set; }
        public DTOOfertaEducativa OfertaEducativa { get; set; }
        public List<DTOPagoDescuento> lstPagoDescuento { get; set; }
        public DTOSubPeriodo DTOSubPeriodo { get; set; }
        public DTOPeriodo DTOPeriodo { get; set; }
        public Pagos_Detalles objAnticipado1 { get; set; }
        public Pagos_Detalles objAnticipado2 { get; set; }
        public Pagos_Detalles objNormal { get; set; }
        public Pagos_Detalles objRetrasado { get; set; }
        public DTOPagoRecargo PagoRecargo { get; set; }
        public int PeriodoAnticipadoId { get; set; }
        public List<DTOAlumnoReferenciaBitacora> AlumnoReferenciaBitacora { get; set; }
        public int Usuario { get; set; }
        public int UsuarioTipo { get; set; }
        public  DTOReferenciadoCabecero ReferenciadoCabecero { get; set; }

        public bool Cancelable { get; set; }
    }
    public class Pagos_Detalles
    {
        public string Monto { get; set; }
        public string Restante { get; set; }
        public string Recargo { get; set; }
        public string Total { get; set; }
        public  string FechaLimite { get; set; }
        public string Observaciones { get; set; }
        public string Estatus { get; set; }
    }



    public class DTOPagoCancelacionSolicitud
    {
        public int solicitudId { get; set; }
        public int pagoId { get; set; }
        public System.DateTime fechaSolicitud1 { get; set; }
        public string fechaSolicitud { get; set; }
        public string comentario { get; set; }
        public string usuarioIdSolicitud { get; set; }
        public System.DateTime fechaAplicacion1 { get; set; }
        public string fechaAplicacion { get; set; }
        public int usuarioIdAutoriza { get; set; }
        public string estatusId { get; set; }
    }


}

