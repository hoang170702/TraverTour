using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using travelH2tour.Models;
using System.IO;
using System.Text;
using PagedList;
using PagedList.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace travelH2tour.Areas.AdminH2T.Controllers
{
	public class QLdiadiemController : Controller
	{
		// GET: AdminH2T/QLdiadiem
		dbTravelTourDataContext data = new dbTravelTourDataContext();

		public ActionResult Index(int? page)
		{
			int pagenumber = (page ?? 1);
			int pagesize = 10;
			return View(data.DIADIEMs.ToList().OrderBy(n => n.MADIADIEM).ToPagedList(pagenumber, pagesize));
		}

	}


}