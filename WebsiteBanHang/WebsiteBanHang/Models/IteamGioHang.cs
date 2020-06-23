using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteBanHang.Models
{
    public class IteamGioHang
    {
        public string TenSP { get; set; }
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }


        public IteamGioHang(int iMaSP)
        {
            using (QuanLyBanHangEntities db = new QuanLyBanHangEntities()) // để đỡ tốn vùng nhớ
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPham.Single(s => s.MaSP == iMaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.SoLuong = 1;
                this.ThanhTien = DonGia * SoLuong;
            }
        }
        public IteamGioHang(int iMaSP, int  sl)
        {
            using (QuanLyBanHangEntities db = new QuanLyBanHangEntities()) // để đỡ tốn vùng nhớ
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPham.Single(s => s.MaSP == iMaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.SoLuong = sl; // để khi nhấn vào nút button tăng số lượng thì sẽ tăng lển 
                this.ThanhTien = DonGia * SoLuong;


            }
        }
        public IteamGioHang()
        {

        }
    }
}