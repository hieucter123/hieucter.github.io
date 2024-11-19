using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace quanlydongho.Models
{
    public class DonHangDetail
    {
        public int Ma { get; set; }
        public int MaDonHang { get; set; } // Khóa ngoại đến bảng DonHang
        public int MaSanPham { get; set; } // Khóa ngoại đến bảng SanPham
        
        public string DoiTuong { get; set; }
        public decimal DonGia { get; set; }

        // Điều hướng đến đối tượng liên quan
        public virtual DonHang DonHang { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}