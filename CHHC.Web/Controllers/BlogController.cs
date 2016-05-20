using AutoMapper;
using CHHC.DAL.Infrastructure;
using CHHC.DomainModel;
using CHHC.Web.Filters;
using CHHC.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CHHC.Web.Controllers
{
    [AuthorizeAnyRole(RoleNames.CHHCAdmin)]
    [HandleError]
    public class BlogController : Controller
    {        
        [HttpGet]
        public ActionResult All(string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Blogs;
            ViewBag.StatusMessage = message;

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

        public ActionResult Create()
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Blogs;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewBlog model)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Blogs;
            if (ModelState.IsValid)
            {
                try
                {
                    using (var context = new DomainContext())
                    {
                        var blog = new Blog
                        {
                            Title = model.Title,
                            Text = model.Text,
                            Date = DateTime.Today
                        };

                        context.Blogs.Add(blog);
                        context.Entry(blog).State = EntityState.Added;
                        context.SaveChanges();
                    }

                    return RedirectToAction("All", new { Message = "Blog '" + model.Title + "' has been created." });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            return View(model);
        }

        public ActionResult Edit(int id, string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Blogs;
            ViewBag.StatusMessage = message;
            using (var context = new DomainContext())
            {
                var blog = context.Blogs.SingleOrDefault(x => x.Id == id);
                var viewBlog = Mapper.Map<ViewBlog>(blog);
                return View(viewBlog);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewBlog model)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Blogs;
            if (ModelState.IsValid)
            {
                try
                {
                    using (var context = new DomainContext())
                    {
                        var blog = context.Blogs.Single(x => x.Id == model.Id);
                        blog.Title = model.Title;
                        blog.Text = model.Text;
                        blog.Date = DateTime.Today;
                        context.SaveChanges();
                    }

                    return RedirectToAction("Edit", new { model.Id, Message = "Blog has been changed." });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                using (var context = new DomainContext())
                {
                    var blog = new Blog { Id = id };
                    context.Blogs.Attach(blog);
                    context.Blogs.Remove(blog);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("All", new { Message = "Blog can not be deleted. " + e.Message });
            }

            return RedirectToAction("All", new { Message = "Blog has been deleted." });
        }
    }
}
