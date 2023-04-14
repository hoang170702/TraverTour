using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using travelH2tour.Models;

namespace travelH2tour.Controllers
{
	public class KhachHangController : Controller
	{
		dbTravelTourDataContext data = new dbTravelTourDataContext();
		// GET: KhachHang

		public List<CT_TOUR> getDanhGia(int count)
		{
			return data.CT_TOURs.Select(p => p).Take(count).ToList();
		}
		public ActionResult DanhGiaCuaKhachHang()
		{
			var Danhgia = getDanhGia(4);
			return PartialView(Danhgia);
		}

		private string mahoamd5(string input)
		{
			using (var md5 = MD5.Create())
			{
				var dulieu = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
				var builder = new StringBuilder();

				for (int i = 0; i < dulieu.Length; i++)
				{
					builder.Append(dulieu[i].ToString("x2"));
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


		[HttpGet] // khi truy cập vào đường dẫn này phương thức sẽ được gọi ra và xử lý yêu cầu 
		public ActionResult dangky()
		{
			return View();
		}
		[HttpPost]
		public ActionResult dangky(FormCollection collection, KHACHHANG kh, HttpPostedFileBase ImageFile)
		{
			var hoten = collection["hotenkh"];
			var taikhoan = collection["tendn"];
			var matkhau = collection["matkhau"];
			var nlmatkhau = collection["nlmatkhau"];
			var email = collection["email"];
			var gt = collection["gioitinh"];
			var ns = string.Format("{0:dd/mm/yyyy}", collection["NGAYSINH"]);
			var cccd = collection["cccd"];

			if (checktk(taikhoan))
			{
				ViewData["loitdn"] = "Tài khoản đã tồn tại!";
				return View();
			}
			else if (checkemail(email))
			{
				ViewData["loiemail"] = "Email đã tồn tại!";
				return View();
			}
			else if (string.IsNullOrEmpty(hoten))
			{
				ViewData["loiht"] = "Họ tên không được trống !";
			}
			else if (hoten.Any(char.IsDigit))
			{
				ViewData["loiht"] = "Họ tên không được nhập số ";
			}
			else if (string.IsNullOrEmpty(taikhoan))
			{
				ViewData["loitdn"] = "Tài khoản không được trống !";
			}
			else if (string.IsNullOrEmpty(matkhau))
			{
				ViewData["loimk"] = "Mật khẩu không được trống !";
			}
			else if (matkhau.Length < 6)
			{
				ViewData["loimk"] = "mật khẩu không được dưới 6 ký tự";
			}
			else if (string.IsNullOrEmpty(nlmatkhau))
			{
				ViewData["loimk"] = "Vui lòng không để trống ô nhập lại mật khẩu !";
			}
			else if (matkhau != nlmatkhau)
			{
				ViewData["loinlmk"] = "xác nhận mật khẩu không đúng";
			}
			else if (!matkhau.Any(char.IsUpper))
			{
				ViewData["loimk"] = "Mật khẩu phải có ít nhất 1 chữ cái in hoa, 1 chữ cái thường, 1 chữ số , 1 ký tự đặc biệt";
			}
			else if (!matkhau.Any(char.IsLower))
			{
				ViewData["loimk"] = "Mật khẩu phải có ít nhất 1 chữ cái in hoa, 1 chữ cái thường, 1 chữ số , 1 ký tự đặc biệt";
			}
			else if (!matkhau.Any(char.IsDigit))
			{
				ViewData["loimk"] = "Mật khẩu phải có ít nhất 1 chữ cái in hoa, 1 chữ cái thường, 1 chữ số , 1 ký tự đặc biệt";
			}
			else if (!matkhau.Any(c => !char.IsLetterOrDigit(c)))
			{
				ViewData["loimk"] = "Mật khẩu phải có ít nhất 1 chữ cái in hoa, 1 chữ cái thường, 1 chữ số , 1 ký tự đặc biệt";
			}
			else if (string.IsNullOrEmpty(nlmatkhau))
			{
				ViewData["loinlmk"] = "Vui lòng nhập lại mật khẩu !";
			}
			else if (string.IsNullOrEmpty(email))
			{
				ViewData["loiemail"] = "Email không được trống !";
			}
			else if (string.IsNullOrEmpty(cccd))
			{
				ViewData["loicccd"] = "cccd không được trống !";
			}
			// khách hàng tải hỉnh ảnh lên 
			else
			{
				kh.TENKH = hoten;
				kh.TAIKHOAN_KH = taikhoan;
				kh.MATKHAU_KH = mahoamd5(matkhau);
				kh.EMAIL = email;
				kh.GIOITINH = gt;
				kh.NGAYSINH = DateTime.Parse(ns);
				kh.CMND_CCCD_KH = cccd;

				if (ImageFile != null)
				{
					string path = Path.Combine(Server.MapPath("~/dbHinhKH"), Path.GetFileName(ImageFile.FileName));
					ImageFile.SaveAs(path);
					kh.HINHKHACHHANG = Path.GetFileName(ImageFile.FileName).ToString();
					ViewBag.FileStatus = "File uploaded successfully.";
				}
				else
				{
					ViewBag.FileStatus = "Error while file uploading."; ;
				}
				data.KHACHHANGs.InsertOnSubmit(kh);
				data.SubmitChanges();
			}
			return RedirectToAction("dangnhap");
		}

		public ActionResult dangnhap()
		{
			return View();
		}

		[HttpPost]
		public ActionResult dangnhap(FormCollection collection)
		{

			var tendn = collection["tdn"];
			var matkhau = mahoamd5(collection["matkhau"]);

			if (string.IsNullOrEmpty(tendn))
			{
				ViewData["loitdn"] = "Vui lòng nhập tên đăng nhập";
			}
			else if (string.IsNullOrEmpty(matkhau))
			{
				ViewData["loimk"] = "Vui lòng nhập mật khẩu";
			}
			else
			{
				var kh = data.KHACHHANGs.SingleOrDefault(p => p.TAIKHOAN_KH == tendn && p.MATKHAU_KH == matkhau);
				if (kh != null)
				{
					Session["KHACHHANG"] = kh;
					Session["TAIKHOAN_KH"] = kh.TENKH;
					return RedirectToAction("Index", "Home");
				}
				else if (!checktk(tendn))
				{
					ViewBag.thongbao = "Tài khoản không tồn tại";
				}

				else
				{
					ViewBag.thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
				}
			}
			return this.dangnhap();

		}

		public ActionResult LichSuBookTour()
		{
			var kh = (KHACHHANG)Session["KHACHHANG"];
			var lichSuBookTour = from ls in data.CT_TOURs where ls.PHIEUDATTOUR.KHACHHANG.MAKH == kh.MAKH select ls;
			return View(lichSuBookTour);
		}

		public ActionResult ThayMatKhau()
		{
			return View();
		}
		[HttpPost]
		public ActionResult ThayMatKhau(FormCollection form)
		{
			var MatKhauCu = mahoamd5(form["matkhaucu"]);
			var MatKhauMoi = form["matkhaumoi"];
			var NhapLaiMKMoi = form["nhaplaimatkhaumoi"];
			var kh = (KHACHHANG)Session["KHACHHANG"];
			var checkKh = data.KHACHHANGs.SingleOrDefault(p => p.TAIKHOAN_KH == kh.TAIKHOAN_KH && p.MATKHAU_KH == kh.MATKHAU_KH);
			if (checkKh != null)
			{

				if (string.IsNullOrEmpty(MatKhauCu))

				{
					ViewData["loimkcu"] = "Mật khẩu Cũ không được trống !";
				}
				else if (MatKhauCu != kh.MATKHAU_KH)
				{
					ViewData["loimkcu"] = "Mật Khẩu Cũ Không Đúng!!!";
				}
				else if (MatKhauMoi.Length < 6)
				{
					ViewData["loimkmoi"] = "mật khẩu mới không được dưới 6 ký tự";
				}
				else if (string.IsNullOrEmpty(NhapLaiMKMoi))
				{
					ViewData["loinhaplaimkmoi"] = "Vui lòng không để trống ô nhập lại mật khẩu !";
				}
				else if (MatKhauMoi != NhapLaiMKMoi)
				{
					ViewData["loinhaplaimkmoi"] = "xác nhận mật khẩu không đúng";
				}
				else if (!MatKhauMoi.Any(char.IsUpper))
				{
					ViewData["loimkmoi"] = "Mật khẩu phải có ít nhất 1 chữ cái in hoa, 1 chữ cái thường, 1 chữ số , 1 ký tự đặc biệt";
				}
				else if (!MatKhauMoi.Any(char.IsLower))
				{
					ViewData["loimkmoi"] = "Mật khẩu phải có ít nhất 1 chữ cái in hoa, 1 chữ cái thường, 1 chữ số , 1 ký tự đặc biệt";
				}
				else if (!MatKhauMoi.Any(char.IsDigit))
				{
					ViewData["loimkmoi"] = "Mật khẩu phải có ít nhất 1 chữ cái in hoa, 1 chữ cái thường, 1 chữ số , 1 ký tự đặc biệt";
				}
				else if (!MatKhauMoi.Any(c => !char.IsLetterOrDigit(c)))
				{
					ViewData["loimkmoi"] = "Mật khẩu phải có ít nhất 1 chữ cái in hoa, 1 chữ cái thường, 1 chữ số , 1 ký tự đặc biệt";
				}
				else if (string.IsNullOrEmpty(NhapLaiMKMoi))
				{
					ViewData["loinhaplaimkmoi"] = "Vui lòng nhập lại mật khẩu !";
				}
				else
				{
					checkKh.MATKHAU_KH = mahoamd5(MatKhauMoi);
					data.SubmitChanges();
					Session.Clear();
					return RedirectToAction("Index", "Home");
				}
			}
			return this.ThayMatKhau();
		}

		public ActionResult DangXuat()
		{
			Session.Clear();
			return RedirectToAction("dangnhap");
		}

	}

}