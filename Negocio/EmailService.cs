using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class EmailService
    {
        private MailMessage email;
        private SmtpClient server;

        public EmailService()
        {
            server = new SmtpClient();
            server.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SmtpUsuario"], ConfigurationManager.AppSettings["SmtpPassword"]);
            server.EnableSsl = true;
            server.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPuerto"]);
            server.Host = ConfigurationManager.AppSettings["SmtpHost"];
        }

        public void armarCorreo(string emailDestino, string asunto, string cuerpo)
        {
            email = new MailMessage();
            email.From = new MailAddress(ConfigurationManager.AppSettings["SmtpRemitente"]);
            email.To.Add(emailDestino);
            email.Subject = asunto;
            email.IsBodyHtml = true;
            email.Body = cuerpo;
        }

        public void enviarEmail()
        {
            try
            {
                server.Send(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
