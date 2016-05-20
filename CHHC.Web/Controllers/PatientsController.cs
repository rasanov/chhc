using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CHHC.DAL.Infrastructure;
using CHHC.DomainModel;
using CHHC.Web.Filters;
using CHHC.Web.Models;
using CHHC.Web.Services;

namespace CHHC.Web.Controllers
{
    [AuthorizeAnyRole(RoleNames.CHHCAdmin, RoleNames.Patient_Edit)]
    public class PatientsController : Controller
    {
        public ActionResult All(string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Patients;
            ViewBag.StatusMessage = message;
            List<ViewPatient> viewPatients;
            using (var context = new DomainContext())
            {
                var patients = context.Patients.ToList();
                viewPatients = Mapper.Map<List<ViewPatient>>(patients);
            }
            return View(viewPatients);
        }

        public ActionResult Create()
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Patients;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewPatient model)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Patients;
            if (ModelState.IsValid)
            {
                var isTitleValid = PatientHelper.NewPatientValidate(model.Name);
                if (!isTitleValid)
                {
                    ModelState.AddModelError("", "There is a patient with such a name.");
                    return View(model);
                }

                try
                {
                    using (var context = new DomainContext())
                    {
                        var patient = new Patient
                        {
                            Name = model.Name,
                            InnerChhcId = model.InnerChhcId
                        };

                        context.Patients.Add(patient);
                        context.Entry(patient).State = EntityState.Added;
                        context.SaveChanges();
                    }

                    return RedirectToAction("All", new { Message = "Patient '" + model.Name + "' has been created." });
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
            ViewBag.ActiveManuItem = AccountMenuItems.Patients;
            ViewBag.StatusMessage = message;
            using (var context = new DomainContext())
            {
                var patient = context.Patients.First(x => x.Id == id);
                var model = Mapper.Map<ViewPatient>(patient);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewPatient model)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Patients;
            if (ModelState.IsValid)
            {
                var isTitleValid = PatientHelper.ExistingPatientValidate(model.Id, model.Name);
                if (!isTitleValid)
                {
                    ModelState.AddModelError("", "There is a patient with such a name.");
                    return View(model);
                }

                try
                {
                    using (var context = new DomainContext())
                    {
                        var patient = context.Patients.First(x => x.Id == model.Id);

                        patient.Name = model.Name;
                        patient.InnerChhcId = model.InnerChhcId;
                        context.SaveChanges();
                    }

                    return RedirectToAction("Edit", new { model.Id, Message = "Patient has been changed." });
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
                    var patient = new Patient { Id = id };
                    context.Patients.Attach(patient);
                    context.Patients.Remove(patient);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("All", new { Message = "Patient can not be deleted. " + e.Message });
            }

            return RedirectToAction("All", new { Message = "Patient has been deleted." });
        }


        public ActionResult Employees(int id, string patientName, string message)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Patients;
            ViewBag.StatusMessage = message;

            List<int> userIds;
            using (var context = new DomainContext())
            {
                userIds = context.UserSettingsPatient
                    .Where(x => x.Patient.Id == id && x.DateUnassigned == null)
                    .Select(x => x.UserSettings.MembershipUserId)
                    .ToList();
            }

            IEnumerable<UserProfile> allUsers;
            using (var context = new UsersContext())
            {
                var userService = new UserService();
                var users = context.UserProfiles.Where(x => !x.IsDeleted);
                var filteredUsers = userService.ApplyAdminFilter(users);
                allUsers = filteredUsers.ToList();
            }

            var model = new PatientUsers
            {
                PatientId = id,
                PatientName = patientName,
                UserIds = userIds,
                AllUsers = allUsers
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Employees(int id, string patientName, int[] userIds)
        {
            ViewBag.ActiveManuItem = AccountMenuItems.Patients;
            try
            {
                SavePatientUsers(id, userIds);

                return RedirectToAction("Employees", new { Id = id, patientName, Message = "Employees have been saved." });
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", "Employees can not be saved. " + exception.Message);
            }

            IEnumerable<UserProfile> allUsers;
            using (var context = new UsersContext())
            {
                var userService = new UserService();
                var users = context.UserProfiles.Where(x => !x.IsDeleted);
                var filteredUsers = userService.ApplyAdminFilter(users);
                allUsers = filteredUsers.ToList();
            }

            var model = new PatientUsers
            {
                PatientId = id,
                PatientName = patientName,
                UserIds = userIds == null ? new List<int>() : userIds.ToList(),
                AllUsers = allUsers
            };

            return View(model);
        }


        private void SavePatientUsers(int id, int[] userIds)
        {
            bool usersListNotEmpty = userIds != null && userIds.Length > 0;

            using (var context = new DomainContext())
            {
                if (usersListNotEmpty)
                {
                    context.UserSettingsPatient
                        .Where(x => x.Patient.Id == id && !userIds.Contains(x.UserSettings.MembershipUserId))
                        .ToList()
                        .ForEach(x => x.DateUnassigned = DateTime.Today);
                    context.SaveChanges();
                }
                else
                {
                    context.UserSettingsPatient
                        .Where(x => x.Patient.Id == id)
                        .ToList()
                        .ForEach(x => x.DateUnassigned = DateTime.Today);
                    context.SaveChanges();
                }

                if (usersListNotEmpty)
                {
                    var existingUserIds = context.UserSettingsPatient
                        .Where(x => x.Patient.Id == id)
                        .Select(u => u.UserSettings.MembershipUserId)
                        .ToList();

                    context.UserSettingsPatient
                        .Where(x => userIds.Contains(x.Id))
                        .ToList()
                        .ForEach(x => x.DateUnassigned = null);
                    context.SaveChanges();

                    List<UserProfile> allUsers;
                    using (var userContext = new UsersContext())
                    {
                        var userService = new UserService();
                        var users = userContext.UserProfiles.Where(x => !x.IsDeleted);
                        var filteredUsers = userService.ApplyAdminFilter(users);
                        allUsers = filteredUsers.ToList();
                    }

                    var usersToAdd = allUsers
                        .Where(x => userIds.Contains(x.UserId) && !existingUserIds.Contains(x.UserId))
                        .ToList();

                    var patient = new Patient { Id = id };
                    context.Patients.Attach(patient);

                    var userSettingsList = context.UserSettings.Where(x => userIds.Contains(x.MembershipUserId)).ToList();
                    foreach (var user in usersToAdd)
                    {
                        UserSettings userSettings = userSettingsList.SingleOrDefault(x => x.MembershipUserId == user.UserId);
                        if (userSettings == default(UserSettings))
                        {
                            userSettings = new UserSettings
                            {
                                MembershipUserId = user.UserId
                            };
                            context.UserSettings.Add(userSettings);
                        }

                        context.UserSettingsPatient.Add(
                            new UserSettingsPatient
                            {
                                UserSettings = userSettings,
                                Patient = patient,
                                DateAssigned = DateTime.Today,
                                DateUnassigned = null
                            }
                        );
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
