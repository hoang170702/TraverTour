using System.Web.Mvc;

namespace travelH2tour.Areas.AdminH2T
{
    public class AdminH2TAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AdminH2T";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AdminH2T_default",
                "AdminH2T/{controller}/{action}/{id}",
                new { Controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}