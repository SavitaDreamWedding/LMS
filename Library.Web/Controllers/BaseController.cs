using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Core.Repositories;
using Library.Core.Services;

namespace Library.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILibraryService _libraryService;

        public BaseController()
        {
            // Initialize repositories and services
            var bookRepository = new BookRepository();
            var memberRepository = new MemberRepository();
            var bookIssueRepository = new BookIssueRepository();

            _libraryService = new LibraryService(bookRepository, memberRepository, bookIssueRepository);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if user is authenticated (except for login)
            if (Session["UserId"] == null &&
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Account")
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}