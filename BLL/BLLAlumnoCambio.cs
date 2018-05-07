using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;

namespace BLL
{
    public class BLLAlumnoCambio
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
    int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll")]
        private static extern Boolean CloseHandle(IntPtr hObject);

        public static object CambioGnral(DTOAlumnoOfertaCuotas Alumno)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<int> LstConceptos = new List<int> { 1, 800, 802, 1000 };
                    OfertaEducativa Ofertadb = db.OfertaEducativa.Where(of => of.OfertaEducativaId == Alumno.OfertaEducativaIdNueva).FirstOrDefault();
                    Alumno AlumnoDb = db.Alumno.Where(a => a.AlumnoId == Alumno.AlumnoId).FirstOrDefault();
                    List<Cuota> lstCutoasNuevas = db.Cuota
                                            .Where(cu => cu.Anio == Alumno.Anio
                                                        && cu.PeriodoId == Alumno.PeriodoId
                                                        && cu.OfertaEducativaId == Alumno.OfertaEducativaIdNueva
                                                        && LstConceptos.Contains(cu.PagoConceptoId))
                                            .ToList();
                    List<Cuota> lstCutoasAnteriores = db.Cuota
                                            .Where(cu => cu.Anio == Alumno.AnioAnterior
                                                        && cu.PeriodoId == Alumno.PeriodoIdAnterior
                                                        && cu.OfertaEducativaId == Alumno.OfertaEducativaIdActual
                                                        && LstConceptos.Contains(cu.PagoConceptoId))
                                            .ToList();
                    var lstDescuentos = Alumno.EsEmpresa ? (db.Descuento
                                                                .Where(de => LstConceptos.Contains(de.PagoConceptoId)
                                                                    && de.OfertaEducativaId == Alumno.OfertaEducativaIdNueva
                                                                    && (de.Descripcion == "Descuento en colegiatura"
                                                                        || de.Descripcion == "Descuento en inscripción"
                                                                        || de.Descripcion == "Descuento en examen diagnóstico"
                                                                        || de.Descripcion == "Descuento en credencial nuevo ingreso")).ToList())
                                        : (db.Descuento
                                                                .Where(de => LstConceptos.Contains(de.PagoConceptoId)
                                                                    && de.OfertaEducativaId == Alumno.OfertaEducativaIdNueva
                                                                    && (de.Descripcion == "Beca Académica"
                                                                        || de.Descripcion == "Descuento en inscripción"
                                                                        || de.Descripcion == "Descuento en examen diagnóstico"
                                                                        || de.Descripcion == "Descuento en credencial nuevo ingreso")).ToList());

                    decimal DescuentoColegiatura = (100 - (Alumno.MontoColegiatura * 100) / (lstCutoasNuevas.Find(a => a.PagoConceptoId == 800)?.Monto ?? 0)),
                        DescuentoInscripcion = (100 - (Alumno.MontoInscripcion * 100) / (lstCutoasNuevas.Find(a => a.PagoConceptoId == 802)?.Monto ?? 0)),
                        DescuentoCredencial = (100 - (Alumno.MontoCredencial * 100) / (lstCutoasNuevas.Find(a => a.PagoConceptoId == 1000)?.Monto ?? 0)),
                        DescuentoExamen = (100 - (Alumno.MontoExamen * 100) / (lstCutoasNuevas.Find(a => a.PagoConceptoId == 1)?.Monto ?? 0));


                    if ((Ofertadb?.Rvoe ?? "").Length > 0)
                    {
                        AlumnoDb.MatriculaId = Herramientas.Matricula.GenerarMatricula(Alumno.Anio, Alumno.PeriodoId, Alumno.AlumnoId, Ofertadb.Rvoe, Alumno.TurnoId);
                    }
                    else { AlumnoDb.MatriculaId = "0"; }

                    AlumnoDb.Anio = Alumno.Anio;
                    AlumnoDb.PeriodoId = Alumno.PeriodoId;



                    AlumnoInscrito AlumnoInscritodb =
                    AlumnoDb.AlumnoInscrito.Where(a => a.Anio == Alumno.AnioAnterior
                                                        && a.PeriodoId == Alumno.PeriodoIdAnterior
                                                        && a.OfertaEducativaId == Alumno.OfertaEducativaIdActual)
                                                  .FirstOrDefault();

