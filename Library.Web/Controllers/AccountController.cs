using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Core.Models.ViewModels;

namespace Library.Web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Simple authentication (in production, use proper password hashing)
            if (model.Username == "admin" && model.Password == "admin123")
            {
                Session["UserId"] = 1;
                Session["Username"] = model.Username;
                Session["FullName"] = "System Administrator";

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}