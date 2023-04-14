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
	public class QLphieudattourController : Controller
	{
		// GET: AdminH2T/QLphieudattour
		dbTravelTourDataContext data = new dbTravelTourDataContext();
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult DS_PDT(int? page)
		{
			int pagenumber = (page ?? 1);
			int pagesize = 10;
			return View(data.PHIEUDATTOURs.ToList().OrderBy(p => p.MAPDT).ToPagedList(pagenumber, pagesize));
		}
	}
}