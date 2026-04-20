using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebCore_Assignment2.Models;

namespace WebCore_Assignment2.Pages
{
    public class LoginModel : PageModel
    {
        // Kết nối Database
        SeAssignment2DbContext db = new SeAssignment2DbContext();

        // [BindProperty] giúp tự động bắt dữ liệu từ form HTML gửi lên
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        // Hàm chạy khi mới load trang
        public void OnGet()
        {
        }

        // Hàm chạy khi bấm nút "Đăng Nhập"
        public IActionResult OnPost()
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == Username && u.Password == Password);

            if (user != null)
            {
                if (user.Lock == true)
                {
                    ErrorMessage = "Tài khoản của bạn đã bị khóa!";
                    return Page();
                }

                // Lưu Session và chuyển về trang chủ (Index)
                HttpContext.Session.SetString("User", user.UserName);
                return RedirectToPage("/Index");
            }

            ErrorMessage = "Sai tên đăng nhập hoặc mật khẩu!";
            return Page();
        }
    }
}