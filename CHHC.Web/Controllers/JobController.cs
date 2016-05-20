using AutoMapper;
using CHHC.DAL.Infrastructure;
using CHHC.Web.Filters;
using CHHC.Web.Models;
using CHHC.Web.Models.JobModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;

namespace CHHC.Web.Controllers
{
    [AuthorizeAnyRole(RoleNames.CHHCAdmin)]
    [HandleError]
    public class JobController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Jobs;

            List<ViewJobApplication> viewJobApplication;
            using (var context = new DomainContext())
            {
                var jobApplication = context.JobApplication
                    .OrderByDescending(x => x.DateFilledAndAgreed)
                    .ToList();
                viewJobApplication = Mapper.Map<List<ViewJobApplication>>(jobApplication);
            }

            return View(viewJobApplication);
        }
        
        public ActionResult View(int id)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Jobs;

            using (var context = new DomainContext())
            {
                var jobApplication = context.JobApplication
                    .Include(x => x.Educations)
                    .Include(x => x.Works)
                    .Single(x => x.Id == id);

                var viewJobApplication = Mapper.Map<ViewJobApplication>(jobApplication);
                return View(viewJobApplication);
            }
        }
    }
}
