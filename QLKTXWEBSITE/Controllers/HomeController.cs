using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKTXWEBSITE.Models;
using System.Diagnostics;

namespace QLKTXWEBSITE.Controllers
{
    public class HomeController : Controller
    {
        private readonly QlktxContext _context;

        public HomeController(QlktxContext context)
        {
            _context = context;
        }

        public IActionResult Index()
            
        {
            var news = _context.News.ToList();
            return View(news);
        }

        public IActionResult Aboutus()
        {
            return View();
        }
        public IActionResult Service()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
