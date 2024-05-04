using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace QLKTXWEBSITE.Areas.StudentUser.Controllers
{
    [Area("StudentUser")]
    public class ForgotPasswordController : Controller
    {
        public IActionResult ForgotPassword()
        {
            // Hiển thị form nhập email để gửi liên kết khôi phục mật khẩu
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendPasswordResetLink(string email)
        {
            // Kiểm tra xem email có tồn tại trong hệ thống hay không
            // Nếu có, gửi email chứa liên kết khôi phục mật khẩu
            // Nếu không, hiển thị thông báo lỗi

            // Sau khi gửi email, chuyển hướng đến trang thông báo
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            // Hiển thị thông báo cho người dùng biết rằng liên kết khôi phục mật khẩu đã được gửi thành công
            return View();
        }
    }
}
