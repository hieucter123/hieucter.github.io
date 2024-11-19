using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
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
            if (Request.IsAjaxRequest())
            {
                return PartialView("_YourPartialView");
            }
            // Trả về danh sách sản phẩm đã được lọc
            ViewBag.WelcomeMessage = TempData["WelcomeMessage"];
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
        // GET: KhachHangs/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: KhachHangs/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "TK, MatKhau")] KhachHang khachHang)
        {
            if (string.IsNullOrEmpty(khachHang.TK) || string.IsNullOrEmpty(khachHang.MatKhau))
            {
                ViewBag.Error = "Tên đăng nhập và mật khẩu không được để trống!";
                return View();
            }

            var user = db.KhachHangs.SingleOrDefault(kh => kh.TK == khachHang.TK);
            if (user == null)
            {
                ViewBag.Error = "Tên đăng nhập không tồn tại!";
                return View();
            }

            // Kiểm tra mật khẩu đã được mã hóa
            if (!BCrypt.Net.BCrypt.Verify(khachHang.MatKhau, user.MatKhau))
            {
                // Nếu mật khẩu không khớp, mã hóa lại và lưu vào DB
                user.MatKhau = BCrypt.Net.BCrypt.HashPassword(khachHang.MatKhau);
                db.SaveChanges();
                ViewBag.Error = "Mật khẩu không chính xác!";
                return View();
            }

            // Đăng nhập thành công, lưu thông tin vào Session
            Session["User"] = user.TK;

            // Trả về thông báo chào mừng người dùng
            TempData["WelcomeMessage"] = "Xin chào, " + user.TK + "!";

            return RedirectToAction("Index", "SanPhams");
        }




        // GET: KhachHangs/Register
        // GET: KhachHangs/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: KhachHangs/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "TK, Email, MatKhau")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tài khoản đã tồn tại
                var existingUser = db.KhachHangs.SingleOrDefault(kh => kh.TK == khachHang.TK);
                if (existingUser != null)
                {
                    ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                    return View();
                }

                // Kiểm tra email đã tồn tại
                var existingEmail = db.KhachHangs.SingleOrDefault(kh => kh.Email == khachHang.Email);
                if (existingEmail != null)
                {
                    ViewBag.Error = "Email đã được sử dụng!";
                    return View();
                }

                // Mã hóa mật khẩu trước khi lưu vào database
                khachHang.MatKhau = BCrypt.Net.BCrypt.HashPassword(khachHang.MatKhau);

                // Thêm vào cơ sở dữ liệu
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();

                // Đăng ký thành công, chuyển hướng đến trang login
                return RedirectToAction("Login", "SanPhams");
            }

            return View(khachHang);
        }

        // GET: SanPhams/Profile
        public ActionResult Profile()
        {
            // Kiểm tra nếu người dùng chưa đăng nhập
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "SanPhams");
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var userTK = Session["User"].ToString();
            var user = db.KhachHangs.SingleOrDefault(kh => kh.TK == userTK);

            // Nếu không tìm thấy người dùng
            if (user == null)
            {
                return RedirectToAction("Login", "SanPhams");
            }

            return View(user);
        }


        // Phương thức thêm vào giỏ hàng
        public ActionResult AddToCart(int id)
        {
            // Kiểm tra nếu người dùng chưa đăng nhập
            if (Session["User"] == null)
            {
                TempData["ErrorMessage"] = "Bạn cần phải đăng nhập để sử dụng chức năng này.";
                return RedirectToAction("Login", "SanPhams");
            }

            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            // Lấy thông tin khách hàng từ Session
            string maKhachHang = Session["User"].ToString();

            // Kiểm tra xem giỏ hàng đã tồn tại trong Session chưa
            var cart = Session["Cart"] as List<DonHang>;
            if (cart == null)
            {
                cart = new List<DonHang>();
            }

            // Kiểm tra xem sản phẩm đã có trong giỏ chưa
            var cartItem = cart.FirstOrDefault(c => c.MaSanPham == id);
            if (cartItem == null)
            {
                // Nếu chưa có thì thêm mới
                cart.Add(new DonHang
                {
                    MaKhachHang = maKhachHang,
                    MaSanPham = sanPham.Ma,
                    SanPham = sanPham,
                    SoLuong = 1,
                    DonGia = sanPham.GiaBanLe,
                    NgayDat = DateTime.Now,
                    TrangThai = "Chờ xử lý"
                });
            }
            else
            {
                // Nếu có rồi thì tăng số lượng
                cartItem.SoLuong++;
            }

            // Lưu giỏ hàng vào Session
            Session["Cart"] = cart;

            TempData["SuccessMessage"] = "Đã thêm vào giỏ hàng!";
            return RedirectToAction("Index", "SanPhams");
        }

        public ActionResult Cart()
        {
            // Lấy giỏ hàng từ Session
            var cart = Session["Cart"] as List<DonHang> ?? new List<DonHang>();

            // Nạp đầy đủ thông tin SanPham và DanhMuc để tránh lỗi "ObjectContext has been disposed"
            foreach (var item in cart)
            {
                // Nạp thông tin đầy đủ về SanPham và DanhMuc
                item.SanPham = db.SanPhams.Include(s => s.DanhMuc).FirstOrDefault(s => s.Ma == item.MaSanPham);
            }

            return View(cart);
        }

        [HttpPost]
        public ActionResult UpdateCart(Dictionary<int, int> updatedCart)
        {
            // Lấy giỏ hàng từ Session
            var cart = Session["Cart"] as List<DonHang>;
            if (cart != null)
            {
                foreach (var update in updatedCart)
                {
                    // Tìm sản phẩm trong giỏ hàng theo ID
                    var cartItem = cart.FirstOrDefault(c => c.MaSanPham == update.Key);
                    if (cartItem != null)
                    {
                        // Cập nhật số lượng
                        cartItem.SoLuong = update.Value;
                    }
                }

                // Lưu lại giỏ hàng vào Session
                Session["Cart"] = cart;
            }

            TempData["SuccessMessage"] = "Giỏ hàng đã được cập nhật!";
            return RedirectToAction("Cart");
        }

        public ActionResult Checkout()
        {
            var cart = Session["Cart"] as List<DonHang>;
            if (cart == null || !cart.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống!";
                return RedirectToAction("Cart");
            }

            // Lưu đơn hàng vào bảng DonHang
            foreach (var item in cart)
            {
                // Kiểm tra để tránh trùng lặp khi lưu vào DB, không cần phải thêm lại DanhMuc
                var existingProduct = db.SanPhams.Find(item.MaSanPham);
                if (existingProduct != null)
                {
                    // Gán lại thông tin sản phẩm (không cần thêm mới DanhMuc)
                    item.SanPham = existingProduct;
                    db.DonHangs.Add(item);
                }
            }

            db.SaveChanges();
            Session["Cart"] = null; // Xóa giỏ hàng sau khi đặt hàng
            TempData["SuccessMessage"] = "Đặt hàng thành công!";
            return RedirectToAction("Cart", "SanPhams");
        }










        // GET: KhachHangs/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "SanPhams");
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
