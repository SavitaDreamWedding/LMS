using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    public class DashboardController : BaseController
    {
        public ActionResult Index()
        {
            var dashboardData = _libraryService.GetDashboardData();

            if (!dashboardData.Success)
            {
                ViewBag.ErrorMessage = dashboardData.Message;
                return View();
            }

            return View(dashboardData.Data);
        }

        [HttpGet]
        public JsonResult GetChartData()
        {
            var dashboardData = _libraryService.GetDashboardData();

            if (!dashboardData.Success)
            {
                return Json(new { success = false, message = dashboardData.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = true,
                issuesPerDay = dashboardData.Data.IssuesPerDay,
                booksByCategory = dashboardData.Data.BooksByCategory
            }, JsonRequestBehavior.AllowGet);
        }
    }
}