using System;
using System.Net.Mail;

namespace Middleware_TimeAtt
{
    class SendMail
    {
        public static void Send(string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(ConfReader.Read("smtpserver"));

                mail.From = new MailAddress(ConfReader.Read("senderemail"));
                mail.To.Add(ConfReader.Read("contactemail"));
                mail.Subject = subject;
                mail.Body = body;

                SmtpServer.Port = Int32.Parse(ConfReader.Read("smtpserverport"));
                SmtpServer.Credentials = new System.Net.NetworkCredential(ConfReader.Read("senderemail"), ConfReader.Read("senderemailpass"));
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                Logger.WriteLog("INFO: mail Send to " + ConfReader.Read("contactemail"));
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ERROR: mail Send failed" + ex.Message.ToString().Trim());
            }
        }
    }
}
