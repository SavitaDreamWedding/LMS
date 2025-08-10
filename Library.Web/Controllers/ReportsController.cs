using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Web.Controllers
{
    public class ReportsController : BaseController
    {
        public ActionResult OverdueMembers()
        {
            var overdueIssues = _libraryService.GetOverdueIssues();

            if (!overdueIssues.Success)
            {
                ViewBag.ErrorMessage = overdueIssues.Message;
                return View();
            }

            return View(overdueIssues.Data);
        }

        public ActionResult BookHistory(int? id)
        {
            if (!id.HasValue)
            {
                ViewBag.ErrorMessage = "Book ID is required";
                return View();
            }

            var bookHistory = _libraryService.GetBookHistory(id.Value);

            if (!bookHistory.Success)
            {
                ViewBag.ErrorMessage = bookHistory.Message;
                return View();
            }

            return View(bookHistory.Data);
        }
    }

}