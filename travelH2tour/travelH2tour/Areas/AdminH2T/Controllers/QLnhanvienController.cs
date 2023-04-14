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
using System.Web.UI;
using System.Drawing.Printing;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Ajax.Utilities;

namespace travelH2tour.Areas.AdminH2T.Controllers
{
	public class QLnhanvienController : Controller
	{
		// GET: AdminH2T/QLnhanvien
		dbTravelTourDataContext data = new dbTravelTourDataContext();

		public ActionResult Index(int? page)
		{
			int pagenumber = (page ?? 1);
			int pagesize = 10;
			return View(data.NHANVIENs.Where(m => m.TRANGTHAI != 0).ToList().OrderBy(p => p.MANV).ToPagedList(pagenumber, pagesize));
		}

		[HttpGet]
		public ActionResult THEM_NV()
		{

			return View();
		}

		[HttpPost]
		public ActionResult THEM_NV(NHANVIEN nhanvien, HttpPostedFileBase fileUpLoad, FormCollection collection)
		{
			if (ModelState.IsValid == true)
			{
				if (fileUpLoad != null && fileUpLoad.ContentLength > 0)
				{
					var filename = Path.GetFileName(fileUpLoad.FileName);
					var path = Path.Combine(Server.MapPath("~/dbHinhnv/"), filename);
					var fileCount = 1;
					while (System.IO.File.Exists(path))
					{
						filename = Path.GetFileNameWithoutExtension(fileUpLoad.FileName) + "-" + fileCount.ToString() + Path.GetExtension(fileUpLoad.FileName);
						path = Path.Combine(Server.MapPath("~/dbHinhnv/"), filename);
						fileCount++;
					}
					nhanvien.SOTOURHOANTHANH = 0;
					nhanvien.NGAYTAO = DateTime.Now;
					nhanvien.TRANGTHAI = 1;
					fileUpLoad.SaveAs(path);
					nhanvien.HINHNHANVIEN = filename;
					data.NHANVIENs.InsertOnSubmit(nhanvien);
					data.SubmitChanges();
					return RedirectToAction("index");
				}
				else
				{
					ModelState.AddModelError("", "Chọn ảnh đi bạn!");
					return View(nhanvien);
				}

			}
			else
			{
				ModelState.AddModelError("", "Chưa đủ dữ liệu nha");
				return View(nhanvien);
			}
		}
		public ActionResult CHITIET(int id)
		{
			NHANVIEN nv = data.NHANVIENs.FirstOrDefault(p => p.MANV == id);
			ViewBag.MANV = nv.MANV;
			if (nv == null)
			{
				Response.StatusCode = 404;
				return null;
			}
			return View(nv);
		}

		[HttpGet]
		public ActionResult XOA_NV(int id)
		{
			NHANVIEN nv = data.NHANVIENs.SingleOrDefault(p => p.MANV == id);
			ViewBag.MAKH = nv.MANV;
			if (nv == null)
			{
				Response.StatusCode = 404;
				return null;
			}

			return View(nv);
		}

		[HttpPost, ActionName("XOA_NV")]
		public ActionResult Xacnhanxoa(int id)
		{
			NHANVIEN nv = data.NHANVIENs.SingleOrDefault(p => p.MANV == id);
			ViewBag.MANV = nv.MANV;
			if (nv == null)
			{
				Response.StatusCode = 404;
				return null;
			}
			data.NHANVIENs.DeleteOnSubmit(nv);
			data.SubmitChanges();
			return RedirectToAction("Index");
		}

		public ActionResult xoatam(int id)
		{
			NHANVIEN nv = data.NHANVIENs.SingleOrDefault(p => p.MANV == id);
			nv.TRANGTHAI = 0;
			UpdateModel(nv);
			data.SubmitChanges();
			return RedirectToAction("");
		}
		//hiện danh sách trong thùng rác 
		public ActionResult THUNGRAC(int? page)
		{

			int pagenumber = (page ?? 1);
			int pagesize = 10;
			return View(data.NHANVIENs.Where(m => m.TRANGTHAI == 0).ToList().OrderBy(n => n.MANV).ToPagedList(pagenumber, pagesize));
		}

		//Thay đổi trang thái 1->2 , 2->1
		public ActionResult TRANGTHAI(int? id)
		{
			NHANVIEN nv = data.NHANVIENs.SingleOrDefault(p => p.MANV == id);
			//dùng toán tử 3 ngôi biến = biểu thức?biểu thức 2: biểu thức 3; (nếu biểu thức 1 đúng trả về biểu thức 2 cỏn sai thì trả bt3 ) 
			nv.TRANGTHAI = (nv.TRANGTHAI == 2) ? 1 : 2;
			UpdateModel(nv);
			data.SubmitChanges();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult CAPNHAT_NV(int id)
		{
			NHANVIEN nv = data.NHANVIENs.SingleOrDefault(p => p.MANV == id);
			ViewBag.MANV = nv.MANV;
			if (nv == null)
			{
				Response.StatusCode = 404;
				return null;
			}

			return View(nv);
		}

		[HttpPost]
		public ActionResult CAPNHAT_NV(HttpPostedFileBase fileUpLoad, NHANVIEN nhanvien)
		{
			if (fileUpLoad == null)
			{
				ModelState.AddModelError("", "Chưa chọn hình");
				return View(nhanvien);

			}
			else
			{
				if (ModelState.IsValid)
				{
					var filename = Path.GetFileName(fileUpLoad.FileName);
					var path = Path.Combine(Server.MapPath("~/dbHinhnv/"), filename);
					var fileCount = 1;
					if (System.IO.File.Exists(path))
					{
						filename = Path.GetFileNameWithoutExtension(fileUpLoad.FileName) + "-" + fileCount.ToString() + Path.GetExtension(fileUpLoad.FileName);
						path = Path.Combine(Server.MapPath("~/dbHinhnv/"), filename);
						fileCount++;
					}
					else
					{
						fileUpLoad.SaveAs(path);
					}
					nhanvien.HINHNHANVIEN = "~/ dbHinhnv/" + filename;
					UpdateModel(nhanvien);
					data.SubmitChanges();
				}
				return RedirectToAction("index");
			}

		}
	}

}
