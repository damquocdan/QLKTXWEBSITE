using Microsoft.AspNetCore.Mvc;

namespace QLKTXWEBSITE.Controllers
{
	public class BaseController : Controller
	{
		protected bool IsLoggedIn
		{
			get
			{
				// Thực hiện logic kiểm tra xem người dùng đã đăng nhập hay chưa
				// Ví dụ: 
				return User.Identity.IsAuthenticated;
			}
		}
	}
}
