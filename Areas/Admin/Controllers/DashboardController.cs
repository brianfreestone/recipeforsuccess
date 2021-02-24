using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForSuccess_mvc.CustomFilters;

namespace RecipeForSuccess_mvc.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        [AdminAuthorizationFilterAttribute]
        public ActionResult Index()
        {
            return View();
        }
    }
}