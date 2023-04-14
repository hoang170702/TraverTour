using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using travelH2tour.Models;
namespace travelH2tour.Areas.AdminH2T.Controllers
{
    public class AdminController : Controller
    {
        dbTravelTourDataContext data = new dbTravelTourDataContext();
        // GET: AdminH2T/Admin
        public ActionResult Index()
        {
            return View();
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
            return data.ADMINs.Count(x => x.TAIKHOAN_ADMIN == tk) > 0;
        }


        [HttpGet]
        public ActionResult dangnhapAdmin()
        {
            return View();
        }

    }

   
}