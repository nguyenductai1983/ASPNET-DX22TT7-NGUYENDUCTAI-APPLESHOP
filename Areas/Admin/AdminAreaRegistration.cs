using System.Web.Mvc;

namespace AppleShop.Areas.Admin
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
            context.MapRoute(
        "Admin_Dashboard",
        "Admin",
        new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
        new[] { "AppleShop.Areas.Admin.Controllers" } 
    );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}