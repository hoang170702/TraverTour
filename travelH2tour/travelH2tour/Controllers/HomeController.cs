using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using travelH2tour.Models;

namespace travelH2tour.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{


			return View();
		}

		dbTravelTourDataContext data = new dbTravelTourDataContext();

		public List<BLOG> getBLOGs(int count)
		{
			return data.BLOGs.OrderByDescending(p => p.NGAYDANG).ToList();
		}

		public ActionResult Blog()
		{
			var blog = getBLOGs(3);
			return PartialView(blog);
		}

		public ActionResult DetailBlog(int? id)
		{
			var details = from detail in data.BLOGs where detail.MABLOG == id select detail;
			return View(details.Single());
		}
	}
}