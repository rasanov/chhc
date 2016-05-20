using AutoMapper;
using CHHC.DAL.Infrastructure;
using CHHC.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace CHHC.Web.Controllers
{
    public class VisitNotesController : Controller
    {
        public ActionResult New()
        {
            ViewBag.ActiveManuItem = AccountMenuItems.VisitNotes;

            var createVisitNote = new CreateVisitNote();

            createVisitNote.UserId = WebSecurity.CurrentUserId;
            createVisitNote.UserName = WebSecurity.CurrentUserName;

            using (var context = new DomainContext())
            {
                var patients = context.UserSettingsPatient
                    .Where(x => x.UserSettings.MembershipUserId == WebSecurity.CurrentUserId)
                    .OrderByDescending(x => x.DateAssigned)
                    .Select(x => x.Patient)
                    .ToList();
                var viewPatients = Mapper.Map<List<ViewPatient>>(patients);
                createVisitNote.ViewPatients = viewPatients;
            }

            return View(createVisitNote);
        }
    }
}
