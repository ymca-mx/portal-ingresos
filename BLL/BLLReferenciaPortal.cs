using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLReferenciaPortal
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Pago"></param>
        /// <param name="Referencia">Para comparar contra Pago.Promesa</param>
        /// <returns>Si refrencia es mayor, menor o igual</returns>
        public static Universidad.DAL.Importe VerificaImporte(DAL.Pago Pago, DTO.DTOReferencia Referencia)
        {
            return Referencia.importe > Pago.Promesa ? Universidad.DAL.Importe.Mayor :
                (Referencia.importe < Pago.Promesa ? Universidad.DAL.Importe.Menor : Universidad.DAL.Importe.Igual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Pago"></param>
        /// <param name="cantidad">Para comparar contra Pago.Promesa</param>
        /// <returns></returns>
        public static Universidad.DAL.Importe VerificaImporteCantidad(DAL.Pago Pago, decimal cantidad)
        {
            return cantidad > Pago.Promesa ? Universidad.DAL.Importe.Mayor :
                (cantidad < Pago.Promesa ? Universidad.DAL.Importe.Menor : Universidad.DAL.Importe.Igual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cantidadReferencia"></param>
        /// <param name="cantidadPromesa"></param>
        /// <returns>Si Referencia es mayor > </returns>
        public static Universidad.DAL.Importe VerificaCantidadCantidad(decimal cantidadReferencia, decimal cantidadPromesa)
        {
            return cantidadReferencia > cantidadPromesa ? Universidad.DAL.Importe.Mayor :
                (cantidadReferencia < cantidadPromesa ? Universidad.DAL.Importe.Menor : Universidad.DAL.Importe.Igual);
        }
        public static string DescuentoAnticipado()
        {
            using (DAL.UniversidadEntities db = new DAL.UniversidadEntities())
            {
                return db.SistemaConfiguracion.Where(S => S.ParametroId == 4).FirstOrDefault().Valor;
            }
        }
    }
}
