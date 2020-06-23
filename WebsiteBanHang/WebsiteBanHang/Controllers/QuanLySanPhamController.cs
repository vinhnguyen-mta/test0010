using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using System.IO;
using System.Net;
namespace WebsiteBanHang.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: /QuanLySanPham/
        public ActionResult Index()
        {
            return View(db.SanPham.Where(s=>s.DaXoa==false).OrderByDescending(s=>s.MaSP));
        }

        [HttpGet]
        public ActionResult TaoMoi()
        {
            // Load Dropdow list  nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(s => s.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(s => s.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(s => s.MaNSX), "MaNSX", "TenNSX");
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp,HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(s => s.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(s => s.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(s => s.MaNSX), "MaNSX", "TenNSX");
            int loi = 0;
            // Kiểm tra hinh ảnh  có tồn tại trong csdl hay chưa?
            for (int i = 0; i < HinhAnh.Count(); i++)
            {//kiểm tra định dạng hình ảnh
                if (HinhAnh[i] != null) 
                { 
                if (HinhAnh[i].ContentLength > 0 )
                {
                    if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpg")
                    {
                        ViewBag.upload += "Hình Ảnh" + i + "Không Hợp Lệ<br />";
                        loi++;
                    }
                    else
                    {
                        //Lấy tên hình ảnh
                        var filename = Path.GetFileName(HinhAnh[0].FileName);
                        //Lấy hình Ảnh chuyển vào thư mục hình ảnh
                        var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), filename);
                        // nếu thư mục hình ảnh có rồi thì xuất ra thông báo
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.upload1 = "Hình Ảnh Đã Tồn Tại";
                            loi++;
                           
                            
                        }

                    }
                   }
                }

            }

            if (loi > 0)
            {
                return View(sp);
            }
            sp.HinhAnh = HinhAnh[0].FileName;
            sp.HinhAnh1 = HinhAnh[1].FileName;
            sp.HinhAnh2 = HinhAnh[2].FileName;
            sp.HinhAnh3 = HinhAnh[3].FileName;
            sp.HinhAnh4 = HinhAnh[4].FileName;


                //for (int i = 0; i <=HinhAnh.Length; i++)
                //{
                //    if(HinhAnh[i]==null)
                //    {
                //        HinhAnh[i] = null;
                //    }
                //    else
                //    {
                //        if (HinhAnh[i].ContentLength > 0)
                //        {
                //            if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpg")

                //            {
                //                ViewBag.upload += "Hình Ảnh" + i + "Không Hợp Lệ<br />";
                //                loi++;
                //            }
                //            //Lấy hình ảnh đưa vào thư mục  HinhAnhSP
                //                if (i == 0)
                //                {
                //                     // Lấy tên hình ảnh
                //                    var filename = Path.GetFileName(HinhAnh[i].FileName);
                //                    //Lấy hình ảnh truyền vào thư mục hình ảnh sp
                //                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), filename);
                //                    // nếu thư mục hình ảnh có rồi thì xuất ra thông báo
                //                    if (System.IO.File.Exists(path))
                //                    {
                //                        ViewBag.upload1 = "Hình Ảnh Đã Tồn Tại";
                //                        return View();
                //                        loi++;
                //                    }
                //                    HinhAnh[i].SaveAs(path);
                //                    sp.HinhAnh = filename;
                //                }
                //                if (i == 1)
                //                {var filename1 = Path.GetFileName(HinhAnh[i].FileName);
                //                    //Lấy hình ảnh truyền vào thư mục hình ảnh sp
                //                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), filename1);
                //                    if (System.IO.File.Exists(path))
                //                    {
                //                        ViewBag.upload = "Hình Ảnh Đã Tồn Tại";
                //                        loi++;
                //                        return View();
                //                    }
                //                    HinhAnh[i].SaveAs(path);
                //                    sp.HinhAnh1 = filename1;

                //                }
                //                if (i == 2)
                //                {

                //                    var filename2 = Path.GetFileName(HinhAnh[i].FileName);
                //                    //Lấy hình ảnh truyền vào thư mục hình ảnh sp
                //                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), filename2);
                //                    if (System.IO.File.Exists(path))
                //                    {
                //                        ViewBag.upload = "Hình Ảnh Đã Tồn Tại";
                //                        loi++;
                //                        return View();
                //                    }
                //                    HinhAnh[i].SaveAs(path);
                //                    sp.HinhAnh2 = filename2;
                //                }
                //                if (i == 3)
                //                {

                //                    var filename3 = Path.GetFileName(HinhAnh[i].FileName);
                //                    //Lấy hình ảnh truyền vào thư mục hình ảnh sp
                //                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), filename3);
                //                    if (System.IO.File.Exists(path))
                //                    {
                //                        ViewBag.upload = "Hình Ảnh Đã Tồn Tại";
                //                        loi++;
                //                        return View();
                //                    }
                //                    HinhAnh[i].SaveAs(path);
                //                    sp.HinhAnh3 = filename3;
                //                }

                //                if (i == 4)
                //                {

                //                    var filename4 = Path.GetFileName(HinhAnh[i].FileName);
                //                    //Lấy hình ảnh truyền vào thư mục hình ảnh sp
                //                    var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), filename4);
                //                    if (System.IO.File.Exists(path))
                //                    {
                //                        ViewBag.upload = "Hình Ảnh Đã Tồn Tại";
                //                        loi++;
                //                        return View();
                //                    }
                //                    HinhAnh[i].SaveAs(path);
                //                    sp.HinhAnh4 = filename4;
                //                }
                //            }
                //        }



                //}
             
            db.SanPham.Add(sp);
            db.SaveChanges();
            ViewBag.ThongBao = "Lưu Thành Công";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ChinhSua(int ? id)
      {
            //Lấy sản phẩm  cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPham.SingleOrDefault(s => s.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();// không tìm thấy
            }
            // Load Dropdow list  nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(s => s.TenNCC), "MaNCC", "TenNCC",sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(s => s.MaLoaiSP), "MaLoaiSP", "TenLoai",sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(s => s.MaNSX), "MaNSX", "TenNSX",sp.MaNSX);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham sp)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(s => s.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(s => s.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(s => s.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            // do chưa kiểm tra validtion nên tạm thời cho nó đúng 
            //if (ModelState.IsValid)
            //{

                db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //return View(sp);
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPham.SingleOrDefault(s => s.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();// không tìm thấy
            }
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(s => s.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(s => s.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(s => s.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [HttpPost]
        public ActionResult Xoa(int  id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPham.SingleOrDefault(s => s.MaSP == id);
            if (sp == null)
            {

                return HttpNotFound();
            }
            db.SanPham.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}