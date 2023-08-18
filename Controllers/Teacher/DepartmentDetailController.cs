using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using SyllabusAutomation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SyllabusAutomation.Controllers.Teacher
{
    [Authorize]
    public class DepartmentDetailController : Controller
    {
        SyllabusAutomationEntities db = new SyllabusAutomationEntities();
        // GET: Department
        #region Program
        public ActionResult ProgramList()
        {
            int uid = (int)Session["uid"];
            var user = GetUser(uid);
            var university = db.Universities.Find(user.UniversityId);
            Session["uniId"] = university.UniversityId;
            var programs = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).OrderBy(x => x.ShortName).ToList();
            var tuple = new Tuple<Program, List<Program>>(new Program(), programs);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewProgram(FormCollection frm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int uid = (int)Session["uid"];
                    var user = GetUser(uid);
                    var program = new Program();
                    program.UniversityId = user.UniversityId;
                    program.DepartmentId = user.DepartmentId;
                    program.FacultyId = user.FacultyId;
                    program.ProgramName = frm["Item1.ProgramName"].ToString();
                    program.ShortName = frm["Item1.ShortName"].ToString();
                    program.Duration = frm["Item1.Duration"].ToString();
                    program.MinimumCreditRequirment = frm["Item1.MinimumCreditRequirment"].ToString();
                    program.CourseDistribution = frm["Item1.CourseDistribution"].ToString();
                    program.GradingPolicy = frm["Item1.GradingPolicy"].ToString();
                    program.CourseWithdrawl = frm["Item1.CourseWithdrawl"].ToString();
                    program.WithdrawlFromSemester = frm["Item1.WithdrawlFromSemester"].ToString();
                    program.Retake = frm["Item1.Retake"].ToString();
                    program.SpecialSemester = frm["Item1.SpecialSemester"].ToString();
                    db.Programs.AddOrUpdate(program);
                    db.SaveChanges();
                    TempData["msg"] = "Program Added Successfully!";
                }
            }
            catch 
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("ProgramList", "DepartmentDetail");
        }

        public ActionResult UpdateProgram(int id)
        {
            int uid = (int)Session["uid"];
            var user = GetUser(uid);
            Session["prgm"] = id;
            var prgm = db.Programs.FirstOrDefault(x=>x.ProgramId == id);
            if (prgm == null)
            {
                return HttpNotFound();
            }
            var programs = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).OrderBy(x => x.ShortName).ToList();
            var tuple = new Tuple<Program, List<Program>>(prgm, programs);
         
            ViewBag.data = true;
            return View("ProgramList", tuple);



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProgram(int id, FormCollection frm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                     var program = db.Programs.Find(id);
                    if (program == null)
                    {
                        return HttpNotFound();
                    }
                    program.ProgramName = frm["Item1.ProgramName"].ToString();
                    program.ShortName = frm["Item1.ShortName"].ToString();
                    program.Duration = frm["Item1.Duration"].ToString();
                    program.MinimumCreditRequirment = frm["Item1.MinimumCreditRequirment"].ToString();
                    program.CourseDistribution = frm["Item1.CourseDistribution"].ToString();
                    program.GradingPolicy = frm["Item1.GradingPolicy"].ToString();
                    program.CourseWithdrawl = frm["Item1.CourseWithdrawl"].ToString();
                    program.WithdrawlFromSemester = frm["Item1.WithdrawlFromSemester"].ToString();
                    program.Retake = frm["Item1.Retake"].ToString();
                    program.SpecialSemester = frm["Item1.SpecialSemester"].ToString();
                    db.SaveChanges();
                    
                    TempData["msg"] = "Program Updated Successfully!";
                }
            }
            catch  
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("ProgramList", "DepartmentDetail");
        }
        public ActionResult Detail(int id)
        {
            var detail = db.Programs.Find(id);
            return View(detail);
        }
        #endregion


        #region Session
        public ActionResult SessionList()
        {
            int uid = (int)Session["uid"];
            var user = GetUser(uid);
            var sessions = db.Sessions.Where(x => x.DepartmentId == user.DepartmentId && x.IsActive == true).OrderByDescending(x=>x.SessionId).ToList();
            var tuple = new Tuple<Session, List<Session>>(new Session(), sessions);
            return View(tuple);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewSession(FormCollection frm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int uid = (int)Session["uid"];
                    var user = GetUser(uid);
                    var session = new Session();

                    session.DepartmentId = (int)user.DepartmentId;
                    session.IsActive = true;
                    
                    session.SessionName = frm["Item1.SessionName"].ToString();
                    
                    db.Sessions.AddOrUpdate(session);
                    db.SaveChanges();
                    TempData["msg"] = "Session Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("SessionList", "DepartmentDetail");
        }

        public ActionResult UpdateSession(int id)
        {
            int uid = (int)Session["uid"];
            var user = GetUser(uid);
            var session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            var sessions = db.Sessions.Where(x => x.DepartmentId == user.DepartmentId && x.IsActive == true).OrderByDescending(x=>x.SessionId).ToList();
            var tuple = new Tuple<Session, List<Session>>(session, sessions);

            ViewBag.data = true;
            return View("SessionList", tuple);



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSession(int id, FormCollection frm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var session = db.Sessions.Find(id);
                    if (session == null)
                    {
                        return HttpNotFound();
                    }
                    session.SessionName = frm["Item1.SessionName"].ToString();
                    
                    db.SaveChanges();

                    TempData["msg"] = "Session Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("SessionList", "DepartmentDetail");
        }
        #endregion
        #region Years
        //public ActionResult YearList()
        //{
        //    int uid = (int)Session["uid"];
        //    var user = GetUser(uid);
        //    var years = db.EduYears.Where(x => x.DepartmentId == user.DepartmentId).OrderByDescending(x => x.YearName).ToList();
        //    var tuple = new Tuple<EduYear, List<EduYear>>(new EduYear(), years);
        //    return View(tuple);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult AddNewYear(FormCollection frm)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            int uid = (int)Session["uid"];
        //            var user = GetUser(uid);
        //            var year = new EduYear();

        //            year.DepartmentId = (int)user.DepartmentId;

        //            year.YearName = frm["Item1.YearName"].ToString();

        //            db.EduYears.AddOrUpdate(year);
        //            db.SaveChanges();
        //            TempData["msg"] = "Year Name Added Successfully!";
        //        }
        //    }
        //    catch
        //    {
        //        TempData["msg"] = "Something Error Occurred! Try Again... ";
        //    }

        //    return RedirectToAction("YearList", "DepartmentDetail");
        //}

        //public ActionResult UpdateYear(int id)
        //{
        //    int uid = (int)Session["uid"];
        //    var user = GetUser(uid);
        //    var year = db.EduYears.FirstOrDefault(x => x.YearId == id);
        //    if (year == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var years = db.EduYears.Where(x => x.DepartmentId == user.DepartmentId).OrderBy(x => x.YearName).ToList();
        //    var tuple = new Tuple<EduYear, List<EduYear>>(year, years);

        //    ViewBag.data = true;
        //    return View("YearList", tuple);



        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UpdateYear(int id, FormCollection frm)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var year = db.EduYears.Find(id);
        //            if (year == null)
        //            {
        //                return HttpNotFound();
        //            }
        //            year.YearName = frm["Item1.YearName"].ToString();

        //            db.SaveChanges();

        //            TempData["msg"] = "Year Name Updated Successfully!";
        //        }
        //    }
        //    catch
        //    {
        //        TempData["msg"] = "Something Error Occurred! Try Again... ";
        //    }

        //    return RedirectToAction("YearList", "DepartmentDetail");
        //}
        #endregion
        public ActionResult SemesterList()
        {
            var sem = db.Semesters.ToList();
            return View(sem);
        }
        private User GetUser(int Id)
        {
            User user = db.Users
                .Where(c => c.UserID == Id).FirstOrDefault();
            return user;
        }

        public ActionResult DeptVision()
        {
            int uid = (int)Session["uid"];
            var user = GetUser(uid);
            var dept = db.Departments.Find(user.DepartmentId);
            return View(dept);
        }

        public ActionResult UpdateDeptDetail(int id)
        {
            var dept = db.Departments.Find(id);
            return View(dept);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDeptDetail(int id, Department dept)
        {
            try
            {
                var existing = db.Departments.Find(id);
                if (existing == null)
                {
                    return HttpNotFound();
                }
                existing.Vision = dept.Vision;
                existing.Introduction = dept.Introduction;
                existing.MessageofChairman = dept.MessageofChairman;
                existing.Mobile = dept.Mobile;
                existing.Fax = dept.Fax;
                existing.Phone = dept.Phone;
                existing.Email = dept.Email;
                existing.Website = dept.Website;
                db.SaveChanges();
                TempData["message"] = "Vision Updated Sucessfully!";
            }
            catch
            {
                TempData["message"] = "Something Error Occurred! Try again...";
            }
            
            return RedirectToAction("DeptVision", "DepartmentDetail");
        }

        public ActionResult DeptMissions()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var dept = db.Departments.Where(x=>x.DepartmentId==user.DepartmentId && x.IsActive == true).FirstOrDefault();
            Session["deptId"] = dept.DepartmentId;
            var missions = db.MissionOfDepartments.Where(x => x.DepartmentId == dept.DepartmentId && x.IsActive == true).ToList();
            var tuple = new Tuple<MissionOfDepartment, List<MissionOfDepartment>>(new MissionOfDepartment(), missions);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDeptMissions(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                if (ModelState.IsValid)
                {
                    var mission = new MissionOfDepartment();
                    mission.UniversityId = user.UniversityId;
                    mission.FacultyId = user.FacultyId;
                    mission.DepartmentId = user.DepartmentId;
                    mission.IsActive = true;
                    mission.Mission = form["Item1.Mission"];
                    db.MissionOfDepartments.AddOrUpdate(mission);
                    db.SaveChanges();
                    TempData["msg"] = "Mission Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("DeptMissions");
        }

        [HttpGet]
        public ActionResult UpdateDeptMissions(int id)
        {
            var mission = db.MissionOfDepartments.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }

            var missions = db.MissionOfDepartments.Where(x => x.DepartmentId == mission.DepartmentId && x.IsActive == true).ToList();
            var tuple = new Tuple<MissionOfDepartment, List<MissionOfDepartment>>(mission, missions);
            ViewBag.data = true;
            return View("DeptMissions", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDeptMissions(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.MissionOfDepartments.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.Mission = form["Item1.Mission"];
                    db.SaveChanges();
                    TempData["msg"] = "Mission Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("DeptMissions");
        }

        public ActionResult MapPEOToMission()
        {
            var peoMissionViewModels = GetPEOMissionViewModels(); // Implement this method to populate view models
            return View(peoMissionViewModels);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MapPEOToMission(List<PEOToMissionViewModel> mappings)
        {
            var x = 1;
            //if(mappings != null || mappings.Count != 0)
            //{
            while(x==1)
            {
                try
                {
                    int uid = (int)Session["uid"];
                    var user = GetUser(uid);
                    int programId = Convert.ToInt32(Session["pid"]);
                    foreach (var mapping in mappings)
                    {
                        PEOToMission peoToMission = new PEOToMission
                        {
                            PEOId = mapping.PEOId,
                            MissionId = mapping.MissionId,
                            Value = mapping.Value,
                            ProgramId = programId,
                            DepartmentId = user.DepartmentId
                        };

                        db.PEOToMissions.AddOrUpdate(peoToMission);
                    }

                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Mappings saved successfully.";
                    return RedirectToAction("MapPEOToMission");
                    
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while saving mappings." + ex;
                    return RedirectToAction("MapPEOToMission");
                }
               
            }
            x--;
            //}
                
            return View(mappings);
            
        }
        private List<PEOToMissionViewModel> GetPEOMissionViewModels()
        {
            // Implement this method to fetch PEOs and Missions data and create view models
            List<PEOToMissionViewModel> peoMissionViewModels = new List<PEOToMissionViewModel>();

            // Example logic:
            int uid = (int)Session["uid"];
            var user = GetUser(uid);
            int programId = Convert.ToInt32(Session["pid"]);
            var peos = db.PEOs.Where(x=>x.ProgramId==programId).ToList();
            var missions = db.MissionOfDepartments.Where(x=>x.DepartmentId == user.DepartmentId).ToList();

            foreach (var peo in peos)
            {
                foreach (var mission in missions)
                {
                    var value = 0;
                    var peotomissions = db.PEOToMissions.Where(x => x.PEOId == peo.PEOId && x.MissionId == mission.MissionId).FirstOrDefault();
                    if (peotomissions != null)
                    {
                        value = Convert.ToInt32(peotomissions.Value);
                    }
                   
                    peoMissionViewModels.Add(new PEOToMissionViewModel
                    {
                        
                        PEOId = peo.PEOId,
                        MissionId = mission.MissionId,
                        Value = value
                        
                    }); ;
                }
            }

            return peoMissionViewModels;
        }



        // Teacher's Designation

        public ActionResult Designations()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var designations = db.TeacherDesignations.Where(x => x.DeptId == user.DepartmentId && x.IsActive == true).ToList();
            var tuple = new Tuple<TeacherDesignation, List<TeacherDesignation>>(new TeacherDesignation(), designations);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDesignations(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                if (ModelState.IsValid)
                {
                    var mission = new TeacherDesignation();
                    mission.DeptId = user.DepartmentId;
                    mission.IsActive = true;
                    mission.Designation = form["Item1.Designation"];
                    db.TeacherDesignations.AddOrUpdate(mission);
                    db.SaveChanges();
                    TempData["msg"] = "Designation Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("Designations");
        }

        [HttpGet]
        public ActionResult UpdateDesignations(int id)
        {
            var mission = db.TeacherDesignations.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }

            var missions = db.TeacherDesignations.Where(x => x.DeptId == mission.DeptId && x.IsActive == true).ToList();
            var tuple = new Tuple<TeacherDesignation, List<TeacherDesignation>>(mission, missions);
            ViewBag.data = true;
            return View("Designations", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDesignations(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.TeacherDesignations.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.Designation = form["Item1.Designation"];
                    db.SaveChanges();
                    TempData["msg"] = "Designation Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("Designations");
        }
    }
}