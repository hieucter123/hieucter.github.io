using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace quanlydongho.Models
{
    public partial class quanly : DbContext
    {
        public quanly()
            : base("name=quanly")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual DbSet<DanhMuc> DanhMucs { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonHang>()
                .HasMany(e => e.ChiTietDonHangs)
                .WithOptional(e => e.DonHang)
                .HasForeignKey(e => e.MaDonHang);

            modelBuilder.Entity<KhachHang>()
                .HasMany(e => e.DonHangs)
                .WithOptional(e => e.KhachHang)
                .HasForeignKey(e => e.MaKhachHang);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.ChiTietDonHangs)
                .WithOptional(e => e.SanPham)
                .HasForeignKey(e => e.MaSanPham);
        }
    }
}
