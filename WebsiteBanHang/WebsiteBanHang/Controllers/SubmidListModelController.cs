using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
namespace WebsiteBanHang.Controllers
{
    public class SubmidListModelController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: /SubmidListModel/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(IEnumerable<ChiTietPhieuNhap> ModelList)
        {
            return View();
        }
	}
}