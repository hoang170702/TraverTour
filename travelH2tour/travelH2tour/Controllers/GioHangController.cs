using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using travelH2tour.Models;

namespace travelH2tour.Controllers
{
	public class GioHangController : Controller
	{
		// GET: GioHang
		dbTravelTourDataContext data = new dbTravelTourDataContext();

		public List<GioHang> layGioHang()
		{
			var lstGiohang = Session["GioHang"] as List<GioHang>;
			if (lstGiohang == null)
			{
				lstGiohang = new List<GioHang>();
				Session["GioHang"] = lstGiohang;
			}
			return lstGiohang;
		}

		public ActionResult ThemGioHang(int id, string strURL)
		{
			var lstGioHang = layGioHang();
			var sanphamGioHang = lstGioHang.Find(p => p.iMaTour == id);
			if (sanphamGioHang == null)
			{
				sanphamGioHang = new GioHang(id);
				lstGioHang.Add(sanphamGioHang);
				return Redirect(strURL);
			}
			else
			{
				ViewBag.ThongBao = "Tour da co trong gio hang";
				return Redirect(strURL);
			}
		}

		public ActionResult BookTourNow(int id)
		{
			var lstGioHang = layGioHang();
			var sanphamGioHang = lstGioHang.Find(p => p.iMaTour == id);
			if (sanphamGioHang == null)
			{
				sanphamGioHang = new GioHang(id);
				lstGioHang.Add(sanphamGioHang);
				return RedirectToAction("Giohang");
			}
			else
			{
				ViewBag.ThongBao = "Tour da co trong gio hang";
				return RedirectToAction("Giohang");
			}
		}

		private int TongSoLuong()
		{
			int sum = 0;
			var lstGioHang = Session["GioHang"] as List<GioHang>;
			if (lstGioHang != null)
			{
				sum += lstGioHang.Count;
			}
			return sum;
		}

		private double TongTien()
		{
			double sum = 0;
			var lstGioHang = Session["GioHang"] as List<GioHang>;
			if (lstGioHang != null)
			{
				sum += lstGioHang.Sum(p => p.dThanhTien);
			}
			return sum;
		}

		public ActionResult Giohang()
		{
			var lstGiohang = layGioHang();
			if (lstGiohang.Count == 0)
			{
				return RedirectToAction("Index", "Home");
			}
			ViewBag.TongSoTour = TongSoLuong();
			ViewBag.TongTien = TongTien();
			return View(lstGiohang);
		}

		public ActionResult GiohangPartial()
		{
			ViewBag.TongSoTour = TongSoLuong();
			ViewBag.TongTien = TongTien();
			return PartialView();
		}

		public ActionResult Delete(int? id)
		{
			var lstGioHang = layGioHang();
			var check = lstGioHang.SingleOrDefault(p => p.iMaTour == id);
			if (check != null)
			{
				lstGioHang.Remove(check);
			}
			if (lstGioHang != null)
			{
				return RedirectToAction("GioHang");
			}
			return RedirectToAction("Index", "Home");
		}

		public ActionResult Update(int? id, FormCollection form)
		{
			var lstGioHang = layGioHang();
			var check = lstGioHang.SingleOrDefault(p => p.iMaTour == id);
			if (check != null)
			{
				check.iSoLuongKH = int.Parse(form["txtSoLuong"].ToString());
				check.dNgayDi = DateTime.Parse(form["dateNgayDi"].ToString());
				check.dNgayVe = DateTime.Parse(form["dateNgayVe"].ToString());
			}
			return RedirectToAction("GioHang");
		}

		public ActionResult DeleteAll()
		{
			var lstGioHang = layGioHang();
			lstGioHang.Clear();
			return RedirectToAction("Index", "Home");
		}

		public ActionResult Pay()
		{
			if (Session["KHACHHANG"] == null)
			{
				return RedirectToAction("dangnhap", "KhachHang");
			}
			if (Session["GioHang"] == null)
			{
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		[HttpPost]
		public ActionResult Pay(FormCollection form)
		{
			PHIEUDATTOUR pdt = new PHIEUDATTOUR();
			KHACHHANG kh = (KHACHHANG)Session["KHACHHANG"];
			var lstGioHang = layGioHang();
			pdt.MAKH = kh.MAKH;
			pdt.NGAYDAT = DateTime.Now;
			pdt.DATHANHTOAN = false;
			data.PHIEUDATTOURs.InsertOnSubmit(pdt);
			data.SubmitChanges();

			foreach (var item in lstGioHang)
			{
				CT_TOUR ctTour = new CT_TOUR();

				ctTour.MAPDT = pdt.MAPDT;
				ctTour.MATOUR = item.iMaTour;
				ctTour.NGAYDI = item.dNgayDi;
				ctTour.NGAYVE = item.dNgayVe;
				ctTour.SOLUONGKHACH = item.iSoLuongKH;
				ctTour.DONGIA = (decimal)item.dDonGia;
				data.CT_TOURs.InsertOnSubmit(ctTour);

			}
			data.SubmitChanges();
			Session["GioHang"] = null;
			return RedirectToAction("XacNhanDatTour");
		}

		public ActionResult XacNhanDatTour()
		{
			return View();
		}
	}
}