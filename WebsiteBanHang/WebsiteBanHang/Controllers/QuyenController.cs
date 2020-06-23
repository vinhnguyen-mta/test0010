using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using System.Net.Mail;
using System.Net;
namespace WebsiteBanHang.Controllers
{
    [Authorize(Roles = "QuanLyQuyen")]
    public class QuyenController : Controller
    {
       
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: /Quyen/
        public ActionResult Index()
        {
            return View(db.Quyen.OrderBy(n => n.TenQuyen));
        }
        [HttpGet]
        public ActionResult ThemQuyen()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ThemQuyen(Quyen quyen)
        {
            if (ModelState.IsValid)
            {
                db.Quyen.Add(quyen);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}