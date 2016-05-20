using System;
using System.Web.Mvc;
using CHHC.Web.Models;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using CHHC.DAL.Infrastructure;
using AutoMapper;
using System.Linq;
using CHHC.Web.Models.JobModels;
using CHHC.DomainModel;
using System.Data;

namespace CHHC.Web.Controllers
{
	[HandleError]
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

        public ActionResult Services()
        {
            return View();
        }

        #region Contact
        [HttpGet]
        public ActionResult Contact()
        {
            var contact = new Contact();
            return View(contact);
        }

        [HttpPost]
        public ActionResult ContactSubmit(Contact contact)
        {
            if (string.IsNullOrEmpty(contact.Email))
            {
                contact.Status = "emptyEmail";
                contact.Notification = "Please input email address.";
                return View("Contact", contact);
            }

            try
            {
                var newMessage = new MailMessage();
                newMessage.From = new MailAddress("chhc.massachusetts@gmail.com");
                newMessage.To.Add("info@centralhomehealthcare.com");
                newMessage.IsBodyHtml = true;
                newMessage.Subject = "Message Online Contact Form";
                newMessage.Body =
                     "<b>Full name:</b><br />"
                    + contact.Name + "<br /><br />"
                    + "<b>Email:</b><br />"
                    + contact.Email + "<br /><br />"
                    + "<b>Phone:</b><br />"
                    + contact.Phone + "<br /><br />"
                    + "<b>Message:</b><br />"
                    + contact.Message + "<br /><br />";
                // Sent from account
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com"); // smtps - 465, msa - 587 //do not use port in winhost hosting
                smtpServer.Credentials = new NetworkCredential("chhc.massachusetts@gmail.com", "centralhhc01");
                smtpServer.EnableSsl = true;
                smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpServer.Send(newMessage);
            }
            catch (Exception ex)
            {
                contact.Status = "error";
                contact.Notification = "Message cannot be delivered now, please try again later." + "<br>" + ex.Message;
                return View("Contact", contact);
            }

            return RedirectToAction("ContactSubmitSuccess");
        }

        [HttpGet]
        public ActionResult ContactSubmitSuccess()
        {
            var contact = new Models.Contact();
            contact.Status = "success";
            contact.Notification = "Your message was successfully submitted. You will be contacted shortly.";
            return View("Contact", contact);
        }

        #endregion Contact

        #region Job
        [HttpGet]
        public ActionResult Job()
        {
            var jobApplication = new ViewJobApplication();
            jobApplication.Educations.Add(new ViewJobApplicationEducation { Id = 1, Type = "High School" });
            jobApplication.Educations.Add(new ViewJobApplicationEducation { Id = 2, Type = "College, University or Technical School" });
            jobApplication.Educations.Add(new ViewJobApplicationEducation { Id = 3, Type = "College, University or Technical School" });
            jobApplication.Educations.Add(new ViewJobApplicationEducation { Id = 4, Type = "College, University or Technical School" });
            jobApplication.Educations.Add(new ViewJobApplicationEducation { Id = 5, Type = "Other Education (Specify)" });
            jobApplication.Works.Add(new ViewJobApplicationWork { Id = 1 });
            jobApplication.Works.Add(new ViewJobApplicationWork { Id = 2 });
            jobApplication.Works.Add(new ViewJobApplicationWork { Id = 3 });
            return View(jobApplication);
        }

        [HttpPost]
        public ActionResult JobSubmit(ViewJobApplication viewJobApplication)
        {
            try
            {
                using (var context = new DomainContext())
                {
                    var jobApplication = Mapper.Map<JobApplication>(viewJobApplication);

                    context.JobApplication.Add(jobApplication);
                    context.Entry(jobApplication).State = EntityState.Added;
                    context.SaveChanges();
                }

                var newMessage = new MailMessage();
                newMessage.From = new MailAddress("chhc.massachusetts@gmail.com");
                //newMessage.To.Add("info@centralhomehealthcare.com");
                newMessage.To.Add("rasanov@gmail.com");
                newMessage.IsBodyHtml = true;
                newMessage.Subject = "Online job application";

                newMessage.Body = "<b>Job title:</b><br />"
                    + viewJobApplication.Position + "<br /><br />"
                    + "<b>First name:</b><br />"
                    + viewJobApplication.ApplicantFirstName + "<br /><br />"
                    + "<b>Last name:</b><br />"
                    + viewJobApplication.ApplicantLastName + "<br /><br />"
                    + "<b>Address:</b><br />"
                    + viewJobApplication.StreetAddress + "<br /><br />"
                    + "<b>City:</b><br />"
                    + viewJobApplication.City + "<br /><br />"
                    + "<b>Zip:</b><br />"
                    + viewJobApplication.Zip + "<br /><br />"
                    + "<b>State:</b><br />"
                    + viewJobApplication.State + "<br /><br />"
                    + "<b>Phone:</b><br />"
                    + viewJobApplication.PhoneCell + "<br /><br />"
                    + "<b>Email:</b><br />"
                    + viewJobApplication.Email + "<br /><br />"
                    + "<b>Message:</b>";

                // Sent from account
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com"); // smtps - 465, msa - 587 //do not use port in winhost hosting
                smtpServer.Credentials = new NetworkCredential("chhc.massachusetts@gmail.com", "centralhhc01");
                smtpServer.EnableSsl = true;
                smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpServer.Send(newMessage);
            }
            catch
            {
                viewJobApplication.ErrorMessage = "Message cannot be delivered now, please try again later.";
                return View("Job", viewJobApplication);
            }

            return View("JobSubmitSuccess");
        }

        #endregion Job

        #region Blog

        public ActionResult Blogs()
        {
            List<ViewBlog> viewBlogs;
            using (var context = new DomainContext())
            {
                var blogs = context.Blogs
                    .OrderByDescending(x => x.Date)
                    .ToList();
                viewBlogs = Mapper.Map<List<ViewBlog>>(blogs);
            }

            return View(viewBlogs);
        }

        public ActionResult Blog(int id)
        {
            ViewBlog viewBlog;

            using (var context = new DomainContext())
            {
                var blog = context.Blogs.SingleOrDefault(x => x.Id == id);
                if (blog != null)
                {
                    viewBlog = Mapper.Map<ViewBlog>(blog);
                }
                else
                {
                    RedirectToAction("Blogs");
                    return null;
                }
            }

            return View(viewBlog);
        }

        #endregion
    }
}
