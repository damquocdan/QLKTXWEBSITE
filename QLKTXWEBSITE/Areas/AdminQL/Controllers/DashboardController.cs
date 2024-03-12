using Microsoft.AspNetCore.Mvc;

namespace QLKTXWEBSITE.Areas.AdminQL.Controllers
{
    //[Area("Admins")]
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
