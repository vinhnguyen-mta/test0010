using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebsiteBanHang.Models
{
    [MetadataTypeAttribute(typeof(ThanhVienMetadata))]
    public partial class ThanhVien
    {
        internal sealed class ThanhVienMetadata
        {
            
            public int MaThanhVien { get; set; }
            [DisplayName("Tên Tài Khoản")]
            [Required(ErrorMessage="{0} Không được bỏ trống!")]
            [StringLength(30,ErrorMessage="{0} Không được quá 30 ký tự")]
            public string TaiKhoan { get; set; }
            public string MatKhau { get; set; }
            public string HoTen { get; set; }
            [Range(1000,50000,ErrorMessage="{0} Phải từ {1} đến {2}")]
            public string DiaChi { get; set; }
            [RegularExpression(@"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$",ErrorMessage="{0} Không hợp lệ!")]
            public string Email { get; set; }
            public string SoDienThoai { get; set; }
            public string CauHoi { get; set; }
            public string CauTraLoi { get; set; }
            public Nullable<int> MaLoaiTV { get; set; }

          
        }
    }
}