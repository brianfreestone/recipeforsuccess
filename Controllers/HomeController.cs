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
        IFriendsService friendsService;
        
        public HomeController(IUsersService usersService, IFriendsService friendsService)
        {
            this.usersService = usersService;
            this.friendsService = friendsService;
        }

        // GET: Home
        public ActionResult Index()
        {
            if (Session["CurrentUserName"] != null)
            {

                ViewBag.Username = Session["CurrentUserName"].ToString();

                // get friend request count
                var friendRequestCount = friendsService.GetFriendRequestCount(Convert.ToInt32(Session["CurrentUserID"]));
                if (friendRequestCount > 0)
                {
                    ViewBag.FriendRequestCount = friendRequestCount;
                }
            }
            return View();
        }
    }
}