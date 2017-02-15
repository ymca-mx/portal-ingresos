using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utilities
{
    public class Validacion
    {
        //public static bool Form(Form Ventana)
        //{
        //    bool bVal = true;
        //    foreach (System.Windows.Forms.Control b in Ventana.Controls)
        //    {
        //        //Si son campos requeridos...
        //        if (b.Name.Contains("1"))
        //        {
        //            if (b is CustomControl.TextBoxTip)
        //            {
        //                //Si hay campos vacios que requieren tener un dato
        //                if ((Validacion.Vacio(b.Text.ToString())))
        //                {
        //                    b.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6D8CE");
        //                    bVal = false;
        //                }

        //                //Si no estan vacios
        //                else
        //                {
        //                    b.BackColor = Color.FromKnownColor(KnownColor.Window);

        //                    //Si son del tipo Mail
        //                    if (b.Name.Contains("2"))
        //                    {
        //                        if (!Validacion.Email(b.Text.ToString()))
        //                        {
        //                            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6D8CE");
        //                            bVal = false;
        //                        }
        //                        else
        //                            b.BackColor = Color.FromKnownColor(KnownColor.Window);
        //                    }
        //                    //Si son numericos
        //                    else if (b.Name.Contains("3"))
        //                    {
        //                        if (!Validacion.Numero(b.Text.ToString()))
        //                        {
        //                            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6D8CE");
        //                            bVal = false;
        //                        }
        //                    }

        //                    else if (b.Name.Contains("4"))
        //                    {
        //                        if (!Validacion.Decimal(b.Text.ToString()))
        //                        {
        //                            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6D8CE");
        //                            bVal = false;
        //                        }
        //                        else
        //                            b.BackColor = Color.FromKnownColor(KnownColor.Window);
        //                    }

        //                    else if (b.Name.Contains("5"))
        //                    {
        //                        if (!Validacion.Moneda(b.Text.ToString()))
        //                        {
        //                            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6D8CE");
        //                            bVal = false;
        //                        }
        //                        else
        //                            b.BackColor = Color.FromKnownColor(KnownColor.Window);
        //                    }

        //                    else if (b.Name.Contains("6"))
        //                    {
        //                        if (!Validacion.Porcentaje(b.Text.ToString()))
        //                        {
        //                            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6D8CE");
        //                            bVal = false;
        //                        }
        //                        else
        //                            b.BackColor = Color.FromKnownColor(KnownColor.Window);
        //                    }
        //                }
        //            }
        //            if (b is System.Windows.Forms.ComboBox)
        //            {
        //                ComboBox cmb = (ComboBox)b;
        //                if (!Validacion.Combo(cmb.SelectedIndex))
        //                {
        //                    cmb.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6D8CE");
        //                    bVal = false;
        //                }
        //                else
        //                    cmb.BackColor = Color.FromKnownColor(KnownColor.Window);
        //            }

        //            //Sin son requeridos tipo boton
        //            if (b is System.Windows.Forms.Button)
        //            {
        //                Button btn = (Button)b;
        //                //btn.Text = "Dos \nLineas";
        //                if (btn.Text.Contains("agregar"))
        //                {
        //                    //btn.Text = "X\n" + b.Text;
        //                    btn.FlatAppearance.BorderSize = 2;
        //                    btn.FlatAppearance.BorderColor = System.Drawing.ColorTranslator.FromHtml("#FE2E2E");
        //                    bVal = false;
        //                }
        //                else
        //                {
        //                    //btn.Text = "X\n" + b.Text;
        //                    btn.FlatAppearance.BorderSize = 1;
        //                    btn.FlatAppearance.BorderColor = Color.Gray;
        //                }
        //            }
        //        }

        //        //Si son campos opcionales
        //        else
        //        {
        //            //Si son TextBox
        //            if (b is CustomControl.TextBoxTip)
        //            {
        //                //Y tienen datos..
        //                if (!(Validacion.Vacio(b.Text.ToString())))
        //                {
        //                    if (b.Name.Contains("2"))
        //                    {
        //                        if (!Validacion.Email(b.Text.ToString()))
        //                        {
        //                            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6D8CE");
        //                            bVal = false;
        //                        }
        //                        else
        //                            b.BackColor = Color.FromKnownColor(KnownColor.Window);
        //                    }
        //                    //Si son numericos
        //                    else if (b.Name.Contains("3"))
        //                    {
        //                        if (!Validacion.Numero(b.Text.ToString()))
        //                        {
        //                            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6D8CE");
        //                            bVal = false;
        //                        }
        //                        else
        //                            b.BackColor = Color.FromKnownColor(KnownColor.Window);
        //                    }

        //                    else if (b.Name.Contains("4"))
        //                    {
        //                        if (!Validacion.Decimal(b.Text.ToString()))
        //                        {
        //                            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6D8CE");
        //                            bVal = false;
        //                        }
        //                        else
        //                            b.BackColor = Color.FromKnownColor(KnownColor.Window);
        //                    }

        //                    else if (b.Name.Contains("5"))
        //                    {
        //                        if (!Validacion.Moneda(b.Text.ToString()))
        //                        {
        //                            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6D8CE");
        //                            bVal = false;
        //                        }
        //                        else
        //                            b.BackColor = Color.FromKnownColor(KnownColor.Window);
        //                    }

        //                    else if (b.Name.Contains("6"))
        //                    {
        //                        if (!Validacion.Porcentaje(b.Text.ToString()))
        //                        {
        //                            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6D8CE");
        //                            bVal = false;
        //                        }
        //                        else
        //                            b.BackColor = Color.FromKnownColor(KnownColor.Window);
        //                    }
        //                }

        //                //Si no tiene datos.
        //                else
        //                    b.BackColor = Color.FromKnownColor(KnownColor.Window);
        //            }
        //        }
        //    }

        //    return bVal;
        //}

        public static bool Fechas(DateTime FechaInicial, DateTime FechaFinal)
        {
            if (FechaInicial.Date <= FechaFinal.Date)
                return true;
            else
                return false;
        }

        public static bool Email(String sEmail)
        {

            string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (Regex.IsMatch(sEmail, expresion))
            {
                if (Regex.Replace(sEmail, expresion, String.Empty).Length == 0)
                    return true;
                else
                    return false;
            }
            else
                return false;

        }

        public static bool Vacio(String sCadena)
        {
            if (sCadena == "")
                return true;
            else
                return false;
        }

        public static bool Numero(String sNumero)
        {
            try
            {
                int.Parse(sNumero);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Decimal(String sNumero)
        {
            try
            {
                double.Parse(sNumero);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Porcentaje(String sNumero)
        {
            try
            {
                double.Parse(sNumero.Replace("%", ""));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Moneda(String sNumero)
        {
            try
            {
                double.Parse(sNumero, NumberStyles.Currency);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Combo(int iSeleccionado)
        {
            if (iSeleccionado > 0)
                return true;
            else
                return false;
        }

        public static bool Fecha(String Fecha)
        {
            try
            {
                DateTime.Parse(Fecha);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
