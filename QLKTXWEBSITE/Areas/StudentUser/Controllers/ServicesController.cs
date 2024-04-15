using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLKTXWEBSITE.Models;

namespace QLKTXWEBSITE.Areas.StudentUser.Controllers
{
    [Area("StudentUser")]
    public class ServicesController : Controller
    {
        private readonly QlktxContext _context;

        public ServicesController(QlktxContext context)
        {
            _context = context;
        }

        // GET: StudentUser/Services
        public async Task<IActionResult> Index(int? studentId, int? serviceType)
        {
            IQueryable<Service> services = _context.Services.Include(s => s.Room).Include(s => s.Student);
            if (studentId != null)
            {
                services = services.Where(o => o.StudentId == studentId);
            }

            if (serviceType == 1)
            {
                services = services.Where(o => o.ServiceName == "Điện");
            }
            else if (serviceType == 2)
            {
                services = services.Where(o => o.ServiceName == "Nước");
            }
            return View(await services.ToListAsync());
        }

        // GET: StudentUser/Services/Details/5
        public async Task<IActionResult> DetailsU(string serviceName, int? studentId)
        {
            if (serviceName == null || studentId == null)
            {
                return NotFound();
            }

            // Lấy thông tin dịch vụ của sinh viên cho dịch vụ và tháng hiện tại
            var service = await _context.Services
                .Include(s => s.Room)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.ServiceName == serviceName && m.StudentId == studentId && m.Month == DateTime.Now.Month);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }
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

        // GET: StudentUser/Services/Create
        public IActionResult Create(int studentId, string serviceName)
        {
            Service service = new Service
            {
                ServiceName = serviceName,
                Month = DateTime.Now.Month,
                Price = 100000,
                StudentId = studentId
            };

            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId");
            return View(service);
        }

        // POST: StudentUser/Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,Month,Price,RoomId,StudentId,Status")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", service.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", service.StudentId);
            return View(service);
        }

        // GET: StudentUser/Services/Edit/5
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", service.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", service.StudentId);
            return View(service);
        }

        // POST: StudentUser/Services/Edit/5
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", service.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", service.StudentId);
            return View(service);
        }

        // GET: StudentUser/Services/Delete/5
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

        // POST: StudentUser/Services/Delete/5
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
        public IActionResult RegisterService(string serviceName, int studentId)
        {
            // Kiểm tra xem sinh viên đã đăng ký dịch vụ đó cho tháng này chưa
            bool hasRegistered = CheckIfStudentHasRegisteredForService(serviceName, studentId);

            if (hasRegistered)
            {
                // Nếu đã đăng ký, chuyển hướng đến trang chi tiết dịch vụ
                return RedirectToAction("DetailsU", new { ServiceName = serviceName, StudentId = studentId });
            }
            else
            {
                // Nếu chưa đăng ký, chuyển hướng đến trang tạo mới dịch vụ
                return RedirectToAction("Create", new { ServiceName = serviceName, StudentId = studentId });
            }
        }

        // Phương thức để kiểm tra xem sinh viên đã đăng ký dịch vụ của tháng đó chưa
        private bool CheckIfStudentHasRegisteredForService(string serviceType, int studentId)
        {
            var service = _context.Services.FirstOrDefault(s => s.ServiceName == serviceType && s.StudentId == studentId);

            // Kiểm tra xem dịch vụ có tồn tại không
            if (service != null)
            {
                // Nếu dịch vụ tồn tại, sinh viên đã đăng ký
                return true;
            }
            else
            {
                // Nếu không có dịch vụ tồn tại, sinh viên chưa đăng ký
                return false;
            }
        }
        private bool ServiceExists(int id)
        {
          return (_context.Services?.Any(e => e.ServiceId == id)).GetValueOrDefault();
        }
    }
}
