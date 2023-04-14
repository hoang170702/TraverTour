using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using travelH2tour.Models;

namespace travelH2tour.Controllers
{
	public class DiaDiemController : Controller
	{
		dbTravelTourDataContext data = new dbTravelTourDataContext();
		// GET: DiaDiem

		public List<DIADIEM> GetDIADIEMs(int count)
		{
			return data.DIADIEMs.Select(p => p).Take(count).ToList();
		}
		public ActionResult HienThiDiaDiem()
		{
			var diadiem = GetDIADIEMs(6);
			return PartialView(diadiem);
		}

		public ActionResult CT_DiaDiem(int id)
		{
			var Tour = from tour in data.TOURDLs where tour.MADIADIEM == id select tour;
			return View(Tour);
		}

		public ActionResult Nav_DiaDiem()
		{
			var diadiem = data.DIADIEMs.Select(p => p).ToList();
			return PartialView(diadiem);
		}
	}
}