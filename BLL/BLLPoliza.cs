using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universidad.DAL;

namespace Universidad.BLL
{
    public class BLLPoliza
    {
        Utilities.ProcessResult Resultado = new Utilities.ProcessResult();

        public static string ArmaCabecero(DTO.DTOCabeceroLayout Layout, DateTime Fecha, int polizaTipoId, int folioId, string concepto)
        {
            string cabecero = "";

            cabecero += Layout.Poliza.valorDefault + string.Concat(Enumerable.Repeat(" ", Layout.Poliza.longitud - Layout.Poliza.valorDefault.Length));
            cabecero += Layout.Poliza.tieneEspacio ? " " : "";

            cabecero += Utilities.Fecha.FechaPoliza(Fecha);
            cabecero += Layout.Fecha.tieneEspacio ? " " : "";

            cabecero += polizaTipoId.ToString() + string.Concat(Enumerable.Repeat(" ", Layout.TipoPoliza.longitud - polizaTipoId.ToString().Length));
            cabecero += Layout.TipoPoliza.tieneEspacio ? " " : "";

            cabecero += string.Concat(Enumerable.Repeat(" ", Layout.Folio.longitud - folioId.ToString().Length)) + folioId.ToString();
            cabecero += Layout.Folio.tieneEspacio ? " " : "";

            cabecero += Layout.Clase.valorDefault + string.Concat(Enumerable.Repeat(" ", Layout.Clase.longitud - Layout.Clase.valorDefault.Length));
            cabecero += Layout.Clase.tieneEspacio ? " " : "";

            cabecero += Layout.DiarioId.valorDefault + string.Concat(Enumerable.Repeat(" ", Layout.DiarioId.longitud - Layout.DiarioId.valorDefault.Length));
            cabecero += Layout.DiarioId.tieneEspacio ? " " : "";

            cabecero += concepto + string.Concat(Enumerable.Repeat(" ", Layout.Concepto.longitud - concepto.Length));
            cabecero += Layout.DiarioId.tieneEspacio ? " " : "";

            cabecero += Layout.SistOrig.valorDefault + string.Concat(Enumerable.Repeat(" ", Layout.SistOrig.longitud - Layout.SistOrig.valorDefault.Length));
            cabecero += Layout.SistOrig.tieneEspacio ? " " : "";

            cabecero += Layout.Impresa.valorDefault + string.Concat(Enumerable.Repeat(" ", Layout.Impresa.longitud - Layout.Impresa.valorDefault.Length));
            cabecero += Layout.Impresa.tieneEspacio ? " " : "";

            return cabecero;
        }

        public static List<string> CreaDetalle(List<DTO.DTORegistroPoliza> Movimientos, DTO.DTODetalleLayout Layout, string tipoMovimiento, string concepto)
        {
            List<string> Filas = new List<string>();

            Movimientos.ForEach(movimiento =>
            {
                string detalle = "";
                
                detalle += Layout.Movimiento.valorDefault + string.Concat(Enumerable.Repeat(" ", Layout.Movimiento.longitud - Layout.Movimiento.valorDefault.Length));
                detalle += Layout.Movimiento.tieneEspacio ? " " : "";

                detalle += movimiento.cuentaContable + string.Concat(Enumerable.Repeat(" ", Layout.CuentaId.longitud - movimiento.cuentaContable.Length));
                detalle += Layout.CuentaId.tieneEspacio ? " " : "";

                detalle += string.Concat(Enumerable.Repeat(" ", Layout.Referencia.longitud));
                detalle += Layout.Referencia.tieneEspacio ? " " : "";

                detalle += tipoMovimiento + string.Concat(Enumerable.Repeat(" ", Layout.TipoMovimiento.longitud - tipoMovimiento.Length));
                detalle += Layout.TipoMovimiento.tieneEspacio ? " " : "";

                detalle += string.Concat(Enumerable.Repeat(" ", Layout.Importe.longitud - movimiento.importe.ToString().Length)) + movimiento.importe;
                detalle += Layout.Importe.tieneEspacio ? " " : "";

                detalle += string.Concat(Enumerable.Repeat(" ", Layout.DiarioId.longitud - Layout.DiarioId.valorDefault.Length)) + Layout.DiarioId.valorDefault;
                detalle += Layout.DiarioId.tieneEspacio ? " " : "";

                detalle += string.Concat(Enumerable.Repeat(" ", Layout.ImporteME.longitud - Layout.ImporteME.valorDefault.Length)) + Layout.ImporteME.valorDefault;
                detalle += Layout.ImporteME.tieneEspacio ? " " : "";

                detalle += concepto + string.Concat(Enumerable.Repeat(" ", Layout.Concepto.longitud - concepto.Length));
                detalle += Layout.Concepto.tieneEspacio ? " " : "";
                
                Filas.Add(detalle);
            });

            return Filas;
        }

