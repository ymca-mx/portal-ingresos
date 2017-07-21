using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using Herramientas;

namespace BLL
{
    public class BLLOfertaEducativaPortal
    {
        public static List<DTOOfertaEducativa> ConsultarOfertasEducativas(int TipoOferta,int Plantel)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                bool Sucursal = (from s in db.Sucursal
                                 where s.SucursalId == Plantel
                                 select s.EsSucursal).FirstOrDefault();
                if (Sucursal == true)
                {
                    return (from a in db.OfertaEducativa
                            where a.OfertaEducativaTipoId == TipoOferta 
                                    && a.SucursalId == Plantel && a.EstatusId==1
                            select a).ToList().ConvertAll(new Converter<OfertaEducativa, DTOOfertaEducativa>(Convertidor.ToDTOOfertaEducativa));
                }
                else
                {
                    return (from a in db.OfertaEducativa
                            where a.OfertaEducativaTipoId == TipoOferta
                                    && a.EstatusId==1
                            select a).ToList().ConvertAll(new Converter<OfertaEducativa, DTOOfertaEducativa>(Convertidor.ToDTOOfertaEducativa));
                }
            }
        }
        public static DTOOfertaEducativa ConsultarOfertaEducativa(int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.OfertaEducativa
                        where a.OfertaEducativaId == OfertaEducativaId
                        select new DTOOfertaEducativa
                        {
                            OfertaEducativaId = a.OfertaEducativaId,
                            OfertaEducativaTipoId = a.OfertaEducativaTipoId,
                            Descripcion = a.Descripcion,
                            Rvoe = a.Rvoe,
                            FechaRvoe = a.FechaRvoe.ToString(),
                            EstatusId = a.EstatusId,
                            SucursalId=a.SucursalId
                        }).FirstOrDefault();
            }
        }
        public static string BuscarOfertaEnAlumnos(int AlumnoId,int OfertaId,DTOPeriodo objPeriodo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTOAlumnoInscrito objAlIn = (from a in db.AlumnoInscrito
                                             where a.AlumnoId==AlumnoId && a.OfertaEducativaId==OfertaId && a.Anio==objPeriodo.Anio && a.PeriodoId==objPeriodo.PeriodoId
                                             select new DTOAlumnoInscrito
                                             {
                                                 AlumnoId=a.AlumnoId,
                                                 Anio=a.Anio,
                                                 FechaInscripcion=a.FechaInscripcion,
                                                 OfertaEducativaId=a.OfertaEducativaId,
                                                 PagoPlanId=a.PagoPlanId,
                                                 PeriodoId=a.PeriodoId,
                                                 TurnoId=a.TurnoId,
                                                 UsuarioId=a.UsuarioId
                                             }).FirstOrDefault();
                if (objAlIn == null)
                { return "0"; }
                else { return "1"; }

            }
        }

        public static List<DTOOfertaEducativa> OfertaEducativaAlumno(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return (from a in db.AlumnoInscrito
                            where a.AlumnoId == AlumnoId
                            select new DTOOfertaEducativa
                        {
                            OfertaEducativaId = a.OfertaEducativa.OfertaEducativaId,
                            OfertaEducativaTipoId = a.OfertaEducativa.OfertaEducativaTipoId,
                            Descripcion = a.OfertaEducativa.Descripcion,
                            Rvoe = a.OfertaEducativa.Rvoe,
                            FechaRvoe = a.OfertaEducativa.FechaRvoe.ToString(),
                            EstatusId = a.OfertaEducativa.EstatusId,
                            SucursalId = a.OfertaEducativa.SucursalId
                        }).ToList();
                }
                catch(Exception)
                {return null;}
            }
        }
        public static List<DTOOfertaEducativa> OfertaAlumno(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DTOPeriodo PeriodoActual = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);
                    List<DTOOfertaEducativa> OfertasAlumnos = (from a in db.AlumnoInscrito
                                                          where a.AlumnoId == AlumnoId && a.OfertaEducativa.OfertaEducativaTipoId != 4
                                                          && a.PeriodoId == PeriodoActual.PeriodoId && a.Anio == PeriodoActual.Anio
                                                          select new DTOOfertaEducativa
                                                          {
                                                              OfertaEducativaId = a.OfertaEducativa.OfertaEducativaId,
                                                              OfertaEducativaTipoId = a.OfertaEducativa.OfertaEducativaTipoId,
                                                              Descripcion = a.OfertaEducativa.Descripcion
                                                          }).ToList();
                    if (OfertasAlumnos.Count == 0)
                    {
                        OfertasAlumnos = (from a in db.AlumnoInscrito
                                     where a.AlumnoId == AlumnoId && a.OfertaEducativa.OfertaEducativaTipoId != 4
                                     select new DTOOfertaEducativa
                                     {
                                         OfertaEducativaId = a.OfertaEducativa.OfertaEducativaId,
                                         OfertaEducativaTipoId = a.OfertaEducativa.OfertaEducativaTipoId,
                                         Descripcion = a.OfertaEducativa.Descripcion
                                     }).ToList();
                    }
                    List<DTOOfertaEducativa> OfertasAgrupadas = OfertasAlumnos.GroupBy(a => a.OfertaEducativaId).
                        Select(s => s.FirstOrDefault()).
                        ToList();
                        

                    List<DTOOfertaEducativa> ListaFinal = new List<DTOOfertaEducativa>();
                    OfertasAgrupadas.ForEach(a =>
                    {
                        DTOOfertaEducativa obja=(DTOOfertaEducativa) a;
                        ListaFinal.Add(new DTOOfertaEducativa
                        {
                            OfertaEducativaId = obja.OfertaEducativaId,
                            OfertaEducativaTipoId = obja.OfertaEducativaTipoId,
                            Descripcion = obja.Descripcion
                        });
                    });

                    return ListaFinal;
                }
                catch (Exception)
                { return null; }
            }
        }

        public static DTOOfertaEducativa TraerMaestriaRelacionada(int EspecialidadId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return db.EspecialidadMaestriaRelacion
                            .Where(a => a.EspecialidadId == EspecialidadId)
                            .Select(a => new DTOOfertaEducativa
                            {
                                OfertaEducativaId = a.OfertaEducativa1.OfertaEducativaId,
                                Descripcion = a.OfertaEducativa1.Descripcion
                            }).FirstOrDefault();
                }
                catch { return null; }
            }
        }
    }
}
