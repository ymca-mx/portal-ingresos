﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data.Entity;
using System.Globalization;

namespace BLL
{
    public class BLLBeca
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");

        public static object ObtenerAlumno(int AlumnoId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    //DTOPeriodo objPEr = BLLPeriodo.TraerPeriodoEntreFechas(DateTime.Now);
                    DTOAlumnoBecaComite Alumno = db.Alumno.Where(a => a.AlumnoId == AlumnoId).Select(a => new DTOAlumnoBecaComite
                    {
                        AlumnoId = a.AlumnoId,
                        Nombre = a.Nombre + " " + a.Paterno + " " + a.Materno,
                    }).FirstOrDefault();

                    Alumno.lstDescuentos = new List<DTOAlumnoDescuento>();
                    Alumno.PeriodosAlumno = new List<PeriodoBeca>();

                    #region Group by Ofertas Alumnos
                    IEnumerable<DTOAlumnoOfertas> OfertasAlumno = db.Pago.Where(s => s.AlumnoId == AlumnoId
                                                     && (s.Cuota1.PagoConceptoId == 800
                                                     || s.Cuota1.PagoConceptoId == 802)
                                                     && s.EstatusId != 2)
                                                     .ToList()
                                                     .GroupBy(s => new { s.AlumnoId, s.OfertaEducativaId, s.EsEmpresa })
                                                     .Select(s => new DTOAlumnoOfertas
                                                     {
                                                         OfertaEducativaId = s.Key.OfertaEducativaId,
                                                         Descripcion = "",
                                                     });

                    Alumno.OfertasAlumnos = (from a in OfertasAlumno
                                             select new DTOAlumnoOfertas
                                             {
                                                 Descripcion = a.Descripcion,
                                                 OfertaEducativaId = a.OfertaEducativaId,
                                                 EsEmpresa = db.AlumnoInscrito
                                                                .Where(b => b.AlumnoId == AlumnoId 
                                                                        && b.OfertaEducativaId == a.OfertaEducativaId)?
                                                                        .FirstOrDefault()?
                                                                        .EsEmpresa ?? false
                                             }).ToList();
                    #endregion

                    Alumno.OfertasAlumnos.ForEach(s2 =>
                    {

                        #region Group By Periodo de Ofertas
                        s2.Descripcion = db.OfertaEducativa.Where(o => o.OfertaEducativaId == s2.OfertaEducativaId).FirstOrDefault().Descripcion;

                        IEnumerable<PeriodoBeca> OfertasAlumnoAnioPeriodo = db.Pago.Where(s => s.AlumnoId == AlumnoId
                                                    && (s.Cuota1.PagoConceptoId == 800
                                                    || s.Cuota1.PagoConceptoId == 802)
                                                    && s.EstatusId != 2
                                                    && s.OfertaEducativaId == s2.OfertaEducativaId)
                                                    .ToList()
                                                    .GroupBy(d => new { d.Anio, d.PeriodoId })
                                                    .Select(d => new PeriodoBeca
                                                    {
                                                        Anio = d.Key.Anio,
                                                        OfertaEducativaId = s2.OfertaEducativaId,
                                                        PeriodoId = d.Key.PeriodoId,
                                                        Descripcion = "",
                                                        
                                                    });

                        Alumno.PeriodosAlumno.AddRange((from a in OfertasAlumnoAnioPeriodo
                                                        select new PeriodoBeca
                                                        {
                                                            Anio = a.Anio,
                                                            Descripcion = a.Descripcion,
                                                            OfertaEducativaId = a.OfertaEducativaId,
                                                            PeriodoId = a.PeriodoId,
                                                            
                                                        }).ToList());

                        Alumno.PeriodosAlumno.ForEach(o =>
                        {
                            List<AlumnoInscrito> AlumnoInscrito = db.AlumnoInscrito.Where(ai =>
                                      ai.AlumnoId == AlumnoId
                                      && ai.Anio == o.Anio
                                      && ai.OfertaEducativaId == o.OfertaEducativaId
                                      && ai.PeriodoId == o.PeriodoId
                                      && ai.EsEmpresa == true).ToList();
                            if (AlumnoInscrito.Count == 0)
                            {
                                List<AlumnoInscritoBitacora> AlumnoInscrito1 = db.AlumnoInscritoBitacora.Where(ai =>
                                            ai.AlumnoId == AlumnoId
                                          && ai.Anio == o.Anio
                                          && ai.OfertaEducativaId == o.OfertaEducativaId
                                          && ai.PeriodoId == o.PeriodoId
                                          && ai.EsEmpresa == true).ToList();
                                o.EsEmprea = AlumnoInscrito1.Count > 0 ? true : false;
                            }
                            else
                            { o.EsEmprea = true; }

                            o.Descripcion = db.Periodo.Where(i => i.Anio == o.Anio && i.PeriodoId == o.PeriodoId).FirstOrDefault().Descripcion;

                            List<AlumnoInscritoDocumento> lstDocumentosAlumno =
                                db.AlumnoInscritoDocumento.Where(doc =>
                                                                      doc.AlumnoId == AlumnoId
                                                                      && doc.OfertaEducativaId == o.OfertaEducativaId
                                                                      && doc.Anio == o.Anio
                                                                      && doc.PeriodoId == o.PeriodoId).ToList();
                            #region Descuentos 
                            AlumnoDescuento DescuentoAlumno = db.AlumnoDescuento.Where(ad => ad.AlumnoId == AlumnoId
                                                                                   && ad.Anio == o.Anio
                                                                                   && ad.PeriodoId == o.PeriodoId
                                                                                   && ad.OfertaEducativaId == s2.OfertaEducativaId
                                                                                   && ad.PagoConceptoId == 800
                                                                                   && ad.EstatusId == 2
                                                                                   && ad.Monto > 0).FirstOrDefault();
                            if (DescuentoAlumno != null)
                            {
                                Alumno.lstDescuentos.Add(new DTOAlumnoDescuento
                                {
                                    DescripcionPeriodo = o.Descripcion,
                                    AlumnoId = AlumnoId,
                                    Anio = o.Anio,
                                    PeriodoId = o.PeriodoId,
                                    AnioPeriodoId = o.Anio + " - " + o.PeriodoId,
                                    Usuario = new DTOUsuario
                                    {
                                        Nombre = DescuentoAlumno.Usuario.Nombre,
                                        UsuarioId = DescuentoAlumno.UsuarioId
                                    },
                                    Monto = DescuentoAlumno.Monto,
                                    BecaComite = DescuentoAlumno.EsComite == true ? "Si" : "No",
                                    BecaDeportiva = DescuentoAlumno.EsDeportiva == true ? "Si" : "No",
                                    BecaSEP = DescuentoAlumno.EsSEP == true ? "Si" : "No",
                                    FechaAplicacionS = (DescuentoAlumno.FechaAplicacion.Value.Day < 10 ?
                                                        "0" + DescuentoAlumno.FechaAplicacion.Value.Day :
                                                            DescuentoAlumno.FechaAplicacion.Value.Day.ToString()) + "/" +
                                                        (DescuentoAlumno.FechaAplicacion.Value.Month < 10 ?
                                                        "0" + DescuentoAlumno.FechaAplicacion.Value.Month :
                                                            DescuentoAlumno.FechaAplicacion.Value.Month.ToString()) + "/" +
                                                        DescuentoAlumno.FechaAplicacion.Value.Year.ToString(),
                                    OfertaEducativaId = s2.OfertaEducativaId,
                                    SMonto = DescuentoAlumno.Monto.ToString(),
                                    DocAcademicaId = (lstDocumentosAlumno?
                                                        .Where(a => a.TipoDocumento == 1)?
                                                        .FirstOrDefault()?
                                                        .AlumnoInscritoDocumentoId.ToString() ?? ""),

                                    DocComiteRutaId = (lstDocumentosAlumno?
                                                            .Where(a => a.TipoDocumento == 2)?
                                                            .FirstOrDefault()?
                                                            .AlumnoInscritoDocumentoId.ToString() ?? "")

                                });
                            }
                            #endregion
                        });
                        #endregion

                    });

                    return new
                    {
                        Alumno,
                        Status = true
                    };
                }
                catch (Exception error)
                {
                    return new
                    {
                        Status = false,
                        error.Message,
                        Inner = error?.InnerException?.Message,
                        Inner2 = error?.InnerException?.InnerException?.Message
                    };
                }
            }
        }


        public static string VerificarInscripcionActual(int AlumnoId, int OfertaEducativaId, int Anio, int PeriodoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<AlumnoInscrito> AlumnoInscrito = db.AlumnoInscrito
                                    .Where(k => k.AlumnoId == AlumnoId
                                        && k.OfertaEducativaId == OfertaEducativaId
                                        && k.PeriodoId == PeriodoId
                                        && k.Anio == Anio).ToList();
                List<AlumnoInscritoBitacora> AlumnoInscritoBitacora = db.AlumnoInscritoBitacora
                                    .Where(k => k.AlumnoId == AlumnoId
                                        && k.OfertaEducativaId == OfertaEducativaId
                                        && k.PeriodoId == PeriodoId
                                        && k.Anio == Anio).ToList();
                if (AlumnoInscrito.Count > 0 || AlumnoInscritoBitacora.Count > 0)
                { return "Procede"; }
                else { return "No tiene"; }
            }
        }

        public static DTOAlumnoBecaDeportiva ObtenerAlumnoDeportiva(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    PeriodoBeca PeriodoActual = db.Periodo.Where(m => m.FechaFinal > DateTime.Today).Take(1).Select(d => new PeriodoBeca
                    {
                        Anio = d.Anio,
                        PeriodoId = d.PeriodoId,
                        Descripcion = d.Descripcion
                    }).FirstOrDefault();

                    DTOAlumnoBecaDeportiva AlumnoDeportiva = db.Alumno.Where(a => a.AlumnoId == AlumnoId).Select(a => new DTOAlumnoBecaDeportiva
                    {
                        AlumnoId = a.AlumnoId,
                        Nombre = a.Nombre + " " + a.Paterno + " " + a.Materno,
                    }).FirstOrDefault();

                    AlumnoDeportiva.lstDescuentos = new List<DTOAlumnoDescuento>();
                    AlumnoDeportiva.PeriodosAlumno = new List<PeriodoBeca>();

                    AlumnoDeportiva.OfertasAlumnos = (db.Pago.Where(s => s.AlumnoId == AlumnoId
                                                    && (s.Cuota1.PagoConceptoId == 800
                                                    || s.Cuota1.PagoConceptoId == 802)
                                                    && s.EstatusId != 2)
                                                    .ToList()
                                                    .GroupBy(z => new { z.AlumnoId, z.OfertaEducativaId })
                                                    .Select(x => new DTOAlumnoOfertas
                                                    {
                                                        OfertaEducativaId = x.Key.OfertaEducativaId,
                                                        Descripcion = ""
                                                    } ) ).ToList();

                    AlumnoDeportiva.OfertasAlumnos.ForEach(s2 =>
                    {
                        s2.Descripcion = db.OfertaEducativa.Where(o => o.OfertaEducativaId == s2.OfertaEducativaId).FirstOrDefault().Descripcion;
                        s2.Mensaje = VerificarInscripcionActual(AlumnoId,s2.OfertaEducativaId, PeriodoActual.Anio, PeriodoActual.PeriodoId);
                    });




                    AlumnoDeportiva.PeriodosAlumno.Add(db.Periodo.Where(m => m.FechaFinal > DateTime.Today).Take(2).Select(d => new PeriodoBeca
                                               {
                                                   Anio = d.Anio,
                                                   PeriodoId = d.PeriodoId,
                                                   Descripcion = d.Descripcion,
                                               }).FirstOrDefault());

                            List<DTOAlumnoDescuento> Descuento = db.AlumnoDescuento.Where(ad => ad.AlumnoId == AlumnoId
                                                                                   && ad.Anio > 2016
                                                                                   && ad.EstatusId == 2
                                                                                   && ad.PagoConceptoId == 800 ).Select(p => new DTOAlumnoDescuento
                                                                                   {
                                                                                       AlumnoDescuentoId = p.AlumnoDescuentoId,
                                                                                       AlumnoId = AlumnoId,
                                                                                       AnioPeriodoId = p.Anio + " - " + p.PeriodoId,
                                                                                       DescripcionPeriodo = p.Periodo.Descripcion,
                                                                                       Usuario = db.Usuario.Where(q=> q.UsuarioId== p.UsuarioId).Select(m=>  new DTOUsuario
                                                                                       {
                                                                                           Nombre = m.Nombre
                                                                                       }).FirstOrDefault(),
                                                                                       Monto = p.Monto,
                                                                                       BecaDeportiva = p.EsDeportiva == true ? p.Monto.ToString() + "%" : "No",
                                                                                       BecaSEP = p.EsSEP == true ? p.Monto.ToString() + "%" : "No",
                                                                                       BecaComite = p.EsComite == true ? p.Monto.ToString() + "%" : "No",
                                                                                       OfertaEducativaId = p.OfertaEducativaId,
                                                                                       FechaAplicacion = p.FechaAplicacion,
                                                                                       FechaAplicacionS = (((DateTime) p.FechaAplicacion).Day.ToString().Length > 1 ? ((DateTime) p.FechaAplicacion).Day.ToString() : "0" + ((DateTime) p.FechaAplicacion).Day.ToString()) + "/" +
                                                                                       (((DateTime) p.FechaAplicacion).Month.ToString().Length > 1 ?((DateTime) p.FechaAplicacion).Month.ToString() : "0" + ((DateTime) p.FechaAplicacion).Month.ToString()) + "/" + (((DateTime) p.FechaAplicacion).Year.ToString()),
                                                                                       DocComiteRutaId = p.EsDeportiva == true ? db.AlumnoInscritoDocumento.Where(doc =>
                                                                                                                                                  doc.AlumnoId == AlumnoId
                                                                                                                                                  && doc.Anio == p.Anio
                                                                                                                                                  && doc.PeriodoId == p.PeriodoId
                                                                                                                                                  && doc.TipoDocumento == 3).OrderByDescending(q=> q.AlumnoInscritoDocumentoId).FirstOrDefault().AlumnoInscritoDocumentoId.ToString(): "",

                                                                                   }
                                                                                       ).ToList();


                    

                            decimal MontoDeportiva = 0;
                    Descuento.ForEach(n => 
                            {
                             
                             if (n.BecaSEP != "No") { n.SMonto = "Beca Sep - " + n.BecaSEP;}
                             else if (n.BecaComite != "No") { n.SMonto = "Beca Comite - " + n.BecaComite;  }
                             else if (n.BecaDeportiva != "No") { n.SMonto = "Beca Deportiva - " + n.BecaDeportiva; MontoDeportiva = n.Monto;  }
                             else { n.SMonto = "Beca Academica - " + n.Monto.ToString() + "%"; }
                             n.MontoDep = MontoDeportiva;
                            
                            });
                            
                            if (Descuento != null)
                            {
                        AlumnoDeportiva.lstDescuentos.AddRange(Descuento);
                            }

                    return AlumnoDeportiva;
                }
                catch { return null; }
            }
        }

        public static DTOAlumnoBecaComite ObtenerAlumnoSEP(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    //DTOPeriodo objPEr = BLLPeriodo.TraerPeriodoEntreFechas(DateTime.Now);
                    DTOAlumnoBecaComite objCom = db.Alumno.Where(a => a.AlumnoId == AlumnoId).Select(a => new DTOAlumnoBecaComite
                    {
                        AlumnoId = a.AlumnoId,
                        Nombre = a.Nombre + " " + a.Paterno + " " + a.Materno,
                    }).FirstOrDefault();

                    objCom.lstDescuentos = new List<DTOAlumnoDescuento>();
                    objCom.PeriodosAlumno = new List<PeriodoBeca>();

                    #region Group by Ofertas Alumnos
                    var oferaPa = db.Pago.Where(s => s.AlumnoId == AlumnoId
                                                    && (s.Cuota1.PagoConceptoId == 800
                                                    || s.Cuota1.PagoConceptoId == 802)
                                                    && s.EstatusId != 2)
                                                    .ToList()
                                                    .GroupBy(s => new { s.AlumnoId, s.OfertaEducativaId })
                                                    .Select(s => new DTOAlumnoOfertas
                                                    {
                                                        OfertaEducativaId = s.Key.OfertaEducativaId,
                                                        Descripcion = ""
                                                    });

                    objCom.OfertasAlumnos = (from a in oferaPa
                                             select new DTOAlumnoOfertas
                                             {
                                                 Descripcion = a.Descripcion,
                                                 OfertaEducativaId = a.OfertaEducativaId
                                             }).ToList();
                    #endregion

                    objCom.OfertasAlumnos.ForEach(s2 =>
                    {
                        #region Group By Periodo de Ofertas
                        s2.Descripcion = db.OfertaEducativa.Where(o => o.OfertaEducativaId == s2.OfertaEducativaId).FirstOrDefault().Descripcion;

                        var oferaPaPeriod = db.Pago.Where(s => s.AlumnoId == AlumnoId
                                                    && (s.Cuota1.PagoConceptoId == 800
                                                    || s.Cuota1.PagoConceptoId == 802)
                                                    && s.EstatusId != 2
                                                    && s.OfertaEducativaId == s2.OfertaEducativaId)
                                                    .ToList()
                                                    .GroupBy(d => new { d.Anio, d.PeriodoId })
                                                    .Select(d => new PeriodoBeca
                                                    {
                                                        Anio = d.Key.Anio,
                                                        OfertaEducativaId = s2.OfertaEducativaId,
                                                        PeriodoId = d.Key.PeriodoId,
                                                        Descripcion = ""
                                                    });

                        objCom.PeriodosAlumno.AddRange((from a in oferaPaPeriod
                                                        select new PeriodoBeca
                                                        {
                                                            Anio = a.Anio,
                                                            Descripcion = a.Descripcion,
                                                            OfertaEducativaId = a.OfertaEducativaId,
                                                            PeriodoId = a.PeriodoId,

                                                        }).ToList());

                        objCom.PeriodosAlumno.ForEach(o =>
                        {
                            List<AlumnoInscrito> lstAl = db.AlumnoInscrito.Where(ai =>
                                      ai.AlumnoId == AlumnoId
                                      && ai.Anio == o.Anio
                                      && ai.OfertaEducativaId == o.OfertaEducativaId
                                      && ai.PeriodoId == o.PeriodoId
                                      && ai.EsEmpresa == true).ToList();
                            if (lstAl.Count == 0)
                            {
                                List<AlumnoInscritoBitacora> lstAl1 = db.AlumnoInscritoBitacora.Where(ai =>
                                            ai.AlumnoId == AlumnoId
                                          && ai.Anio == o.Anio
                                          && ai.OfertaEducativaId == o.OfertaEducativaId
                                          && ai.PeriodoId == o.PeriodoId
                                          && ai.EsEmpresa == true).ToList();
                                o.EsEmprea = lstAl1.Count > 0 ? true : false;
                            }
                            else
                            { o.EsEmprea = true; }

                            o.Descripcion = db.Periodo.Where(i => i.Anio == o.Anio && i.PeriodoId == o.PeriodoId).FirstOrDefault().Descripcion;

                            List<AlumnoInscritoDocumento> lstDocumentosAlumno =
                                db.AlumnoInscritoDocumento.Where(doc =>
                                                                      doc.AlumnoId == AlumnoId
                                                                      && doc.OfertaEducativaId == o.OfertaEducativaId
                                                                      && doc.Anio == o.Anio
                                                                      && doc.PeriodoId == o.PeriodoId).ToList();
                            #region Descuentos 
                            AlumnoDescuento objDesc = db.AlumnoDescuento.Where(ad => ad.AlumnoId == AlumnoId
                                                                                   && ad.Anio == o.Anio
                                                                                   && ad.PeriodoId == o.PeriodoId
                                                                                   && ad.OfertaEducativaId == s2.OfertaEducativaId
                                                                                   && ad.PagoConceptoId == 800
                                                                                   && ad.EstatusId == 2
                                                                                   && ad.Monto > 0).FirstOrDefault();
                            if (objDesc != null)
                            {
                                objCom.lstDescuentos.Add(new DTOAlumnoDescuento
                                {
                                    DescripcionPeriodo = o.Descripcion,
                                    AlumnoId = AlumnoId,
                                    Anio = o.Anio,
                                    PeriodoId = o.PeriodoId,
                                    AnioPeriodoId = o.Anio + " - " + o.PeriodoId,
                                    Usuario = new DTOUsuario
                                    {
                                        Nombre = objDesc.Usuario.Nombre,
                                        UsuarioId = objDesc.UsuarioId
                                    },
                                    Monto = objDesc.Monto,
                                    BecaComite = objDesc.EsComite == true ? "Si" : "No",
                                    BecaDeportiva = objDesc.EsDeportiva == true ? "Si" : "No",
                                    BecaSEP = objDesc.EsSEP == true ? "Si" : "No",
                                    FechaAplicacionS = (objDesc.FechaAplicacion.Value.Day < 10 ?
                                                        "0" + objDesc.FechaAplicacion.Value.Day :
                                                            objDesc.FechaAplicacion.Value.Day.ToString()) + "/" +
                                                        (objDesc.FechaAplicacion.Value.Month < 10 ?
                                                        "0" + objDesc.FechaAplicacion.Value.Month :
                                                            objDesc.FechaAplicacion.Value.Month.ToString()) + "/" +
                                                        objDesc.FechaAplicacion.Value.Year.ToString(),
                                    OfertaEducativaId = s2.OfertaEducativaId,
                                    SMonto = objDesc.Monto.ToString(),
                                    DocAcademicaId = lstDocumentosAlumno.Count > 0 ?
                                                        lstDocumentosAlumno.Where(a => a.TipoDocumento == 1).ToList().Count > 0 ?
                                                            lstDocumentosAlumno.Where(a => a.TipoDocumento == 1).FirstOrDefault().AlumnoInscritoDocumentoId.ToString()
                                                            : string.Empty
                                                        : string.Empty,

                                    DocComiteRutaId = lstDocumentosAlumno.Count > 0 ?
                                                        lstDocumentosAlumno.Where(a => a.TipoDocumento == 2).ToList().Count > 0 ?
                                                            lstDocumentosAlumno.Where(a => a.TipoDocumento == 2).FirstOrDefault().AlumnoInscritoDocumentoId.ToString()
                                                            : string.Empty
                                                        : string.Empty,
                                });
                            }
                            #endregion
                        });
                        #endregion

                    });

                    return objCom;
                }
                catch { return null; }
            }
        }

    }




}
