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
    public class EmailController : ApiController
    {

        IUsersService usersService;

        public EmailController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

       
        public string Get(string email)
        {
            if (usersService.CheckUserExistsByEmail(email) != false)
            {
                return "Found";
            }
            else
            {
                return "Not Found";
            }
        }
    }
}
