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
	public class QLchitiettourController : Controller
	{

		// GET: AdminH2T/QLchitiettour
		dbTravelTourDataContext data = new dbTravelTourDataContext();
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult DS_CTT(int? page)
		{
			int pagenumber = (page ?? 1);
			int pagesize = 10;
			return View(data.CT_TOURs.ToList().OrderBy(p => p.MAPDT).ToPagedList(pagenumber, pagesize));
		}

	}
}