﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using System.Data.Entity;
using System.Globalization;


namespace BLL
{
    public class BLLAlumnoPermitido
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");
        static CultureInfo ci = CultureInfo.InvariantCulture;
        public static object InsertarAlumno(int AlumnoId, int UsuarioId, string Comentario)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DateTime fHoy = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    if (fHoy.Month == 4 || fHoy.Month == 8 || fHoy.Month == 12)
                    { fHoy.AddMonths(1); }

                    Periodo objPer = db.Periodo
                                    .Where(p => fHoy >= p.FechaInicial
                                            && fHoy <= p.FechaFinal)
                                    .FirstOrDefault();

                    db.AlumnoPermitido.Add(new AlumnoPermitido
                    {
                        AlumnoId = AlumnoId,
                        UsuarioId = UsuarioId,
                        Anio = objPer.Anio,
                        PeriodoId = objPer.PeriodoId,
                        FechaRegistro = DateTime.Now,
                        HoraRegistro = DateTime.Now.TimeOfDay,
                        Descripcion = Comentario
                    });
                    db.SaveChanges();

                    return
                    db.AlumnoPermitido
                        .Where(a => a.AlumnoId == AlumnoId)
                        .ToList()
                        .AsQueryable()
                        .Select(b => new
                        {
                            b.AlumnoId,
                            b.UsuarioId,
                            b.Anio,
                            b.PeriodoId,
                            FechaRegistroS = b.FechaRegistro.ToString("dd/MM/yyyy", Cultura),
                            HoraRegistroS = b.HoraRegistro.ToString(),
                            b.Descripcion
                        })
                        .ToList();
                }
                catch(Exception error)
                {
                    return new
                    {
                        error.Message,
                        Inner = error?.InnerException?.Message ?? "",
                        Inner2 = error?.InnerException?.InnerException?.Message
                    };
                }
            }
        }

        public static List<DTOAlumnoPermitido> RegistrosdeAlumno(int AlumnoId)
        {
            using(UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<DTOAlumnoPermitido> ListaPermitido = (from b in db.AlumnoPermitido
                                                             where b.AlumnoId == AlumnoId
                                                             select new DTOAlumnoPermitido
                                                             {
                                                                 AlumnoId = b.AlumnoId,
                                                                 UsuarioId = b.UsuarioId,
                                                                 Anio = (int)b.Anio,
                                                                 PeriodoId = (int)b.PeriodoId,
                                                                 FechaRegistro = b.FechaRegistro,
                                                                 HoraRegistro = b.HoraRegistro,
                                                                 Descripcion = b.Descripcion
                                                             }).ToList();

                    ListaPermitido.ForEach(c =>
                    {
                        c.FechaRegistroS = c.FechaRegistro.ToString("dd/MM/yyyy", Cultura);
                        c.HoraRegistroS = c.HoraRegistro.ToString();
                    });

                    return ListaPermitido;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
