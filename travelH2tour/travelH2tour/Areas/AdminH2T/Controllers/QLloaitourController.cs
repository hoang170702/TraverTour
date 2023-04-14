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

namespace travelH2tour.Areas.AdminH2T.Controllers
{
	public class QLloaitourController : Controller
	{
		// GET: AdminH2T/QLloaitour
		dbTravelTourDataContext data = new dbTravelTourDataContext();
		public ActionResult Index(int? page)
		{
			int pagenumber = (page ?? 1);
			int pagesize = 5;
			return View(data.LOAITOURs.ToList().OrderBy(p => p.MALOAITOUR).ToPagedList(pagenumber, pagesize));
		}

	}
}