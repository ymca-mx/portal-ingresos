using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Anticipado
    {
        public static string AplicarDescuento(int PagoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    Pago objPago = db.Pago.Where(P => P.PagoId == PagoId).FirstOrDefault();
                    Descuento objDesc = db.Descuento.Where(D => D.OfertaEducativaId == objPago.OfertaEducativaId && D.Descripcion == "Pago Anticipado" && D.PagoConceptoId == objPago.Cuota1.PagoConceptoId).FirstOrDefault();
                    string Monto = db.SistemaConfiguracion.Where(S => S.ParametroId == 4).FirstOrDefault().Valor;
                    decimal DMonto = decimal.Parse(Monto);
                    decimal Promesa = 100 - DMonto;
                    DMonto = DMonto / 100;
                    Promesa = Promesa / 100;
                    decimal Pagar = Math.Round(objPago.Promesa * DMonto);

                    objPago.Promesa = Math.Round(decimal.Parse((objPago.Promesa * decimal.Parse(Promesa.ToString())).ToString()));
                    objPago.EsAnticipado = true;
                    db.PagoDescuento.Add(new PagoDescuento
                    {
                        DescuentoId = objDesc.DescuentoId,
                        Monto = Pagar,
                        PagoId = objPago.PagoId
                    });

                    db.SaveChanges();
                    return "Guardado";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }

        }
    }
}
