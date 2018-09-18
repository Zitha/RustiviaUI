using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace TussoTechWebsite.Models
{
    public class HelperFunction
    {
        public void DeleteFile(string fileLocation)
        {
            if (System.IO.File.Exists(fileLocation))
            {
                System.IO.File.Delete(fileLocation);
            }
        }

        public void SendInvoiceEmail(string emailAddress, string description, string attachmentFilename)
        {
            string[] reciepientts = emailAddress.Split(',');

            //string imgUrl = ConfigurationManager.AppSettings["LogoPath"];
            string fromEmail = ConfigurationManager.AppSettings["WebsiteEmail"];
            string emailUserName = ConfigurationManager.AppSettings["EmailUserName"];
            string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            string emailHost = ConfigurationManager.AppSettings["EmailHost"];
            var mail = new MailMessage();

            foreach (string reciepient in reciepientts)
            {
                mail.To.Add(reciepient);
            }

            mail.From = new MailAddress(fromEmail);

            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = emailHost,
                Timeout = 10000,
                Credentials = new NetworkCredential(emailUserName, emailPassword)
            };
            mail.Subject = description;
            const string htmlBody = "<html><body><h4>Good day</h4>" +
                                    "<br><p>Please find attached Invoice</p>" +
                                    "</body></html>";

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                (htmlBody, null, MediaTypeNames.Text.Html);

            mail.AlternateViews.Add(avHtml);

            if (attachmentFilename != null)
            {
                var attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                try
                {
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mail.Attachments.Add(attachment);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                  ex);
                }
                finally
                {
                    if (attachment != null)
                    {
                        attachment.Dispose();
                    }
                }
            }
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex);
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
                if (avHtml != null)
                {
                    avHtml.Dispose();
                }
                if (mail != null)
                {
                    mail.Dispose();
                }
            }
        }

        public void SendQouteEmail(string emailAddress, string description, string attachmentFilename)
        {
            string[] reciepientts = emailAddress.Split(',');

            //string imgUrl = ConfigurationManager.AppSettings["LogoPath"];
            string fromEmail = ConfigurationManager.AppSettings["WebsiteEmail"];
            string emailUserName = ConfigurationManager.AppSettings["EmailUserName"];
            string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            string emailHost = ConfigurationManager.AppSettings["EmailHost"];
            var mail = new MailMessage();

            foreach (string reciepient in reciepientts)
            {
                mail.To.Add(reciepient);
            }

            mail.From = new MailAddress(fromEmail);

            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = emailHost,
                Timeout = 10000,
                Credentials = new NetworkCredential(emailUserName, emailPassword)
            };
            mail.Subject = description;
            const string htmlBody = "<html><body><h4>Good day</h4>" +
                                    "<br><p>Please find attached Quotation</p>" +
                                    "</body></html>";

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                (htmlBody, null, MediaTypeNames.Text.Html);

            mail.AlternateViews.Add(avHtml);

            if (attachmentFilename != null)
            {
                var attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = attachment.ContentDisposition;
                try
                {
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mail.Attachments.Add(attachment);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                  ex);
                }
                finally
                {
                    if (attachment != null)
                    {
                        attachment.Dispose();
                    }
                }
            }
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex);
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
                if (avHtml != null)
                {
                    avHtml.Dispose();
                }
                if (mail != null)
                {
                    mail.Dispose();
                }
            }
        }
    }
}