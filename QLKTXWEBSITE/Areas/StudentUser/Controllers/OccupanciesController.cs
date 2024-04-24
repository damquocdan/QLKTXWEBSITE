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
    public class OccupanciesController : Controller
    {
        private readonly QlktxContext _context;

        public OccupanciesController(QlktxContext context)
        {
            _context = context;
        }

        // GET: StudentUser/Occupancies
        public async Task<IActionResult> Index(int? studentId)
        {
            IQueryable<Occupancy> occupancies = _context.Occupancies.Include(o => o.Room).Include(o => o.Student);

            if (studentId != null)
            {
                occupancies = occupancies.Where(o => o.StudentId == studentId);
            }

            return View(await occupancies.ToListAsync());
        }

        // GET: StudentUser/Occupancies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Occupancies == null)
            {
                return NotFound();
            }

            var occupancy = await _context.Occupancies
                .Include(o => o.Room)
                .Include(o => o.Student)
                .FirstOrDefaultAsync(m => m.OccupancyId == id);
            if (occupancy == null)
            {
                return NotFound();
            }

            return View(occupancy);
        }

        // GET: StudentUser/Occupancies/Create
        public IActionResult Create(int? studentId)
        {
            if (studentId == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Lấy thông tin sinh viên từ ID
            var student = _context.Students.FirstOrDefault(s => s.StudentId == studentId);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Tạo SelectList cho RoomId và truyền thông tin sinh viên
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName", studentId);

            // Tạo đối tượng Occupancy và set StudentId
            var occupancy = new Occupancy { StudentId = studentId };

            // Truyền occupancy vào view
            return View(occupancy);
        }

        // POST: StudentUser/Occupancies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("OccupancyId,StudentId,RoomId,RenewalDate,ExpirationDate,CycleMonths,Status")] Occupancy occupancy)
{
    if (ModelState.IsValid)
    {
        // Thiết lập ngày bắt đầu là ngày hiện tại
        occupancy.RenewalDate = DateTime.Today;

        // Thiết lập ngày kết thúc là 6 tháng sau ngày hiện tại
        occupancy.ExpirationDate = DateTime.Today.AddMonths(6);

        // Thiết lập số tháng là 6
        occupancy.CycleMonths = 6;

        // Thiết lập trạng thái là chưa thanh toán
        occupancy.Status = false;

        // Lưu thông tin hợp đồng vào cơ sở dữ liệu
        _context.Add(occupancy);
        await _context.SaveChangesAsync();

        // Tạo đối tượng dịch vụ
        var room = _context.Rooms.FirstOrDefault(r => r.RoomId == occupancy.RoomId);
        if (room != null)
        {
            var hd = new Service
            {
                StudentId = occupancy.StudentId,
                RoomId = occupancy.RoomId,
                ServiceName = "Phòng ở kí túc xá",
                Month = DateTime.Now.Month,
                Price = room.Floor, // Sử dụng giá trị Floor của phòng
                Status = false // Chưa thanh toán
            };
            _context.Add(hd);
            await _context.SaveChangesAsync();
        }

        // Chuyển hướng đến trang danh sách hợp đồng của sinh viên đó
        return RedirectToAction("Index", "Occupancies", new { studentId = occupancy.StudentId });
    }

    // Nếu ModelState không hợp lệ, trả về view với dữ liệu và thông báo lỗi
    ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", occupancy.RoomId);
    ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName", occupancy.StudentId);
    return View(occupancy);
}
        // GET: StudentUser/Occupancies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Occupancies == null)
            {
                return NotFound();
            }

            var occupancy = await _context.Occupancies.FindAsync(id);
            if (occupancy == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", occupancy.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", occupancy.StudentId);
            return View(occupancy);
        }

        // POST: StudentUser/Occupancies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OccupancyId,StudentId,RoomId,RenewalDate,ExpirationDate,CycleMonths,Status")] Occupancy occupancy)
        {
            if (id != occupancy.OccupancyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(occupancy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OccupancyExists(occupancy.OccupancyId))
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", occupancy.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", occupancy.StudentId);
            return View(occupancy);
        }

        // GET: StudentUser/Occupancies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Occupancies == null)
            {
                return NotFound();
            }

            var occupancy = await _context.Occupancies
                .Include(o => o.Room)
                .Include(o => o.Student)
                .FirstOrDefaultAsync(m => m.OccupancyId == id);
            if (occupancy == null)
            {
                return NotFound();
            }

            return View(occupancy);
        }

        // POST: StudentUser/Occupancies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Occupancies == null)
            {
                return Problem("Entity set 'QlktxContext.Occupancies'  is null.");
            }
            var occupancy = await _context.Occupancies.FindAsync(id);
            if (occupancy != null)
            {
                _context.Occupancies.Remove(occupancy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OccupancyExists(int id)
        {
          return (_context.Occupancies?.Any(e => e.OccupancyId == id)).GetValueOrDefault();
        }
    }
}
