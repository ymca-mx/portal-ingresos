using System;
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
    public class BLLGrupo
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");

        public static List<DTOGrupo> ListaGrupo(int EmpresaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTOGrupo> grupo = (from a in db.Grupo
                                           where a.EmpresaId == EmpresaId
                                           select new DTOGrupo
                                           {
                                               GrupoId = a.GrupoId,
                                               EmpresaId = a.EmpresaId,
                                               RFC = a.Empresa.RFC,
                                               Descripcion = a.Descripcion,
                                               SucursalId = a.SucursalId,
                                               SucursalDireccion = a.SucursalDireccion,
                                               FechaInicio = a.FechaInicio,
                                               FechaInicioS = (a.FechaInicio.Day.ToString().Length > 1 ? a.FechaInicio.Day.ToString() : "0" + a.FechaInicio.Day.ToString()) + "/" +
                                               (a.FechaInicio.Month.ToString().Length > 1 ? a.FechaInicio.Month.ToString() : "0" + a.FechaInicio.Month.ToString()) + "/" + (a.FechaInicio.Year.ToString()),
                                               FechaRegistro = a.FechaRegistro,
                                               FechaRegistroS = (a.FechaRegistro.Day.ToString().Length > 1 ? a.FechaRegistro.Day.ToString() : "0" + a.FechaRegistro.Day.ToString()) + "/" +
                                               (a.FechaRegistro.Month.ToString().Length > 1 ? a.FechaRegistro.Month.ToString() : "0" + a.FechaRegistro.Month.ToString()) + "/" + (a.FechaRegistro.Year.ToString()),
                                               NumeroDePagos = a.NumeroPagos
                                           }).ToList();


                return grupo;
            }
        }

        public static int GuardarGrupo(DTOGrupo objGrupo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                //db.GrupoDetalle.Add(new GrupoDetalle
                //    {
                //        //PorcentajeInscripcion = objGrupo.GrupoDetalle.PorcentajeInscripcion ?? 0,
                //        //SiempreInscripcion = objGrupo.GrupoDetalle.SiempreInscripcion,
                //        NoPagos = objGrupo.GrupoDetalle.NoPagos,
                //        Grupo = new Grupo
                //        {
                //            EmpresaId=objGrupo.EmpresaId,
                //            Descripcion = objGrupo.Descripcion,
                //            SucursalId = objGrupo.SucursalId,
                //            SucursalDireccion = objGrupo.SucursalDireccion,
                //            FechaInicio = objGrupo.FechaInicio
                //        },
                //        Cuota = new Cuota
                //        {
                //            Anio = objGrupo.GrupoDetalle.Cuota.Anio,
                //            PeriodoId = objGrupo.GrupoDetalle.Cuota.PeriodoId,
                //            OfertaEducativaId = objGrupo.GrupoDetalle.Cuota.OfertaEducativaId,
                //            PagoConceptoId = objGrupo.GrupoDetalle.Cuota.PagoConceptoId,
                //            Monto = objGrupo.GrupoDetalle.Cuota.Monto,
                //            EsEmpresa = true
                //        }
                //    });
                db.SaveChanges();
                return db.Grupo.Local[0].GrupoId;
            }
        }

        public static DTOGrupo ObtenerGrupo(int GrupoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from d in db.Grupo
                        where d.GrupoId == GrupoId
                        select new DTOGrupo
                        {
                            GrupoId = d.GrupoId,
                            EmpresaId = d.EmpresaId,
                            RFC = d.Empresa.RFC,
                            Descripcion = d.Descripcion,
                            SucursalId = d.SucursalId,
                            FechaInicio = d.FechaInicio,
                            //GrupoDetalle = new DTOGrupoDetalle
                            //{
                            //    GrupoId = d.GrupoId,
                            //    //PorcentajeColegiatura = d.GrupoDetalle.PorcentajeColegiatura,F
                            //    //PorcentajeInscripcion = d.GrupoDetalle.PorcentajeInscripcion,
                            //    //SiempreInscripcion = d.GrupoDetalle.SiempreInscripcion,
                            //    NoPagos = d.GrupoDetalle.NoPagos,
                            //    CuotaId = d.GrupoDetalle.CuotaId,
                            //    Cuota = new DTOCuota
                            //    {
                            //        CuotaId = d.GrupoDetalle.Cuota.CuotaId,
                            //        Anio = d.GrupoDetalle.Cuota.Anio,
                            //        PeriodoId = d.GrupoDetalle.Cuota.PeriodoId,
                            //        OfertaEducativaId = d.GrupoDetalle.Cuota.OfertaEducativaId,
                            //        PagoConceptoId = d.GrupoDetalle.Cuota.PagoConceptoId,
                            //        Monto = d.GrupoDetalle.Cuota.Monto,
                            //        EsEmpresa = d.GrupoDetalle.Cuota.EsEmpresa

                            //    }
                            //}
                        }).FirstOrDefault();
            }
        }

        public static DTOGrupoAlumnoCuota TraerConfiguracion(int alumnoId, int especialidadId, int anio, int periodoId)
        {
            using(UniversidadEntities db=new UniversidadEntities())
            {
                return 
                db.GrupoAlumnoConfiguracion
                    .Where(a => a.AlumnoId == alumnoId
                                && a.OfertaEducativaId == especialidadId
                                && a.Anio == anio
                                && a.PeriodoId == periodoId)
                                .Select(a => new DTOGrupoAlumnoCuota
                                {
                                    AlumnoId = a.AlumnoId,
                                    Anio = a.Anio,
                                    PeriodoId = a.PeriodoId,
                                    OfertaEducativaId = a.OfertaEducativaId,
                                    CuotaColegiatura = a.CuotaColegiatura,
                                    CuotaCongelada = a.EsCuotaCongelada,
                                    CuotaInscripcion = a.CuotaInscripcion,
                                    InscripcionCongelada = a.EsInscripcionCongelada
                                })
                                .FirstOrDefault() ?? null;
            }
        }

        public static int GuardarGrupo2(DTOGrupo objGrupo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                //db.GrupoDetalle.Add(new GrupoDetalle
                //{
                //    //PorcentajeInscripcion = objGrupo.GrupoDetalle.PorcentajeInscripcion??0,
                //    //PorcentajeColegiatura = objGrupo.GrupoDetalle.PorcentajeColegiatura,
                //    //SiempreInscripcion = objGrupo.GrupoDetalle.SiempreInscripcion,
                //    //NoPagos = objGrupo.GrupoDetalle.NoPagos,
                //    //EsCuotaCongelada = objGrupo.GrupoDetalle.EsCongelada,



                if (objGrupo.GrupoId == 0)
                {
                    db.Grupo.Add(new Grupo
                    {
                        EmpresaId = objGrupo.EmpresaId,
                        Descripcion = objGrupo.Descripcion,
                        SucursalId = objGrupo.SucursalId,
                        SucursalDireccion = objGrupo.SucursalDireccion,
                        FechaInicio = objGrupo.FechaInicio,
                        FechaRegistro = DateTime.Now,
                        UsuarioId = objGrupo.UsuarioId
                    });
                }
                else
                {
                    var GrupoBitacora1 = db.Grupo.First(a => a.GrupoId == objGrupo.GrupoId);

                    //meter grupo a bitacora
                    db.GrupoBitacora.Add(new GrupoBitacora
                    {
                        GrupoId = GrupoBitacora1.GrupoId,
                        EmpresaId = GrupoBitacora1.EmpresaId,
                        Descripcion = GrupoBitacora1.Descripcion,
                        SucursalId = GrupoBitacora1.SucursalId,
                        SucursalDireccion = GrupoBitacora1.SucursalDireccion,
                        FechaInicio = GrupoBitacora1.FechaInicio,
                        FechaRegistro= GrupoBitacora1.FechaRegistro,
                        UsuarioId = GrupoBitacora1.UsuarioId,
                        FechaModificacion = DateTime.Now,
                        HoraModificacion = DateTime.Now.TimeOfDay,
                       UsuarioIdModificacion = objGrupo.UsuarioId
                    });



                    Grupo actualizarGrupo = db.Grupo.First(a => a.GrupoId == objGrupo.GrupoId);

                    //actualizar grupo
                    actualizarGrupo.Descripcion = objGrupo.Descripcion;
                    actualizarGrupo.SucursalId = objGrupo.SucursalId;
                    actualizarGrupo.SucursalDireccion = objGrupo.SucursalDireccion;
                    actualizarGrupo.FechaInicio = objGrupo.FechaInicio;
                    actualizarGrupo.FechaRegistro = DateTime.Now;
                    actualizarGrupo.UsuarioId = objGrupo.UsuarioId;

                }

                db.SaveChanges();

                return db.Grupo.Local[0].GrupoId;
            }
        }

        public static List<DTOAlumnoEspecial> TraerAlumnosDeEpresa(int GrupoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<DTOAlumnoEspecial> alumnos = new List<DTOAlumnoEspecial>();

                    db.Configuration.LazyLoadingEnabled = true;

                    if (GrupoId != 0)
                    {
                        alumnos = (from al in db.GrupoAlumnoConfiguracion
                                      join ale in db.AlumnoInscrito on new { al.AlumnoId, al.OfertaEducativaId } equals
                                                                      new { ale.AlumnoId, ale.OfertaEducativaId }
                                      where
                                      ale.EsEmpresa == true
                                      && ale.OfertaEducativa.OfertaEducativaTipoId != 4
                                      && al.GrupoId == GrupoId
                                      && al.Alumno.EstatusId != 3
                                      select new DTOAlumnoEspecial
                                      {
                                          AlumnoId = al.AlumnoId,
                                          Anio = al.Anio,
                                          Nombre = al.Alumno.Nombre + " " + al.Alumno.Paterno + " " + al.Alumno.Materno,
                                          OfertaEducativaId = al.OfertaEducativaId,
                                          OfertaEducativaS = al.OfertaEducativa.Descripcion,
                                          PagoPlanId = (int)al.PagoPlanId,
                                          PeriodoId = al.PeriodoId,
                                          Estatus = al.EstatusId,
                                          AlumnoCuota = al.Alumno.GrupoAlumnoConfiguracion
                                                  .Where(ac =>
                                                  ac.OfertaEducativaId == al.OfertaEducativaId).
                                              Select(k => new DTOGrupoAlumnoCuota
                                              {
                                                  AlumnoId = k.AlumnoId,
                                                  OfertaEducativaId = k.OfertaEducativaId,
                                                  EmpresaId = k.GrupoId != null ? k.Grupo.EmpresaId : 0,
                                                  GrupoId = k.GrupoId != null ? (int)k.GrupoId : 0,
                                                  CuotaColegiatura = k.CuotaColegiatura,
                                                  CuotaCongelada = k.EsCuotaCongelada,
                                                  CuotaInscripcion = k.CuotaInscripcion,
                                                  InscripcionCongelada = k.EsInscripcionCongelada,
                                                  EsEspecial = k.EsEspecial,
                                                  UsuarioId = k.UsuarioId,
                                                  NoPagos =  k.NumeroPagos == null ? 0 : (int) k.NumeroPagos,
                                                  EstatusId = k.EstatusId,
                                                  AnioGrupo = db.Periodo.Where(p => p.FechaInicial <= k.Grupo.FechaInicio
                                                                         && k.Grupo.FechaInicio <= p.FechaFinal).FirstOrDefault().Anio,
                                                  PeriodoIdGrupo = db.Periodo.Where(p => p.FechaInicial <= k.Grupo.FechaInicio
                                                 && k.Grupo.FechaInicio <= p.FechaFinal).FirstOrDefault().PeriodoId,
                                                  DescipcionPeriodo = db.Periodo.Where(a => a.Anio == k.Anio && a.PeriodoId == k.PeriodoId).FirstOrDefault().Descripcion,
                                                  SucuralGrupo =k.Grupo.SucursalId

                                              }).FirstOrDefault()
                                      }
                                     ).ToList();
                                        
                    }else
                    {
                        alumnos = db.AlumnoInscrito.Where(al =>
                                     al.EsEmpresa == true
                                     && al.OfertaEducativa.OfertaEducativaTipoId != 4
                                     && al.Alumno.EstatusId != 3
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
                                                 ac.OfertaEducativaId == al.OfertaEducativaId).
                                             Select(k => new DTOGrupoAlumnoCuota
                                             {
                                                 AlumnoId = k.AlumnoId,
                                                 OfertaEducativaId = k.OfertaEducativaId,
                                                 EmpresaId = k.GrupoId != null ? k.Grupo.EmpresaId :0,
                                                 GrupoId =k.GrupoId!= null? (int)k.GrupoId: 0,
                                                 CuotaColegiatura = k.CuotaColegiatura,
                                                 CuotaCongelada = k.EsCuotaCongelada,
                                                 CuotaInscripcion = k.CuotaInscripcion,
                                                 InscripcionCongelada = k.EsInscripcionCongelada,
                                                 EsEspecial = k.EsEspecial,
                                                 UsuarioId = k.UsuarioId,
                                                 EstatusId = k.EstatusId,
                                                 NoPagos = k.NumeroPagos == null ? 0 : (int)k.NumeroPagos,
                                                 AnioGrupo = k.GrupoId != null ?db.Periodo.Where(p => p.FechaInicial <= k.Grupo.FechaInicio
                                                                        && k.Grupo.FechaInicio <= p.FechaFinal).FirstOrDefault().Anio:0,
                                                 PeriodoIdGrupo =  k.GrupoId != null ?db.Periodo.Where(p => p.FechaInicial <= k.Grupo.FechaInicio
                                                && k.Grupo.FechaInicio <= p.FechaFinal).FirstOrDefault().PeriodoId:0,
                                                Anio= k.Anio,
                                                PeriodoId = k.PeriodoId,
                                                DescipcionPeriodo = db.Periodo.Where(a => a.Anio == k.Anio && a.PeriodoId == k.PeriodoId).FirstOrDefault().Descripcion,
                                                SucuralGrupo = k.Grupo.SucursalId
                                             }).FirstOrDefault()
                                     }).AsNoTracking()
                                     .OrderByDescending(K => K.AlumnoId)
                                     .ToList();
                    }


                    return alumnos;
                }
                catch
                { return null; }
            }
        }

        public static void GenerarCuotas(DTOGrupoAlumnoCuotaString alumnoConfig)
        {
            using(UniversidadEntities db=new UniversidadEntities())
            {
                int Anio =  int.Parse(alumnoConfig.Anio),
                    PeriodoId = int.Parse(alumnoConfig.PeriodoId),
                    OfertaEducativaId = int.Parse(alumnoConfig.OfertaEducativaId);

                decimal CuotaInscripcion = decimal.Parse(alumnoConfig.CuotaInscripcion),
                    CuotaColegiatura = decimal.Parse(alumnoConfig.CuotaColegiatura);                

                decimal CuotaInscripcionbd = db.Cuota.Where(a=> a.OfertaEducativaId == OfertaEducativaId 
                                                            &&  a.Anio == Anio
                                                            &&  a.PeriodoId == PeriodoId
                                                            &&  a.PagoConceptoId == 802).FirstOrDefault().Monto;
                decimal CuotaColegiaturabd = db.Cuota.Where(a => a.OfertaEducativaId == OfertaEducativaId
                                                            && a.Anio == Anio
                                                            && a.PeriodoId == PeriodoId
                                                            && a.PagoConceptoId == 800).FirstOrDefault().Monto;

                decimal PorcentajeInscripcion = 100 - ((CuotaInscripcion * 100) / CuotaInscripcionbd);
                PorcentajeInscripcion = Math.Round(PorcentajeInscripcion, 2);

                decimal PorcentajeColegiatura = 100 - ((CuotaColegiatura * 100) / CuotaColegiaturabd);
                PorcentajeColegiatura = Math.Round(PorcentajeColegiatura, 2);


                DTO.Alumno.Beca.DTOAlumnoBeca Alumno = new DTO.Alumno.Beca.DTOAlumnoBeca
                {
                    alumnoId = int.Parse(alumnoConfig.AlumnoId),
                    anio = int.Parse(alumnoConfig.Anio),
                    periodoId = int.Parse(alumnoConfig.PeriodoId),
                    ofertaEducativaId = int.Parse(alumnoConfig.OfertaEducativaId),
                    porcentajeBeca = PorcentajeColegiatura, //70.15
                    porcentajeInscripcion = PorcentajeInscripcion,
                    esSEP = false,
                    esComite = false,
                    esEmpresa = true,
                    usuarioId = int.Parse(alumnoConfig.UsuarioId), //Usua4rio que inscribio  -> Alejandra 6070
                    fecha = "", // Solo si esta en AlumnoInscrito Fecha 23/01/2017
                    genera = true
                    //    //Colegiatura = decimal
                    //    //Inscripcion = decimal
                };

                BLL.BLLAlumnoPortal.AplicaBeca(Alumno, false);
            }


        }

        public static string MovimientosAlumnoGrupo(int GrupoId, int AlumnoId, int UsuarioId, int OfertaId, int TipoMovimiento)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                try
                {


                    GrupoAlumnoConfiguracion actualizarGrupoAlumnoConfiguracion = db.GrupoAlumnoConfiguracion.First(a => a.AlumnoId == AlumnoId && a.OfertaEducativaId == OfertaId);

                    if (TipoMovimiento == 1)
                    {
                        
                        actualizarGrupoAlumnoConfiguracion.GrupoId = GrupoId;

                    }
                    else {
                        // actualzar GrupoAlumnoConfiguracion 

                        db.GrupoAlumnoConfiguracionBitacora.Add(new GrupoAlumnoConfiguracionBitacora
                        {
                            AlumnoId = actualizarGrupoAlumnoConfiguracion.AlumnoId,
                            GrupoId = actualizarGrupoAlumnoConfiguracion.GrupoId,
                            OfertaEducativaId = actualizarGrupoAlumnoConfiguracion.OfertaEducativaId,
                            Anio = actualizarGrupoAlumnoConfiguracion.Anio,
                            PeriodoId = actualizarGrupoAlumnoConfiguracion.PeriodoId,
                            CuotaColegiatura = actualizarGrupoAlumnoConfiguracion.CuotaColegiatura,
                            CuotaInscripcion = actualizarGrupoAlumnoConfiguracion.CuotaInscripcion,
                            EsCuotaCongelada = actualizarGrupoAlumnoConfiguracion.EsCuotaCongelada,
                            EsInscripcionCongelada = actualizarGrupoAlumnoConfiguracion.EsInscripcionCongelada,
                            EsEspecial = actualizarGrupoAlumnoConfiguracion.EsEspecial,
                            UsuarioId = actualizarGrupoAlumnoConfiguracion.UsuarioId,
                            PagoPlanId = actualizarGrupoAlumnoConfiguracion.PagoPlanId,
                            FechaRegistro = actualizarGrupoAlumnoConfiguracion.FechaRegistro,
                            HoraRegistro = actualizarGrupoAlumnoConfiguracion.HoraRegistro

                        });

                        // actualzar GrupoAlumnoConfiguracion 
                        actualizarGrupoAlumnoConfiguracion.GrupoId = GrupoId;
                        actualizarGrupoAlumnoConfiguracion.UsuarioId = UsuarioId;
                        actualizarGrupoAlumnoConfiguracion.FechaRegistro = DateTime.Now;
                        actualizarGrupoAlumnoConfiguracion.HoraRegistro = DateTime.Now.TimeOfDay;

                    }

                    
                    db.SaveChanges();
                    return "Guardado";
                }
                catch (Exception)
                {
                    return null;
                }
            }

        }

        public static bool GuardarAlumnoConfiguracion(DTOGrupoAlumnoCuotaString alumnoConfiguracion)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {

                    int alumnoid = int.Parse(alumnoConfiguracion.AlumnoId), ofertaEducativa = int.Parse(alumnoConfiguracion.OfertaEducativaId), ofertaEducativaAnterior = int.Parse(alumnoConfiguracion.OfertaEducativaIdAnterior);
                    AlumnoInscrito alumno = db.AlumnoInscrito.
                        Where(a =>
                            a.AlumnoId == alumnoid
                            && a.OfertaEducativaId == ofertaEducativaAnterior
                            && a.EsEmpresa == true).FirstOrDefault();

                    ///solo si va a cambiar la oferta educativa
                    if (alumno.OfertaEducativaId != ofertaEducativa)

                    {

                        db.AlumnoInscrito.Add(new AlumnoInscrito
                        {
                            AlumnoId = int.Parse(alumnoConfiguracion.AlumnoId),
                            OfertaEducativaId = int.Parse(alumnoConfiguracion.OfertaEducativaId),
                            Anio = int.Parse(alumnoConfiguracion.Anio),
                            PeriodoId = int.Parse(alumnoConfiguracion.PeriodoId),
                            FechaInscripcion = DateTime.Now,
                            HoraInscripcion = DateTime.Now.TimeOfDay,
                            PagoPlanId = int.Parse(alumnoConfiguracion.PagoPlanId),
                            TurnoId = alumno.TurnoId,
                            EsEmpresa = alumno.EsEmpresa,
                            UsuarioId = int.Parse(alumnoConfiguracion.UsuarioId),
                            EstatusId = 1
                        });

                        db.AlumnoInscrito.Remove(alumno);


                        Alumno alumnoDB = db.Alumno.Where(a => a.AlumnoId == alumnoid).FirstOrDefault();

                        ////Actualiza MAtricula
                        if (db.OfertaEducativa.Where(l => l.OfertaEducativaId == ofertaEducativa).FirstOrDefault().OfertaEducativaTipoId != 4)
                        {

                            #region Bitacora Alumno
                            db.AlumnoBitacora.Add(new AlumnoBitacora
                            {
                                AlumnoId = alumnoDB.AlumnoId,
                                Anio = alumnoDB.Anio,
                                EstatusId = alumnoDB.EstatusId,
                                Fecha = DateTime.Now,
                                FechaRegistro = alumnoDB.FechaRegistro,
                                Materno = alumnoDB.Materno,
                                MatriculaId = alumnoDB.MatriculaId,
                                Nombre = alumnoDB.Nombre,
                                Paterno = alumnoDB.Paterno,
                                PeriodoId = alumnoDB.PeriodoId,
                                UsuarioId = alumnoDB.UsuarioId,
                                UsuarioIdBitacora = int.Parse(alumnoConfiguracion.UsuarioId)

                            });
                            #endregion
                            string NMatricula = Herramientas.Matricula.ObtenerMatricula(new DTOAlumnoInscrito
                            {
                                Anio = alumno.Anio,
                                PeriodoId = alumno.PeriodoId,
                                TurnoId = alumno.TurnoId

                            },
                               new DTOOfertaEducativa
                               {
                                   OfertaEducativaId = ofertaEducativa,
                                   Rvoe = db.OfertaEducativa.Where(l => l.OfertaEducativaId == ofertaEducativa).FirstOrDefault().Rvoe,
                               }, alumnoid);

                            if (alumnoDB.MatriculaId != NMatricula)
                            {
                                #region Update Alumno

                                alumnoDB.MatriculaId = NMatricula;
                                alumnoDB.Anio = int.Parse(alumnoConfiguracion.Anio);
                                alumnoDB.PeriodoId = int.Parse(alumnoConfiguracion.PeriodoId);
                                alumnoDB.EstatusId = 1;
                                alumnoDB.FechaRegistro = DateTime.Now;
                                alumnoDB.UsuarioId = int.Parse(alumnoConfiguracion.UsuarioId);

                                #endregion



                                #region Bitacora Matricula
                                db.Matricula.Add(new Matricula
                                {
                                    AlumnoId = alumnoDB.AlumnoId,
                                    Anio = alumnoDB.Anio,
                                    FechaAsignacion = alumnoDB.FechaRegistro,
                                    MatriculaId = alumnoDB.MatriculaId,
                                    OfertaEducativaId = ofertaEducativa,
                                    PeriodoId = alumnoDB.PeriodoId,
                                    UsuarioId = alumnoDB.UsuarioId
                                });
                                #endregion
                            }

                        }

                    }
                    else
                    {
                        if (alumno.UsuarioId != 6070)
                        {
                            db.AlumnoInscritoBitacora.Add(new AlumnoInscritoBitacora
                            {
                                AlumnoId = alumno.AlumnoId,
                                OfertaEducativaId = alumno.OfertaEducativaId,
                                Anio = alumno.Anio,
                                PeriodoId = alumno.PeriodoId,
                                FechaInscripcion = alumno.FechaInscripcion,
                                HoraInscripcion = alumno.HoraInscripcion,
                                PagoPlanId = alumno.PagoPlanId,
                                TurnoId = alumno.TurnoId,
                                EsEmpresa = alumno.EsEmpresa,
                                UsuarioId = alumno.UsuarioId
                            });

                            db.AlumnoInscrito.Add(new AlumnoInscrito
                            {
                                AlumnoId = int.Parse(alumnoConfiguracion.AlumnoId),
                                OfertaEducativaId = int.Parse(alumnoConfiguracion.OfertaEducativaId),
                                Anio = int.Parse(alumnoConfiguracion.Anio),
                                PeriodoId = int.Parse(alumnoConfiguracion.PeriodoId),
                                FechaInscripcion = DateTime.Now,
                                HoraInscripcion = DateTime.Now.TimeOfDay,
                                PagoPlanId = int.Parse(alumnoConfiguracion.PagoPlanId),
                                TurnoId = alumno.TurnoId,
                                EsEmpresa = alumno.EsEmpresa,
                                UsuarioId = int.Parse(alumnoConfiguracion.UsuarioId),
                                EstatusId = 1
                            });

                            db.AlumnoInscrito.Remove(alumno);

                        }

                    }

                    GrupoAlumnoConfiguracion actualizarGrupoAlumnoConfiguracion = db.GrupoAlumnoConfiguracion.Where(a => a.AlumnoId == alumnoid && a.OfertaEducativaId == ofertaEducativaAnterior).FirstOrDefault();

                    db.GrupoAlumnoConfiguracionBitacora.Add(
                        new GrupoAlumnoConfiguracionBitacora
                        {
                            AlumnoId = actualizarGrupoAlumnoConfiguracion.AlumnoId,
                            GrupoId = actualizarGrupoAlumnoConfiguracion.GrupoId,
                            Anio = actualizarGrupoAlumnoConfiguracion.Anio,
                            PeriodoId = actualizarGrupoAlumnoConfiguracion.PeriodoId,
                            CuotaColegiatura = actualizarGrupoAlumnoConfiguracion.CuotaColegiatura,
                            CuotaInscripcion = actualizarGrupoAlumnoConfiguracion.CuotaInscripcion,
                            EsCuotaCongelada = actualizarGrupoAlumnoConfiguracion.EsCuotaCongelada,
                            EsInscripcionCongelada = actualizarGrupoAlumnoConfiguracion.EsCuotaCongelada,
                            EsEspecial = actualizarGrupoAlumnoConfiguracion.EsEspecial,
                            FechaRegistro = actualizarGrupoAlumnoConfiguracion.FechaRegistro,
                            HoraRegistro = actualizarGrupoAlumnoConfiguracion.HoraRegistro,
                            OfertaEducativaId = actualizarGrupoAlumnoConfiguracion.OfertaEducativaId,
                            PagoPlanId = actualizarGrupoAlumnoConfiguracion.PagoPlanId,
                            NumeroPagos = actualizarGrupoAlumnoConfiguracion.NumeroPagos,
                            UsuarioId = actualizarGrupoAlumnoConfiguracion.UsuarioId
                        });

                    db.GrupoAlumnoConfiguracion.Remove(actualizarGrupoAlumnoConfiguracion);

                    db.GrupoAlumnoConfiguracion.Add(
                        new GrupoAlumnoConfiguracion
                        {
                            AlumnoId = int.Parse(alumnoConfiguracion.AlumnoId),
                            GrupoId = int.Parse(alumnoConfiguracion.GrupoId),
                            Anio = int.Parse(alumnoConfiguracion.Anio),
                            PeriodoId = int.Parse(alumnoConfiguracion.PeriodoId),
                            CuotaColegiatura = decimal.Parse(alumnoConfiguracion.CuotaColegiatura),
                            CuotaInscripcion = decimal.Parse(alumnoConfiguracion.CuotaInscripcion),
                            EsCuotaCongelada = bool.Parse(alumnoConfiguracion.EsCuotaCongelada),
                            EsInscripcionCongelada = bool.Parse(alumnoConfiguracion.EsCuotaCongelada),
                            EsEspecial = bool.Parse(alumnoConfiguracion.EsEspecial),
                            FechaRegistro = DateTime.Now,
                            HoraRegistro = DateTime.Now.TimeOfDay,
                            OfertaEducativaId = int.Parse(alumnoConfiguracion.OfertaEducativaId),
                            PagoPlanId = int.Parse(alumnoConfiguracion.PagoPlanId),
                            UsuarioId = int.Parse(alumnoConfiguracion.UsuarioId),
                            NumeroPagos = alumnoConfiguracion.NoPagos,
                            EstatusId = 1
                        });
                    //Credenciales
                    if (alumnoConfiguracion.Credenciales)
                    {
                        int Anio = int.Parse(alumnoConfiguracion.Anio), PeriodoId = int.Parse(alumnoConfiguracion.PeriodoId), UsuarioId = int.Parse(alumnoConfiguracion.UsuarioId);
                        List<Cuota> CuotaCredencial = db.Cuota.Where(
                            C => C.Anio == Anio
                                && C.PeriodoId == PeriodoId
                                && C.OfertaEducativaId == ofertaEducativa
                                && C.PagoConceptoId == 1000).ToList();

                        if (CuotaCredencial.Count > 0)
                        {
                            int ccred = CuotaCredencial.FirstOrDefault().CuotaId;
                            if (db.Pago.Where(p => p.AlumnoId == alumno.AlumnoId
                                                 && p.Anio == Anio
                                                 && p.PeriodoId == PeriodoId
                                                 && p.OfertaEducativaId == p.OfertaEducativaId
                                                 && p.CuotaId == ccred
                                                 && (p.EstatusId == 1 || p.EstatusId == 4)).ToList().Count == 0)
                            {
                                db.Pago.Add(
                                    new Pago
                                    {
                                        AlumnoId = int.Parse(alumnoConfiguracion.AlumnoId),
                                        Anio = Anio,
                                        PeriodoId = PeriodoId,
                                        OfertaEducativaId = int.Parse(alumnoConfiguracion.OfertaEducativaId),
                                        UsuarioId = UsuarioId,
                                        Cuota = CuotaCredencial.FirstOrDefault().Monto,
                                        CuotaId = CuotaCredencial.FirstOrDefault().CuotaId,
                                        EsAnticipado = false,
                                        EsEmpresa = false,
                                        EstatusId = 1,
                                        FechaGeneracion = DateTime.Now,
                                        HoraGeneracion = DateTime.Now.TimeOfDay,
                                        Promesa = CuotaCredencial.FirstOrDefault().Monto,
                                        Restante = CuotaCredencial.FirstOrDefault().Monto,
                                        UsuarioTipoId = db.Usuario.Where(k => k.UsuarioId == UsuarioId).FirstOrDefault().UsuarioTipoId,
                                        ReferenciaId = "",
                                        SubperiodoId = 1,
                                    });
                            }
                        }
                    }
                    //Examen Diagnostico
                    int anio = int.Parse(alumnoConfiguracion.Anio), periodo = int.Parse(alumnoConfiguracion.PeriodoId);
                    List<Pago> pExamen = db.Pago.Where(p => p.AlumnoId == alumno.AlumnoId
                                                    && p.Anio == anio
                                                    && p.PeriodoId == periodo
                                                    && p.OfertaEducativaId == p.OfertaEducativaId
                                                    && p.Cuota1.PagoConceptoId == 1
                                                    && (p.EstatusId == 1 || p.EstatusId == 4)).ToList();
                    if (pExamen.Count > 0)
                    {
                        pExamen.FirstOrDefault().EstatusId = 2;
                        if (pExamen.FirstOrDefault().PagoParcial.Count > 0)
                        {
                            pExamen.FirstOrDefault().PagoParcial.ToList().ForEach(pp =>
                            {
                                if (pp.EstatusId == 4)
                                {
                                    
                                    pp.EstatusId = 2;
                                    pp.ReferenciaProcesada.Restante = (pp.ReferenciaProcesada.Restante + pp.Pago);
                                    pp.ReferenciaProcesada.SeGasto = false;
                                    pp.Pago = 0;
                                }
                            });
                        }
                    }

                    db.SaveChanges();

                    db.Pago.Local.ToList().ForEach(P =>
                    {
                        P.ReferenciaId = db.spGeneraReferencia(P.PagoId).FirstOrDefault();
                    });

                    db.SaveChanges();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static DTOGrupoAlumnoCuota ActualizarConfiguracion(DTOGrupoAlumnoCuota objAlumno)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    decimal
                        CuotaIns = db.Cuota.Where(
                                c => c.Anio == objAlumno.Anio
                                     && c.PeriodoId == objAlumno.PeriodoId
                                     && c.OfertaEducativaId == objAlumno.OfertaEducativaId
                                     && c.PagoConceptoId == 802).FirstOrDefault().Monto,
                        CuotaCol = db.Cuota.Where(
                                c => c.Anio == objAlumno.Anio
                                     && c.PeriodoId == objAlumno.PeriodoId
                                     && c.OfertaEducativaId == objAlumno.OfertaEducativaId
                                     && c.PagoConceptoId == 800).FirstOrDefault().Monto;



                    var AlumnoConf = db.GrupoAlumnoConfiguracion.Where(k =>
                                             k.AlumnoId == objAlumno.AlumnoId
                                             && k.OfertaEducativaId == objAlumno.OfertaEducativaId
                                             && k.Anio == objAlumno.Anio
                                             && k.PeriodoId == objAlumno.PeriodoId)
                                            .ToList();

                    if (AlumnoConf.Count == 0)
                    {
                        return objAlumno;
                    }
                    if (AlumnoConf.First().EsEspecial) //Es Especial
                    {
                        if (!AlumnoConf.First().EsCuotaCongelada) // No Es Cuota Congelada
                        {
                            //Colegiatura  
                            decimal PorcentajeColegiatura = 0;
                            PorcentajeColegiatura = objAlumno.CuotaColegiatura * CuotaCol;
                            PorcentajeColegiatura /= 100;
                            PorcentajeColegiatura = Math.Round(PorcentajeColegiatura);

                            PorcentajeColegiatura = CuotaCol - PorcentajeColegiatura;

                            AlumnoConf.First().CuotaColegiatura = PorcentajeColegiatura;
                        }
                        else //Si Es Congelada
                        {
                            //Colegiatura  
                            decimal PorcentajeColegiatura = 0;
                            PorcentajeColegiatura = AlumnoConf.First().CuotaColegiatura * 100;
                            PorcentajeColegiatura /= CuotaCol;

                            PorcentajeColegiatura = 100 - PorcentajeColegiatura;

                            objAlumno.CuotaColegiatura = Math.Round(PorcentajeColegiatura, 2);
                        }

                        if (!AlumnoConf.First().EsInscripcionCongelada) // No Es Inscripcion Congelada
                        {
                            //Inscripcion 
                            decimal PorcentajeInscripcion = 0;
                            PorcentajeInscripcion = objAlumno.CuotaInscripcion * CuotaIns;
                            PorcentajeInscripcion /= 100;
                            PorcentajeInscripcion = Math.Round(PorcentajeInscripcion);

                            PorcentajeInscripcion = CuotaIns - PorcentajeInscripcion;

                            AlumnoConf.First().CuotaInscripcion = PorcentajeInscripcion;
                        }
                        else
                        {
                            //Inscripcion 
                            decimal PorcentajeInscripcion = 0;
                            PorcentajeInscripcion = AlumnoConf.First().CuotaInscripcion * 100;
                            PorcentajeInscripcion /= CuotaIns;

                            PorcentajeInscripcion = 100 - PorcentajeInscripcion;

                            objAlumno.CuotaInscripcion = Math.Round(PorcentajeInscripcion, 2);
                        }
                    }
                    else // Es Inscripcion Congelada
                    {
                        //Colegiatura  
                        decimal PorcentajeColegiatura = 0;
                        PorcentajeColegiatura = AlumnoConf.First().CuotaColegiatura * 100;
                        PorcentajeColegiatura /= CuotaCol;

                        PorcentajeColegiatura = 100 - PorcentajeColegiatura;

                        objAlumno.CuotaColegiatura = Math.Round(PorcentajeColegiatura, 2);

                        //Inscripcion 
                        decimal PorcentajeInscripcion = 0;
                        PorcentajeInscripcion = AlumnoConf.First().CuotaInscripcion * 100;
                        PorcentajeInscripcion /= CuotaIns;

                        PorcentajeInscripcion = 100 - PorcentajeInscripcion;

                        objAlumno.CuotaInscripcion = Math.Round(PorcentajeInscripcion, 2);
                    }
                    return objAlumno;
                }
                catch
                { return null; }
            }
        }

        public static List<DTOCuota> TraerInscripcion(int AlumnoId, int OfertaEducativaId, int Anio, int PeriodoId, int UsuarioId, decimal BecaColegiatura)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    if ((db.AlumnoInscrito.Where(al => al.AlumnoId == AlumnoId
                                                     && al.EsEmpresa)?.FirstOrDefault()?.EsEmpresa ?? null) != null)
                    {
                        var ConfigAl =
                        db.GrupoAlumnoConfiguracion
                                .Where(K => K.AlumnoId == AlumnoId
                                            && K.OfertaEducativaId == OfertaEducativaId
                                            && K.PeriodoId == PeriodoId
                                            && K.Anio == Anio)
                                .ToList();

                        if (ConfigAl.Count > 0)
                        {
                            var ConfigAl1 = ConfigAl.FirstOrDefault();

                            var cuotadb = db.Cuota
                                            .Where(k => k.Anio == ConfigAl1.Anio
                                                        && k.PeriodoId == ConfigAl1.PeriodoId
                                                        && k.OfertaEducativaId == ConfigAl1.OfertaEducativaId
                                                        && k.PagoConceptoId == 802).First();
                            var InscriMo = (ConfigAl1.CuotaInscripcion * 100) / cuotadb.Monto;
                            InscriMo = 100 - Math.Round(InscriMo, 2);

                            var cuotadb2 = db.Cuota
                                          .Where(k => k.Anio == ConfigAl1.Anio
                                                      && k.PeriodoId == ConfigAl1.PeriodoId
                                                      && k.OfertaEducativaId == ConfigAl1.OfertaEducativaId
                                                      && k.PagoConceptoId == 800).First();
                            var ColegMo = (ConfigAl1.CuotaColegiatura * 100) / cuotadb2.Monto;
                            ColegMo = 100 - Math.Round(ColegMo, 2);


                            if (ColegMo != BecaColegiatura && BecaColegiatura > 0)
                            {

                                db.GrupoAlumnoConfiguracionBitacora
                                    .Add(
                                    ConfigAl.Count > 0 ?
                                        new GrupoAlumnoConfiguracionBitacora
                                        {
                                            AlumnoId = ConfigAl1.AlumnoId,
                                            Anio = ConfigAl1.Anio,
                                            CuotaColegiatura = ConfigAl1.CuotaColegiatura,
                                            CuotaInscripcion = ConfigAl1.CuotaInscripcion,
                                            EsCuotaCongelada = ConfigAl1.EsCuotaCongelada,
                                            EsEspecial = ConfigAl1.EsEspecial,
                                            EsInscripcionCongelada = ConfigAl1.EsInscripcionCongelada,
                                            FechaRegistro = ConfigAl1.FechaRegistro,
                                            GrupoId = ConfigAl1.GrupoId,
                                            HoraRegistro = ConfigAl1.HoraRegistro,
                                            NumeroPagos = ConfigAl1.NumeroPagos,
                                            OfertaEducativaId = ConfigAl1.OfertaEducativaId,
                                            PagoPlanId = ConfigAl1.PagoPlanId,
                                            PeriodoId = ConfigAl1.PeriodoId,
                                            UsuarioId = ConfigAl1.UsuarioId
                                        }
                                        : null);
                                ConfigAl1.CuotaColegiatura = BecaColegiatura;
                                ConfigAl1.HoraRegistro = DateTime.Now.TimeOfDay;
                                ConfigAl1.FechaRegistro = DateTime.Now;
                                ConfigAl1.UsuarioId = UsuarioId;
                                db.SaveChanges();
                            }

                            return new List<DTOCuota> {
                            new DTOCuota
                            {
                                DTOPagoConcepto = new DTOPagoConcepto
                                {
                                    PagoConceptoId=802,
                                    Descripcion = "Inscripcion"
                                },
                                Monto=InscriMo
                            },
                            new DTOCuota
                            {
                                 DTOPagoConcepto = new DTOPagoConcepto
                                {
                                    PagoConceptoId=800,
                                    Descripcion = "Colegiatura"
                                },
                                Monto=ColegMo
                            }
                        };
                        }
                        else
                        {

                            UpdateAlumnoConfiguracion(Anio, PeriodoId, AlumnoId, OfertaEducativaId, UsuarioId);

                            ConfigAl =
                        db.GrupoAlumnoConfiguracion
                                .Where(K => K.AlumnoId == AlumnoId
                                            && K.OfertaEducativaId == OfertaEducativaId
                                            && K.PeriodoId == PeriodoId
                                            && K.Anio == Anio)
                                .ToList();
                            if (ConfigAl.Count == 0) { return null; }

                            var configAl1 = ConfigAl.FirstOrDefault();

                            var cuotadb = db.Cuota
                                            .Where(k => k.Anio == configAl1.Anio
                                                        && k.PeriodoId == configAl1.PeriodoId
                                                        && k.OfertaEducativaId == configAl1.OfertaEducativaId
                                                        && k.PagoConceptoId == 802).First();
                            var InscriMo = (configAl1.CuotaInscripcion * 100) / cuotadb.Monto;
                            InscriMo = 100 - Math.Round(InscriMo, 2);

                            var cuotadb2 = db.Cuota
                                          .Where(k => k.Anio == configAl1.Anio
                                                      && k.PeriodoId == configAl1.PeriodoId
                                                      && k.OfertaEducativaId == configAl1.OfertaEducativaId
                                                      && k.PagoConceptoId == 800).First();
                            var ColegMo = (configAl1.CuotaColegiatura * 100) / cuotadb2.Monto;
                            ColegMo = 100 - Math.Round(ColegMo, 2);

                            if (ColegMo != BecaColegiatura && BecaColegiatura > 0)
                            {

                                db.GrupoAlumnoConfiguracionBitacora
                                    .Add(
                                    ConfigAl.Count > 0 ?
                                        new GrupoAlumnoConfiguracionBitacora
                                        {
                                            AlumnoId = configAl1.AlumnoId,
                                            Anio = configAl1.Anio,
                                            CuotaColegiatura = configAl1.CuotaColegiatura,
                                            CuotaInscripcion = configAl1.CuotaInscripcion,
                                            EsCuotaCongelada = configAl1.EsCuotaCongelada,
                                            EsEspecial = configAl1.EsEspecial,
                                            EsInscripcionCongelada = configAl1.EsInscripcionCongelada,
                                            FechaRegistro = configAl1.FechaRegistro,
                                            GrupoId = configAl1.GrupoId,
                                            HoraRegistro = configAl1.HoraRegistro,
                                            NumeroPagos = configAl1.NumeroPagos,
                                            OfertaEducativaId = configAl1.OfertaEducativaId,
                                            PagoPlanId = configAl1.PagoPlanId,
                                            PeriodoId = configAl1.PeriodoId,
                                            UsuarioId = configAl1.UsuarioId
                                        }
                                        : null);
                                configAl1.CuotaColegiatura = BecaColegiatura;
                                configAl1.HoraRegistro = DateTime.Now.TimeOfDay;
                                configAl1.FechaRegistro = DateTime.Now;
                                configAl1.UsuarioId = UsuarioId;
                                db.SaveChanges();
                            }
                            return new List<DTOCuota> {
                            new DTOCuota
                            {
                                DTOPagoConcepto = new DTOPagoConcepto
                                {
                                    PagoConceptoId=802,
                                    Descripcion = "Inscripcion"
                                },
                                Monto=InscriMo
                            },
                            new DTOCuota
                            {
                                 DTOPagoConcepto = new DTOPagoConcepto
                                {
                                    PagoConceptoId=800,
                                    Descripcion = "Colegiatura"
                                },
                                Monto=ColegMo
                            }
                        };
                        }
                    }
                    else { return null; }
                }
                catch
                { return null; }
            }
        }

        public static void UpdateAlumnoConfiguracion( int Anio, int PeriodoId, int AlumnoId, int OfertaEducativaId, int UsuarioId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var CuotasIncrementos = db.CuotaIncremento.Where(c => c.Anio == Anio && c.PeriodoId == PeriodoId 
                                                                    && (c.PagoConceptoId == 800 || c.PagoConceptoId == 802)
                                                                    && c.OfertaEducativaTipoId == db.OfertaEducativa.Where(o=> o.OfertaEducativaId==OfertaEducativaId).FirstOrDefault().OfertaEducativaTipoId  ).ToList();

                var Cuotas = db.Cuota.Where(c => c.Anio == Anio && c.PeriodoId == PeriodoId
                                               && (c.PagoConceptoId == 800 || c.PagoConceptoId == 802)
                                                && c.OfertaEducativaId == OfertaEducativaId).ToList();

                decimal PorcentajeInscripcion = 0, PorcentajeColegiatura = 0;

                PorcentajeInscripcion = 100 - ((CuotasIncrementos.Where(k => k.PagoConceptoId == 802).First().ImporteIncremento * 100) /
                                            Cuotas.Where(c => c.PagoConceptoId == 802).FirstOrDefault().Monto);

                PorcentajeColegiatura = 100 - ((CuotasIncrementos.Where(k => k.PagoConceptoId == 800).First().ImporteIncremento * 100) /
                                            Cuotas.Where(c => c.PagoConceptoId == 800).FirstOrDefault().Monto);

                var AlumnoConfiguracion = db
                                        .GrupoAlumnoConfiguracion
                                        .Where(AC =>
                                            AC.AlumnoId == AlumnoId
                                            && AC.OfertaEducativaId == OfertaEducativaId
                                            && (AC.Anio != Anio
                                                || AC.PeriodoId != PeriodoId)).ToList();
                if (AlumnoConfiguracion.Count > 0)
                {
                    #region Bitacora 
                    db.GrupoAlumnoConfiguracionBitacora.Add(new GrupoAlumnoConfiguracionBitacora
                    {
                        AlumnoId = AlumnoId,
                        Anio = AlumnoConfiguracion.First().Anio,
                        PeriodoId = AlumnoConfiguracion.First().PeriodoId,
                        CuotaColegiatura = AlumnoConfiguracion.First().CuotaColegiatura,
                        CuotaInscripcion = AlumnoConfiguracion.First().CuotaInscripcion,
                        EsCuotaCongelada = AlumnoConfiguracion.First().EsCuotaCongelada,
                        EsEspecial = AlumnoConfiguracion.First().EsEspecial,
                        EsInscripcionCongelada = AlumnoConfiguracion.First().EsInscripcionCongelada,
                        FechaRegistro = AlumnoConfiguracion.First().FechaRegistro,
                        GrupoId = AlumnoConfiguracion.First().GrupoId,
                        HoraRegistro = AlumnoConfiguracion.First().HoraRegistro,
                        OfertaEducativaId = OfertaEducativaId,
                        PagoPlanId = AlumnoConfiguracion.First().PagoPlanId,
                        UsuarioId = AlumnoConfiguracion.First().UsuarioId
                    });
                    #endregion

                    #region Calculos de Nuevas Cuotas
                    if (!AlumnoConfiguracion.First().EsCuotaCongelada)
                    {
                        AlumnoConfiguracion.First().CuotaColegiatura = (AlumnoConfiguracion.First().CuotaColegiatura + AlumnoConfiguracion.First().CuotaColegiatura * PorcentajeColegiatura);
                    }
                    if (!AlumnoConfiguracion.First().EsInscripcionCongelada)
                    {
                        AlumnoConfiguracion.First().CuotaInscripcion = (AlumnoConfiguracion.First().CuotaInscripcion + AlumnoConfiguracion.First().CuotaInscripcion * PorcentajeInscripcion);
                    }
                    #endregion

                    #region Actualizamos Configuracion
                    AlumnoConfiguracion.First().Anio = Anio;
                    AlumnoConfiguracion.First().PeriodoId = PeriodoId;
                    AlumnoConfiguracion.First().HoraRegistro = DateTime.Now.TimeOfDay;
                    AlumnoConfiguracion.First().FechaRegistro = DateTime.Now;
                    AlumnoConfiguracion.First().UsuarioId = UsuarioId;
                    #endregion

                    db.SaveChanges();
                }
            }
        }
    }
}