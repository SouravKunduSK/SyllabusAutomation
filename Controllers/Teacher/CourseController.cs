using SyllabusAutomation.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace SyllabusAutomation.Controllers.Teacher
{
    public class CourseController : Controller
    {
        SyllabusAutomationEntities db = new SyllabusAutomationEntities();
        // GET: Course
        
        public ActionResult CourseSection()
        {
            return View();
        }
        #region Coursetype
        public ActionResult CourseType()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var pid = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            ViewBag.ProgramList = new SelectList(pid, "ProgramId", "ShortName");
            var types = db.CourseTypes.Where(x =>x.DepartmentId == user.DepartmentId).ToList();
            var tuple = new Tuple<CourseType, List<CourseType>>(new CourseType(), types);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCourseType(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                var pid = Convert.ToInt32(form["Item1.ProgramId"]);
                if (ModelState.IsValid)
                {
                    var peo = new CourseType();
                    peo.UniversityId = user.UniversityId;
                    peo.FacultyId = user.FacultyId;
                    peo.DepartmentId = user.DepartmentId;
                    peo.ProgramId = pid;
                    peo.CourseType1 = form["Item1.CourseType1"];
                    db.CourseTypes.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "Course Types Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("CourseType", "Course");

        }

        [HttpGet]
        public ActionResult UpdateCourseType(int id)
        {
            
            var mission = db.CourseTypes.Find(id);

            if (mission == null)
            {
                return HttpNotFound();
            }
            var pid = db.Programs.Where(x => x.DepartmentId == mission.DepartmentId).ToList();
            ViewBag.ProgramList = new SelectList(pid, "ProgramId", "ShortName", mission.ProgramId);
            var missions = db.CourseTypes.Where(x =>x.DepartmentId == mission.DepartmentId).ToList();
            var tuple = new Tuple<CourseType, List<CourseType>>(mission, missions);
            ViewBag.data = true;
            return View("CourseType", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCourseType(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.CourseTypes.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.CourseType1 = form["Item1.CourseType1"];
                    existingMission.ProgramId = Convert.ToInt32(form["Item1.ProgramId"]);
                    db.SaveChanges();
                    TempData["msg"] = "Course Type Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("CourseType", "Course");
        }
        #endregion


        #region Marks
        public ActionResult Marks()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var pid = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            ViewBag.ProgramId = new SelectList(pid, "ProgramId", "ShortName");
            var plos = db.Marks.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            var tuple = new Tuple<Mark, List<Mark>>(new Mark(), plos);
            return View(tuple);
        }
        public ActionResult GetCourseTypessByProgram(int programId)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var courseTypes = new[] { new {
                CourseTypeId = 0,
                CourseType1 = ""
            } }.ToList();
            courseTypes = db.CourseTypes.Where(d => d.ProgramId == programId && d.DepartmentId == user.DepartmentId).Select(d => new {
                CourseTypeId = d.CourseTypeId,
                CourseType1 = d.CourseType1
            }).OrderBy(x => x.CourseType1).ToList();
            var defItem = new[]{
                    new
                {
                    CourseTypeId = 0,
                    CourseType1 = "--- Select Course Type ---"
                }
                };

            courseTypes.InsertRange(0, defItem);
            return Json(courseTypes, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMarks(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                
                if (ModelState.IsValid)
                {
                    var peo = new Mark();
                    peo.UniversityId = user.UniversityId;
                    peo.FacultyId = user.FacultyId;
                    peo.DepartmentId = user.DepartmentId;
                    peo.ProgramId = Convert.ToInt32(form["ProgramId"]);
                    peo.CourseTypeId =Convert.ToInt32( form["CourseTypeId"]) ;
                    peo.CT = form["Item1.CT"].AsFloat();
                    peo.Attendence = form["Item1.Attendence"].AsFloat();
                    peo.SE = form["Item1.SE"].AsFloat();
                    peo.SEE = form["Item1.SEE"].AsFloat();
                    peo.Total = (peo.CT+peo.Attendence+peo.SE+peo.SEE);
                    db.Marks.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "Marks Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("Marks", "Course");

        }

        [HttpGet]
        public ActionResult UpdateMarks(int id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var mission = db.Marks.Find(id);

            if (mission == null)
            {
                return HttpNotFound();
            }
            var pid = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            ViewBag.ProgramId = new SelectList(pid, "ProgramId", "ShortName",mission.ProgramId);
            // Populate Course Types for the selected Program
            var courseTypes = db.CourseTypes
                .Where(ct => ct.ProgramId == mission.ProgramId && ct.DepartmentId == mission.DepartmentId)
                .ToList();
            ViewBag.CourseTypeId = new SelectList(courseTypes, "CourseTypeId", "CourseType1", mission.CourseTypeId);
            var missions = db.Marks.Where(x => x.ProgramId == mission.ProgramId && x.DepartmentId == mission.DepartmentId).ToList();
            var tuple = new Tuple<Mark, List<Mark>>(mission, missions);
            ViewBag.data = true;
            return View("Marks", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMarks(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var peo = db.Marks.Find(id);
                    if (peo == null)
                    {
                        return HttpNotFound();
                    }
                    peo.ProgramId = Convert.ToInt32(form["ProgramId"]);
                    peo.CourseTypeId = Convert.ToInt32(form["CourseTypeId"]);
                    peo.CT = form["Item1.CT"].AsFloat();
                    peo.Attendence = form["Item1.Attendence"].AsFloat();
                    peo.SE = form["Item1.SE"].AsFloat();
                    peo.SEE = form["Item1.SEE"].AsFloat();
                    peo.Total = (peo.CT + peo.Attendence + peo.SE + peo.SEE);
                    db.SaveChanges();
                    TempData["msg"] = "Marks Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("Marks", "Course");
        }
        #endregion

        #region CourseList
        public ActionResult CourseList()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var pid = Convert.ToInt32(Session["programId"]);
            ViewBag.CourseTypeList = new SelectList(db.CourseTypes.Where(x => x.ProgramId == pid && x.DepartmentId == user.DepartmentId).ToList(), "CourseTypeId", "CourseType1");
            var plos = db.Courses.Where(x => x.ProgramId == pid && x.DepartmentId == user.DepartmentId).ToList();
            var tuple = new Tuple<Course, List<Course>>(new Course(), plos);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCourseList(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                var pid = Convert.ToInt32(Session["programId"]);
                if (ModelState.IsValid)
                {
                    var peo = new Course();
                    peo.UniversityId = user.UniversityId;
                    peo.FacultyId = user.FacultyId;
                    peo.DepartmentId = user.DepartmentId;
                    peo.ProgramId = pid;
                    peo.CourseTypeId = Convert.ToInt32(form["Item1.CourseTypeId"]);
                    var mark = db.Marks.Find(peo.CourseTypeId);
                    peo.CourseCode = form["Item1.CourseCode"];
                    peo.CourseTitle = form["Item1.CourseTitle"];
                    peo.Credit = form["Item1.Credit"].AsFloat();
                    peo.CourseSummary= form["Item1.CourseSummary"];
                    peo.TotalCredit = peo.Credit;
                    peo.MarksId = mark.MarksId;
                    peo.TotalMarks = mark.Total;
                    db.Courses.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "Course Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("CourseList", "Course");

        }

        [HttpGet]
        public ActionResult UpdateCourseList(int id)
        {
            var pid = Convert.ToInt32(Session["programId"]);
            var mission = db.Courses.Find(id);

            if (mission == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseTypeList = new SelectList(db.CourseTypes.Where(x => x.ProgramId == pid && x.DepartmentId == mission.DepartmentId).ToList(), "CourseTypeId", "CourseType1", mission.CourseTypeId);
            var missions = db.Courses.Where(x => x.ProgramId == pid && x.DepartmentId == mission.DepartmentId).ToList();
            var tuple = new Tuple<Course, List<Course>>(mission, missions);
            ViewBag.data = true;
            return View("CourseList", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCourseList(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var peo = db.Courses.Find(id);
                    if (peo == null)
                    {
                        return HttpNotFound();
                    }
                    peo.CourseTypeId = Convert.ToInt32(form["Item1.CourseTypeId"]);
                    var mark = db.Marks.Find(peo.CourseTypeId);
                    peo.CourseCode = form["Item1.CourseCode"];
                    peo.CourseTitle = form["Item1.CourseTitle"];
                    peo.Credit = form["Item1.Credit"].AsFloat();
                    peo.CourseSummary = form["Item1.CourseSummary"];
                    peo.TotalCredit = peo.Credit;
                    peo.MarksId = mark.MarksId;
                    peo.TotalMarks = mark.Total;
                    db.Courses.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "Course Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("CourseList", "Course");
        }

        public ActionResult CourseDetail(int id)
        {
            var course = db.Courses.Find(id);
            ViewBag.Marks = db.Marks.Where(x=>x.CourseTypeId==course.CourseTypeId).ToList();
            return View(course);
        }


        public ActionResult CLOList(int id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var pid = Convert.ToInt32(Session["programId"]); 
            Session["cid"] = id;
            var clos = db.CLOes.Where(x => x.CourseId == id && x.ProgramId == pid).ToList();
            var tuple = new Tuple<CLO, List<CLO>>(new CLO(), clos);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCLOList(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                if (ModelState.IsValid)
                {
                    var peo = new CLO();
                 
                    peo.ProgramId = Convert.ToInt32(Session["programId"]);
                    peo.CourseId = Convert.ToInt32(Session["cid"]);
                    peo.Outcomes = form["Item1.Outcomes"];
                    db.CLOes.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "CLO Added Successfully!";
                }
            }
            catch (DbException ex)
            {
                TempData["msg"] = "Something Error Occurred! Try Again... "+ex;
            }
            return RedirectToAction("CLOList", new RouteValueDictionary(new { Controller = "Course", Action = "CLOList", id = (int)Session["cid"] }));

        }

        [HttpGet]
        public ActionResult UpdateCLOList(int id)
        {
            var mission = db.CLOes.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }

            var missions = db.CLOes.Where(x => x.CourseId == mission.CourseId && x.ProgramId == mission.ProgramId).ToList();
            var tuple = new Tuple<CLO, List<CLO>>(mission, missions);
            ViewBag.data = true;
            return View("CLOList", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCLOList(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.CLOes.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.Outcomes = form["Item1.Outcomes"];
                    db.SaveChanges();
                    TempData["msg"] = "CLOList Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("CLOList", new RouteValueDictionary(new { Controller = "Course", Action = "PEOList", id = (int)Session["cid"] }));
        }

        //CLOToPLO

        public ActionResult CLOToPLO(int? id)
        {
            Session["cid"] = id;
            var cloPloViewModels = GetCLOToPLOViewModels(id);// Implement this method to populate view models

            return View(cloPloViewModels);
        }

        [HttpPost]

        public ActionResult AddCLOToPLO(List<CLOToPLO> mappings)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            int programId = Convert.ToInt32(Session["programId"]);
            int  courseId = Convert.ToInt32(Session["cid"]);

            try
            {


                foreach (var mapping in mappings)
                {
                    var existingMapping = db.CLOToPLOes.FirstOrDefault(
                        pm => pm.CLOId == mapping.CLOId && pm.PLOId == mapping.PLOId && pm.CourseId == courseId
                        && pm.DepartmentId == user.DepartmentId && pm.ProgramId == programId);

                    if (existingMapping != null)
                    {
                        existingMapping.Value = mapping.Value;
                    }
                    else
                    {
                        // Handle adding new mappings if necessary
                        db.CLOToPLOes.Add(new CLOToPLO
                        {
                            CLOId = mapping.CLOId,
                            PLOId = mapping.PLOId,
                            Value = mapping.Value,
                            CourseId = courseId,
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
        private List<CLOToPLO> GetCLOToPLOViewModels(int? cid)
        {
            // Implement this method to fetch PEOs and Missions data and create view models
            List<CLOToPLO> cloPloViewModels = new List<CLOToPLO>();

            // Example logic:
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var programId = Convert.ToInt32(Session["programId"]);
            var courseId = cid;
            var clos = db.CLOes.Where(x => x.CourseId == cid && x.ProgramId == programId).ToList();
            
            var plos = db.PLOes.Where(x => x.DepartmentId == user.DepartmentId && x.ProgramId == programId).ToList();

            foreach (var clo in clos)
            {
                foreach (var plo in plos)
                {
                    var value = 0;
                    var clotoplos = db.CLOToPLOes.Where(x => x.CLOId == clo.CLOId && x.PLOId == plo.PLOId
                    && x.DepartmentId == user.DepartmentId && x.ProgramId == programId).FirstOrDefault();
                    if (clotoplos != null)
                    {
                        value = Convert.ToInt32(clotoplos.Value);
                    }

                    cloPloViewModels.Add(new CLOToPLO
                    {

                        CLOId = clo.CLOId,
                        PLOId = plo.PLOId,
                        Value = value,
                        CourseId = courseId,
                        DepartmentId = user.DepartmentId,
                        ProgramId = programId

                    }); ;
                }
            }



            return cloPloViewModels;
        }






        public ActionResult SEEList(int id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            Session["cid"] = id;
            var peos = db.SEEs.Where(x => x.CourseId == id && x.DepartmentId == user.DepartmentId).ToList();
            var tuple = new Tuple<SEE, List<SEE>>(new SEE(), peos);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSEEList(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                if (ModelState.IsValid)
                {
                    var peo = new SEE();
                    peo.UniversityId = user.UniversityId;
                    peo.FacultyId = user.FacultyId;
                    peo.DepartmentId = user.DepartmentId;
                    peo.CourseId = Convert.ToInt32(Session["cid"]);
                    peo.ProgramId = Convert.ToInt32(Session["programId"]);
                    peo.Category = form["Item1.Category"];
                    peo.Test = form["Item1.Test"];
                    db.SEEs.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "SEEList Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("SEEList", new RouteValueDictionary(new { Controller = "Course", Action = "SEEList", id = (int)Session["cid"] }));

        }

        [HttpGet]
        public ActionResult UpdateSEEList(int id)
        {
            var mission = db.SEEs.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }

            var missions = db.SEEs.Where(x => x.CourseId == mission.CourseId && x.ProgramId == mission.ProgramId).ToList();
            var tuple = new Tuple<SEE, List<SEE>>(mission, missions);
            ViewBag.data = true;
            return View("SEEList", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSEEList(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.SEEs.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.Category = form["Item1.Category"];
                    existingMission.Test = form["Item1.Test"];
                    db.SaveChanges();
                    TempData["msg"] = "SEEList Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("SEEList", new RouteValueDictionary(new { Controller = "Course", Action = "SEEList", id = (int)Session["cid"] }));
        }

        public ActionResult CIEList(int id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            Session["cid"] = id;
            var peos = db.CIEs.Where(x => x.CourseId == id && x.DepartmentId == user.DepartmentId).ToList();
            var tuple = new Tuple<CIE, List<CIE>>(new CIE(), peos);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCIEList(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                if (ModelState.IsValid)
                {
                    var peo = new CIE();
                    peo.UniversityId = user.UniversityId;
                    peo.FacultyId = user.FacultyId;
                    peo.DepartmentId = user.DepartmentId;
                    peo.ProgramId = Convert.ToInt32(Session["programId"]);
                    peo.CourseId = Convert.ToInt32(Session["cid"]);
                    peo.Category = form["Item1.Category"];
                    peo.Mark = form["Item1.Mark"];
                    peo.Assignment = form["Item1.Assignment"];
                    peo.Test = form["Item1.Test"];
                    peo.Quizzes = form["Item1.Quizzes"];
                    peo.CCA = form["Item1.CCA"];
                    db.CIEs.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "CIE Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("CIEList", new RouteValueDictionary(new { Controller = "Course", Action = "CIEList", id = (int)Session["cid"] }));

        }

        [HttpGet]
        public ActionResult UpdateCIEList(int id)
        {
            var mission = db.CIEs.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }

            var missions = db.CIEs.Where(x => x.CourseId == mission.CourseId && x.ProgramId == mission.ProgramId).ToList();
            var tuple = new Tuple<CIE, List<CIE>>(mission, missions);
            ViewBag.data = true;
            return View("CIEList", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCIEList(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var peo = db.CIEs.Find(id);
                    if (peo == null)
                    {
                        return HttpNotFound();
                    }
                    peo.Category = form["Item1.Category"];
                    peo.Mark = form["Item1.Mark"];
                    peo.Assignment = form["Item1.Assignment"];
                    peo.Test = form["Item1.Test"];
                    peo.Quizzes = form["Item1.Quizzes"];
                    peo.CCA = form["Item1.CCA"];
                    db.SaveChanges();
                    TempData["msg"] = "CIE Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("CIEList", new RouteValueDictionary(new { Controller = "Course", Action = "CIEList", id = (int)Session["cid"] }));
        }















        public ActionResult Objectives(int id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            Session["cid"] = id;
            var peos = db.CourseObjectives.Where(x => x.CourseId == id && x.DepartmentId == user.DepartmentId).ToList();
            var tuple = new Tuple<CourseObjective, List<CourseObjective>>(new CourseObjective(), peos);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddObjectives(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                if (ModelState.IsValid)
                {
                    var peo = new CourseObjective();
                    peo.UniversityId = user.UniversityId;
                    peo.FacultyId = user.FacultyId;
                    peo.DepartmentId = user.DepartmentId;
                    peo.ProgramId = Convert.ToInt32(Session["programId"]);
                    peo.CourseId = Convert.ToInt32(Session["cid"]);
                    peo.Objectives = form["Item1.Objectives"];
                    db.CourseObjectives.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "Objectives Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("Objectives", new RouteValueDictionary(new { Controller = "Course", Action = "Objectives", id = (int)Session["cid"] }));

        }

        [HttpGet]
        public ActionResult UpdateObjectives(int id)
        {
            var mission = db.CourseObjectives.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }

            var missions = db.CourseObjectives.Where(x => x.CourseId == mission.CourseId && x.ProgramId == mission.ProgramId).ToList();
            var tuple = new Tuple<CourseObjective, List<CourseObjective>>(mission, missions);
            ViewBag.data = true;
            return View("Objectives", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateObjectives(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.CourseObjectives.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.Objectives = form["Item1.Objectives"];
                    db.SaveChanges();
                    TempData["msg"] = "Objectives Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("Objectives", new RouteValueDictionary(new { Controller = "Course", Action = "Objectives", id = (int)Session["cid"] }));
        }




        #endregion

        public ActionResult AssesmentStrategies(int id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var pid = Convert.ToInt32(Session["programId"]);
            Session["cid"] = id;
            var plos = db.AssessmentStrategies.Where(x =>x.CourseId==id&&x.ProgramId==pid&& x.DepartmentId == user.DepartmentId).ToList();
            var tuple = new Tuple<AssessmentStrategie, List<AssessmentStrategie>>(new AssessmentStrategie(), plos);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAssesmentStrategies(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                var pid = Convert.ToInt32(Session["programId"]);
                var courseId = Convert.ToInt32(Session["cid"]);
                if (ModelState.IsValid)
                {
                    var peo = new AssessmentStrategie();
                    peo.UniversityId = user.UniversityId;
                    peo.FacultyId = user.FacultyId;
                    peo.DepartmentId = user.DepartmentId;
                    peo.CourseId = courseId;
                    peo.ProgramId = pid;
                    peo.Strategies = form["Item1.Strategies"];
                    peo.Description = form["Item1.Description"];
                    db.AssessmentStrategies.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "Strategie Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("AssesmentStrategies", new RouteValueDictionary(new { Controller = "Course", Action = "AssesmentStrategies", id = (int)Session["cid"] }));

        }

        [HttpGet]
        public ActionResult UpdateAssesmentStrategies(int id)
        {
            var mission = db.AssessmentStrategies.Find(id);
            var pid = Convert.ToInt32(Session["programId"]);
            var courseId = Convert.ToInt32(Session["cid"]);
            if (mission == null)
            {
                return HttpNotFound();
            }

            var missions = db.AssessmentStrategies.Where(x =>x.CourseId==courseId&&x.ProgramId==pid).ToList();
            var tuple = new Tuple<AssessmentStrategie, List<AssessmentStrategie>>(mission, missions);
            ViewBag.data = true;
            return View("AssesmentStrategies", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAssesmentStrategies(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.AssessmentStrategies.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.Strategies = form["Item1.Strategies"];
                    existingMission.Description = form["Item1.Description"];
                    db.SaveChanges();
                    TempData["msg"] = "Strategie Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            
            return RedirectToAction("AssesmentStrategies", new RouteValueDictionary(new { Controller = "Course", Action = "AssesmentStrategies", id = (int)Session["cid"] }));
        }

        public ActionResult ResourceList(int id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var pid = Convert.ToInt32(Session["programId"]);
            Session["cid"] = id;
            ViewBag.ResourceTypeList = new SelectList(db.ResourceTypes.ToList(),"ResourceTypeId","TypeName");
            var plos = db.Resources.Where(x => x.CourseId == id && x.ProgramId == pid && x.DepartmentId == user.DepartmentId).ToList();
            var tuple = new Tuple<Resource, List<Resource>>(new Resource(), plos);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddResourceList(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                var pid = Convert.ToInt32(Session["programId"]);
                var courseId = Convert.ToInt32(Session["cid"]);
                if (ModelState.IsValid)
                {
                    var peo = new Resource();
                    peo.UniversityId = user.UniversityId;
                    peo.FacultyId = user.FacultyId;
                    peo.DepartmentId = user.DepartmentId;
                    peo.CourseId = courseId;
                    peo.ProgramId = pid;
                    peo.ResourceName = form["Item1.ResourceName"];
                    peo.ResourceTypeId =Convert.ToInt32(form["Item1.ResourceTypeId"]);
                    db.Resources.AddOrUpdate(peo);
                    db.SaveChanges();
                    TempData["msg"] = "Resource Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }
            return RedirectToAction("ResourceList", new RouteValueDictionary(new { Controller = "Course", Action = "ResourceList", id = (int)Session["cid"] }));

        }

        [HttpGet]
        public ActionResult UpdateResourceList(int id)
        {
            var mission = db.Resources.Find(id);
            var pid = Convert.ToInt32(Session["programId"]);
            var courseId = Convert.ToInt32(Session["cid"]);
            if (mission == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResourceTypeList = new SelectList(db.ResourceTypes.ToList(), "ResourceTypeId", "TypeName");
            var missions = db.Resources.Where(x => x.CourseId == courseId && x.ProgramId == pid).ToList();
            var tuple = new Tuple<Resource, List<Resource>>(mission, missions);
            ViewBag.data = true;
            return View("ResourceList", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateResourceList(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.Resources.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.ResourceName = form["Item1.ResourceName"];
                    existingMission.ResourceTypeId = Convert.ToInt32(form["Item1.ResourceTypeId"]);
                    db.SaveChanges();
                    TempData["msg"] = "Resource Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            
            return RedirectToAction("ResourceList", new RouteValueDictionary(new { Controller = "Course", Action = "ResourceList", id = (int)Session["cid"] }));
        }

        public ActionResult LearningPlan(int id)

        {


            var p = db.LearningPlans.Find(id);
            var ci = db.LearningPlans.Where(x => x.CourseId == id).OrderByDescending(x=>x.Week.WeekId).ToList();
            ViewBag.closes = db.CLOes.Where(x => x.CourseId == id).ToList();
            ViewBag.ci = ci;
            Session["courseId"] = id;
            return View();
        }
        
        public ActionResult CreateLearningPlan()
        {
            var idd = (int)Session["courseId"];
            List<Week> w = db.Weeks.ToList();
            

            var list = db.LearningPlans.Where(x => x.CourseId == idd).ToList();
            //ViewBag.l = list;
            foreach (var item in list)
            {
                var s = db.Weeks.Find(item.WeekId);
                w.Remove(s);
            }

            ViewBag.TimeLineId = new SelectList(w, "WeekId", "Timeline");

            List<Course> CourseList = db.Courses.ToList();
            ViewBag.CourseList = new SelectList(CourseList, "CourseId", "CourseCode");
            ViewBag.CLOId = new SelectList(db.CLOes, "CLOId", "Outcomes");
            //ViewBag.TimeLineId = new SelectList(db.Weeks, "WeekId", "Timeline");
            ViewBag.AssessmentStrategieId = new SelectList(db.AssessmentStrategies, "AssessmentStrategieId", "Strategies");
            ViewBag.TeachingStrategieId = new SelectList(db.TeachingStrategies, "TeachingStrategieId", "Strategies");

            return View();
        }
        [HttpPost]
        public ActionResult CreateLearningPlan(LearningPlan learningPlan)
        {
            var idd = (int)Session["courseId"];
            learningPlan.CourseId = (int)Session["courseId"];
            db.LearningPlans.Add(learningPlan);
            db.SaveChanges();
            Session["subjectId"] = learningPlan.CourseId;
            Session["planId"] = learningPlan.PlanId;
            return RedirectToAction("LPCLO", new RouteValueDictionary(new { Controller = "Course", Action = "LPCLO", id = (int)Session["planId"], cid = (int)Session["courseId"] }));

        }

        public ActionResult LPCLO(int? id, int? cid)
        {
            List<CLO> CloList = db.CLOes.Where(x => x.CourseId == cid).ToList();
            var cloSelectList = CloList.Select((item, index) => new SelectListItem
            {
                Value = item.CLOId.ToString(),
                Text = "CLO " + (index + 1)
            });

            ViewBag.CloList = new SelectList(cloSelectList, "Value", "Text");

            var q = db.LPCLOes.Where(x => x.PlanId == id).ToList();
            var data = db.LPCLOes.Find(id);
            var tuple = new Tuple<LPCLO, List<LPCLO>>(data, q);

            //var cid = (int)Session["subjectId"];
            //List<CLO> CloList = db.CLOes.Where(x => x.CourseId == cid).ToList();

            //ViewBag.CloList = new SelectList(CloList, "CLOId", "Outcomes");
            //ViewBag.CloList = new SelectList(CloList.Select((item, index) => new { Index = item.CLOId, Text = "CLO " +  (index + 1) }), "Index", "Text");

            //var q = db.LPCLOes.Where(x => x.PlanId == id).ToList();
            //var data = db.LPCLOes.Find(id);
            //var tuple = new Tuple<LPCLO, List<LPCLO>>(data, q);
            Session["planId"] = id;
            return View(tuple);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLPCLO(FormCollection frm)
        {
            //List<CLO> CloList = db.CLOes.Where(x => x.CourseId == cid).ToList();
            //var cloSelectList = CloList.Select((item, index) => new SelectListItem
            //{
            //    Value = item.CLOId.ToString(),
            //    Text = "CLO " + (index + 1)
            //});

            //ViewBag.CloList = new SelectList(cloSelectList, "Value", "Text");
            var cid = (int)Session["courseId"];
            var b = new LPCLO();
            b.CourseId = cid;
            b.CLOId = Convert.ToInt32(frm["CloId"]);
            var clo = db.CLOes.Find(b.CLOId);
            b.Outcomes = clo.Outcomes;
            b.PlanId = (int)Session["planId"];
            db.LPCLOes.Add(b);
            db.SaveChanges();
            return RedirectToAction("LPCLO", new RouteValueDictionary(new { Controller = "Course", Action = "LPCLO", id = (int)Session["planId"], cid = cid }));
        }
        public ActionResult LPA(int id)
        {
            List<AssessmentStrategie> AssesmntList = db.AssessmentStrategies.ToList();

            ViewBag.AssesmntList = new SelectList(AssesmntList, "AssessmentStrategieId", "Strategies");
            var q = db.LPAssessmentStrategies.Where(x => x.PlanId == id).ToList();
            var data = db.LPAssessmentStrategies.Find(id);
            var tuple = new Tuple<LPAssessmentStrategie, List<LPAssessmentStrategie>>(data, q);
            Session["planId"] = id;
            return View(tuple);
        }
        [HttpPost]
        public ActionResult CreateLPA(FormCollection frm)
        {
            List<AssessmentStrategie> AssesmntList = db.AssessmentStrategies.ToList();

            ViewBag.AssesmntList = new SelectList(AssesmntList, "AssessmentStrategieId", "Strategies");

            var b = new LPAssessmentStrategie();
            b.CourseId = (int)Session["courseId"];
            b.AssessmentStrategieId = Convert.ToInt32(frm["AsmntId"]);
            var lpa = db.AssessmentStrategies.Find(b.AssessmentStrategieId);
            b.Strategies = lpa.Strategies;
            b.PlanId = (int)Session["planId"];
            //     b.Strategies = db.AssessmentStrategies.Find(b.AssessmentStrategieId).Strategies.ToString();
            db.LPAssessmentStrategies.Add(b);
            db.SaveChanges();
            return RedirectToAction("LPA", new RouteValueDictionary(new { Controller = "Course", Action = "LPA", id = (int)Session["planId"] }));
        }
        public ActionResult LPTS(int id)
        {
            List<TeachingStrategie> StrategieList = db.TeachingStrategies.ToList();

            ViewBag.StrategieList = new SelectList(StrategieList, "TeachingStrategieId", "Strategies");
            var q = db.LPTeachingStrategies.Where(x => x.PlanId == id).ToList();
            var data = db.LPTeachingStrategies.Find(id);
            var tuple = new Tuple<LPTeachingStrategie, List<LPTeachingStrategie>>(data, q);
            Session["planId"] = id;
            return View(tuple);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLPTS(FormCollection frm)
        {
            List<TeachingStrategie> StrategieList = db.TeachingStrategies.ToList();

            ViewBag.StrategieList = new SelectList(StrategieList, "TeachingStrategieId", "Strategies");

            var b = new LPTeachingStrategie();
            b.CourseId = (int)Session["courseId"];
            b.TeachingStrategieId = Convert.ToInt32(frm["tsId"]);
            var lpts = db.TeachingStrategies.Find(b.TeachingStrategieId);
            b.Strategies = lpts.Strategies;
            b.PlanId = (int)Session["planId"];
            db.LPTeachingStrategies.Add(b);
            db.SaveChanges();
            return RedirectToAction("LPTS", new RouteValueDictionary(new { Controller = "Course", Action ="LPTS", id = (int)Session["planId"] }));
        }
    }
}