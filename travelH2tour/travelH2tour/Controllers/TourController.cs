using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using travelH2tour.Models;

namespace travelH2tour.Controllers
{
	public class TourController : Controller
	{
		dbTravelTourDataContext data = new dbTravelTourDataContext();
		// GET: Tour
		public List<TOURDL> getTour(int count)
		{
			return data.TOURDLs.OrderByDescending(p => p.TIENTOUR).Take(count).ToList();
		}
		public ActionResult tour()
		{
			var Tour = getTour(6);
			return PartialView(Tour);
		}

		public ActionResult TrangTour(int? page)
		{
			int pagenumber = (page ?? 1);
			int pageSize = 3;
			var ctTour = data.TOURDLs.OrderByDescending(p => p.TIENTOUR).ToList();
			return View(ctTour.ToPagedList(pagenumber, pageSize));
		}

		public ActionResult Detailstours(int id)
		{
			var details = from detail in data.TOURDLs where detail.MATOUR == id select detail;
			return View(details.Single());
		}


		[HttpGet]
		public ActionResult toursReviews(int? idTour, int? idPDT)
		{
			var review = data.CT_TOURs.SingleOrDefault(p => p.MATOUR == idTour && p.PHIEUDATTOUR.MAPDT == idPDT);
			return View(review);
		}


		[HttpPost]
		public ActionResult toursReviews(CT_TOUR ct)
		{
			if (ModelState.IsValid)
			{
				var length = ct.DANHGIATUKHACHHANG.Length;
				if (length > 4000)
				{
					ViewData["ErrorMessage"] = "Không Được Quá 4000 kí tự";
				}
				else
				{
					var FindTour = data.CT_TOURs.SingleOrDefault(p => p.MATOUR == ct.MATOUR && p.PHIEUDATTOUR.MAPDT == ct.MAPDT);
					FindTour.DANHGIATUKHACHHANG = ct.DANHGIATUKHACHHANG;
					UpdateModel(FindTour);
					data.SubmitChanges();
					return RedirectToAction("Index", "Home");
				}

			}
			return RedirectToAction("toursReviews");
		}
	}
}