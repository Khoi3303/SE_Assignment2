using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Assignment2.Models; // Gọi Models (Database) vào

namespace Web_Assignment2.Controllers
{
    public class HomeController : Controller
    {
        // Khởi tạo đối tượng kết nối DB (Tên này lấy theo hệ thống tự sinh)
        SE_Assignment2_DBEntities db = new SE_Assignment2_DBEntities();

        // 1. Hàm hiển thị Form Đăng nhập (Mặc định chạy đầu tiên)
        public ActionResult Login()
        {
            return View();
        }

        // 2. Hàm xử lý khi người dùng bấm nút "Đăng nhập"
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // Tìm user trong database có trùng tên và mật khẩu không
            var user = db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);

            if (user != null)
            {
                if (user.Lock == true)
                {
                    ViewBag.Error = "Tài khoản của bạn đã bị khóa!";
                    return View();
                }

                // Lưu thông tin vào Session và chuyển hướng vào trang chủ
                Session["User"] = user.UserName;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
                return View();
            }
        }

        // 3. Trang chủ (Main Form) - Chỉ cho phép vào khi đã đăng nhập
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login"); // Chưa đăng nhập thì đuổi về trang Login
            }

            return View();
        }

        // 4. Hàm Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}