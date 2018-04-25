using DAL;
using DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Pruebas
{
    [TestClass]
    public class PruebasBecas
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");
        
        [TestMethod]
        public void AlumnoDatos()
        {

            //BLL.BLLAlumno.ObtenerAlumno(8174);
            //BLL.BLLPago.ConsultarAdeudo(8176);
            //BLL.BLLAlumno.ObtenerAlumnoCompleto(5945);
        }
        [TestMethod]
        public void BuscarAlumno()
        {
            var objRes =
            BLL.BLLAlumnoPortal.BuscarAlumno(8196, 2);
            Console.WriteLine(objRes.Nombre);
        }

        [TestMethod]
        public void PEriodos()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DateTime fhoy = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var fprev = fhoy.AddDays(15);
                var periodo = (from a in db.Periodo
                               where fhoy >= a.FechaInicial && fhoy <= a.FechaFinal
                                    || (fprev >= a.FechaInicial && fprev <= a.FechaFinal)
                               select new DTOPeriodo
                               {
                                   Descripcion = a.Descripcion,
                                   Anio = a.Anio,
                                   PeriodoId = a.PeriodoId,
                               }).ToList();
                //var periodo = db.Periodo
                //                 .Where(P =>
                //                   fhoy >= P.FechaInicial && fhoy <= P.FechaFinal).FirstOrDefault();
                periodo.ForEach(l =>
                {
                    Console.WriteLine(l.Descripcion);
                });
            }
        }

        [TestMethod]
        public void Alumnos()
        {
            var obj = BLL.BLLAlumnoPortal.ListarAlumnos();
            obj.ForEach(k =>
            {
                Console.WriteLine(k.AlumnoId + " -  " + k.Nombre + " -  " + k.Descripcion);
            });
        }
        [TestMethod]
        public void generarobj()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                //var obj = ((from a in db.Alumno
                //                where a.EstatusId != 3
                //                //where a.FechaRegistro.Year >= DateTime.Now.Year - 7
                //                select new DTO.DTOAlumnoLigero2
                //                {
                //                    AlumnoId = a.AlumnoId,
                //                    Nombre = a.Nombre + " " + a.Paterno + " " + a.Materno,
                //                    FechaRegistro = a.FechaRegistro.ToString(),
                //                    Usuario = a.Usuario.Nombre,
                //                    OfertasEducativas = a.AlumnoInscrito
                //                                       .Select(AI => AI.OfertaEducativa.Descripcion)
                //                                           .ToList(),
                //                }).AsNoTracking().ToList());

                //    obj.ForEach(s =>
                //    {
                //        s.Descripcion = string.Join(" / ", s.OfertasEducativas.ToArray());
                //    });

            }

        }

        [TestMethod]
        public void generarobj2()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var obj = ((from a in db.Alumno
                            where a.EstatusId != 3
                            //where a.FechaRegistro.Year >= DateTime.Now.Year - 7
                            select new DTO.DTOAlumnoLigero
                            {
                                AlumnoId = a.AlumnoId,
                                Nombre = a.Nombre + " " + a.Paterno + " " + a.Materno,
                                FechaRegistro = a.FechaRegistro.ToString(),
                                Usuario = a.Usuario.Nombre,
                                OfertasEducativas = a.AlumnoInscrito
                                                   .Select(AI => AI.OfertaEducativa.Descripcion)
                                                       .ToList()
                            }).AsNoTracking().ToList());

            }

        }
        [TestMethod]
        public void BecaAcademica()
        {
            List<DTO.Alumno.Beca.DTOAlumnoBeca> Alumno = new List<DTO.Alumno.Beca.DTOAlumnoBeca>();


            Alumno.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 8490,
                anio = 2017,
                periodoId = 3,
                ofertaEducativaId = 29,
                porcentajeBeca = 55m,//70.15
                porcentajeInscripcion = 100m,
                esSEP = false,
                esComite = false,
                esEmpresa = true,
                usuarioId = 8358, //Usua4rio que inscribio  -> Alejandra 6070
                fecha = "2016-09-12", // Solo si esta en AlumnoInscrito Fecha 23/01/2017
                genera = true
                //    //Colegiatura = decimal
                //    //Inscripcion = decimal
            });
            
            Alumno.ForEach(a =>
            {
                BLL.BLLAlumnoPortal.AplicaBeca_Excepcion(a, false);
            });


        }


        [TestMethod]
        public void AplicaBecaSep()
        {
            #region Variables 
            List<DTO.Alumno.Beca.DTOAlumnoBeca> lstAlumnos = new List<DTO.Alumno.Beca.DTOAlumnoBeca>();
            
            #endregion
            List<string> Errores = new List<string>();
            lstAlumnos.ForEach(alumno =>
            {
                try
                {
                    BLL.BLLAlumnoPortal.AplicaBeca(alumno, false);
                    Errores.Add("Alumno Correcto :  " + alumno.alumnoId);
                }
                catch
                {
                    Errores.Add("Alumno Incorrecto :  " + alumno.alumnoId);
                }
            });
            Errores.ForEach(i => Console.WriteLine("{0}\t", i));
            //7589 Sin ningun descuento
            DTO.Alumno.Beca.DTOAlumnoBeca Alumno =
            new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7992,
                anio = 2017,
                periodoId = 2,
                ofertaEducativaId = 3,
                porcentajeBeca = 25.00m,
                porcentajeInscripcion = 25.00m,
                esSEP = true,
                esComite = false,
                esEmpresa = false,
                usuarioId = 6070,
                fecha = "2017-02-07",
                genera = true
            };

            BLL.BLLAlumnoPortal.AplicaBeca(Alumno, false);

        }

        [TestMethod]
        public void GenerarReferencias()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var lstpagos = db.Pago.Where(o => o.ReferenciaId.Length < 1).ToList();
                lstpagos.ForEach(p =>
                {
                    p.ReferenciaId = db.spGeneraReferencia(p.PagoId).FirstOrDefault();
                });

                db.SaveChanges();
            }
        }

        [TestMethod]
        public void PuebasDatosAlumno()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                var fechaactual = DateTime.Now;
                var periodos = db.Periodo.Where(p => p.FechaFinal >= fechaactual).Take(2)
                                         .Select(s => new DTOPeriodoPromocionCasa
                                         {
                                             Descripcion = s.Descripcion,
                                             Anio = s.Anio,
                                             PeriodoId = s.PeriodoId,
                                             Meses = db.Subperiodo.Where(sp => sp.PeriodoId == s.PeriodoId)
                                                                  .Select(d => new DTOMes
                                                                  {
                                                                      Descripcion = d.Mes.Descripcion,
                                                                      MesId = d.MesId
                                                                  }).ToList()
                                         }).ToList();

            }
        }



        [TestMethod]
        public void PuebasPromocionCasa()
        {

            DTOAlumnoPromocionCasa Promocion = new DTOAlumnoPromocionCasa
            {
                AlumnoId = 7486,
                OfertaEducativaIdActual = 29,
                AlumnoIdProspecto = 8117,
                Anio = 2017,
                PeriodoId = 2,
                SubPeriodoId = 4,
                Monto = 1500,
                EstatusId = 1,
                UsuarioId = 8272
            };

            BLL.BLLAlumnoPortal.AplicarPromocionCasa(Promocion);




        }

        [TestMethod]
        public void AlumnosVariados()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var AlumnoTodos = db.Alumno
                                        .Where(k =>
                                                k.AlumnoRevision
                                                    .Where(ar =>
                                                            ar.Anio == 2017
                                                            && ar.PeriodoId == 2
                                                            && ar.OfertaEducativa.OfertaEducativaTipoId != 4).ToList().Count > 0
                                                || k.AlumnoInscrito
                                                        .Where(ai =>
                                                                ai.Anio == 2017
                                                                && ai.PeriodoId == 2
                                                                && ai.PagoPlanId != null
                                                                && ai.OfertaEducativa.OfertaEducativaTipoId != 4
                                                                && db.AlumnoInscritoBitacora
                                                                        .Where(AIB => ai.AlumnoId == AIB.AlumnoId
                                                                                            && AIB.OfertaEducativaId == ai.OfertaEducativaId
                                                                                             && (AIB.Anio != ai.Anio
                                                                                            || (AIB.Anio == ai.Anio && AIB.PeriodoId != ai.PeriodoId))
                                                                                             ).ToList().Count > 0
                                                                        ).ToList().Count > 0).ToList();


                var alumnoTodos2 = db.Alumno
                                        .Where(k =>
                                               k.AlumnoInscrito
                                                        .Where(ai =>
                                                                ai.Anio == 2017
                                                                && ai.PeriodoId == 2
                                                                && ai.OfertaEducativa.OfertaEducativaTipoId != 4
                                                                && ai.PagoPlanId != null
                                                                && db.AlumnoInscritoBitacora
                                                                        .Where(AIB => ai.AlumnoId == AIB.AlumnoId
                                                                                            && AIB.OfertaEducativaId == ai.OfertaEducativaId
                                                                                            && (AIB.Anio != ai.Anio
                                                                                            || (AIB.Anio == ai.Anio && AIB.PeriodoId != ai.PeriodoId))
                                                                                             && AIB.OfertaEducativa.OfertaEducativaTipoId != 4).ToList().Count == 0
                                                                        ).ToList().Count > 0).Select(P =>
                                                                      new DTO.DTOAlumno
                                                                      {
                                                                          AlumnoId = P.AlumnoId,
                                                                          AlumnoOfertaEducativa = P.AlumnoInscrito
                                                                                                  .Where(o => o.Anio == 2017
                                                                                                      && o.PeriodoId == 2).FirstOrDefault().OfertaEducativaId
                                                                      }).ToList();
                Console.WriteLine("insert into #AlumnosNue Values");
                alumnoTodos2.ForEach(i => Console.WriteLine("{0}\t", ",(" + i.AlumnoId + ", " + i.AlumnoOfertaEducativa + ",2017,2)"));
                Console.WriteLine("Todos");
            }
        }

        [TestMethod]
        public void PuebasreporteVoBo()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var anio = 2017;
                var periodoid = 2;

                var todos = db.Alumno.Where(a => a.AlumnoRevision.Where(ar => ar.Anio == anio
                                                                                     && ar.PeriodoId == periodoid
                                                                                     && ar.OfertaEducativa.OfertaEducativaTipoId != 4).Count() > 0

                                                           || a.AlumnoInscrito.Where(ai => ai.Anio == anio
                                                                                     && ai.PeriodoId == periodoid
                                                                                     && ai.OfertaEducativa.OfertaEducativaTipoId != 4
                                                                                     && db.AlumnoInscritoBitacora.Where(aib => aib.AlumnoId == ai.AlumnoId
                                                                                                                        && aib.OfertaEducativaId == ai.OfertaEducativaId
                                                                                                                        && (aib.Anio != anio || (aib.Anio == anio && aib.PeriodoId != periodoid))).Count() > 0
                                                                                     ).Count() > 0)
                                                  .Select(b => new DTOAlumnosVoBo
                                                  {
                                                      AlumnoId = b.AlumnoId,
                                                      Nombre = b.Paterno + " " + b.Materno + " " + b.Nombre,
                                                      AlumnoInscrito = b.AlumnoInscrito.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault(),
                                                      AlumnoInscritoBitacora = b.AlumnoInscritoBitacora.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault(),
                                                      AlumnoRevision = b.AlumnoRevision.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault()
                                                  }).ToList();


                var todos1 = todos.Select(td => new DTOReporteVoBo
                {
                    AlumnoId = td.AlumnoId,
                    Nombre = td.Nombre,
                    OfertaEducativaid = td.AlumnoInscrito?.OfertaEducativaId ?? td.AlumnoRevision.OfertaEducativaId,
                    OfertaEducativa = td.AlumnoInscrito?.OfertaEducativa.Descripcion ?? td.AlumnoRevision.OfertaEducativa.Descripcion,
                    Inscrito = td.AlumnoInscrito != null ? "Si" : "No",
                    FechaInscrito = td.AlumnoInscritoBitacora?.FechaInscripcion.ToString("dd/MM/yyyy", Cultura) ?? td.AlumnoInscrito?.FechaInscripcion.ToString("dd/MM/yyyy", Cultura) ?? "-",
                    HoraInscrito = td.AlumnoInscritoBitacora?.HoraInscripcion.ToString() ?? td.AlumnoInscrito?.HoraInscripcion.ToString() ?? "-",
                    UsuarioInscribio = td.AlumnoInscritoBitacora != null ? td.AlumnoInscritoBitacora.Usuario.Paterno + " " + td.AlumnoInscritoBitacora.Usuario.Materno + " " + td.AlumnoInscritoBitacora.Usuario.Nombre
                       : td.AlumnoInscrito != null ? td.AlumnoInscrito.Usuario.Paterno + " " + td.AlumnoInscrito.Usuario.Materno + " " + td.AlumnoInscrito.Usuario.Nombre : "-",
                    FechaVoBo = td.AlumnoRevision?.FechaRevision.ToString("dd/MM/yyyy", Cultura) ?? "-",
                    HoraVoBo = td.AlumnoRevision?.HoraRevision.ToString() ?? "-",
                    InscripcionCompleta = td.AlumnoRevision?.InscripcionCompleta == true ? "Si" : td.AlumnoRevision?.InscripcionCompleta == false ? "No" : "-",
                    Asesorias = td.AlumnoRevision?.AsesoriaEspecial.ToString() ?? "-",
                    Materias = td.AlumnoRevision?.AdelantoMateria.ToString() ?? "-",
                    UsuarioVoBo = td.AlumnoRevision != null ? td.AlumnoRevision.Usuario.Paterno + " " + td.AlumnoRevision.Usuario.Materno + " " + td.AlumnoRevision.Usuario.Nombre : "-"
                }).ToList();
            }
        }

        [TestMethod]
        public void CancelarPago()
        {
            int PagoId = 40037;
            int UsuarioId = 8272;
            string Comentario = "El concepto era para pago de colegiaturas.";

            BLL.BLLCargo.CancelarTotal(PagoId, UsuarioId, Comentario);

        }


        [TestMethod]
        public void reportesPuebas()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                //int anio = 2018;
                //int periodoid = 1;


                //List<DTOAlumnosVoBo> alumnoRevision = db.Alumno.Where(a => a.AlumnoRevision.Where(ar => ar.Anio == anio
                //                                                       && ar.PeriodoId == periodoid
                //                                                       && ar.OfertaEducativa.OfertaEducativaTipoId != 4).Count() > 0

                //                             || a.AlumnoInscrito.Where(ai => ai.Anio == anio
                //                                                       && ai.PeriodoId == periodoid
                //                                                       && ai.OfertaEducativa.OfertaEducativaTipoId != 4
                //                                                       && db.AlumnoInscritoBitacora.Where(aib => aib.AlumnoId == ai.AlumnoId
                //                                                                                          && aib.OfertaEducativaId == ai.OfertaEducativaId
                //                                                                                          && (aib.Anio != anio || (aib.Anio == anio && aib.PeriodoId != periodoid))).Count() > 0
                //                                                       ).Count() > 0)
                //                    .Select(b => new DTOAlumnosVoBo
                //                    {
                //                        AlumnoId = b.AlumnoId,
                //                        Nombre = b.Paterno + " " + b.Materno + " " + b.Nombre,
                //                        AlumnoInscrito = b.AlumnoInscrito.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault(),
                //                        AlumnoInscritoBitacora = b.AlumnoInscritoBitacora.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault(),
                //                        AlumnoRevision = b.AlumnoRevision.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault(),
                //                        Email = b.AlumnoDetalle.Email
                //                    }).ToList();



                //List<int> alumnoId = db.spAlumnoInscritoCompleto(anio, periodoid).Where(a => a.TipoAlumno == "Reinscrito").Select(b => b.AlumnoId).ToList();
                //alumnoId.AddRange(db.AlumnoRevision.Where(a => a.Anio == anio && a.PeriodoId == periodoid && a.OfertaEducativa.OfertaEducativaTipoId != 4).Select(b => b.AlumnoId).ToList());
                //alumnoId = alumnoId.Distinct().ToList();


                //List<DTOAlumnosVoBo1> alumnoRevision2 = db.Alumno.Where(a => alumnoId.Contains(a.AlumnoId))

                //                                                 .Select(b => new DTOAlumnosVoBo1
                //                                                 {
                //                                                     AlumnoId = b.AlumnoId,
                //                                                     Nombre = b.Paterno + " " + b.Materno + " " + b.Nombre,
                //                                                     AlumnoInscritoBitacora = b.AlumnoInscritoBitacora.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault(),
                //                                                     AlumnoRevision = b.AlumnoRevision.Where(c => c.Anio == anio && c.PeriodoId == periodoid && c.OfertaEducativa.OfertaEducativaTipoId != 4).FirstOrDefault(),
                //                                                     Email = b.AlumnoDetalle.Email
                //                                                 }).ToList();

                //List<AlumnoInscritoCompleto> alumnoInscrito = db.spAlumnoInscritoCompleto(anio, periodoid).Where(a => a.TipoAlumno == "Reinscrito").ToList();

                //alumnoRevision2.ForEach(n=>
                //{
                //    n.AlumnoInscrito = alumnoInscrito.Where(c => c.AlumnoId == n.AlumnoId)?.FirstOrDefault();
                //});







            }

        }



        [TestMethod]
        public void generarReferenciasDoctorado()
        {
            BLL.BLLPagoPortal.GenerarSemestre(8316, 44,2017, 11,2018, 4, 8358, decimal.Parse("0"), decimal.Parse("2500"));
        }
    }



}

