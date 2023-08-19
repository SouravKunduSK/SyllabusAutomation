using Newtonsoft.Json.Linq;
using SyllabusAutomation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace SyllabusAutomation.Controllers.Teacher
{
    public class ProgramController : Controller
    {
        SyllabusAutomationEntities db = new SyllabusAutomationEntities();
        // GET: Program
        #region PEOS
        public ActionResult PEOList(int id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            Session["pid"] = id;
            var peos = db.PEOs.Where(x => x.ProgramId == id && x.DepartmentId == user.DepartmentId && x.IsActive == true).ToList();
            var tuple = new Tuple<PEO, List<PEO>>(new PEO(), peos);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPEO(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                if (ModelState.IsValid)
                {
                    var peo = new PEO();
                    peo.UniversityId = user.UniversityId;
                    peo.FacultyId = user.FacultyId;
                    peo.DepartmentId = user.DepartmentId;
                    peo.ProgramId = (int)Session["pid"];
                    peo.PEO1 = form["Item1.PEO1"];
                    peo.IsActive = true;
                    db.PEOs.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "Program Education Objectives (PEOs) Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("PEOList", new RouteValueDictionary(new { Controller = "Program", Action = "PEOList", id = (int)Session["pid"] }));
            
        }

        [HttpGet]
        public ActionResult UpdatePEO(int id)
        {
            var mission = db.PEOs.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }

            var missions = db.PEOs.Where(x => x.DepartmentId == mission.DepartmentId && x.ProgramId == mission.ProgramId && x.IsActive == true).ToList();
            var tuple = new Tuple<PEO, List<PEO>>(mission, missions);
            ViewBag.data = true;
            return View("PEOList", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePEO(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.PEOs.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.PEO1 = form["Item1.PEO1"];
                    db.SaveChanges();
                    TempData["msg"] = "Program Education Objectives (PEOs) Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("PEOList", new RouteValueDictionary(new { Controller = "Program", Action = "PEOList", id = (int)Session["pid"] }));
        }
        #endregion

        #region PLOs
        public ActionResult PLOList(int id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            Session["pid"] = id;
            var plos = db.PLOes.Where(x => x.ProgramId == id && x.DepartmentId == user.DepartmentId && x.IsActive == true).ToList();
            var tuple = new Tuple<PLO, List<PLO>>(new PLO(), plos);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPLO(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                if (ModelState.IsValid)
                {
                    var peo = new PLO();
                    peo.UniversityId = user.UniversityId;
                    peo.FacultyId = user.FacultyId;
                    peo.DepartmentId = user.DepartmentId;
                    peo.ProgramId = (int)Session["pid"];
                    peo.PLO1 = form["Item1.PLO1"];
                    peo.IsActive = true;
                    db.PLOes.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "Program Learning Outcome (PLO) Added Successfully!";
                }
            }
            catch            
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("PLOList", new RouteValueDictionary(new { Controller = "Program", Action = "PLOList", id = (int)Session["pid"] }));

        }

        [HttpGet]
        public ActionResult UpdatePLO(int id)
        {
            var mission = db.PLOes.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }

            var missions = db.PLOes.Where(x => x.DepartmentId == mission.DepartmentId && x.ProgramId == mission.ProgramId && x.IsActive == true).ToList();
            var tuple = new Tuple<PLO, List<PLO>>(mission, missions);
            ViewBag.data = true;
            return View("PLOList", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePLO(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.PLOes.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.PLO1= form["Item1.PLO1"];
                    db.SaveChanges();
                    TempData["msg"] = "Program Learning Outcome (PLO) Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("PLOList", new RouteValueDictionary(new { Controller = "Program", Action = "PLOList", id = (int)Session["pid"] }));
        }

        #endregion

        #region Generic Skill

        public ActionResult GSList(int id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            Session["pid"] = id;
            var plos = db.GenericSkills.Where(x => x.ProgramId == id && x.DepartmentId == user.DepartmentId && x.IsActive == true).ToList();
            var tuple = new Tuple<GenericSkill, List<GenericSkill>>(new GenericSkill(), plos);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGS(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                if (ModelState.IsValid)
                {
                    var peo = new GenericSkill();
                    peo.UniversityId = user.UniversityId;
                    peo.FacultyId = user.FacultyId;
                    peo.DepartmentId = user.DepartmentId;
                    peo.ProgramId = (int)Session["pid"];
                    peo.GS = form["Item1.GS"];
                    peo.IsActive = true;
                    db.GenericSkills.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "Generic Skill Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("GSList", new RouteValueDictionary(new { Controller = "Program", Action = "GSList", id = (int)Session["pid"] }));

        }

        [HttpGet]
        public ActionResult UpdateGS(int id)
        {
            var mission = db.GenericSkills.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }

            var missions = db.GenericSkills.Where(x => x.DepartmentId == mission.DepartmentId && x.ProgramId == mission.ProgramId && x.IsActive == true).ToList();
            var tuple = new Tuple<GenericSkill, List<GenericSkill>>(mission, missions);
            ViewBag.data = true;
            return View("GSList", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGS(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.GenericSkills.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.GS = form["Item1.GS"];
                    db.SaveChanges();
                    TempData["msg"] = "Generic Skill Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("GSList", new RouteValueDictionary(new { Controller = "Program", Action = "GSList", id = (int)Session["pid"] }));
        }

        #endregion

        #region PEO To Mission mapping
        // PEO To Mission
        public ActionResult PEOToMission(int? id)
        {
            Session["pid"] = id;
            var peoMissionViewModels = GetPEOMissionViewModels(id);// Implement this method to populate view models
          
            return View(peoMissionViewModels);
        }

        [HttpPost]
        
        public ActionResult AddPEOToMission(List<PEOToMission> mappings)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            int programId = Convert.ToInt32(Session["pid"]);
                  
                        try
                        {


                            foreach (var mapping in mappings)
                            {
                            var existingMapping = db.PEOToMissions.FirstOrDefault(
                                pm => pm.PEOId == mapping.PEOId && pm.MissionId == mapping.MissionId 
                                && pm.DepartmentId == user.DepartmentId && pm.ProgramId == programId);

                            if (existingMapping != null)
                            {
                                existingMapping.Value = mapping.Value;
                            }
                            else
                            {
                                // Handle adding new mappings if necessary
                                db.PEOToMissions.Add(new PEOToMission
                                {
                                    PEOId = mapping.PEOId,
                                    MissionId = mapping.MissionId,
                                    Value = mapping.Value,
                                    DepartmentId = user.DepartmentId,
                                    ProgramId = programId
                                });
                            }
                        }

                            db.SaveChanges();
                            TempData["msg"] = "Mappings saved successfully.";
                            return Json(new { success = true, message = "Mappings saved successfully." });


            }
                        catch (Exception ex)
                        {
                            TempData["msg"] = "An error occurred while saving mappings." + ex;
                            return Json(new { success = false, message = "An error occurred while saving mappings: " + ex.Message });
            }
        }
        private List<PEOToMission> GetPEOMissionViewModels(int? pid)
        {
            // Implement this method to fetch PEOs and Missions data and create view models
            List<PEOToMission> peoMissionViewModels = new List<PEOToMission>();

            // Example logic:
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var programId = pid;
            var peos = db.PEOs.Where(x => x.ProgramId == programId && x.IsActive == true).ToList();
            var missions = db.MissionOfDepartments.Where(x => x.DepartmentId == user.DepartmentId && x.IsActive == true).ToList();

            foreach (var peo in peos)
            {
                foreach (var mission in missions)
                {
                    var value = 0;
                    var peotomissions = db.PEOToMissions.Where(x => x.PEOId == peo.PEOId && x.MissionId == mission.MissionId && x.DepartmentId == user.DepartmentId && x.ProgramId == programId).FirstOrDefault();
                    if (peotomissions != null)
                    {
                        value = Convert.ToInt32(peotomissions.Value);
                    }

                    peoMissionViewModels.Add(new PEOToMission
                    {

                        PEOId = peo.PEOId,
                        MissionId = mission.MissionId,
                        Value = value,
                        DepartmentId = user.DepartmentId,
                        ProgramId = programId

                    }); ;
                }
            }
            

           
            return peoMissionViewModels;
        }
        #endregion



        #region PEO to PLO mapping
        //PEO to PLO

        public ActionResult PEOToPLO(int? id)
        {
            Session["pid"] = id;
            var peoPloViewModels = GetPEOToPLOViewModels(id);// Implement this method to populate view models

            return View(peoPloViewModels);
        }

        [HttpPost]

        public ActionResult AddPEOToPLO(List<PEOToPLO> mappings)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            int programId = Convert.ToInt32(Session["pid"]);

            try
            {


                foreach (var mapping in mappings)
                {
                    var existingMapping = db.PEOToPLOes.FirstOrDefault(
                        pm => pm.PEOId == mapping.PEOId && pm.PLOId == mapping.PLOId
                        && pm.DepartmentId == user.DepartmentId && pm.ProgramId == programId);

                    if (existingMapping != null)
                    {
                        existingMapping.Value = mapping.Value;
                    }
                    else
                    {
                        // Handle adding new mappings if necessary
                        db.PEOToPLOes.Add(new PEOToPLO
                        {
                            PEOId = mapping.PEOId,
                            PLOId = mapping.PLOId,
                            Value = mapping.Value,
                            DepartmentId = user.DepartmentId,
                            ProgramId = programId
                        });
                    }
                }

                db.SaveChanges();
                TempData["msg"] = "Mappings saved successfully.";
                return Json(new { success = true, message = "Mappings saved successfully." });


            }
            catch (Exception ex)
            {
                TempData["msg"] = "An error occurred while saving mappings." + ex;
                return Json(new { success = false, message = "An error occurred while saving mappings: " + ex.Message });
            }
        }
        private List<PEOToPLO> GetPEOToPLOViewModels(int? pid)
        {
            // Implement this method to fetch PEOs and Missions data and create view models
            List<PEOToPLO> peoPloViewModels = new List<PEOToPLO>();

            // Example logic:
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var programId = pid;
            var peos = db.PEOs.Where(x => x.ProgramId == programId && x.IsActive == true).ToList();
            var plos = db.PLOes.Where(x => x.DepartmentId == user.DepartmentId && x.ProgramId == programId && x.IsActive == true).ToList();

            foreach (var peo in peos)
            {
                foreach (var plo in plos)
                {
                    var value = 0;
                    var peotoplos = db.PEOToPLOes.Where(x => x.PEOId == peo.PEOId && x.PLOId == plo.PLOId 
                    && x.DepartmentId == user.DepartmentId && x.ProgramId == programId).FirstOrDefault();
                    if (peotoplos != null)
                    {
                        value = Convert.ToInt32(peotoplos.Value);
                    }

                    peoPloViewModels.Add(new PEOToPLO
                    {

                        PEOId = peo.PEOId,
                        PLOId = plo.PLOId,
                        Value = value,
                        DepartmentId = user.DepartmentId,
                        ProgramId = programId

                    }); ;
                }
            }



            return peoPloViewModels;
        }

        #endregion




        #region TeachingStrategies
        public ActionResult TeachingStrategies()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var plos = db.TeachingStrategies.Where(x => x.DepartmentId == user.DepartmentId && x.IsActive == true).ToList();
            var tuple = new Tuple<TeachingStrategie, List<TeachingStrategie>>(new TeachingStrategie(), plos);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStrategies(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                if (ModelState.IsValid)
                {
                    var peo = new TeachingStrategie();
                    peo.UniversityId = user.UniversityId;
                    peo.FacultyId = user.FacultyId;
                    peo.DepartmentId = user.DepartmentId;
                    peo.IsActive = true;
                    peo.Strategies = form["Item1.Strategies"];
                    db.TeachingStrategies.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "Strategie Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("TeachingStrategies","Program");

        }

        [HttpGet]
        public ActionResult UpdateStrategie(int id)
        {
            var mission = db.TeachingStrategies.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }

            var missions = db.TeachingStrategies.Where(x => x.DepartmentId == mission.DepartmentId && x.IsActive == true).ToList();
            var tuple = new Tuple<TeachingStrategie, List<TeachingStrategie>>(mission, missions);
            ViewBag.data = true;
            return View("TeachingStrategies", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStrategie(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.TeachingStrategies.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.Strategies = form["Item1.Strategies"];
                    db.SaveChanges();
                    TempData["msg"] = "Strategie Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("TeachingStrategies", "Program");
        }

        #endregion



        #region Grading
        public ActionResult Grading(int? id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            Session["pid"] = id;
            var grading = db.Grades.Where(x => x.ProgramId == id && x.UniversityId == user.UniversityId).ToList();
            var tuple = new Tuple<Grade, List<Grade>>(new Grade(), grading);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGrading(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                var programId = Convert.ToInt32(Session["pid"]);
                if (ModelState.IsValid)
                {
                    var grade = new Grade();
                   grade.ProgramId = programId;
                    grade.UniversityId = user.UniversityId;
                    grade.Numerical = form["Item1.Numerical"].AsFloat();
                    grade.Point = form["Item1.Point"];
                    grade.Letter = form["Item1.Letter"];
                    db.Grades.AddOrUpdate(grade);
                    db.SaveChanges();
                    TempData["msg"] = "Grade Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("Grading", "Program");

        }

        [HttpGet]
        public ActionResult UpdateGrading(int ?id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var programId = Convert.ToInt32(Session["pid"]);
            var grade = db.Grades.Find(id);
            if (grade == null)
            {
                return HttpNotFound();
            }

            var grading = db.Grades.Where(x => x.ProgramId == programId && x.UniversityId == user.UniversityId).ToList();
            var tuple = new Tuple<Grade, List<Grade>>(grade, grading);
            ViewBag.data = true;
            return View("Grading", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGrading(int ?id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.Grades.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.Numerical = form["Item1.Numerical"].AsFloat();
                    existingMission.Point = form["Item1.Point"];
                    existingMission.Letter = form["Item1.Letter"];
                    db.SaveChanges();
                    TempData["msg"] = "Grade Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("Grading", "Program");
        }

        #endregion


        public ActionResult SelectProgram()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var programs = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).OrderBy(x=>x.ShortName).ToList();
            ViewBag.ProgramList = new SelectList(programs, "ProgramId", "ShortName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectProgram(FormCollection frm)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var programs = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).OrderBy(x => x.ShortName).ToList();
            ViewBag.ProgramList = new SelectList(programs, "ProgramId", "ShortName");
            Session["programId"] = frm["ProgramId"];
            TempData["msg"] = "Program is selected";
            return RedirectToAction("CourseList", "Course");
        }
    }
}