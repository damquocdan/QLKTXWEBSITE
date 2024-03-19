using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLKTXWEBSITE.Models;
using OfficeOpenXml;
using System.IO;
using System.Data;
using OfficeOpenXml.Table;

namespace QLKTXWEBSITE.Areas.AdminQL.Controllers
{
    [Area("AdminQL")]
    public class StudentsController : Controller
    {
        private readonly QlktxContext _context;

        public StudentsController(QlktxContext context)
        {
            _context = context;
        }

        // GET: AdminQL/Students
        public async Task<IActionResult> ListStudentBed(string gender)
        {
            IQueryable<Student> studentsWithoutBedQuery = _context.Students
                .Where(s => s.BedId == null)
                .Include(s => s.Bed)
                .Include(s => s.Department)
                .Include(s => s.Dh)
                .Include(s => s.Room);

            if (!string.IsNullOrEmpty(gender))
            {
                studentsWithoutBedQuery = studentsWithoutBedQuery.Where(r => r.Gender == gender);
            }

            var studentsWithoutBed = await studentsWithoutBedQuery.ToListAsync();

            return View(studentsWithoutBed);
        }
        // POST: AdminQL/Students/ChooseBed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChooseBed(int studentId, int bedId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }

            var bed = await _context.BedOfRooms.FindAsync(bedId);
            if (bed == null)
            {
                return NotFound();
            }

            // Cập nhật ID giường cho sinh viên
            student.BedId = bedId;
            // Đặt trạng thái của giường là đã chọn
            bed.Status = true;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index","BedOfRooms");
        }
        public async Task<IActionResult> Index(string name)
        {
            IQueryable<Student> students = _context.Students.Include(s => s.Bed).Include(s => s.Department).Include(s => s.Dh).Include(s => s.Room);

            if (!string.IsNullOrEmpty(name))
            {
                students = students.Where(s => s.FullName.Contains(name));
            }

            return View(await students.ToListAsync());

        }
        public async Task<IActionResult> Search(string name)
        {
            var students = await _context.Students
                .Where(s => s.FullName.Contains(name))
                .Include(s => s.Bed)
                .Include(s => s.Department)
                .Include(s => s.Dh)
                .Include(s => s.Room)
                .ToListAsync();

            return View("Index", students);
        }

        public async Task<IActionResult> ExportToExcel()
        {
            var students = await _context.Students
                .Include(s => s.Bed)
                .Include(s => s.Department)
                .Include(s => s.Dh)
                .Include(s => s.Room)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Students");

                // Header row
                worksheet.Cells[1, 1].Value = "Full Name";
                worksheet.Cells[1, 2].Value = "Date of Birth";
                worksheet.Cells[1, 3].Value = "Gender";
                worksheet.Cells[1, 4].Value = "Phone Number";
                worksheet.Cells[1, 5].Value = "Parent Phone Number";
                worksheet.Cells[1, 6].Value = "Email";
                worksheet.Cells[1, 7].Value = "Student Code";
                worksheet.Cells[1, 8].Value = "Department";
                worksheet.Cells[1, 9].Value = "Class";
                worksheet.Cells[1, 10].Value = "Admission Confirmation";

                // Data rows
                int row = 2;
                foreach (var student in students)
                {
                    worksheet.Cells[row, 1].Value = student.FullName;
                    worksheet.Cells[row, 2].Value = student.DateOfBirth;
                    worksheet.Cells[row, 3].Value = student.Gender;
                    worksheet.Cells[row, 4].Value = student.PhoneNumber;
                    worksheet.Cells[row, 5].Value = student.ParentPhoneNumber;
                    worksheet.Cells[row, 6].Value = student.Email;
                    worksheet.Cells[row, 7].Value = student.StudentCode;
                    worksheet.Cells[row, 8].Value = student.Department?.DepartmentName; // Assuming DepartmentName property exists
                    worksheet.Cells[row, 9].Value = student.Class;
                    worksheet.Cells[row, 10].Value = student.AdmissionConfirmation;

                    row++;
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                // Convert package to a byte array
                byte[] excelBytes = package.GetAsByteArray();

                // Return Excel file
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Students.xlsx");
            }
        }
        // GET: AdminQL/Students/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: AdminQL/Students/Create
        public IActionResult Create()
        {
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "BedId");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId");
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhid");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            return View();
        }

        // POST: AdminQL/Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FullName,DateOfBirth,Gender,PhoneNumber,ParentPhoneNumber,Email,Password,StudentCode,Dhid,DepartmentId,Class,AdmissionConfirmation,RoomId,BedId")] Student student)
        {
            if (ModelState.IsValid)
            {
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

        // GET: AdminQL/Students/Edit/5
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", student.DepartmentId);
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhid", student.Dhid);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", student.RoomId);
            return View(student);
        }

        // POST: AdminQL/Students/Edit/5
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "BedId", student.BedId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", student.DepartmentId);
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhid", student.Dhid);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", student.RoomId);
            return View(student);
        }

        // GET: AdminQL/Students/Delete/5
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

        // POST: AdminQL/Students/Delete/5
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
