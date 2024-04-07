using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKTXWEBSITE.Models;

namespace QLKTXWEBSITE.Areas.StudentUser.Controllers
{
    public class SharedController : Controller
    {
        private readonly QlktxContext _context;

        public SharedController(QlktxContext context)
        {
            _context = context;
        }
        public IActionResult _Students()
        {

            int? studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                // Xử lý khi không tìm thấy StudentId trong session
                return RedirectToAction("Index", "Login"); // Ví dụ: Chuyển hướng đến trang đăng nhập
            }
            else
            {
                // Sử dụng studentId để thực hiện các xử lý cần thiết
                // Ví dụ:
                var student = _context.Students.FirstOrDefault(s => s.StudentId == studentId);
                if (student == null)
                {
                    return NotFound(); // Trả về lỗi 404 nếu không tìm thấy sinh viên
                }

                // Tiếp tục xử lý

                return View();
            }
        }
    }
}