        public static List<string> ArmaDetalle(DTO.DTODetalleLayout Layout, DateTime Fecha, string concepto)
        {
            List<string> Filas = new List<string>();

            //Filas.AddRange(CreaDetalle(PolizaCargos(Fecha), Layout, "2", concepto));
            //Filas.AddRange(CreaDetalle(PolizaAbonos(Fecha), Layout, "1", concepto));
            //Filas.AddRange(CreaDetalle(PolizaDescuentos(Fecha), Layout, "1", concepto));

            


            return Filas;
        }

        //public static List<DTO.DTORegistroPoliza> PolizaAbonos(DateTime Fecha)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        return (from consulta in
        //                    (from a in db.Pago
        //                     join b in db.PagoDetalle on a.PagoId equals b.PagoId
        //                     join c in db.PagoMetodo on b.PagoMetodoId equals c.PagoMetodoId
        //                     where a.FechaPago == Fecha.Date
        //                     && a.EstatusId == 4
        //                     select new { a, b, c })
        //                group consulta by new
        //                {
        //                    consulta.c.CuentaContable
        //                } into g

        //                select new DTO.DTORegistroPoliza
        //                {
        //                    cuentaContable = g.Key.CuentaContable,
        //                    importe = g.Sum(cuenta => cuenta.b.Monto)
        //                }).ToList();
        //    }
        //}

        //public static List<DTO.DTORegistroPoliza> PolizaDescuentos(DateTime Fecha)
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        return (from consulta in
        //                    (from a in db.PagoDescuento
        //                     join b in db.Pago on a.PagoId equals b.PagoId
        //                     join c in db.Descuento on a.DescuentoId equals c.DescuentoId
        //                     where b.EstatusId == 4
        //                     && b.FechaPago == Fecha.Date
        //                     select new { a, b, c })
        //                group consulta by new
        //                {
        //                    consulta.c.CuentaContable
        //                } into g

        //                select new DTO.DTORegistroPoliza
        //                {
        //                    cuentaContable = g.Key.CuentaContable.Trim(),
        //                    importe = g.Sum(cuenta => cuenta.a.Monto)
        //                }).ToList();
        //    }
        //}
        //public static List<DTO.DTORegistroPoliza> PolizaCargos(DateTime Fecha)
        //{
        //   //Cargos a cuentas de carreras
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        int idComisionTC = BLLSistemaConfiguracion.ComisionTC();

        //        return (from consulta in
        //                    (from a in db.Pago
        //                     join b in db.Cuota on a.CuotaId equals b.CuotaId
        //                     join c in db.PagoConcepto on new { b.PagoConceptoId, b.OfertaEducativaId } equals new { c.PagoConceptoId, c.OfertaEducativaId }
        //                     where a.EstatusId == 4
        //                     && a.FechaPago == Fecha.Date
        //                     && !a.EsReferencia
        //                     select new { a, b, c })
        //                group consulta by new
        //                {
        //                    consulta.c.CuentaContable
        //                } into g
        //                select new DTO.DTORegistroPoliza
        //                {
        //                    cuentaContable = g.Key.CuentaContable,
        //                    importe = g.Sum(cuenta => 
        //                            //cuenta.a.Cuota == 0 || 
        //                            //    (
        //                            //        ( cuenta.a.Promesa < cuenta.a.Cuota && (db.PagoDescuento.Count(pago => pago.PagoId == cuenta.a.PagoId) == 0)) ||
        //                            //        (cuenta.a.Promesa > cuenta.a.Cuota && (db.PagoDescuento.Count(pago => pago.PagoId == cuenta.a.PagoId) == 0))
        //                            //    ) ? cuenta.a.Promesa : cuenta.a.Cuota
        //                            cuenta.a.PagoId <= 2588 || cuenta.a.Cuota == 0 ? cuenta.a.Promesa : cuenta.a.Cuota
        //                            )
        //                    //importe = g.Sum(cuenta => cuenta.c.PagoConceptoId == idComisionTC ? cuenta.a.Promesa : cuenta.a.Cuota)
        //                }).ToList();
        //    }
        //}

        public static int Folio(DTO.Poliza.Tipo.Tipo Tipo, DTO.Poliza.Tipo.Subtipo Subtipo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var Consecutivo = db.PolizaConsecutivo.Where(p => p.AsociacionId == 6
                                    && p.PolizaTipoId == (int)Tipo
                                    && p.PolizaSubtipoId == (int)Subtipo
                                    && p.Anio == DateTime.Now.Year
                                    && p.MesId == DateTime.Now.Month).FirstOrDefault();

                if (Consecutivo == null)
                    return (db.PolizaNumeracion.Where(p => p.AsociacionId == 6
                                    && p.PolizaTipoId == (int)Tipo
                                    && p.PolizaSubtipoId == (int)Subtipo).FirstOrDefault().FolioInicial);
                else
                    return Consecutivo.Consecutivo + 1;
            }
        }

