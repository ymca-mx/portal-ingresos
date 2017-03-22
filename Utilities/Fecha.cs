using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Fecha
    {
        /// <summary>
        /// Retorna la fecha en el formato ddMMyyyy
        /// </summary>
        /// <param name="Fecha">Representa la fecha que se va a formatear</param>
        /// <returns></returns>
        public static string FechaPoliza(DateTime Fecha)
        {
            string resultado = Fecha.Date.Year.ToString();
            resultado += Fecha.Date.Month <= 9 ? "0" + Fecha.Date.Month.ToString() : Fecha.Date.Month.ToString();
            resultado += Fecha.Date.Day <= 9 ? "0" + Fecha.Date.Day.ToString() : Fecha.Date.Day.ToString();

            return resultado;
        }

        /// <summary>
        /// Obtiene el día domingo de semana santa. jueves(-3) y viernes(-2) se obtienen restando dias
        /// </summary>
        /// <param name="anio">Año del cual se quiere conocer el domingo de semana santa</param>
        /// <returns></returns>
        public static DateTime SemanaSanta(int anio)
        {
            int M = 25, N = 5;

            if (anio >= 1583 && anio <= 1699) { M = 22; N = 2; }
            else if (anio >= 1700 && anio <= 1799) { M = 23; N = 3; }
            else if (anio >= 1800 && anio <= 1899) { M = 23; N = 4; }
            else if (anio >= 1900 && anio <= 2099) { M = 24; N = 5; }
            else if (anio >= 2100 && anio <= 2199) { M = 24; N = 6; }
            else if (anio >= 2200 && anio <= 2299) { M = 25; N = 0; }

            int a, b, c, d, e, dia, mes;

            a = anio % 19;
            b = anio % 4;
            c = anio % 7;
            d = (19 * a + M) % 30;
            e = (2 * b + 4 * c + 6 * d + N) % 7;

            if (d + e < 10) { dia = d + e + 22; mes = 3; }
            else { dia = d + e - 9; mes = 4; }

            if (dia == 26 && mes == 4) dia = 19;
            if (dia == 25 && mes == 4 && d == 28 && e == 6 && a > 10) dia = 18;
            
            //Regresa el día domingo, restando dias se obtiene jueves, viernes, etc
            return new DateTime(anio, mes, dia);
        }

        /// <summary>
        /// Determina si un año es bisiesto
        /// </summary>
        /// <param name="anio">Año que quiere evaluarse</param>
        /// <returns></returns>
        public static bool AnioBisiesto(int anio)
        {
            return anio % 4 == 0 && anio % 100 != 0 || anio % 400 == 0 ? true : false;
        }

        private static List<int> DiasMes(int anio, int mes)
        {
            List<int> Dias = new List<int>();
            int noDias = DateTime.DaysInMonth(anio, mes);

            for (int i = 1; i <= noDias; i++)
                Dias.Add(i);

            return Dias;
        }

        public static DateTime Prorroga(int anio, int mes, bool finSemana, int diasProroga)
        {
            List<int> Dias = DiasMes(anio, mes);
            List<int> Festivos = DiasFestivos(anio, mes);

            Festivos.ForEach(festivo =>
            {
                Dias.Remove(festivo);
            });

            List<int> Aux = new List<int>(Dias);

            if (finSemana)
            {
                Dias.ForEach(dia =>
                {
                    int diaSemana = ((int)new DateTime(anio, mes, dia).DayOfWeek);
                    if (diaSemana < 1 | diaSemana > 5)
                        Aux.Remove(dia);
                });
            }

            try { return new DateTime(anio, mes, Aux[diasProroga - 1]); }
            catch { return new DateTime(1990, 0, 0); }
        }

        private static List<int> DiasFestivos(int anio, int mes)
        {
            List<int> Resultado = new List<int>();

            List<int[]> Festivos = new List<int[]> { 
                new int[] { 1 }, 
                new int[] { 5 }, 
                new int[] { 21 }, 
                new int[] {  }, 
                new int[] { 1 }, 
                new int[] {  }, 
                new int[] {  }, 
                new int[] {  }, 
                new int[] { 16 }, 
                new int[] {  }, 
                new int[] { 2, 20 }, 
                new int[] { 12, 25 }
            };

            List<int[]> Variables = new List<int[]> { 
                new int[]{},
                new int[]{5},
                new int[]{21},
                new int[]{},
                new int[]{},
                new int[]{},
                new int[]{},
                new int[]{},
                new int[]{},
                new int[]{},
                new int[]{20},
                new int[]{},
            };

            DateTime DomingoSanto = SemanaSanta(anio);

            //Si se quieren obtener dias festivos del mes de la semana santa
            if (DomingoSanto.Month == mes)
            {
                List<int> MesSanto = Festivos.ElementAt<int[]>(mes - 1).ToList();
                int[] diasSantos = new int[] { DomingoSanto.Day - 3, DomingoSanto.Day - 2 };
                MesSanto.AddRange(diasSantos);
                Festivos.RemoveAt(mes - 1);
                Festivos.Insert((mes - 1), MesSanto.ToArray());
            }

            Festivos.ElementAt<int[]>(mes -1).ToList().ForEach(dia => {
                if (Variables.ElementAt<int[]>(mes - 1).ToArray().Contains(dia))
                {
                    int diaSemana = (int)new DateTime(anio, mes, dia).DayOfWeek;
                    DateTime FechasBorrar = new DateTime(anio, mes, dia).AddDays(diaSemana > 0 & diaSemana <= 5 ? (1 - diaSemana) : (diaSemana == 6 ? 2 : 1));
                    Resultado.Add(FechasBorrar.Day);
                }
                else
                    Resultado.Add(dia);
            });

            return Resultado;
        }

     
    }
}
