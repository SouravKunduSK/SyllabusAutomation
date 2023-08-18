using SyllabusAutomation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SyllabusAutomation.Controllers.Admin
{
    public class DepartmentController : Controller
    {  
            private SyllabusAutomationEntities db = new SyllabusAutomationEntities();

           
        public ActionResult DepartmentNamesOfUniversity()
        {

            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var university = db.Universities.Find(user.UniversityId);
            Session["uniId"] = university.UniversityId;
            //var faculties = db.Faculties.Find(university.UniversityId);
            List<Faculty> FacultyList = db.Faculties.Where(x=>x.UniversityId == user.UniversityId && x.IsActive == true).OrderBy(x => x.ShortName).ToList();
            ViewBag.FacultyList = new SelectList(FacultyList, "FacultyId", "ShortName");
            var departments = db.Departments.Where(x=>x.UniversityId == university.UniversityId && x.IsActive == true).ToList();
            var tuple = new Tuple<Department, List<Department>>(new Department(), departments);
            return View(tuple);
        }
           
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult AddNewDepartmentName(FormCollection form)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var department = new Department();
                        department.UniversityId = (int)Session["uniId"];
                        department.FacultyId = Convert.ToInt32(form["Item1.FacultyId"]);
                        department.DeptName = form["Item1.DeptName"];
                        department.ShortName = form["Item1.ShortName"];
                        department.IsActive = true;
                        db.Departments.AddOrUpdate(department);
                        db.SaveChanges();
                        TempData["msg"] = "Department Name Added Successfully!";
                    }
                }
                catch
                {
                    TempData["msg"] = "Something Error Occurred! Try Again... ";
                }

                return RedirectToAction("DepartmentNamesOfUniversity", "Department");
            }

            [HttpGet]
            public ActionResult UpdateDepartmentName(int id)
            {
                var dept = db.Departments.Find(id);
                if (dept == null)
                {
                    return HttpNotFound();
                }

                var faculties = db.Faculties.Where(x => x.UniversityId == dept.UniversityId && x.IsActive == true).OrderBy(x=>x.ShortName).ToList();
                ViewBag.facultyList = new SelectList(faculties, "FacultyId", "ShortName");
                var depts = db.Departments.Where(x => x.UniversityId == dept.UniversityId && x.IsActive == true).ToList();
                var tuple = new Tuple<Department, List<Department>>(dept, depts);
                ViewBag.data = true;
                return View("DepartmentNamesOfUniversity", tuple);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult UpdateDepartmentName(int id, FormCollection form)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var existingDept = db.Departments.Find(id);
                        if (existingDept == null)
                        {
                            return HttpNotFound();
                        }
                        existingDept.FacultyId = Convert.ToInt32(form["Item1.FacultyId"]);
                        existingDept.DeptName = form["Item1.DeptName"];
                        existingDept.ShortName = form["Item1.ShortName"];
                        db.SaveChanges();
                        TempData["msg"] = "Department Updated Successfully!";
                    }
                }
                catch
                {
                    TempData["msg"] = "Something Error Occurred! Try Again... ";
                }

                return RedirectToAction("DepartmentNamesOfUniversity");
            }
        }
    }