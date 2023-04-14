using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using travelH2tour.Models;

namespace travelH2tour.Controllers
{
	public class NhanVienController : Controller
	{
		dbTravelTourDataContext data = new dbTravelTourDataContext();
		// GET: NhanVien

		public List<NHANVIEN> getNhanVien(int count)
		{
			return data.NHANVIENs.OrderByDescending(p => p.SOTOURHOANTHANH).Take(count).ToList();
		}
		public ActionResult loadnhanvien()
		{
			var nhanvien = getNhanVien(4);
			return PartialView(nhanvien);
		}
	}
}