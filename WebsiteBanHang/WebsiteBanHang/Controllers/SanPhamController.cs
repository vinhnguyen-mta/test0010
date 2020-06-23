using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using PagedList;
namespace WebsiteBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
       
        // GET: /SanPham/
        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }
           [ChildActionOnly] // chặn ko cho partial chạy.. chỉ được chạy trên action khác có chứa partial này
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }

        //Xây dựng trang xem chi tiết
        public ActionResult ChiTietSanPham(int? id)
        {
            //kiểm tra tham số có truyền vào hay không
            if(id==null)
            {
                return new  HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // nếu không thì truy suất csdl lấy tra sản phẩm tương ứng
            var sanpham = db.SanPham.SingleOrDefault(s => s.MaSP == id && s.DaXoa == false);
            if(sanpham== null)
            {
                return HttpNotFound();
            }

            return View(sanpham);
            
        }
        //xây dựng action load sản phẩm theo loại sản phẩm và theo mã nhà sản xuất
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX,int? page)
        {   // chỉ cho xem sản phẩm khi đăng nhập vào
            //if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            //{
            //    return RedirectToAction("Index","Home");
            //}

            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            /*Load sản phẩm dựa theo 2 tiêu chí là Mã loại sản phẩm và mã nhà sản xuất (2 trường
            trong bảng sản phẩm */
            var lstSP = db.SanPham.Where(s => s.MaLoaiSP == MaLoaiSP && s.MaNSX == MaNSX);
            if (lstSP.Count() == 0)
            {
                //Thông báo nếu như không có sản phẩm đó
                return HttpNotFound();
            }
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //Thực hiện chức năng phân trang
            //Tạo biến số sản phẩm trên trang
            int PageSize = 9;
            //Tạo biến thứ 2: Số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(lstSP.OrderBy(n => n.MaSP).ToPagedList(PageNumber, PageSize));
        }
	}
}