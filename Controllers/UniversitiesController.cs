﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SyllabusAutomation.Models;

namespace SyllabusAutomation.Controllers.Admin
{
    [Authorize]
    public class UniversitiesController : Controller
    {
        private SyllabusAutomationEntities db = new SyllabusAutomationEntities();

        // GET: Universities
        public ActionResult Index()
        {
            return View(db.Universities.ToList());
        }

        // GET: Universities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            University university = db.Universities.Find(id);
            if (university == null)
            {
                return HttpNotFound();
            }
            if(university.QualityPolicy==null||university.VisionOfUniversity == null || university.Logo == null)
            {
                TempData["message"] = "Update Your University Detail, Please.";
            }
            return View(university);
        }

        // GET: Universities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Universities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UniversityId,UniName,ShortName")] University university)
        {
            if (ModelState.IsValid)
            {
                db.Universities.Add(university);
                db.SaveChanges();
                return RedirectToAction("Index", "Universities");
            }

            return View(university);
        }

        // GET: Universities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            University university = db.Universities.Find(id);
            if (university == null)
            {
                return HttpNotFound();
            }
            return View(university);
        }

        // POST: Universities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UniversityId,UniName,ShortName")] University university)
        {
            if (ModelState.IsValid)
            {
                university.Logo = university.Logo;
                university.QualityPolicy = university.QualityPolicy;
                university.VisionOfUniversity = university.VisionOfUniversity;
                db.Entry(university).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(university);
        }


        // GET: Universities/Edit/5
        public ActionResult EditDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            University university = db.Universities.Find(id);
            if (university == null)
            {
                return HttpNotFound();
            }
            return View(university);
        }

        // POST: Universities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetail([Bind(Include = "UniversityId,UniName,ShortName,Logo,VisionOfUniversity,QualityPolicy")] University university)
        {
            if (ModelState.IsValid)
            {
                university.UniName = university.UniName;
                university.ShortName = university.ShortName;
                db.Entry(university).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(university);
        }

        // GET: Universities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            University university = db.Universities.Find(id);
            if (university == null)
            {
                return HttpNotFound();
            }
            return View(university);
        }

        // POST: Universities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            University university = db.Universities.Find(id);
            db.Universities.Remove(university);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
