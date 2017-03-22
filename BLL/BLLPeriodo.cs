using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Universidad.DTO;

namespace Universidad.BLL
{
    public class BLLPeriodo
    {
        /// <summary>
        /// Saber Anticipado calcula la fecha de pagos anticipados 
        /// </summary>
        /// <param name="AlumnoId">Numero del Alumno para buscar adeudos</param>
        /// <returns>Retornamos dos valores el primero es Aplica y el segundo es la fecha maxima</returns>
        public static string[] SaberAnticipado(int AlumnoId)
        {
            using(UniversidadEntities db = new UniversidadEntities())
            {
                DateTime Fhoy=DateTime.Now;
                Periodo objPeriodo = db.Periodo.Where(P => Fhoy >= P.FechaInicial && Fhoy <= P.FechaFinal).FirstOrDefault();
                Subperiodo objSubperiodo = db.Subperiodo.Where(S => S.PeriodoId == objPeriodo.PeriodoId && S.MesId == Fhoy.Month).FirstOrDefault();

                objSubperiodo.SubperiodoId = objPeriodo.PeriodoId == 1 ? 4 : Fhoy.Day < 15 ? 0 : 4;
                
                if(objSubperiodo.SubperiodoId==4)
                {
                    Periodo objPeriodoS = db.Periodo.Where(P => P.Anio == (objPeriodo.PeriodoId == 3 ? objPeriodo.Anio + 1 : objPeriodo.Anio)
                        && P.PeriodoId == (objPeriodo.PeriodoId == 3 ? 1 : objPeriodo.PeriodoId + 1)).FirstOrDefault();
                    List<Pago> lstPAgos=db.Pago.Where(P=>P.AlumnoId==AlumnoId && P.Anio==objPeriodoS.Anio && P.PeriodoId==objPeriodoS.PeriodoId && 
                        (P.Cuota1.PagoConceptoId==800 || P.Cuota1.PagoConceptoId==802 || P.Cuota1.PagoConceptoId==807) && P.SubperiodoId==1).ToList();

                    if (lstPAgos.Count > 0)
                    {
                        if (lstPAgos.Where(P => P.EstatusId == 1).ToList().Count > 0)
                        {
                            string fec = (objSubperiodo.MesId == 4 ? "30 - " : "31 - ") + objSubperiodo.Mes.Descripcion + " - " + objPeriodo.Anio;
                            return new string[] { "Aplica", fec };
                        }
                        else if (lstPAgos.Where(P => P.EstatusId == 4).ToList().Count > 0)
                        {
                            return new string[] { "No" };
                        }
                    }
                    else     
                    {
                        string fec = (objSubperiodo.MesId == 4 ? "30 - " : "31 - ") + objSubperiodo.Mes.Descripcion + " - " + objPeriodo.Anio;
                        return new string[] { "Aplica", fec }; 
                    }
                }
                return new string[] { "No" };
            }
            
        }
    }
}
