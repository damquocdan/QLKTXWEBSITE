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
    //[Area("StudentUser")]
    public class StudentsController : BaseController
    {
        private readonly QlktxContext _context;

        public StudentsController(QlktxContext context)
        {
            _context = context;
        }

        // GET: StudentUser/Students
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: StudentUser/Students/Details/5
        public async Task<IActionResult> Details(int? studentId)
        {
            if (studentId == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Bed)
                .Include(s => s.Department)
                .Include(s => s.Dh)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.StudentId == studentId);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: StudentUser/Students/Create
        public IActionResult Create()
        {
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "BedId");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId");
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhid");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            return View();
        }

        // POST: StudentUser/Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FullName,DateOfBirth,Gender,PhoneNumber,ParentPhoneNumber,Email,Password,StudentCode,Dhid,DepartmentId,Class,AdmissionConfirmation,RoomId,BedId")] Student student)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var FileName = file.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\students", FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        student.AdmissionConfirmation = "/images/students/" + FileName;
                    }
                }
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "BedId", student.BedId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", student.DepartmentId);
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhid", student.Dhid);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", student.RoomId);
            return View(student);
        }

        // GET: StudentUser/Students/Edit/5
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
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "NumberBed", student.BedId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", student.DepartmentId);
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhcode", student.Dhid);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", student.RoomId);
            return View(student);
        }

        // POST: StudentUser/Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FullName,DateOfBirth,Gender,PhoneNumber,ParentPhoneNumber,Email,Password,StudentCode,Dhid,DepartmentId,Class,AdmissionConfirmation,RoomId,BedId")] Student student)
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
                return RedirectToAction("Details", new { id = student.StudentId });
            }
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "NumberBed", student.BedId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", student.DepartmentId);
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhcode", student.Dhid);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", student.RoomId);
            return View(student);
        }

        // GET: StudentUser/Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Bed)
                .Include(s => s.Department)
                .Include(s => s.Dh)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: StudentUser/Students/Delete/5
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
        // GET: StudentUser/Students/ChangePassword
        public async Task<IActionResult> ChangePassword(int? id)
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
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "NumberBed", student.BedId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", student.DepartmentId);
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhcode", student.Dhid);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", student.RoomId);
            return View(student);
        }

        // POST: StudentUser/Students/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(int id, string password, string newPassword, string confirmNewPassword)
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

            // Kiểm tra mật khẩu cũ
            if (student.Password != password)
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu cũ không chính xác.");
                return View(student);
            }

            // Kiểm tra mật khẩu mới và xác nhận mật khẩu mới
            if (newPassword != confirmNewPassword)
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu mới và xác nhận mật khẩu mới không khớp.");
                return View(student);
            }

            // Cập nhật mật khẩu mới
            student.Password = newPassword;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = student.StudentId });
            }
            catch (Exception)
            {
                // Xử lý lỗi nếu có
                return RedirectToAction("Error");
            }
        }

    }
}
