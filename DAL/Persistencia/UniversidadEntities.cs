using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public partial class UniversidadEntities
    {
        [DbFunction("UniversidadModel.Store", "fnConceptoDescuento")]
        public decimal fnConceptoDescuento(int conceptoPagoId, int alumnoId, int periodo, int anio)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;

            var parameters = new List<ObjectParameter>();
            parameters.Add(new ObjectParameter("conceptoPagoId", conceptoPagoId));
            parameters.Add(new ObjectParameter("alumnoId", alumnoId));
            parameters.Add(new ObjectParameter("periodo", periodo));
            parameters.Add(new ObjectParameter("anio", anio));

            return decimal.Parse(objectContext.CreateQuery<string>("UniversidadModel.Store.fnConceptoDescuento(@conceptoPagoId, @alumnoId, @periodo, @anio)", parameters.ToArray())
                 .Execute(MergeOption.NoTracking)
                 .FirstOrDefault());
        }

        [DbFunction("UniversidadModel.Store", "fnPagoDescuento")]
        public decimal fnPagoDescuento(int pagoId)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;

            var parameters = new List<ObjectParameter>();
            parameters.Add(new ObjectParameter("pagoId", pagoId));

            return decimal.Parse(objectContext.CreateQuery<string>("UniversidadModel.Store.fnPagoDescuento(@pagoId)", parameters.ToArray())
                 .Execute(MergeOption.NoTracking)
                 .FirstOrDefault());
        }

        [DbFunction("UniversidadModel.Store", "fnImporteLetra")]
        public string fnImporteLetra(string numero, string moneda, bool centavo)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;

            var parameters = new List<ObjectParameter>();
            parameters.Add(new ObjectParameter("numeroProcesar", numero));
            parameters.Add(new ObjectParameter("moneda", moneda));
            parameters.Add(new ObjectParameter("centavo", centavo));

            return (objectContext.CreateQuery<string>("UniversidadModel.Store.fnImporteLetra(@numeroProcesar, @moneda, @centavo)", parameters.ToArray())
                 .Execute(MergeOption.NoTracking)
                 .FirstOrDefault());
        }

        [DbFunction("UniversidadModel.Store", "fnDetalleDescuentoConcepto")]
        public decimal fnDetalleDescuentoConcepto(int alumnoDescuentoId)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;

            var parameters = new List<ObjectParameter>();
            parameters.Add(new ObjectParameter("alumnoDescuentoId", alumnoDescuentoId));


            return decimal.Parse(objectContext.CreateQuery<string>("UniversidadModel.Store.fnDetalleDescuentoConcepto(@alumnoDescuentoId)", parameters.ToArray())
                 .Execute(MergeOption.NoTracking)
                 .FirstOrDefault());
        }       

    }
}
