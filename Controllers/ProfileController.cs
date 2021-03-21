using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForSuccess.ServiceLayer;
using RecipeForSuccess.ViewModels;
using RecipeForSuccess_mvc.CustomFilters;

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
        [UserAuthorizationFilterAttribute]
        public void AddFriend(string friend)
        {
            string loggedInUser = User.Identity.Name;
            // get logged in user's user_id
            int id1 = usersService.GetUserIDByUserName(loggedInUser);
         
            int id2 = usersService.GetUserIDByUserName(friend);
            friendsService.NewFriendRequest(id1, id2);
        }

        [HttpPost]
        public JsonResult DisplayFriendRequests()
        {
            string loggedInUser = User.Identity.Name;
            // get userID
            int userID = usersService.GetUserIDByUserName(loggedInUser);
            List<UserVM> listRequests = friendsService.GetFriendRequestsByUserID(userID);

            return Json(listRequests);
        }

        [HttpPost]
        [UserAuthorizationFilterAttribute]
        public void AcceptFriendRequest(int friendId)
        {
            string loggedInUser = User.Identity.Name;
            // get userID
            int userID = usersService.GetUserIDByUserName(loggedInUser);
            friendsService.AcceptFriendRequest(friendId,  userID);
        }

        [HttpPost]
        [UserAuthorizationFilter]
        public void DeclineFriendRequest(int friendId)
        {
            string loggedInUser = User.Identity.Name;
            // get userID
            int userID = usersService.GetUserIDByUserName(loggedInUser);
            bool declined = friendsService.DeclineFriendRequest(friendId, userID);
            
            // create notification 
            if (declined)
            {
                TempData["Success"] = "Request Declined";
            }
            else
            {
                TempData["Error"] = "Request Declined";
            }
        }

        [HttpPost]
        public JsonResult LiveSearchUser(string searchVal, string userName)
        { 
        List<UserVM> usernames = usersService.LiveSearchTenUsers(searchVal, userName);


            return Json(usernames);
        }
    }
}