using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using System.Data.Entity;
using DTO.Reinscripcion;

namespace BLL
{
    public class BLLReinscripcion
    {
        public static DTOMateriasAsesorias TraerAlumno(int AlumnoId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    DTOMateriasAsesorias objMAS = new DTOMateriasAsesorias();
                    objMAS.lstPeriodos = new List<DTOPeriodo>();
                    objMAS.lstOfertas = new List<DTOOfertaEducativa>();
                    objMAS.Referencias = new List<dtoReferenciasReinsc>();
                    objMAS.Cuotas = new List<dtoCuotaReinc>();
                    objMAS.EstatusAl = new List<dtoEstatusMA>();
                    //periodo
                    DateTime fhoy = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    //DateTime fhoy = new DateTime(2017, 4, 16);
                    DateTime fprev = fhoy.AddDays(15);
                    objMAS.lstPeriodos.AddRange((from a in db.Periodo
                                                 where fhoy >= a.FechaInicial && fhoy <= a.FechaFinal
                                                      || (fprev >= a.FechaInicial && fprev <= a.FechaFinal)
                                                 select new DTOPeriodo
                                                 {
                                                     Descripcion = a.Descripcion,
                                                     Anio = a.Anio,
                                                     PeriodoId = a.PeriodoId,
                                                 }).ToList());

                    objMAS.lstPeriodos
                            .ForEach(l => {
                                l.Descripcion = db.Periodo.Where(k => k.Anio == l.Anio && k.PeriodoId == l.PeriodoId).FirstOrDefault().Descripcion;
                            });

                    //Alumno
                    Alumno objAlumno = db.Alumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault();
                    objMAS.AlumnoId = AlumnoId;
                    objMAS.Nombre = objAlumno.Nombre + " " + objAlumno.Paterno + " " + objAlumno.Materno;

                    #region Dejar ofertas mas altas
                    List<AlumnoInscrito> Mayores = new List<AlumnoInscrito>{ objAlumno.AlumnoInscrito
                                                            .Where(o => o.OfertaEducativa.OfertaEducativaTipoId != 4)
                                                            .OrderByDescending(O => O.Anio)
                                                            .ThenByDescending(O => O.PeriodoId)
                                                            .Select(a => a)
                                                            .FirstOrDefault() };
                    Mayores.AddRange(objAlumno.AlumnoInscrito
                                                .Where(M => M.Anio == Mayores.FirstOrDefault().Anio
                                                        && M.PeriodoId == Mayores.FirstOrDefault().PeriodoId
                                                        && M.OfertaEducativaId != Mayores.FirstOrDefault().OfertaEducativaId)
                                                 .ToList());

                    List<AlumnoInscritoBitacora> MayoresB= new List<AlumnoInscritoBitacora>{ objAlumno.AlumnoInscritoBitacora
                                                            .Where(o => o.OfertaEducativa.OfertaEducativaTipoId != 4)
                                                            .OrderByDescending(O => O.Anio)
                                                            .ThenByDescending(O => O.PeriodoId)
                                                            .Select(a => a)
                                                            .FirstOrDefault() };
                    MayoresB.AddRange(objAlumno.AlumnoInscritoBitacora
                                                .Where(M => M.Anio == MayoresB.FirstOrDefault().Anio
                                                        && M.PeriodoId == MayoresB.FirstOrDefault().PeriodoId
                                                        && M.OfertaEducativaId != MayoresB.FirstOrDefault().OfertaEducativaId)
                                                 .ToList());
                    #endregion

                    // Ofertas
                    objMAS.lstOfertas.AddRange(Mayores.Select(i => new DTOOfertaEducativa
                    {
                        OfertaEducativaId = i.OfertaEducativaId,
                        Descripcion = i.OfertaEducativa.Descripcion,
                        OfertaEducativaTipoId = i.OfertaEducativa.OfertaEducativaTipoId,
                        Cuatrimestre = i.Alumno.AlumnoCuatrimestre.Where(f=> f.OfertaEducativaId == i.OfertaEducativaId).FirstOrDefault()?.Cuatrimestre??0,
                        SucursalId=i.OfertaEducativa.SucursalId
                    }));
                    objMAS.lstOfertas.AddRange(MayoresB.Select(i => new DTOOfertaEducativa
                    {
                        OfertaEducativaId = i.OfertaEducativaId,
                        Descripcion = i.OfertaEducativa.Descripcion,
                        Cuatrimestre = i.Alumno.AlumnoCuatrimestre.Where(f => f.OfertaEducativaId == i.OfertaEducativaId).FirstOrDefault()?.Cuatrimestre ?? 0,
                        SucursalId = i.OfertaEducativa.SucursalId
                    }));

                    List<DTOOfertaEducativa> liIDs = new List<DTOOfertaEducativa>();
                    objMAS.lstOfertas.ForEach(o =>
                    {
                        if (liIDs.FindIndex(d => d.OfertaEducativaId == o.OfertaEducativaId) == -1)
                        {
                            if (o.OfertaEducativaTipoId == 2)
                            {
                                List<AlumnoInscritoBitacora> listahitorial = objAlumno
                                                                .AlumnoInscritoBitacora
                                                                .Where(alb => alb.OfertaEducativaId == o.OfertaEducativaId)
                                                                .GroupBy(alb => new { alb.Anio, alb.PeriodoId })
                                                                .Select(alb => alb.FirstOrDefault())
                                                                .ToList();
                                                       
                                List<AlumnoInscrito> listaactual = objAlumno
                                                                .AlumnoInscrito
                                                                .Where(al => al.OfertaEducativaId == o.OfertaEducativaId)
                                                                .GroupBy(al => new { al.Anio, al.PeriodoId })
                                                                .Select(al => al.FirstOrDefault())
                                                                .ToList();
                                listaactual.ForEach(la =>
                                {
                                    listahitorial.RemoveAll(sd => sd.Anio == la.Anio
                                                                && sd.PeriodoId == la.PeriodoId);
                                });

                                o.AplicaMaestria = (listahitorial.Count + listaactual.Count) >= 2 ? true : false;
                            }
                            liIDs.Add(o);
                        }
                    });

                    objMAS.lstOfertas = liIDs;

                    //Referencias 
                    objMAS.lstOfertas.ForEach(o =>
                    {
                        objMAS.lstPeriodos.ForEach(l =>
                        {
                            int numerodepagos = objAlumno.Pago
                                                    .Where(a => a.OfertaEducativaId == o.OfertaEducativaId
                                                            && a.Anio == l.Anio
                                                            && a.PeriodoId == l.PeriodoId
                                                            && (a.EstatusId == 1 || a.EstatusId == 4))
                                                    .ToList()
                                                    .Count;
                            List<dtoEstatusMA> Revision = db.AlumnoRevision
                                                   .Where(i => i.AlumnoId == AlumnoId
                                                             && i.OfertaEducativaId == o.OfertaEducativaId
                                                             && i.Anio == l.Anio
                                                             && i.PeriodoId == l.PeriodoId).Select(k => new dtoEstatusMA
                                                             {
                                                                 Anio = k.Anio,
                                                                 OfertaEducativaId = k.OfertaEducativaId,
                                                                 Periodo = k.PeriodoId
                                                             }).ToList();

                            if (numerodepagos > 0 && Revision.Count > 0)
                            {
                                objMAS.EstatusAl.AddRange(Revision);
                            }
                            objMAS.Cuotas.AddRange(db.Cuota.Where(c => c.Anio == l.Anio
                                                             && c.PeriodoId == l.PeriodoId
                                                             && c.OfertaEducativaId == o.OfertaEducativaId
                                                             && (c.PagoConceptoId == 15
                                                             || c.PagoConceptoId == 320
                                                             || c.PagoConceptoId == 304))
                                                    .Select(ds => new dtoCuotaReinc
                                                    {
                                                        Anio = ds.Anio,
                                                        CuotaId = ds.CuotaId,
                                                        Monto = ds.Monto,
                                                        OfertaEducativaId = ds.OfertaEducativaId,
                                                        PeriodoId = ds.PeriodoId,
                                                        PagoConceptoId = ds.PagoConceptoId
                                                    }).ToList());
                            objMAS.Referencias.AddRange(db.Pago
                                                           .Where(d => d.AlumnoId == AlumnoId
                                                                     && d.OfertaEducativaId == o.OfertaEducativaId
                                                                     && d.Anio == l.Anio
                                                                     && d.PeriodoId == l.PeriodoId
                                                                     && d.EstatusId != 2
                                                                     && (d.Cuota1.PagoConceptoId == 800
                                                                     || d.Cuota1.PagoConceptoId == 802
                                                                     || d.Cuota1.PagoConceptoId == 304
                                                                     || d.Cuota1.PagoConceptoId == 15
                                                                     || d.Cuota1.PagoConceptoId == 320))
                                                             .Select(k => new dtoReferenciasReinsc
                                                             {
                                                                 Anio = k.Anio,
                                                                 Concepto = k.Cuota1.PagoConcepto.Descripcion,
                                                                 Fecha = (k.FechaGeneracion.Day < 10
                                                                        ? "0" + k.FechaGeneracion.Day : k.FechaGeneracion.Day.ToString()) + "/" +
                                                                        (k.FechaGeneracion.Month < 10
                                                                        ? "0" + k.FechaGeneracion.Month : k.FechaGeneracion.Day.ToString()) + "/" +
                                                                        k.FechaGeneracion.Year.ToString(),
                                                                 Monto = k.Cuota.ToString(),
                                                                 OfertaEducativaId = k.OfertaEducativaId,
                                                                 PeriodoId = k.PeriodoId,
                                                                 Referencia = k.ReferenciaId.ToString()
                                                             }).ToList());
                        });
                    });
                    objMAS.Referencias.ForEach(i =>
                    {
                        i.Referencia = int.Parse(i.Referencia).ToString();
                    });
                    objMAS.EstatusAl.ForEach(i =>
                    {
                        i.Estado = "Ya se le dio el Visto Bueno";
                    });

                        return objMAS;
                }catch  {
                    return null;
                }
            }
        }

        public static string Pasar_a_Maestria(int alumnoId, int anio, int periodoId, int especialidadId, int maestriaId, int usuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    AlumnoInscrito alumno = db.AlumnoInscrito
                                            .Where(ai => ai.AlumnoId == alumnoId
                                                    && ai.OfertaEducativaId == especialidadId)
                                            .FirstOrDefault();
                    Usuario usuariodb = db.Usuario.Where(Au => Au.UsuarioId == usuarioId).FirstOrDefault();
                    #region Alumno
                    db.AlumnoBitacora.Add(new AlumnoBitacora
                    {
                        AlumnoId = alumno.Alumno.AlumnoId,
                        Anio = alumno.Alumno.Anio,
                        EstatusId = alumno.Alumno.EstatusId,
                        Fecha = DateTime.Now,
                        FechaRegistro = alumno.Alumno.FechaRegistro,
                        MatriculaId = alumno.Alumno.MatriculaId,
                        Materno = alumno.Alumno.Materno,
                        Nombre = alumno.Alumno.Nombre,
                        Paterno = alumno.Alumno.Paterno,
                        PeriodoId = alumno.Alumno.PeriodoId,
                        UsuarioId = alumno.Alumno.UsuarioId,
                        UsuarioIdBitacora = usuarioId
                    });

                    string RVOE = db.OfertaEducativa.Where(ak => ak.OfertaEducativaId == maestriaId)?.FirstOrDefault()?.Rvoe;
                    alumno.Alumno.Anio = anio;
                    alumno.Alumno.PeriodoId = periodoId;
                    alumno.Alumno.MatriculaId = Herramientas.Matricula.GenerarMatricula(anio, periodoId, alumnoId, RVOE, alumno.TurnoId);

                    db.Matricula.Add(new Matricula
                    {
                        MatriculaId = alumno.Alumno.MatriculaId,
                        AlumnoId = alumnoId,
                        Anio = anio,
                        OfertaEducativaId = maestriaId,
                        PeriodoId = periodoId,
                        FechaAsignacion = DateTime.Now,
                        UsuarioId = usuarioId,                        
                    });
                    #endregion
                    #region Cuatrimestre
                    db.AlumnoCuatrimestre.Add(new AlumnoCuatrimestre
                    {
                        AlumnoId = alumnoId,
                        Anio = anio,
                        esRegular = null,
                        Cuatrimestre = 1,
                        FechaAsignacion = DateTime.Now,
                        HoraAsignacion = DateTime.Now.TimeOfDay,
                        OfertaEducativaId = maestriaId,
                        PeriodoId = periodoId,
                        UsuarioId = usuarioId
                    });
                    #endregion
                    #region AlumnoInscrito && AlumnoRevision
                    AlumnoInscrito AlumnoMaestria = alumno.Alumno.AlumnoInscrito.Where(a => a.OfertaEducativaId == maestriaId).FirstOrDefault();
                    if ((AlumnoMaestria?.AlumnoId ?? null) == null)
                    {
                        alumno.Alumno.AlumnoInscrito.ToList().ForEach(al => { al.EstatusId = al.OfertaEducativa.OfertaEducativaTipoId != 4 ? 2 : al.EstatusId; });
                        db.AlumnoInscrito.Add(new AlumnoInscrito
                        {
                            AlumnoId = alumnoId,
                            Anio = anio,
                            EsEmpresa = alumno.EsEmpresa,
                            EstatusId = 1,
                            FechaInscripcion = DateTime.Now,
                            HoraInscripcion = DateTime.Now.TimeOfDay,
                            OfertaEducativaId = maestriaId,
                            PagoPlanId = db.OfertaEducativaPlan.Where(of => of.OfertaEducativaTipoId == 3 && of.PagoPlan.Pagos == 4).FirstOrDefault().PagoPlanId,
                            PeriodoId = periodoId,
                            TurnoId = alumno.TurnoId,
                            UsuarioId = usuarioId
                        });
                    }
                    else
                    {
                        db.AlumnoInscritoBitacora.Add(new AlumnoInscritoBitacora
                        {
                            AlumnoId = AlumnoMaestria.AlumnoId,
                            Anio = AlumnoMaestria.Anio,
                            EsEmpresa = AlumnoMaestria.EsEmpresa,
                            FechaInscripcion = AlumnoMaestria.FechaInscripcion,
                            HoraInscripcion = AlumnoMaestria.HoraInscripcion,
                            OfertaEducativaId = AlumnoMaestria.OfertaEducativaId,
                            PagoPlanId = AlumnoMaestria.PagoPlanId,
                            PeriodoId = AlumnoMaestria.PeriodoId,
                            TurnoId = AlumnoMaestria.TurnoId,
                            UsuarioId = AlumnoMaestria.UsuarioId
                        });

                        db.AlumnoInscrito.Add(new AlumnoInscrito
                        {
                            AlumnoId = alumnoId,
                            Anio = anio,
                            EsEmpresa = AlumnoMaestria.EsEmpresa,
                            EstatusId = 1,
                            FechaInscripcion = AlumnoMaestria.FechaInscripcion,
                            HoraInscripcion = AlumnoMaestria.HoraInscripcion,
                            OfertaEducativaId = AlumnoMaestria.OfertaEducativaId,
                            PagoPlanId = AlumnoMaestria.PagoPlanId,
                            PeriodoId = AlumnoMaestria.PeriodoId,
                            TurnoId = AlumnoMaestria.TurnoId,
                            UsuarioId = usuarioId
                        });

                        db.AlumnoRevision.Add(new AlumnoRevision
                        {
                            AlumnoId = alumnoId,
                            OfertaEducativaId = maestriaId,
                            Anio = anio,
                            PeriodoId = periodoId,
                            UsuarioId = usuarioId,
                            FechaRevision = DateTime.Now,
                            HoraRevision = DateTime.Now.TimeOfDay,
                            InscripcionCompleta = true,
                            AdelantoMateria = 0,
                            AsesoriaEspecial = 0,
                            Observaciones = "Pase Automatico a Maestria"
                        });
                        db.AlumnoInscrito.Remove(AlumnoMaestria);
                    }
                    #endregion

                    #region Cuotas
                    Cuota CuotaColegiatura = db.Cuota.Where(C =>
                                                    C.OfertaEducativaId == maestriaId
                                                    && C.Anio == anio
                                                    && C.PeriodoId == periodoId
                                                    && C.PagoConceptoId == 800).FirstOrDefault();
                    Cuota CuotaInscripcion = db.Cuota.Where(C =>
                                                      C.OfertaEducativaId == maestriaId
                                                      && C.Anio == anio
                                                      && C.PeriodoId == periodoId
                                                      && C.PagoConceptoId == 802).FirstOrDefault();
                    //Montos
                    decimal PromesaColegiatura = CuotaColegiatura?.Monto ?? 0;
                    decimal PromesaInscripcion = CuotaInscripcion?.Monto ?? 0;
                    //Porcentaje Descuentos
                    decimal DescuentoColegiatura = 0;
                    decimal DescuentoInscripcion = 0;
                    //DescuentosId'd
                    int DescuentoIdColegiatura;
                    int DescuentoIdInscripcion;
                    #endregion

                    if (alumno.EsEmpresa)
                    {
                        #region  Es Empresa
                        #region Configuracion
                        GrupoAlumnoConfiguracion ConfigEspecialidad = alumno.Alumno
                                                ?.GrupoAlumnoConfiguracion
                                                ?.Where(al => al.OfertaEducativaId == especialidadId)
                                                ?.FirstOrDefault() ?? null;
                        if (ConfigEspecialidad == null)
                        { return "El alumno es de empresa y no tiene creada su configuración, favor de acudir con RP."; }

                        db.GrupoAlumnoConfiguracion.Add(new GrupoAlumnoConfiguracion
                        {
                            AlumnoId = alumnoId,
                            Anio = anio,
                            CuotaColegiatura = ConfigEspecialidad.CuotaColegiatura,
                            CuotaInscripcion = ConfigEspecialidad.CuotaInscripcion,
                            EsCuotaCongelada = ConfigEspecialidad.EsCuotaCongelada,
                            EsEspecial = ConfigEspecialidad.EsEspecial,
                            EsInscripcionCongelada = ConfigEspecialidad.EsInscripcionCongelada,
                            EstatusId = 1,
                            FechaRegistro = DateTime.Now,
                            GrupoId = ConfigEspecialidad.GrupoId,
                            HoraRegistro = DateTime.Now.TimeOfDay,
                            NumeroPagos = ConfigEspecialidad.NumeroPagos,
                            OfertaEducativaId = maestriaId,
                            PagoPlanId = ConfigEspecialidad.PagoPlanId,
                            PeriodoId = periodoId,
                            UsuarioId = usuarioId
                        });
                        ConfigEspecialidad.EstatusId = 2;
                        #endregion
                        #region Descuentos
                        PromesaColegiatura = ConfigEspecialidad.CuotaColegiatura;
                        PromesaInscripcion = ConfigEspecialidad.CuotaInscripcion;

                        DescuentoColegiatura = 100 - ((ConfigEspecialidad.CuotaColegiatura * 100) / (CuotaColegiatura?.Monto ?? 0));
                        DescuentoInscripcion = 100 - ((ConfigEspecialidad.CuotaInscripcion * 100) / (CuotaInscripcion?.Monto ?? 0));

                        //Descuentoid
                        DescuentoIdColegiatura = db.Descuento.Where(a =>
                                                      a.OfertaEducativaId == maestriaId
                                                      && a.PagoConceptoId == 800
                                                      && a.Descripcion == "Descuento en colegiatura")
                                                      .FirstOrDefault()?
                                                      .DescuentoId ?? 0;

                        DescuentoIdInscripcion = db.Descuento.Where(a =>
                                                      a.OfertaEducativaId == maestriaId
                                                      && a.PagoConceptoId == 802
                                                      && a.Descripcion == "Descuento en inscripción")
                                                      .FirstOrDefault()?
                                                      .DescuentoId ?? 0;
                        #endregion
                        #endregion
                    }
                    else {
                        DescuentoColegiatura = db.AlumnoDescuento.Where(ad =>
                                                        ad.AlumnoId == alumnoId
                                                        && ad.OfertaEducativaId == especialidadId
                                                        && ad.PagoConceptoId == 800
                                                        && ad.EstatusId == 2)
                                                        .ToList()
                                                        .OrderByDescending(al=> al.AlumnoDescuentoId)?
                                                        .FirstOrDefault()?
                                                        .Monto ?? 0;
                        DescuentoInscripcion = db.AlumnoDescuento.Where(ad =>
                                                         ad.AlumnoId == alumnoId
                                                         && ad.OfertaEducativaId == especialidadId
                                                         && ad.PagoConceptoId == 802
                                                         && ad.EstatusId == 2)?
                                                        .ToList()
                                                        .OrderByDescending(al => al.AlumnoDescuentoId)?
                                                        .FirstOrDefault()?
                                                        .Monto ?? 0;
                        //promesas
                        PromesaColegiatura = DescuentoColegiatura * CuotaColegiatura.Monto;
                        PromesaColegiatura =CuotaColegiatura.Monto - Math.Round(PromesaColegiatura / 100);

                        PromesaInscripcion = DescuentoInscripcion * CuotaInscripcion.Monto;
                        PromesaInscripcion = CuotaInscripcion.Monto - Math.Round(PromesaInscripcion / 100);

                        //Descuentoid
                        DescuentoIdColegiatura = db.Descuento.Where(a =>
                                                      a.OfertaEducativaId == maestriaId
                                                      && a.PagoConceptoId == 800
                                                      && a.Descripcion == "Beca Académica")
                                                      .FirstOrDefault()?
                                                      .DescuentoId ?? 0;

                        DescuentoIdInscripcion = db.Descuento.Where(a =>
                                                      a.OfertaEducativaId == maestriaId
                                                      && a.PagoConceptoId == 802
                                                      && a.Descripcion == "Descuento en inscripción")
                                                      .FirstOrDefault()?
                                                      .DescuentoId ?? 0;
                    }

                    #region Generar Descuentos y Pagos
                    db.AlumnoDescuento.AddRange(new List<AlumnoDescuento>
                    {//Inscripcion
                        new AlumnoDescuento
                        {
                            AlumnoId=alumnoId,
                            Anio=anio,
                            Comentario="",
                            DescuentoId=DescuentoIdInscripcion,
                            EsComite=false,
                            EsDeportiva=false,
                            EsSEP=false,
                            EstatusId=2,
                            FechaAplicacion=DateTime.Now,
                            FechaGeneracion=DateTime.Now,
                            HoraGeneracion=DateTime.Now.TimeOfDay,
                            Monto=DescuentoInscripcion,
                            OfertaEducativaId=maestriaId,
                            PagoConceptoId=802,
                            PeriodoId=periodoId,
                            UsuarioId=usuarioId
                        },
                        //Colegiatura
                        new AlumnoDescuento
                        {
                             AlumnoId=alumnoId,
                            Anio=anio,
                            Comentario="",
                            DescuentoId=DescuentoIdColegiatura,
                            EsComite=false,
                            EsDeportiva=false,
                            EsSEP=false,
                            EstatusId=2,
                            FechaAplicacion=DateTime.Now,
                            FechaGeneracion=DateTime.Now,
                            HoraGeneracion=DateTime.Now.TimeOfDay,
                            Monto=DescuentoColegiatura,
                            OfertaEducativaId=maestriaId,
                            PagoConceptoId=800,
                            PeriodoId=periodoId,
                            UsuarioId=usuarioId
                        }
                    });
                    db.Pago.Add(new Pago
                    {
                        AlumnoId = alumnoId,
                        Anio = anio,
                        Cuota = CuotaInscripcion.Monto,
                        CuotaId = CuotaInscripcion.CuotaId,
                        EsAnticipado = false,
                        EsEmpresa = false,
                        EstatusId = DescuentoInscripcion == 100 ? 4 : 1,
                        FechaGeneracion = DateTime.Now,
                        HoraGeneracion = DateTime.Now.TimeOfDay,
                        OfertaEducativaId = maestriaId,
                        PeriodoId = periodoId,
                        PeriodoAnticipadoId = 0,
                        Promesa = PromesaInscripcion,
                        ReferenciaId = "",
                        Restante = PromesaInscripcion,
                        SubperiodoId = 1,
                        UsuarioId = usuarioId,
                        UsuarioTipoId = usuariodb.UsuarioTipoId,
                        PagoDescuento = new List<PagoDescuento>
                        {
                            new PagoDescuento
                            {
                                DescuentoId = DescuentoIdInscripcion,
                                Monto = CuotaInscripcion.Monto-PromesaInscripcion
                            }
                        }
                    });
                    for (int i = 1; i <= 4; i++)
                    {
                        db.Pago.Add(new Pago
                        {
                            AlumnoId = alumnoId,
                            Anio = anio,
                            Cuota = CuotaColegiatura.Monto,
                            CuotaId = CuotaColegiatura.CuotaId,
                            EsAnticipado = false,
                            EsEmpresa = false,
                            EstatusId = DescuentoColegiatura == 100 ? 4 : 1,
                            FechaGeneracion = DateTime.Now,
                            HoraGeneracion = DateTime.Now.TimeOfDay,
                            OfertaEducativaId = maestriaId,
                            PeriodoId = periodoId,
                            PeriodoAnticipadoId = 0,
                            Promesa = PromesaColegiatura,
                            ReferenciaId = "",
                            Restante = PromesaColegiatura,
                            SubperiodoId = i,
                            UsuarioId = usuarioId,
                            UsuarioTipoId = usuariodb.UsuarioTipoId,
                            PagoDescuento = new List<PagoDescuento>
                        {
                            new PagoDescuento
                            {
                                DescuentoId = DescuentoIdColegiatura,
                                Monto = CuotaColegiatura.Monto-PromesaColegiatura
                            }
                        }
                        });
                    }
                    #endregion
                    db.SaveChanges();

                    db.Pago.Local.ToList().ForEach(p =>
                    {
                        p.ReferenciaId = db.spGeneraReferencia(p.PagoId).FirstOrDefault();
                    });

                    db.SaveChanges();

                    return "Guardado";
                }
                catch(Exception a) { return "Fallo "+ a.InnerException.Message; }
            }
        }

        public static void ReinscripcionAcademico(Universidad.DTO.Reinscripcion.DTOReinscripcionAcademico Alumno)
        {
            List<DAL.Pago> Cargos = new List<Pago>();
            List<Pago> Cabecero = new List<Pago>();
            int[] Estatus = { 1, 4, 13, 14 };
            DTO.Usuario.DTOUsuario Usuario;
            int ofertaEducativaTipoId = 0;

            using (UniversidadEntities db = new UniversidadEntities())
            {
                Usuario = (from a in db.Usuario
                           where a.UsuarioId == Alumno.usuarioId
                           select new DTO.Usuario.DTOUsuario
                           {
                               usuarioId = a.UsuarioId,
                               usuarioTipoId = a.UsuarioTipoId
                           }).AsNoTracking().FirstOrDefault();

                ofertaEducativaTipoId = db.OfertaEducativa.AsNoTracking().Where(n => n.OfertaEducativaId == Alumno.ofertaEducativaId).FirstOrDefault().OfertaEducativaTipoId;

                #region Pagos

                Cargos.AddRange((from a in db.Pago
                                 where (a.Cuota1.PagoConceptoId == 800 || a.Cuota1.PagoConceptoId == 802)
                                    && Estatus.Contains(a.EstatusId)
                                    && (a.Anio == Alumno.anio && a.PeriodoId == Alumno.periodoId)
                                    && a.Promesa > 0
                                    && a.AlumnoId == Alumno.alumnoId
                                    && a.OfertaEducativaId == Alumno.ofertaEducativaId
                                 select a).ToList());

                #endregion Pagos

                if (Alumno.inscripcionCompleta)
                {
                    #region Generación de Cargos

                    var Colegiaturas = (from a in db.Pago
                                        join b in db.Cuota on a.CuotaId equals b.CuotaId
                                        where a.AlumnoId == Alumno.alumnoId
                                        && Estatus.Contains(a.EstatusId)
                                        && b.PagoConceptoId == 800
                                        && a.Anio == Alumno.anio
                                        && a.PeriodoId == Alumno.periodoId
                                        && a.OfertaEducativaId == Alumno.ofertaEducativaId
                                        select a).AsNoTracking().ToList();

                    var Inscripcion = (from a in db.Pago
                                       join b in db.Cuota on a.CuotaId equals b.CuotaId
                                       where a.AlumnoId == Alumno.alumnoId
                                       && Estatus.Contains(a.EstatusId)
                                       && b.PagoConceptoId == 802
                                       && a.Anio == Alumno.anio
                                       && a.PeriodoId == Alumno.periodoId
                                       && a.OfertaEducativaId == Alumno.ofertaEducativaId
                                       select a).AsNoTracking().ToList();

                    var CuotaColegiatura = (from a in db.Cuota
                                            where a.Anio == Alumno.anio
                                            && a.PeriodoId == Alumno.periodoId
                                            && a.OfertaEducativaId == Alumno.ofertaEducativaId
                                            && a.PagoConceptoId == 800
                                            select a).AsNoTracking().FirstOrDefault();

                    var CuotaInscripcion = (from a in db.Cuota
                                            where a.Anio == Alumno.anio
                                            && a.PeriodoId == Alumno.periodoId
                                            && a.OfertaEducativaId == Alumno.ofertaEducativaId
                                            && a.PagoConceptoId == 802
                                            select a).AsNoTracking().FirstOrDefault();


                    if (Alumno.ofertaEducativaId != 44)
                    {
                        if (Inscripcion == null || Inscripcion.Count == 0)
                            Cabecero.Add(new Pago
                            {
                                ReferenciaId = "",
                                AlumnoId = Alumno.alumnoId,
                                Anio = Alumno.anio,
                                PeriodoId = Alumno.periodoId,
                                SubperiodoId = 1,
                                OfertaEducativaId = Alumno.ofertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                HoraGeneracion = DateTime.Now.TimeOfDay,
                                CuotaId = CuotaInscripcion.CuotaId,
                                Cuota = CuotaInscripcion.Monto,
                                Promesa = CuotaInscripcion.Monto,
                                Restante = CuotaInscripcion.Monto,
                                EstatusId = 1,
                                EsEmpresa = false,
                                EsAnticipado = false,
                                UsuarioId = Usuario.usuarioId,
                                UsuarioTipoId = Usuario.usuarioTipoId,
                                PeriodoAnticipadoId = 0
                            });

                        for (int i = 1; i <= 4; i++)

                            if (Colegiaturas.Count(n => n.SubperiodoId == i) == 0)
                                Cabecero.Add(new Pago
                                {
                                    ReferenciaId = "",
                                    AlumnoId = Alumno.alumnoId,
                                    Anio = Alumno.anio,
                                    PeriodoId = Alumno.periodoId,
                                    SubperiodoId = i,
                                    OfertaEducativaId = Alumno.ofertaEducativaId,
                                    FechaGeneracion = DateTime.Now,
                                    HoraGeneracion = DateTime.Now.TimeOfDay,
                                    CuotaId = CuotaColegiatura.CuotaId,
                                    Cuota = CuotaColegiatura.Monto,
                                    Promesa = CuotaColegiatura.Monto,
                                    Restante = CuotaColegiatura.Monto,
                                    EstatusId = 1,
                                    EsEmpresa = false,
                                    EsAnticipado = false,
                                    UsuarioId = Usuario.usuarioId,
                                    UsuarioTipoId = Usuario.usuarioTipoId,
                                    PeriodoAnticipadoId = 0
                                });
                    }

                    #endregion Generación de Cargos
                }

                else if (!Alumno.inscripcionCompleta)
                {
                    Cargos.ForEach(s => {
                        BLL.BLLCargo.CancelarTotal(s.PagoId, Usuario.usuarioId, "El coordinador indico que no es una inscripción total al cuatrimestre");
                    });
                }

                if (Alumno.materia > 0)
                {
                    #region Adelanto de Materia

                    var CuotaMateria = ofertaEducativaTipoId == 1
                                                    ? (from a in db.Cuota
                                                       where a.Anio == Alumno.anio
                                                       && a.PeriodoId == Alumno.periodoId
                                                       && a.OfertaEducativaId == Alumno.ofertaEducativaId
                                                       && a.PagoConceptoId == 304
                                                       select a).AsNoTracking().FirstOrDefault()
                                                    : (ofertaEducativaTipoId == 2 || ofertaEducativaTipoId == 3)
                                                            ? (from a in db.Cuota
                                                               where a.Anio == Alumno.anio
                                                               && a.PeriodoId == Alumno.periodoId
                                                               && a.OfertaEducativaId == Alumno.ofertaEducativaId
                                                               && a.PagoConceptoId == 320
                                                               select a).AsNoTracking().FirstOrDefault()
                                                            : null;

                    if (CuotaMateria != null)
                    {
                        for (int i = 0; i < Alumno.materia; i++)
                            Cabecero.Add(new Pago
                            {
                                ReferenciaId = "",
                                AlumnoId = Alumno.alumnoId,
                                Anio = Alumno.anio,
                                PeriodoId = Alumno.periodoId,
                                SubperiodoId = 1,
                                OfertaEducativaId = Alumno.ofertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                HoraGeneracion = DateTime.Now.TimeOfDay,
                                CuotaId = CuotaMateria.CuotaId,
                                Cuota = CuotaMateria.Monto,
                                Promesa = CuotaMateria.Monto,
                                Restante = CuotaMateria.Monto,
                                EstatusId = 1,
                                EsEmpresa = false,
                                EsAnticipado = false,
                                UsuarioId = Usuario.usuarioId,
                                UsuarioTipoId = Usuario.usuarioTipoId,
                                PeriodoAnticipadoId = 0
                            });
                    }

                    #endregion Adelanto de Materia
                }

                if (Alumno.asesoria > 0)
                {
                    #region Asesoria Especial

                    var CuotaAsesoria = (from a in db.Cuota
                                         where a.Anio == Alumno.anio
                                         && a.PeriodoId == Alumno.periodoId
                                         && a.OfertaEducativaId == Alumno.ofertaEducativaId
                                         && a.PagoConceptoId == 15
                                         select a).AsNoTracking().FirstOrDefault();

                    if (CuotaAsesoria != null)
                    {
                        for (int i = 0; i < Alumno.asesoria; i++)
                            Cabecero.Add(new Pago
                            {
                                ReferenciaId = "",
                                AlumnoId = Alumno.alumnoId,
                                Anio = Alumno.anio,
                                PeriodoId = Alumno.periodoId,
                                SubperiodoId = 1,
                                OfertaEducativaId = Alumno.ofertaEducativaId,
                                FechaGeneracion = DateTime.Now,
                                HoraGeneracion = DateTime.Now.TimeOfDay,
                                CuotaId = CuotaAsesoria.CuotaId,
                                Cuota = CuotaAsesoria.Monto,
                                Promesa = CuotaAsesoria.Monto,
                                Restante = CuotaAsesoria.Monto,
                                EstatusId = 1,
                                EsEmpresa = false,
                                EsAnticipado = false,
                                UsuarioId = Usuario.usuarioId,
                                UsuarioTipoId = Usuario.usuarioTipoId,
                                PeriodoAnticipadoId = 0
                            });
                    }

                    #endregion AsesoriaEspecial
                }

                #region Revisión

                var AlumnoRev = db.AlumnoRevision.Where(c => c.AlumnoId == Alumno.alumnoId && c.OfertaEducativaId == Alumno.ofertaEducativaId && c.Anio == Alumno.anio && c.PeriodoId == Alumno.periodoId).FirstOrDefault();

                db.AlumnoRevision.Add(new AlumnoRevision
                {
                    AlumnoId = Alumno.alumnoId,
                    OfertaEducativaId = Alumno.ofertaEducativaId,
                    Anio = Alumno.anio,
                    PeriodoId = Alumno.periodoId,
                    UsuarioId = Alumno.usuarioId,
                    FechaRevision = DateTime.Now,
                    HoraRevision = DateTime.Now.TimeOfDay,
                    InscripcionCompleta = Alumno.inscripcionCompleta,
                    AsesoriaEspecial = Alumno.asesoria,
                    AdelantoMateria = Alumno.materia,
                    Observaciones = Alumno.observaciones
                });

                if (AlumnoRev != null)
                    db.AlumnoRevision.Remove(AlumnoRev);

                #endregion Revisión

                #region Cuatrimestre

                var AlumnoCuatrimestre = db.AlumnoCuatrimestre.Where(c => c.AlumnoId == Alumno.alumnoId
                                                                     && c.OfertaEducativaId == Alumno.ofertaEducativaId)
                                                              ?.FirstOrDefault()?? null;

                if (AlumnoCuatrimestre != null)
                {
                    db.AlumnoCuatrimestreBitacora.Add(new AlumnoCuatrimestreBitacora
                    {
                        AlumnoId = AlumnoCuatrimestre.AlumnoId,
                        OfertaEducativaId = AlumnoCuatrimestre.OfertaEducativaId,
                        Cuatrimestre = AlumnoCuatrimestre.Cuatrimestre,
                        Anio = AlumnoCuatrimestre.Anio,
                        PeriodoId = AlumnoCuatrimestre.PeriodoId,
                        esRegular = AlumnoCuatrimestre.esRegular,
                        FechaAsignacion = AlumnoCuatrimestre.FechaAsignacion,
                        HoraAsignacion = AlumnoCuatrimestre.HoraAsignacion,
                        UsuarioId = AlumnoCuatrimestre.UsuarioId
                    });

                    db.AlumnoCuatrimestre.Remove(AlumnoCuatrimestre);

                    var Cuatrimestre = Alumno.esRegular == true  ? AlumnoCuatrimestre.Cuatrimestre + 1 : Alumno.Cuatrimestre ;

                   

                    db.AlumnoCuatrimestre.Add(new AlumnoCuatrimestre
                    {
                        AlumnoId = Alumno.alumnoId,
                        OfertaEducativaId = Alumno.ofertaEducativaId,
                        Cuatrimestre = Cuatrimestre,
                        Anio = Alumno.anio,
                        PeriodoId = Alumno.periodoId,
                        esRegular = Alumno.esRegular,
                        FechaAsignacion = DateTime.Now,
                        HoraAsignacion = DateTime.Now.TimeOfDay,
                        UsuarioId = Alumno.usuarioId
                    });

                }
                else
                {
                    db.AlumnoCuatrimestre.Add(new AlumnoCuatrimestre
                    {
                        AlumnoId = Alumno.alumnoId,
                        OfertaEducativaId = Alumno.ofertaEducativaId,
                        Cuatrimestre = Alumno.Cuatrimestre,
                        Anio = Alumno.anio,
                        PeriodoId = Alumno.periodoId,
                        esRegular = Alumno.esRegular,
                        FechaAsignacion = DateTime.Now,
                        HoraAsignacion = DateTime.Now.TimeOfDay,
                        UsuarioId = Alumno.usuarioId
                    });
                }


                #endregion Cuatrimestre

                if (Cabecero.Count > 0)
                {
                    Cabecero.ForEach(n =>
                    {
                        db.Pago.Add(n);
                        db.SaveChanges();
                    });

                    db.SaveChanges();

                    Cabecero.ForEach(n =>
                    {
                        n.ReferenciaId = db.spGeneraReferencia(n.PagoId).FirstOrDefault();
                    });

                    db.SaveChanges();
                }

                else
                    db.SaveChanges();

                #region Becas Academica, SEP, Comite

                var Pendiente = (from a in db.AlumnoDescuentoPendiente
                                 where a.AlumnoDescuento.AlumnoId == Alumno.alumnoId
                                 && a.AlumnoDescuento.OfertaEducativaId == Alumno.ofertaEducativaId
                                 && a.AlumnoDescuento.Anio == Alumno.anio
                                 && a.AlumnoDescuento.PeriodoId == Alumno.periodoId
                                 && a.EstatusId == 1
                                 && !a.AlumnoDescuento.EsDeportiva
                                 && a.AlumnoDescuento.PagoConceptoId == 800
                                 select a).FirstOrDefault();

                if (Pendiente != null)
                {
                    BLL.BLLAlumnoPortal.AplicaBeca(new DTO.Alumno.Beca.DTOAlumnoBeca
                    {
                        alumnoId = Pendiente.AlumnoDescuento.AlumnoId,
                        anio = Pendiente.AlumnoDescuento.Anio,
                        periodoId = Pendiente.AlumnoDescuento.PeriodoId,
                        ofertaEducativaId = Pendiente.AlumnoDescuento.OfertaEducativaId,
                        porcentajeBeca = Pendiente.AlumnoDescuento.Monto,
                        esSEP = Pendiente.AlumnoDescuento.EsSEP,
                        esComite = Pendiente.AlumnoDescuento.EsComite,
                        esEmpresa = (db.AlumnoInscrito.Where(n => n.AlumnoId == Alumno.alumnoId && n.OfertaEducativaId == Alumno.ofertaEducativaId && n.Anio == Alumno.anio && n.PeriodoId == Alumno.periodoId).FirstOrDefault().EsEmpresa),
                        usuarioId = Usuario.usuarioId
                    }, true);

                    Pendiente.FechaAplicacion = DateTime.Now;
                    Pendiente.HoraAplicacion = DateTime.Now.TimeOfDay;
                    Pendiente.UsuarioIdAplicacion = Usuario.usuarioId;
                    Pendiente.EstatusId = 2;
                }

                db.SaveChanges();

                #endregion Becas Academica, SEP, Comite

                #region Beca Deportiva

                var PendienteDeportiva = (from a in db.AlumnoDescuentoPendiente
                                          where a.AlumnoDescuento.AlumnoId == Alumno.alumnoId
                                          && a.AlumnoDescuento.OfertaEducativaId == Alumno.ofertaEducativaId
                                          && a.AlumnoDescuento.Anio == Alumno.anio
                                          && a.AlumnoDescuento.PeriodoId == Alumno.periodoId
                                          && a.EstatusId == 1
                                          && a.AlumnoDescuento.EsDeportiva
                                          && a.AlumnoDescuento.PagoConceptoId == 800
                                          select a).FirstOrDefault();

                if (PendienteDeportiva != null)
                {
                    BLL.BLLAlumnoPortal.AplicaBecaDeportiva(new DTO.Alumno.Beca.DTOAlumnoBecaDeportiva
                    {
                        alumnoId = Pendiente.AlumnoDescuento.AlumnoId,
                        anio = Pendiente.AlumnoDescuento.Anio,
                        periodoId = Pendiente.AlumnoDescuento.PeriodoId,
                        ofertaEducativaId = Pendiente.AlumnoDescuento.OfertaEducativaId,
                        porcentajeBeca = Pendiente.AlumnoDescuento.Monto,
                        usuarioId = Usuario.usuarioId
                    }, true);

                    PendienteDeportiva.FechaAplicacion = DateTime.Now;
                    PendienteDeportiva.HoraAplicacion = DateTime.Now.TimeOfDay;
                    PendienteDeportiva.UsuarioIdAplicacion = Usuario.usuarioId;
                    PendienteDeportiva.EstatusId = 2;
                }

                db.SaveChanges();

                #endregion Beca Deportiva
            }
        }

        }
}
