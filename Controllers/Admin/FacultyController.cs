using SyllabusAutomation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SyllabusAutomation.Controllers.Admin
{
    public class FacultyController : Controller
    {
        // GET: Faculty
        private SyllabusAutomationEntities db = new SyllabusAutomationEntities();

        // GET: Uni_Mission
        public ActionResult FacultyNamesOfUniversity()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var university = db.Universities.Find(user.UniversityId);
            Session["uniId"] = university.UniversityId;
            var faculties = db.Faculties.Where(x => x.UniversityId == university.UniversityId).ToList();
            var tuple = new Tuple<Faculty, List<Faculty>>(new Faculty(), faculties);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewFacultyName(FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var faculty = new Faculty();
                    faculty.UniversityId = (int)Session["uniId"];
                    faculty.FacultyName = form["Item1.FacultyName"];
                    faculty.ShortName = form["Item1.ShortName"];
                    db.Faculties.AddOrUpdate(faculty);
                    db.SaveChanges();
                    TempData["msg"] = "Faculty Name Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("FacultyNamesOfUniversity","Faculty");
        }

        [HttpGet]
        public ActionResult UpdateFacultyName(int id)
        {
            var faculty = db.Faculties.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }

            var faculties = db.Faculties.Where(x => x.UniversityId == faculty.UniversityId).ToList();
            var tuple = new Tuple<Faculty, List<Faculty>>(faculty, faculties);
            ViewBag.data = true;
            return View("FacultyNamesOfUniversity", tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFacultyName(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingFaculty = db.Faculties.Find(id);
                    if (existingFaculty == null)
                    {
                        return HttpNotFound();
                    }
                    existingFaculty.FacultyName = form["Item1.FacultyName"];
                    existingFaculty.ShortName = form["Item1.ShortName"];
                    db.SaveChanges();
                    TempData["msg"] = "Faculty Updated Successfully!";
                }
            }
            catch 
            {
                TempData["msg"] = "Something Error Occurred! Try Again... " ;
            }

            return RedirectToAction("FacultyNamesOfUniversity");
        }
    }
}