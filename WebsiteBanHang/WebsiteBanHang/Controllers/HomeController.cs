using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;
namespace WebsiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: /Home/
        public ActionResult Index()
        {
            var lstDTM = db.SanPham.Where(s => s.MaLoaiSP == 1 && s.Moi == 1 && s.DaXoa == false);
            ViewBag.listDTM = lstDTM;
            var lstLTM = db.SanPham.Where(s => s.MaLoaiSP == 3 && s.Moi == 1 && s.DaXoa == false);
            ViewBag.listLTM = lstLTM;
            var lstMTB = db.SanPham.Where(s => s.MaLoaiSP == 2 && s.Moi == 1 && s.DaXoa == false);
            ViewBag.lstMTBM = lstMTB;
            return View();
        }
        //Tạo menu động
        public ActionResult MenuPartial()
        {
            var listsp = db.SanPham;
            return PartialView(listsp);
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            // kiểm tra capstra hợp lệ
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                if(ModelState.IsValid){ // kiểm tra tất cả các thuộc tính trong form mới cho save change 
                    ViewBag.ThongBao = "Thêm Thành Công";
                    db.ThanhVien.Add(tv);
                db.SaveChanges();
                }
                else
                {
                    ViewBag.ThongBao = "Thêm Thất Bại";
                }
                return View();
             
            }
                ViewBag.ThongBao = "Sai Cmn mã rồi bạn ơi";
            

            //Thêm Khách Hàng vào cơ sở dữ liệu

            return View();
        }
        [HttpGet]
        public ActionResult DangKy1()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky1(ThanhVien tv)
        {
            return View();
        }
        public List<string> LoadCauHoi()
        {
            List<string> lstCauhoi = new List<string>();
            lstCauhoi.Add("Con vật mà bạn yêu thích?");
            lstCauhoi.Add("Ai Là trùm cuối trong truyện conan?");
            lstCauhoi.Add("Tuổi thọ của con thằn lằn là bao nhiêu ?");
            lstCauhoi.Add("Ca sĩ mà bạn yêu cmn thích");
            lstCauhoi.Add("Bạn thường ỉa vào buổi nào trong ngày");
            lstCauhoi.Add("phim hậu duệ mặt trời có bao nhiêu tập?");
            lstCauhoi.Add("bạn có bao giờ yêu cô giáo mình chưa?");
            return lstCauhoi;

        }
        //xây dựng action đăng nhập
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            //truy vấn kiểm tra đăng nhập lấy thông tin thành viên
            ThanhVien tv = db.ThanhVien.SingleOrDefault(s => s.TaiKhoan == sTaiKhoan && s.MatKhau == sMatKhau);
            if (tv != null)
            {
                var lis_quyen = db.LoaiThanhVien_Quyen.Where(s => s.MaLoaiTV == tv.MaLoaiTV);
                string Quyen = "";
                if (lis_quyen.Count() != 0)
                {
                    foreach (var i in lis_quyen)
                    {
                        Quyen += i.Quyen.MaQuyen + ",";// lấy quyền trong bảng chi tiết quyền và loại thành viên "DangKy,QuanLyDonHang,QuanLySanPham"
                        Quyen = Quyen.Substring(0, Quyen.Length - 1);// cắt dấu , cuối cùng
                        PhanQuyen(tv.MaThanhVien.ToString(), Quyen);
                        Session["TaiKhoan"] = tv;
                        return Content("<script>window.location.reload();</script>"); // reload lại trang
                    }
                }
        

            }
          
                return Content("Tài khoản hoặc mật khẩu không đúng!"); 
            
           

        }
        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                                          TaiKhoan, //user
                                          DateTime.Now, //Thời gian bắt đầu
                                          DateTime.Now.AddHours(3), //Thời gian kết thúc
                                          false, //Ghi nhớ?
                                          Quyen, // "DangKy,QuanLyDonHang,QuanLySanPham"
                                          FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
       // Tạo Trang ngăn chặn quyền truy cập

        public ActionResult LoiPhanQuyen()
        {
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index"); 
        }

    }
}