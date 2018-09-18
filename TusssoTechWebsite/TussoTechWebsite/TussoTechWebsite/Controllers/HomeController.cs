using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace TussoTechWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {

            return View();
        }


        public ActionResult ErrorsMessage(string message = "")
        {
            ViewBag.ErrorMessage = message;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.IsContacted = false;
            return View();
        }

        [HttpPost]
        public ActionResult Contact(string email, string name, string message, string phoneNo)
        {
            string fromEmail = ConfigurationManager.AppSettings["EmailUserName"];
            string websiteEmail = ConfigurationManager.AppSettings["WebsiteEmail"];
            string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            string emailHost = ConfigurationManager.AppSettings["EmailHost"];
            var mail = new MailMessage();

            mail.To.Add(fromEmail);
            mail.From = new MailAddress(websiteEmail);

            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = emailHost,
                Timeout = 10000,
                Credentials = new NetworkCredential(fromEmail, emailPassword)
            };
            mail.Subject = "Message from Tusso Technologies Webite.";
            string htmlBody = "<html><body>" +
                              "<h3>" + "Name: " + name + "</h3>" +
                              "<br>" +
                              "<h3>" + "Email: " + email + "</h3>" +
                              "<br>" +
                              "<h3>" + "Phone No: " + phoneNo + "</h3>" +
                              "<br>" +
                              "<h3>" + "Message: " + message + "</h3>" +
                              "<br>" +
                              "</body></html>";

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                (htmlBody, null, MediaTypeNames.Text.Html);

            mail.AlternateViews.Add(avHtml);
            try
            {
                client.Send(mail);
                ViewBag.IsContacted = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex);
            }

            return View();
        }

        public ActionResult Services()
        {
            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }

        public ActionResult Support()
        {
            return View();
        }
    }
}