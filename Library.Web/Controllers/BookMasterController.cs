using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Library.Core.Models;
using Library.Core.Services;
using Library.Core.Repositories;
namespace Library.Web.Controllers
{

    public class BookMasterController : BaseController
    {
        public ActionResult Index()
        {
            var booksResponse = _libraryService.GetAllBooks();

            if (!booksResponse.Success)
            {
                ViewBag.ErrorMessage = booksResponse.Message;
                return View();
            }

            return View(booksResponse.Data);
        }

        public ActionResult Create()
        {
            return View(new BookMaster());
        }

        [HttpPost]
        public JsonResult Create(BookMaster book)
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

            var result = _libraryService.AddBook(book);
            return Json(new { success = result.Success, message = result.Message, data = result.Data });
        }

        public ActionResult Edit(int id)
        {
            var bookResponse = _libraryService.GetBookById(id);

            if (!bookResponse.Success)
            {
                ViewBag.ErrorMessage = bookResponse.Message;
                return RedirectToAction("Index");
            }

            return View(bookResponse.Data);
        }

        [HttpPost]
        public JsonResult Edit(BookMaster book)
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

            var result = _libraryService.UpdateBook(book);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _libraryService.DeleteBook(id);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpGet]
        public JsonResult GetBook(int id)
        {
            var result = _libraryService.GetBookById(id);
            return Json(new { success = result.Success, message = result.Message, data = result.Data }, JsonRequestBehavior.AllowGet);
        }
    }
    //public class BookMasterController : Controller
    //{
    //    private readonly BookMasterService _bookService;
    //    public BookMasterController()
    //    {
    //        _bookService = new BookMasterService(new BookMasterRepository());
    //    }

    //    public ActionResult Index()
    //    {
    //        var books = _bookService.GetBooks();
    //        return View(books);
    //    }

    //    public PartialViewResult Create()
    //    {
    //        return PartialView("_CreateEditPartial", new BookMaster());
    //    }
    //    [HttpPost]
    //    public ActionResult Create(BookMaster model)
    //    {
    //        try
    //        {
    //            _bookService.AddBook(model);
    //            return Json(new { success = true });
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new { success = false, message = ex.Message });
    //        }
    //    }

    //    public PartialViewResult Edit(int id)
    //    {
    //        var book = _bookService.GetBook(id);
    //        return PartialView("_CreateEditPartial", book);
    //    }

    //    [HttpPost]
    //    public JsonResult Save(BookMaster model)
    //    {
    //        if (!ModelState.IsValid)
    //            return Json(new { success = false, message = "Validation failed." });

    //        if (model.BookId == 0)
    //            _bookService.AddBook(model);
    //        else
    //            _bookService.UpdateBook(model);

    //        return Json(new { success = true });
    //    }

    //    public ActionResult Delete(int id)
    //    {
    //        var book = _bookService.GetBook(id);
    //        if (book == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(book);
    //    }
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult DeleteConfirmed(int id)
    //    {
    //        _bookService.DeleteBook(id);
    //        return RedirectToAction("Index");
    //    }
    //}
}