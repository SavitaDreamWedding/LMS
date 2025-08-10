using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Core.Models.ViewModels;

namespace Library.Web.Controllers
{
    public class BookIssueController : BaseController
    {
        public ActionResult Index()
        {
            var activeIssues = _libraryService.GetActiveIssues();

            if (!activeIssues.Success)
            {
                ViewBag.ErrorMessage = activeIssues.Message;
                return View();
            }

            return View(activeIssues.Data);
        }

        public ActionResult Issue()
        {
            var viewModel = new IssueBookViewModel
            {
                Books = _libraryService.GetBookDropdownList(),
                Members = _libraryService.GetMemberDropdownList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult IssueBook(int bookId, int memberId)
        {
            var result = _libraryService.IssueBook(bookId, memberId);
            return Json(new { success = result.Success, message = result.Message, data = result.Data });
        }

        [HttpPost]
        public JsonResult ReturnBook(int issueId)
        {
            var result = _libraryService.ReturnBook(issueId);
            return Json(new { success = result.Success, message = result.Message });
        }

        public ActionResult History(int? memberId, int? bookId)
        {
            if (bookId.HasValue)
            {
                var bookHistory = _libraryService.GetBookHistory(bookId.Value);
                ViewBag.Title = "Book History";
                return View("History", bookHistory.Data);
            }

            // Could add member history here
            return View();
        }
    }
}