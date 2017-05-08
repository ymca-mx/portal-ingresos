using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Globalization;

namespace Pruebas
{
    [TestClass]
    public class Alumno
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");

        [TestMethod]
        public void BuscarAlumnoBeca()
        {
            var obj=
            BLL.BLLAlumnoPortal.BuscarAlumno(8182, 3);

            Console.WriteLine(obj.AlumnoId + " " + obj.Nombre + " ");
        }

        [TestMethod]
        public void DescuentosBeca()
        {
            BLL.BLLDescuentos.TraerDescuentos(5669, 29);
        }

        [TestMethod]
        public void PruebaDatosAlumo()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var AlumnoId = 5669;
                

                var datos = db.Alumno.Where(a => a.AlumnoId == AlumnoId)
                                    .Select(b => new DTOAlumnoDatos
                                    {
                                        AlumnoId = b.AlumnoId,
                                        Nombre = b.Nombre,
                                        Paterno = b.Paterno,
                                        Materno = b.Materno,
                                        FechaNacimiento = b.AlumnoDetalle.FechaNacimiento,
                                        GeneroId = b.AlumnoDetalle.GeneroId,
                                        CURP = b.AlumnoDetalle.CURP,
                                        PaisId = b.AlumnoDetalle.PaisId,
                                        EntidadNacimientoId = b.AlumnoDetalle.EntidadNacimientoId,
                                    }).FirstOrDefault();

                datos.FechaNacimientoC = datos.FechaNacimiento.ToString("dd/MM/yyyy", Cultura);

