using ALZAGRO.AppRendicionGastos.Fwk.CrossCutting;
using System;
using System.Configuration;

namespace ALZAGRO.AppRendicionGastos.CrossCutting {
    public class EmailService : IEmailService {

        public void SendGenericMail(string subject, string message) {
            try {
                SendGenericMail("", subject, message);
            }
            catch (Exception) {

            }
        }

        public void SendGenericMail(string to, string subject, string message) {
            try {
                String useRelay = ConfigurationManager.AppSettings["UseRelay"].ToString();

                if (useRelay == "true") {
                    //SendByRelay(subject, message, to);
                }
                else {
                    SendNormalMail(to, subject, message);
                }
            }
            catch (Exception) {

            }
        }

        //private void SendByRelay(string subject, string message, String to) {
        //    String relaySenderMail = ConfigurationManager.AppSettings["RelaySenderMail"].ToString();
        //    String relaySenderText = ConfigurationManager.AppSettings["RelaySenderText"].ToString();


        //    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        //    mail.From = new System.Net.Mail.MailAddress(relaySenderMail, relaySenderText);
        //    mail.ReplyTo = new System.Net.Mail.MailAddress(relaySenderMail, relaySenderText);

        //    if (!String.IsNullOrEmpty(to)) {
        //        mail.To.Add(to);
        //    }
        //    else {

        //        var mailReceivers = ConfigurationManager.AppSettings["MailReceiver"].ToString().Split(';');
        //        foreach (var item in mailReceivers) {
        //            if (!String.IsNullOrEmpty(item)) {
        //                mail.To.Add(item);
        //            }
        //        }
        //    }

        //    mail.Subject = subject;
        //    mail.Body = message;
        //    mail.IsBodyHtml = true;
        //    mail.Priority = System.Net.Mail.MailPriority.Normal;
        //    System.Net.Mail.SmtpClient Smtp = new System.Net.Mail.SmtpClient();

        //    String relayHost = ConfigurationManager.AppSettings["RelayHost"].ToString();

        //    Smtp.Host = relayHost;
        //    Smtp.Send(mail);
        //}

        private void SendNormalMail(String to, string subject, string message) {

            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();

            String sendMail = ConfigurationManager.AppSettings["SendMail"].ToString();
            if (sendMail == "true") {
                String mailSender = ConfigurationManager.AppSettings["MailSender"].ToString();
                var mailReceivers = ConfigurationManager.AppSettings["MailReceiver"].ToString().Split(';');

                String host = ConfigurationManager.AppSettings["Host"].ToString();
                String passwordMailSender = ConfigurationManager.AppSettings["PasswordMailSender"].ToString();
                String useDefaultCredentials = ConfigurationManager.AppSettings["UseDefaultCredentials"].ToString();

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
                    mailMessage.Body = message;
                    mailMessage.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.EnableSsl = true;
                    smtp.Host = host;

                    if (!String.IsNullOrWhiteSpace(passwordMailSender)) {
                        smtp.Credentials = new System.Net.NetworkCredential(mailSender, passwordMailSender);
                    }
                    else {
                        smtp.UseDefaultCredentials = Convert.ToBoolean(useDefaultCredentials);
                    }
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