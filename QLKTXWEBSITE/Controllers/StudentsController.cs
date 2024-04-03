using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLKTXWEBSITE.Models;

namespace QLKTXWEBSITE.Controllers
{
    public class StudentsController : Controller
    {
        private readonly QlktxContext _context;
        

        public StudentsController(QlktxContext context)
        {
            _context = context;
        }

        // GET: Students
        public IActionResult Index(Student model)
        {
            return View();
        }


        [HttpGet]// thoát đăng nhập, huỷ session
        public IActionResult Logout()
        {

            HttpContext.Session.Remove("StudentLogin"); // huỷ session với key AdminLogin đã lưu trước đó

            return RedirectToAction("Index");
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Bed)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        public async Task<IActionResult> Notifications(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Occupancies)
                .Include(s => s.Services)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return PartialView("Notifications", student);
        }
        public async Task<IActionResult> DetailsPop(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Bed)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return PartialView("PopUpStudent", student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "BedId");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Student student)
        {
            if (ModelState.IsValid)
            {
                // Lưu dữ liệu vào cơ sở dữ liệu
                _context.Add(student);
                await _context.SaveChangesAsync();

                // Hiển thị thông báo thành công
                TempData["SuccessMessage"] = "Thông tin đã được gửi thành công! Vui lòng đợi để được liên hệ";

                return RedirectToAction(nameof(Contact));
            }
            return View(student);
        }
        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,DateOfBirth,Gender,PhoneNumber,ParentPhoneNumber,Email,Password,StudentCode,Dh,Department,AdmissionConfirmation,RoomId,BedId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "BedId", student.BedId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", student.RoomId);
            return View(student);
        }


        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "BedId", student.BedId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", student.RoomId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,DateOfBirth,Gender,PhoneNumber,ParentPhoneNumber,Email,Password,StudentCode,Dh,Department,AdmissionConfirmation,RoomId,BedId")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "BedId", student.BedId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", student.RoomId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Bed)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'QlktxContext.Students'  is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.StudentId == id)).GetValueOrDefault();
        }
    }
}
