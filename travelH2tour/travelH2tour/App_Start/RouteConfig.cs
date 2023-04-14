using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace travelH2tour
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
			  name: "Admin_default1",
			  url: "Admin/{controller}/{action}/{id}",
			  defaults: new { area = "Admin", controller = "QLKhachHang", action = "DS_KH", id = UrlParameter.Optional },
			  namespaces: new[] { "travelH2tour.Areas.Admin.Controllers" }
		  );
		}
	}
}