        public static void ProcesaPoliza(DateTime Fecha, DTO.Poliza.Tipo.Tipo Tipo, DTO.Poliza.Tipo.Subtipo Subtipo)
        {
            DTO.DTOCabeceroLayout LayoutCabecero = CabeceroLayout((int)Tipo);
            DTO.DTODetalleLayout LayoutDetalle = DetalleLayout((int)Tipo);
            string concepto = Subtipo == DTO.Poliza.Tipo.Subtipo.Caja ? "Poliza Caja " + Fecha.ToShortDateString() : "Poliza Referenciados " + Fecha.ToShortDateString();

            int folio = Folio(Tipo, Subtipo);

            List<string> Poliza = new List<string>();
            Poliza.Add(ArmaCabecero(LayoutCabecero, Fecha, (int)Tipo, folio, concepto));

            List<string> Detalle = ArmaDetalle(LayoutDetalle, Fecha, concepto);

            if (Detalle.Count == 0)
                return;

            Detalle.ForEach(detalle => {
                Poliza.Add(detalle);
            });

            string[] nombrePoliza = NombrePoliza(Fecha, Tipo, Subtipo, folio);
            //Archivo

            Console.WriteLine("Se va a crear..");
            Utilities.Archivo.CrearTXT(nombrePoliza[0] + nombrePoliza[1], Poliza);
            Console.WriteLine("Se creo...");

            Console.ReadLine();

            //Email
            var Email = BLLVarios.CredencialesDeEnvio();
            Utilities.ProcessResult resultado = new Utilities.ProcessResult();
            Utilities.Email.Enviar(Email.email, Email.password, Email.displayName, EmailDestinatarios(Tipo, Subtipo), ';', "Poliza de ingresos por caja: " + Fecha.ToShortDateString(), Email.body, nombrePoliza[0] + nombrePoliza[1], ';', Email.smtp, Email.puerto, Email.ssl, true, ref resultado);
        }

