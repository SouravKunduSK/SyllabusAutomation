using Newtonsoft.Json.Linq;
using SyllabusAutomation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;

namespace SyllabusAutomation.Controllers.Teacher
{
    public class MappingPEOToMissionController : Controller
    {
        
        private SyllabusAutomationEntities db = new SyllabusAutomationEntities();


        public ActionResult MapPEOToMission()
        {
            var peoMissionViewModels = GetPEOMissionViewModels();
            return View(peoMissionViewModels);
        }
        private List<PEOToMission> GetPEOMissionViewModels()
        {
            // Implement this method to fetch PEOs and Missions data and create view models
            List<PEOToMission> peoMissionViewModels = new List<PEOToMission>();

            // Example logic:
            var peos = db.PEOs.ToList();
            var missions = db.MissionOfDepartments.ToList();
            var peoMission = db.PEOToMissions.ToList();

            foreach (var peo in peos)
            {
                foreach (var mission in missions)
                {
                    peoMission = db.PEOToMissions.Where(x => x.PEOId == peo.PEOId && x.MissionId == mission.MissionId).ToList();
                    peoMissionViewModels.Add(new PEOToMission
                    {
                        PEOId = peo.PEOId,
                        MissionId = mission.MissionId,
                        Value  = 0 // Assuming you have a property named MissionName in MissionOfDept
                    });
                }
            }

            return peoMissionViewModels;
        }

        [HttpPost]
        
        public ActionResult AddMapPEOToMission(List<PEOToMission> mappings)
        {
            try
            {

                foreach (var mapping in mappings)
                {
                    var existingMapping = db.PEOToMissions.FirstOrDefault(
                        pm => pm.PEOId == mapping.PEOId && pm.MissionId == mapping.MissionId);

                    if (existingMapping != null)
                    {
                        existingMapping.Value = mapping.Value;
                    }
                    else
                    {
                        // Handle adding new mappings if necessary
                        db.PEOToMissions.Add(new PEOToMission
                        {
                            PEOId = mapping.PEOId,
                            MissionId = mapping.MissionId,
                            Value = mapping.Value
                        });
                    }
                }
                db.SaveChanges();

                TempData["SuccessMessage"] = "Mappings saved successfully.";
                return RedirectToAction("MapPEOToMission", "MappingPEOToMission");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving mappings: " + ex.Message;
                return RedirectToAction("MapPEOToMission", "MappingPEOToMission");
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult MapPEOToMission(List<PEOToMission> mappings)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            foreach (var mapping in mappings)
        //            {
        //                var existingMapping = db.PEOToMissions
        //                .FirstOrDefault(pm => pm.PEOId == mapping.PEOId && pm.MissionId == mapping.MissionId);

        //                if (existingMapping != null)
        //                {
        //                    existingMapping.Value = mapping.Value;
        //                }
        //                else
        //                {
        //                    PEOToMission peoToMission = new PEOToMission
        //                    {
        //                        PEOId = mapping.PEOId,
        //                        MissionId = mapping.MissionId,
        //                        Value = mapping.Value,
        //                        ProgramId = 1, // Replace with your program ID
        //                        DepartmentId = 1 // Replace with your department ID
        //                    };

        //                    db.PEOToMissions.Add(peoToMission);
        //                }

        //            }

        //            db.SaveChanges();
        //            TempData["msg"] = "Mappings saved successfully.";
        //            return RedirectToAction("MapPEOToMission");
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["msg"] = "An error occurred while saving mappings."+ex;
        //            return RedirectToAction("MapPEOToMission");
        //        }
        //    }

        //    return View(mappings);
        //    //if (ModelState.IsValid)
        //    //{
        //    //    foreach (var item in viewModel)
        //    //    {
        //    //        var existingMapping = db.PEOToMissions
        //    //            .FirstOrDefault(pm => pm.PEOId == item.PEOId && pm.MissionId == item.MissionId);

        //    //        if (existingMapping != null)
        //    //        {
        //    //            existingMapping.Value = item.Value;
        //    //        }
        //    //        else
        //    //        {
        //    //            var newMapping = new PEOToMission
        //    //            {
        //    //                PEOId = item.PEOId,
        //    //                MissionId = item.MissionId,
        //    //                Value = item.Value
        //    //            };
        //    //            db.PEOToMissions.Add(newMapping);
        //    //        }
        //    //    }

        //    //    db.SaveChanges();

        //    //    TempData["msg"] = "Mappings saved successfully.";
        //    //    return RedirectToAction("MapPEOToMission");
        //    //}

        //    //// If ModelState is invalid, redisplay the view with errors
        //    //return View(viewModel);
        //}





        // GET: MappingPEOToMission
        //public ActionResult Index()
        //{

        //    var user = db.Users.Find(40);
        //    int programId = 3;
        //    List<PEO> peos = db.PEOs.Where(x=>x.ProgramId == programId).ToList();
        //    List<MissionOfDepartment> missions = db.MissionOfDepartments.Where(x=>x.DepartmentId == user.DepartmentId).ToList();
        //    List<PEOToMission> peoToMissions = db.PEOToMissions.Where(x=>x.ProgramId==programId && x.DepartmentId==user.DepartmentId).ToList();

        //    List<PEOToMissionViewModel> viewModel = new List<PEOToMissionViewModel>();
        //    foreach (var peo in peos)
        //    {
        //        var peoMissions = missions.Select(mission =>
        //            peoToMissions.FirstOrDefault(pm => pm.PEOId == peo.PEOId && pm.MissionId == mission.MissionId));
        //        foreach(var mission in peoToMissions)
        //        {
        //            viewModel.Add(new PEOToMissionViewModel
        //            {
        //                PEOId = peo.PEOId,
        //                MissionId =(int) mission.MissionId ,
        //                Value = (int)peoToMissions.First().Value
        //            });
        //        }

        //    }

        //    return View(viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult MapPEOToMissions(List<PEOToMissionViewModel> viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var item in viewModel)
        //        {
        //            foreach (var mission in item.Missions)
        //            {
        //                var existingMapping = db.PEOToMissions
        //                    .FirstOrDefault(pm => pm.PEOId == item.PEO.PEOId && pm.MissionId == mission.MissionId);

        //                if (existingMapping != null)
        //                {
        //                    existingMapping.Value = existingMapping.Value;
        //                }
        //            }
        //        }

        //        db.SaveChanges();

        //        TempData["msg"] = "Mappings saved successfully.";
        //        return RedirectToAction("Index");
        //    }

        //    // If ModelState is invalid, redisplay the view with errors
        //    return View(viewModel);
        //}
    }
}