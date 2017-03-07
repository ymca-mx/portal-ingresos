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
                List<DTOGrupo> lstGrupo = (from a in db.Grupo
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
                                               //GrupoDetalle = new DTOGrupoDetalle
                                               //{
                                               //    CuotaId = a.GrupoDetalle.CuotaId,
                                               //    GrupoId = a.GrupoDetalle.GrupoId,
                                               //    NoPagos = a.GrupoDetalle.NoPagos,
                                               //    EsCongelada = a.GrupoDetalle.EsCuotaCongelada,
                                               //    Cuota = new DTOCuota
                                               //    {
                                               //        CuotaId = a.GrupoDetalle.Cuota.CuotaId,
                                               //        Anio = a.GrupoDetalle.Cuota.Anio,
                                               //        PeriodoId = a.GrupoDetalle.Cuota.PeriodoId,
                                               //        OfertaEducativaId = a.GrupoDetalle.Cuota.OfertaEducativaId,
                                               //        PagoConceptoId = a.GrupoDetalle.Cuota.PagoConceptoId,
                                               //        Monto = a.GrupoDetalle.Cuota.Monto,
                                               //        EsEmpresa = a.GrupoDetalle.Cuota.EsEmpresa
                                               //    }
                                               //}
                                           }).ToList();

                //lstGrupo.ForEach(delegate(DTOGrupo objGrupo)
                //{
                //    DTOPeriodo objPeriodo = BLLPeriodo.TraerPeriodoEntreFechas(objGrupo.FechaRegistro);
                //    objGrupo.GrupoDetalle.CuotaI = (from b in db.Cuota
                //                                    where b.OfertaEducativaId == objGrupo.OfertaEducativaId && b.Anio == objPeriodo.Anio
                //                                    && b.PeriodoId == objPeriodo.PeriodoId && b.PagoConceptoId == 802
                //                                    select new DTOCuota
                //                                    {
                //                                        PagoConceptoId = b.PagoConceptoId,
                //                                        //Monto = Math.Round(b.Monto - ((b.Monto * ((decimal) objGrupo.GrupoDetalle.PorcentajeInscripcion)) / 100))
                //                                    }).FirstOrDefault();
                //    objGrupo.GrupoDetalle.CuotaB = new DTOCuota
                //    {
                //        PagoConceptoId = objGrupo.GrupoDetalle.Cuota.PagoConceptoId,
                //        Monto = 0,// Math.Round(objGrupo.GrupoDetalle.Cuota.Monto - ((objGrupo.GrupoDetalle.Cuota.Monto * ((decimal)objGrupo.GrupoDetalle.PorcentajeColegiatura)) / 100))
                //    };
                //    objGrupo.GrupoDetalle.CuotaI.MontoS = objGrupo.GrupoDetalle.CuotaI.Monto.ToString("C", Cultura);
                //    objGrupo.GrupoDetalle.CuotaB.MontoS = objGrupo.GrupoDetalle.CuotaB.Monto.ToString("C", Cultura);
                //});

                return lstGrupo;
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
                    List<DTOAlumnoEspecial> lstAlumnos = new List<DTOAlumnoEspecial>();

                    db.Configuration.LazyLoadingEnabled = true;

                    if (GrupoId != 0)
                    {
                        //lstAlumnos = lstAlumnos.Where(a => a.GrupoAlumno != null).ToList().Where(b=> b.GrupoAlumno.GrupoId == GrupoId ).ToList();
                        lstAlumnos = (from al in db.GrupoAlumnoConfiguracion
                                      join ale in db.AlumnoInscrito on new { al.AlumnoId, al.OfertaEducativaId } equals
                                                                      new { ale.AlumnoId, ale.OfertaEducativaId }
                                      where
                                      ale.EsEmpresa == true
                                      && ale.OfertaEducativa.OfertaEducativaTipoId != 4
                                      && al.GrupoId == GrupoId
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


                    return lstAlumnos;
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
                decimal PorcentajeColegiatura = 100 - ((CuotaColegiatura * 100) / CuotaColegiaturabd);

                

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

                BLL.BLLAlumno.AplicaBeca(Alumno, false);
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

        public static bool GuardarAlumnoConfiguracion(DTOGrupoAlumnoCuotaString objAlumno)
        {
            using(UniversidadEntities db=new UniversidadEntities())
            {
                try
                {

                    int Alumnoid = int.Parse(objAlumno.AlumnoId), ofertaEducativa = int.Parse(objAlumno.OfertaEducativaId), ofertaEducativaAnterior = int.Parse(objAlumno.OfertaEducativaIdAnterior);
                    var alumno = db.AlumnoInscrito.
                        Where(a =>
                            a.AlumnoId == Alumnoid
                            && a.OfertaEducativaId == ofertaEducativaAnterior
                            //&& a.PagoPlanId == null
                            && a.EsEmpresa == true).FirstOrDefault();
                    
                    ///solo si va a cambiar la oferta educativa
                    if (alumno.OfertaEducativaId != ofertaEducativa)
                    {

                        //alumno.OfertaEducativaId = ofertaEducativa;
                        db.AlumnoInscrito.Add(new AlumnoInscrito
                        {
                            AlumnoId = int.Parse(objAlumno.AlumnoId),
                            OfertaEducativaId = int.Parse(objAlumno.OfertaEducativaId),
                            Anio = int.Parse(objAlumno.Anio),
                            PeriodoId = int.Parse(objAlumno.PeriodoId),
                            FechaInscripcion = DateTime.Now,
                            HoraInscripcion = DateTime.Now.TimeOfDay,
                            PagoPlanId = int.Parse(objAlumno.PagoPlanId),
                            TurnoId = alumno.TurnoId,
                            EsEmpresa = alumno.EsEmpresa,
                            UsuarioId = int.Parse(objAlumno.UsuarioId),
                            EstatusId = 1
                        });


                        db.AlumnoInscrito.Remove(alumno);


                        Alumno objAlumnoDb = db.Alumno.Where(a => a.AlumnoId == Alumnoid).FirstOrDefault();

                        ////Actualiza MAtricula
                        if (db.OfertaEducativa.Where(l => l.OfertaEducativaId == ofertaEducativa).FirstOrDefault().OfertaEducativaTipoId != 4)
                        {

                            #region Bitacora Alumno
                            db.AlumnoBitacora.Add(new AlumnoBitacora
                            {
                                AlumnoId = objAlumnoDb.AlumnoId,
                                Anio = objAlumnoDb.Anio,
                                EstatusId = objAlumnoDb.EstatusId,
                                Fecha = DateTime.Now,
                                FechaRegistro = objAlumnoDb.FechaRegistro,
                                Materno = objAlumnoDb.Materno,
                                MatriculaId = objAlumnoDb.MatriculaId,
                                Nombre = objAlumnoDb.Nombre,
                                Paterno = objAlumnoDb.Paterno,
                                PeriodoId = objAlumnoDb.PeriodoId,
                                UsuarioId = objAlumnoDb.UsuarioId,
                                UsuarioIdBitacora = int.Parse(objAlumno.UsuarioId)

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
                               }, Alumnoid);

                            if (objAlumnoDb.MatriculaId != NMatricula)
                            {
                                #region Update Alumno

                                objAlumnoDb.MatriculaId = NMatricula;
                                objAlumnoDb.Anio = int.Parse(objAlumno.Anio);
                                objAlumnoDb.PeriodoId = int.Parse(objAlumno.PeriodoId);
                                objAlumnoDb.EstatusId = 1;
                                objAlumnoDb.FechaRegistro = DateTime.Now;
                                objAlumnoDb.UsuarioId = int.Parse(objAlumno.UsuarioId);

                                #endregion



                                #region Bitacora Matricula
                                db.Matricula.Add(new Matricula
                                {
                                    AlumnoId = objAlumnoDb.AlumnoId,
                                    Anio = objAlumnoDb.Anio,
                                    FechaAsignacion = objAlumnoDb.FechaRegistro,
                                    MatriculaId = objAlumnoDb.MatriculaId,
                                    OfertaEducativaId = ofertaEducativa,
                                    PeriodoId = objAlumnoDb.PeriodoId,
                                    UsuarioId = objAlumnoDb.UsuarioId
                                });
                                #endregion
                            }

                        }

                    }
                    else
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
                            AlumnoId = int.Parse(objAlumno.AlumnoId),
                            OfertaEducativaId = int.Parse(objAlumno.OfertaEducativaId),
                            Anio = int.Parse(objAlumno.Anio),
                            PeriodoId = int.Parse(objAlumno.PeriodoId),
                            FechaInscripcion = DateTime.Now,
                            HoraInscripcion = DateTime.Now.TimeOfDay,
                            PagoPlanId = int.Parse(objAlumno.PagoPlanId),
                            TurnoId = alumno.TurnoId,
                            EsEmpresa = alumno.EsEmpresa,
                            UsuarioId = int.Parse(objAlumno.UsuarioId),
                            EstatusId = 1
                        });


                        db.AlumnoInscrito.Remove(alumno);
                    }

                    var actualizarGrupoAlumnoConfiguracion = db.GrupoAlumnoConfiguracion.Where(a => a.AlumnoId == Alumnoid && a.OfertaEducativaId == ofertaEducativaAnterior).FirstOrDefault();

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
                            EsEspecial =actualizarGrupoAlumnoConfiguracion.EsEspecial,
                            FechaRegistro = actualizarGrupoAlumnoConfiguracion.FechaRegistro,
                            HoraRegistro = actualizarGrupoAlumnoConfiguracion.HoraRegistro,
                            OfertaEducativaId =actualizarGrupoAlumnoConfiguracion.OfertaEducativaId,
                            PagoPlanId =actualizarGrupoAlumnoConfiguracion.PagoPlanId,
                            NumeroPagos=actualizarGrupoAlumnoConfiguracion.NumeroPagos,
                            UsuarioId = actualizarGrupoAlumnoConfiguracion.UsuarioId
                        });

                    db.GrupoAlumnoConfiguracion.Remove(actualizarGrupoAlumnoConfiguracion);

                    db.GrupoAlumnoConfiguracion.Add(
                        new GrupoAlumnoConfiguracion
                        {
                            AlumnoId = int.Parse(objAlumno.AlumnoId),
                            GrupoId = int.Parse(objAlumno.GrupoId),
                            Anio = int.Parse(objAlumno.Anio),
                            PeriodoId = int.Parse(objAlumno.PeriodoId),
                            CuotaColegiatura = decimal.Parse(objAlumno.CuotaColegiatura),
                            CuotaInscripcion = decimal.Parse(objAlumno.CuotaInscripcion),
                            EsCuotaCongelada = bool.Parse(objAlumno.EsCuotaCongelada),
                            EsInscripcionCongelada = bool.Parse(objAlumno.EsCuotaCongelada),
                            EsEspecial = bool.Parse(objAlumno.EsEspecial),
                            FechaRegistro = DateTime.Now,
                            HoraRegistro = DateTime.Now.TimeOfDay,
                            OfertaEducativaId = int.Parse(objAlumno.OfertaEducativaId),
                            PagoPlanId = int.Parse(objAlumno.PagoPlanId),
                            UsuarioId = int.Parse(objAlumno.UsuarioId),
                            NumeroPagos = objAlumno.NoPagos,
                            EstatusId = 1
                        });

                    if (objAlumno.Credenciales)
                    {
                        int Anio = int.Parse(objAlumno.Anio), PeriodoId = int.Parse(objAlumno.PeriodoId), UsuarioId = int.Parse(objAlumno.UsuarioId);
                        var CuotaCredencial = db.Cuota.Where(
                            C => C.Anio == Anio
                                && C.PeriodoId == PeriodoId
                                && C.OfertaEducativaId == ofertaEducativa
                                && C.PagoConceptoId == 1000).ToList();

                        if (CuotaCredencial.Count > 0)
                        {
                            db.Pago.Add(
                                new Pago
                                {
                                    AlumnoId = int.Parse(objAlumno.AlumnoId),
                                    Anio = Anio,
                                    PeriodoId = PeriodoId,
                                    OfertaEducativaId = int.Parse(objAlumno.OfertaEducativaId),
                                    UsuarioId = UsuarioId,
                                    Cuota = CuotaCredencial.First().Monto,
                                    CuotaId = CuotaCredencial.First().CuotaId,
                                    EsAnticipado = false,
                                    EsEmpresa = false,
                                    EstatusId = 1,
                                    FechaGeneracion = DateTime.Now,
                                    HoraGeneracion = DateTime.Now.TimeOfDay,
                                    Promesa = CuotaCredencial.First().Monto,
                                    Restante = CuotaCredencial.First().Monto,
                                    UsuarioTipoId = db.Usuario.Where(k => k.UsuarioId == UsuarioId).FirstOrDefault().UsuarioTipoId,
                                    ReferenciaId = "",
                                    SubperiodoId = 1,
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
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    var ConfigAl=
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


                        if (ColegMo != BecaColegiatura && BecaColegiatura>0)
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
                    else {
                        
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

                        if(ColegMo !=  BecaColegiatura && BecaColegiatura > 0)
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