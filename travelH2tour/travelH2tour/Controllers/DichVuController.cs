using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using travelH2tour.Models;

namespace travelH2tour.Controllers
{
	public class DichVuController : Controller
	{
		dbTravelTourDataContext data = new dbTravelTourDataContext();
		// GET: DichVu

		public List<DICHVU> getDichVu(int count)
		{
			return data.DICHVUs.OrderByDescending(p => p.GIADV).Take(count).ToList();
		}
		public ActionResult loadDichVu()
		{
			var dichvu = getDichVu(3);
			return PartialView(dichvu);
		}

		public ActionResult TrangDichVu()
		{
			var dichvu = data.DICHVUs.Select(p => p).ToList();
			return View(dichvu);
		}
	}
}