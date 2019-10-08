using System;
using System.Configuration;

namespace ALZAGRO.AppRendicionGastos.Fwk.Email {
    public class Mailing {

        public static void SendGenericMail(string subject, String to, object model, string template) {

            var html = ViewRenderer.RenderView(template, model);

            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();

            String sendMail = ConfigurationManager.AppSettings["SendMail"].ToString();
            if (sendMail == "true") {
                String mailSender = ConfigurationManager.AppSettings["MailSender"].ToString();
                var mailReceivers = ConfigurationManager.AppSettings["MailReceiver"].ToString().Split(';');

                String host = ConfigurationManager.AppSettings["relayHost"].ToString();
                String passwordMailSender = ConfigurationManager.AppSettings["PasswordMailSender"].ToString();

                if (mailSender != null && mailReceivers != null && host != null) {
                    mailMessage.From = new System.Net.Mail.MailAddress(mailSender);

                    if (!String.IsNullOrEmpty(to)) {
                        mailMessage.To.Add(to);
                    }
                    else {
                        foreach (var item in mailReceivers) {
                            if (!String.IsNullOrEmpty(item)) {
                                mailMessage.To.Add(item);
                            }
                        }
                    }

                    mailMessage.Subject = subject;
                    mailMessage.Body = html;
                    mailMessage.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.EnableSsl = true;
                    smtp.Host = host;
                    smtp.Credentials = new System.Net.NetworkCredential(mailSender, passwordMailSender);
                    try {
                        smtp.Send(mailMessage);
                    }
                    catch (Exception ex) {
                        System.Diagnostics.Trace.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}