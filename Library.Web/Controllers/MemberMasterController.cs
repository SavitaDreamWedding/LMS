using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Core.Models;

namespace Library.Web.Controllers
{
    public class MemberMasterController : BaseController
    {
        public ActionResult Index()
        {
            var membersResponse = _libraryService.GetAllMembers();

            if (!membersResponse.Success)
            {
                ViewBag.ErrorMessage = membersResponse.Message;
                return View();
            }

            return View(membersResponse.Data);
        }

        public ActionResult Create()
        {
            return View(new MemberMaster());
        }

        [HttpPost]
        public JsonResult Create(MemberMaster member)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                return Json(new { success = false, message = "Validation failed", errors = errors });
            }

            var result = _libraryService.AddMember(member);
            return Json(new { success = result.Success, message = result.Message, data = result.Data });
        }

        public ActionResult Edit(int id)
        {
            var memberResponse = _libraryService.GetMemberById(id);

            if (!memberResponse.Success)
            {
                ViewBag.ErrorMessage = memberResponse.Message;
                return RedirectToAction("Index");
            }

            return View(memberResponse.Data);
        }

        [HttpPost]
        public JsonResult Edit(MemberMaster member)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                return Json(new { success = false, message = "Validation failed", errors = errors });
            }

            var result = _libraryService.UpdateMember(member);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _libraryService.DeleteMember(id);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpGet]
        public JsonResult GetMember(int id)
        {
            var result = _libraryService.GetMemberById(id);
            return Json(new { success = result.Success, message = result.Message, data = result.Data }, JsonRequestBehavior.AllowGet);
        }
    }
}