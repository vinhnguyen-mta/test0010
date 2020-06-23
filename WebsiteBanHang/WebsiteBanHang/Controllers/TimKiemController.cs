using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using PagedList;

namespace WebsiteBanHang.Controllers
{
    public class TimKiemController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: /TimKiem/
        [HttpGet ]
        public ActionResult KQTimKiem(string sTuKhoa,int? page)
        {
            //tìm kiếm theo Tên sản phẩm
            var lstsanpham = db.SanPham.Where(s => s.TenSP.Contains(sTuKhoa));

            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //Thực hiện chức năng phân trang
            //Tạo biến số sản phẩm trên trang
            int PageSize = 9;
            //Tạo biến thứ 2: Số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstsanpham.OrderBy(n => n.TenSP).ToPagedList(PageNumber, PageSize));
        }
        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa1)
        {
           //Gọi về hàm get tìm kiếm cho nó tìm kiếm...
            return RedirectToAction("KQTimKiem", new { @sTuKhoa=sTuKhoa1});
        }

        // Làm tìm kiếm theo ajax
        public ActionResult KQTimKiemPartial(string sTuKhoa)
        {
            var lstsanpham = db.SanPham.Where(s => s.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return PartialView(lstsanpham.OrderBy(s=>s.DonGia));
        }
        
	}
}