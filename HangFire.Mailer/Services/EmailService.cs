using HangFire.Mailer.Models;
using Postal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace HangFire.Mailer.Services
{
    public class EmailService
    {

        public static void NotifyNewComment(int commentId)
        {
            // Prepare Postal classes to work outside of ASP.NET request
            var viewsPath = Path.GetFullPath(HostingEnvironment.MapPath(@"~/Views/Emails"));
            var engines = new ViewEngineCollection();
            engines.Add(new FileSystemRazorViewEngine(viewsPath));

            var postalEmailService = new Postal.EmailService(engines);

            // Get comment and send a notification.
            using (var db = new MailerDbContext())
            {
                var comment = db.Comments.Find(commentId);

                var email = new NewCommentEmail
                {
                    To = "yourmail@example.com",
                    UserName = comment.UserName,
                    Comment = comment.Text
                };

                email.ViewName = "NewCommentEmail";
                postalEmailService.Send(email);
            }
        }
    }
}