using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class DigitoVerificador
    {
        public static int Obtener(int referenciaId)
        {
            int contador = 1, validador = 0, totalSuma = 0, valor = 0, verificador = 0, i = 1;
            string digito, referencia;

            referencia = string.Concat(Enumerable.Repeat(" ", 10 - referenciaId.ToString().Length)) + referenciaId;

            do
            {
                digito = referencia.Substring((referencia.Length - i), 1);
                contador++;
                i++;

                if (contador == 11)
                    break;

                if (validador == 0)
                {
                    if(digito == "0" || digito == " ")
                    {
                        valor = 0;
                        validador = 1;
                    }
                    else
                    {
                        switch (digito)
                        {
                            case "1":
                                valor = 2;
                                break;
                            case "2":
                                valor = 4;
                                break;
                            case "3":
                                valor = 6;
                                break;
                            case "4":
                                valor = 8;
                                break;
                            case "5":
                                valor = 1;
                                break;
                            case "6":
                                valor = 3;
                                break;
                            case "7":
                                valor = 5;
                                break;
                            case "8":
                                valor = 7;
                                break;
                            case "9":
                                valor = 9;
                                break;
                        }

                        validador = 1;
                    }
                }
                else
                {
                    if (digito == " ")
                        digito = "0";
                    valor = int.Parse(digito);
                    validador = 0;
                }

                totalSuma = totalSuma + valor;

            } while (true);

            verificador = 10 - int.Parse((totalSuma >= 10) ? totalSuma.ToString().Substring(totalSuma.ToString().Length - 1, 1) : totalSuma.ToString());

            return verificador == 10 ? 0: verificador;
        }
    }
}
