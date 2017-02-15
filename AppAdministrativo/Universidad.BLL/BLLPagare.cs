using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universidad.DAL;

namespace Universidad.BLL
{
    public class BLLPagare
    {
        public static List<DTO.Pagare.Visor.DTOPagare> ConsultaVisor()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                return (from a in db.Pagare.AsNoTracking()
                        join b in db.Alumno.AsNoTracking() on a.AlumnoId equals b.AlumnoId
                        join c in db.PagoPagare.AsNoTracking() on a.PagareId equals c.PagareId
                        join d in db.Usuario.AsNoTracking() on a.UsuarioId equals d.UsuarioId
                        join e in db.Estatus on a.EstatusId equals e.EstatusId
                        select new DTO.Pagare.Visor.DTOPagare { 
                            pagareId = a.PagareId,
                            alumno = b.AlumnoId + " | " + (b.Nombre + " " + b.Paterno  + " " + b.Materno).Trim(),
                            FechaGeneracion = a.FechaGeneracion,
                            importe = a.Importe,
                            interes = a.Interes,
                            referenciaId = c.ReferenciaId,
                            FechaVencimiento = a.FechaVencimiento,
                            usuario = d.UsuarioId + " | " + (d.Nombre + " " + d.Paterno + " " + d.Materno).Trim(),
                            estatus = e.Descripcion
                        }).ToList();
            }
        }

        public static List<int> Generar(DTO.DTOAlumnoDatosGenerales DatosAlumno, DTO.DTOLogin Credenciales, List<DTO.DTOPago> Pagos, DTO.DTOPagare DatosPagare)
        {
            List<int> Pagares = new List<int>();
            using (UniversidadEntities db = new UniversidadEntities())
            {
                Pagos.ForEach(pago =>
                {
                    db.Pago.Where(registro => registro.PagoId == pago.pagoId).FirstOrDefault().EstatusId = 5;

                    db.Pagare.Add(new DAL.Pagare
                    {
                        AlumnoId = DatosAlumno.alumnoId,
                        FechaGeneracion = DateTime.Now,
                        FechaVencimiento = DatosPagare.fechaVencimiento,
                        Importe = pago.importe,
                        Interes = DatosPagare.interes,
                        Observacion = DatosPagare.observacion,
                        EstatusId = 1,
                        UsuarioId = Credenciales.usuarioId,
                        PagareDocumento = DatosPagare.documento == null ? null : new DAL.PagareDocumento { PagareDocumento1 = DatosPagare.documento },
                        PagoPagare = new List<DAL.PagoPagare> { 
                            new DAL.PagoPagare{
                                PagoId = pago.pagoId,
                                ReferenciaId = (from a in db.Pago.AsNoTracking()
                                                    where a.PagoId == pago.pagoId
                                                    select a.ReferenciaId).FirstOrDefault()
                            }
                        }
                    });
                });

                db.SaveChanges();

                db.Pagare.Local.ToList().ForEach(s =>
                {
                    Pagares.Add(s.PagareId);
                });

                return Pagares;
            }
        }

        public static List<DTO.Pagare.Impresion.DTOPagare> Impresion(List<int> PagaresId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                return (from a in db.Pagare.AsNoTracking()
                        join b in db.Alumno.AsNoTracking() on a.AlumnoId equals b.AlumnoId
                        join c in db.AlumnoDetalle.AsNoTracking() on a.AlumnoId equals c.AlumnoId
                        join d in db.Municipio.AsNoTracking() on new { c.EntidadFederativaId, c.MunicipioId } equals new { d.EntidadFederativaId, d.MunicipioId }
                        join e in db.PagoPagare.AsNoTracking() on a.PagareId equals e.PagareId
                        join f in db.Pago.AsNoTracking() on e.PagoId equals f.PagoId

                        where PagaresId.Any(p => p.Equals(a.PagareId))
                        select new DTO.Pagare.Impresion.DTOPagare
                        {
                            Documento = new List<DTO.Pagare.Impresion.DTODocumento>{
                                new DTO.Pagare.Impresion.DTODocumento { 
                                    importe = a.Importe.ToString(),
                                    fechaGeneracion = a.FechaGeneracion,
                                    fechaVencimiento = a.FechaVencimiento,
                                    interes = a.Interes.ToString(),
                                    observaciones = a.Observacion,
                                    diaGeneracion = a.FechaGeneracion.Day.ToString(),
                                    mesGeneracion = (from z in db.Mes
                                                         where z.MesId == a.FechaGeneracion.Month
                                                         select z.Descripcion).FirstOrDefault(),
                                    anioGeneracion = a.FechaGeneracion.Year.ToString(),
                                    no = a.PagareId.ToString(),
                                    diaVencimiento = a.FechaVencimiento.Day.ToString(),
                                    mesVencimiento = (from z in db.Mes
                                                          where z.MesId == a.FechaVencimiento.Month
                                                          select z.Descripcion).FirstOrDefault(),
                                    anioVencimiento = a.FechaVencimiento.Year.ToString(),
                                    importeLetra = db.fnImporteLetra(a.Importe.ToString(), "pesos", true),
                                    referenciaId = f.ReferenciaId
                                }
                            },
                            Acreedor = new List<DTO.Pagare.Impresion.DTOAcreedor>{
                            
                            new DTO.Pagare.Impresion.DTOAcreedor { 
                                razonSocial = db.Asociacion.Where(p => p.AsociacionId == 6).FirstOrDefault().Descripcion,
                                lugar = "México D. F."
                                }   
                            },
                            Deudor = new List<DTO.Pagare.Impresion.DTODeudor>{
                                new DTO.Pagare.Impresion.DTODeudor { 
                                nombre = (b.Nombre + " " + b.Paterno + " " + b.Materno).Trim(),
                                calle = c.Calle,
                                noExterior = c.NoExterior,
                                colonia = c.Colonia,
                                delegacion = d.Descripcion,
                                telefono = c.TelefonoCasa
                            }
                        },
                            Banco = new List<DTO.Pagare.Impresion.DTOBanco> { 
                                (from z in db.Asociacion
                                     join y in db.AsociacionDetalle on z.AsociacionId equals y.AsociacionId
                                         where z.AsociacionId == 6
                                     select new DTO.Pagare.Impresion.DTOBanco{
                                         banco = y.Banco,
                                         cuenta = y.BancoCuenta,
                                         moneda = y.Moneda,
                                         cliente = z.Descripcion
                                     }).FirstOrDefault()
                            }
                        }).ToList();

                //db.Configuration.LazyLoadingEnabled = true;
            }
        }
    }
}
