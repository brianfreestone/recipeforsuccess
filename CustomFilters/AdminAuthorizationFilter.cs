
using System.Web.Mvc;

namespace RecipeForSuccess_mvc.CustomFilters
{
    public class AdminAuthorizationFilterAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Session["CurrentUserIsAdmin"]==null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new {controller="Home", action="Index", area="" }));
            }
        }
    }
}