using Microsoft.AspNetCore.Mvc;

namespace QLKTXWEBSITE.Areas.StudentUser.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
