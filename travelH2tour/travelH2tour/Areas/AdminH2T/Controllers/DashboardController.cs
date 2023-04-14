using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace travelH2tour.Areas.AdminH2T.Controllers
{
    public class DashboardController : Controller
    {
        // GET: AdminH2T/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}