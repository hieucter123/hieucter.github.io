using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using quanlydongho.Models;

namespace quanlydongho.Controllers
{
    public class SanPhamsController : Controller
    {
        private quanly db = new quanly();

        // GET: SanPhams
        public ActionResult Index(string searchQuery)
        {
            var sanPhams = db.SanPhams.Include(s => s.DanhMuc);

            // Nếu có từ khóa tìm kiếm, lọc các sản phẩm theo từ khóa
            if (!string.IsNullOrEmpty(searchQuery))
            {
                sanPhams = sanPhams.Where(s => s.TenDongHo.Contains(searchQuery) || s.DanhMuc.TenDanhMuc.Contains(searchQuery));
            }

            // Trả về danh sách sản phẩm đã được lọc
            return View(sanPhams.ToList());
        }
        
        // GET: SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
