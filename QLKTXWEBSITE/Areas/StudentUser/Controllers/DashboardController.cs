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
                .Where(r => r.Building == "D" && r.Mowroom==student.Gender)
                .ToList();

                var roomsInBuildingsE = _context.Rooms
                    .Where(r => r.Building == "E" && r.Mowroom == student.Gender)
                    .ToList();

                var roomsInBuildingsG = _context.Rooms
                    .Where(r => r.Building == "G" && r.Mowroom == student.Gender)
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
        [HttpPost]
        public IActionResult RegisterBed(int? studentId, int bedId, int roomId)
        {
            if (studentId == null)
            {
                return BadRequest("Student ID is required.");
            }

            var student = _context.Students.Include(s => s.Bed).FirstOrDefault(s => s.StudentId == studentId);

            if (student == null)
            {
                return NotFound("Student not found.");
            }

            var bed = _context.BedOfRooms.FirstOrDefault(b => b.BedId == bedId);

            if (bed == null)
            {
                return NotFound("Bed not found");
            }

            if (bed.Status == true)
            {
                // Nếu giường đã có sinh viên, trả về thông báo lỗi
                return BadRequest("This bed has already been registered.");
            }

            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == roomId);

            if (room == null)
            {
                return NotFound("Room not found");
            }

            // Cập nhật trạng thái của giường và thông tin giường cho sinh viên
            bed.Status = true;
            student.Bed = bed;
            student.Room = room;

            // Kiểm tra nếu số lượng sinh viên bằng số lượng giường thì cập nhật trạng thái của phòng
            room.NumberOfStudents++;
            if (room.NumberOfStudents == room.BedNumber)
            {
                room.Status = true;
            }

            var contract = new Occupancy
            {
                StudentId = studentId,
                RoomId = roomId,
                RenewalDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMonths(6), // Hợp đồng có thời hạn 6 tháng
                CycleMonths = 6,
                Status = false // Chưa thanh toán
            };
            var hd = new Service
            {
                StudentId = studentId,
                RoomId = roomId,
                ServiceName = "Phòng ở kí túc xá",
                Month = DateTime.Now.Month,
                Price = room.Floor,
                Status = false // Chưa thanh toán
            };
            _context.Services.Add(hd);
            _context.Occupancies.Add(contract);
            _context.SaveChanges();

            // Tạo một công việc lên lịch để kiểm tra thanh toán hợp đồng sau mỗi 3 ngày
            ScheduleContractCheck(hd.ServiceId, TimeSpan.FromDays(3));

            // Chuyển hướng về trang chủ sau khi đăng ký thành công
            return RedirectToAction("Index", "Dashboard");
        }

        private void ScheduleContractCheck(int serviceId, TimeSpan interval)
        {
            // Tạo một công việc lên lịch để kiểm tra hợp đồng sau mỗi khoảng thời gian interval
            // Ở đây, bạn có thể sử dụng một cơ chế như Hangfire hoặc Quartz.NET để quản lý công việc lên lịch.
            // Đây là một ví dụ giả định sử dụng cách tiếp cận đơn giản với một Timer.
            var timer = new System.Timers.Timer();
            timer.Elapsed += (sender, e) => ContractCheck(serviceId);
            timer.Interval = interval.TotalMilliseconds;
            timer.AutoReset = false;
            timer.Start();
        }

        private void ContractCheck(int serviceId)
        {
            // Kiểm tra hợp đồng
            var hd = _context.Services.Find(serviceId);
            if (hd != null && hd.Status == false)
            {
                // Xóa dịch vụ không được thanh toán
                _context.Services.Remove(hd);
                _context.SaveChanges();

                // Xóa bản ghi Occupancy tương ứng
                var occupancy = _context.Occupancies.FirstOrDefault(o => o.StudentId == hd.StudentId && o.RoomId == hd.RoomId);
                if (occupancy != null)
                {
                    _context.Occupancies.Remove(occupancy);
                    _context.SaveChanges();
                }
            }
        }

    }
}
