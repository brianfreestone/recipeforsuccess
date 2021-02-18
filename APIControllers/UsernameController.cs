using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using RecipeForSuccess.ServiceLayer;

using System.Web.Http;

namespace RecipeForSuccess_mvc.APIControllers
{
    public class UsernameController : ApiController
    {
        IUsersService usersService;

        public UsernameController(IUsersService usersService)
        {
            this.usersService = usersService;
        }


        public string Get(string username)
        {

            if (usersService.UserExistsByUsername(username) == false)
            {
                return "Not Found";
            }
            else
            {
                return "Found";
            }


        }
    }
}