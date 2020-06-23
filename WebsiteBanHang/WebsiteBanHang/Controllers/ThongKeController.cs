using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class ThongKeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        
        // GET: /ThongKe/
        public ActionResult Index()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();// Số người truy cập từ Aplication đã được tạo
            ViewBag.SoLuongNguoiOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();// Lấy số lượng người đang online
            ViewBag.TongDoangThu = ThongKeTongDoanhThu();
            ViewBag.ThongKeDonHang = ThongKeDonHang();
            ViewBag.ThongKeThanhVien = ThongKeThanhVien();
     
            return View();
        }
        public decimal ThongKeTongDoanhThu()
        {
            decimal TongDoanhThu = db.ChiTietDonDatHang.Sum(s => s.DonGia * s.SoLuong).Value;
            return TongDoanhThu;
        }
        public decimal DoanhThuTheoThang(int thang, int nam)
        {
           // List ra  những đơn hàng nào có tháng, năm tương ứng
            var lstDDH = db.DonDatHang.Where(s => s.NgayDat.Value.Month == thang && s.NgayDat.Value.Year == nam && s.DaThanhToan == true );
            decimal Tong = 0;
            foreach (var i in lstDDH)
            {
                Tong += i.ChiTietDonDatHang.Sum(s => s.DonGia * s.SoLuong).Value;

            }
            return Tong;
        }
        public double ThongKeDonHang()
        {
            double dondathang= db.DonDatHang.Count();
            return dondathang;
        }
        public double ThongKeThanhVien()
        {
            double sltv = db.ThanhVien.Count();
            return sltv;
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