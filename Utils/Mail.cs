using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebApp.Utils
{
    public class Mail
    {
        public string NOME_DESTINATARIO { get; set; }
        public string EMAIL_DESTINATARIO { get; set; }
        public string ASSUNTO { get; set; }
        public string MENSAGEM { get; set; }

        public Mail()
        {
            EMAIL_DESTINATARIO = "";
            ASSUNTO = "";
            MENSAGEM = "";
        }

        public bool EnviarEmail()
        {
            // Email de teste
            string remetente = "ideia.idealizando@gmail.com";
            string senha = "369963256";
            string msg = this.MENSAGEM;

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = new System.Net.Mail.MailAddress(remetente);
            mail.To.Add(new System.Net.Mail.MailAddress(this.EMAIL_DESTINATARIO));
            mail.Subject = this.ASSUNTO;
            mail.Body = msg;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential(remetente, senha);
            
            try
            {
                smtp.Send(mail);
                return true;
            }
            catch (Exception erro)
            {
                return false;
            }
            finally
            {
                mail = null;
            }
        }
    }
}