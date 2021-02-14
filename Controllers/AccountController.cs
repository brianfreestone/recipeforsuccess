using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForSuccess.ViewModels;
using RecipeForSuccess.ServiceLayer;

namespace RecipeForSuccess_mvc.Controllers
{
    public class AccountController : Controller
    {


        IUsersService usersService;


        public AccountController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        // GET: Account/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                int userID = usersService.InsertUser(registerVM);
                Session["CurrentUserID"] = userID;
                Session["CurrentUserName"] = registerVM.username;
                Session["CurrentUserIsAdmin"] = false;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("key", "Fields must be completed");
            }

            return View();

        }

        // GET: Account/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                UserVM userVM = usersService.GetUserByEmailAndPassword(loginVM.email, loginVM.password);
                if (userVM != null)
                {
                    Session["CurrentUserID"] = userVM.user_id;
                    Session["CurrentUserName"] = userVM.username;
                    Session["CurrentUserIsAdmin"] = userVM.is_admin;

                    if (userVM.is_admin)
                    {
                        return RedirectToRoute(new { area = "Admin", controller = "AdminHome", action = "Index" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("key", "Fields must be completed");
            }

            return View();

        }

    }
}