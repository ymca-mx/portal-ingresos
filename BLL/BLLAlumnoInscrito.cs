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
                    objAlumnoIncrito.EstatusId = objAlumnoIncrito.OfertaEducativa.OfertaEducativaTipoId != 4 ? 8 : 1;
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

        public static object GuardarDescuentosNuevoIngreso(DTODescuentoAlumno ObjAlumno)
        {
            using(UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    int[] PagoConceptoId = { 1, 800, 802, 1000 };

                    AlumnoInscrito objAlumnoIncrito = db.AlumnoInscrito.Where(X => X.AlumnoId == ObjAlumno.AlumnoId).FirstOrDefault();
                    var Cuotas = db.Cuota.Where(c => c.OfertaEducativaId == ObjAlumno.OfertaEducativaId
                                                      && c.Anio == ObjAlumno.Anio
                                                      && c.PeriodoId == ObjAlumno.PeriodoId
                                                      && PagoConceptoId.Contains(c.PagoConceptoId))
                                                    .Select(c => new
                                                    {
                                                        c.CuotaId,
                                                        c.PagoConceptoId,
                                                        c.Monto
                                                    })
                                                    .ToList();

                    var Descuentos = db.Descuento.Where(d => d.OfertaEducativaId == ObjAlumno.OfertaEducativaId
                                                            && PagoConceptoId.Contains(d.PagoConceptoId)
                                                            && (d.Descripcion == "Beca Académica"
                                                                        || d.Descripcion == "Descuento en inscripción"
                                                                        || d.Descripcion == "Descuento en examen diagnóstico"
                                                                        || d.Descripcion == "Descuento en credencial nuevo ingreso"))
                                                    .Select(d => new
                                                    {
                                                        d.DescuentoId,
                                                        d.PagoConceptoId
                                                    })
                                                    .ToList();




                    objAlumnoIncrito.PagoPlanId = ObjAlumno.SistemaPagoId;
                    objAlumnoIncrito.EstatusId = objAlumnoIncrito.OfertaEducativa.OfertaEducativaTipoId != 4 ? 8 : 1;

                    if (db.AlumnoPassword.Where(a => a.AlumnoId == ObjAlumno.AlumnoId).ToList().Count == 0)
                    {
                        db.AlumnoPassword.Add(new AlumnoPassword
                        {
                            AlumnoId = ObjAlumno.AlumnoId,
                            Password = Utilities.Seguridad.Encripta(27, Herramientas.ConvertidorT.CrearPass())
                        });
                    }


                    Descuentos.ForEach(desc =>
                    {
                        var descuento = ObjAlumno.Descuentos.Find(a => a.PagoConceptoId == desc.PagoConceptoId);
                        var cuota = Cuotas.Find(a => a.PagoConceptoId == desc.PagoConceptoId);

                        if (descuento != null)
                        {
                            if (descuento.TotalPagar != cuota.Monto)
                            {
                                db.AlumnoDescuento.Add(new AlumnoDescuento
                                {
                                    AlumnoId = ObjAlumno.AlumnoId,
                                    OfertaEducativaId = ObjAlumno.OfertaEducativaId,
                                    Anio = ObjAlumno.Anio,
                                    PeriodoId = ObjAlumno.PeriodoId,
                                    DescuentoId = desc.DescuentoId,
                                    PagoConceptoId = desc.PagoConceptoId,
                                    Monto = Math.Round(100 - ((descuento.TotalPagar * 100) / cuota.Monto)),
                                    UsuarioId = ObjAlumno.UsuarioId,
                                    Comentario = descuento.Justificacion,
                                    FechaGeneracion = DateTime.Now,
                                    HoraGeneracion = DateTime.Now.TimeOfDay,
                                    EsSEP = false,
                                    EsComite = false,
                                    EsDeportiva = false,
                                    FechaAplicacion = DateTime.Now,
                                    EstatusId = 2,
                                });
                            }

                            #region pagos

                            if (objAlumnoIncrito.EstatusId != 8)
                            {

                                int NPagos = desc.PagoConceptoId == 800 ? 4 : 1;

                                for (int indice = 1; indice <= NPagos; indice++)
                                {
                                    db.Pago.Add(new Pago
                                    {
                                        ReferenciaId = "",
                                        AlumnoId = ObjAlumno.AlumnoId,
                                        Anio = ObjAlumno.Anio,
                                        PeriodoId = ObjAlumno.PeriodoId,
                                        SubperiodoId = indice,
                                        OfertaEducativaId = ObjAlumno.OfertaEducativaId,
                                        FechaGeneracion = DateTime.Now,
                                        HoraGeneracion = DateTime.Now.TimeOfDay,
                                        CuotaId = cuota.CuotaId,
                                        Cuota = cuota.Monto,
                                        Promesa = descuento.TotalPagar,
                                        Restante = descuento.TotalPagar,
                                        EstatusId = descuento.TotalPagar == 0 ? 4 : 1,
                                        UsuarioTipoId = 1,
                                        EsEmpresa = false,
                                        EsAnticipado = false,
                                        PagoDescuento = descuento.TotalPagar != cuota.Monto ? new List<PagoDescuento>
                                {
                                    new PagoDescuento
                                    {
                                        DescuentoId=desc.DescuentoId,
                                        Monto=(cuota.Monto-descuento.TotalPagar)
                                    }
                                } : null
                                    });
                                }
                            }

                            #endregion
                        }
                        });

                    db.SaveChanges();

                    db.Pago.Local.ToList().ForEach(pago =>
                    {
                        pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId).FirstOrDefault();
                    });

                    db.AlumnoDescuento.Local.ToList().ForEach(desc =>
                    {
                        var objdesc = ObjAlumno.Descuentos.Find(a => a.PagoConceptoId == desc.PagoConceptoId);

                        if (objdesc.Comprobante != null)
                        {
                            db.AlumnoDescuentoDocumento.Add(new AlumnoDescuentoDocumento
                            {
                                AlumnoDescuentoDocumento1 = objdesc.Comprobante,
                                AlumnoDescuentoId = desc.AlumnoDescuentoId
                            });
                        }
                    });

                    db.SaveChanges();

                    return new
                    {
                        ObjAlumno.AlumnoId,
                        Estatus = "Se guardo Correctamente"
                    };
                }
                catch(Exception error )
                {
                    return new
                    {
                        error.Message,
                        inner = error?.InnerException?.Message
                    };
                }
            }
        }

        public static object InscribirNuevaOferta(DTOAlumno objAlumno)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {
                    Alumno alumnodb = db.Alumno.Where(a => a.AlumnoId == objAlumno.AlumnoId)
                         .FirstOrDefault();

                    if (alumnodb != null)
                    {
                        db.AlumnoBitacora.Add(new AlumnoBitacora
                        {
                            AlumnoId = alumnodb.AlumnoId,
                            Anio = alumnodb.Anio,
                            EstatusId = alumnodb.EstatusId,
                            Fecha = DateTime.Now,
                            FechaRegistro = alumnodb.FechaRegistro,
                            Materno = alumnodb.Materno,
                            MatriculaId = alumnodb.MatriculaId,
                            Nombre = alumnodb.Nombre,
                            Paterno = alumnodb.Paterno,
                            PeriodoId = alumnodb.PeriodoId,
                            UsuarioId = alumnodb.UsuarioId,
                            UsuarioIdBitacora = objAlumno.UsuarioId
                        });

                        if (db.Matricula.Where(kl => kl.MatriculaId == alumnodb.MatriculaId).ToList().Count == 0)
                        {
                            db.Matricula.Add(new Matricula
                            {
                                AlumnoId = alumnodb.AlumnoId,
                                Anio = alumnodb.Anio,
                                FechaAsignacion = alumnodb.FechaRegistro,
                                MatriculaId = alumnodb.MatriculaId,
                                OfertaEducativaId = objAlumno.AlumnoInscrito.OfertaEducativaId,
                                PeriodoId = alumnodb.PeriodoId,
                                UsuarioId = alumnodb.UsuarioId
                            });
                        }

                        db.SaveChanges();

                        alumnodb.Anio = objAlumno.Anio;
                        alumnodb.PeriodoId = objAlumno.PeriodoId;
                        alumnodb.FechaRegistro = DateTime.Now;
                        alumnodb.MatriculaId = Herramientas.Matricula
                                            .GenerarMatricula(objAlumno.Anio, objAlumno.PeriodoId, objAlumno.AlumnoId,
                                                        db.OfertaEducativa.Where(a => a.OfertaEducativaId == objAlumno.AlumnoInscrito.OfertaEducativaId)
                                                        .FirstOrDefault()?.Rvoe ?? null, objAlumno.AlumnoInscrito.TurnoId);

                        AlumnoInscrito AlumnoInscritodb = db.AlumnoInscrito
                                                        .Where(a => a.AlumnoId == objAlumno.AlumnoId
                                                                && a.OfertaEducativaId == objAlumno.AlumnoInscrito.OfertaEducativaId)
                                                        .FirstOrDefault();

                        if (AlumnoInscritodb != null)
                        {
                            db.AlumnoInscrito.Remove(AlumnoInscritodb);
                            db.SaveChanges();
                        }

                        List<AlumnoInscrito> lssOFertasInscritas = db.AlumnoInscrito
                                                                    .Where(a => a.AlumnoId == objAlumno.AlumnoId)
                                                                    .ToList();

                        lssOFertasInscritas.ForEach(oferta =>
                        {
                            if (db.AlumnoInscritoBitacora.Where(a =>
                                 a.AlumnoId == objAlumno.AlumnoId
                                 && a.OfertaEducativaId == objAlumno.AlumnoInscrito.OfertaEducativaId
                                 && a.Anio == objAlumno.Anio
                                 && a.PeriodoId == objAlumno.PeriodoId
                                 && a.HoraInscripcion == oferta.HoraInscripcion
                                 && a.FechaInscripcion == oferta.FechaInscripcion)
                               .ToList().Count == 0)
                            {
                                db.AlumnoInscritoBitacora.Add(new AlumnoInscritoBitacora
                                {
                                    AlumnoId = oferta.AlumnoId,
                                    Anio = oferta.Anio,
                                    EsEmpresa = oferta.EsEmpresa,
                                    FechaInscripcion = oferta.FechaInscripcion,
                                    HoraInscripcion = oferta.HoraInscripcion,
                                    OfertaEducativaId = oferta.OfertaEducativaId,
                                    PagoPlanId = oferta.PagoPlanId,
                                    PeriodoId = oferta.PeriodoId,
                                    TurnoId = oferta.TurnoId,
                                    UsuarioId = oferta.UsuarioId
                                });

                                oferta.EstatusId = 2;
                                oferta.UsuarioId = objAlumno.UsuarioId;
                                oferta.HoraInscripcion = DateTime.Now.TimeOfDay;
                                oferta.FechaInscripcion = DateTime.Now;

                                db.SaveChanges();
                            }
                        });

                        AlumnoCuatrimestre AlumnoCuatridb = db.AlumnoCuatrimestre
                                                            .Where(a => a.AlumnoId == objAlumno.AlumnoId
                                                                    && a.OfertaEducativaId == objAlumno.AlumnoInscrito.OfertaEducativaId)
                                                            .FirstOrDefault();
                        if (AlumnoCuatridb != null)
                        {
                            db.AlumnoCuatrimestre.Remove(AlumnoCuatridb);
                            db.SaveChanges();
                        }

                        db.AlumnoCuatrimestre.Add(new AlumnoCuatrimestre
                        {
                            AlumnoId = alumnodb.AlumnoId,
                            OfertaEducativaId = objAlumno.AlumnoInscrito.OfertaEducativaId,
                            Cuatrimestre = 1,
                            Anio = objAlumno.Anio,
                            PeriodoId = objAlumno.PeriodoId,
                            esRegular = true,
                            FechaAsignacion = DateTime.Now,
                            HoraAsignacion = DateTime.Now.TimeOfDay,
                            UsuarioId = objAlumno.UsuarioId
                        });

                        db.AlumnoInscrito.Add(new AlumnoInscrito
                        {
                            AlumnoId=objAlumno.AlumnoId,
                            Anio = objAlumno.Anio,
                            EsEmpresa = objAlumno.AlumnoInscrito.EsEmpresa,
                            EstatusId = objAlumno.AlumnoInscrito.EsEmpresa ? 1 : 8,
                            FechaInscripcion = DateTime.Now,
                            HoraInscripcion = DateTime.Now.TimeOfDay,
                            OfertaEducativaId = objAlumno.AlumnoInscrito.OfertaEducativaId,
                            PagoPlanId = objAlumno.AlumnoInscrito.EsEmpresa ? null : objAlumno.AlumnoInscrito.PagoPlanId,
                            PeriodoId = objAlumno.PeriodoId,
                            TurnoId = objAlumno.AlumnoInscrito.TurnoId,
                            UsuarioId = objAlumno.UsuarioId
                        });

                        #region Grupo Alumno
                        if (objAlumno.AlumnoInscrito.EsEmpresa)
                        {
                            GrupoAlumnoConfiguracion GrupoAlumnodb = db.GrupoAlumnoConfiguracion
                                .Where(a => a.AlumnoId == objAlumno.AlumnoId
                                        && a.OfertaEducativaId == objAlumno.AlumnoInscrito.OfertaEducativaId)
                                .FirstOrDefault();

                            if (GrupoAlumnodb != null)
                            {
                                db.GrupoAlumnoConfiguracionBitacora.Add(new GrupoAlumnoConfiguracionBitacora
                                {
                                    AlumnoId = GrupoAlumnodb.AlumnoId,
                                    Anio = GrupoAlumnodb.Anio,
                                    CuotaColegiatura = GrupoAlumnodb.CuotaColegiatura,
                                    CuotaInscripcion = GrupoAlumnodb.CuotaInscripcion,
                                    EsCuotaCongelada = GrupoAlumnodb.EsCuotaCongelada,
                                    EsEspecial = GrupoAlumnodb.EsEspecial,
                                    EsInscripcionCongelada = GrupoAlumnodb.EsInscripcionCongelada,
                                    FechaRegistro = GrupoAlumnodb.FechaRegistro,
                                    GrupoId = GrupoAlumnodb.GrupoId,
                                    HoraRegistro = GrupoAlumnodb.HoraRegistro,
                                    NumeroPagos = GrupoAlumnodb.NumeroPagos,
                                    OfertaEducativaId = GrupoAlumnodb.OfertaEducativaId,
                                    PagoPlanId = GrupoAlumnodb.PagoPlanId,
                                    PeriodoId = GrupoAlumnodb.PeriodoId,
                                    UsuarioId = GrupoAlumnodb.UsuarioId
                                });

                                GrupoAlumnodb.Anio = objAlumno.Anio;
                                GrupoAlumnodb.PeriodoId = objAlumno.PeriodoId;
                                GrupoAlumnodb.GrupoId = null;
                                GrupoAlumnodb.CuotaColegiatura = 0;
                                GrupoAlumnodb.CuotaInscripcion = 0;
                                GrupoAlumnodb.EsCuotaCongelada = false;
                                GrupoAlumnodb.EsInscripcionCongelada = false;
                                GrupoAlumnodb.EsEspecial = false;
                                GrupoAlumnodb.UsuarioId = objAlumno.UsuarioId;
                                GrupoAlumnodb.PagoPlanId = null;
                                GrupoAlumnodb.NumeroPagos = null;
                                GrupoAlumnodb.HoraRegistro = DateTime.Now.TimeOfDay;
                                GrupoAlumnodb.FechaRegistro = DateTime.Now;
                                GrupoAlumnodb.EstatusId = 8;
                            }
                            else
                            {
                                db.GrupoAlumnoConfiguracion.Add(
                                    new GrupoAlumnoConfiguracion
                                    {
                                        AlumnoId = objAlumno.AlumnoId,
                                        OfertaEducativaId = objAlumno.AlumnoInscrito.OfertaEducativaId,
                                        Anio = objAlumno.Anio,
                                        PeriodoId = objAlumno.PeriodoId,
                                        GrupoId = null,
                                        CuotaColegiatura = 0,
                                        CuotaInscripcion = 0,
                                        EsCuotaCongelada = false,
                                        EsEspecial = false,
                                        EsInscripcionCongelada = false,
                                        UsuarioId = objAlumno.UsuarioId,
                                        PagoPlanId = null,
                                        NumeroPagos = null,
                                        FechaRegistro = DateTime.Now,
                                        HoraRegistro = DateTime.Now.TimeOfDay,
                                        EstatusId = 8
                                    });
                            }
                        }
                        #endregion

                        #region Material Didactico
                        if (objAlumno.AlumnoInscrito.Material)
                        {
                            Cuota CuotaMateria = db.Cuota
                                        .Where(a => a.OfertaEducativaId == objAlumno.AlumnoInscrito.OfertaEducativaId
                                            && a.Anio == objAlumno.AlumnoInscrito.Anio
                                            && a.PeriodoId == objAlumno.AlumnoInscrito.PeriodoId
                                            && a.PagoConceptoId == 808)
                                        .FirstOrDefault();

                            if (CuotaMateria != null)
                            {
                                db.Pago.Add(new Pago
                                {
                                    AlumnoId = objAlumno.AlumnoId,
                                    Anio = objAlumno.AlumnoInscrito.Anio,
                                    PeriodoId = objAlumno.AlumnoInscrito.PeriodoId,
                                    SubperiodoId = 1,
                                    OfertaEducativaId = objAlumno.AlumnoInscrito.OfertaEducativaId,
                                    FechaGeneracion = DateTime.Now,
                                    CuotaId = CuotaMateria.CuotaId,
                                    Cuota = CuotaMateria.Monto,
                                    Promesa = CuotaMateria.Monto,
                                    Restante = CuotaMateria.Monto,
                                    EstatusId = 1,
                                    ReferenciaId = "",
                                    EsEmpresa = false,
                                    UsuarioId = objAlumno.UsuarioId,
                                    UsuarioTipoId = 1,
                                    HoraGeneracion = DateTime.Now.TimeOfDay
                                });
                            }
                        }
                        #endregion

                        db.SaveChanges();

                        db.Pago.Local.ToList().ForEach(pago =>
                        {
                            pago.ReferenciaId = db.spGeneraReferencia(pago.PagoId).FirstOrDefault();
                        });

                        db.SaveChanges();
                    }

                    return new
                    {
                        objAlumno.AlumnoId,
                        Message = "Todo Correcto",
                        Estatus = true
                    };
                }
                catch (Exception Error)
                {
                    return new
                    {
                        Estatus = false,
                        Error.Message,
                        Inner = Error?.InnerException?.Message ?? ""
                    };
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
                            UsuarioId = a.UsuarioId,
                            EstatusId=a.EstatusId
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
                            UsuarioId = a.UsuarioId,
                            EstatusId=a.EstatusId
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

                    db.Entry(objAlumnoDb).State = System.Data.Entity.EntityState.Modified;

                    objAlumnoDb.MatriculaId = Herramientas.Matricula.ObtenerMatricula(objAlumnoInscrito,
                        new DTOOfertaEducativa
                        {
                            OfertaEducativaId = objAlumnoInscrito.OfertaEducativaId,
                            Rvoe = db.OfertaEducativa.Where(l => l.OfertaEducativaId == objAlumnoInscrito.OfertaEducativaId).FirstOrDefault().Rvoe,
                        }, objAlumnoInscrito.AlumnoId);
                    objAlumnoDb.Anio = objAlumnoInscrito.Anio;
                    objAlumnoDb.PeriodoId = objAlumnoInscrito.PeriodoId;
                    objAlumnoDb.EstatusId = ofertaN.OfertaEducativaTipoId != 4 ? 8 : 1;
                    objAlumnoDb.FechaRegistro = DateTime.Now;
                    objAlumnoDb.UsuarioId = objAlumnoInscrito.UsuarioId;
                    
                    #endregion

                }

                /////Si en alumno inscrito hay un registro con la misma oferta la mandamos a bitacora y la eliminamos de la tabla
                AlumnoInscrito AlumnoInscritoOferta = db.AlumnoInscrito.Where(a => a.AlumnoId == objAlumnoInscrito.AlumnoId
                                            && a.OfertaEducativaId == objAlumnoInscrito.OfertaEducativaId).FirstOrDefault() ?? null;
                if (AlumnoInscritoOferta != null)
                {
                    db.AlumnoInscritoBitacora.Add(new AlumnoInscritoBitacora
                    {
                        AlumnoId = AlumnoInscritoOferta.AlumnoId,
                        Anio = AlumnoInscritoOferta.Anio,
                        EsEmpresa = AlumnoInscritoOferta.EsEmpresa,
                        FechaInscripcion = AlumnoInscritoOferta.FechaInscripcion,
                        HoraInscripcion = AlumnoInscritoOferta.HoraInscripcion,
                        OfertaEducativaId = AlumnoInscritoOferta.OfertaEducativaId,
                        PagoPlanId = AlumnoInscritoOferta.PagoPlanId,
                        PeriodoId = AlumnoInscritoOferta.PeriodoId,
                        TurnoId = AlumnoInscritoOferta.TurnoId,
                        UsuarioId = AlumnoInscritoOferta.UsuarioId
                    });
                    db.AlumnoInscrito.Remove(AlumnoInscritoOferta);
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
                    EstatusId = objAlumnoInscrito.EsEmpresa ? 1 : 8
                });

                if (db.AlumnoCuatrimestre.Where(ac => ac.AlumnoId == objAlumnoInscrito.AlumnoId
                                                     && ac.OfertaEducativaId == objAlumnoInscrito.OfertaEducativaId
                                                     && ac.Anio == objAlumnoInscrito.Anio
                                                     && ac.PeriodoId == objAlumnoInscrito.PeriodoId).ToList().Count == 0
                                                     && db.OfertaEducativa.Where(of=> of.OfertaEducativaId==objAlumnoInscrito.OfertaEducativaId
                                                                    && of.OfertaEducativaTipoId==4)                                                        
                                                                .ToList().Count>0)
                {
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
                }
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

        public static bool ActializarAlumnoInscrito(DTOAlumnoInscrito objinsc, int AnioN, int PeriodoIdN)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    string RVO = db.OfertaEducativa.Where(a => a.OfertaEducativaId == objinsc.OfertaEducativaId).FirstOrDefault()?.Rvoe ?? "";
                    string Matricula = Herramientas.Matricula.GenerarMatricula(AnioN, PeriodoIdN, objinsc.AlumnoId, RVO, objinsc.TurnoId);

                    Alumno AlumnoBase = db.Alumno.Where(a => a.AlumnoId == objinsc.AlumnoId).FirstOrDefault();
                    #region Matricula
                    db.Matricula.Add(new Matricula
                    {
                        AlumnoId = objinsc.AlumnoId,
                        Anio = AnioN,
                        MatriculaId = Matricula,
                        FechaAsignacion = DateTime.Now,
                        OfertaEducativaId = objinsc.OfertaEducativaId,
                        PeriodoId = PeriodoIdN,
                        UsuarioId = objinsc.UsuarioId
                    });
                    #endregion
                    #region Alumno
                    db.AlumnoBitacora.Add(new AlumnoBitacora
                    {
                        AlumnoId = AlumnoBase.AlumnoId,
                        Anio = AlumnoBase.Anio,
                        EstatusId = AlumnoBase.EstatusId,
                        Fecha = DateTime.Now,
                        FechaRegistro = AlumnoBase.FechaRegistro,
                        Materno = AlumnoBase.Materno,
                        MatriculaId = AlumnoBase.MatriculaId,
                        Nombre = AlumnoBase.Nombre,
                        Paterno = AlumnoBase.Paterno,
                        PeriodoId = AlumnoBase.PeriodoId,
                        UsuarioId = AlumnoBase.UsuarioId,
                        UsuarioIdBitacora = objinsc.UsuarioId
                    });

                    db.Entry(AlumnoBase).State = System.Data.Entity.EntityState.Modified;

                    AlumnoBase.MatriculaId = Matricula;
                    AlumnoBase.Anio = AnioN;
                    AlumnoBase.PeriodoId = PeriodoIdN;
                    AlumnoBase.FechaRegistro = DateTime.Now;
                    AlumnoBase.UsuarioId = objinsc.UsuarioId;
                    #endregion
                    #region AlumnoInscrito
                    
                    AlumnoInscrito AlumnoInscritoDB = db.AlumnoInscrito.Where(a =>
                                                            a.AlumnoId == objinsc.AlumnoId
                                                            && a.Anio == objinsc.Anio
                                                            && a.PeriodoId == objinsc.PeriodoId
                                                            && a.OfertaEducativaId == objinsc.OfertaEducativaId)
                                                            .FirstOrDefault();
                    if (AlumnoInscritoDB != null)
                    {
                        

                        db.AlumnoInscritoBitacora.Add(new AlumnoInscritoBitacora
                        {
                            AlumnoId = objinsc.AlumnoId,
                            Anio = AlumnoInscritoDB.Anio,
                            EsEmpresa = AlumnoInscritoDB.EsEmpresa,
                            FechaInscripcion = AlumnoInscritoDB.FechaInscripcion,
                            HoraInscripcion = AlumnoInscritoDB.HoraInscripcion,
                            OfertaEducativaId = AlumnoInscritoDB.OfertaEducativaId,
                            PagoPlanId = AlumnoInscritoDB.PagoPlanId,
                            PeriodoId = AlumnoInscritoDB.PeriodoId,
                            TurnoId = AlumnoInscritoDB.TurnoId,
                            UsuarioId = objinsc.UsuarioId
                        });
                        db.AlumnoInscrito.Remove(AlumnoInscritoDB);
                    }

                    db.AlumnoInscrito.Add(new AlumnoInscrito
                    {
                        AlumnoId = objinsc.AlumnoId,
                        Anio = AnioN,
                        EsEmpresa = false,
                        EstatusId = db.OfertaEducativa.Where(of => of.OfertaEducativaId == objinsc.OfertaEducativaId
                                   && of.OfertaEducativaTipoId == 4).ToList().Count > 0 ? 1 : 8,
                        FechaInscripcion = DateTime.Now,
                        HoraInscripcion = DateTime.Now.TimeOfDay,
                        OfertaEducativaId = objinsc.OfertaEducativaId,
                        PagoPlanId = objinsc.PagoPlanId,
                        PeriodoId = PeriodoIdN,
                        TurnoId = objinsc.TurnoId,
                        UsuarioId = objinsc.UsuarioId
                    });

                    #endregion
                    #region Alumno Cuatrimestre
                    AlumnoCuatrimestre alumnoCuatrimestre =
                        db.AlumnoCuatrimestre.Where(a =>
                            a.AlumnoId == objinsc.AlumnoId
                            && a.OfertaEducativaId == objinsc.OfertaEducativaId)
                            .FirstOrDefault();
                    if (alumnoCuatrimestre != null)
                    {
                        db.AlumnoCuatrimestreBitacora.Add(new AlumnoCuatrimestreBitacora
                        {
                            AlumnoId = alumnoCuatrimestre.AlumnoId,
                            Anio = alumnoCuatrimestre.Anio,
                            Cuatrimestre = alumnoCuatrimestre.Cuatrimestre,
                            esRegular = alumnoCuatrimestre.esRegular,
                            FechaAsignacion = alumnoCuatrimestre.FechaAsignacion,
                            HoraAsignacion = alumnoCuatrimestre.HoraAsignacion,
                            OfertaEducativaId = alumnoCuatrimestre.OfertaEducativaId,
                            PeriodoId = alumnoCuatrimestre.PeriodoId,
                            UsuarioId = alumnoCuatrimestre.UsuarioId
                        });
                        db.AlumnoCuatrimestre.Remove(alumnoCuatrimestre);
                    }
                    db.AlumnoCuatrimestre.Add(new AlumnoCuatrimestre
                    {
                        AlumnoId = objinsc.AlumnoId,
                        OfertaEducativaId = objinsc.OfertaEducativaId,
                        Cuatrimestre = 1,
                        Anio = AnioN,
                        PeriodoId = PeriodoIdN,
                        esRegular = true,
                        FechaAsignacion = DateTime.Now,
                        HoraAsignacion = DateTime.Now.TimeOfDay,
                        UsuarioId = objinsc.UsuarioId
                    });



                    #endregion

                    db.SaveChanges();
                    return true;
                }
                catch { return false; }
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
