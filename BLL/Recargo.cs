using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Recargo
    {
        public static int GenerarRecargo(int PagoId, DateTime Fecha)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    string Monto = db.SistemaConfiguracion.Where(S => S.ParametroId == 3).FirstOrDefault().Valor;

                    decimal Promesa = decimal.Parse(Monto);
                    Promesa = Promesa / 100;
                    DateTime fHoy = Fecha;

                    Periodo objPeriodo = db.Periodo.Where(P => fHoy >= P.FechaInicial && fHoy <= P.FechaFinal).FirstOrDefault();

                    Pago objPago = db.Pago.Where(P => P.PagoId == PagoId).FirstOrDefault();

                    Cuota objCuota = db.Cuota.Where(C => C.OfertaEducativaId == objPago.OfertaEducativaId
                        && C.PeriodoId == objPeriodo.PeriodoId && C.Anio == objPeriodo.Anio && C.PagoConceptoId == 306).FirstOrDefault();

                    Promesa = Math.Round(Promesa * objPago.Promesa);
                    db.Pago.Add(new Pago
                    {
                        ReferenciaId = "",
                        AlumnoId = objPago.AlumnoId,
                        Anio = objPeriodo.Anio,
                        PeriodoId = objPeriodo.PeriodoId,
                        SubperiodoId = 1,
                        OfertaEducativaId = objPago.OfertaEducativaId,
                        FechaGeneracion = DateTime.Now,
                        CuotaId = objCuota.CuotaId,
                        Cuota = Promesa,
                        Promesa = Promesa,
                        EstatusId = 1,
                        EsEmpresa = false,
                        EsAnticipado = false

                    });

                    db.SaveChanges();
                    Pago objPagoRecargo = db.Pago.Local.Where(PL => PL.ReferenciaId == "").FirstOrDefault();

                    objPagoRecargo.ReferenciaId = db.spGeneraReferencia(objPagoRecargo.PagoId).FirstOrDefault();

                    db.PagoRecargo.Add(new PagoRecargo
                    {
                        PagoId = objPago.PagoId,
                        PagoIdRecargo = objPagoRecargo.PagoId,
                        Fecha = DateTime.Now
                    });

                    db.SaveChanges();
                    return objPagoRecargo.PagoId;
                }
                catch 
                {
                    return 0;
                }
            }
        }

        public static void LlamarReporte()
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                //var consulta = db.SPSBecasCuatrimestre(2017, 1);

            }
        }
    }
}
