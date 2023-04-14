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
using System.Web.Helpers;
using System.Web.UI.WebControls;

namespace travelH2tour.Areas.AdminH2T.Controllers
{
	public class QLkhachhangController : Controller
	{
		dbTravelTourDataContext data = new dbTravelTourDataContext();
		// GET: AdminH2T/QLkhachhang
		public ActionResult Index(int? page)
		{

			int pagenumber = (page ?? 1);
			int pagesize = 10;
			return View(data.KHACHHANGs.Where(m => m.TRANGTHAI != 0).ToList().OrderBy(n => n.MAKH).ToPagedList(pagenumber, pagesize));
		}


		private string mahoamd5(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return string.Empty;
			}

			using (var md5 = MD5.Create())
			{
				byte[] inputBytes = Encoding.UTF8.GetBytes(input);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < hashBytes.Length; i++)
				{
					builder.Append(hashBytes[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}
		private bool checktk(string tk) // check tk tồn tại chưa
		{
			return data.KHACHHANGs.Count(x => x.TAIKHOAN_KH == tk) > 0;
		}

		private bool checkemail(string em) // check tk tồn tại chưa
		{
			return data.KHACHHANGs.Count(x => x.EMAIL == em) > 0;
		}




		[HttpGet]
		public ActionResult THEM_KH()
		{
			return View();
		}

		[HttpPost]

		public ActionResult THEM_KH(KHACHHANG khachhang, HttpPostedFileBase fileUpLoad)
		{
			if (ModelState.IsValid)
			{
				if (fileUpLoad != null && fileUpLoad.ContentLength > 0)
				{
					var filename = Path.GetFileName(fileUpLoad.FileName);
					var path = Path.Combine(Server.MapPath("~/dbHinhKH"), filename);
					var fileCount = 1;
					while (System.IO.File.Exists(path))
					{
						filename = Path.GetFileNameWithoutExtension(fileUpLoad.FileName) + "-" + fileCount.ToString() + Path.GetExtension(fileUpLoad.FileName);
						path = Path.Combine(Server.MapPath("~/dbHinhKH"), filename);
						fileCount++;
					}
					khachhang.NGAYTAO = DateTime.Now;
					khachhang.TRANGTHAI = 1;
					fileUpLoad.SaveAs(path);
					khachhang.HINHKHACHHANG = filename;
					data.KHACHHANGs.InsertOnSubmit(khachhang);
					data.SubmitChanges();
					return RedirectToAction("index");
				}
				else
				{
					ModelState.AddModelError("fileUpLoad", "Please upload an image file.");
				}

			}
			return View(khachhang);

		}



		//hiển thị
		public ActionResult CHITIET(int id)
		{
			KHACHHANG kh = data.KHACHHANGs.FirstOrDefault(p => p.MAKH == id);
			ViewBag.MAKH = kh.MAKH;
			if (kh == null)
			{
				Response.StatusCode = 404;
				return null;
			}
			return View(kh);
		}



		// xóa khỏi CSDL
		[HttpGet]
		public ActionResult XOA_KH(int id)
		{
			KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(p => p.MAKH == id);
			ViewBag.MAKH = kh.MAKH;
			if (kh == null)
			{
				Response.StatusCode = 404;
				return null;
			}

			return View(kh);
		}


		[HttpPost, ActionName("XOA_KH")]
		public ActionResult Xacnhanxoa(int id)
		{
			KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(p => p.MAKH == id);
			ViewBag.MAKH = kh.MAKH;
			if (kh == null)
			{
				Response.StatusCode = 404;
				return null;
			}
			data.KHACHHANGs.DeleteOnSubmit(kh);
			data.SubmitChanges();
			return RedirectToAction("Index");
		}

		//Xóa vào thùng rác set trangthai = 0
		public ActionResult Xoatam(int id)
		{
			KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(p => p.MAKH == id);
			kh.TRANGTHAI = 0;
			UpdateModel(kh);
			data.SubmitChanges();
			return RedirectToAction("");
		}



		//Thay đổi trang thái 1->2 , 2->1
		public ActionResult TRANGTHAI(int? id)
		{
			KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(p => p.MAKH == id);
			//dùng toán tử 3 ngôi biến = biểu thức?biểu thức 2: biểu thức 3; (nếu biểu thức 1 đúng trả về biểu thức 2 cỏn sai thì trả bt3 ) 
			kh.TRANGTHAI = (kh.TRANGTHAI == 2) ? 1 : 2;
			UpdateModel(kh);
			data.SubmitChanges();
			return RedirectToAction("Index");
		}



		//Khôi phục cho trạng thái = 2




		//hiện danh sách trong thùng rác 
		public ActionResult THUNGRAC(int? page)
		{

			int pagenumber = (page ?? 1);
			int pagesize = 10;
			return View(data.KHACHHANGs.Where(m => m.TRANGTHAI == 0).ToList().OrderBy(n => n.MAKH).ToPagedList(pagenumber, pagesize));
		}


		[HttpGet]
		public ActionResult CAPNHAT_KH(int id)
		{
			KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(p => p.MAKH == id);
			ViewBag.MAKH = kh.MAKH;
			if (kh == null)
			{
				Response.StatusCode = 404;
				return null;
			}

			return View(kh);
		}

		[HttpPost]

		public ActionResult CAPNHAT_KH(KHACHHANG khachhang, HttpPostedFileBase fileUpLoad)
		{
			if (ModelState.IsValid)
			{
				if (fileUpLoad != null && fileUpLoad.ContentLength > 0)
				{
					var filename = Path.GetFileName(fileUpLoad.FileName);
					var path = Path.Combine(Server.MapPath("~/dbHinhKH"), filename);
					var fileCount = 1;
					while (System.IO.File.Exists(path))
					{
						filename = Path.GetFileNameWithoutExtension(fileUpLoad.FileName) + "-" + fileCount.ToString() + Path.GetExtension(fileUpLoad.FileName);
						path = Path.Combine(Server.MapPath("~/dbHinhKH"), filename);
						fileCount++;
					}
					fileUpLoad.SaveAs(path);
					khachhang.HINHKHACHHANG = filename;
					UpdateModel(khachhang);
					data.SubmitChanges();
					return RedirectToAction("index");
				}
				else
				{
					ModelState.AddModelError("fileUpLoad", "Please upload an image file.");
				}

			}
			return View(khachhang);
		}
	}
}