using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForSuccess.ServiceLayer;
using RecipeForSuccess_mvc.CustomFilters;
using RecipeForSuccess.ViewModels;

namespace RecipeForSuccess_mvc.Areas.Admin.Controllers

{
    public class AdminHomeController : Controller
    {

        IUsersService usersService;

        public AdminHomeController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        // GET: Admin/AdminHome
        [AdminAuthorizationFilterAttribute]
        public ActionResult Index()
        {
            List<UserVM> users = usersService.GetUsers();
            return View(users);
        }

        [AdminAuthorizationFilterAttribute]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            usersService.DeleteUser(id);
            return RedirectToAction("Index");
        }
    }
}