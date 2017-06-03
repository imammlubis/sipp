using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Sipp.Web.Utils
{
    public class EmailServices
    {
        public int SendAsyncDefault(EmailContent emailContent)
        {
            // Plug in your email service here to send an email.
            try
            {
                //Utils.Logger.WriteLog("---- sending email --------");
                #region dolphin_mail              
                //SmtpClient client = new SmtpClient("5.172.194.212", 587);
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.Credentials =
                //  new System.Net.NetworkCredential("reports@dolphin-group.com",
                //  "reports@09");
                //MailMessage mailMessage = new MailMessage();
                //mailMessage.From = new MailAddress("reports@dolphin-group.com",
                //    "Dolphin Alarming Service");
                //foreach (var item in emailContent.Destination)
                //{
                //    try
                //    {
                //        mailMessage.To.Add(new MailAddress(item));
                //    }
                //    catch (Exception) { }
                //}
                //mailMessage.Body = emailContent.Body;
                //mailMessage.Subject = emailContent.Subject;
                //mailMessage.IsBodyHtml = true;
                //client.EnableSsl = false;
                //client.Send(mailMessage);
                #endregion

                #region gmail
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress("auditesdm@gmail.com");
                foreach (var item in emailContent.Destination)
                {
                    try
                    {
                        mailMessage.To.Add(new MailAddress(item));
                    }
                    catch (Exception) { }
                }
                foreach (var item in emailContent.CC)
                {
                    try
                    {
                        mailMessage.CC.Add(new MailAddress(item));
                    }
                    catch (Exception) { }
                }
                mailMessage.Body = emailContent.Body;
                mailMessage.Subject = emailContent.Subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.From = new MailAddress("auditesdm@gmail.com", emailContent.Subject);

                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials =
                    new System.Net.NetworkCredential("auditesdm@gmail.com", "jangkrikbos");
                client.EnableSsl = true;
                client.Send(mailMessage);
                #endregion
                return 1;// Task.FromResult(1);
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                //Utils.Logger.WriteLog(ex.ToString());
                return 0;// Task.FromResult(0);
            }
        }
        public void WriteLog(string message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + message.Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
    }
}