using Microsoft.AspNetCore.Mvc;
using QLKTXWEBSITE.Models;

namespace QLKTXWEBSITE.Controllers
{
    public class LoginController : Controller
    {
        public QlktxContext _context;
        public LoginController(QlktxContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost] // POST -> khi submit form
        public IActionResult Index(LoginUser model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không hợp lệ.");
                return View(model);
            }

            var pass = model.Password;
            var dataLogin = _context.Students.FirstOrDefault(x => x.StudentCode.Equals(model.StudentCode) && x.Password.Equals(pass));
            if (dataLogin != null)
            {
                ViewBag.IsLoggedIn = true;
                HttpContext.Session.SetString("StudentLogin", model.StudentCode);
                return RedirectToAction("Details", "Students",new {id = dataLogin.StudentId});
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không chính xác.");
                return View(model);
            }

        }
        [HttpGet]// thoát đăng nhập, huỷ session
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("StudentLogin"); // huỷ session với key AdminLogin đã lưu trước đó

            return RedirectToAction("Create","Students");
        }
    }
}
