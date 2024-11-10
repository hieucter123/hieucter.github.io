namespace quanlydongho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SanPham")]
    public partial class SanPham
    {
        public SanPham()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // For auto-generated IDs
        public int Ma { get; set; }

        [Display(Name = "Tên Đồng Hồ")]
        [Required(ErrorMessage = "Tên đồng hồ là bắt buộc")]
        [StringLength(100)]
        public string TenDongHo { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(50)]
        public string MoTa { get; set; }

        [Display(Name = "Đối Tượng")]
        [StringLength(50)]
        public string DoiTuong { get; set; }

        [Display(Name = "Hình Ảnh")]
        [StringLength(255)]
        public string HinhAnh { get; set; }

        [Display(Name = "Giá Bán Lẻ")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá bán lẻ phải là một số không âm")]
        public decimal GiaBanLe { get; set; }

        [Display(Name = "Danh Mục")]
        [Required(ErrorMessage = "Danh mục là bắt buộc")]
        public int MaDanhMuc { get; set; }

        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual DanhMuc DanhMuc { get; set; }
    }
}
