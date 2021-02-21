using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RecipeForSuccess.ServiceLayer;
using RecipeForSuccess.ViewModels;

namespace RecipeForSuccess_mvc.APIControllers
{
    public class LiveSearchUserController : ApiController
    {
        IUsersService usersService;

        public LiveSearchUserController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        
        public IHttpActionResult Get(string searchVal, string userName)
        {
            List<UserVM> usernames = usersService.LiveSearchUsers(searchVal, userName);

            return Json(usernames);
        }
    }
}
