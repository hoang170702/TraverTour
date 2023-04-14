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
	public class QLdichvuController : Controller
	{
		dbTravelTourDataContext data = new dbTravelTourDataContext();
		// GET: AdminH2T/QLdichvu
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult DS_DV(int? page)
		{
			int pagenumber = (page ?? 1);
			int pagesize = 10;
			return View(data.DICHVUs.ToList().OrderBy(n => n.MADV).ToPagedList(pagenumber, pagesize));
		}
	}
}