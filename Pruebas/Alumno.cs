using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using BLL;
using System.Globalization;

namespace Pruebas
{
    [TestClass]
    public class Alumno
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");

        [TestMethod]
        public void ActualizarOfertaMatricula()
        {
            DTOAlumnoInscrito objInscribir = new DTOAlumnoInscrito
            {
                AlumnoId = 8299,
                PeriodoId = 3,
                Anio = 2017,
                OfertaEducativaId = 24,
                PagoPlanId = 4,
                EsEmpresa = false,
                TurnoId = 3,
                UsuarioId = 100000
            };


            BLLAlumnoInscrito.ActializarAlumnoInscrito(objInscribir, 2018, 1);

        }

        [TestMethod]
        public void GetMatricula()
        {
            Console.WriteLine(Herramientas.Matricula
                                            .GenerarMatricula(2018, 3, 8838, null, 4));
        }

        [TestMethod]
        public void BuscarAlumnoBeca()
        {
            var obj =
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
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EstadoCivilId.ToString() ?? "",
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Correo Electrónico",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Email.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Teléfono Celular",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Celular.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Teléfono Casa",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.TelefonoCasa.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Calle",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Calle.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Número Exterior",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoExterior.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Numero Interior",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoInterior.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Código Postal",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.CP.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Colonia",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Colonia.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Estado",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EntidadFederativaId.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Delegación | Municipio",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.MunicipioId.ToString() ?? ""
                }
                );

            }
        }

        [TestMethod]
        public void CambioCarrera()
        {
            DTOAlumnoCambioCarrera Cambio = new DTOAlumnoCambioCarrera
            {
                AlumnoId = 7876,
                OfertaEducativaIdActual = 2,
                OfertaEducativaIdNueva = 4,
                Anio = 2017,
                PeriodoId = 3,
                Observaciones = "Cuando se generó referencia de cambio de carrera aún no se liberaba el modulo.",
                UsuarioId = 8289
            };


            BLLAlumnoPortal.AplicarCambioCarrera(Cambio);


        }

        [TestMethod]
        public void GenerarMatricula()
        {
            Console.WriteLine("LA matricula del alumno 7940 es : ------- ");
            Console.Write(
            Herramientas.Matricula.ObtenerMatricula(new DTOAlumnoInscrito
            {
                Anio = 2017,
                PeriodoId = 1,
                TurnoId = 1
            },
            new DTOOfertaEducativa
            {
                Rvoe = "20081067"
            },
            7940));
        }

        [TestMethod]
        public void MatriculaMasiva()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DAL.Alumno> lstAlum = db.Alumno.
                                        Where(a => a.MatriculaId.Contains("0000"))
                                        //Where(a=> a.AlumnoId== 732)
                                        .ToList();

                List<string> MatriculasN = new List<string>();
                lstAlum.ForEach(k =>
                {
                    var ali = k.AlumnoInscrito
                                        .Where(o => o.OfertaEducativa.OfertaEducativaTipoId != 4 && o.OfertaEducativaId != 43)
                                        .ToList()
                                        .OrderBy(i => i.Anio)
                                            .ThenBy(i => i.PeriodoId)
                                            .ThenBy(i => i.FechaInscripcion)
                                            .ThenBy(i => i.HoraInscripcion)
                                        .ToList();
                    if (ali.Count > 0)
                    {
                        var ult = ali.LastOrDefault();
                        string MAtric =
                        Herramientas.Matricula.ObtenerMatricula(new DTOAlumnoInscrito
                        {
                            Anio = k.Anio,
                            PeriodoId = k.PeriodoId,
                            TurnoId = ult.TurnoId
                        },
                       new DTOOfertaEducativa
                       {
                           Rvoe = ult.OfertaEducativa.Rvoe
                       },
                       k.AlumnoId);

                        MatriculasN.Add("AlumnoId: " + k.AlumnoId + "- Matricula: " + MAtric);
                        k.MatriculaId = MAtric;
                    }
                });

                //MatriculasN.ForEach(l => Console.WriteLine(l));
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void BuscarFiltro()
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {
                string Cadena = "Lopez GARCÍA JULIO";
                string[] varios = Cadena.Split(' ');
                varios = varios.Where(c => c != " ")
               .ToArray();

                List<DAL.Alumno> alumnos = (from a in db.Alumno
                                            where varios.Contains(a.Nombre) && varios.Contains(a.Paterno) && varios.Contains(a.Materno)
                                            select a)
                                              .ToList();

            }
        }

        [TestMethod]
        public void CambiarPerido()
        {
            var AlumnoB = new
            {
                AlumnoId = 8727,
                OfertaEducativaId = 29,
                AnioA = 2018,
                PeriodoA = 2,
                AnioC = 2018,
                PeriodoC = 3
            };
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DAL.Alumno alumno = db.Alumno
                                        .Where(a =>
                                            a.AlumnoId == AlumnoB.AlumnoId
                                            && a.Anio == AlumnoB.AnioC
                                            && a.PeriodoId == AlumnoB.PeriodoC)
                                            .FirstOrDefault();

                    if (alumno.AlumnoInscrito.Count == 0)
                    {

                        alumno.Anio = AlumnoB.AnioC;
                        alumno.PeriodoId = AlumnoB.PeriodoC;

                        #region GetDatos
                        var AlumnoCuatrimestre = alumno.AlumnoCuatrimestre
                                                .Select(cua => new
                                                {
                                                    cua.esRegular,
                                                    cua.HoraAsignacion,
                                                    cua.FechaAsignacion,
                                                    cua.UsuarioId
                                                })
                                                .FirstOrDefault();

                        var AlumnoInscrito = alumno.AlumnoInscrito
                                                .Select(ali => new
                                                {
                                                    ali.FechaInscripcion,
                                                    ali.HoraInscripcion,
                                                    ali.PagoPlanId,
                                                    ali.TurnoId,
                                                    ali.EsEmpresa,
                                                    ali.UsuarioId,
                                                    ali.EstatusId
                                                })
                                                .FirstOrDefault();
                        var GrupoAlumnoConfiguracion = alumno.AlumnoInscrito
                                                .FirstOrDefault().EsEmpresa ?
                                                alumno.GrupoAlumnoConfiguracion
                                                .Select(grpAl => new
                                                {
                                                    grpAl.GrupoId,
                                                    grpAl.CuotaColegiatura,
                                                    grpAl.CuotaInscripcion,
                                                    grpAl.EsCuotaCongelada,
                                                    grpAl.EsInscripcionCongelada,
                                                    grpAl.EsEspecial,
                                                    grpAl.UsuarioId,
                                                    grpAl.PagoPlanId,
                                                    grpAl.NumeroPagos,
                                                    grpAl.FechaRegistro,
                                                    grpAl.HoraRegistro,
                                                    grpAl.EstatusId
                                                }).FirstOrDefault() : null;
                        var AlumnoDescuento = new
                        {
                            DescuentoInscripcion = new
                            {
                                PagoConceptoId = 802,
                                Cuota = db.Cuota.Where(c =>
                                                c.OfertaEducativaId == AlumnoB.OfertaEducativaId
                                                && c.Anio == alumno.Anio
                                                && c.PeriodoId == alumno.PeriodoId
                                                && c.PagoConceptoId == 802)
                                            .Select(c => new
                                            {
                                                c.CuotaId,
                                                c.Monto
                                            }).FirstOrDefault(),
                                alumno.AlumnoDescuento
                                                .Where(d => d.PagoConceptoId == 802)
                                                .FirstOrDefault().DescuentoId,
                                alumno.AlumnoDescuento
                                                .Where(d => d.PagoConceptoId == 802)
                                                .FirstOrDefault().Monto
                            },
                            DescuentoColegiatura = new
                            {
                                PagoConceptoId = 800,
                                Cuota = db.Cuota.Where(c =>
                                                 c.OfertaEducativaId == AlumnoB.OfertaEducativaId
                                                 && c.Anio == alumno.Anio
                                                 && c.PeriodoId == alumno.PeriodoId
                                                 && c.PagoConceptoId == 800)
                                            .Select(c => new
                                            {
                                                c.CuotaId,
                                                c.Monto
                                            }).FirstOrDefault(),
                                alumno.AlumnoDescuento
                                                .Where(d => d.PagoConceptoId == 800)
                                                .FirstOrDefault().DescuentoId,
                                alumno.AlumnoDescuento
                                                .Where(d => d.PagoConceptoId == 800)
                                                .FirstOrDefault().Monto
                            }
                        };

                        int TurnoId = alumno.AlumnoInscrito
                                        .Where(ai => ai.OfertaEducativaId == AlumnoB.OfertaEducativaId)
                                        .FirstOrDefault()
                                        .TurnoId;
                        #endregion

                        db.Matricula.Remove(alumno.Matricula.FirstOrDefault());
                        db.AlumnoCuatrimestre.Remove(alumno.AlumnoCuatrimestre.FirstOrDefault());
                        db.AlumnoInscrito.Remove(alumno.AlumnoInscrito.FirstOrDefault());

                        List<DAL.GrupoAlumnoConfiguracionBitacora> ListaBitacora = new List<GrupoAlumnoConfiguracionBitacora>();

                        if (GrupoAlumnoConfiguracion != null)
                            db.GrupoAlumnoConfiguracion.Remove(alumno.GrupoAlumnoConfiguracion.FirstOrDefault());

                        var lstDescuentos = alumno.AlumnoDescuento.ToList();

                        lstDescuentos.ForEach(aldes =>
                        {
                            aldes.Anio = AlumnoB.AnioC;
                            aldes.PeriodoId = AlumnoB.PeriodoC;
                        });


                        db.SaveChanges();

                        #region Matricula
                        alumno.MatriculaId = Herramientas.Matricula.ObtenerMatricula(new DTOAlumnoInscrito
                        {
                            Anio = alumno.Anio,
                            PeriodoId = alumno.PeriodoId,
                            TurnoId = TurnoId
                        },
                               new DTOOfertaEducativa
                               {
                                   Rvoe = db.OfertaEducativa
                                            .Where(of => of.OfertaEducativaId == AlumnoB.OfertaEducativaId)
                                            .FirstOrDefault()
                                            .Rvoe
                               },
                               alumno.AlumnoId);
                        db.Matricula.Add(new Matricula
                        {
                            MatriculaId = alumno.MatriculaId,
                            AlumnoId = alumno.AlumnoId,
                            OfertaEducativaId = AlumnoB.OfertaEducativaId,
                            Anio = alumno.Anio,
                            PeriodoId = alumno.PeriodoId,
                            FechaAsignacion = alumno.FechaRegistro,
                            UsuarioId = alumno.UsuarioId
                        });

                        #endregion

                        #region AlumnoCuatrimestre
                        db.AlumnoCuatrimestre.Add(new DAL.AlumnoCuatrimestre
                        {
                            AlumnoId = alumno.AlumnoId,
                            OfertaEducativaId = AlumnoB.OfertaEducativaId,
                            Cuatrimestre = 1,
                            Anio = alumno.Anio,
                            PeriodoId = alumno.PeriodoId,
                            esRegular = AlumnoCuatrimestre.esRegular,
                            FechaAsignacion = AlumnoCuatrimestre.FechaAsignacion,
                            HoraAsignacion = AlumnoCuatrimestre.HoraAsignacion,
                            UsuarioId = AlumnoCuatrimestre.UsuarioId
                        });


                        #endregion

                        #region AlumnoInscrito
                        db.AlumnoInscrito.Add(new DAL.AlumnoInscrito
                        {
                            AlumnoId = alumno.AlumnoId,
                            OfertaEducativaId = AlumnoB.OfertaEducativaId,
                            Anio = alumno.Anio,
                            PeriodoId = alumno.PeriodoId,
                            FechaInscripcion = AlumnoInscrito.FechaInscripcion,
                            HoraInscripcion = AlumnoInscrito.HoraInscripcion,
                            PagoPlanId = AlumnoInscrito.PagoPlanId,
                            TurnoId = AlumnoInscrito.TurnoId,
                            EsEmpresa = AlumnoInscrito.EsEmpresa,
                            UsuarioId = AlumnoInscrito.UsuarioId,
                            EstatusId = AlumnoInscrito.EstatusId
                        });
                        #endregion

                        #region GrupoAlumnoConfiguracion
                        if (GrupoAlumnoConfiguracion != null)
                        {
                            db.GrupoAlumnoConfiguracion.Add(new DAL.GrupoAlumnoConfiguracion
                            {
                                AlumnoId = alumno.AlumnoId,
                                Anio = alumno.Anio,
                                PeriodoId = alumno.PeriodoId,
                                OfertaEducativaId = AlumnoB.OfertaEducativaId,
                                GrupoId = GrupoAlumnoConfiguracion.GrupoId,
                                CuotaColegiatura = GrupoAlumnoConfiguracion.CuotaColegiatura,
                                CuotaInscripcion = GrupoAlumnoConfiguracion.CuotaInscripcion,
                                EsCuotaCongelada = GrupoAlumnoConfiguracion.EsCuotaCongelada,
                                EsInscripcionCongelada = GrupoAlumnoConfiguracion.EsInscripcionCongelada,
                                EsEspecial = GrupoAlumnoConfiguracion.EsEspecial,
                                UsuarioId = GrupoAlumnoConfiguracion.UsuarioId,
                                PagoPlanId = GrupoAlumnoConfiguracion.PagoPlanId,
                                NumeroPagos = GrupoAlumnoConfiguracion.NumeroPagos,
                                FechaRegistro = GrupoAlumnoConfiguracion.FechaRegistro,
                                HoraRegistro = GrupoAlumnoConfiguracion.HoraRegistro,
                                EstatusId = GrupoAlumnoConfiguracion.EstatusId
                            });

                            ListaBitacora =
                            db.GrupoAlumnoConfiguracionBitacora
                                .Where(bit => bit.AlumnoId == alumno.AlumnoId
                                            && bit.Anio == AlumnoB.AnioA
                                            && bit.PeriodoId == AlumnoB.PeriodoA
                                            && bit.OfertaEducativaId == AlumnoB.OfertaEducativaId)
                                .ToList();
                            ListaBitacora.ForEach(a =>
                            {
                                a.Anio = alumno.Anio;
                                a.PeriodoId = alumno.PeriodoId;
                            });
                        }
                        #endregion

                        

                        db.SaveChanges();
                        
                        Console.WriteLine("Todo Correcto");
                    }

                    #region Pagos Alumno 
                    var PagosAlumno = alumno.Pago
                                        .Where(a => a.OfertaEducativaId == AlumnoB.OfertaEducativaId
                                                     && a.Anio == AlumnoB.AnioA
                                                     && a.PeriodoId == AlumnoB.PeriodoA)
                                        .ToList();
                    PagosAlumno.ForEach(pago =>
                    {
                        DAL.Cuota Cuotadb = db.Cuota
                                            .Where(c =>
                                                c.Anio == alumno.Anio
                                                && c.PeriodoId == alumno.PeriodoId
                                                && c.OfertaEducativaId == AlumnoB.OfertaEducativaId
                                                && c.PagoConceptoId == pago.Cuota1.PagoConceptoId)
                                            .FirstOrDefault();

                        pago.Anio = alumno.Anio;
                        pago.PeriodoId = alumno.PeriodoId;

                        pago.CuotaId = Cuotadb.CuotaId;

                    });
                    #endregion

                    db.SaveChanges();
                }
                catch (Exception err)
                {
                    Console.WriteLine("Error " + err.Message);
                }
            }
        }


        [TestMethod]
        public void GetNuevosV1()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                try
                {
                    DTOPeriodo PeriodoActual = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);
                    DTOPeriodo PeriodoSiguiente = new DTOPeriodo
                    {
                        Anio = PeriodoActual.PeriodoId == 3 ? PeriodoActual.Anio + 1 : PeriodoActual.Anio,
                        PeriodoId = PeriodoActual.PeriodoId == 3 ? 1 : PeriodoActual.PeriodoId + 1,
                    };
                    DTOPeriodo PeriodoAnterior = new DTOPeriodo
                    {
                        Anio = PeriodoActual.PeriodoId == 1 ? PeriodoActual.Anio - 1 : PeriodoActual.Anio,
                        PeriodoId = PeriodoActual.PeriodoId == 1 ? 3 : PeriodoActual.PeriodoId - 1,
                    };

                     db.AlumnoInscrito
                                    .Where(a => ((a.Alumno.Anio == PeriodoActual.Anio && a.Alumno.PeriodoId == PeriodoActual.PeriodoId) ||
                                                    (a.Alumno.Anio == PeriodoSiguiente.Anio && a.Alumno.PeriodoId == PeriodoSiguiente.PeriodoId) ||
                                                    (a.Alumno.Anio == PeriodoAnterior.Anio && a.Alumno.PeriodoId == PeriodoAnterior.PeriodoId))
                                                && ((a.Anio == PeriodoActual.Anio && a.PeriodoId == PeriodoActual.PeriodoId) ||
                                                    (a.Anio == PeriodoSiguiente.Anio && a.PeriodoId == PeriodoSiguiente.PeriodoId) ||
                                                    (a.Anio == PeriodoAnterior.Anio && a.PeriodoId == PeriodoAnterior.PeriodoId))
                                                && a.OfertaEducativa.OfertaEducativaTipoId != 4
                                                && a.Alumno.EstatusId != 3
                                                && (a.Alumno.AlumnoInscritoBitacora.Count == 0 ||
                                                    a.Alumno.AlumnoInscritoBitacora.Where(k => k.OfertaEducativaId == a.OfertaEducativaId && k.PagoPlanId == null).ToList().Count == 1 ||
                                                    a.Alumno.AlumnoInscritoBitacora.Where(ab => ab.OfertaEducativaId == a.OfertaEducativaId && ab.Anio == a.Anio && ab.PeriodoId == a.PeriodoId).ToList().Count == 1)
                                                && a.EstatusId != 2)
                                    .ToList()
                                    .GroupBy(a => new { a.AlumnoId, a.Anio, a.PeriodoId, a.OfertaEducativaId })
                                    .Select(a => a.FirstOrDefault())
                                    .ToList()
                                             .Select(a => new
                                             {
                                                 a.AlumnoId,
                                                 Nombre = a.Alumno.Nombre + " " + a.Alumno.Paterno + " " + a.Alumno.Materno,
                                                 Usuario = a.Alumno.Usuario.Nombre,
                                                 FechaRegistro = ((a.Alumno.FechaRegistro.Day < 10 ? "0" + a.Alumno.FechaRegistro.Day : "" + a.Alumno.FechaRegistro.Day) + "/" +
                                                                (a.Alumno.FechaRegistro.Month < 10 ? "0" + a.Alumno.FechaRegistro.Month : "" + a.Alumno.FechaRegistro.Month) + "/" +
                                                                "" + a.Alumno.FechaRegistro.Year),
                                                 a.OfertaEducativa.Descripcion,
                                                 a.OfertaEducativaId
                                             })
                                             .OrderBy(k => k.AlumnoId)
                                            .ToList();
                }
                catch (Exception err)
                {
                }

            }
        }

        [TestMethod]
        public void GetNuevosV2()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                try
                {
                    DTOPeriodo PeriodoActual = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);
                    DTOPeriodo PeriodoSiguiente = new DTOPeriodo
                    {
                        Anio = PeriodoActual.PeriodoId == 3 ? PeriodoActual.Anio + 1 : PeriodoActual.Anio,
                        PeriodoId = PeriodoActual.PeriodoId == 3 ? 1 : PeriodoActual.PeriodoId + 1,
                    };
                    DTOPeriodo PeriodoAnterior = new DTOPeriodo
                    {
                        Anio = PeriodoActual.PeriodoId == 1 ? PeriodoActual.Anio - 1 : PeriodoActual.Anio,
                        PeriodoId = PeriodoActual.PeriodoId == 1 ? 3 : PeriodoActual.PeriodoId - 1,
                    };

                    db.AlumnoInscrito
                                   .Where(a => ((a.Alumno.Anio == PeriodoActual.Anio && a.Alumno.PeriodoId == PeriodoActual.PeriodoId) ||
                                                   (a.Alumno.Anio == PeriodoSiguiente.Anio && a.Alumno.PeriodoId == PeriodoSiguiente.PeriodoId) ||
                                                   (a.Alumno.Anio == PeriodoAnterior.Anio && a.Alumno.PeriodoId == PeriodoAnterior.PeriodoId))
                                               && ((a.Anio == PeriodoActual.Anio && a.PeriodoId == PeriodoActual.PeriodoId) ||
                                                   (a.Anio == PeriodoSiguiente.Anio && a.PeriodoId == PeriodoSiguiente.PeriodoId) ||
                                                   (a.Anio == PeriodoAnterior.Anio && a.PeriodoId == PeriodoAnterior.PeriodoId))
                                               && a.OfertaEducativa.OfertaEducativaTipoId != 4
                                               && a.Alumno.EstatusId != 3
                                               && (a.Alumno.AlumnoInscritoBitacora.Count == 0 ||
                                                   a.Alumno.AlumnoInscritoBitacora.Where(k => k.OfertaEducativaId == a.OfertaEducativaId && k.PagoPlanId == null).ToList().Count == 1 ||
                                                   a.Alumno.AlumnoInscritoBitacora.Where(ab => ab.OfertaEducativaId == a.OfertaEducativaId && ab.Anio == a.Anio && ab.PeriodoId == a.PeriodoId).ToList().Count == 1)
                                               && a.EstatusId != 2)
                                   .ToList()
                                   .GroupBy(a => new { a.AlumnoId, a.Anio, a.PeriodoId, a.OfertaEducativaId })
                                   .Select(a => a.FirstOrDefault())
                                   .ToList()
                                            .Select(a => new
                                            {
                                                a.AlumnoId,
                                                Nombre = a.Alumno.Nombre + " " + a.Alumno.Paterno + " " + a.Alumno.Materno,
                                                Usuario = a.Alumno.Usuario.Nombre,
                                                FechaRegistro = ((a.Alumno.FechaRegistro.Day < 10 ? "0" + a.Alumno.FechaRegistro.Day : "" + a.Alumno.FechaRegistro.Day) + "/" +
                                                               (a.Alumno.FechaRegistro.Month < 10 ? "0" + a.Alumno.FechaRegistro.Month : "" + a.Alumno.FechaRegistro.Month) + "/" +
                                                               "" + a.Alumno.FechaRegistro.Year),
                                                a.OfertaEducativa.Descripcion,
                                                a.OfertaEducativaId
                                            })
                                            .OrderBy(k => k.AlumnoId)
                                           .ToList();
                }
                catch (Exception err)
                {
                }

            }
        }
    }
}
