using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTLIB
{
    public class Gmail
    {
        private String FromEmail { get; set; }
        private String ID { get; set; }
        private String Pass { get; set; }
        private System.Net.Mail.SmtpClient _smtp;

        public Gmail(String fromEmail, String id, String pass)
        {
            this.FromEmail = fromEmail;
            this.ID = id;
            this.Pass = pass;
            this._smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            this._smtp.Credentials = new System.Net.NetworkCredential(id, pass);
            this._smtp.EnableSsl = true;
        }

        public void SentMail(String toEmail, String subject, String message)
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(this.FromEmail, toEmail, subject, message);
            this._smtp.Send(msg);
        }
    }
}
