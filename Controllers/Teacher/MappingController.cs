using SyllabusAutomation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SyllabusAutomation.Controllers.Teacher
{
    public class MappingController : Controller
    {
        // GET: Mapping
        SyllabusAutomationEntities db = new SyllabusAutomationEntities(); // Your database context

        public ActionResult Index()
        {
            var viewModel = new SubjectViewModel
            {
                Subjects = db.Courses.ToList(),
                ClotoploMappings = new List<CLOToPLO>(), // Initialize the list
                IsEditMode = false
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(SubjectViewModel viewModel)
        {
            if (viewModel.SelectedSubjectId != 0)
            {
                viewModel.ClotoploMappings = db.CLOToPLOes.Where(c => c.CourseId == viewModel.SelectedSubjectId).ToList();
            }

            viewModel.Subjects = db.Courses.ToList();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ToggleEditMode(SubjectViewModel viewModel)
        {
            viewModel.IsEditMode = !viewModel.IsEditMode;

            if (viewModel.SelectedSubjectId != 0)
            {
                viewModel.ClotoploMappings = db.CLOToPLOes.Where(c => c.CourseId == viewModel.SelectedSubjectId).ToList();
            }

            viewModel.Subjects = db.Courses.ToList();

            return View("Index", viewModel);
        }
    }
}