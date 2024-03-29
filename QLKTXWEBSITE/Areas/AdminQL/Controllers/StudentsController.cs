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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace QLKTXWEBSITE.Areas.AdminQL.Controllers
{
    //[Area("AdminQL")]
    public class StudentsController : BaseController
    {
        private readonly QlktxContext _context;

        public StudentsController(QlktxContext context)
        {
            _context = context;
        }

        // GET: AdminQL/Students
        public async Task<IActionResult> Index(string name)
        {
            IQueryable<Student> students = _context.Students.Include(s => s.Bed).Include(s => s.Department).Include(s => s.Dh).Include(s => s.Room);

            if (!string.IsNullOrEmpty(name))
            {
                students = students.Where(s => s.FullName.Contains(name));
            }

            return View(await students.ToListAsync());

        }
        [HttpGet]
        public IActionResult GetBedsByRoomId(int roomId)
        {
            // Lấy danh sách giường của phòng theo roomId và có trạng thái trống từ cơ sở dữ liệu
            var emptyBedsInRoom = _context.BedOfRooms.Where(b => b.RoomId == roomId && b.Status==false).ToList();

            // Tạo danh sách giường để trả về
            var bedList = emptyBedsInRoom.Select(b => new SelectListItem
            {
                Value = b.BedId.ToString(),
                Text = b.NumberBed.ToString() // Thay bằng tên trường chứa tên giường trong model
            }).ToList();

            // Trả về danh sách giường dưới dạng JSON
            return Json(bedList);
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
                    worksheet.Cells[1, 1].Value = "Họ và tên";
                    worksheet.Cells[1, 2].Value = "NTNS";
                    worksheet.Cells[1, 3].Value = "Giới tính";
                    worksheet.Cells[1, 4].Value = "Điện thoại";
                    worksheet.Cells[1, 5].Value = "Parent Phone Number";
                    worksheet.Cells[1, 6].Value = "Email";
                    worksheet.Cells[1, 7].Value = "Mã sinh viên";
                    worksheet.Cells[1, 8].Value = "Department";
                    worksheet.Cells[1, 9].Value = "Lớp";
                    worksheet.Cells[1, 10].Value = "Admission Confirmation";

                    // Style for header row
                    using (var range = worksheet.Cells["A1:J1"])
                    {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.WrapText = true; // Enable text wrapping
                            range.Style.Font.Size = 12; // Set font size
                            range.Style.Font.Color.SetColor(Color.White); // Set font color
                            range.Style.Font.Name = "Calibri"; // Set font name

                            // Custom width for each column
                            worksheet.Row(1).Height = 35;
                            worksheet.Column(1).Width = 40; 
                            worksheet.Column(2).Width = 15;
                            worksheet.Column(3).Width = 15;
                            worksheet.Column(4).Width = 20;
                            worksheet.Column(5).Width = 32;
                            worksheet.Column(6).Width = 30;
                            worksheet.Column(7).Width = 30;
                            worksheet.Column(8).Width = 35;
                            worksheet.Column(9).Width = 15;
                            worksheet.Column(10).Width = 30; 

                            // Custom height for header row
                            range.Style.Font.Bold = false;
                            range.Style.Font.Size = 13;
                            range.Style.Font.Color.SetColor(Color.White);
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                            range.Style.WrapText = false;
                  


                        }

                        // Data rows
                        int row = 2;
                    foreach (var student in students)
                    {   
                        worksheet.Row(row).Height = 20;
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
                    //worksheet.Cells.AutoFitColumns();

                    // Add borders for all cells
                    using (var range = worksheet.Cells[1, 1, row - 1, 10])
                    {
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

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
            // Lấy danh sách các phòng còn trống từ cơ sở dữ liệu
            var emptyRooms = _context.Rooms.Where(r => r.Status == false).ToList();

            // Chuyển danh sách các phòng còn trống thành SelectList
            SelectList roomList = new SelectList(emptyRooms, "RoomId", "NumberRoom");

            // Đặt danh sách các phòng vào ViewBag để sử dụng trong view
            ViewBag.RoomId = roomList;

            // Tạo SelectList cho các thông tin khác cần thiết
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "NumberBed");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhcode");

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
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "NumberBed", student.BedId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", student.DepartmentId);
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhcode", student.Dhid);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", student.RoomId);
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
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "NumberBed", student.BedId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", student.DepartmentId);
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhcode", student.Dhid);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", student.RoomId);
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
            ViewData["BedNumbers"] = new SelectList(_context.BedOfRooms, "BedId", "NumberBed", student.BedId);
            ViewData["DepartmentNames"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", student.DepartmentId);
            ViewData["DhCodes"] = new SelectList(_context.Dhs, "Dhid", "Dhcode", student.Dhid);
            ViewData["RoomNumbers"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", student.RoomId);
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
