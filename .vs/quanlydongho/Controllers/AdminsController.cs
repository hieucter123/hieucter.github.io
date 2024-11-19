using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using quanlydongho.Models;

namespace quanlydongho.Controllers
{
    public class AdminsController : Controller
    {
        private quanly db = new quanly();

        // GET: Admins
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admins"); 
            }

            return View(db.Admins.ToList());
        }

        // GET: Admins/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaiKhoan,Email,MatKhau")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaiKhoan,Email,MatKhau")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //================= Quản lý Sản Phẩm =================

        // GET: Admins/SanPhams
        public ActionResult SanPhams()
        {
            var sanPhams = db.SanPhams.Include(s => s.DanhMuc);
            return View(sanPhams.ToList());
        }

        // GET: Admins/SanPhams/Details/5
        public ActionResult SanPhamDetails(int? id)
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

        // GET: Admins/SanPhams/Create
        // GET: Admins/CreateSanPham
        public ActionResult CreateSanPham()
        {
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Admins/CreateSanPham
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSanPham([Bind(Include = "TenDongHo,MoTa,DoiTuong,GiaBanLe,MaDanhMuc")] SanPham sanPham, HttpPostedFileBase HinhAnhFile)
        {
            if (ModelState.IsValid)
            {
                // Handle the file upload if there is one
                if (HinhAnhFile != null && HinhAnhFile.ContentLength > 0)
                {
                    // Ensure a unique filename
                    var fileName = Path.GetFileName(HinhAnhFile.FileName);
                    var fileExtension = Path.GetExtension(fileName);
                    var uniqueFileName = Guid.NewGuid().ToString() + fileExtension; // Add GUID to make the filename unique

                    // Save the file in the 'Images' directory
                    var path = Path.Combine(Server.MapPath("~/Images/"), uniqueFileName);
                    HinhAnhFile.SaveAs(path);

                    // Set the file name in the product model (to store in the database)
                    sanPham.HinhAnh = uniqueFileName;
                }

                // Add the new product to the database
                db.SanPhams.Add(sanPham);
                db.SaveChanges(); // Save the changes to the database

                return RedirectToAction("SanPhams"); // Redirect to the product list page
            }

            // If the model is not valid, populate the dropdown list for categories and return the view with the current model
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            return View(sanPham);
        }




        // GET: Admins/SanPhams/Edit/5
        // GET: Admins/SanPhams/Edit/5
        public ActionResult EditSanPham(int? id)
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

            // Tạo ViewBag cho danh mục và hiển thị ảnh cũ
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            ViewBag.HinhAnhHienTai = sanPham.HinhAnh; // Lưu đường dẫn ảnh hiện tại vào ViewBag

            return View(sanPham);
        }

        // POST: Admins/SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSanPham([Bind(Include = "Ma,TenDongHo,MoTa,DoiTuong,GiaBanLe,MaDanhMuc,HinhAnh")] SanPham sanPham, HttpPostedFileBase HinhAnhFile)
        {
            if (ModelState.IsValid)
            {
                
                db.SanPhams.Attach(sanPham);

                if (HinhAnhFile != null && HinhAnhFile.ContentLength > 0)
                {
                    
                    var fileName = Path.GetFileName(HinhAnhFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    HinhAnhFile.SaveAs(path);

                    sanPham.HinhAnh = fileName;
                }
                else
                {
                    
                    db.Entry(sanPham).Property(s => s.HinhAnh).IsModified = false;
                }

                
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SanPhams");
            }

            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            return View(sanPham);
        }


        
        public ActionResult DeleteSanPham(int? id)
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

        // POST: Admins/SanPhams/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSanPhamConfirmed(int Ma)
        {
            // Find the product by its 'Ma' (id)
            var sanPham = db.SanPhams.Find(Ma);

            if (sanPham != null)
            {
                // Remove the product from the database
                db.SanPhams.Remove(sanPham);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Sản phẩm đã được xóa thành công!";
            }

            // After deleting, redirect back to the product list
            return RedirectToAction("SanPhams");
        }



        public ActionResult DanhMucs()
        {
            return View(db.DanhMucs.ToList());
        }

        // GET: Admins/CreateDanhMuc
        public ActionResult CreateDanhMuc()
        {
            return View();
        }

        // POST: Admins/CreateDanhMuc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDanhMuc([Bind(Include = "MaDanhMuc,TenDanhMuc")] DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                db.DanhMucs.Add(danhMuc);
                db.SaveChanges();
                return RedirectToAction("DanhMucs");
            }
            return View(danhMuc);
        }

        // GET: Admins/DetailsDanhMuc/5
        public ActionResult DetailsDanhMuc(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // GET: Admins/EditDanhMuc/5
        public ActionResult EditDanhMuc(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // POST: Admins/EditDanhMuc/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDanhMuc([Bind(Include = "MaDanhMuc,TenDanhMuc")] DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhMuc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhMucs");
            }
            return View(danhMuc);
        }

        // GET: Admins/DeleteDanhMuc/5
        public ActionResult DeleteDanhMuc(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // POST: Admins/DeleteDanhMuc/5
        [HttpPost, ActionName("DeleteDanhMuc")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDanhMucConfirmed(int id)
        {
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            db.DanhMucs.Remove(danhMuc);
            db.SaveChanges();
            return RedirectToAction("DanhMucs");
        }
        // GET: Admins/ManageOrders
        public ActionResult ManageOrders()
        {
            // Bao gồm thông tin khách hàng và sản phẩm khi lấy đơn hàng
            var orders = db.DonHangs
                           .Include(d => d.KhachHang)
                           .Include(d => d.SanPham)
                           .ToList();

            return View(orders);
        }
        public ActionResult OrderDetails(int id)
        {
            var orderDetails = db.DonHangs.Where(d => d.Ma == id).ToList(); // Lấy chi tiết đơn hàng từ cơ sở dữ liệu
            ViewBag.OrderDetails = orderDetails;
            return View();
        }
        public ActionResult EditOrder(int id)
        {
            var order = db.DonHangs
                          .Where(d => d.Ma == id)
                          .FirstOrDefault();

            if (order == null)
            {
                return HttpNotFound();
            }

            // Truyền thông tin đơn hàng vào view
            return View(order);
        }

        // POST: Admins/EditOrder/5
        // POST: Admins/EditOrder/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrder(int id, string TrangThai)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin đơn hàng từ cơ sở dữ liệu
                var order = db.DonHangs.FirstOrDefault(d => d.Ma == id);

                if (order != null)
                {
                    // Chỉ cập nhật trạng thái
                    order.TrangThai = TrangThai;

                    db.SaveChanges(); // Lưu thay đổi
                    TempData["SuccessMessage"] = "Cập nhật trạng thái đơn hàng thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Đơn hàng không tồn tại!";
                }

                return RedirectToAction("ManageOrders");
            }

            TempData["ErrorMessage"] = "Vui lòng kiểm tra thông tin!";
            return RedirectToAction("EditOrder", new { id });
        }




        public ActionResult DeleteOrder(int id)
        {
            var order = db.DonHangs
                          .Where(d => d.Ma == id)
                          .FirstOrDefault();

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // POST: Admins/DeleteOrder/5
        [HttpPost, ActionName("DeleteOrder")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrderConfirmed(int id)
        {
            var order = db.DonHangs
                          .Where(d => d.Ma == id)
                          .FirstOrDefault();

            if (order != null)
            {
                // Xóa đơn hàng
                db.DonHangs.Remove(order);
                db.SaveChanges();
            }

            return RedirectToAction("ManageOrders");
        }

        // GET: Admins/Login
        public ActionResult Login()
        {
            return View(); // Hiển thị trang đăng nhập (login.cshtml)
        }

        // POST: Admins/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string TaiKhoan, string MatKhau)
        {
            var admin = db.Admins.SingleOrDefault(a => a.TaiKhoan == TaiKhoan && a.MatKhau == MatKhau);

            if (admin != null)
            {
                // Lưu thông tin admin vào session
                Session["Admin"] = admin;
                return RedirectToAction("Index", "Admins"); // Redirect đến trang chính của admin
            }
            else
            {
                // Thông báo lỗi khi tài khoản hoặc mật khẩu không đúng
                ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return View();
            }
        }
        // Thêm vào AdminsController
        public ActionResult Logout()
        {
            Session["Admin"] = null;  // Xóa thông tin admin khỏi session
            return RedirectToAction("Login", "Admins"); // Chuyển hướng về trang đăng nhập
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
