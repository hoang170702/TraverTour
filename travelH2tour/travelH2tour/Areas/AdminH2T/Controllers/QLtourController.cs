using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using travelH2tour.Models;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PagedList;
using PagedList.Mvc;

namespace travelH2tour.Areas.AdminH2T.Controllers
{
	public class QLtourController : Controller
	{
		dbTravelTourDataContext data = new dbTravelTourDataContext();
		// GET: AdminH2T/QLtour
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult DS_T(int? page)
		{
			int pagenumber = (page ?? 1);
			int pagesize = 10;
			return View(data.TOURDLs.ToList().OrderBy(p => p.MATOUR).ToPagedList(pagenumber, pagesize));
		}
	}
}