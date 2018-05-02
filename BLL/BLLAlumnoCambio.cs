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

                    decimal DescuentoColegiatura = 100 - ((lstCutoasNuevas.Find(a => a.PagoConceptoId == 800)?.Monto ?? 0) * 100) / Alumno.MontoColegiatura,
                        DescuentoInscripcion = 100 - ((lstCutoasNuevas.Find(a => a.PagoConceptoId == 802)?.Monto ?? 0) * 100) / Alumno.MontoInscripcion,
                        DescuentoCredencial = 100 - ((lstCutoasNuevas.Find(a => a.PagoConceptoId == 1000)?.Monto ?? 0) * 100) / Alumno.MontoCredencial,
                        DescuentoExamen = 100 - ((lstCutoasNuevas.Find(a => a.PagoConceptoId == 1)?.Monto ?? 0) * 100) / Alumno.MontoExamen;


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
                            FechaRegistro = AlumnoConfiguraciondb.FechaRegistro,
                            GrupoId = AlumnoConfiguraciondb.GrupoId,
                            HoraRegistro = AlumnoConfiguraciondb.HoraRegistro,
                            NumeroPagos = AlumnoConfiguraciondb.NumeroPagos,
                            PagoPlanId = AlumnoConfiguraciondb.PagoPlanId,
                            PeriodoId = AlumnoConfiguraciondb.PeriodoId
                        });

                        AlumnoConfiguraciondb.Anio = Alumno.Anio;
                        AlumnoConfiguraciondb.OfertaEducativaId = Alumno.OfertaEducativaIdNueva;
                        AlumnoConfiguraciondb.PeriodoId = Alumno.PeriodoId;
                        AlumnoConfiguraciondb.UsuarioId = Alumno.UsuarioId;
                        AlumnoConfiguraciondb.CuotaColegiatura = Alumno.MontoColegiatura;
                        AlumnoConfiguraciondb.CuotaInscripcion = Alumno.MontoInscripcion;

                        db.SaveChanges();
                    }

                    AlumnoDescuentodb.ForEach(AlumnoDes =>
                    {
                        AlumnoDes.Anio = Alumno.Anio;
                        AlumnoDes.PeriodoId = Alumno.PeriodoId;
                        AlumnoDes.OfertaEducativaId = Alumno.OfertaEducativaIdNueva;
                        AlumnoDes.UsuarioId = Alumno.UsuarioId;

                        Descuento Descuentodb = db.Descuento.Where(a => a.DescuentoId == AlumnoDes.DescuentoId).FirstOrDefault();

                        AlumnoDes.DescuentoId = db.Descuento
                                                .Where(a => a.OfertaEducativaId == Alumno.OfertaEducativaIdNueva
                                                && a.PagoConceptoId == AlumnoDes.PagoConceptoId
                                                && a.Descripcion == Descuentodb.Descripcion)
                                                .FirstOrDefault()
                                                .DescuentoId;

                        switch (AlumnoDes.PagoConceptoId)
                        {
                            case 1:
                                AlumnoDes.Monto = DescuentoExamen;
                                break;
                            case 800:
                                AlumnoDes.Monto = DescuentoColegiatura;
                                break;
                            case 802:
                                AlumnoDes.Monto = DescuentoInscripcion;
                                break;
                            case 1000:
                                AlumnoDes.Monto = DescuentoCredencial;
                                break;
                        }

                        db.SaveChanges();
                    });

                    PagosAlumno.ForEach(pago =>
                    {
                        Cuota CuotaBuena = lstCutoasNuevas
                                        .Where(c => c.PagoConceptoId == pago.Cuota1.PagoConceptoId)
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
                                pago.Promesa = DescuentoExamen;
                                break;
                            case 800:
                                pago.Promesa = DescuentoColegiatura;
                                break;
                            case 802:
                                pago.Promesa = DescuentoInscripcion;
                                break;
                            case 1000:
                                pago.Promesa = DescuentoCredencial;
                                break;
                        }

                        decimal MontoPagado = pago.Promesa - pago.PagoParcial.Sum(o => o.Pago);
                        pago.Restante = MontoPagado <= 0 ? 0 : MontoPagado;
                        pago.EstatusId = pago.EstatusId != 2 ? (MontoPagado <= 0 ? 4 : 1) : 2;

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

                    db.SaveChanges();

                    return new
                    {
                        EstatusId = true,
                        Message = "Se guardo Correctamete"
                    };
                }
                catch(Exception Error)
                {
                    return new
                    {
                        StatusId = false,
                        Error.Message
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
