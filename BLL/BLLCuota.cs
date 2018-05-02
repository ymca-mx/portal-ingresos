using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Globalization;
using System.Data.Entity;

namespace BLL
{
    public class CString_Alumno
    {
        public string AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public static CString_Alumno Convert(DTOAlumno objAlumno)
        {
            return new CString_Alumno
            {
                AlumnoId = objAlumno.AlumnoId.ToString(),
                Materno = objAlumno.Materno,
                Paterno = objAlumno.Paterno,
                Nombre = objAlumno.Nombre
            };
        }
    }
    public class CString_OfertaEducativa
    {
        public string OfertaEducativaId { get; set; }
        public string OfertaEducativaTipoId { get; set; }
        public string Descripcion { get; set; }
        public static CString_OfertaEducativa Convert(DTOOfertaEducativa b)
        {
            return new CString_OfertaEducativa
            {
                Descripcion = b.Descripcion,
                OfertaEducativaId = b.OfertaEducativaId.ToString(),
                OfertaEducativaTipoId = b.OfertaEducativaTipoId.ToString()
            };
        }
    }
    public class Oferta_Costo
    {
        public string OfertaEducativa { get; set; }
        public string ReferenciaInsc { get; set; }
        public string ReferenciaColg { get; set; }
        public string MontoReins{ get; set; }
        public string MontoColeg { get; set; }
    }
    public class CString_Cuota
    {
        public string CuotaId { get; set; }
        public string Anio { get; set; }
        public string PeriodoId { get; set; }
        public string OfertaEducativaId { get; set; }
        public string PagoConceptoId { get; set; }
        public string Monto { get; set; }
        public string MontoReposicion { get; set; }
        public string EsEmpresa { get; set; }
        public static CString_Cuota Convert(DTOCuota objCuotaCred)
        {
            return new CString_Cuota
            {
                Anio = objCuotaCred.Anio.ToString(),
                CuotaId = objCuotaCred.CuotaId.ToString(),
                EsEmpresa = objCuotaCred.EsEmpresa.ToString(),
                Monto = objCuotaCred.Monto.ToString("C"),
                OfertaEducativaId = objCuotaCred.OfertaEducativaId.ToString(),
                PagoConceptoId = objCuotaCred.PagoConceptoId.ToString(),
                PeriodoId = objCuotaCred.PagoConceptoId.ToString()
            };
        }
    }
    public class BLLCuota
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");
        public static DTOCuota traerCuotaParametros(DTOAlumnoInscrito objOferta,DTODescuentos objDescuento)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from d in db.Cuota
                        where d.OfertaEducativaId == objOferta.OfertaEducativaId && d.Anio == objOferta.Anio &&
                        d.PeriodoId == objOferta.PeriodoId && d.PagoConceptoId == objDescuento.PagoConceptoId
                        select new DTOCuota
                        {
                            CuotaId = d.CuotaId,
                            Anio = d.Anio,
                            PeriodoId = d.PeriodoId,
                            OfertaEducativaId = d.OfertaEducativaId,
                            PagoConceptoId = d.PagoConceptoId,
                            Monto = d.Monto
                        }).FirstOrDefault();
            }
        }
        public static DTOCuota TraerCuotaPagoConcepto(int PagoConceptoId, DTOAlumnoInscrito objOferta)
        {
            using(UniversidadEntities db=new UniversidadEntities())
            {
                return (from d in db.Cuota
                        where d.OfertaEducativaId == objOferta.OfertaEducativaId && d.Anio == objOferta.Anio &&
                        d.PeriodoId == objOferta.PeriodoId && d.PagoConceptoId == PagoConceptoId
                        select new DTOCuota
                        {
                            CuotaId = d.CuotaId,
                            Anio = d.Anio,
                            PeriodoId = d.PeriodoId,
                            OfertaEducativaId = d.OfertaEducativaId,
                            PagoConceptoId = d.PagoConceptoId,
                            Monto = d.Monto
                        }).FirstOrDefault();
            }
        }
        public static DTOCuota TraerCuota(int CuotaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.Cuota
                        where a.CuotaId == CuotaId
                        select new DTOCuota
                        {
                            Anio = a.Anio,
                            CuotaId = a.CuotaId,
                            EsEmpresa = a.EsEmpresa,
                            Monto = a.Monto,
                            OfertaEducativaId = a.OfertaEducativaId,
                            PagoConceptoId = a.PagoConceptoId,
                            PeriodoId = a.PeriodoId
                        }).FirstOrDefault();
            }
        }
        public static DTOCuota CuotaUnPago(int OfertaEducativaId, int PagoPlanId, string Periodo)
        {
            DTOPeriodo objP = BLLPeriodoPortal.ConsultarPeriodo(Periodo);
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.Cuota
                        join d in db.OfertaEducativa on a.OfertaEducativaId equals d.OfertaEducativaId
                        join c in db.OfertaEducativaTipo on d.OfertaEducativaTipoId equals c.OfertaEducativaTipoId
                        join b in db.OfertaEducativaPlan on c.OfertaEducativaTipoId equals b.OfertaEducativaTipoId
                        where d.OfertaEducativaId==OfertaEducativaId && b.PagoPlanId==PagoPlanId && a.PeriodoId==objP.PeriodoId && a.PagoConceptoId==800
                        select new DTOCuota
                        {
                            Anio = a.Anio,
                            CuotaId = a.CuotaId,
                            EsEmpresa = a.EsEmpresa,
                            Monto = a.Monto,
                            OfertaEducativaId = a.OfertaEducativaId,
                            PagoConceptoId = a.PagoConceptoId,
                            PeriodoId = a.PeriodoId
                        }).FirstOrDefault();
            }
        }
        public static async Task<Tuple<List<CString_Alumno>,List<CString_OfertaEducativa>, List<CString_Cuota>>> CuotaCredencial(int AlumnoId, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTOPeriodo objPeriodo = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);
                DTOAlumno objAlumno =await (from a in db.Alumno
                                       join b in db.AlumnoInscrito on a.AlumnoId equals b.AlumnoId
                                       where a.AlumnoId == AlumnoId && b.OfertaEducativaId == OfertaEducativaId
                                       select new DTOAlumno
                                       {
                                           AlumnoId = a.AlumnoId,
                                           Nombre = a.Nombre,
                                           Paterno = a.Paterno,
                                           Materno = a.Materno,
                                           AlumnoInscrito = new DTOAlumnoInscrito
                                           {
                                               Anio = b.Anio,
                                               PeriodoId = b.PeriodoId,
                                               OfertaEducativa = new DTOOfertaEducativa
                                               {
                                                   OfertaEducativaId = b.OfertaEducativa.OfertaEducativaId,
                                                   OfertaEducativaTipoId = b.OfertaEducativa.OfertaEducativaTipoId,
                                                   Descripcion = b.OfertaEducativa.Descripcion,

                                               }
                                           }
                                       }
                                     ).FirstOrDefaultAsync();
                DTOCuota objCuotaCred =await (from a in db.Cuota
                                         where a.OfertaEducativaId == OfertaEducativaId 
                                            && a.Anio == objAlumno.AlumnoInscrito.Anio 
                                            && a.PeriodoId==objAlumno.AlumnoInscrito.PeriodoId 
                                            && a.PagoConceptoId == 1000
                                         select new DTOCuota
                                         {
                                             CuotaId = a.CuotaId,
                                             Anio = a.Anio,
                                             PeriodoId = a.PeriodoId,
                                             OfertaEducativaId = a.OfertaEducativaId,
                                             PagoConceptoId = a.PagoConceptoId,
                                             Monto = a.Monto,
                                             EsEmpresa = a.EsEmpresa
                                         }).FirstOrDefaultAsync();
                CString_Alumno objAlumnoA = CString_Alumno.Convert(objAlumno);
                CString_OfertaEducativa objOferta = CString_OfertaEducativa.Convert(objAlumno.AlumnoInscrito.OfertaEducativa);
                List<CString_Cuota> lstCuotas = new List<CString_Cuota>();

                lstCuotas.Add(CString_Cuota.Convert(objCuotaCred));
                lstCuotas[0].MontoReposicion = (await (from a in db.Cuota
                                                       where a.OfertaEducativaId == OfertaEducativaId
                                                       && a.Anio == objAlumno.AlumnoInscrito.Anio
                                                       && a.PagoConceptoId == 5
                                                       select a.Monto).FirstOrDefaultAsync()).ToString("C");

                List<CString_Alumno> listaAlumno = new List<CString_Alumno> { objAlumnoA };
                List<CString_OfertaEducativa> ListaOfertaNombre = new List<CString_OfertaEducativa> { objOferta };

                return Tuple.Create(listaAlumno, ListaOfertaNombre, lstCuotas);
               
            }
        }
        public static Cuota TraerPeriodoParcialIngles(int OfertaEducativaId, int Anio, int PeriodoId, int PagoConceptoId, DateTime fCorriendo)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<LenguasRelacion> lstLenguas = db.LenguasRelacion.Where(L => L.Cuota.Anio == Anio && L.Cuota.PeriodoId == PeriodoId &&
                        L.Cuota.PagoConceptoId == PagoConceptoId && L.Cuota.OfertaEducativaId == OfertaEducativaId).ToList();

                    Cuota objCuota;
                    if (fCorriendo.Day >= 7 && fCorriendo.Day <= 28)
                    { objCuota = lstLenguas.Where(P => fCorriendo.Day >= P.DiaInicial && fCorriendo.Day <= P.DiaFinal).FirstOrDefault().Cuota; }
                    else
                    {
                        objCuota = db.Cuota.Where(L => L.Anio == Anio && L.PeriodoId == PeriodoId &&
                        L.PagoConceptoId == PagoConceptoId && OfertaEducativaId == L.OfertaEducativaId).ToList().Where(P => P.LenguasRelacion == null).FirstOrDefault();
                    }
                    return objCuota;
                }
                catch 
                {
                    return null;
                }
            }
        }
        
        public static List<Oferta_Costo> TraerOfertasCuotasAlumno(int AlumnoId, int Anio, int PeriodoId)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                try
                {

                    DateTime FechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                    Periodo PeriodoActual = db.Periodo.Where(P => FechaActual >= P.FechaInicial && FechaActual <= P.FechaFinal).FirstOrDefault();
                    int SubPeriodoId = db.Subperiodo.Where(S => S.PeriodoId == PeriodoActual.PeriodoId && S.MesId == FechaActual.Month).FirstOrDefault().SubperiodoId;

                    if (Anio == 0 || PeriodoId == 0)
                    {
                        List<Pago> Pagos = db.Pago.Where(a => a.AlumnoId == AlumnoId
                                                          && a.OfertaEducativa.OfertaEducativaTipoId != 4
                                                          && a.SubperiodoId == 1
                                                          && a.EstatusId != 2
                                                          && (a.Cuota1.PagoConceptoId == 800 || a.Cuota1.PagoConceptoId == 802))
                                                   .OrderByDescending(p => p.Anio).ThenBy(p => p.PeriodoId)
                                                   .ToList();

                        Anio = Pagos.FirstOrDefault().Anio;
                        PeriodoId = Pagos.FirstOrDefault().PeriodoId;
                    }




                   DTOPeriodo PeriodoSiguiente = new DTOPeriodo
                    {
                        Anio = PeriodoActual.PeriodoId == 3 ? PeriodoActual.Anio + 1 : PeriodoActual.Anio,
                        PeriodoId = PeriodoActual.PeriodoId == 3 ? 1 : PeriodoActual.PeriodoId + 1,
                    };
                    if (db.Alumno
                                            .Where(a => ((a.Anio == PeriodoActual.Anio && a.PeriodoId == PeriodoActual.PeriodoId) ||
                                                        (a.Anio == PeriodoSiguiente.Anio && a.PeriodoId == PeriodoSiguiente.PeriodoId))
                                                       && a.AlumnoInscrito.Count == 1
                                                       && (a.AlumnoInscritoBitacora.Count == 0 || a.AlumnoInscritoBitacora
                                                                                                    .Where(k => k.PagoPlanId == null).ToList().Count == 1)
                                                       && a.AlumnoId == AlumnoId
                                                       && a.EstatusId != 3).ToList().Count > 0) { return null; }


                    if (SubPeriodoId != 4) { return null; }
                    int PeriodoTexto = PeriodoActual.PeriodoId == 3 ? 1 : PeriodoActual.PeriodoId + 1, AnioS = PeriodoActual.PeriodoId == 3 ? PeriodoActual.Anio + 1 : PeriodoActual.Anio;

                    List<Pago> Cuotas = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.Anio == AnioS && P.PeriodoId == PeriodoTexto
                        && P.EstatusId == 1 && (P.Cuota1.PagoConceptoId == 800 || P.Cuota1.PagoConceptoId == 802)).ToList();
                    List<OfertaEducativa> OfertasAlumno=new List<OfertaEducativa>();
                    List<Oferta_Costo> CostosOfertas = new List<Oferta_Costo>();
                    Cuotas.ForEach(Pago=> 
                    {
                        if (OfertasAlumno.Count < 1)
                        {
                            OfertasAlumno.Add(db.OfertaEducativa.Where(OF => OF.OfertaEducativaId == Pago.OfertaEducativaId).FirstOrDefault());
                        }
                        else
                        {
                            if (OfertasAlumno.Where(O => O.OfertaEducativaId == Pago.OfertaEducativaId).ToList().Count < 1)
                            {
                                OfertasAlumno.Add(db.OfertaEducativa.Where(OF => OF.OfertaEducativaId == Pago.OfertaEducativaId).FirstOrDefault());
                            }
                        }
                    });

                    OfertasAlumno.ForEach(OFerta=>
                    {
                        decimal DescuentoInscripcion = db.PeriodoAnticipado.Where(l => l.Anio == Anio
                                                                       && l.PeriodoId == PeriodoId
                                                                       && l.OfertaEducativaTipoId == OFerta.OfertaEducativaTipoId
                                                                       && l.PagoConceptoId == 802).FirstOrDefault().ImporteDescuento;
                        decimal DescuentoColegiatura = db.PeriodoAnticipado.Where(l => l.Anio == Anio
                                                                       && l.PeriodoId == PeriodoId
                                                                       && l.OfertaEducativaTipoId == OFerta.OfertaEducativaTipoId
                                                                       && l.PagoConceptoId == 800).FirstOrDefault().ImporteDescuento; 

                        Pago PagoCol = Cuotas.Where(C => C.OfertaEducativaId == OFerta.OfertaEducativaId && C.SubperiodoId == 1 && C.Cuota1.PagoConceptoId == 800).FirstOrDefault();
                        Pago PagoIns = Cuotas.Where(C => C.OfertaEducativaId == OFerta.OfertaEducativaId && C.SubperiodoId == 1 && C.Cuota1.PagoConceptoId == 802).FirstOrDefault();
                        CostosOfertas.Add(new Oferta_Costo
                        {
                            OfertaEducativa = OFerta.Descripcion,
                            ReferenciaInsc = PagoIns != null ? int.Parse(PagoIns.ReferenciaId).ToString() : null,
                            MontoReins = PagoIns != null ? PagoIns.Cuota.ToString("C", Cultura) + " - " + DescuentoInscripcion.ToString("C", Cultura) + " = " + (PagoIns.Cuota - DescuentoInscripcion).ToString("C", Cultura) : null,
                            ReferenciaColg = PagoCol != null ? int.Parse(PagoCol.ReferenciaId).ToString() : null,
                            MontoColeg = PagoCol != null ? PagoCol.Cuota.ToString("C", Cultura) + " - " + DescuentoColegiatura.ToString("C", Cultura) + " = " + (PagoCol.Cuota - DescuentoColegiatura).ToString("C", Cultura) : null,
                        });
                    });
                    
                    return CostosOfertas.Count > 0 ? CostosOfertas : null;

                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
