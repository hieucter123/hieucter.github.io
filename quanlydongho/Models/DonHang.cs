using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace quanlydongho.Models
{
    [Table("DonHang")]
    public partial class DonHang
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Để Ma tự động tăng
        [DisplayName("Mã Đơn Hàng")]
        public int Ma { get; set; }

        [StringLength(50)]
        [DisplayName("Mã Khách Hàng")]
        public string MaKhachHang { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày Đặt")]
        public DateTime NgayDat { get; set; }

        [StringLength(50)]
        [DisplayName("Trạng Thái")]
        public string TrangThai { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Mã Sản Phẩm")]
        public int MaSanPham { get; set; }

        [DisplayName("Số Lượng")]
        public int SoLuong { get; set; }

        [DisplayName("Đơn Giá")]
        public decimal DonGia { get; set; }

        public virtual KhachHang KhachHang { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
