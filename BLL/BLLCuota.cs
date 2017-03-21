using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Globalization;

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
        public static int CrearCuota(DTOCuota objCuota)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.Cuota.Add(new Cuota
                {
                    Anio = objCuota.Anio,
                    PeriodoId = objCuota.PeriodoId,
                    OfertaEducativaId = objCuota.OfertaEducativaId,
                    PagoConceptoId = objCuota.PagoConceptoId,
                    Monto = objCuota.Monto
                });
                return db.Cuota.Local[0].CuotaId;
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
        public static List<object> CuotaCredencial(int AlumnoId, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DTOPeriodo objPeriodo = BLLPeriodoPortal.TraerPeriodoEntreFechas(DateTime.Now);
                DTOAlumno objAlumno = (from a in db.Alumno
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
                                     ).FirstOrDefault();
                DTOCuota objCuotaCred = (from a in db.Cuota
                                         where a.OfertaEducativaId == OfertaEducativaId && a.Anio == objAlumno.AlumnoInscrito.Anio && a.PeriodoId==objAlumno.AlumnoInscrito.PeriodoId && a.PagoConceptoId == 1000
                                         select new DTOCuota
                                         {
                                             CuotaId = a.CuotaId,
                                             Anio = a.Anio,
                                             PeriodoId = a.PeriodoId,
                                             OfertaEducativaId = a.OfertaEducativaId,
                                             PagoConceptoId = a.PagoConceptoId,
                                             Monto = a.Monto,
                                             EsEmpresa = a.EsEmpresa
                                         }).FirstOrDefault();
                CString_Alumno objAlumnoA = CString_Alumno.Convert(objAlumno);
                CString_OfertaEducativa objOferta = CString_OfertaEducativa.Convert(objAlumno.AlumnoInscrito.OfertaEducativa);
                List<CString_Cuota> lstCuotas = new List<CString_Cuota>();

                lstCuotas.Add(CString_Cuota.Convert(objCuotaCred));
                lstCuotas[0].MontoReposicion = (from a in db.Cuota
                                                where a.OfertaEducativaId == OfertaEducativaId && a.Anio == objAlumno.AlumnoInscrito.Anio && a.PagoConceptoId == 5
                                                select a.Monto).FirstOrDefault().ToString("C");
                List<object> tdLista = new List<object>() { 
                   new List<CString_Alumno>{ objAlumnoA},
                   new List<CString_OfertaEducativa>{ objOferta},
                    lstCuotas
                };


                return tdLista;
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
                    DateTime fHoy = DateTime.Now;
                    Periodo objPerActual = db.Periodo.Where(P => fHoy >= P.FechaInicial && fHoy <= P.FechaFinal).FirstOrDefault();
                    int SubId = db.Subperiodo.Where(S => S.PeriodoId == objPerActual.PeriodoId && S.MesId == fHoy.Month).FirstOrDefault().SubperiodoId;

                    if (SubId != 4) { return null; }
                    int PeriodoS = objPerActual.PeriodoId == 3 ? 1 : objPerActual.PeriodoId + 1, AnioS = objPerActual.PeriodoId == 3 ? objPerActual.Anio + 1 : objPerActual.Anio;

                    List<Pago> lstCuotas = db.Pago.Where(P => P.AlumnoId == AlumnoId && P.Anio == AnioS && P.PeriodoId == PeriodoS
                        && P.EstatusId == 1 && (P.Cuota1.PagoConceptoId == 800 || P.Cuota1.PagoConceptoId == 802)).ToList();
                    List<OfertaEducativa> lstOfertas=new List<OfertaEducativa>();
                    List<Oferta_Costo> lstofcos = new List<Oferta_Costo>();
                    lstCuotas.ForEach(delegate(Pago objPago)
                    {
                        if (lstOfertas.Count < 1)
                        {
                            lstOfertas.Add(db.OfertaEducativa.Where(OF => OF.OfertaEducativaId == objPago.OfertaEducativaId).FirstOrDefault());
                        }
                        else
                        {
                            if (lstOfertas.Where(O => O.OfertaEducativaId == objPago.OfertaEducativaId).ToList().Count < 1)
                            {
                                lstOfertas.Add(db.OfertaEducativa.Where(OF => OF.OfertaEducativaId == objPago.OfertaEducativaId).FirstOrDefault());
                            }
                        }
                    });

                    lstOfertas.ForEach(delegate(OfertaEducativa objOferta)
                    {
                        decimal DescIn = db.PeriodoAnticipado.Where(l => l.Anio == Anio
                                                                       && l.PeriodoId == PeriodoId
                                                                       && l.OfertaEducativaTipoId == objOferta.OfertaEducativaTipoId
                                                                       && l.PagoConceptoId == 802).FirstOrDefault().ImporteDescuento;
                        decimal DescCol = db.PeriodoAnticipado.Where(l => l.Anio == Anio
                                                                       && l.PeriodoId == PeriodoId
                                                                       && l.OfertaEducativaTipoId == objOferta.OfertaEducativaTipoId
                                                                       && l.PagoConceptoId == 800).FirstOrDefault().ImporteDescuento; 

                        Pago objPagoCol = lstCuotas.Where(C => C.OfertaEducativaId == objOferta.OfertaEducativaId && C.SubperiodoId == 1 && C.Cuota1.PagoConceptoId == 800).FirstOrDefault();
                        Pago objPagoIns = lstCuotas.Where(C => C.OfertaEducativaId == objOferta.OfertaEducativaId && C.SubperiodoId == 1 && C.Cuota1.PagoConceptoId == 802).FirstOrDefault();
                        lstofcos.Add(new Oferta_Costo
                        {
                            OfertaEducativa = objOferta.Descripcion,
                            ReferenciaInsc = objPagoIns != null ? int.Parse(objPagoIns.ReferenciaId).ToString() : null,
                            MontoReins = objPagoIns != null ? objPagoIns.Cuota.ToString("C", Cultura) + " - " + DescIn.ToString("C", Cultura) + " = " + (objPagoIns.Cuota - DescIn).ToString("C", Cultura) : null,
                            ReferenciaColg = objPagoCol != null ? int.Parse(objPagoCol.ReferenciaId).ToString() : null,
                            MontoColeg = objPagoCol != null ? objPagoCol.Cuota.ToString("C", Cultura) + " - " + DescCol.ToString("C", Cultura) + " = " + (objPagoCol.Cuota - DescCol).ToString("C", Cultura) : null,
                        });
                    });
                    
                    //lstOfertas.ForEach(delegate(OfertaEducativa oftobj)
                    //{
                        
                    //});
                    return lstofcos.Count > 0 ? lstofcos : null;

                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
