using SyllabusAutomation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SyllabusAutomation.Controllers.Admin
{
    public class UniversityController : Controller
    {
        SyllabusAutomationEntities db = new SyllabusAutomationEntities();
        // GET: University
        //Showing University Detail
        public ActionResult UniversityDetail()
        {
            int uid = (int)Session["uid"];
            var user = db.Users.Find(uid);
            var university = db.Universities.Find(user.UniversityId);
            return View(university);
        }

        //Update University Details
        [HttpGet]
        public ActionResult UpdateUniversityDetail(int?id)
        {
            Session["universityId"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var university = db.Universities.Find(id);
            Session["logo"] = university.Logo;
            if (university == null)
            {
                return HttpNotFound();
            }
            return View(university);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUniversityDetail(University university, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    int width = 100;
                    int height = 100;
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);

                    fileName = fileName + Guid.NewGuid() + extension;
                    var folder = Server.MapPath("~/Uploads/");
                    string path = Path.Combine(folder, fileName);
                    university.Logo = "~/Uploads/" + fileName;
                    
                    Image image = Image.FromStream(file.InputStream, true, true);
                    var newImage = new Bitmap(width, height);
                    using (var a = Graphics.FromImage(newImage))
                    {
                        a.DrawImage(image, 0, 0, width, height);
                        newImage.Save(path);
                    }
                }
                else if(Session["logo"] != null)
                {
                    university.Logo =Convert.ToString(Session["logo"]);
                }
                //else
                //{
                //    university.Logo = "~/Uploads/NoImage.png";
                //}
                db.Universities.AddOrUpdate(university);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                TempData["msg"] = "University Details are Upated Sucessfully!";
                return RedirectToAction("UniversityDetail", "University");
             
            }
            else
            {
                TempData["msg"] = "Something error occurred! Try again...";
                return RedirectToAction("UpdateUniversityDetail", "University");
            }
            
        }
    }
}