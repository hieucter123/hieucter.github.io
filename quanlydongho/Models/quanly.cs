using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace quanlydongho.Models
{
    public partial class quanly : DbContext
    {
        public quanly()
            : base("name=qldongho2")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<DanhMuc> DanhMucs { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KhachHang>()
                .HasMany(e => e.DonHangs)
                .WithOptional(e => e.KhachHang)
                .HasForeignKey(e => e.MaKhachHang);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.DonHangs)
                .WithRequired(e => e.SanPham)
                .HasForeignKey(e => e.MaSanPham)
                .WillCascadeOnDelete(false);
        }
    }
}
