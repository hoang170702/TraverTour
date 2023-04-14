using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using travelH2tour.Models;

namespace travelH2tour.Controllers
{
	public class ThanhToanOnlineController : Controller
	{
		// GET: ThanhToanOnline
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

		public ActionResult PayOnl()
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

		public ActionResult PaymentWithPaypal()
		{
			APIContext apiContext = PaypalConfiguration.getAPIContext();
			try
			{

				string payerId = Request.Params["PayerID"];
				if (string.IsNullOrEmpty(payerId))
				{

					string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/ThanhToanOnline/PaymentWithPaypal?";

					var guid = Convert.ToString((new Random()).Next(100000));

					var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

					var links = createdPayment.links.GetEnumerator();
					string paypalRedirectUrl = null;
					while (links.MoveNext())
					{
						Links lnk = links.Current;
						if (lnk.rel.ToLower().Trim().Equals("approval_url"))
						{
							paypalRedirectUrl = lnk.href;
						}

					}
					Session.Add(guid, createdPayment.id);
					return Redirect(paypalRedirectUrl);
				}
				else
				{

					var guid = Request.Params["guid"];
					var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

					if (executedPayment.state.ToLower() != "approved")
					{
						throw new Exception();
					}
					else
					{
						PHIEUDATTOUR pdt = new PHIEUDATTOUR();
						KHACHHANG kh = (KHACHHANG)Session["KHACHHANG"];
						var lstGioHang = layGioHang();
						pdt.MAKH = kh.MAKH;
						pdt.NGAYDAT = DateTime.Now;
						pdt.DATHANHTOAN = true;
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
					}

				}
			}
			catch (Exception ex)
			{
				ViewBag.Loi = ex.Message; ViewBag.Error = ex.StackTrace;
				return View("DatTourThatBai");
			}

			return RedirectToAction("DatTourThanhCong");
		}

		private PayPal.Api.Payment payment;
		private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
		{
			var paymentExecution = new PaymentExecution()
			{
				payer_id = payerId
			};
			this.payment = new Payment()
			{
				id = paymentId
			};
			return this.payment.Execute(apiContext, paymentExecution);
		}
		private Payment CreatePayment(APIContext apiContext, string redirectUrl)
		{
			//create itemlist and add item objects to it  
			var itemList = new ItemList()
			{
				items = new List<Item>()
			};

			//Adding Item Details like name, currency, price etc
			if (Session["GioHang"] != null)
			{
				List<GioHang> cart = (List<GioHang>)Session["GioHang"];

				foreach (var item in cart)
				{
					// calculate price based on quantity
					decimal itemPrice = decimal.Parse(item.dThanhTien.ToString()) / item.iSoLuongKH;

					itemList.items.Add(new Item()
					{
						name = item.sTenLoaiTour,
						currency = "USD",
						quantity = item.iSoLuongKH.ToString(),
						price = itemPrice.ToString("0.00"),
						sku = "sku"
					});
				}

				var payer = new Payer()
				{
					payment_method = "paypal"
				};

				// Configure Redirect Urls here with RedirectUrls object  
				var redirUrls = new RedirectUrls()
				{
					cancel_url = redirectUrl + "&Cancel=true",
					return_url = redirectUrl
				};

				// Adding Tax, shipping and Subtotal details  
				var subtotal = TongTien().ToString("0.00");
				var details = new Details()
				{
					tax = "1.00",
					shipping = "2.00",
					subtotal = subtotal,
				};

				//Final amount with details  
				var amount = new Amount()
				{
					currency = "USD",
					total = (decimal.Parse(details.tax) + decimal.Parse(details.shipping) + decimal.Parse(details.subtotal)).ToString("0.00"),
					details = details
				};

				var transactionList = new List<Transaction>();
				// Adding description about the transaction  
				transactionList.Add(new Transaction()
				{
					description = "Transaction description",
					invoice_number = Convert.ToString((new Random()).Next(100000)),
					amount = amount,
					item_list = itemList
				});

				this.payment = new Payment()
				{
					intent = "sale",
					payer = payer,
					transactions = transactionList,
					redirect_urls = redirUrls
				};
			}

			// Create a payment using a APIContext  
			return this.payment.Create(apiContext);
		}



		public ActionResult DatTourThanhCong()
		{
			return View();
		}

		public ActionResult DatTourThatBai()
		{
			return View();
		}
	}
}