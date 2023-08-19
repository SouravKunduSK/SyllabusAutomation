using SyllabusAutomation.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace SyllabusAutomation.Controllers.Teacher
{
    public class TeacherController : Controller
    {
        SyllabusAutomationEntities db = new SyllabusAutomationEntities();
        // GET: Teacher
        #region Selection
        public ActionResult Selection()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var programs = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).OrderBy(x => x.ShortName).ToList();
            ViewBag.ProgramList = new SelectList(programs, "ProgramId", "ShortName");
            var sessions = db.Sessions.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            ViewBag.SessionList = new SelectList(sessions, "SessionId", "SessionName");
            var years = db.EduYears.ToList();
            ViewBag.YearList = new SelectList(years, "YearId", "YearName");
            var semesters = db.Semesters.ToList();
            ViewBag.semesterList = new SelectList(semesters, "SemesterId", "SemesterName");
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Selection(FormCollection frm)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            Session["programId"] = frm["ProgramId"];
            Session["sessionId"] = frm["SessionId"];
            Session["year"] = frm["YearId"];
            Session["semester"] = frm["SemisterId"];

            return RedirectToAction("CourseList", "Teacher");
        }

        #endregion

        #region CourseList
        public ActionResult CourseList()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var pid = Convert.ToInt32(Session["programId"]);
            var sessionId = Convert.ToInt32(Session["sessionId"]);
            var yearId = Convert.ToInt32(Session["year"]);
            var semesterId = Convert.ToInt32(Session["semester"]);


            ViewBag.program = db.Programs.Find(pid).ProgramName;
            ViewBag.session = db.Sessions.Find(sessionId).SessionName;
            ViewBag.year = db.EduYears.Find(yearId).YearName;
            ViewBag.semester = db.Semesters.Find(semesterId).SemesterName;
            
            ViewBag.CourseTypeList = new SelectList(db.CourseTypes.Where(x => x.ProgramId == pid && x.DepartmentId == user.DepartmentId).ToList(), "CourseTypeId", "CourseType1");
            var plos = db.Courses.Where(x => x.ProgramId == pid && x.SessionId == sessionId
            && x.YearId == yearId && x.SemisterId == semesterId && x.DepartmentId == user.DepartmentId && x.UserId == user.UserID).ToList();
            var tuple = new Tuple<Course, List<Course>>(new Course(), plos);
            return View(tuple);
        }

        [HttpGet]
        public ActionResult UpdateCourseList(int id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var pid = Convert.ToInt32(Session["programId"]);
            var sessionId = Convert.ToInt32(Session["sessionId"]);
            var yearId = Convert.ToInt32(Session["year"]);
            var semesterId = Convert.ToInt32(Session["semester"]);


            var mission = db.Courses.Find(id);

            if (mission == null)
            {
                return HttpNotFound();
            }

            ViewBag.program = db.Programs.Find(pid).ProgramName;
            ViewBag.session = db.Sessions.Find(sessionId).SessionName;
            ViewBag.year = db.EduYears.Find(yearId).YearName;
            ViewBag.semester = db.Semesters.Find(semesterId).SemesterName;
            ViewBag.CourseTypeList = new SelectList(db.CourseTypes.Where(x => x.ProgramId == pid && x.DepartmentId == mission.DepartmentId).ToList(), "CourseTypeId", "CourseType1", mission.CourseTypeId);
            var missions = db.Courses.Where(x => x.ProgramId == pid && x.SessionId == sessionId
            && x.YearId == yearId && x.SemisterId == semesterId && x.DepartmentId == user.DepartmentId && x.UserId == user.UserID).ToList();


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
                    peo.MarksId = mark.MarksId;
                    
                    db.SaveChanges();
                    TempData["msg"] = "Course Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("CourseList", "Teacher");
        }


        #endregion

        #region CourseWise Syllabus
        public ActionResult ProgramSelection()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var programs = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).OrderBy(x => x.ShortName).ToList();
            ViewBag.ProgramList = new SelectList(programs, "ProgramId", "ShortName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProgramSelection(FormCollection frm)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var programs = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).OrderBy(x => x.ShortName).ToList();
            ViewBag.ProgramList = new SelectList(programs, "ProgramId", "ShortName");
            var programId = Convert.ToInt32(frm["ProgramId"]);
            return RedirectToAction("MyCourses", new RouteValueDictionary(new { Controller = "Teacher", Action = "MyCourses", id = programId }));
            
        }
        public ActionResult MyCourses(int? id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var pid = id;
            ViewBag.program = db.Programs.Find(pid).ProgramName;
            var courses = db.Courses.Where(x => x.ProgramId == pid && x.DepartmentId == user.DepartmentId && x.UserId == user.UserID).ToList();
            return View(courses);
        }
        #endregion
    }
}