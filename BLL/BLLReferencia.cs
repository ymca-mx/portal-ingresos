using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using DAL;

namespace Universidad.BLL
{
    public class BLLReferencia
    {
        static Utilities.ProcessResult Resultado = new Utilities.ProcessResult();
        public static DTO.DTOReferenciaLayout2 LayoutScotiabank()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTO.DTOReferenciaLayout> Layout = (from a in db.ReferenciadoLayout
                                                        where a.Banco.Equals("Scotiabank")
                                                     select new DTO.DTOReferenciaLayout
                                                     {
                                                         descripcion = a.Descripcion,
                                                         posicionInicial = a.PosicionInicial,
                                                         posicionFinal = a.PosicionFinal
                                                     }).ToList();

                return (
                    new DTO.DTOReferenciaLayout2
                    {
                        //Tipo = new DTO.DTOPosicion
                        //{
                        //    inicial = Layout.Where(conf => conf.descripcion.Equals("Tipo")).FirstOrDefault().posicionInicial,
                        //    final = Layout.Where(conf => conf.descripcion.Equals("Tipo")).FirstOrDefault().posicionFinal
                        //},
                        //Moneda = new DTO.DTOPosicion
                        //{
                        //    inicial = Layout.Where(conf => conf.descripcion.Equals("Moneda")).FirstOrDefault().posicionInicial,
                        //    final = Layout.Where(conf => conf.descripcion.Equals("Moneda")).FirstOrDefault().posicionFinal
                        //},
                        //Plaza = new DTO.DTOPosicion
                        //{
                        //    inicial = Layout.Where(conf => conf.descripcion.Equals("Plaza")).FirstOrDefault().posicionInicial,
                        //    final = Layout.Where(conf => conf.descripcion.Equals("Plaza")).FirstOrDefault().posicionFinal
                        //},
                        //Cuenta = new DTO.DTOPosicion
                        //{
                        //    inicial = Layout.Where(conf => conf.descripcion.Equals("Cuenta")).FirstOrDefault().posicionInicial,
                        //    final = Layout.Where(conf => conf.descripcion.Equals("Cuenta")).FirstOrDefault().posicionFinal
                        //},
                        FechaPago = new DTO.DTOPosicion
                        {
                            inicial = Layout.Where(conf => conf.descripcion.Equals("Fecha")).FirstOrDefault().posicionInicial,
                            final = Layout.Where(conf => conf.descripcion.Equals("Fecha")).FirstOrDefault().posicionFinal
                        },
                        ReferenciaId = new DTO.DTOPosicion
                        {
                            inicial = Layout.Where(conf => conf.descripcion.Equals("Referencia")).FirstOrDefault().posicionInicial,
                            final = Layout.Where(conf => conf.descripcion.Equals("Referencia")).FirstOrDefault().posicionFinal
                        },
                        Importe = new DTO.DTOPosicion
                        {
                            inicial = Layout.Where(conf => conf.descripcion.Equals("Importe")).FirstOrDefault().posicionInicial,
                            final = Layout.Where(conf => conf.descripcion.Equals("Importe")).FirstOrDefault().posicionFinal
                        },
                        Movimiento = new DTO.DTOPosicion
                        {
                            inicial = Layout.Where(conf => conf.descripcion.Equals("Movimiento")).FirstOrDefault().posicionInicial,
                            final = Layout.Where(conf => conf.descripcion.Equals("Movimiento")).FirstOrDefault().posicionFinal
                        },
                        //Saldo = new DTO.DTOPosicion
                        //{
                        //    inicial = Layout.Where(conf => conf.descripcion.Equals("Saldo")).FirstOrDefault().posicionInicial,
                        //    final = Layout.Where(conf => conf.descripcion.Equals("Saldo")).FirstOrDefault().posicionFinal
                        //},
                        //Transaccion = new DTO.DTOPosicion
                        //{
                        //    inicial = Layout.Where(conf => conf.descripcion.Equals("Transaccion")).FirstOrDefault().posicionInicial,
                        //    final = Layout.Where(conf => conf.descripcion.Equals("Transaccion")).FirstOrDefault().posicionFinal
                        //},
                        //Leyenda = new DTO.DTOPosicion
                        //{
                        //    inicial = Layout.Where(conf => conf.descripcion.Equals("Leyenda")).FirstOrDefault().posicionInicial,
                        //    final = Layout.Where(conf => conf.descripcion.Equals("Leyenda")).FirstOrDefault().posicionFinal
                        //},
                        //Leyenda2 = new DTO.DTOPosicion
                        //{
                        //    inicial = Layout.Where(conf => conf.descripcion.Equals("Leyenda2")).FirstOrDefault().posicionInicial,
                        //    final = Layout.Where(conf => conf.descripcion.Equals("Leyenda2")).FirstOrDefault().posicionFinal
                        //},
                        Consecutivo = new DTO.DTOPosicion
                        {
                            inicial = Layout.Where(conf => conf.descripcion.Equals("Consecutivo")).FirstOrDefault().posicionInicial,
                            final = Layout.Where(conf => conf.descripcion.Equals("Consecutivo")).FirstOrDefault().posicionFinal
                        },
                    }
                    );
            }
        }

        public static void Procesar()
        {
            DTO.DTOReferenciaLayout2 Lay = LayoutScotiabank();
            List<string> Lineas = Utilities.Archivo.LecturaTXT("C://RefScotiaBank//SC070116.txt");
            List<DTO.DTOReferencia> Referencias = new List<DTO.DTOReferencia>();
            
            //Para pruebas ///////////////////
            /*
            int miReferencia = 1;
            string refer = "";
            */
            //Para pruebas ///////////////////
            Lineas.ForEach(linea => {

                //Para pruebas ///////////////////
                
                /*
                int digitoVerificador = Utilities.DigitoVerificador.Obtener(miReferencia);

                refer = string.Concat(Enumerable.Repeat("0", 10 - (miReferencia.ToString() + "" + digitoVerificador).Length)) +
                                       (miReferencia.ToString() + "" + digitoVerificador);
                miReferencia++;
                */
                //Para pruebas ///////////////////

                Referencias.Add(new DTO.DTOReferencia
                {
                    //tipo = linea.Substring(Lay.Tipo.inicial, Lay.Tipo.final),
                    //moneda = linea.Substring(Lay.Moneda.inicial, Lay.Moneda.final),
                    //plaza = linea.Substring(Lay.Plaza.inicial, Lay.Plaza.final),
                    //cuenta = linea.Substring(Lay.Cuenta.inicial, Lay.Cuenta.final),
                    fechaPago = DateTime.Parse(linea.Substring(Lay.FechaPago.inicial, Lay.FechaPago.final)),
                    referenciaId = linea.Substring(Lay.ReferenciaId.inicial, Lay.ReferenciaId.final),

                    //Para pruebas ///////////////////
                    //referenciaId = refer,
                    //Para pruebas ///////////////////
                    importe = decimal.Parse(linea.Substring(Lay.Importe.inicial, Lay.Importe.final)),
                    movimiento = linea.Substring(Lay.Movimiento.inicial, Lay.Movimiento.final),
                    //saldo = decimal.Parse(linea.Substring(Lay.Saldo.inicial, Lay.Saldo.final)),
                    //transaccion = linea.Substring(Lay.Transaccion.inicial, Lay.Transaccion.final),
                    //leyenda = linea.Substring(Lay.Leyenda.inicial, Lay.Leyenda.final),
                    //leyenda2 = linea.Substring(Lay.Leyenda2.inicial, Lay.Leyenda2.final),
                    consecutivo = int.Parse(linea.Substring(Lay.Consecutivo.inicial, Lay.Consecutivo.final))
                });
            });

            //Aplicacion(Referencias);
        }

        public static DAL.Importe VerificaImporte(Pago Pago, DTO.DTOReferencia Referencia)
        {
            return Referencia.importe > Pago.Promesa ? DAL.Importe.Mayor : 
                (Referencia.importe < Pago.Promesa ? DAL.Importe.Menor : DAL.Importe.Igual);
        }



        //public static void Aplicacion(List<DTO.DTOReferencia> Referencias)
        //{

        //    int estatusId = 0;
        //    decimal importeAplicado = 0, importeRestante = 0;
        //    bool esIgual = false;

        //    using (DAL.UniversidadEntities db = new DAL.UniversidadEntities())
        //    {
        //        Referencias.ForEach(referencia =>
        //        {
        //            //Si es un movimiento abono
        //            if (referencia.movimiento == "ABONO")
        //            {
        //                //Si existe la referencia
        //                var pagoBD = db.Pago
        //                    .Where(pago => pago.ReferenciaId == referencia.referenciaId)
        //                    .ToList()
        //                    .FirstOrDefault();

        //                //Si la referencia existe en BD
        //                if (pagoBD != null)
        //                {
        //                    //Si aun no ha sido aplicada
        //                    if (pagoBD.EstatusId == 1)
        //                    {
        //                        var Validacion = VerificaImporte(pagoBD, referencia);
        //                        //Si los importes coinciden
        //                        if (Validacion == DAL.Importe.Igual)
        //                        {
        //                            pagoBD.Pago1 = referencia.importe;
        //                            pagoBD.FechaPago = referencia.fechaPago;
        //                            pagoBD.HoraPago = referencia.fechaPago.TimeOfDay;
        //                            pagoBD.EstatusId = 4;
        //                            pagoBD.EsReferencia = true;
        //                            importeAplicado = referencia.importe;
        //                            estatusId = 4;
        //                            esIgual = true;
        //                        }

        //                        else if (Validacion == DAL.Importe.Mayor)
        //                        {
        //                            pagoBD.Pago1 = pagoBD.Promesa;
        //                            pagoBD.FechaPago = referencia.fechaPago;
        //                            pagoBD.HoraPago = referencia.fechaPago.TimeOfDay;
        //                            pagoBD.EstatusId = 4;
        //                            pagoBD.EsReferencia = true;
        //                            estatusId = 4;
        //                            importeAplicado = pagoBD.Promesa;
        //                            importeRestante = referencia.importe - pagoBD.Promesa;
        //                        }

        //                        else if (Validacion == DAL.Importe.Menor)
        //                        {
        //                            estatusId = 12;
        //                            importeRestante = referencia.importe;
        //                            importeAplicado = 0;
        //                        }

        //                        if (Validacion == DAL.Importe.Igual | Validacion == DAL.Importe.Mayor)
        //                        {

        //                            List<DTO.DTOPagoMetodo> Metodos = (from a in db.PagoMetodo.AsNoTracking()
        //                                                               where a.Descripcion.Contains("Referenciado Scotiabank")
        //                                                               select new DTO.DTOPagoMetodo
        //                                                               {
        //                                                                   pagoMetodoId = a.PagoMetodoId,
        //                                                                   descripcion = a.Descripcion,
        //                                                                   cuentaContable = a.CuentaContable,
        //                                                                   importe = importeAplicado
        //                                                               }).ToList();


        //                            //var a = new DTO.DTOPago { };
        //                            string adw = "" + (int)DateTime.Now.DayOfWeek;

        //                            BLLPago.Aplicar(ref Resultado,
        //                                            new DTO.DTOAlumnoDatosGenerales
        //                                            {
        //                                                alumnoId = pagoBD.AlumnoId,
        //                                                ofertaEducativaId = pagoBD.OfertaEducativaId
        //                                            },
        //                                            Metodos,
        //                                            new List<DTO.DTOPago> { new DTO.DTOPago { pagoId = pagoBD.PagoId,
        //                                                importe = Validacion == DAL.Importe.Igual ? referencia.importe : referencia.importe - pagoBD.Promesa,
        //                                                conceptoPagoId = pagoBD.Cuota1.PagoConceptoId
        //                                            } },
        //                                            new DTO.DTOLogin {usuarioId = 0,
        //                                            sucursalCajaId = 1},
        //                                            ""
        //                            );
        //                        }
        //                    }

        //                    //Si ya fue aplicada
        //                    else
        //                    {
        //                        estatusId = 9;
        //                        importeRestante = referencia.importe;
        //                        importeAplicado = 0;
        //                    }
        //                }

        //                //Si no existe marcarla como inexistente
        //                else
        //                {
        //                    estatusId = 10;
        //                    importeRestante = referencia.importe;
        //                    importeAplicado = 0;
        //                }

        //                db.ReferenciadoBitacora.Add(new DAL.ReferenciadoBitacora
        //                {
        //                    ReferenciaId = referencia.referenciaId,
        //                    //Tipo = referencia.tipo,
        //                    //Moneda = referencia.moneda,
        //                    //Plaza = referencia.plaza,
        //                    FechaPago = referencia.fechaPago,
        //                    Importe = referencia.importe,
        //                    Movimiento = referencia.movimiento,
        //                    //Saldo = referencia.saldo,
        //                    //Transaccion = referencia.transaccion,
        //                    //Leyenda = referencia.leyenda,
        //                    //Leyenda2 = referencia.leyenda2,
        //                    Consecutivo = referencia.consecutivo,
        //                    FechaProcesado = DateTime.Now,
        //                    HoraProcesado = DateTime.Now.TimeOfDay,
        //                    EstatusId = estatusId,
        //                    ImporteAplicado = importeAplicado

        //                });

        //                //Si sobra o falta la metemos a pendiente
        //                if (!esIgual)
        //                {
        //                    db.ReferenciadoPendiente.Add(new DAL.ReferenciadoPendiente
        //                    {
        //                        ReferenciaId = referencia.referenciaId,
        //                        //Tipo = referencia.tipo,
        //                        //Moneda = referencia.moneda,
        //                        //Plaza = referencia.plaza,
        //                        FechaPago = referencia.fechaPago,
        //                        Importe = referencia.importe,
        //                        Movimiento = referencia.movimiento,
        //                        //Saldo = referencia.saldo,
        //                        //Transaccion = referencia.transaccion,
        //                        //Leyenda = referencia.leyenda,
        //                        //Leyenda2 = referencia.leyenda2,
        //                        Consecutivo = referencia.consecutivo,
        //                        FechaProcesado = DateTime.Now,
        //                        HoraProcesado = DateTime.Now.TimeOfDay,
        //                        EstatusId = estatusId,
        //                        ImporteRestante = importeRestante
        //                    });
        //                }

        //                esIgual = false;
        //            }
        //        });

        //        db.SaveChanges();
        //    }
        //}      
    }
}
