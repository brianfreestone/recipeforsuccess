using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForSuccess.ServiceLayer;

namespace RecipeForSuccess_mvc.Views.Home
{
    public class HomeController : Controller
    {

        IUsersService usersService;
        
        public HomeController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}