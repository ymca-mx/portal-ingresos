using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Email
    {
        public static bool Enviar(string remitente, string password, string display, string destinos, char separadorDestinos,
                                  string asunto, string body, string adjuntos, char separadorAdjuntos,
                                  string smtp, int puertoSmtp, bool isSsl, bool isHtml, ref Utilities.ProcessResult Resultado)
        {
            string[] destinatarios = Utilities.Cadena.Separar(separadorDestinos, destinos);
            string[] archivosAdjuntos = adjuntos == string.Empty ? null : Utilities.Cadena.Separar(separadorAdjuntos, adjuntos);

                MailMessage Nuevo = new MailMessage();
                Nuevo.From = new MailAddress(remitente, display);

                if (destinatarios.Length > 0)
                    destinatarios.ToList().ForEach(destino =>
                    {
                        Nuevo.To.Add(destino);
                    });

                else 
                    return false;

                Nuevo.Subject = asunto;
                Nuevo.Body = body;
                Nuevo.IsBodyHtml = isHtml;
                
                if (archivosAdjuntos != null)
                {
                    archivosAdjuntos.ToList().ForEach(adjunto => {
                        Nuevo.Attachments.Add(new Attachment(adjunto));
                    });
                }

                SmtpClient ServidorSmtp = new SmtpClient(smtp, puertoSmtp);
                ServidorSmtp.Credentials = new NetworkCredential(remitente, password);
                ServidorSmtp.EnableSsl = isSsl;
                try
                {
                    ServidorSmtp.Send(Nuevo);
                    Nuevo.Dispose();
                    ServidorSmtp.Dispose();

                    return true;
                }
                catch (Exception Ex)
                {
                    Resultado.Estatus = true;
                    Resultado.Mensaje = Ex.Message;
                    Resultado.MensajeDetalle = (Ex.InnerException != null) ? Ex.InnerException.InnerException.Message : string.Empty;
                    Resultado.Informacion = "Email.Enviar()";

                    return false;
                }
        }
        public static bool Enviar(string remitente, string password, string display, string destinos, char separadorDestinos,
            string DestinosCopia, char SeparadorDestinosCC,string asunto, string body, string adjuntos, char separadorAdjuntos,
            string smtp, int puertoSmtp, bool isSsl, bool isHtml, ref Utilities.ProcessResult Resultado)
        {
            string[] destinatarios = Utilities.Cadena.Separar(separadorDestinos, destinos);
            string[] archivosAdjuntos = adjuntos == string.Empty ? null : Utilities.Cadena.Separar(separadorAdjuntos, adjuntos);

            MailMessage Nuevo = new MailMessage();
            Nuevo.From = new MailAddress(remitente, display);

            if (DestinosCopia.Length > 1)
            {
                string[] Destinos = DestinosCopia.Split(SeparadorDestinosCC);
                foreach (string cc in Destinos)
                {
                    Nuevo.Bcc.Add(cc);
                }
            }

            if (destinatarios.Length > 0)
                destinatarios.ToList().ForEach(destino =>
                {
                    Nuevo.To.Add(destino);
                });

            else
                return false;

            Nuevo.Subject = asunto;
            Nuevo.Body = body;
            Nuevo.IsBodyHtml = isHtml;
            

            if (archivosAdjuntos != null)
            {
                archivosAdjuntos.ToList().ForEach(adjunto =>
                {
                    Nuevo.Attachments.Add(new Attachment(adjunto));
                });
            }

            SmtpClient ServidorSmtp = new SmtpClient(smtp, puertoSmtp);
            ServidorSmtp.Credentials = new NetworkCredential(remitente, password);
            ServidorSmtp.EnableSsl = isSsl;
            try
            {
                ServidorSmtp.Send(Nuevo);
                Nuevo.Dispose();
                ServidorSmtp.Dispose();

                return true;
            }
            catch (Exception Ex)
            {
                Resultado.Estatus = true;
                Resultado.Mensaje = Ex.Message;
                Resultado.MensajeDetalle = (Ex.InnerException != null) ? Ex.InnerException.InnerException.Message : string.Empty;
                Resultado.Informacion = "Email.Enviar()";

                return false;
            }
        }

        public static ProcessResult Enviar(string remitente, string password, string display, string destinos, char separadorDestinos,
    string DestinosCopia, char SeparadorDestinosCC, string asunto, string body, string adjuntos, char separadorAdjuntos,
    string smtp, int puertoSmtp, bool isSsl, bool isHtml)
        {
            string[] destinatarios = Separar(separadorDestinos, destinos);
            string[] archivosAdjuntos = adjuntos == string.Empty ? null : Separar(separadorAdjuntos, adjuntos);

            MailMessage Nuevo = new MailMessage()
            {
                From = new MailAddress(remitente, display)
            };
            if (DestinosCopia.Length > 1)
            {
                string[] Destinos = DestinosCopia.Split(SeparadorDestinosCC);
                foreach (string cc in Destinos)
                {
                    Nuevo.Bcc.Add(cc);
                }
            }

            if (destinatarios.Length > 0)
                destinatarios.ToList().ForEach(destino =>
                {
                    Nuevo.To.Add(destino);
                });

            else
                return new ProcessResult
                {
                    Estatus = false,
                    Mensaje = "No existen destinararios para enviar el email."
                };

            Nuevo.Subject = asunto;
            Nuevo.Body = body;
            Nuevo.IsBodyHtml = isHtml;

            if (archivosAdjuntos != null)
            {
                archivosAdjuntos.ToList().ForEach(adjunto =>
                {
                    Nuevo.Attachments.Add(new Attachment(adjunto));
                });
            }

            SmtpClient ServidorSmtp = new SmtpClient(smtp, puertoSmtp)
            {
                Credentials = new NetworkCredential(remitente, password),
                EnableSsl = isSsl
            };
            try
            {
                ServidorSmtp.Send(Nuevo);
                Nuevo.Dispose();
                ServidorSmtp.Dispose();

                return new ProcessResult
                {
                    Estatus = true,
                    Mensaje = "Correo enviado correctamente."
                };
            }
            catch (Exception Ex)
            {
                return new ProcessResult
                {
                    Estatus = false,
                    Mensaje = "Error al enviar el email: " + Ex.Message
                };
            }
        }

        public static string[] Separar(char separador, string texto)
        {
            char[] separadores = { separador };
            return texto.Split(separadores);
        }
    }
}