        public static string EmailDestinatarios(DTO.Poliza.Tipo.Tipo Tipo, DTO.Poliza.Tipo.Subtipo Subtipo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.PolizaNumeracion.AsNoTracking()
                        where a.PolizaTipoId == (int)Tipo
                            && a.PolizaSubtipoId == (int)Subtipo
                        select a.Email).FirstOrDefault();
            }
        }

        public static string[] NombrePoliza(DateTime Fecha, DTO.Poliza.Tipo.Tipo Tipo, DTO.Poliza.Tipo.Subtipo Subtipo, int folio)
        {
            string[] archivoPoliza = new string[2];

            using (UniversidadEntities db = new UniversidadEntities())
            {
                string archivo = (from a in db.Asociacion.AsNoTracking()
                                      where a.AsociacionId == 6
                                      select a.Abreviacion).FirstOrDefault();

                archivo += "_" + Fecha.Year + " " +
                    (Fecha.Month < 10 ? "0" + Fecha.Month : Fecha.Month.ToString()) +
                    (Fecha.Day < 10 ? "0" + Fecha.Day : Fecha.Day.ToString());

                archivo += "_" + string.Concat(Enumerable.Repeat("0", 2 - ((int)Tipo).ToString().Length)) + ((int)Tipo) + "_";

                var Consecutivo = db.PolizaConsecutivo.Where(p => p.AsociacionId == 6
                                && p.PolizaTipoId == (int)Tipo
                                && p.PolizaSubtipoId == (int)Subtipo
                                && p.Anio == DateTime.Now.Year
                                && p.MesId == DateTime.Now.Month).FirstOrDefault();

                if (Consecutivo == null)
                {
                    db.PolizaConsecutivo.Add(new PolizaConsecutivo
                    {
                        AsociacionId = 6,
                        PolizaTipoId = (int)Tipo,
                        PolizaSubtipoId = (int)Subtipo,
                        Anio = DateTime.Now.Year,
                        MesId = DateTime.Now.Month,
                        Consecutivo = folio
                    });
                }

                else
                    Consecutivo.Consecutivo = folio;

                archivo += string.Concat(Enumerable.Repeat("0", 2 - (folio).ToString().Length)) + (folio).ToString();

                string ruta = (from a in db.PolizaNumeracion
                               join b in db.Asociacion on a.AsociacionId equals b.AsociacionId
                               select  a.Ruta + "\\").FirstOrDefault();
                
                db.SaveChanges();

                archivoPoliza[0] = ruta;
                archivoPoliza[1] = archivo + ".txt";

                return archivoPoliza;
            }
        }

        public static DTO.DTODetalleLayout DetalleLayout(int polizaTipoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTO.DTODetalleAlmacen> Almacen = (from a in db.PolizaDetalleLayout
                                                       where a.PolizaTipoId == polizaTipoId
                                                       select new DTO.DTODetalleAlmacen { 
                                                           descripcion = a.Descripcion,
                                                           valorDefault = a.ValorDefault,
                                                           longitud = a.Longitud,
                                                           tieneEspacio = a.TieneEspacio
                                                       }).ToList();

                return (new DTO.DTODetalleLayout
                {
                    Movimiento = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("Movimiento")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("Movimiento")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("Movimiento")).FirstOrDefault().tieneEspacio,
                    },
                    CuentaId = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("CuentaId")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("CuentaId")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("CuentaId")).FirstOrDefault().tieneEspacio,
                    },
                    Referencia = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("Referencia")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("Referencia")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("Referencia")).FirstOrDefault().tieneEspacio,
                    },
                    TipoMovimiento = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("TipoMovimiento")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("TipoMovimiento")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("TipoMovimiento")).FirstOrDefault().tieneEspacio,
                    },
                    Importe = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("Importe")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("Importe")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("Importe")).FirstOrDefault().tieneEspacio,
                    },
                    DiarioId = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("DiarioId")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("DiarioId")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("DiarioId")).FirstOrDefault().tieneEspacio,
                    },
                    ImporteME = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("ImporteME")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("ImporteME")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("ImporteME")).FirstOrDefault().tieneEspacio,
                    },
                    Concepto = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("Concepto")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("Concepto")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("Concepto")).FirstOrDefault().tieneEspacio,
                    }
                });
            }
        }

        public static DTO.DTOCabeceroLayout CabeceroLayout(int polizaTipoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTO.DTOCabeceroAlmacen> Almacen = (from a in db.PolizaCabeceroLayout
                                                        where a.PolizaTipoId == polizaTipoId
                                                        select new DTO.DTOCabeceroAlmacen
                                                        {
                                                            descripcion = a.Descripcion,
                                                            valorDefault = a.ValorDefault,
                                                            longitud = a.Longitud,
                                                            tieneEspacio = a.TieneEspacio
                                                        }).ToList();

                return (new DTO.DTOCabeceroLayout
                {
                    Poliza = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("Poliza")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("Poliza")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("Poliza")).FirstOrDefault().tieneEspacio,
                    },
                    Fecha = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("Fecha")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("Fecha")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("Fecha")).FirstOrDefault().tieneEspacio,
                    },
                    TipoPoliza = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("TipoPoliza")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("TipoPoliza")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("TipoPoliza")).FirstOrDefault().tieneEspacio,
                    },
                    Folio = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("Folio")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("Folio")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("Folio")).FirstOrDefault().tieneEspacio,
                    },
                    Clase = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("Clase")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("Clase")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("Clase")).FirstOrDefault().tieneEspacio,
                    },
                    DiarioId = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("DiarioId")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("DiarioId")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("DiarioId")).FirstOrDefault().tieneEspacio,
                    },
                    Concepto = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("Concepto")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("Concepto")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("Concepto")).FirstOrDefault().tieneEspacio,
                    },
                    SistOrig = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("SistOrig")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("SistOrig")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("SistOrig")).FirstOrDefault().tieneEspacio,
                    },
                    Impresa = new DTO.DTOPropiedad
                    {

                        valorDefault = Almacen.Where(conf => conf.descripcion.Equals("Impresa")).FirstOrDefault().valorDefault,
                        longitud = Almacen.Where(conf => conf.descripcion.Equals("Impresa")).FirstOrDefault().longitud,
                        tieneEspacio = Almacen.Where(conf => conf.descripcion.Equals("Impresa")).FirstOrDefault().tieneEspacio,
                    },
                });
            }
        }
    }
}
