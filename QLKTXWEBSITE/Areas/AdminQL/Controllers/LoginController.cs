using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using QLKTXWEBSITE.Models;
using QLKTXWEBSITE.Areas.AdminQL.Models;

namespace WebQLKTX.Areas.Admin.Controllers
{
    [Area("AdminQL")]
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
        public IActionResult Index(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);// trả về trạng thái lỗi
            }
            // sẽ xử lý logic phần đăng nhập tại đây
            var email = model.Email;
            var pass = model.Password;
            var dataLogin = _context.Admins.Where(x => x.Email == email && x.Password == pass);
            if (dataLogin != null)         
            {
                // Lưu session khi đăng nhập thành công
                HttpContext.Session.SetString("AdminLogin", model.Email);


                return RedirectToAction("Index", "Dashboard");
            }
            ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không chính xác.");
            return View(model);

        }
        [HttpGet]// thoát đăng nhập, huỷ session
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminLogin"); // huỷ session với key AdminLogin đã lưu trước đó

            return RedirectToAction("Index");
        }
    }
}

