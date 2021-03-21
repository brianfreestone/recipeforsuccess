using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForSuccess.ViewModels;
using RecipeForSuccess.ServiceLayer;
using System.Web.Security;
using RecipeForSuccess_mvc.CustomFilters;

namespace RecipeForSuccess_mvc.Controllers
{
    public class AccountController : Controller
    {

        IUsersService usersService;
        IFriendsService friendsService;


        public AccountController(IUsersService usersService, IFriendsService friendsService)
        {
            this.usersService = usersService;
            this.friendsService = friendsService;
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

                if (usersService.UserExistsByEmail(registerVM.Email))
                {
                    ModelState.AddModelError("Email", "Email already used");
                    return View(registerVM);
                }
                if (usersService.UserExistsByUsername(registerVM.Username))
                {
                    ModelState.AddModelError("Username", "Username already used");
                    return View(registerVM);
                }

                int userID = usersService.InsertUser(registerVM);
                Session["CurrentUserID"] = userID;
                Session["CurrentUserName"] = registerVM.Username;
                Session["CurrentUserIsAdmin"] = false;

                // login user
                FormsAuthentication.SetAuthCookie(registerVM.Username, false);

                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.Error = "All Fields must be completed.";
                ModelState.AddModelError("key", "Fields must be completed");
            }

            return View(registerVM);

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
                UserVM userVM = usersService.GetUserByEmailAndPassword(loginVM.Email, loginVM.Password);
                if (userVM.User_id != 0)
                {
                    // log user in
                    FormsAuthentication.SetAuthCookie(userVM.Username, false);

                    Session["CurrentUserID"] = userVM.User_id;
                    Session["CurrentUserName"] = userVM.Username;
                    Session["CurrentUserIsAdmin"] = userVM.Is_admin;


                    // redirect to admin, not used. We chose to show a gear instead
                    //if (userVM.is_admin)
                    //{
                    //    return RedirectToRoute(new { area = "Admin", controller = "AdminHome", action = "Index" });
                    //}
                    //else
                    //{
                    //    ViewBag.Error = "";
                    return RedirectToAction("Index", "Home");
                    //}
                }
                else
                {
                    ModelState.AddModelError("password", "Incorrect Username/Password");
                    ViewBag.Error = "Incorrect Username/Password";
                }
            }
            else
            {
                ModelState.AddModelError("key", "Fields must be completed");
            }

            return View();

        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Account");

        }

        // POST: Account/AddFriend
        [HttpPost]
        public void AddFriend(int friendId)
        {
            int userID = Convert.ToInt32(Session["CurrentUserID"]);

            friendsService.NewFriendRequest(userID, friendId);

        }

        [Authorize]
        [UserAuthorizationFilterAttribute]
        public ActionResult Username(string username = "")
        {

            // get logged in users userid
            //int user_id = Convert.ToInt32(Session["CurrentUserID"]);

            //string user = Session["CurrentUserName"].ToString();

            string user = User.Identity.Name;

            // get logged in user's user_id
            int user_id = usersService.GetUserIDByUserName(user);
            ViewBag.UserID = user_id;

            // get logged in user's username
            ViewBag.UserName = user;

            // viewbag user's full name
            ViewBag.UserFullName = usersService.GetUserFullNameByUserID(user_id);

            // get viewing's full name
            ViewBag.ViewingFullName = usersService.GetUserFullNameByUserName(username);
            ViewBag.ViewingUserName = username;

            // get viewint's user_id
            int viewing_id = usersService.GetUserIDByUserName(username);
            //Session["ViewingID"] = viewing_id;
            ViewBag.ViewingID = viewing_id;

            string userType = "guest";
            if (username.Equals(user))
            {
                userType = "owner";
            }

            ViewBag.UserType = userType;
            //Session["UserType"] = userType;

            // check if they are friends
            if (userType == "guest")
            {

                string areFriends = friendsService.AreFriends(user_id, viewing_id);

                switch (areFriends)
                {
                    case "none":
                        ViewBag.NotFriends = "True";
                        break;
                    case "pending":
                        ViewBag.NotFriends = "Pending";
                        break;
                }
            }

            // get friend request count
            var friendRequestCount = friendsService.GetFriendRequestCount(Convert.ToInt32(user_id));
            if (friendRequestCount > 0)
            {
                ViewBag.FriendRequestCount = friendRequestCount;
           
                List<UserVM> listRequests = friendsService.GetFriendRequestsByUserID(user_id);
                ViewBag.ListRequests = listRequests;
            }

            //get count of friends
            int friendCount = friendsService.GetFriendCount(viewing_id);

            ViewBag.FriendCount = friendCount;

            return View();
        }

        [HttpGet]
        [UserAuthorizationFilterAttribute]
        public ActionResult ChangePassword()
        {
            int user_id = Convert.ToInt32(Session["CurrentUserID"]);
            EditUserPasswordVM editUserPasswordVM = new EditUserPasswordVM() { User_id = user_id, Password = "", Confirm_password = "" };
            return View(editUserPasswordVM);

        }

        [HttpPost]
        [UserAuthorizationFilterAttribute]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(EditUserPasswordVM editUserPasswordVM)
        {
            if (ModelState.IsValid)
            {
                // check old password
                if (!usersService.ExistingPasswordMatches(editUserPasswordVM.User_id, editUserPasswordVM.OldPassword))
                {
                    ModelState.AddModelError("Password", "Password does not match existing password");
                    return View();
                }

                if (!usersService.PasswordExists(editUserPasswordVM.Password, editUserPasswordVM.User_id))
                {
                    usersService.ChangeUserPassword(editUserPasswordVM);

                    TempData["Success"] = "Password successfully updated!";

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "Password cannot be a previously used password");
                    return View();
                }

            }
            else
            {
                ModelState.AddModelError("pasword", "Passwords must match");
                return View(editUserPasswordVM);
            }
        }
    }
}