                    AlumnoCuatrimestre AlumnoCuatrimestredb=
                        AlumnoDb.AlumnoCuatrimestre.Where(a => a.Anio == Alumno.AnioAnterior
                                                        && a.PeriodoId == Alumno.PeriodoIdAnterior
                                                        && a.OfertaEducativaId == Alumno.OfertaEducativaIdActual)
                                                  .FirstOrDefault();

                    GrupoAlumnoConfiguracion AlumnoConfiguraciondb =
                        AlumnoDb.GrupoAlumnoConfiguracion.Where(a => a.Anio == Alumno.AnioAnterior
                                                        && a.PeriodoId == Alumno.PeriodoIdAnterior
                                                        && a.OfertaEducativaId == Alumno.OfertaEducativaIdActual)
                                                  .FirstOrDefault();

                   List<AlumnoDescuento> AlumnoDescuentodb=
                        AlumnoDb.AlumnoDescuento.Where(a => a.Anio == Alumno.AnioAnterior
                                                        && a.PeriodoId == Alumno.PeriodoIdAnterior
                                                        && a.OfertaEducativaId == Alumno.OfertaEducativaIdActual)
                                                  .ToList();

                    List<Pago> PagosAlumno=
                        AlumnoDb.Pago.Where(a => a.Anio == Alumno.AnioAnterior
                                                        && a.PeriodoId == Alumno.PeriodoIdAnterior
                                                        && a.OfertaEducativaId == Alumno.OfertaEducativaIdActual)
                                                  .ToList();

                    if (AlumnoInscritodb != null)
                    {
                        AlumnoInscrito alumnoInscritoadd = new AlumnoInscrito
                        {
                            Anio = Alumno.Anio,
                            EsEmpresa = Alumno.EsEmpresa,
                            EstatusId = AlumnoInscritodb.EstatusId,
                            FechaInscripcion = AlumnoInscritodb.FechaInscripcion,
                            HoraInscripcion = AlumnoInscritodb.HoraInscripcion,
                            OfertaEducativaId = Alumno.OfertaEducativaIdNueva,
                            PagoPlanId = AlumnoInscritodb.PagoPlanId,
                            PeriodoId = Alumno.PeriodoId,
                            TurnoId = Alumno.TurnoId,
                            UsuarioId = Alumno.UsuarioId
                        };

                        AlumnoDb.AlumnoInscrito.Remove(AlumnoInscritodb);

                        db.SaveChanges();
                        AlumnoDb.AlumnoInscrito.Add(alumnoInscritoadd);
                        db.SaveChanges();
                    }

                    if (AlumnoCuatrimestredb != null && Alumno.EsEmpresa)
                    {
                        AlumnoCuatrimestre AlumnoCuatrimestreAdd = new AlumnoCuatrimestre
                        {
                            AlumnoId = Alumno.AlumnoId,
                            Anio = Alumno.Anio,
                            Cuatrimestre = 1,
                            esRegular = true,
                            FechaAsignacion = AlumnoCuatrimestredb.FechaAsignacion,
                            HoraAsignacion = AlumnoCuatrimestredb.HoraAsignacion,
                            OfertaEducativaId = Alumno.OfertaEducativaIdNueva,
                            PeriodoId = Alumno.PeriodoId,
                            UsuarioId = Alumno.UsuarioId
                        };

                        AlumnoDb.AlumnoCuatrimestre.Remove(AlumnoCuatrimestredb);
                        db.SaveChanges();

                        AlumnoDb.AlumnoCuatrimestre.Add(AlumnoCuatrimestreAdd);
                    }
                    else if(AlumnoCuatrimestredb!=null && !Alumno.EsEmpresa)
                    {
                        AlumnoDb.AlumnoCuatrimestre.Remove(AlumnoCuatrimestredb);
                        db.SaveChanges();
                    }

