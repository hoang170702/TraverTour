using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace travelH2tour.Models
{
	public class GioHang
	{
		dbTravelTourDataContext data = new dbTravelTourDataContext();

		public int iMaTour { set; get; }
		public string sTenLoaiTour { set; get; }
		public string sTenDiaDiem { set; get; }
		public string sHinhTour { set; get; }
		public DateTime dNgayDi { set; get; }
		public DateTime dNgayVe { set; get; }
		public Double dDonGia { set; get; }
		public int iSoLuongKH { set; get; }

		public int Time()
		{
			var NgayDi = Convert.ToDateTime(dNgayDi);
			var NgayVe = Convert.ToDateTime(dNgayVe);
			TimeSpan TimeDay = NgayVe - NgayDi;
			return TimeDay.Days;

		}
		public Double dThanhTien
		{
			get => Double.Parse(Time().ToString()) * (dDonGia * iSoLuongKH);
		}

		public GioHang(int MaTour)
		{
			iMaTour = MaTour;
			var tour = data.TOURDLs.SingleOrDefault(p => p.MATOUR == iMaTour);
			sTenLoaiTour = tour.LOAITOUR.TENLOAITOUR;
			sTenDiaDiem = tour.DIADIEM.TENDIADIEM;
			sHinhTour = tour.HINHTOUR;
			dDonGia = double.Parse(tour.TIENTOUR.ToString());
			dNgayDi = DateTime.Now;
			dNgayVe = DateTime.Now.AddDays(3);
			iSoLuongKH = 1;
		}
	}
}