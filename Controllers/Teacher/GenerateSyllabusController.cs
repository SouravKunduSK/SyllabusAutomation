using Rotativa;
using SelectPdf;
using SyllabusAutomation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SyllabusAutomation.Controllers.Teacher
{
    public class GenerateSyllabusController : Controller
    {
        SyllabusAutomationEntities db = new SyllabusAutomationEntities();
        // GET: GenerateSyllabus

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
            Session["progrmId"] = frm["ProgramId"];
            TempData["msg"] = "Program is selected";
            return RedirectToAction("SubjectList", "GenerateSyllabus");
        }
        public ActionResult SubjectList()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var prgmId = Convert.ToInt32(Session["progrmId"]);
            var q = db.Courses.Where(x=>x.DepartmentId==user.DepartmentId && x.ProgramId==prgmId).ToList();
            return View(q);
        }
        public ActionResult GeneratedFullSyllabus(int? id, int? uid)
        {
            if(Session["uid"] == null)
            {
                Session["uid"] = uid;
            }
            int usrid = (int)Session["uid"];
            ViewBag.uid = uid;
            var user = db.Users.Find(uid);
            
            Session["courseId"] = id;
            var course = db.Courses.Find(id);
            var prgmId =(int) course.ProgramId;
            var program = db.Programs.Where(x=>x.ProgramId==prgmId).FirstOrDefault();
            ViewBag.ProgramList = program.ProgramName;
            ViewBag.userDetail = db.Users.Where(x=>x.UserID == uid).ToList();
            ViewBag.missionofUniversity = db.MissionOfUniversities.Where(x => x.UniversityId == user.UniversityId).ToList();
            ViewBag.deptDetail = db.Departments.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            ViewBag.missionofDept = db.MissionOfDepartments.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            ViewBag.peoList = db.PEOs.Where(x=>x.ProgramId==prgmId).ToList();
            ViewBag.ploList = db.PLOes.Where(x => x.ProgramId == prgmId).ToList();
            ViewBag.genericSkillList = db.GenericSkills.Where(x => x.ProgramId == prgmId).ToList();
            /*TempData["msg"] = db.Courses.Where(x => x.CourseId == ).ToList();*/
            var q = db.LearningPlans.Where(x => x.CourseId == id).ToList();
            var r = db.Courses.Where(x => x.CourseId == id).ToList();
            ViewBag.courseDetail = db.Courses.Where(x => x.CourseId == id).ToList();
            ViewBag.cloDetail = db.CLOes.Where(x => x.CourseId == id).ToList();
            ViewBag.objective = db.CourseObjectives.Where(x => x.CourseId == id).ToList();
            //    ViewBag.lesonPlans = db.LessonPlans.Where(x => x.CourseId == id).ToList();
            ViewBag.assessments = db.AssessmentStrategies.ToList();
            ViewBag.textBook = db.Resources.Where(x => x.ResourceType.ResourceTypeId == 1 && x.CourseId == id).ToList();
            ViewBag.refBook = db.Resources.Where(x => x.ResourceType.ResourceTypeId == 2 && x.CourseId == id).ToList();

            var p = db.LearningPlans.Find(id);
            var ci = db.LearningPlans.Where(x => x.CourseId == id).ToList();
            ViewBag.ci = ci;
            ViewBag.cieList = db.CIEs.Where(x=>x.CourseId==id && x.ProgramId==prgmId).ToList();
            ViewBag.seeList = db.SEEs.Where(x => x.CourseId == id && x.ProgramId == prgmId).ToList();
            ViewBag.markList = db.Marks.Where(x=>x.MarksId == course.MarksId && x.ProgramId==prgmId).ToList() ;


            return View();
        }
        public ActionResult GeneratePdf()
        {
            
            return new Rotativa.ActionAsPdf("GeneratedFullSyllabus", new RouteValueDictionary(new { Controller = "GenerateSyllabus", Action = "GeneratedFullSyllabus", id = (int)Session["courseId"], uid = (int)Session["uid"]}));
        }


        #region Creating Detail syllabus
        public ActionResult Selection()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var programs = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).OrderBy(x => x.ShortName).ToList();
            ViewBag.ProgramList = new SelectList(programs, "ProgramId", "ShortName");
            var sessions = db.Sessions.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            ViewBag.SessionList = new SelectList(sessions, "SessionId", "SessionName");
            //var years = db.EduYears.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            //ViewBag.YearList = new SelectList(years, "YearId", "YearName");
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
            //var programs = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).OrderBy(x => x.ShortName).ToList();
            //ViewBag.ProgramList = new SelectList(programs, "ProgramId", "ShortName");
            //var sessions = db.Sessions.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            //ViewBag.SessionList = new SelectList(sessions, "SessionId", "SessionName");
            //var years = db.EduYears.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            //ViewBag.YearList = new SelectList(years, "YearId", "YearName");
            //var semesters = db.Semesters.ToList();
            //ViewBag.semesterList = new SelectList(semesters, "SemesterId", "SemesterName");
            Session["progrmId"] = frm["ProgramId"];
            Session["sessionId"] = frm["SessionId"];
            Session["year"] = frm["YearLength"];
            Session["semester"] = frm["SemesterPerYear"];
            return RedirectToAction("AddSubjects", "GenerateSyllabus");
        }

        public ActionResult AddSubjects()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var dept = db.Departments.Find(user.DepartmentId);
            Session["deptId"] = dept.DepartmentId;
            var programId = Convert.ToInt32(Session["progrmId"]);
            var sessionId = Convert.ToInt32(Session["sessionId"]);
            var yearId = Convert.ToInt32(Session["year"]);
            var semesterId = Convert.ToInt32(Session["semester"]);
            var syllabus = db.Syllabus.Where(x => x.DeptId == dept.DepartmentId && x.YearId == yearId
                                                && x.SemesterId == semesterId && x.SessionId == sessionId).ToList();
            var tuple = new Tuple<Syllabu, List<Syllabu>>(new Syllabu(), syllabus);

            ViewBag.program = db.Programs.Find(Convert.ToInt32(Session["progrmId"])).ProgramName;
            ViewBag.session = db.Sessions.Find(sessionId).SessionName;
            ViewBag.year = db.EduYears.Find(yearId).YearName;
            ViewBag.semester = db.Semesters.Find(semesterId).SemesterName;
            var subjects = db.Courses.Where(x=>x.ProgramId == programId 
                                            && x.DepartmentId == dept.DepartmentId).ToList();
            ViewBag.SubjectList = new SelectList(subjects, "CourseId", "CourseTitle");


            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSemseterSubjects(FormCollection form)
        {
            try
            {
                int uid = (int)Session["uid"];
                var user = db.Users.Find(uid);
                if (ModelState.IsValid)
                {
                    var syllabus = new Syllabu();
                    syllabus.ProgramId = Convert.ToInt32(Session["progrmId"]);
                    syllabus.DeptId = user.DepartmentId;
                    syllabus.CourseCode = form["Item1.CourseCode"];
                    syllabus.CourseId = Convert.ToInt32(form["Item1.CourseId"]);
                    syllabus.SessionId = Convert.ToInt32(Session["sessionId"]);
                    syllabus.YearId = Convert.ToInt32(Session["year"]);
                    syllabus.SemesterId = Convert.ToInt32(Session["semester"]);
                    db.Syllabus.AddOrUpdate(syllabus);
                    db.SaveChanges();
                    TempData["msg"] = "Subject Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("AddSubjects");
        }

        [HttpGet]
        public ActionResult UpdateSemseterSubjects(int id)
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var dept = db.Departments.Find(user.DepartmentId);
            var mission = db.Syllabus.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }
            var programId = Convert.ToInt32(Session["progrmId"]);
           
            var sessionId = Convert.ToInt32(Session["sessionId"]);
            var yearId = Convert.ToInt32(Session["year"]);
            var semesterId = Convert.ToInt32(Session["semester"]);
            ViewBag.program = db.Programs.Find(programId).ProgramName;
            ViewBag.session = db.Sessions.Find(sessionId).SessionName;
            ViewBag.year = db.EduYears.Find(yearId).YearName;
            ViewBag.semester = db.Semesters.Find(semesterId).SemesterName;
            var missions = db.Syllabus.Where(x => x.DeptId == mission.DeptId && x.YearId == yearId
                                                && x.SemesterId == semesterId && x.SessionId == sessionId).ToList();
            var tuple = new Tuple<Syllabu, List<Syllabu>>(mission, missions);
            ViewBag.data = true;
            var subjects = db.Courses.Where(x => x.ProgramId == programId
                                            && x.DepartmentId == dept.DepartmentId).ToList();
            ViewBag.SubjectList = new SelectList(subjects, "CourseId", "CourseTitle");
            return View("AddSubjects", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSemseterSubjects(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMission = db.Syllabus.Find(id);
                    if (existingMission == null)
                    {
                        return HttpNotFound();
                    }
                    existingMission.CourseCode = form["Item1.CourseCode"];
                    existingMission.CourseId = Convert.ToInt32(form["Item1.CourseId"]);
                    db.SaveChanges();
                    TempData["msg"] = "Course Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("AddSubjects");
        }
        #endregion

        #region PDF Creation

        public ActionResult SessionSelection()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var programs = db.Programs.Where(x => x.DepartmentId == user.DepartmentId).OrderBy(x => x.ShortName).ToList();
            ViewBag.ProgramList = new SelectList(programs, "ProgramId", "ShortName");
            var sessions = db.Sessions.Where(x => x.DepartmentId == user.DepartmentId).OrderBy(x => x.SessionId).ToList();
            ViewBag.SessionList = new SelectList(sessions, "SessionId", "SessionName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SessionSelection(FormCollection frm)
        {
            int userid = (int)Session["uid"];
            var user = db.Users.Find(userid);
            Session["progrmId"] = frm["ProgramId"];
            Session["sessionId"] = frm["SessionId"];
            //TempData["msg"] = "Program is selected";
            //return RedirectToAction("FullSyllabus", "GenerateSyllabus");
            return RedirectToAction("FullSyllabus", new RouteValueDictionary(new { Controller = "GenerateSyllabus", Action = "FullSyllabus", pid = Convert.ToInt32(Session["progrmId"]), sid = Convert.ToInt32(Session["sessionId"]), uid = userid }));
        }
        public ActionResult FullSyllabus(int? pid,int? sid, int? uid)
        {
            if (Session["uid"] == null)
            { 
                Session["uid"] = uid;
                Session["progrmId"] = pid;
                Session["sessionId"] = sid;
            }
            
            //int usrid = (int)Session["uid"];
            ViewBag.uid = uid;
            var user = db.Users.Find(uid);

            
            //var course = db.Courses.Find(id);
            //var prgmId = (int)course.ProgramId;


            var program = db.Programs.Where(x => x.ProgramId == pid).FirstOrDefault();
            var programdetail = db.Programs.Where(x => x.ProgramId == pid).ToList();
            ViewBag.ProgramFullName = program.ProgramName;
            ViewBag.ProgramShortName = program.ShortName;
            ViewBag.Program = programdetail;
            ViewBag.CurriculumStructureList = db.CurriculmnStructures.Where(x=>x.ProgramId==pid && x.DepartmentId == user.DepartmentId).ToList();

            var session = db.Sessions.Where(x=>x.SessionId==sid).FirstOrDefault();
            ViewBag.SessionName = session.SessionName;

            ViewBag.userDetail = db.Users.Where(x => x.UserID == uid).ToList();
            ViewBag.missionofUniversity = db.MissionOfUniversities.Where(x => x.UniversityId == user.UniversityId).ToList();
            ViewBag.deptDetail = db.Departments.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            ViewBag.missionofDept = db.MissionOfDepartments.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            ViewBag.peoList = db.PEOs.Where(x => x.ProgramId == pid).ToList();
            ViewBag.ploList = db.PLOes.Where(x => x.ProgramId == pid).ToList();
            ViewBag.genericSkillList = db.GenericSkills.Where(x => x.ProgramId == pid).ToList();


            /*TempData["msg"] = db.Courses.Where(x => x.CourseId == ).ToList();*/
            //var viewModelList = new List<CourseViewModel>();

            //var courses = db.Syllabus.Where(x=>x.SessionId==sid && x.ProgramId == pid && x.DeptId==user.DepartmentId).ToList();
            //foreach(var course in courses)
            //{
            //    ViewBag.courseDetail = db.Courses.Where(x => x.CourseId == course.CourseId).ToList();
            //    var q = db.LearningPlans.Where(x => x.CourseId == course.CourseId).ToList();
            //    var marks = db.Marks.Find(course.CourseId);
            //    var viewModel = new CourseViewModel
            //    {
            //        YearId =course.EduYear.YearId,
            //        SemesterId = course.Semester.SemesterId,
            //        CourseId = course.Course.CourseId,
            //        CourseCode = course.CourseCode,
            //        MarksId = marks.MarksId

            //    };
            //    viewModelList.Add(viewModel);
            //}
            //ViewBag.ViewModelList = viewModelList;

            //var r = db.Courses.Where(x => x.CourseId == id).ToList();
            //ViewBag.courseDetail = db.Courses.Where(x => x.CourseId == id).ToList();
            //ViewBag.cloDetail = db.CLOes.Where(x => x.CourseId == id).ToList();
            //ViewBag.objective = db.CourseObjectives.Where(x => x.CourseId == id).ToList();
            //    ViewBag.lesonPlans = db.LessonPlans.Where(x => x.CourseId == id).ToList();
            //ViewBag.assessments = db.AssessmentStrategies.ToList();
            //ViewBag.textBook = db.Resources.Where(x => x.ResourceType.ResourceTypeId == 1 && x.CourseId == id).ToList();
            //ViewBag.refBook = db.Resources.Where(x => x.ResourceType.ResourceTypeId == 2 && x.CourseId == id).ToList();

            //var p = db.LearningPlans.Find(id);
            //var ci = db.LearningPlans.Where(x => x.CourseId == id).ToList();
            //ViewBag.ci = ci;
            //ViewBag.cieList = db.CIEs.Where(x => x.CourseId == id && x.ProgramId == prgmId).ToList();
            //ViewBag.seeList = db.SEEs.Where(x => x.CourseId == id && x.ProgramId == prgmId).ToList();
            //ViewBag.markList = db.Marks.Where(x => x.MarksId == course.MarksId && x.ProgramId == prgmId).ToList();


            var syllabusData = db.Syllabus.Where(x => x.SessionId == sid && x.ProgramId == pid && x.DeptId == user.DepartmentId).ToList();
            var organizedData = OrganizeData(syllabusData);
            ViewBag.SyllabusData = organizedData;
            return View();
        }
        private List<List<Syllabu>> OrganizeData(List<Syllabu> syllabusData)
        {
            // Organize syllabus data by year and semester
            var organizedData = syllabusData
                .GroupBy(s => new { s.YearId, s.SemesterId })
                .OrderBy(g => g.Key.YearId)
                .ThenBy(g => g.Key.SemesterId)
                .Select(g => g.ToList())
                .ToList();

            return organizedData;
        }


        public ActionResult FullSyllabusPdf()
        {
             var userId = Convert.ToInt32(Session["uid"]);
            var programId = Convert.ToInt32(Session["progrmId"]);
            var sessionId = Convert.ToInt32(Session["sessionId"]);
            return new Rotativa.ActionAsPdf("FullSyllabus", new RouteValueDictionary(new { Controller = "GenerateSyllabus", Action = "FullSyllabus",  pid = programId,   sid = sessionId, uid = userId }));
            
        }
        #endregion
    }
}