                    if (AlumnoConfiguraciondb != null)
                    {
                        db.GrupoAlumnoConfiguracionBitacora.Add(new GrupoAlumnoConfiguracionBitacora
                        {
                            AlumnoId = AlumnoConfiguraciondb.AlumnoId,
                            Anio = AlumnoConfiguraciondb.Anio,
                            UsuarioId = AlumnoConfiguraciondb.UsuarioId,
                            OfertaEducativaId = AlumnoConfiguraciondb.OfertaEducativaId,
                            CuotaColegiatura = AlumnoConfiguraciondb.CuotaColegiatura,
                            CuotaInscripcion = AlumnoConfiguraciondb.CuotaInscripcion,
                            EsCuotaCongelada = AlumnoConfiguraciondb.EsCuotaCongelada,
                            EsEspecial = AlumnoConfiguraciondb.EsEspecial,
                            EsInscripcionCongelada = AlumnoConfiguraciondb.EsInscripcionCongelada,
                            FechaRegistro = DateTime.Now,
                            GrupoId = AlumnoConfiguraciondb.GrupoId,
                            HoraRegistro =  DateTime.Now.TimeOfDay,
                            NumeroPagos = AlumnoConfiguraciondb.NumeroPagos,
                            PagoPlanId = AlumnoConfiguraciondb.PagoPlanId,
                            PeriodoId = AlumnoConfiguraciondb.PeriodoId
                            
                        });
                        db.SaveChanges();

                        if (Alumno.EsEmpresa)
                        {
                            AlumnoConfiguraciondb.Anio = Alumno.Anio;
                            AlumnoConfiguraciondb.OfertaEducativaId = Alumno.OfertaEducativaIdNueva;
                            AlumnoConfiguraciondb.PeriodoId = Alumno.PeriodoId;
                            AlumnoConfiguraciondb.UsuarioId = Alumno.UsuarioId;
                            AlumnoConfiguraciondb.CuotaColegiatura = Alumno.MontoColegiatura;
                            AlumnoConfiguraciondb.CuotaInscripcion = Alumno.MontoInscripcion;
                        }
                        else
                        {
                            AlumnoDb.GrupoAlumnoConfiguracion.Remove(AlumnoConfiguraciondb);
                            AlumnoInscritodb.EsEmpresa = false;
                        }

                        db.SaveChanges();
                    }
                    else if(Alumno.EsEmpresa)
                    {
                        db.GrupoAlumnoConfiguracion.Add(new GrupoAlumnoConfiguracion
                        {
                            AlumnoId = Alumno.AlumnoId,
                            Anio = Alumno.Anio,
                            PeriodoId = Alumno.PeriodoId,
                            CuotaColegiatura = 0,
                            CuotaInscripcion = 0,
                            EsCuotaCongelada = false,
                            EsEspecial = false,
                            EsInscripcionCongelada = false,
                            EstatusId = 8,
                            FechaRegistro = DateTime.Now,
                            GrupoId = null,
                            HoraRegistro = DateTime.Now.TimeOfDay,
                            NumeroPagos = null,
                            OfertaEducativaId = Alumno.OfertaEducativaIdNueva,
                            PagoPlanId = null,
                            UsuarioId = Alumno.UsuarioId,
                        });

                        AlumnoInscritodb.PagoPlanId = null;
                        AlumnoInscritodb.EsEmpresa = true;
                    }

