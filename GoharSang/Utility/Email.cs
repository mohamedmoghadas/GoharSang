using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace System
{
    public class Email
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
        public string Password { get; set; }

        public string ErrorDescription { get; set; }
        public Email()
        {
            HostName = "smtp.gmail.com";
            Port = 587;
            From = "behtechproject@gmail.com";
            #region Property
            Password = "Apple1373";
            #endregion
        }


        public bool Send(string Body, string Subject, List<string> ListEmail)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.BodyEncoding = System.Text.UTF8Encoding.UTF8;
                mail.Subject = Subject;
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(From, " BehTech مدیریت ");
                mail.Body = "<div style='background-color:aliceblue;text-align:center;'>" + Body + "  </div>";
                foreach (var item in ListEmail)
                {
                    mail.To.Add(new MailAddress(item));
                }

                // mail.Attachments.Add(new Attachment(Global.RootDirectory+ "/Applicant/Images/12.jpg"));

                SmtpClient smtp = new SmtpClient(HostName, 587);
                smtp.Credentials = new NetworkCredential(From, Password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                ErrorDescription = ex.ToString();
                return false;
            }
        }
    }
}