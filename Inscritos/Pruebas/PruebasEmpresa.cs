using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL;
using System.Linq;
using System.Data.Entity;
using DTO;
using System.Collections.Generic;

namespace Pruebas
{
    [TestClass]
    public class PruebasEmpresa
    {
        int GrupoId = 36;
        [TestMethod]
        public void LazyOff()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<DTOAlumnoEspecial> lstAlumnos = new List<DTOAlumnoEspecial>();
                    
                    if (GrupoId != 0)
                    {
                        //lstAlumnos = lstAlumnos.Where(a => a.GrupoAlumno != null).ToList().Where(b=> b.GrupoAlumno.GrupoId == GrupoId ).ToList();
                        lstAlumnos = (from al in db.GrupoAlumno
                                      join ale in db.AlumnoInscrito on al.AlumnoId equals ale.AlumnoId
                                      where
                                      ale.EsEmpresa == true
                                      && ale.OfertaEducativa.OfertaEducativaTipoId != 4
                                      && al.GrupoId == GrupoId
                                      && al.EstatusId != 3
                                      select new DTOAlumnoEspecial
                                      {
                                          AlumnoId = ale.AlumnoId,
                                          Anio = ale.Anio,
                                          Nombre = ale.Alumno.Nombre + " " + ale.Alumno.Paterno + " " + ale.Alumno.Materno,
                                          OfertaEducativaId = ale.OfertaEducativaId,
                                          OfertaEducativaS = ale.OfertaEducativa.Descripcion,
                                          PagoPlanId = (int)ale.PagoPlanId,
                                          PeriodoId = ale.PeriodoId,
                                          Estatus = ale.EstatusId,

                                          AlumnoCuota = db.GrupoAlumnoConfiguracion
                                                 .Where(ac =>
                                                 ac.AlumnoId == al.AlumnoId
                                                 && ac.GrupoId == al.GrupoId).
                                             Select(k => new DTOGrupoAlumnoCuota
                                             {
                                                 AlumnoId = k.AlumnoId,
                                                 OfertaEducativaId = k.OfertaEducativaId,
                                                 CuotaColegiatura = k.CuotaColegiatura,
                                                 CuotaCongelada = k.EsCuotaCongelada,
                                                 CuotaInscripcion = k.CuotaInscripcion,
                                                 InscripcionCongelada = k.EsInscripcionCongelada,
                                                 EsEspecial = k.EsEspecial,
                                                 UsuarioId = k.UsuarioId
                                             }).FirstOrDefault()
                                      }
                                     ).ToList();





                    }
                    else
                    {
                        //&& k.Anio == db.AlumnoInscrito.Where(c => c.AlumnoId == k.AlumnoId).ToList().Max(d => d.Anio)
                        //&& k.PeriodoId == db.AlumnoInscrito.Where(c => c.Anio == (db.AlumnoInscrito.Where(d => d.AlumnoId == k.AlumnoId).ToList().Max(e => e.Anio))).ToList().Max(b => b.PeriodoId)
                        lstAlumnos = db.AlumnoInscrito.Where(al =>
                                     al.EsEmpresa == true
                                     && al.OfertaEducativa.OfertaEducativaTipoId != 4
                                     && al.EstatusId == 1)
                                     .Select(al => new DTOAlumnoEspecial
                                     {
                                         AlumnoId = al.AlumnoId,
                                         Anio = al.Anio,
                                         Nombre = al.Alumno.Nombre + " " + al.Alumno.Paterno + " " + al.Alumno.Materno,
                                         OfertaEducativaId = al.OfertaEducativaId,
                                         OfertaEducativaS = al.OfertaEducativa.Descripcion,
                                         PagoPlanId = (int)al.PagoPlanId,
                                         PeriodoId = al.PeriodoId,
                                         Estatus = al.EstatusId,
                                         AlumnoCuota = al.Alumno
                                             .GrupoAlumnoConfiguracion
                                                 .Where(ac =>
                                                 ac.OfertaEducativaId == al.OfertaEducativaId
                                                 && ac.EstatusId == 1).
                                             Select(k => new DTOGrupoAlumnoCuota
                                             {
                                                 AlumnoId = k.AlumnoId,
                                                 OfertaEducativaId = k.OfertaEducativaId,
                                                 CuotaColegiatura = k.CuotaColegiatura,
                                                 CuotaCongelada = k.EsCuotaCongelada,
                                                 CuotaInscripcion = k.CuotaInscripcion,
                                                 InscripcionCongelada = k.EsInscripcionCongelada,
                                                 EsEspecial = k.EsEspecial,
                                                 UsuarioId = k.UsuarioId
                                             }).FirstOrDefault()
                                     })
                                     .OrderByDescending(K => K.AlumnoId)
                                     .ToList();
                    }


                }
                catch
                { }
            }
        }
        [TestMethod]
        public void LazyOn()
        {
            
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<DTOAlumnoEspecial> lstAlumnos = new List<DTOAlumnoEspecial>();

                    db.Configuration.LazyLoadingEnabled = true;

                    if (GrupoId != 0)
                    {
                        //lstAlumnos = lstAlumnos.Where(a => a.GrupoAlumno != null).ToList().Where(b=> b.GrupoAlumno.GrupoId == GrupoId ).ToList();
                        lstAlumnos = (from al in db.GrupoAlumno
                                      join ale in db.AlumnoInscrito on al.AlumnoId equals ale.AlumnoId
                                      where
                                      ale.EsEmpresa == true
                                      && ale.OfertaEducativa.OfertaEducativaTipoId != 4
                                      && al.GrupoId == GrupoId
                                      && al.EstatusId != 3
                                      select new DTOAlumnoEspecial
                                      {
                                          AlumnoId = ale.AlumnoId,
                                          Anio = ale.Anio,
                                          Nombre = ale.Alumno.Nombre + " " + ale.Alumno.Paterno + " " + ale.Alumno.Materno,
                                          OfertaEducativaId = ale.OfertaEducativaId,
                                          OfertaEducativaS = ale.OfertaEducativa.Descripcion,
                                          PagoPlanId = (int)ale.PagoPlanId,
                                          PeriodoId = ale.PeriodoId,
                                          Estatus = ale.EstatusId,

                                          AlumnoCuota = db.GrupoAlumnoConfiguracion
                                                 .Where(ac =>
                                                 ac.AlumnoId == al.AlumnoId
                                                 && ac.GrupoId == al.GrupoId).
                                             Select(k => new DTOGrupoAlumnoCuota
                                             {
                                                 AlumnoId = k.AlumnoId,
                                                 OfertaEducativaId = k.OfertaEducativaId,
                                                 CuotaColegiatura = k.CuotaColegiatura,
                                                 CuotaCongelada = k.EsCuotaCongelada,
                                                 CuotaInscripcion = k.CuotaInscripcion,
                                                 InscripcionCongelada = k.EsInscripcionCongelada,
                                                 EsEspecial = k.EsEspecial,
                                                 UsuarioId = k.UsuarioId
                                             }).FirstOrDefault()
                                      }
                                     ).ToList();





                    }
                    else
                    {
                        //&& k.Anio == db.AlumnoInscrito.Where(c => c.AlumnoId == k.AlumnoId).ToList().Max(d => d.Anio)
                        //&& k.PeriodoId == db.AlumnoInscrito.Where(c => c.Anio == (db.AlumnoInscrito.Where(d => d.AlumnoId == k.AlumnoId).ToList().Max(e => e.Anio))).ToList().Max(b => b.PeriodoId)
                        lstAlumnos = db.AlumnoInscrito.Where(al =>
                                     al.EsEmpresa == true
                                     && al.OfertaEducativa.OfertaEducativaTipoId != 4
                                     && al.EstatusId == 1)
                                     .Select(al => new DTOAlumnoEspecial
                                     {
                                         AlumnoId = al.AlumnoId,
                                         Anio = al.Anio,
                                         Nombre = al.Alumno.Nombre + " " + al.Alumno.Paterno + " " + al.Alumno.Materno,
                                         OfertaEducativaId = al.OfertaEducativaId,
                                         OfertaEducativaS = al.OfertaEducativa.Descripcion,
                                         PagoPlanId = (int)al.PagoPlanId,
                                         PeriodoId = al.PeriodoId,
                                         Estatus = al.EstatusId,
                                         
                                         AlumnoCuota = al.Alumno
                                             .GrupoAlumnoConfiguracion
                                                 .Where(ac =>
                                                 ac.OfertaEducativaId == al.OfertaEducativaId
                                                 && ac.EstatusId == 1).
                                             Select(k => new DTOGrupoAlumnoCuota
                                             {
                                                 AlumnoId = k.AlumnoId,
                                                 OfertaEducativaId = k.OfertaEducativaId,
                                                 CuotaColegiatura = k.CuotaColegiatura,
                                                 CuotaCongelada = k.EsCuotaCongelada,
                                                 CuotaInscripcion = k.CuotaInscripcion,
                                                 InscripcionCongelada = k.EsInscripcionCongelada,
                                                 EsEspecial = k.EsEspecial,
                                                 UsuarioId = k.UsuarioId
                                             }).FirstOrDefault()
                                     }).AsNoTracking()
                                     .OrderByDescending(K => K.AlumnoId)
                                     .ToList();
                    }

                    
                }
                catch
                {  }


            }
        }
    }
}