                    LstConceptos.ForEach(ConceptoId =>
                    {
                        #region AlumnoDescuento
                        var Descuento = AlumnoDescuentodb.Find(a => a.PagoConceptoId == ConceptoId);
                        if (Descuento != null)
                        {
                            db.AlumnoDescuentoBitacora.Add(new AlumnoDescuentoBitacora
                            {
                                AlumnoDescuentoId = Descuento.AlumnoDescuentoId,
                                AlumnoId = Descuento.AlumnoId,
                                Anio = Descuento.Anio,
                                Comentario = Descuento.Comentario,
                                DescuentoId = Descuento.DescuentoId,
                                EsComite = Descuento.EsComite,
                                EsDeportiva = Descuento.EsDeportiva,
                                EsSEP = Descuento.EsSEP,
                                EstatusId = Descuento.EstatusId,
                                FechaAplicacion = Descuento.FechaAplicacion,
                                FechaGeneracion = Descuento.FechaGeneracion,
                                HoraGeneracion = Descuento.HoraGeneracion,
                                Monto = Descuento.Monto,
                                OfertaEducativaId = Descuento.OfertaEducativaId,
                                PagoConceptoId = Descuento.PagoConceptoId,
                                PeriodoId = Descuento.PeriodoId,
                                UsuarioId = Descuento.UsuarioId
                            });

                            Descuento.Anio = Alumno.Anio;
                            Descuento.PeriodoId = Alumno.PeriodoId;
                            Descuento.OfertaEducativaId = Alumno.OfertaEducativaIdNueva;
                            Descuento.UsuarioId = Alumno.UsuarioId;

                            Descuento.DescuentoId = lstDescuentos.Find(x => x.PagoConceptoId == ConceptoId).DescuentoId;

                            switch (Descuento.PagoConceptoId)
                            {
                                case 1:
                                    Descuento.Monto = DescuentoExamen;
                                    if (Alumno.EsEmpresa) { Descuento.EstatusId = 3; }
                                    break;
                                case 800:
                                    Descuento.Monto = DescuentoColegiatura;
                                    break;
                                case 802:
                                    Descuento.Monto = DescuentoInscripcion;
                                    break;
                                case 1000:
                                    Descuento.Monto = DescuentoCredencial;
                                    break;
                            }
                        }
                        else
                        {
                            if (!Alumno.EsEmpresa || ConceptoId != 1)
                            {
                                db.AlumnoDescuento.Add(new AlumnoDescuento
                                {
                                    AlumnoId = Alumno.AlumnoId,
                                    Anio = Alumno.Anio,
                                    DescuentoId = lstDescuentos.Find(x => x.PagoConceptoId == ConceptoId).DescuentoId,
                                    PagoConceptoId = ConceptoId,
                                    Comentario = "",
                                    EsComite = false,
                                    EsDeportiva = false,
                                    EsSEP = false,
                                    EstatusId = 2,
                                    FechaAplicacion = DateTime.Now,
                                    FechaGeneracion = DateTime.Now,
                                    HoraGeneracion = DateTime.Now.TimeOfDay,
                                    Monto = ConceptoId == 1 ? DescuentoExamen :
                                            ConceptoId == 800 ? DescuentoColegiatura :
                                            ConceptoId == 802 ? DescuentoInscripcion :
                                            DescuentoCredencial,
                                    OfertaEducativaId = Alumno.OfertaEducativaIdNueva,
                                    PeriodoId = Alumno.PeriodoId,
                                    UsuarioId = Alumno.UsuarioId
                                });
                            }
                        }
                        #endregion

                        #region Pagos
                        var pagos = PagosAlumno.FindAll(p => p.Cuota1.PagoConceptoId == ConceptoId);
                        if (pagos.Count>0)
                        {
                            pagos.ForEach(pago =>
                            {
                                Cuota CuotaBuena = lstCutoasNuevas
                                            .Where(c => c.PagoConceptoId == ConceptoId)
                                            .FirstOrDefault();

                                pago.Anio = Alumno.Anio;
                                pago.PeriodoId = Alumno.PeriodoId;
                                pago.OfertaEducativaId = Alumno.OfertaEducativaIdNueva;
                                pago.UsuarioId = Alumno.UsuarioId;

                                pago.CuotaId = CuotaBuena.CuotaId;

                                pago.Cuota = CuotaBuena.Monto;

                                switch (pago.Cuota1.PagoConceptoId)
                                {
                                    case 1:
                                        pago.Promesa = Alumno.MontoExamen;
                                        break;
                                    case 800:
                                        pago.Promesa = Alumno.MontoColegiatura;
                                        break;
                                    case 802:
                                        pago.Promesa = Alumno.MontoInscripcion;
                                        break;
                                    case 1000:
                                        pago.Promesa = Alumno.MontoCredencial;
                                        break;
                                }

                                decimal MontoPagado = pago.Promesa - pago.PagoParcial.Sum(o => o.Pago);
                                pago.Restante = MontoPagado <= 0 ? 0 : MontoPagado;
                                pago.EstatusId = !Alumno.EsEmpresa ? pago.EstatusId != 2 ? (MontoPagado <= 0 ? 4 : 1) : 2 :
                                (pago.Cuota1.PagoConceptoId == 1 ? 2 : pago.EstatusId != 2 ? (MontoPagado <= 0 ? 4 : 1) : 2);

                                var lstDescuentosPago = pago.PagoDescuento
                                                                    .Select(a => new
                                                                    {
                                                                        a.DescuentoId,
                                                                        a.PagoId
                                                                    })
                                                                    .ToList();

                                pago.PagoDescuento.Clear();

                                lstDescuentosPago.ForEach(desc =>
                                {

                                    db.PagoDescuento.Add(new PagoDescuento
                                    {
                                        Monto = pago.Cuota - pago.Promesa,
                                        DescuentoId = desc.DescuentoId,
                                        PagoId = desc.PagoId
                                    });

                                });
                            });
                        }
                        else if(AlumnoInscritodb.EstatusId!=8)
                        {
                            if (!Alumno.EsEmpresa || ConceptoId != 1)
                            {
                                Cuota CuotaBuena = lstCutoasNuevas
                                           .Where(c => c.PagoConceptoId == ConceptoId)
                                           .FirstOrDefault();

                                if (ConceptoId == 800)
                                {
                                    for (int i = 1; i <= 4; i++)
                                    {
                                        db.Pago.Add(new Pago
                                        {
                                            AlumnoId = Alumno.AlumnoId,
                                            Anio = Alumno.Anio,
                                            CuotaId = CuotaBuena.CuotaId,
                                            Cuota = CuotaBuena.Monto,
                                            EsAnticipado = false,
                                            EsEmpresa = false,
                                            EstatusId = Alumno.MontoColegiatura == 0 ? 4 : 1,
                                            FechaGeneracion = DateTime.Now,
                                            HoraGeneracion = DateTime.Now.TimeOfDay,
                                            OfertaEducativaId = Alumno.OfertaEducativaIdNueva,
                                            PeriodoAnticipadoId = 0,
                                            PeriodoId = Alumno.PeriodoId,
                                            Promesa = Alumno.MontoColegiatura,
                                            ReferenciaId = "",
                                            Restante = Alumno.MontoColegiatura,
                                            SubperiodoId = i,
                                            UsuarioId = Alumno.UsuarioId,
                                            UsuarioTipoId = 1,
                                            PagoDescuento = new List<PagoDescuento>{
                                                new PagoDescuento
                                                {
                                                    Monto = CuotaBuena.Monto - Alumno.MontoColegiatura,
                                                    DescuentoId = lstDescuentos.Find(x => x.PagoConceptoId == ConceptoId).DescuentoId,
                                                }
                                            }
                                        });
                                    }
                                }
                                else
                                {
                                    db.Pago.Add(new Pago
                                    {
                                        AlumnoId = Alumno.AlumnoId,
                                        Anio = Alumno.Anio,
                                        CuotaId = CuotaBuena.CuotaId,
                                        Cuota = CuotaBuena.Monto,
                                        EsAnticipado = false,
                                        EsEmpresa = false,
                                        EstatusId = (ConceptoId == 1 ? Alumno.MontoExamen : ConceptoId == 802 ? Alumno.MontoInscripcion : Alumno.MontoCredencial) == 0 ? 4 : 1,
                                        FechaGeneracion = DateTime.Now,
                                        HoraGeneracion = DateTime.Now.TimeOfDay,
                                        OfertaEducativaId = Alumno.OfertaEducativaIdNueva,
                                        PeriodoAnticipadoId = 0,
                                        PeriodoId = Alumno.PeriodoId,
                                        Promesa = ConceptoId == 1 ? Alumno.MontoExamen : ConceptoId == 802 ? Alumno.MontoInscripcion : Alumno.MontoCredencial,
                                        ReferenciaId = "",
                                        Restante = ConceptoId == 1 ? Alumno.MontoExamen : ConceptoId == 802 ? Alumno.MontoInscripcion : Alumno.MontoCredencial,
                                        SubperiodoId = 1,
                                        UsuarioId = Alumno.UsuarioId,
                                        UsuarioTipoId = 1,
                                        PagoDescuento = new List<PagoDescuento>{
                                                new PagoDescuento
                                                {
                                                    Monto = CuotaBuena.Monto - (ConceptoId == 1 ? Alumno.MontoExamen : ConceptoId == 802 ? Alumno.MontoInscripcion : Alumno.MontoCredencial),
                                                    DescuentoId = lstDescuentos.Find(x => x.PagoConceptoId == ConceptoId).DescuentoId,
                                                }
                                            }
                                    });
                                }
                            }
                        }
                        #endregion
                    });
                    

                    db.SaveChanges();

                    db.Pago.Local.ToList().ForEach(pag =>
                    {
                        pag.ReferenciaId = db.spGeneraReferencia(pag.PagoId).FirstOrDefault();
                    });

                    db.SaveChanges();

                    return new
                    {
                        Status = true,
                        Message = "Se guardo Correctamete"
                    };
                }
                catch(Exception Error)
                {
                    return new
                    {
                        Status = false,
                        Error.Message,
                        Inner = (Error?.InnerException?.InnerException?.Message) ?? ""
                    };
                }
            }
        }

        public static object GetRoot()
        {
            IntPtr token = IntPtr.Zero;
            LogonUser("JG_Rodriguez", "172.16.1.204", "Am2015-16",
                9, 0, ref token);
            using (WindowsImpersonationContext person = new WindowsIdentity(token).Impersonate())
            {
                try
                {
                    string[] allImgs = Directory.GetFiles(@"\\172.16.1.204\wwwroot");

                    return allImgs;
                }
                catch (IOException e)
                {
                    return e;
                }
                finally
                {
                    person.Undo();
                    CloseHandle(token);
                }
            }
        }
    }
}
