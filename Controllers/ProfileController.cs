using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForSuccess.ServiceLayer;
using RecipeForSuccess.ViewModels;

namespace RecipeForSuccess_mvc.Controllers
{
    public class ProfileController : Controller
    {
        readonly IUsersService usersService;
        readonly IFriendsService friendsService;

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

        [HttpPost]
        public JsonResult DisplayFriendRequests()
        { 
            // get userID
            int userID = Convert.ToInt32(Session["CurrentUserID"]);
            List<UserVM> listRequests = friendsService.GetFriendRequestsByUserID(userID);

            return Json(listRequests);
        }

        [HttpPost]
        public void AcceptFriendRequest(int friendId)
        {
            int userID = Convert.ToInt32(Session["CurrentUserID"]);
            friendsService.AcceptFriendRequest(friendId,  userID);
        }
    }
}