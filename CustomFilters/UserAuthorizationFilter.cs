
using System.Web.Mvc;

namespace RecipeForSuccess_mvc.CustomFilters
{
    public class UserAuthorizationFilterAttribute : FilterAttribute, IAuthorizationFilter
    {
       public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["CurrentUserName"]==null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
        }
    }
}