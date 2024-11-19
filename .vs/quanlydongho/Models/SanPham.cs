namespace quanlydongho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            DonHangs = new HashSet<DonHang>();
        }

        [Key]
        public int Ma { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Tên đồng hồ")]
        public string TenDongHo { get; set; }

        public string MoTa { get; set; }

        [StringLength(50)]
        public string DoiTuong { get; set; }

        [StringLength(255)]
        public string HinhAnh { get; set; }

        public decimal GiaBanLe { get; set; }

        public int? MaDanhMuc { get; set; }

        public virtual DanhMuc DanhMuc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}
