using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForSuccess.ServiceLayer;

namespace RecipeForSuccess_mvc.Controllers
{
    public class ProfileController : Controller
    {
        IUsersService usersService;
        IFriendsService friendsService;

        public ProfileController(IUsersService usersService, IFriendsService friendsService)
        {
            this.usersService = usersService;
            this.friendsService = friendsService;
        }
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void AddFriend(string friend)
        {
            int id1 = Convert.ToInt32(Session["CurrentUserID"]);
            int id2 = usersService.GetUserIDByUserName(friend);
            friendsService.NewFriendRequest(id1, id2);
        }
    }
}