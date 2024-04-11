using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLKTXWEBSITE.Models;

namespace QLKTXWEBSITE.Areas.StudentUser.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly QlktxContext _context;

        public DashboardController(QlktxContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? id)
        {
            var news = _context.News.ToList();
            return View(news);
        }
        public IActionResult Aboutus()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Service(int? id)
        {
            var student = _context.Students
                .Include(s => s.Bed)
                .FirstOrDefault(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            if (student.BedId == null)
            {
                var roomsInBuildingsD = _context.Rooms
                .Where(r => r.Building == "D")
                .ToList();

                var roomsInBuildingsE = _context.Rooms
                    .Where(r => r.Building == "E")
                    .ToList();

                var roomsInBuildingsG = _context.Rooms
                    .Where(r => r.Building == "G")
                    .ToList();

                // Truyền danh sách phòng của các tòa nhà D, E, G đến view
                return View("RegisterRoom",new List<List<Room>> { roomsInBuildingsD, roomsInBuildingsE, roomsInBuildingsG });

            }
            else
            {
                // Nếu sinh viên đã có giường, hiển thị form đăng ký các dịch vụ khác
                return View("RegisterServices");
            }
        }
        public IActionResult GetBeds(int RoomId)
        {
            var bedsInRoom = _context.BedOfRooms
                .Where(b => b.RoomId == RoomId)
                .ToList();

            return PartialView("_Beds", bedsInRoom);
        }
        public IActionResult GetStudents(int BedId)
        {
            var studentsInRoom = _context.Students
                .Where(b => b.BedId == BedId)
                .ToList();

            return PartialView("_Students", studentsInRoom);
        }
    }
}