                datos.DatosContacto = new List<DTOAlumnoDatos2>();

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Estado Civil",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EstadoCivilId.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EstadoCivilId.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EstadoCivilId.ToString() ?? "",
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Correo Electrónico",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Email.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Email.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Email.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Teléfono Celular",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Celular.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Celular.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Celular.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Teléfono Casa",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.TelefonoCasa.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.TelefonoCasa.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.TelefonoCasa.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Calle",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Calle.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Calle.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Calle.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Número Exterior",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoExterior.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoExterior.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoExterior.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Numero Interior",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoInterior.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoInterior.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoInterior.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Código Postal",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.CP.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.CP.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.CP.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Colonia",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Colonia.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Colonia.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Colonia.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Estado",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EntidadFederativaId.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EntidadFederativaId.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EntidadFederativaId.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Delegación | Municipio",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.MunicipioId.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.MunicipioId.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.MunicipioId.ToString() ?? ""
                }
                );

            }
        }

        [TestMethod]
        public void CambioCarrera()
        {
            int AlumnoId = 8178;
            int OfertaAnterior = 29;
            int OfertaNueva = 3;
            int Anio = 2017;
            int PeriodoId = 3;
            string  mensaje;
            int UsuarioId = 8224;
            string Observaciones = "";

            using (UniversidadEntities db = new UniversidadEntities())
            {
                var AlumnoInscrito = db.AlumnoInscrito.Where(a => a.AlumnoId == AlumnoId 
                                                        && a.OfertaEducativaId == OfertaAnterior 
                                                        && a.EstatusId == 1
                                                        && a.Anio == Anio
                                                        && a.PeriodoId == PeriodoId)?.FirstOrDefault();
                //verificar si el alumno esta inscrito
                if (AlumnoInscrito != null)
                {
                    db.AlumnoInscrito.Add(new AlumnoInscrito
                    {
                        AlumnoId = AlumnoInscrito.AlumnoId,
                        OfertaEducativaId = OfertaNueva,
                        Anio = AlumnoInscrito.Anio,
                        PeriodoId = AlumnoInscrito.PeriodoId,
                        FechaInscripcion = AlumnoInscrito.FechaInscripcion,
                        HoraInscripcion = AlumnoInscrito.HoraInscripcion,
                        PagoPlanId = AlumnoInscrito.PagoPlanId,
                        TurnoId = AlumnoInscrito.TurnoId,
                        EsEmpresa = AlumnoInscrito.EsEmpresa,
                        UsuarioId = AlumnoInscrito.UsuarioId,
                        EstatusId = AlumnoInscrito.EstatusId
                    });
            
                    db.AlumnoInscrito.Remove(AlumnoInscrito);

                var AlumnoDescuento = db.AlumnoDescuento.Where(a => a.AlumnoId == AlumnoId
                                                        && a.OfertaEducativaId == OfertaAnterior
                                                        && a.EstatusId !=3
                                                        && a.Anio == Anio
                                                        && a.PeriodoId == PeriodoId
                                                        )?.ToList();

                    AlumnoDescuento.ForEach(a=>
                    {
                        a.OfertaEducativaId = OfertaNueva;
                        a.DescuentoId = db.Descuento.Where(m => m.PagoConceptoId == a.PagoConceptoId && m.OfertaEducativaId == OfertaNueva).FirstOrDefault().DescuentoId;
                    });

                var Pago = db.Pago.Where(a => a.AlumnoId == AlumnoId
                                                        && a.OfertaEducativaId == OfertaAnterior
                                                        && a.EstatusId != 2
                                                        && a.Anio == Anio
                                                        && a.PeriodoId == PeriodoId
                                                        )?.ToList();
                    Pago.ForEach(a=> 
                    {
                        a.OfertaEducativaId = OfertaNueva;
                        a.CuotaId = db.Cuota.Where(w => w.PagoConceptoId == a.Cuota1.PagoConceptoId && w.OfertaEducativaId == OfertaNueva).FirstOrDefault().CuotaId;
                    });
                    var pago2 = Pago.Select(i => i.PagoId).ToList();
                    var PagoDescuento = db.PagoDescuento.Where(q => pago2.Contains(q.PagoId)).ToList();

                    PagoDescuento.ForEach(a=>
                    {
                        db.PagoDescuento.Add(new PagoDescuento
                        {
                            PagoId = a.PagoId,
                            DescuentoId = db.Descuento.Where(e => e.PagoConceptoId == a.Pago.Cuota1.PagoConceptoId && e.OfertaEducativaId == OfertaNueva).FirstOrDefault().DescuentoId,
                            Monto = a.Monto
                        });
                    });

                    db.PagoDescuento.RemoveRange(PagoDescuento);

                    var alumno = db.Alumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault();

                    #region Bitacora Alumno
                    db.AlumnoBitacora.Add(new AlumnoBitacora
                    {
                        AlumnoId = alumno.AlumnoId,
                        Anio = alumno.Anio,
                        EstatusId = alumno.EstatusId,
                        Fecha = DateTime.Now,
                        FechaRegistro = alumno.FechaRegistro,
                        Materno = alumno.Materno,
                        MatriculaId = alumno.MatriculaId,
                        Nombre = alumno.Nombre,
                        Paterno = alumno.Paterno,
                        PeriodoId = alumno.PeriodoId,
                        UsuarioId = alumno.UsuarioId,
                        UsuarioIdBitacora = UsuarioId

                    });
                    #endregion

                    string NMatricula = Herramientas.Matricula.ObtenerMatricula(new DTOAlumnoInscrito
                    {
                        Anio = Anio,
                        PeriodoId = PeriodoId,
                        TurnoId = AlumnoInscrito.TurnoId

                    },
                       new DTOOfertaEducativa
                       {
                           OfertaEducativaId = OfertaNueva,
                           Rvoe = db.OfertaEducativa.Where(l => l.OfertaEducativaId == OfertaNueva).FirstOrDefault().Rvoe,
                       }, AlumnoId);

                    if (alumno.MatriculaId != NMatricula)
                    {
                        #region Update Alumno

                        alumno.MatriculaId = NMatricula;
                        alumno.Anio = Anio;
                        alumno.PeriodoId = PeriodoId;
                        alumno.EstatusId = 1;
                        alumno.FechaRegistro = DateTime.Now;
                        alumno.UsuarioId = UsuarioId;

                        #endregion

                        #region Bitacora Matricula
                        db.Matricula.Add(new Matricula
                        {
                            AlumnoId = alumno.AlumnoId,
                            Anio = alumno.Anio,
                            FechaAsignacion = alumno.FechaRegistro,
                            MatriculaId = alumno.MatriculaId,
                            OfertaEducativaId = OfertaNueva,
                            PeriodoId = alumno.PeriodoId,
                            UsuarioId = alumno.UsuarioId
                        });
                        #endregion
                    }

                    db.AlumnoMovimiento.Add(new AlumnoMovimiento
                    {
                        AlumnoId = AlumnoId,
                        OfertaEducativaId = OfertaAnterior,
                        TipoMovimientoId = 4,
                        Fecha = DateTime.Now,
                        Hora = DateTime.Now.TimeOfDay,
                        UsuarioId = UsuarioId,
                        EstatusId = 1,
                        AlumnoMovimientoCarrera = new AlumnoMovimientoCarrera
                        {
                            OfertaEducativaId = OfertaNueva,
                            Observaciones = Observaciones
                        }

                });
                    db.SaveChanges();
                }
                else { mensaje = "El alumno no está inscrito para el periodo seleccionado."; }


            }
        }

        [TestMethod]
        public void GenerarMatrucula()
        {
            Console.WriteLine("LA matricula del alumno 7584 es : ------- ");
            Console.Write(
            Herramientas.Matricula.ObtenerMatricula(new DTOAlumnoInscrito
            {
                Anio = 2016,
                PeriodoId = 1,
                TurnoId = 1
            },
            new DTOOfertaEducativa
            {
                Rvoe = "20100446"
            },
            7584));
        }
    }
}
