﻿using DAL;
using DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas
{
    [TestClass]
    public class PruebasBecas
    {

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
            BLL.BLLAlumno.BuscarAlumno(8196, 2);
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
            var obj = BLL.BLLAlumno.ListarAlumnos();
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
            //7589 Sin ningun descuento
            DTO.Alumno.Beca.DTOAlumnoBeca Alumno = new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7646,
                anio = 2017,
                periodoId = 2,
                ofertaEducativaId = 5,
                porcentajeBeca = 64.33m, //70.15
                porcentajeInscripcion = 0m,
                esSEP = false,
                esComite = false,
                esEmpresa = true,
                usuarioId = 6070, //Usua4rio que inscribio  -> Alejandra 6070
                fecha = "12/01/2017", // Solo si esta en AlumnoInscrito Fecha 23/01/2017
                genera = true

            //    //Colegiatura = decimal
            //    //Inscripcion = decimal
            };

            BLL.BLLAlumno.AplicaBeca_Excepcion(Alumno, false);

        }


        [TestMethod]
        public void AplicaBecaSep()
        {
            #region Variables 
            List<DTO.Alumno.Beca.DTOAlumnoBeca> lstAlumnos = new List<DTO.Alumno.Beca.DTOAlumnoBeca>();
            //lstAlumnos.Add(
            //     new DTO.Alumno.Beca.DTOAlumnoBeca
            //     {
            //         alumnoId = 599,
            //         anio = 2017,
            //         periodoId = 2,
            //         ofertaEducativaId = 25,
            //         porcentajeBeca = 60.00m,
            //         porcentajeInscripcion = 60.00m,
            //         esSEP = true,
            //         esComite = false,
            //         esEmpresa = false,
            //         usuarioId = 6070,
            //         fecha = "2017-02-07",
            //         genera = true
            //     });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 1197,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 9,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 4226,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 16,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 5429,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 27,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 5955,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 18,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 5992,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 17,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6056,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 18,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6122,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 28,
            //                    porcentajeBeca = 65.00m,
            //                    porcentajeInscripcion = 65.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6206,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 18,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6511,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 5,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6512,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 5,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6557,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 14,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6683,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 2,
            //                    porcentajeBeca = 45.00m,
            //                    porcentajeInscripcion = 45.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6717,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 1,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6768,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6775,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 28,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6787,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 3,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6827,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 2,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6828,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 14,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6835,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6841,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6842,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 65.00m,
            //                    porcentajeInscripcion = 65.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6860,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 5,
            //                    porcentajeBeca = 65.00m,
            //                    porcentajeInscripcion = 65.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6880,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 28,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6911,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 14,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6913,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6920,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 65.00m,
            //                    porcentajeInscripcion = 65.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6928,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6929,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 65.00m,
            //                    porcentajeInscripcion = 65.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6947,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 1,
            //                    porcentajeBeca = 65.00m,
            //                    porcentajeInscripcion = 65.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 6989,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7081,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 35.00m,
            //                    porcentajeInscripcion = 35.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7119,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 1,
            //                    porcentajeBeca = 40.00m,
            //                    porcentajeInscripcion = 40.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7142,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 5,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7316,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7337,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 7,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7396,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 1,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7418,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7429,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 10,
            //                    porcentajeBeca = 70.00m,
            //                    porcentajeInscripcion = 70.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7503,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 25,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7513,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 65.00m,
            //                    porcentajeInscripcion = 65.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7516,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 2,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7524,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7548,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 60.00m,
            //                    porcentajeInscripcion = 60.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7576,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7609,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 5,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7615,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7636,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7677,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7678,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7696,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7697,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 28,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7699,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 1,
            //                    porcentajeBeca = 40.00m,
            //                    porcentajeInscripcion = 40.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7715,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 2,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7746,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 6,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7753,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7772,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 3,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7781,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 14,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7787,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7791,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7794,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7796,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7798,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 28,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7800,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 5,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7806,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 2,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7808,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7809,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7810,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 5,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7811,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 1,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7812,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7813,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7820,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 3,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7833,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7846,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7850,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 14,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7852,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7853,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7854,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7866,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 14,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7886,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7893,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7896,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7902,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 28,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7904,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 28,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7912,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 3,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7914,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7918,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 28,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7924,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 3,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7938,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7940,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 14,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7956,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 3,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7961,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 2,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7976,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7985,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 8,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            lstAlumnos.Add(
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
                            });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 7999,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 55.00m,
            //                    porcentajeInscripcion = 55.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 8013,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 29,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 8048,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 3,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            //lstAlumnos.Add(
            //                new DTO.Alumno.Beca.DTOAlumnoBeca
            //                {
            //                    alumnoId = 8137,
            //                    anio = 2017,
            //                    periodoId = 2,
            //                    ofertaEducativaId = 3,
            //                    porcentajeBeca = 50.00m,
            //                    porcentajeInscripcion = 50.00m,
            //                    esSEP = true,
            //                    esComite = false,
            //                    esEmpresa = false,
            //                    usuarioId = 6070,
            //                    fecha = "2017-02-07",
            //                    genera = true
            //                });
            #endregion
            List<string> Errores = new List<string>();
            lstAlumnos.ForEach(alumno =>
            {
                try
                {
                    BLL.BLLAlumno.AplicaBeca(alumno, false);
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

            BLL.BLLAlumno.AplicaBeca(Alumno, false);

        }

        [TestMethod]
        public void GenerarReferencias()
        {
            using(UniversidadEntities db= new UniversidadEntities())
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
            using(UniversidadEntities db= new UniversidadEntities())
            {
               
                var v = false;
                if (db.AlumnoDetalleAlumno.Where(w=> w.AlumnoId == 8173).Count()>0)
                {

                    var fechaActual = DateTime.Now;
                    var Periodo = db.Periodo.Where(p => p.FechaInicial <= fechaActual
                                                                         && fechaActual <= p.FechaFinal).FirstOrDefault();
                    DateTime fechaActualizo = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == 8173).FirstOrDefault().Fecha;

                    if ((Periodo.FechaInicial <= fechaActualizo && fechaActualizo <= Periodo.FechaFinal))
                    {
                        v = false;
                    }
                    else
                    {
                        v = true;
                    }
                    
                }else
                {
                    v = true;
                }

               
                Console.WriteLine(v);
            }
        }

      [TestMethod]
      public void AlumnosVariados()
        {
            using(UniversidadEntities db= new UniversidadEntities())
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

                
                var alumnoTodos2= db.Alumno
                                        .Where(k =>
                                               k.AlumnoInscrito
                                                        .Where(ai =>
                                                                ai.Anio == 2017
                                                                && ai.PeriodoId == 2
                                                                && ai.OfertaEducativa.OfertaEducativaTipoId != 4
                                                                && ai.PagoPlanId!=null
                                                                && db.AlumnoInscritoBitacora
                                                                        .Where(AIB => ai.AlumnoId == AIB.AlumnoId
                                                                                            && AIB.OfertaEducativaId == ai.OfertaEducativaId
                                                                                            && ( AIB.Anio != ai.Anio
                                                                                            || (AIB.Anio == ai.Anio && AIB.PeriodoId != ai.PeriodoId))
                                                                                             && AIB.OfertaEducativa.OfertaEducativaTipoId != 4).ToList().Count == 0
                                                                        ).ToList().Count > 0).Select ( P=> 
                                                                        new DTO.DTOAlumno
                                                                        {
                                                                             AlumnoId=P.AlumnoId,
                                                                             AlumnoOfertaEducativa=P.AlumnoInscrito
                                                                                                    .Where(o=>o.Anio ==2017 
                                                                                                        && o.PeriodoId ==2 ).FirstOrDefault().OfertaEducativaId
                                                                        }).ToList();
                Console.WriteLine("insert into #AlumnosNue Values");
                alumnoTodos2.ForEach(i => Console.WriteLine("{0}\t", ",("+i.AlumnoId + ", " + i.AlumnoOfertaEducativa + ",2017,2)"));
                Console.WriteLine("Todos");
            }
        }

    }
}


                                                                                        //&& AIB.Anio == 2017
                                                                                        //&& AIB.PeriodoId == 2
                                                                                        //&& AIB.OfertaEducativaId == ai.OfertaEducativaId