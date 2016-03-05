using System.Linq;
using System.Web.Mvc;
using HangFire.Mailer.Models;
using Hangfire;
using System;
using HangFire.Mailer.Services;

namespace HangFire.Mailer.Controllers
{
    public class HomeController : Controller
    {
        private readonly MailerDbContext _db = new MailerDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            var comments = _db.Comments.OrderBy(x => x.Id).ToList();
            return View(comments);
        }

        [HttpPost]
        public ActionResult Create(Comment model)
        {
            if (ModelState.IsValid)
            {
                _db.Comments.Add(model);
                _db.SaveChanges();

                // Send email to subscribers (background job)
                BackgroundJob.Enqueue(() => EmailService.NotifyNewComment(model.Id));
            }

            var email = new NewCommentEmail
            {
                To = "yourmail@example.com",
                UserName = model.UserName,
                Comment = model.Text
            };

            email.ViewName = "NewCommentEmail";
            email.Send();

            // Redirect to Index
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}