using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;


namespace WebsiteBanHang.Controllers
{
    public class QuanLyPhieuNhapController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //
        // GET: /QuanLyPhieuNhap/
        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCap;
            ViewBag.ListSanPham = db.SanPham;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model,IEnumerable<ChiTietPhieuNhap> lstModel)
        {
            ViewBag.MaNCC = db.NhaCungCap;
            ViewBag.ListSanPham = db.SanPham;
            model.NgayNhap = DateTime.Now;
            model.DaXoa = false;
            db.PhieuNhap.Add(model);
            db.SaveChanges();
            SanPham sp;
            // Save change để lấy mã phiếu nhập gán cho Chi Tiếp Phiếu nhập
            foreach (var i in lstModel)
            {
                // Gán mã phiếu nhập cho tất cả các chi tiết phiếu nhập
                i.MaPN = model.MaPN;
                //Cập nhập số lượng tồn 
                sp = db.SanPham.SingleOrDefault(s => s.MaSP == i.MaSP);
                sp.SoLuongTon += i.SoLuongNhap;
            }
            db.ChiTietPhieuNhap.AddRange(lstModel);// Phương thức Add một list 
            db.SaveChanges();
            return View();
        }
        [HttpGet]
        public ActionResult DSSPHetHang()
        {
            //Danh sách sản phẩm gần hết hàng với số lượng tồn bé hơn hoặc bằng 5
            var lstSP = db.SanPham.Where(n => n.DaXoa == false && n.SoLuongTon <= 5);
            return View(lstSP);

        }
        //Tạo 1 view phục vụ cho việc nhập từng sản phẩm
        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            //Tương tự như trang chỉnh sửa sản phẩm nhưng ta không cần phải show hết các thuộc tính 
            //Chỉ thuộc tính nào cần thiết mà thôi đó là số lượng tồn hình ảnh... thông tin hiển thị cần thiết
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);

        }
        // Xử lý nhập hàng từng sản phẩm
        [HttpPost]
        public ActionResult NhapHangDon(PhieuNhap model, ChiTietPhieuNhap ctpn)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            //Sau khi các bạn đã kiểm tra tất cả dữ liệu đầu vào
            //Gán đã xóa: False
            model.NgayNhap = DateTime.Now;
            model.DaXoa = false;
            db.PhieuNhap.Add(model);
            db.SaveChanges();
            //SaveChanges để lấy được mã phiếu nhập gán cho lstChiTietPhieuNhap
            ctpn.MaPN = model.MaPN;
            //Cập nhật tồn 
            SanPham sp = db.SanPham.Single(n => n.MaSP == ctpn.MaSP);
            sp.SoLuongTon += ctpn.SoLuongNhap;
            db.ChiTietPhieuNhap.Add(ctpn);
            db.SaveChanges();
            return View(sp);

        }
        //Giải phóng biến cho vùng nhớ
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