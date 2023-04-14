using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using travelH2tour.Models;

namespace travelH2tour.Controllers
{
	public class TimKiemController : Controller
	{
		dbTravelTourDataContext data = new dbTravelTourDataContext();
		// GET: TimKiem
		[HttpGet]
		public ActionResult Find()
		{
			return PartialView();
		}


		public ActionResult FindText(FormCollection form)
		{
			var find = data.TOURDLs.Where(p => p.DIADIEM.TENDIADIEM.Contains(form["txtSearch"].ToString()) || p.LOAITOUR.TENLOAITOUR.Contains(form["txtSearch"].ToString())).ToList();
			if (find != null)
			{
				return View(find);
			}
			return View("NotFound404");

		}

	}
}