using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLKTXWEBSITE.Models;

namespace QLKTXWEBSITE.Areas.AdminQL.Controllers
{
    //[Area("AdminQL")]
    public class ServicesController : BaseController
    {
        private readonly QlktxContext _context;

        public ServicesController(QlktxContext context)
        {
            _context = context;
        }

        // GET: AdminQL/Services
        public async Task<IActionResult> Index(string studentName, string serviceName, string month, bool? status)
        {
            // Lấy toàn bộ dữ liệu dịch vụ từ cơ sở dữ liệu
            var services = _context.Services.Include(s => s.Room).Include(s => s.Student).AsQueryable();

            // Áp dụng các điều kiện tìm kiếm nếu được cung cấp
            if (!string.IsNullOrEmpty(studentName))
            {
                services = services.Where(s => s.Student.FullName.Contains(studentName));
            }

            if (!string.IsNullOrEmpty(serviceName))
            {
                services = services.Where(s => s.ServiceName.Contains(serviceName));
            }

            if (!string.IsNullOrEmpty(month))
            {
                if (int.TryParse(month, out int monthValue))
                {
                    services = services.Where(s => s.Month == monthValue);
                }
            }

            if (status.HasValue)
            {
                services = services.Where(s => s.Status == status);
            }

            // Thực hiện truy vấn để lấy dữ liệu dịch vụ và chuyển sang danh sách
            var serviceList = await services.ToListAsync();

            return View(serviceList);
        }

        // GET: AdminQL/Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Room)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }
        public IActionResult GetRoomsByBuilding(string building)
        {
            var rooms = _context.Rooms.Where(r => r.Building == building)
                                       .Select(r => new { value = r.RoomId, text = r.NumberRoom })
                                       .ToList();
            return Json(rooms);
        }
        public IActionResult GetStudentsByRoom(int roomId)
        {
            var students = _context.Students.Where(s => s.RoomId == roomId)
                                            .Select(s => new { value = s.StudentId, text = s.FullName })
                                            .ToList();
            return Json(students);
        }
        // GET: AdminQL/Services/Create
        public IActionResult Create()
        {
            ViewData["Building"] = new SelectList(_context.Rooms, "RoomId", "Building");

            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName");
            return View();
        }

        // POST: AdminQL/Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,Month,Price,RoomId,StudentId,Status")] Service service)
        {
            if (ModelState.IsValid)
            {
                // Lấy danh sách sinh viên trong phòng đã chọn
                var studentsInRoom = await _context.Students.Where(s => s.RoomId == service.RoomId).ToListAsync();

                // Tính giá trị dịch vụ cho mỗi sinh viên
                decimal pricePerStudent;
                if (service.Price == null)
                {
                    pricePerStudent = 0;
                }
                else
                {
                    pricePerStudent = (decimal)service.Price / studentsInRoom.Count;
                }

                // Gửi dịch vụ cho từng sinh viên trong danh sách
                foreach (var student in studentsInRoom)
                {
                    // Thêm dịch vụ cho sinh viên
                    var studentService = new Service
                    {
                        RoomId = service.RoomId,
                        ServiceName = service.ServiceName,
                        StudentId = student.StudentId,
                        ServiceId = service.ServiceId,
                        Month = service.Month,
                        Price = pricePerStudent, // Giá trị dịch vụ cho mỗi sinh viên
                        Status = service.Status
                        // Các thuộc tính khác của StudentService nếu có
                    };
                    _context.Services.Add(studentService);
                }

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, trả về view với dữ liệu đã nhập và danh sách phòng
            ViewData["Building"] = new SelectList(_context.Rooms, "RoomId", "Building", service.RoomId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", service.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName", service.StudentId);
            return View(service);
        }



        // GET: AdminQL/Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["Building"] = new SelectList(_context.Rooms, "RoomId", "Building", service.RoomId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", service.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName", service.StudentId);
            return View(service);
        }

        // POST: AdminQL/Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceName,Month,Price,RoomId,StudentId,Status")] Service service)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Building"] = new SelectList(_context.Rooms, "RoomId", "Building", service.RoomId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", service.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName", service.StudentId);
            return View(service);
        }

        // GET: AdminQL/Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Room)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: AdminQL/Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Services == null)
            {
                return Problem("Entity set 'QlktxContext.Services'  is null.");
            }
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
          return (_context.Services?.Any(e => e.ServiceId == id)).GetValueOrDefault();
        }
    }
}
