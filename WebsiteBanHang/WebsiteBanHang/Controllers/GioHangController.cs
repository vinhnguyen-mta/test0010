using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;


namespace WebsiteBanHang.Controllers
{
    public class GioHangController : Controller
    {
        //
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //Lấy giỏ hàng 
        public List<IteamGioHang> LayGioHang()
        {
            //2 trường hợp: 1_Giỏ hàng đã tồn tại
            List<IteamGioHang> lstGioHang = Session["GioHang"] as List<IteamGioHang>;  //as để ép kiểu session giỏ hàng về list giỏ hàng
            // nếu session này ko tồn tại thì sẽ trả về cho lstgiohang là null 
            if (lstGioHang == null)
            {// nếu session giỏ hàng chưa tồn tại 
                lstGioHang= new List<IteamGioHang>();// tạo ra 1 List giỏ hàng khác
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //Thêm giỏ hàng thông thường load lại trang (Không hay)
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //kiểm tra sản phẩm có tồn tại trong CSDL hay không
            SanPham sp = db.SanPham.SingleOrDefault(s => s.MaSP == MaSP);
            if (sp == null)
            {
                 // phương thức return về trang 404
                Response.StatusCode = 404;
                return null;
            }
            // nếu sp có tồn tại thì : Lấy giỏ hàng
            List<IteamGioHang> lstGioHang = LayGioHang();
            //kiểm tra 2 bước  nếu như sản phẩm  có tồn tại trong giỏ hàng này r thì cho trường số lượng ++ lên. nếu chưa có thì khởi tạo nó và cho =1
            //th 1: sản phẩm đã tồn tại trong giỏ hàng
            IteamGioHang spCheck = lstGioHang.SingleOrDefault(s => s.MaSP == MaSP);
            if (spCheck != null)
            {
                //kiểm tra số lượng tồn có nhiều hơn số lượng đặt thì mới cho đặt. không thì thông báo hết cmn hàng
                if(sp.SoLuongTon<spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);// phương thức truyền đường dẫn 
            }
           
            IteamGioHang itemgiohang = new IteamGioHang(MaSP);
            if (sp.SoLuongTon < itemgiohang.SoLuong)
            {
                return View("ThongBao");
            }
            lstGioHang.Add(itemgiohang);
            return Redirect(strURL);


        }
       
        //Phương thức  tính tổng số lượng 
        public double TongSoLuong()
        {
            // Lấy Giỏ Hàng
            List<IteamGioHang> lstGioHang = Session["GioHang"] as List<IteamGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(s => s.SoLuong);

        }
        //Tính Tổng Tiền ''
        public decimal TinhTongTien()
        {
            List<IteamGioHang> lstGioHang = Session["GioHang"] as List<IteamGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(s => s.ThanhTien);
            
        }
        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.Tongtien = 0;
                return PartialView();
            }
            else
            {
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.Tongtien = TinhTongTien();
                return PartialView();
            }
           
         
        }
        // GET: /GioHang/
        public ActionResult XemGioHang()
        {
            //Lấy giỏ hàng 
            List<IteamGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        // chỉnh sửa giỏ hàng 
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {

            //kiểm tra session giỏ hàng tồn tại hay chưa 
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPham.SingleOrDefault(s => s.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            //lấy list giỏ hàng từ session
            List<IteamGioHang> listGioHang = LayGioHang();
            //kiểm tra xem sản phẩm có tồn tại trong giỏ hàng hay chưa
            IteamGioHang spCheck = listGioHang.SingleOrDefault(s => s.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lấy List Giỏ Hang tạo giao diện
            ViewBag.GioHang = listGioHang;
            return View(spCheck);
        }
        [HttpPost]
        public ActionResult CapNhapGioHang(IteamGioHang itemGH)
        {
            //kiểm tra số lượng tồn
            SanPham spcheck = db.SanPham.SingleOrDefault(s => s.MaSP == itemGH.MaSP);
            if (spcheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            //Cập Nhập số lượng trong session giỏ hàng 
            //B1: Lấy List<GioHang> từ seesion GioHang
            List<IteamGioHang> listgiohang = LayGioHang();
            //b2: Lấy sp cập nhập từ trong list<GioHang> ra
            IteamGioHang itemGHupdate = listgiohang.Find(s => s.MaSP == itemGH.MaSP);
            // cập nhập lại số lượng
            itemGHupdate.SoLuong = itemGH.SoLuong;
            // Cập nhập lại số tiền
            itemGHupdate.ThanhTien = itemGHupdate.SoLuong * itemGHupdate.DonGia;
            return RedirectToAction("XemGioHang");
        }

        public ActionResult XoaGioHang(int MaSP)
        {

            //kiểm tra session giỏ hàng tồn tại hay chưa 
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPham.SingleOrDefault(s => s.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            //lấy list giỏ hàng từ session
            List<IteamGioHang> listGioHang = LayGioHang();
            //kiểm tra xem sản phẩm có tồn tại trong giỏ hàng hay chưa
            IteamGioHang spCheck = listGioHang.SingleOrDefault(s => s.MaSP == MaSP);
            listGioHang.Remove(spCheck);

            return RedirectToAction("XemGioHang");
        }

        public ActionResult DatHang(KhachHang kh)
        {
            // kiểm tra session giỏ hàng có tồn tại hay chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khang = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                //thêm khách hàng vào bảng khách hàng đối với khách vãng lai 
                khang = kh;
                db.KhachHang.Add(khang);
                db.SaveChanges();
            }
            else
            {
                //Đối với khách hàng là thành viên 
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien; // ép kiểu dữ liệu session tài khoản về thành viên
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                khang.TenKH = tv.HoTen;
                khang.MaThanhVien = tv.MaLoaiTV;
                db.KhachHang.Add(khang);
                db.SaveChanges();
            }
            //Thêm Đơn Hàng
            DonDatHang dh = new DonDatHang();
            dh.NgayDat = DateTime.Now;
            dh.TinhTrangGiaoHang = false;
            dh.DaThanhToan = false;
            dh.UuDai = 0;
            dh.DaHuy = false;
            dh.DaXoa = false;
            db.DonDatHang.Add(dh);
            db.SaveChanges(); 
            //Thêm chi tiết đơn hàng
            List<IteamGioHang> lstGH = LayGioHang();
            foreach (var i in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = dh.MaDDH;
                ctdh.MaSP = i.MaSP;
                ctdh.TenSP = i.TenSP;
                ctdh.DonGia = i.DonGia;
                ctdh.SoLuong = i.SoLuong;
                db.ChiTietDonDatHang.Add(ctdh);
                
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
        public ActionResult ThemGioHangAjax(int MaSP, string strURL)
        {
            //kiểm tra sản phẩm có tồn tại trong CSDL hay không
            SanPham sp = db.SanPham.SingleOrDefault(s => s.MaSP == MaSP);
            if (sp == null)
            {
                // phương thức return về trang 404
                Response.StatusCode = 404;
                return null;
            }
            // nếu sp có tồn tại thì : Lấy giỏ hàng
            List<IteamGioHang> lstGioHang = LayGioHang();
            //kiểm tra 2 bước  nếu như sản phẩm  có tồn tại trong giỏ hàng này r thì cho trường số lượng ++ lên. nếu chưa có thì khởi tạo nó và cho =1
            //th 1: sản phẩm đã tồn tại trong giỏ hàng
            IteamGioHang spCheck = lstGioHang.SingleOrDefault(s => s.MaSP == MaSP);
            if (spCheck != null)
            {
                //kiểm tra số lượng tồn có nhiều hơn số lượng đặt thì mới cho đặt. không thì thông báo hết cmn hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script>alert(\"Sản Phẩm đã hết hàng!\")");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }

            IteamGioHang itemgiohang = new IteamGioHang(MaSP);
            if (sp.SoLuongTon < itemgiohang.SoLuong)
            {
                return Content("<script>alert(\"Sản Phẩm đã hết hàng!\")");
            }
            lstGioHang.Add(itemgiohang);
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView("GioHangPartial");


        }
       
	}
}