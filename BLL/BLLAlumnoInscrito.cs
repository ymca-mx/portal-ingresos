using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLAlumnoInscrito
    {
        public static int ActializarAlumnoInscrito(int AlumnoId, int PagoPlanId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    AlumnoInscrito objAlumnoIncrito = db.AlumnoInscrito.Where(X => X.AlumnoId == AlumnoId).FirstOrDefault();
                    objAlumnoIncrito.PagoPlanId = PagoPlanId;
                    db.SaveChanges();
                    return 1;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    return 2;
                }
            }

        }
        public static DTOAlumnoInscrito ConsultarAlumnoInscrito(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.AlumnoInscrito
                        where a.AlumnoId == AlumnoId
                        select new DTOAlumnoInscrito
                        {
                            OfertaEducativaId = a.OfertaEducativaId,
                            Anio = a.Anio,
                            PeriodoId = a.PeriodoId,
                            AlumnoId = a.AlumnoId,
                            PagoPlanId = a.PagoPlanId,
                            FechaInscripcion = a.FechaInscripcion,
                            TurnoId = a.TurnoId,
                            UsuarioId = a.UsuarioId
                        }).FirstOrDefault();
            }
        }
        public static DTOAlumnoInscrito ConsultarAlumnoInscrito(int AlumnoId, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.AlumnoInscrito
                        where a.AlumnoId == AlumnoId && a.OfertaEducativaId == OfertaEducativaId
                        select new DTOAlumnoInscrito
                        {
                            OfertaEducativaId = a.OfertaEducativaId,
                            Anio = a.Anio,
                            PeriodoId = a.PeriodoId,
                            AlumnoId = a.AlumnoId,
                            PagoPlanId = a.PagoPlanId,
                            FechaInscripcion = a.FechaInscripcion,
                            TurnoId = a.TurnoId,
                            UsuarioId = a.UsuarioId
                        }).FirstOrDefault();
            }
        }
        public static DTOAlumnoInscrito ConsultarAlumnoInscrito(int AlumnoId, int OfertaEducativaId, int Anio, int Periodo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.AlumnoInscrito
                        where a.AlumnoId == AlumnoId && a.OfertaEducativaId == OfertaEducativaId
                        && a.Anio == Anio && a.PeriodoId == Periodo
                        select new DTOAlumnoInscrito
                        {
                            OfertaEducativaId = a.OfertaEducativaId,
                            Anio = a.Anio,
                            PeriodoId = a.PeriodoId,
                            AlumnoId = a.AlumnoId,
                            PagoPlanId = a.PagoPlanId,
                            FechaInscripcion = a.FechaInscripcion,
                            TurnoId = a.TurnoId,
                            UsuarioId = a.UsuarioId
                        }).FirstOrDefault();
            }
        }
        public static DTOAlumnoInscrito ConsultarAlumnoInscrito(int AlumnoId, int anio, int periodo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.AlumnoInscrito
                        where a.AlumnoId == AlumnoId && a.Anio == anio && a.PeriodoId == periodo
                        select new DTOAlumnoInscrito
                        {
                            OfertaEducativaId = a.OfertaEducativaId,
                            Anio = a.Anio,
                            PeriodoId = a.PeriodoId,
                            AlumnoId = a.AlumnoId,
                            PagoPlanId = a.PagoPlanId,
                            FechaInscripcion = a.FechaInscripcion,
                            TurnoId = a.TurnoId,
                            UsuarioId = a.UsuarioId
                        }).FirstOrDefault();
            }
        }
        public static DTOAlumnoInscrito ConsultarAlumnoInscrito(DTOAlumnoInscrito Buscar)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.AlumnoInscrito
                        where a.AlumnoId == Buscar.AlumnoId && a.Anio == Buscar.Anio &&
                        a.OfertaEducativaId == Buscar.OfertaEducativaId && a.PeriodoId == Buscar.PeriodoId
                        select new DTOAlumnoInscrito
                        {
                            OfertaEducativaId = a.OfertaEducativaId,
                            Anio = a.Anio,
                            PeriodoId = a.PeriodoId,
                            AlumnoId = a.AlumnoId,
                            PagoPlanId = a.PagoPlanId,
                            FechaInscripcion = a.FechaInscripcion,
                            TurnoId = a.TurnoId,
                            UsuarioId = a.UsuarioId
                        }).FirstOrDefault();
            }
        }
        public static DTOAlumnoInscrito InsertarAlumnoInscrito(DTOAlumnoInscrito objAlumnoInscrito)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.AlumnoInscrito.Add(new AlumnoInscrito
                {
                    AlumnoId = objAlumnoInscrito.AlumnoId,
                    OfertaEducativaId = objAlumnoInscrito.OfertaEducativaId,
                    Anio = objAlumnoInscrito.Anio,
                    PeriodoId = objAlumnoInscrito.PeriodoId,
                    FechaInscripcion = DateTime.Now,
                    PagoPlanId = objAlumnoInscrito.PagoPlanId,
                    UsuarioId = objAlumnoInscrito.UsuarioId,
                    TurnoId = objAlumnoInscrito.TurnoId,
                    EsEmpresa = objAlumnoInscrito.EsEmpresa,
                    EstatusId = 1,
                    HoraInscripcion = DateTime.Now.TimeOfDay,
                });
                db.SaveChanges();
                return new DTOAlumnoInscrito
                {
                    OfertaEducativaId = db.AlumnoInscrito.Local[0].OfertaEducativaId,
                    Anio = db.AlumnoInscrito.Local[0].Anio,
                    PeriodoId = db.AlumnoInscrito.Local[0].PeriodoId,
                    AlumnoId = db.AlumnoInscrito.Local[0].AlumnoId,
                    PagoPlanId = db.AlumnoInscrito.Local[0].PagoPlanId,
                    FechaInscripcion = db.AlumnoInscrito.Local[0].FechaInscripcion,
                    TurnoId = db.AlumnoInscrito.Local[0].TurnoId,
                    UsuarioId = db.AlumnoInscrito.Local[0].UsuarioId
                };
            }
        }
        public static DTOAlumnoInscrito InsertarAlumnoInscrito2(DTOAlumnoInscrito objAlumnoInscrito)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<AlumnoInscrito> lstAlumnoOfertas = db.AlumnoInscrito.Where(w => w.AlumnoId == objAlumnoInscrito.AlumnoId &&
                    w.OfertaEducativa.OfertaEducativaTipoId != 4).ToList();
                Alumno objAlumnoDb = db.Alumno.Where(a => a.AlumnoId == objAlumnoInscrito.AlumnoId).FirstOrDefault();
                //OFerta
                OfertaEducativa ofertaN = db.OfertaEducativa.Where(o => o.OfertaEducativaId == objAlumnoInscrito.OfertaEducativaId).FirstOrDefault();

                ////Actualiza MAtricula
                if (db.OfertaEducativa.Where(l => l.OfertaEducativaId == objAlumnoInscrito.OfertaEducativaId).FirstOrDefault().OfertaEducativaTipoId != 4)
                {
                    #region Bitacora Matricula
                    //if (lstAlumnoOfertas.Count > 0)
                    if (db.Matricula.Where(kl => kl.MatriculaId == objAlumnoDb.MatriculaId).ToList().Count == 0)
                    {
                        db.Matricula.Add(new Matricula
                        {
                            AlumnoId = objAlumnoDb.AlumnoId,
                            Anio = objAlumnoDb.Anio,
                            FechaAsignacion = objAlumnoDb.FechaRegistro,
                            MatriculaId = objAlumnoDb.MatriculaId,
                            OfertaEducativaId = objAlumnoInscrito.OfertaEducativaId,
                            PeriodoId = objAlumnoDb.PeriodoId,
                            UsuarioId = objAlumnoDb.UsuarioId
                        });
                    }
                    #endregion
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
                        UsuarioIdBitacora = objAlumnoInscrito.UsuarioId
                    });
                    #endregion
                    #region Update Alumno
                    objAlumnoDb.MatriculaId = Herramientas.Matricula.ObtenerMatricula(objAlumnoInscrito,
                        new DTOOfertaEducativa
                        {
                            OfertaEducativaId = objAlumnoInscrito.OfertaEducativaId,
                            Rvoe = db.OfertaEducativa.Where(l => l.OfertaEducativaId == objAlumnoInscrito.OfertaEducativaId).FirstOrDefault().Rvoe,
                        }, objAlumnoInscrito.AlumnoId);
                    objAlumnoDb.Anio = objAlumnoInscrito.Anio;
                    objAlumnoDb.PeriodoId = objAlumnoInscrito.PeriodoId;
                    objAlumnoDb.EstatusId = 1;
                    objAlumnoDb.FechaRegistro = DateTime.Now;
                    objAlumnoDb.UsuarioId = objAlumnoInscrito.UsuarioId;
                    #endregion

                }


                db.AlumnoInscrito.Add(new AlumnoInscrito
                {
                    AlumnoId = objAlumnoInscrito.AlumnoId,
                    OfertaEducativaId = objAlumnoInscrito.OfertaEducativaId,
                    Anio = objAlumnoInscrito.Anio,
                    PeriodoId = objAlumnoInscrito.PeriodoId,
                    FechaInscripcion = DateTime.Now,
                    HoraInscripcion = DateTime.Now.TimeOfDay,
                    PagoPlanId = objAlumnoInscrito.PagoPlanId,
                    UsuarioId = objAlumnoInscrito.UsuarioId,
                    TurnoId = objAlumnoInscrito.TurnoId,
                    EsEmpresa = objAlumnoInscrito.EsEmpresa,
                    EstatusId = 1
                });

                db.AlumnoCuatrimestre.Add(new AlumnoCuatrimestre
                {
                    AlumnoId = objAlumnoInscrito.AlumnoId,
                    OfertaEducativaId = objAlumnoInscrito.OfertaEducativaId,
                    Cuatrimestre = 1,
                    Anio = objAlumnoInscrito.Anio,
                    PeriodoId = objAlumnoInscrito.PeriodoId,
                    esRegular = true,
                    FechaAsignacion = DateTime.Now,
                    HoraAsignacion = DateTime.Now.TimeOfDay,
                    UsuarioId = objAlumnoInscrito.UsuarioId
                });

                //Configuracion Empresa 
                if (objAlumnoInscrito.EsEmpresa)
                {
                    if (objAlumnoDb.GrupoAlumnoConfiguracion.Count > 0)
                    {
                        objAlumnoDb.GrupoAlumnoConfiguracion.ToList().ForEach(l => { l.EstatusId = 2; });
                    }
                    db.GrupoAlumnoConfiguracion.Add(
                        new GrupoAlumnoConfiguracion
                        {
                            AlumnoId = objAlumnoInscrito.AlumnoId,
                            Anio = objAlumnoInscrito.Anio,
                            CuotaColegiatura = 0,
                            CuotaInscripcion = 0,
                            EsCuotaCongelada = false,
                            EsEspecial = false,
                            EsInscripcionCongelada = false,
                            EstatusId = 8, //Estatus Pendiente 
                            FechaRegistro = DateTime.Now,
                            GrupoId = null,
                            HoraRegistro = DateTime.Now.TimeOfDay,
                            PeriodoId = objAlumnoInscrito.PeriodoId,
                            OfertaEducativaId = objAlumnoInscrito.OfertaEducativaId,
                            UsuarioId = objAlumnoInscrito.UsuarioId,
                            PagoPlanId = null
                        });
                }

                if (ofertaN.OfertaEducativaTipoId != 4)
                {
                    List<AlumnoInscrito> OFertas = db.AlumnoInscrito
                        .Where(k => k.AlumnoId == objAlumnoInscrito.AlumnoId
                                && k.OfertaEducativaId != objAlumnoInscrito.OfertaEducativaId
                                && k.OfertaEducativa.OfertaEducativaTipoId != 4)
                                .ToList();
                
                    OFertas.ForEach(O => { O.EstatusId = 2; });
                }

                db.SaveChanges();
                return new DTOAlumnoInscrito
                {
                    OfertaEducativaId = db.AlumnoInscrito.Local[0].OfertaEducativaId,
                    Anio = db.AlumnoInscrito.Local[0].Anio,
                    PeriodoId = db.AlumnoInscrito.Local[0].PeriodoId,
                    AlumnoId = db.AlumnoInscrito.Local[0].AlumnoId,
                    PagoPlanId = db.AlumnoInscrito.Local[0].PagoPlanId,
                    FechaInscripcion = db.AlumnoInscrito.Local[0].FechaInscripcion,
                    TurnoId = db.AlumnoInscrito.Local[0].TurnoId,
                    UsuarioId = db.AlumnoInscrito.Local[0].UsuarioId
                };

            }
        }
        public static string NombreCalendario(int Alumno)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                string Archivo = "Documentos/";
                List<AlumnoInscrito> OfertasAlumno = db.AlumnoInscrito
                                                        .Where(al => al.AlumnoId == Alumno
                                                            && al.OfertaEducativa.OfertaEducativaTipoId != 4)
                                                        .OrderByDescending(a=> a.Anio)
                                                        .ThenByDescending(a=> a.PeriodoId)
                                                        .ToList();

                Archivo += OfertasAlumno?.FirstOrDefault()?.OfertaEducativa?.SucursalId == 2 ?
                    (OfertasAlumno?.FirstOrDefault()?.OfertaEducativa?.OfertaEducativaTipoId == 2 ? "Posgrado C.pdf" :
                    OfertasAlumno?.FirstOrDefault()?.OfertaEducativa?.OfertaEducativaTipoId == 3 ? "" : "") :
                    (OfertasAlumno?.FirstOrDefault()?.OfertaEducativa?.OfertaEducativaTipoId == 1 ? OfertasAlumno?.FirstOrDefault()?.OfertaEducativaId == 30 ? "Musica.pdf" : "Licenciatura.pdf" :
                    OfertasAlumno?.FirstOrDefault()?.OfertaEducativa?.OfertaEducativaTipoId == 2 ? "Posgrado.pdf" : OfertasAlumno?.FirstOrDefault()?.OfertaEducativa?.OfertaEducativaTipoId == 4 ? "Ingles.pdf" : "");
                

                return Archivo.Length > 11 ? Archivo : null;
            }
        }

        public static byte[] TraerDocumento(int DocumentoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    AlumnoInscritoDocumento objAlu = db.AlumnoInscritoDocumento.Where(a => a.AlumnoInscritoDocumentoId == DocumentoId).FirstOrDefault();

                    if (objAlu != null)
                    { return objAlu.Archivo; }
                    else { return null; }
                }
                catch
                {
                    return null;
                }
            }
        }

        public static string CambiarCarrera(int Alumnoid, int OfertaEducativaAct, int OfertaEducativaNue, int Usuarioid)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    Alumno objAlumno = db.Alumno.Where(d => d.AlumnoId == Alumnoid).FirstOrDefault();
                    if (objAlumno != null)
                    {
                        //AlumnoInscrito
                        AlumnoInscrito objAlIns = objAlumno.AlumnoInscrito.Where(s => s.OfertaEducativaId == OfertaEducativaAct).
                            OrderBy(a => a.Anio).ThenBy(s => s.PeriodoId).ToList().LastOrDefault();
                        if (objAlIns != null)
                        {
                            #region AlumnoDescuento
                            List<AlumnoDescuento> lstDescuetnoAlumnos = db.AlumnoDescuento.Where(s => s.AlumnoId == Alumnoid &&
                              s.OfertaEducativaId == OfertaEducativaAct && s.EstatusId != 3 && s.Anio == objAlIns.Anio &&
                              s.PeriodoId == objAlIns.PeriodoId).ToList();
                            if (lstDescuetnoAlumnos.Count > 0)
                            {
                                lstDescuetnoAlumnos.ForEach(d =>
                                {
                                    Descuento objd = db.Descuento.Where(o => o.PagoConceptoId == d.PagoConceptoId && o.OfertaEducativaId == OfertaEducativaNue
                                      && o.Descripcion == (db.Descuento.Where(d1 => d1.DescuentoId == d.DescuentoId).FirstOrDefault().Descripcion)).FirstOrDefault();

                                    d.OfertaEducativaId = OfertaEducativaNue;
                                    d.DescuentoId = objd.DescuentoId;
                                });
                            }
                            #endregion

                            #region Pago
                            List<Pago> lstPagos = db.Pago.Where(p => p.AlumnoId == Alumnoid && p.OfertaEducativaId == OfertaEducativaAct
                              && p.Anio == objAlIns.Anio && p.PeriodoId == objAlIns.PeriodoId && p.EstatusId != 2).ToList();
                            List<Pago> PagosNuevos = new List<Pago>();
                            if (lstPagos.Count > 0)
                            {
                                lstPagos.ForEach(k =>
                                {
                                    Cuota objC = db.Cuota.Where(c1 => c1.PagoConceptoId == k.Cuota1.PagoConceptoId &&
                                     c1.Anio == k.Cuota1.Anio && c1.PeriodoId == k.Cuota1.PeriodoId && c1.OfertaEducativaId == OfertaEducativaNue).FirstOrDefault();


                                });
                            }
                            #endregion
                        }
                        else { return "No tiene ese OfertaEducativa"; }
                    }
                    else { return "No Exixste"; }
                    return "No hay conexion....";
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
