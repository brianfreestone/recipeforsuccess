using System.Web.Mvc;

namespace RecipeForSuccess_mvc.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute("Delete", "Admin/AdminHome/Delete/{id}", new { controller = "AdminHome", action = "Delete" });
            context.MapRoute( "Admin_default", "Admin/{controller}/{action}/{id}",  new { action = "Index", id = UrlParameter.Optional } );
        }
    }
}