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
            server.Timeout = 10000;
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
            // Sin credenciales no se intenta conectar.
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["SmtpUsuario"]) ||
                string.IsNullOrEmpty(ConfigurationManager.AppSettings["SmtpPassword"]))
                throw new InvalidOperationException("No están configuradas las credenciales de MailTrap (SmtpUsuario y SmtpPassword en appSettings.local.config).");

            server.Send(email);
        }
    }
}
