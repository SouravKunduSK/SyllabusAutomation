using SyllabusAutomation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace SyllabusAutomation.Controllers.Admin
{
    public class Uni_MissionController : Controller
    {
        private SyllabusAutomationEntities db = new SyllabusAutomationEntities();

        // GET: Uni_Mission
        public ActionResult UniversityMissions()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var university = db.Universities.Find(user.UniversityId);
            Session["uniId"] = university.UniversityId;
            var missions = db.MissionOfUniversities.Where(x => x.UniversityId == university.UniversityId && x.IsActive == true).ToList();
            var tuple = new Tuple<MissionOfUniversity, List<MissionOfUniversity>>(new MissionOfUniversity(), missions);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUniversityMissions(FormCollection form )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mission = new MissionOfUniversity();
                    mission.UniversityId = (int)Session["uniId"];
                    mission.Mission = form["Item1.Mission"];
                    mission.IsActive = true;
                    db.MissionOfUniversities.AddOrUpdate(mission);
                    db.SaveChanges();
                    TempData["msg"] = "Mission Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("UniversityMissions");
        }

        [HttpGet]
        public ActionResult UpdateUniversityMissions(int id)
        {
            var mission = db.MissionOfUniversities.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }

            var missions = db.MissionOfUniversities.Where(x => x.UniversityId == mission.UniversityId && x.IsActive == true).ToList();
            var tuple = new Tuple<MissionOfUniversity, List<MissionOfUniversity>>(mission, missions);
            ViewBag.data = true;
            return View("UniversityMissions", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUniversityMissions(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.MissionOfUniversities.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.Mission = form["Item1.Mission"];
                    db.SaveChanges();
                    TempData["msg"] = "Mission Updated Successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Something Error Occurred! Try Again... " + ex.Message;
            }

            return RedirectToAction("UniversityMissions");
        }
    }
}
