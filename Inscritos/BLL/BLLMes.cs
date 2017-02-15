using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace BLL
{
    public class BLLMes
    {
        public static List<DTOPeriodo> CalcularMesesDeFechas(DateTime FechaI, DateTime FechaF)
        {
            int cPeriodo = 0;
            List<DTOPeriodo> listaFechas = new List<DTOPeriodo>();
            DateTime FechaN = DateTime.Now;
            DateTime FechaV = new DateTime(FechaI.Year, FechaI.Month + 1, FechaI.Day);
            FechaV = FechaV.AddDays(-1);

            for (int i = FechaI.Month; i <= FechaF.Month; i++)
            {
                cPeriodo += 1;   
                if (i>=FechaN.Month )
                {
                    listaFechas.Add(
                        new DTOPeriodo{
                            FechaInicial= new DateTime(FechaI.Year, i, FechaI.Day), 
                            FechaFinal= FechaV,
                            SubPeriodoId=cPeriodo
                        }
                        );
                    FechaV = new DateTime(FechaV.Year, FechaV.Month, 1);
                    FechaV = FechaV.AddMonths(2);
                    FechaV = FechaV.AddDays(-1);
                }
                else
                {
                    FechaV = new DateTime(FechaV.Year, FechaV.Month, 1);
                    FechaV = FechaV.AddMonths(2);
                    FechaV = FechaV.AddDays(-1);
                }
            }
            return listaFechas;
        }
    }
}
