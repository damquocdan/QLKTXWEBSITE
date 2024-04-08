using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            return View(new List<List<Room>> { roomsInBuildingsD, roomsInBuildingsE, roomsInBuildingsG });

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
