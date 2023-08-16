
using SyllabusAutomation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace SyllabusAutomation.Controllers
{
    public class AccountController : Controller
    {
        SyllabusAutomationEntities db = new SyllabusAutomationEntities();
        //Login
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //Login post
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login)
        {
            string message = "";

            var v = db.Users.Where(x => x.Email == login.Email).FirstOrDefault();

            if (v != null && string.Compare(Crypto.Hash(login.Password), v.HashPassword) == 0)
            {
                

                login.RememberMe = false;
                int timeout = login.RememberMe?43200:10080;
                var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeout);
                string encrypted = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                cookie.Expires = DateTime.Now.AddMinutes(timeout);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);

                Session["username"] = v.Username;
                Session["uid"] = v.UserID;


                FormsAuthentication.SetAuthCookie(v.Email, false);
                var roleId = v.RoleId;
                switch(roleId)
                {
                    case 1:
                        return RedirectToAction("AdminDashboard", "Home");
                        case 2:
                        return RedirectToAction("AdministrationDashboard", "Home");
                        case 3:
                        return RedirectToAction("DepartmentDashboard", "Home");
                    case 4:
                        return RedirectToAction("Index", "Home");
                    default:
                        return View();
                }   
            }
            else
            {
                message = "Invalid credential provided!";
            }
            ViewBag.Message = message;
            return View();
        }
        public ActionResult Logout(int?id)
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            
            return RedirectToAction("Login", "Account");
        }
        [NonAction]
        public bool IsEmailExist(string email)
        {
            var v = db.Users.Where(a => a.Email == email).FirstOrDefault();
            return v != null;
        }
        // GET: Users/Create
        public ActionResult CreateUser()
        {
           
            ViewBag.RoleId = new SelectList(db.Roles.Where(x=>x.RoleName!= "SuperAdmin"), "RoleID", "RoleName");
            ViewBag.UniversityId = new SelectList(db.Universities.OrderBy(x=>x.UniversityId), "UniversityId", "UniName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser([Bind(Include = "UserID,FirstName,LastName,Username,Password,HashPassword,Email,IsActive,RoleId,ProgramId,SemisterId,YearId,SessionId,DepartmentId,FacultyId,UniversityId")] User user)
        {
            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(user.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist! Try another Email.");
                    return View(user);
                }
                int id = Convert.ToInt32(Session["uid"]);
                user.HashPassword = Crypto.Hash(user.Password);
                db.Users.Add(user);
                db.SaveChanges();
                switch(id)
                {
                        case 1:
                        return RedirectToAction("Index", "Account");
                        case 2:
                        return RedirectToAction("EndUser", "Account");
                }
                
            }

            ViewBag.RoleId = new SelectList(db.Roles.Where(x => x.RoleName != "SuperAdmin"), "RoleID", "RoleName", user.RoleId);
            ViewBag.UniversityId = new SelectList(db.Universities.OrderBy(x => x.UniversityId), "UniversityId", "UniName", user.UniversityId);
            return View(user);
        }
        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles.Where(x => x.RoleName != "SuperAdmin"), "RoleID", "RoleName");
            ViewBag.UniversityId = new SelectList(db.Universities.OrderBy(x => x.UniversityId), "UniversityId", "UniName");
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Username,Password,HashPassword,Email,IsActive,RoleId,ProgramId,SemisterId,YearId,SessionId,DepartmentId,FacultyId,UniversityId")] User user)
        {
            if (ModelState.IsValid)
            {
                user.HashPassword = Crypto.Hash(user.Password);
                db.Entry(user).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.Roles.Where(x => x.RoleName != "SuperAdmin"), "RoleID", "RoleName", user.RoleId);
            ViewBag.UniversityId = new SelectList(db.Universities.OrderBy(x => x.UniversityId), "UniversityId", "UniName", user.UniversityId);
            return View(user);
        }
        [Authorize]
        // GET: AllUsers
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Department).Include(u => u.EduYear).Include(u => u.Faculty).Include(u => u.Program).Include(u => u.Role).Include(u => u.Semester).Include(u => u.Session).Include(u => u.University).Where(x => x.RoleId != 1);
            return View(users.ToList());
        }
        [Authorize]
        // GET: EndUsers
        public ActionResult EndUser()
        {
            var users = db.Users.Include(u => u.Department).Include(u => u.EduYear).Include(u => u.Faculty).Include(u => u.Program).Include(u => u.Role).Include(u => u.Semester).Include(u => u.Session).Include(u => u.University).Where(x => x.RoleId == 3);
            return View(users.ToList());
        }


        #region for Admin
        //Show Admin Profile
        public ActionResult AdminProfile(int ?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var admin = db.Users.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }

            return View(admin);

        }
        // UpdateAdminProfile Details
        [HttpGet]
        public ActionResult UpdateAdminProfile(int? id)
        {
            Session["adminId"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var admin = db.Users.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            
            return View(admin);
        }
        [HttpPost]
        public ActionResult UpdateAdminProfile(User user)
        {
            if (ModelState.IsValid)
            {
                user.FirstName = user.FirstName;
                user.LastName = user.LastName;
                user.Email = user.Email;
                
                db.Entry(user).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                TempData["message"] = "Profile Details Updated Succefully....";
                return RedirectToAction("AdminProfile", new RouteValueDictionary(new { Controller = "Account", Action = "AdminProfile", id = (int)Session["adminId"] }));
                
            }
            return View();
        }
        #endregion for  Admin
        #region Student
        public ActionResult AssignedStudents()
        {
            int uid = (int)Session["uid"];
            var user = GetUser(uid);
            var university = db.Universities.Find(user.UniversityId);
            Session["uniId"] = university.UniversityId;
            ViewBag.ProgramList = new SelectList(db.Programs.Where(f => f.UniversityId == user.UniversityId && f.DepartmentId==user.DepartmentId).OrderBy(x => x.ShortName).ToList(), "ProgramId", "ShortName");
            ViewBag.YearList = new SelectList(db.EduYears.Where(f => f.DepartmentId == user.DepartmentId).OrderBy(x => x.YearName).ToList(), "YearId", "YearName");
            ViewBag.SessionList = new SelectList(db.Sessions.Where(f => f.DepartmentId == user.DepartmentId).OrderByDescending(x => x.SessionName).ToList(), "SessionId", "SessionName");
            var users = db.Users.Where(x => x.UniversityId == university.UniversityId && x.DepartmentId == user.DepartmentId && x.RoleId == 4).OrderBy(x => x.Username).ToList();
            var tuple = new Tuple<User, List<User>>(new User(), users);
            return View(tuple);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewStudent(FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int uid = (int)Session["uid"];
                    var teacher = GetUser(uid);
                    var user = new User();
                    user.UniversityId = teacher.UniversityId;
                    user.FacultyId = teacher.FacultyId;
                    user.DepartmentId = teacher.DepartmentId;
                    user.Username = form["Item1.Username"];
                    user.Password = form["Item1.Password"];
                    user.HashPassword = Crypto.Hash(user.Password);
                    user.RoleId = 4;
                    user.ProgramId = Convert.ToInt32(form["Item1.ProgramId"]);
                    user.YearId = Convert.ToInt32(form["Item1.YearId"]);
                    
                    user.SessionId = Convert.ToInt32(form["Item1.SessionId"]);
                    db.Users.AddOrUpdate(user);
                    db.SaveChanges();
                    TempData["msg"] = "Student Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("AssignedStudents", "Account");
        }

        [HttpGet]
        public ActionResult UpdateStudent(int id)
        {

            Session["user"] = id;
            User user = GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgramList = new SelectList(db.Programs.Where(f => f.UniversityId == user.UniversityId && f.DepartmentId == user.DepartmentId).OrderBy(x => x.ShortName).ToList(), "ProgramId", "ShortName",user.ProgramId);
            ViewBag.YearList = new SelectList(db.EduYears.Where(f => f.DepartmentId == user.DepartmentId).OrderBy(x => x.YearName).ToList(), "YearId", "YearName",user.YearId);
            ViewBag.SessionList = new SelectList(db.Sessions.Where(f => f.DepartmentId == user.DepartmentId).OrderByDescending(x => x.SessionName).ToList(), "SessionId", "SessionName",user.SessionId);
            var users = db.Users.Where(x => x.UniversityId == user.UniversityId && x.DepartmentId == user.DepartmentId && x.RoleId == 4).OrderBy(x => x.Username).ToList();
            var tuple = new Tuple<User, List<User>>(user, users);
            ViewBag.data = true;
            return View("AssignedStudents", tuple);



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStudent(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = db.Users.Find(id);
                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    user.HashPassword = Crypto.Hash(user.Password);
                    user.Username = form["Item1.Username"];
                    user.Password = form["Item1.Password"];
                    user.HashPassword = Crypto.Hash(user.Password);
                    user.ProgramId = Convert.ToInt32(form["Item1.ProgramId"]);
                    user.YearId = Convert.ToInt32(form["Item1.YearId"]);
                    user.SessionId = Convert.ToInt32(form["Item1.SessionId"]);
                    db.SaveChanges();
                    TempData["msg"] = "Student Detail Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("AssignedStudents", "Account");
        }

        #endregion



        #region Teacher
        public ActionResult AssignedTeachers()
        {

            int uid = (int)Session["uid"];
            var user = GetUser(uid);
            var university = db.Universities.Find(user.UniversityId);
            Session["uniId"] = university.UniversityId;
            Session["Visited"] = false;
            List<Faculty> faculties = db.Faculties.Where(f => f.UniversityId == user.UniversityId).OrderBy(x => x.ShortName).ToList();
            ViewBag.FacultyId = new SelectList(faculties, "FacultyId", "ShortName");
            //ViewBag.DepartmentId = new SelectList(Enumerable.Empty<SelectListItem>());
            var users = db.Users.Where(x => x.UniversityId == university.UniversityId && x.RoleId == 3).OrderBy(x => x.Username).ToList();
            var tuple = new Tuple<User, List<User>>(new User(), users);
            return View(tuple);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewTeacher(FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new User();
                    user.UniversityId = (int)Session["uniId"];
                    user.FacultyId = Convert.ToInt32(form["FacultyId"]);
                    user.DepartmentId = Convert.ToInt32(form["DepartmentId"]);
                    user.Username = form["Item1.Username"];
                    user.Password = form["Item1.Password"];
                    user.HashPassword = Crypto.Hash(user.Password);
                    user.RoleId = 3;
                    db.Users.AddOrUpdate(user);
                    db.SaveChanges();
                    TempData["msg"] = "Teacher Added Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("AssignedTeachers", "Account");
        }


        public ActionResult FillDept(int FacultyId)
        {

            var departments = new[] { new {
                DepartmentId = 0,
                DepartmentName = ""
            } }.ToList();


            int uid = (int)Session["user"];
            var user = GetUser(uid);
            if (FacultyId == user.FacultyId && Convert.ToBoolean(Session["Visited"]) != true)
            {

                departments = db.Departments.Where(d => d.FacultyId == FacultyId && d.DepartmentId == user.DepartmentId).Select(d => new {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.ShortName
                }).OrderBy(x => x.DepartmentName).ToList();
                Session["Visited"] = true;

                return Json(departments, JsonRequestBehavior.AllowGet);
            }

            else
            {

                departments = db.Departments.Where(d => d.FacultyId == FacultyId).Select(d => new {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.ShortName
                }).OrderBy(x => x.DepartmentName).ToList();
                var defItem = new[]{
                    new
                {
                    DepartmentId = 0,
                    DepartmentName = "----Select Department----"
                }
                };

                departments.InsertRange(0, defItem);
                return Json(departments, JsonRequestBehavior.AllowGet);
            }
            //var departments = new SelectList(db.Departments.Where(x => x.FacultyId == FacultyId).OrderBy(x => x.ShortName).ToList(),"DepartmentId","ShortName");


        }
        [HttpGet]
        public ActionResult UpdateTeacher(int id)
        {

            Session["user"] = id;
            User user = GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            List<Faculty> faculties = db.Faculties.Where(f => f.UniversityId == user.UniversityId).OrderBy(x => x.ShortName).ToList();
            ViewBag.FacultyList = new SelectList(faculties, "FacultyId", "ShortName", user.FacultyId);
            ViewBag.FacultyId = new SelectList(faculties, "FacultyId", "ShortName");
            Session["Visited"] = false;
            ViewBag.DepartmentList = new SelectList(db.Departments.Where(x => x.FacultyId == user.FacultyId && x.DepartmentId == user.DepartmentId).OrderBy(x => x.ShortName).ToList(), "DepartmentId", "ShortName", user.DepartmentId);
            var users = db.Users.Where(x => x.UniversityId == user.UniversityId && x.RoleId == 3).OrderBy(x => x.Username).ToList();
            var tuple = new Tuple<User, List<User>>(user, users);
            ViewBag.data = true;
            return View("AssignedTeachers", tuple);



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTeacher(int id, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = db.Users.Find(id);
                    if (existingUser == null)
                    {
                        return HttpNotFound();
                    }
                    existingUser.FacultyId = Convert.ToInt32(form["Item1.FacultyId"]);
                    existingUser.DepartmentId = Convert.ToInt32(form["Item1.DepartmentId"]);
                    existingUser.Username = form["Item1.Username"];
                    existingUser.Password = form["Item1.Password"];
                    existingUser.HashPassword = Crypto.Hash(existingUser.Password);
                    db.SaveChanges();
                    TempData["msg"] = "Teacher Updated Successfully!";
                }
            }
            catch
            {
                TempData["msg"] = "Something Error Occurred! Try Again... ";
            }

            return RedirectToAction("AssignedTeachers","Account");
        }



        private User GetUser(int Id)
        {
            User user = db.Users
                .Where(c => c.UserID == Id).FirstOrDefault();
            return user;
        }

        public ActionResult GetDepartmentsByFaculty(int facultyId)
        {
            var departments = new[] { new {
                DepartmentId = 0,
                DepartmentName = ""
            } }.ToList();
            departments = db.Departments.Where(d => d.FacultyId == facultyId).Select(d => new {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.ShortName
            }).OrderBy(x => x.DepartmentName).ToList();
            var defItem = new[]{
                    new
                {
                    DepartmentId = 0,
                    DepartmentName = "----Select Department----"
                }
                };

            departments.InsertRange(0, defItem);
            return Json(departments, JsonRequestBehavior.AllowGet);
        }

        //Show Admin Profile
        public ActionResult TeacherProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var teacher = db.Users.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            return View(teacher);

        }
        // UpdateAdminProfile Details
        [HttpGet]
        public ActionResult UpdateTeacherProfile(int? id)
        {
            Session["teacherId"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var teacher = db.Users.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            return View(teacher);
        }
        [HttpPost]
        public ActionResult UpdateTeacherProfile(User user)
        {
            if (ModelState.IsValid)
            {
                user.FirstName = user.FirstName;
                user.LastName = user.LastName;
                user.Email = user.Email;

                db.Entry(user).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                TempData["message"] = "Profile Details Updated Succefully....";
                return RedirectToAction("TeacherProfile", new RouteValueDictionary(new { Controller = "Account", Action = "TeacherProfile", id = (int)Session["teacherId"] }));

            }
            return View();
        }

        #endregion Teacher
    }
}