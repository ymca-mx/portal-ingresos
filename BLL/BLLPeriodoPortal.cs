using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using Herramientas;
using System.Data.Entity;
using System.Globalization;

namespace BLL
{
    public class BLLPeriodoPortal
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");
        public static List<DTOPeriodo> ConsultarPeriodos()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                DateTime fhoy = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00); 
                List<DTOPeriodo> lstPeriodo = (from a in db.Periodo
                                               where a.FechaInicial >= fhoy 
                                               select a).Take(2).ToList().ConvertAll(new Converter<Periodo, DTOPeriodo>(Convertidor.ToDTOPeriodo));
                lstPeriodo.InsertRange(0, (from a in db.Periodo
                                           where a.FechaInicial <= fhoy && fhoy <= a.FechaFinal
                                           select a).ToList().ConvertAll(new Converter<Periodo, DTOPeriodo>(Convertidor.ToDTOPeriodo)));
                
                return lstPeriodo;
            }
        }
        public static DTOPeriodo ConsultarPeriodo(string Descripcion)
        {
            int PeriodoId = int.Parse(Descripcion.Substring(0, 1));
            string Descri = Descripcion.Substring(1);
            using (UniversidadEntities db = new UniversidadEntities())
            {


                return (from a in db.Periodo
                        where a.PeriodoId == PeriodoId && a.Descripcion == Descri
                        select new DTOPeriodo
                        {
                            PeriodoId = a.PeriodoId,
                            Anio = a.Anio,
                            Descripcion = a.Descripcion,
                            FechaInicial = a.FechaInicial,
                            FechaFinal = a.FechaFinal,
                            Meses = a.Meses
                        }).FirstOrDefault();
            }
        }
        public static DTOPeriodo ConsultarPeriodo2(string Descripcion)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {


                return (from a in db.Periodo
                        where a.Descripcion == Descripcion
                        select new DTOPeriodo
                        {
                            PeriodoId = a.PeriodoId,
                            Anio = a.Anio,
                            Descripcion = a.Descripcion,
                            FechaInicial = a.FechaInicial,
                            FechaFinal = a.FechaFinal,
                            Meses = a.Meses
                        }).FirstOrDefault();
            }
        }
        public static DTOPeriodo ConsultarPeriodo(int Anio, int PeriodoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.Periodo
                        where a.Anio == Anio && a.PeriodoId == PeriodoId
                        select new DTOPeriodo
                        {
                            Anio = a.Anio,
                            Descripcion = a.Descripcion,
                            FechaFinal = a.FechaFinal,
                            FechaInicial = a.FechaInicial,
                            Meses = a.Meses,
                            PeriodoId = a.PeriodoId
                        }).FirstOrDefault();
            }
        }
        public static DTOPeriodo TraerPeriodoCompleto(int Anio, int PeriodoId, int ofertaEducativa)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                int PagoConcepto = BLLOfertaEducativaTipo.ConsultarOferta(ofertaEducativa).OfertaEducativaTipoId == 4 ? 807 : 800;
                DTOCuota objTem;
                DTOLenguasRelacion objRel = null, objRel2 = null;
                DTOPeriodo objPeriodo = (from a in db.Periodo
                                         where a.Anio == Anio && a.PeriodoId == PeriodoId
                                         select new DTOPeriodo
                                         {
                                             Anio = a.Anio,
                                             Descripcion = a.Descripcion,
                                             FechaFinal = a.FechaFinal,
                                             FechaInicial = a.FechaInicial,
                                             Meses = a.Meses,
                                             PeriodoId = a.PeriodoId,
                                             lstSubPeriodo = (from b in db.Subperiodo
                                                              where b.PeriodoId == PeriodoId
                                                              select new DTOSubPeriodo
                                                              {
                                                                  SubperiodoId = b.SubperiodoId,
                                                                  PeriodoId = b.PeriodoId,
                                                                  MesId = b.MesId,
                                                                  Mes = (from c in db.Mes
                                                                         where c.MesId == b.MesId
                                                                         select new DTOMes
                                                                         {
                                                                             Descripcion = c.Descripcion,
                                                                             MesId = c.MesId,
                                                                         }).FirstOrDefault()
                                                              }).ToList()
                                         }).FirstOrDefault();
                DateTime fActual = DateTime.Now < objPeriodo.FechaInicial ? objPeriodo.FechaInicial : DateTime.Now;// new DateTime(2016, 9, 20);
                List<DTOCuota> lstCuotas = (from cu in db.Cuota
                                            join le in db.LenguasRelacion on new { cu.CuotaId } equals new { le.CuotaId }
                                            into rel
                                            from X in rel.DefaultIfEmpty()
                                            where cu.PagoConceptoId == PagoConcepto && cu.OfertaEducativaId == ofertaEducativa 
                                                    && cu.PeriodoId == PeriodoId && cu.Anio==Anio
                                            orderby cu.CuotaId descending
                                            select new DTOCuota
                                            {
                                                Anio = cu.Anio,
                                                CuotaId = cu.CuotaId,
                                                Monto = cu.Monto,
                                                OfertaEducativaId = cu.OfertaEducativaId,
                                                PagoConceptoId = cu.PagoConceptoId,
                                                PeriodoId = PeriodoId,
                                                CuotaLentuaId=(from l in db.LenguasRelacion
                                                           where l.CuotaId==cu.CuotaId
                                                           select l.CuotaId).FirstOrDefault()
                                            }).ToList();
                List<DTOLenguasRelacion> lstLenguas= new List<DTOLenguasRelacion>();
                lstCuotas.ForEach(delegate(DTOCuota objlsc){
                    lstLenguas.Add((from a in db.LenguasRelacion
                                    where a.CuotaId == objlsc.CuotaId
                                    select new DTOLenguasRelacion
                                    {
                                        CuotaId = a.CuotaId,
                                        Descripcion = a.Descripcion,
                                        DiaFinal = a.DiaFinal,
                                        DiaInicial = a.DiaInicial
                                    }).FirstOrDefault());
                });
                
                objTem = lstCuotas[lstCuotas.FindIndex(X => X.CuotaLentuaId == 0)];
                lstLenguas.RemoveAll(d => d == null);
                foreach (DTOSubPeriodo objSub in objPeriodo.lstSubPeriodo)
                {
                    if (objRel != null)
                    {
                        objSub.Mes.MontoLengua = objRel;
                        objRel = null;
                        continue;
                    }
                    
                    if (objSub.MesId == fActual.Month)
                    {
                        if (fActual.Day < 7)
                        {
                            objSub.Mes.MontoLengua = objSub.Mes.MontoLengua = new DTOLenguasRelacion
                            {
                                CuotaId = objTem.CuotaId,
                                Cuota = objTem
                            };
                        }
                        else if (fActual.Day >= 7 && fActual.Day <= 14)
                        {
                            objRel2=lstLenguas[lstLenguas.FindIndex(X=>X.DiaFinal==14)];
                            //objSub.Mes.MontoLengua = lstCuotas[lstCuotas.FindIndex(X => X.Lenguas.DiaFinal == 14)].Lenguas;
                            objSub.Mes.MontoLengua = objRel2;
                            objSub.Mes.MontoLengua.Cuota = lstCuotas[lstCuotas.FindIndex(X => X.CuotaId == objRel2.CuotaId)];
                        }
                        else if (fActual.Day > 14 && fActual.Day <= 21)
                        {
                            objRel2 = lstLenguas.Where(X => X.DiaFinal == 21).FirstOrDefault();

                            //objSub.Mes.MontoLengua = lstCuotas[lstCuotas.FindIndex(X => X.Lenguas.DiaFinal == 21)].Lenguas;
                            objSub.Mes.MontoLengua = objRel2;
                            objSub.Mes.MontoLengua.Cuota = lstCuotas[lstCuotas.FindIndex(X => X.CuotaId == objRel2.CuotaId)];
                        }
                        else if (fActual.Day > 21 && fActual.Day <= 28)
                        {
                            objRel2 = lstLenguas[lstLenguas.FindIndex(X => X.DiaFinal == 28)];
                            //objSub.Mes.MontoLengua = lstCuotas[lstCuotas.FindIndex(X => X.Lenguas.DiaFinal == 28)].Lenguas;
                            objSub.Mes.MontoLengua = objRel2;
                            objSub.Mes.MontoLengua.Cuota = lstCuotas[lstCuotas.FindIndex(X => X.CuotaId == objRel2.CuotaId)];
                        }
                        else if (fActual.Day > 28)
                        {
                            
                            objRel = new DTOLenguasRelacion
                            {
                                CuotaId = objTem.CuotaId,
                                Cuota = objTem
                            };
                            
                        }
                    }
                    else if(objSub.MesId>fActual.Month)
                    {
                        objSub.Mes.MontoLengua = new DTOLenguasRelacion
                        {
                            CuotaId = objTem.CuotaId,
                            Cuota = objTem
                        };
                    }
                }
                return objPeriodo;
            }
        }
        public static DTOPeriodo TraerPeriodoEntreFechas(DateTime FechaActual)
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                DateTime fh = new DateTime(FechaActual.Year, FechaActual.Month, FechaActual.Day, 00, 00, 00);

                DTOPeriodo objPeriodo= (from b in db.Periodo
                        where b.FechaInicial <= fh && b.FechaFinal >= fh
                        select new DTOPeriodo
                        {
                            Anio=b.Anio,
                            Descripcion=b.Descripcion,
                            FechaFinal=b.FechaFinal,
                            FechaInicial=b.FechaInicial,
                            Meses=b.Meses,
                            PeriodoId=b.PeriodoId,
                        }).AsNoTracking().FirstOrDefault();

                objPeriodo.SubPeriodoId = TraerSubPeriodoEntreFechas(FechaActual);
                return objPeriodo;
            }
        }
        public static int TraerSubPeriodoEntreFechas(DateTime FechaActual)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return db.Subperiodo.Where(S => S.MesId == FechaActual.Month).FirstOrDefault().SubperiodoId;
            }
        }

        public static DateTime TraerPeriodoCompletoS(int Anio, int PeriodoId, int SubPeriodoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
               DTOPeriodo objPErio= BLLPeriodoPortal.ConsultarPeriodo(Anio, PeriodoId);
               objPErio.SubPeriodoId = SubPeriodoId;//Sacar Anio del Periodo para evitar confunciones
               string sFecha = "01/" + (db.Subperiodo.Where(S => S.SubperiodoId == SubPeriodoId && S.PeriodoId == PeriodoId).FirstOrDefault().MesId < 10 ?
                   "0" + db.Subperiodo.Where(S => S.SubperiodoId == SubPeriodoId && S.PeriodoId == PeriodoId).FirstOrDefault().MesId.ToString() :
                   db.Subperiodo.Where(S => S.SubperiodoId == SubPeriodoId && S.PeriodoId == PeriodoId).FirstOrDefault().MesId.ToString()) + "/" + (PeriodoId == 1 ? (Anio - 1).ToString() : Anio.ToString());
               DateTime FechaF = DateTime.ParseExact(sFecha, "dd/MM/yyyy", Cultura);
               return FechaF;
            }
        }

        public static List<DTOPeriodo> ConsultarPeriodos(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    List<DTOPeriodo> lstPeriodos = (from a in db.Pago
                                                    where a.AlumnoId == AlumnoId && a.EstatusId != 2 && a.EstatusId != 3
                                                     && (a.Cuota1.PagoConceptoId != 1007 && a.Cuota1.PagoConceptoId != 1001)
                                                    //&& (a.PeriodoId != 1 || a.Anio != 2016)
                                                    select new DTOPeriodo
                                                    {
                                                        Anio = a.Periodo.Anio,
                                                        PeriodoId = a.Periodo.PeriodoId,
                                                        Descripcion = a.Periodo.Descripcion,
                                                        FechaFinal = a.Periodo.FechaFinal,
                                                        FechaInicial = a.Periodo.FechaInicial
                                                    }).Distinct().ToList();
                    lstPeriodos.ForEach(delegate(DTOPeriodo objPer)
                    {
                        objPer._FechaFinal = objPer.FechaFinal.ToString("dd/MM/yyyy", Cultura);
                        objPer._FechaInicial = objPer.FechaInicial.ToString("dd/MM/yyyy", Cultura);
                    });
                    return lstPeriodos;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static List<DTOPeriodo> ConsutlarPeriodosAlumno(int AlumnoId, int OfertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    var Group = db.Pago.Where(a => (a.Cuota1.PagoConceptoId == 800
                                                            || a.Cuota1.PagoConceptoId == 802)
                                                            && a.EstatusId != 2
                                                            && a.AlumnoId == AlumnoId
                                                            && a.OfertaEducativaId == OfertaEducativaId)
                                                            .GroupBy(s => new { s.Anio, s.PeriodoId })
                                                            .AsQueryable();

                    List<DTOPeriodo> lstPeriodo = new List<DTOPeriodo>();
                    foreach (var op in Group)
                    {
                        lstPeriodo.Add(new DTOPeriodo
                        {
                            Anio = op.Key.Anio,
                            PeriodoId = op.Key.PeriodoId,
                            Descripcion = db.Periodo.Where(o => o.Anio == op.Key.Anio && o.PeriodoId == op.Key.PeriodoId).FirstOrDefault().Descripcion
                        });
                    }
                    return lstPeriodo;

                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
